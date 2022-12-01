using System.Timers;

namespace PartsInventory.Models
{
   public interface ITimerWrapper<T>
   {
      T? State { get; }
   }
}