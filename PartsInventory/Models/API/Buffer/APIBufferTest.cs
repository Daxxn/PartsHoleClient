using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Timers;

using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

using PartsInventory.Models.Inventory.Main;
using PartsInventory.Resources.Settings;

namespace PartsInventory.Models.API.Buffer
{
   public class APIBufferTest : IAPIBuffer
   {
      #region Local Props
      private Timer Timer { get; set; }

      private APIBufferCollection Buffer { get; set; } = new();
      private readonly ILogger<APIBufferTest> _logger;
      #endregion

      #region Constructors
      public APIBufferTest(IOptions<GeneralSettings> settings, ILogger<APIBufferTest> logger)
      {
         _logger = logger;
         Timer = new()
         {
            Interval = settings.Value.APIUpdateIntervalms,
            AutoReset = false,
            Enabled = false,
         };
         Timer.Elapsed += Timer_Elapsed;
      }

      ~APIBufferTest()
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

      private async Task RunUpdatesParallel()
      {
         await Parallel.ForEachAsync(Buffer.GetAllModels(), (model, token) =>
         {
            if (token.IsCancellationRequested)
               return new ValueTask();
            UpdatePart(model);
            return new ValueTask();
         });
      }

      private void UpdatePart(KeyValuePair<PriorityKey, BufferModel> model)
      {
         _logger.LogInformation("Sending model {type} to the API - {value}", model.Value.GetType().Name, model.Value);
         Buffer.RemoveModel(model.Key.ID);
      }

      public async Task UpdateAll()
      {
         await RunUpdatesParallel();
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
}
