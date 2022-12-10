using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

using MVVMLibrary;

using PartsInventory.Models.Enums;

namespace PartsInventory.Utils.Messager
{
   public struct Message
   {
      public string Text { get; set; }
      public Severity Severity { get; set; }
      public Message(string message, Severity severity)
      {
         Text = message;
         Severity = severity;
      }
   }
}
