using MVVMLibrary;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PartsInventory.Models
{
   public class RevisionModel : Model
   {
      #region Local Props
      private uint _main = 0;
      private uint _sub = 0;
      #endregion

      #region Constructors
      public RevisionModel() { }
      #endregion

      #region Methods
      public override string ToString()
      {
         return $"REV{Main}{(Sub == 0 ? "" : $".{Sub}")}";
      }
      #endregion

      #region Full Props
      public uint Main
      {
         get => _main;
         set
         {
            _main = value;
            OnPropertyChanged();
         }
      }

      public uint Sub
      {
         get => _sub;
         set
         {
            _sub = value;
            OnPropertyChanged();
         }
      }
      #endregion
   }
}
