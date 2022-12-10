using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PartsInventory.Models.Events;

public class TimerElapsedEventArgs<T> : EventArgs
{
   public double Interval { get; init; }
   public T? Data { get; init; }

   public TimerElapsedEventArgs(double interval)
   {
      Interval = interval;
   }
   public TimerElapsedEventArgs(double interval, T? data)
   {
      Interval = interval;
      Data = data;
   }
}
