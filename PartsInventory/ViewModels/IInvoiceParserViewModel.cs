﻿using System;
using System.Collections.ObjectModel;

using MVVMLibrary;
using PartsInventory.Models.Events;
using PartsInventory.Models.Inventory;
using PartsInventory.Models.Inventory.Main;

namespace PartsInventory.ViewModels
{
   public interface IInvoiceParserViewModel
   {
      Command AddToPartsCmd { get; init; }
      Command ClearInvoicesCmd { get; init; }
      ObservableCollection<InvoiceModel> Invoices { get; set; }
      bool InvoicesAdded { get; set; }
      Command OpenAllInvoicesCmd { get; init; }
      Command OpenInvoicesCmd { get; init; }
      Command ParseTestCmd { get; init; }
      InvoiceModel? SelectedInvoice { get; set; }

      event EventHandler<AddInvoiceToPartsEventArgs> AddToPartsEvent;
   }
}