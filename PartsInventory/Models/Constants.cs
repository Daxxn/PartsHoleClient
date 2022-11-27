using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PartsInventory.Models
{
   public static class Constants
   {
      public const double MEGA  = 1000000;
      public const double KILO  = 1000;
      public const double MILLI = 0.001;
      public const double MICRO = 0.000001;
      public const double NANO  = 0.000000001;
      public const double PICO  = 0.000000000001;

      public const double MEGA_CHECK = 999999;
      public const double KILO_CHECK = 999;
      public const double BASE_CHECK = 0.99999999999;
      public const double MILLI_CHECK = 0.0009;
      public const double MICRO_CHECK = 0.0000009;
      public const double NANO_CHECK = 0.0000000009;
      public const double PICO_CHECK = 0.0000000000009;


      public const double MEGA_INVERT = 0.000001;
      public const double KILO_INVERT = 0.001;
      public const double MILLI_INVERT = 1000;
      public const double MICRO_INVERT = 1000000;
      public const double NANO_INVERT = 1000000000;
      public const double PICO_INVERT = 1000000000000;

      public static readonly string[] StandardSMDPackages = new string[]
      {
         "0201",
         "0402",
         "0502",
         "0603",
         "0805",
         "1206",
         "1210",
         "1505",
         "1812",
         "2010",
         "2012",
         "2512",
         "2520",
      };

      public static readonly Dictionary<string, (double aspectRatio, double aspectSqrt)> StandardAspectRatios = new()
      {
         { "16/9", (16.0 / 9.0, Math.Sqrt(Math.Pow(16.0/9.0, 2.0) + 1.0)) },
         { "4/3", (4.0 / 3.0, Math.Sqrt(Math.Pow(4.0/3.0, 2.0) + 1.0)) }
      };
   }
}
