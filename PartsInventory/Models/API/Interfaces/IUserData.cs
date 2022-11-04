using System.Collections.Generic;

using PartsInventory.Models.API.Models;
using PartsInventory.Models.Inventory.Main;

namespace PartsInventory.Models.API
{
   public interface IUserData
   {
      IEnumerable<InvoiceApiModel>? Invoices { get; set; }
      IEnumerable<PartApiModel>? Parts { get; set; }

      IEnumerable<InvoiceModel>? ToInvoices();
      IEnumerable<PartModel>? ToParts();
   }
}