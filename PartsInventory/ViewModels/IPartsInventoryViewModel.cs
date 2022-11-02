﻿using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;

using MVVMLibrary;

using PartsInventory.Models;
using PartsInventory.Models.Events;

namespace PartsInventory.ViewModels
{
   public interface IPartsInventoryViewModel
   {
      Command AddPartCmd { get; init; }
      PartsCollection? PartsCollection { get; set; }
      Command RemovePartCmd { get; init; }
      BinModel? SelectedBin { get; set; }
      string? SelectedBinName { get; set; }
      InvoiceModel? SelectedInvoice { get; set; }
      ObservableCollection<PartModel>? SelectedParts { get; set; }

      event EventHandler<PartModel> OpenDatasheetEvent;
      event EventHandler<IEnumerable<PartModel>?> SelectedPartsChanged;

      void NewPartsEventHandler(object sender, AddInvoiceToPartsEventArgs e);
      void OpenDatasheet(object sender, PartModel part);
      void PartsChanged_Main(object sender, PartsCollection e);
   }
}