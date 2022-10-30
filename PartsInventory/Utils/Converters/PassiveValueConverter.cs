using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;

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
         // Will need to put in parsing. Otherwise the only values that can be entered will be the raw values.
         return value;
      }
   }
}
