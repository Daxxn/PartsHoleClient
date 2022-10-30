using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;
using PartsInventory.Models;

namespace PartsInventory.Utils.Converters
{
   public class PassiveValueConverter : IValueConverter
   {
      public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
      {
         if (value is double num)
         {
            if (num > 999999)
            {
               return $"{Math.Round(num * 0.000001, 4)}M{parameter ?? ""}";
            }
            else if (num > 999)
            {
               return $"{Math.Round(num * 0.001, 4)}K{parameter ?? ""}";
            }
            else if (num > 0.99999999999)
            {
               return $"{Math.Round(num, 4)}{parameter ?? ""}";
            }
            else if (num > 0.0009)
            {
               return $"{Math.Round(num * 1000, 4)}m{parameter ?? ""}";
            }
            else if (num > 0.0000009)
            {
               return $"{Math.Round(num * 1000000, 4)}µ{parameter ?? ""}";
            }
            else if (num > 0.0000000009)
            {
               return $"{Math.Round(num * 1000000000, 4)}n{parameter ?? ""}";
            }
            else
            {
               return $"{Math.Round(num * 1000000000000, 4)}p{parameter ?? ""}";
            }
         }
         return value;
      }

      public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
      {
         if (value is string str)
         {
            if (double.TryParse(str, out double d))
            {
               return d;
            }
            if (double.TryParse(str[..^1], out double d2))
            {
               if (char.IsLetter(str[^1]))
               {
                  char suffix = str[^1];
                  if (suffix == 'M')
                  {
                     return d2 * Constants.MEGA;
                  }
                  else if (char.ToLower(suffix) == 'k')
                  {
                     return d2 * Constants.KILO;
                  }
                  else if (suffix == 'm')
                  {
                     return d2 * Constants.MILLI;
                  }
                  else if (char.ToLower(suffix) == 'u')
                  {
                     return d2 * Constants.MICRO;
                  }
                  else if (char.ToLower(suffix) == 'n')
                  {
                     return d2 * Constants.NANO;
                  }
                  else if (char.ToLower(suffix) == 'p')
                  {
                     return d2 * Constants.PICO;
                  }
               }
            }
         }
         return value;
      }
   }
}
