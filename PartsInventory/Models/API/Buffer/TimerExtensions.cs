using System.Timers;

using Timer = System.Timers.Timer;

namespace PartsInventory.Models.API.Buffer;

public static class TimerExtensions
{
   public static void Restart(this Timer timer)
   {
      timer.Stop();
      timer.Start();
   }
}
