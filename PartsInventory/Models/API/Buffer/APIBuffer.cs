using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Timers;

using Microsoft.Extensions.Options;

using MVVMLibrary;

using PartsInventory.Models.Inventory.Main;

namespace PartsInventory.Models.API.Buffer;

public class APIBuffer : IAPIBuffer
{
   #region Local Props
   private Timer Timer { get; set; }

   private APIBufferCollection Buffer { get; set; } = new();
   private readonly IAPIController _apiController;
   #endregion

   #region Constructors
   public APIBuffer(IOptions<Settings> settings, IAPIController apiController)
   {
      _apiController = apiController;
      Timer = new()
      {
         Interval = settings.Value.ApiUpdateInterval,
         AutoReset = true,
         Enabled = false,
      };
      Timer.Elapsed += Timer_Elapsed;
   }

   ~APIBuffer()
   {
      Timer.Close();
      Timer.Elapsed -= Timer_Elapsed;
   }
   #endregion

   #region Methods
   private async void Timer_Elapsed(object? sender, ElapsedEventArgs e)
   {
      //await RunUpdatesAsync();
      await RunUpdatesParallel();
   }

   /// <summary>
   /// For testing. Would wrather use the parallel version.
   /// </summary>
   private async Task RunUpdatesAsync()
   {
      foreach (var model in Buffer.GetAllModels())
      {
         await UpdatePart(model);
      }
   }

   private async Task RunUpdatesParallel()
   {
      await Parallel.ForEachAsync(Buffer.GetAllModels(), async (model, token) =>
      {
         if (token.IsCancellationRequested)
            return;
         await UpdatePart(model);
      });
   }

   private async Task UpdatePart(KeyValuePair<PriorityKey, BaseModel> model)
   {
      if (model.Value is PartModel part)
      {
         if (await _apiController.UpdatePart(part))
         {
            Buffer.RemoveModel(model.Key.ID);
         }
      }
      else if (model.Value is InvoiceModel invoice)
      {
         if (await _apiController.UpdateInvoice(invoice))
         {
            Buffer.RemoveModel(model.Key.ID);
         }
      }
      else if (model.Value is BinModel bin)
      {
         if (await _apiController.UpdateBin(bin))
         {
            Buffer.RemoveModel(model.Key.ID);
         }
      }
   }

   public void ForceUpdateAll()
   {
      RunUpdatesParallel().Wait();
   }

   public void UpdateModel(BaseModel model)
   {
      Buffer.AddModel(model.Id, model);
      Timer.Restart();
   }
   #endregion

   #region Full Props
   #endregion
}
