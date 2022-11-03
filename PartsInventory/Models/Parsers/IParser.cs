using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PartsInventory.Models.Inventory;
using PartsInventory.Models.Inventory.Main;

namespace PartsInventory.Models.Parsers
{
   public interface IParser
   {
      InvoiceModel Parse();
      Task<InvoiceModel> ParseAsync();

      void GetOrderDetails(InvoiceModel model, string path);
   }
}
