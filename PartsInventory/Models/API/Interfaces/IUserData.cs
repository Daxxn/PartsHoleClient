using System.Collections.Generic;

using PartsInventory.Models.Inventory.Main;

namespace PartsInventory.Models.API
{
   public interface IUserData
   {
      List<InvoiceModel> Invoices { get; set; }
      List<PartModel> Parts { get; set; }
   }
}