using System.Collections.ObjectModel;

using PartsInventory.Models.Enums;
using PartsInventory.Models.Inventory.Main;

namespace PartsInventory.Models.Inventory
{
   public interface IInvoiceModel
   {
      uint OrderNumber { get; set; }
      ObservableCollection<InvoicePartModel> Parts { get; set; }
      string Path { get; set; }
      decimal SubTotal { get; set; }
      SupplierType? SupplierType { get; set; }
      bool IsAddedToParts { get; set; }
   }
}