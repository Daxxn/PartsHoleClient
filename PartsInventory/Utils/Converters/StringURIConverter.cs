using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;

namespace PartsInventory.Utils.Converters
{
   public class StringURIConverter : IValueConverter
   {
      private static readonly UriCreationOptions Opt = new()
      { DangerousDisablePathAndQueryCanonicalization = false };

      public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
      {
         if (value is Uri uri)
         {
            return uri.AbsoluteUri;
         }
         return value;
      }

      public object? ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
      {
         if (value is string str)
         {
            if (Uri.TryCreate(str, Opt, out Uri? uri))
            {
               return uri;
            }
         }
         return null;
      }
   }
}
