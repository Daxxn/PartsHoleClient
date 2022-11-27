using MVVMLibrary;
using PartsInventory.Models.BOM;
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
         'M',
         'u',
         'G',
         'n',
         'p'
      };

      private char _suffix = '\0';
      private char _type = '\0';
      private double _value = 0;
      private string? _raw = null;
      private string? _otherInfo = null;
      private bool _isParsable = false;
      #endregion

      #region Constructors
      public ValueModel() { }
      public ValueModel(string data)
      {
         _raw = data;
         ParseValue(data);
      }
      #endregion

      #region Methods
      private void ParseValue(string newValue)
      {
         if (string.IsNullOrEmpty(newValue))
         {
            IsParsable = false;
            return;
         }
         if (!char.IsDigit(newValue[0]))
         {
            IsParsable = false;
            return;
         }
         if (newValue.Contains(' '))
         {
            var spl = newValue.Split(' ', StringSplitOptions.RemoveEmptyEntries);
            if (spl.Length >= 1)
            {
               if (spl.Length > 1)
               {
                  OtherInfo = string.Join(" ", spl[1..]);
               }
               ParseValue(spl[0]);
            }
            else return;
         }
         else
         {
            if (newValue.Length > 0 && newValue.Length <= 8)
            {
               if (double.TryParse(newValue, out double val))
               {
                  Value = val;
                  IsParsable = true;
               }
               else
               {
                  double sigFig;
                  if (typicalTypes.Contains(newValue[^1]))
                  {
                     Type = newValue[^1];
                     if (typicalSuffixes.Contains(newValue[^2]))
                     {
                        Suffix = newValue[^2];
                        if (double.TryParse(newValue[..^2], out sigFig))
                        {
                           Value = sigFig;
                           IsParsable = true;
                        }
                     }
                     else if (double.TryParse(newValue[..^2], out sigFig))
                     {
                        Value = sigFig;
                        IsParsable = true;
                     }
                  }
                  else if (typicalSuffixes.Contains(newValue[^1]))
                  {
                     Suffix = newValue[^1];
                     if (double.TryParse(newValue[..^1], out sigFig))
                     {
                        Value = sigFig;
                        IsParsable = true;
                     }
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
            OnPropertyChanged(nameof(IsParsable));
            OnPropertyChanged(nameof(Display));
         }
      }

      public string? OtherInfo
      {
         get => _otherInfo;
         set
         {
            _otherInfo = value;
            OnPropertyChanged();
         }
      }

      public bool IsParsable
      {
         get => _isParsable;
         set
         {
            _isParsable = value;
            OnPropertyChanged();
         }
      }

      public string? Display
      {
         get
         {
            if (!IsParsable) return RawValue;
            return $"{Value}{Suffix}{Type}{(OtherInfo != null ? $" {OtherInfo}" : "")}";
         }
      }
      #endregion
   }
}
