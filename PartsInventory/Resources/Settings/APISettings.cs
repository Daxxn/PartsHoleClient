using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PartsInventory.Resources.Settings
{
   public class APISettings
   {
      public int Port { get; set; }
      public string BaseUrl { get; set; }
      public string UserEndpoint { get; set; }
      public string PartsEndpoint { get; set; }
      public string InvoicesEndpoint { get; set; }
      public string BinsEndpoint { get; set; }
   }
}
