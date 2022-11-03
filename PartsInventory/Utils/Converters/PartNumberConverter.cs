using PartsInventory.Models.Inventory.Main;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;

namespace PartsInventory.Utils.Converters
{
   public class PartNumberConverter : IValueConverter
   {
      public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
      {
         if (value is PartNumber pn)
         {
            return pn.ToString();
         }
         return value;
      }

      public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
      {
         if (value is string str)
         {
            return new PartNumber(str);
         }
         return value;
      }
   }
}
