using JsonReaderLibrary;
using MVVMLibrary;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PartsInventory.Models
{
   public class BinModel : Model
   {
      #region Local Props
      private int _horz = 0;
      private int _vert = 0;
      private string _name = "BIN";
      #endregion

      #region Constructors
      public BinModel() { }
      #endregion

      #region Methods
      public override string ToString()
      {
         return $"{Name} - {Horizontal} - {Vertical}";
      }
      #endregion

      #region Full Props
      public string Name
      {
         get => _name;
         set
         {
            _name = value;
            OnPropertyChanged();
         }
      }

      public int Horizontal
      {
         get => _horz;
         set
         {
            _horz = value;
            OnPropertyChanged();
         }
      }

      public int Vertical
      {
         get => _vert;
         set
         {
            _vert = value;
            OnPropertyChanged();
         }
      }
      #endregion
   }
}
