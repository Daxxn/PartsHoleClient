using PartsInventory.Models.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PartsInventory.Models.Events
{
   public class NewBookEventArgs : EventArgs
   {
      public PassiveType Type { get; init; }

      public NewBookEventArgs(PassiveType type)
      {
         Type = type;
      }
   }
}
