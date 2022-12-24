using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Timers;

using Microsoft.Extensions.Options;

using PartsInventory.Models.Inventory;
using PartsInventory.Models.Inventory.Main;
using PartsInventory.Resources.Settings;
using PartsInventory.Utils.Messager;

namespace PartsInventory.Models.API.Buffer;

public class SimpleAPIBuffer : IAPIBuffer
{
   #region Local Props
   private readonly IAPIController _apicontroller;
   private readonly IOptions<GeneralSettings> _settings;
   private readonly IMessageService _messageService;
   private BufferCollection Buffer { get; set; }
   private Timer _timer;
   #endregion

   #region Constructors
   public SimpleAPIBuffer(IOptions<GeneralSettings> settings, IAPIController apiController, IMessageService messageService)
   {
      _settings = settings;
      _apicontroller = apiController;
      _messageService = messageService;

      Buffer = new BufferCollection();
      _timer = new()
      {
         Interval = settings.Value.APIUpdateIntervalms,
         AutoReset = false,
         Enabled = false,
      };
      _timer.Elapsed += _timer_Elapsed;
   }

   ~SimpleAPIBuffer()
   {
      _timer.Elapsed -= _timer_Elapsed;
      _timer.Dispose();
   }
   #endregion

   #region Methods
   private async void _timer_Elapsed(object? sender, ElapsedEventArgs e)
   {
      await RunUpdatesAsync();
   }

   public async Task RunUpdatesAsync()
   {
      if (Buffer.Count > 0)
      {
         foreach (var model in Buffer)
         {
            if (model.InProgress)
            {
               continue;
            }
            if (model.AttemptCount > _settings.Value.APIAttemptCount)
            {
               _messageService.AddMessage($"Unable to update {model.Model.Id}.", Enums.Severity.Error);
               Buffer.RemoveModel(model.Model.Id);
            }
            if (model.Model is PartModel part)
            {
               model.InProgress = true;
               if (await _apicontroller.UpdatePart(part))
               {
                  Buffer.RemoveModel(part.Id);
               }
               else
               {
                  model.AttemptCount++;
               }
            }
            else if (model.Model is InvoiceModel inv)
            {
               model.InProgress = true;
               if (await _apicontroller.UpdateInvoice(inv))
               {
                  Buffer.RemoveModel(inv.Id);
               }
               else
               {
                  model.AttemptCount++;
               }
            }
            else if (model.Model is BinModel bin)
            {
               model.InProgress = true;
               if (await _apicontroller.UpdateBin(bin))
               {
                  Buffer.RemoveModel(bin.Id);
               }
               else
               {
                  model.AttemptCount++;
               }
            }
         }
      }
   }

   public void UpdateModel(IModel model)
   {
      Buffer.AddModel(model);
      _timer.Restart();
   }

   public void ForceUpdateAll()
   {
      RunUpdatesAsync().Wait();
   }

   public async Task UpdateAll()
   {
      await RunUpdatesAsync();
   }
   #endregion

   #region Full Props

   #endregion
}
