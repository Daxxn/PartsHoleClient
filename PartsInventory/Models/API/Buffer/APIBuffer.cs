using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Timers;

using Microsoft.Extensions.Options;

namespace PartsInventory.Models.API.Buffer;

public class APIBuffer
{
   #region Local Props
   private Timer Timer { get; set; }

   private APIBufferCollection2 Buffer { get; set; } = new();
   #endregion

   #region Constructors
   public APIBuffer(IOptions<Settings> settings, IAPIController apiController)
   {
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

   private async void Timer_Elapsed(object? sender, ElapsedEventArgs e)
   {
      foreach (var model in Buffer.GetAllModels())
      {

      }
   }
   #endregion

   #region Methods

   #endregion

   #region Full Props

   #endregion
}
