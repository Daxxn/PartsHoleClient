using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using PartsHoleRestLibrary.Enums;

namespace PartsInventory.Resources.Settings
{
   public class APISettings
   {
      public int Port { get; set; }
      public string BaseUrl { get; set; } = null!;
      public string UserEndpoint { get; set; } = null!;
      public string PartsEndpoint { get; set; } = null!;
      public string InvoicesEndpoint { get; set; } = null!;
      public string BinsEndpoint { get; set; } = null!;

      public string GetModelSelectorEndpoint(ModelIDSelector selector, bool isAdd) => selector switch
      {
         ModelIDSelector.PARTS => $"{UserEndpoint}/{(isAdd ? "add" : "remove")}-part",
         ModelIDSelector.INVOICES => $"{UserEndpoint}/{(isAdd ? "add" : "remove")}-invoice",
         ModelIDSelector.BINS => $"{UserEndpoint}/{(isAdd ? "add" : "remove")}-bin",
         ModelIDSelector.NONE => throw new ArgumentOutOfRangeException(nameof(selector), "NONE is not a valid endpoint."),
         _ => throw new ArgumentOutOfRangeException(nameof(selector), "Selector is not recognized."),
      };
   }
}
