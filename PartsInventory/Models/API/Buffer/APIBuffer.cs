using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Timers;

using Microsoft.Extensions.Options;

using MVVMLibrary;

using PartsInventory.Models.Inventory.Main;
using PartsInventory.Resources.Settings;

namespace PartsInventory.Models.API.Buffer;

public class APIBuffer : IAPIBuffer
{
   #region Local Props
   private int _maxAttemptCount = 3;
   private Timer Timer { get; set; }

   private APIBufferCollection Buffer { get; set; } = new();
   private readonly IAPIController _apiController;
   #endregion

   #region Constructors
   public APIBuffer(IOptions<GeneralSettings> settings, IAPIController apiController)
   {
      _apiController = apiController;
      _maxAttemptCount = settings.Value.APIAttemptCount;
      Timer = new()
      {
         Interval = settings.Value.APIUpdateIntervalms,
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
      await RunUpdatesAsync();
   }

   /// <summary>
   /// For testing. Would wrather use the parallel version.
   /// </summary>
   private async Task RunUpdatesAsync()
   {
      if (Buffer.Count <= 0)
         return;
      foreach (var model in Buffer.GetAllModels())
      {
         await UpdatePart(model);
      }
   }

   /// <summary>
   /// Not fully tested. Need to test further. May be fixed.
   /// </summary>
   private async Task RunUpdatesParallel()
   {
      if (Buffer.Count <= 0)
         return;
      await Parallel.ForEachAsync(Buffer.GetAllModels(), async (model, token) =>
      {
         if (token.IsCancellationRequested)
            return;
         await UpdatePart(model);
      });
   }

   private async Task UpdatePart(KeyValuePair<PriorityKey, BufferModel> model)
   {
      if (Buffer[model.Key] is null)
         return;
      if (Buffer[model.Key]?.InProgress == false && Buffer[model.Key]?.Model is PartModel part)
      {
         Buffer[model.Key]!.InProgress = true;
         if (await _apiController.UpdatePart(part) || Buffer[model.Key]?.AttemptCount > _maxAttemptCount)
         {
            Buffer.RemoveModel(model.Key.ID);
         }
         else
         {
            Buffer[model.Key]!.AttemptCount++;
         }
      }
      else if (Buffer[model.Key]?.InProgress == false && Buffer[model.Key]?.Model is InvoiceModel invoice)
      {
         Buffer[model.Key]!.InProgress = true;
         if (await _apiController.UpdateInvoice(invoice) || Buffer[model.Key]?.AttemptCount > _maxAttemptCount)
         {
            Buffer.RemoveModel(model.Key.ID);
         }
         else
         {
            Buffer[model.Key]!.AttemptCount++;
         }
      }
      else if (Buffer[model.Key]?.InProgress == false && Buffer[model.Key]?.Model is BinModel bin)
      {
         Buffer[model.Key]!.InProgress = true;
         if (await _apiController.UpdateBin(bin) || Buffer[model.Key]?.AttemptCount > _maxAttemptCount)
         {
            Buffer.RemoveModel(model.Key.ID);
         }
         else
         {
            Buffer[model.Key]!.AttemptCount++;
         }
      }
   }

   public void ForceUpdateAll()
   {
      RunUpdatesAsync().Wait();
   }

   public async Task UpdateAll()
   {
      await RunUpdatesAsync();
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
