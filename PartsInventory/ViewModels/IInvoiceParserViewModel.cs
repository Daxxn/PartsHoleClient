using System;
using System.Collections.ObjectModel;

using MVVMLibrary;
using PartsInventory.Models.Events;
using PartsInventory.Models.Inventory;
using PartsInventory.Models.Inventory.Main;

namespace PartsInventory.ViewModels
{
   public interface IInvoiceParserViewModel
   {
      IMainViewModel MainVM { get; }
      Command AddToPartsCmd { get; init; }
      Command ClearInvoicesCmd { get; init; }
      bool InvoicesAdded { get; set; }
      Command OpenAllInvoicesCmd { get; init; }
      Command OpenInvoicesCmd { get; init; }
      Command ParseTestCmd { get; init; }
      ObservableCollection<InvoiceModel>? SelectedInvoices { get; set; }

      event EventHandler<AddInvoiceToPartsEventArgs> AddToPartsEvent;
   }
}