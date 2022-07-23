using MVVMLibrary;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PartsInventory.Models.KiCAD
{
   public class LibSourceModel : Model
   {
      #region Local Props
      private string _lib = "";
      private string _part = "";
      private string _desc = "";
      #endregion

      #region Constructors
      public LibSourceModel() { }
      #endregion

      #region Methods
      public override string ToString()
      {
         return $"{Library}:{Part}";
      }
      #endregion

      #region Full Props
      public string Library
      {
         get => _lib;
         set
         {
            _lib = value;
            OnPropertyChanged();
         }
      }

      public string Part
      {
         get => _part;
         set
         {
            _part = value;
            OnPropertyChanged();
         }
      }

      public string Description
      {
         get => _desc;
         set
         {
            _desc = value;
            OnPropertyChanged();
         }
      }
      #endregion
   }
}
