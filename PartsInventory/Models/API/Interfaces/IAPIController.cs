using System.Collections.Generic;
using System.Threading.Tasks;

using PartsInventory.Models.API.Models;
using PartsInventory.Models.Inventory;
using PartsInventory.Models.Inventory.Main;

namespace PartsInventory.Models.API
{
   public interface IAPIController
   {
      #region GET

      Task<UserModel?> GetUser(UserModel user);
      Task<InvoiceModel?> GetInvoice(string id);
      Task<IEnumerable<InvoiceModel>?> GetInvoices(string[] ids);
      Task<PartModel?> GetPart(string id);
      Task<IEnumerable<PartModel>?> GetParts(string[] ids);

      Task<UserData?> GetUserData(UserModel user);
      #endregion
   }
}