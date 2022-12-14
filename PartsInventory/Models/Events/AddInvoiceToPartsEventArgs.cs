using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PartsInventory.Models.Inventory.Main;

namespace PartsInventory.Models.Events
{
   public class AddInvoiceToPartsEventArgs : EventArgs
   {
      public IList<InvoiceModel> NewInvoices { get; init; }

      public AddInvoiceToPartsEventArgs(IList<InvoiceModel> newInvoices)
      {
         NewInvoices = newInvoices;
      }
   }
}
