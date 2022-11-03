using System.Collections.Generic;
using System.Threading.Tasks;
using PartsInventory.Models.Inventory;
using PartsInventory.Models.Inventory.Main;

namespace PartsInventory.Models.API
{
   public interface IAPIController
   {
      Task<InvoiceModel?> GetInvoice(string id);
      Task<IEnumerable<InvoiceModel>?> GetInvoices(string userID);
      Task<PartModel?> GetPart(string id);
      Task<UserModel?> GetPartsCollection();
   }
}