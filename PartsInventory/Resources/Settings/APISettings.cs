using System;

using PartsInventory.Models.Enums;

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
      public string PartNumberEndpoint { get; set; } = null!;

      public string GetModelSelectorEndpoint(ModelIDSelector selector, bool isAdd) => selector switch
      {
         ModelIDSelector.PARTS => $"{UserEndpoint}/{(isAdd ? "add" : "remove")}-part",
         ModelIDSelector.INVOICES => $"{UserEndpoint}/{(isAdd ? "add" : "remove")}-invoice",
         ModelIDSelector.BINS => $"{UserEndpoint}/{(isAdd ? "add" : "remove")}-bin",
         ModelIDSelector.PARTNUMBERS => $"{UserEndpoint}/{(isAdd ? "add" : "remove")}-partnum",
         ModelIDSelector.NONE => throw new ArgumentOutOfRangeException(nameof(selector), "NONE is not a valid endpoint."),
         _ => throw new ArgumentOutOfRangeException(nameof(selector), "Selector is not recognized."),
      };
   }
}
