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
      bool InvoicesAdded { get; set; }
      Command OpenInvoicesCmd { get; init; }
      Command OpenInvoiceCmd { get; init; }
      Command AddToPartsCmd { get; init; }
      Command ClearTempInvoiceCmd { get; init; }
      Command AddTempInvoiceCmd { get; init; }
      Command DeleteInvoiceCmd { get; init; }
      ObservableCollection<InvoiceModel>? SelectedInvoices { get; set; }

      event EventHandler<AddInvoiceToPartsEventArgs> AddToPartsEvent;
   }
}