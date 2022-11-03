using System.Collections.Generic;
using System.Collections.ObjectModel;
using PartsInventory.Models.Inventory.Main;
using PartsInventory.Models.Passives;

namespace PartsInventory.Models.Inventory
{
   public interface IPartsCollection
   {
      IPartModel? this[IPartNumber pn] { get; }

      ObservableCollection<IInvoiceModel> Invoices { get; set; }
      ObservableCollection<IPartModel> Parts { get; set; }
      PassivesCollection Passives { get; set; }

      void AddInvoices(IList<IInvoiceModel> invoices);
   }
}