using MVVMLibrary;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PartsInventory.Models.Passives.Book
{
   public class ValueModel : Model
   {
      #region Local Props
      private int _index = 0;
      private double _value = 0;
      #endregion

      #region Constructors
      public ValueModel() { }
      public ValueModel(int index)
      {
         Index = index;
      }
      public ValueModel(int index, double value)
      {
         Index = index;
         Value = value;
      }
      #endregion

      #region Methods
      public override string ToString() => $"{Index} : {Value}";
      #endregion

      #region Full Props
      public int Index
      {
         get => _index;
         set
         {
            _index = value;
            OnPropertyChanged();
            OnPropertyChanged(nameof(DisplayIndex));
         }
      }

      public int DisplayIndex
      {
         get => _index + 1;
      }

      public double Value
      {
         get => _value;
         set
         {
            _value = value;
            OnPropertyChanged();
         }
      }

      public double Rounded
      {
         get => Math.Round(Value, -1);
      }
      #endregion
   }
}