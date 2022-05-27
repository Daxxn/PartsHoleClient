using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PartsInventory.Models.Events
{
   public class AddInvoiceToPartsEventArgs : EventArgs
   {
      public IList<PartModel> NewParts { get; init; }
      public IList<uint> InvoiceID { get; init; }

      public AddInvoiceToPartsEventArgs(IList<uint> id, IList<PartModel> newParts)
      {
         InvoiceID = id;
         NewParts = newParts;
      }
   }
}
