using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;

namespace PartsInventory.Utils.Converters
{
   public class EmptyConverter : IValueConverter
   {
      public object? Convert(object value, Type targetType, object parameter, CultureInfo culture)
      {
         if (value is null)
         {
            return default;
         }
         return value;
      }

      public object? ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
      {
         if (value is string str)
         {
            if (string.IsNullOrEmpty(str))
            {
               return null;
            }
         }
         return value;
      }
   }
}
