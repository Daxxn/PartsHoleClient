using MVVMLibrary;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PartsInventory.Models
{
   public class ValueModel : Model
   {
      #region Local Props
      public static readonly List<char> typicalTypes = new()
      {
         'F',
         'Ω',
         'z',
         'H',
         'R',
      };

      public static readonly List<char> typicalSuffixes = new()
      {

         'K',
         'm',
         'u',
         'G',
         'n',
         'p'
      };

      private char _suffix = '\0';
      private char _type = '\0';
      private double _value = 0;
      private string? _raw = null;
      #endregion

      #region Constructors
      public ValueModel() { }
      #endregion

      #region Methods
      private void ParseValue(string newValue)
      {
         RawValue = newValue;
         if (newValue.Length > 0 && newValue.Length <= 8)
         {
            if (double.TryParse(newValue, out double val))
            {
               Value = val;
            }
            else
            {
               double sigFig = 0;
               if (typicalTypes.Contains(newValue[^1]))
               {
                  Type = newValue[^1];
                  if (typicalSuffixes.Contains(newValue[^2]))
                  {
                     Suffix = newValue[^2];
                     if (double.TryParse(newValue[..^3], out sigFig))
                     {
                        Value = sigFig;
                     }
                  }
                  if (double.TryParse(newValue[..^2], out sigFig))
                  {
                     Value = sigFig;
                  }
               }
               else if (typicalSuffixes.Contains(newValue[^1]))
               {
                  Suffix = newValue[^1];
                  if (double.TryParse(newValue[..^2], out sigFig))
                  {
                     Value = sigFig;
                  }
               }
            }
         }
      }
      #endregion

      #region Full Props
      public char Suffix
      {
         get => _suffix;
         set
         {
            _suffix = value;
            OnPropertyChanged();
         }
      }

      public char Type
      {
         get => _type;
         set
         {
            _type = value;
            OnPropertyChanged();
         }
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

      public string? RawValue
      {
         get => _raw;
         set
         {
            _raw = value;
            if (value is not null)
            {
               ParseValue(value);
            }
            OnPropertyChanged();
         }
      }
      #endregion
   }
}
