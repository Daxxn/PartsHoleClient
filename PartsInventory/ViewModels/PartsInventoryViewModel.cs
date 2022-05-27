﻿using MVVMLibrary;
using PartsInventory.Models;
using PartsInventory.Models.Events;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PartsInventory.ViewModels
{
   public class PartsInventoryViewModel : ViewModel
   {
      #region Local Props
      public event EventHandler<PartModel> OpenDatasheetEvent = (s,e) => { };
      private PartsCollection? _partsCollection = null;
      private InvoiceModel? _selectedInvoice = null;
      private PartModel? _selectedPart = null;

      #region commands
      public Command AddPartCmd { get; init; }
      public Command RemovePartCmd { get; init; }
      #endregion
      #endregion

      #region Constructors
      public PartsInventoryViewModel()
      {
         AddPartCmd = new(AddPart);
         RemovePartCmd = new(RemovePart);
      }
      #endregion

      #region Methods
      public void NewPartsEventHandler(object sender, AddInvoiceToPartsEventArgs e)
      {
         if (PartsCollection is null) PartsCollection = new();
         PartsCollection.AddInvoices(e.NewInvoices);
      }

      public void OpenDatasheet(object sender, PartModel part)
      {
         OpenDatasheetEvent?.Invoke(sender, part);
      }

      private void AddPart()
      {
         if (PartsCollection is null) return;
         PartsCollection.Parts.Add(new());
      }

      private void RemovePart()
      {
         if (PartsCollection is null || SelectedPart is null) return;

         PartsCollection.Parts.Remove(SelectedPart);
      }
      #endregion

      #region Full Props
      public PartsCollection? PartsCollection
      {
         get => _partsCollection;
         set
         {
            _partsCollection = value;
            OnPropertyChanged();
         }
      }

      public PartModel? SelectedPart
      {
         get => _selectedPart;
         set
         {
            _selectedPart = value;
            OnPropertyChanged();
         }
      }

      public InvoiceModel? SelectedInvoice
      {
         get => _selectedInvoice;
         set
         {
            _selectedInvoice = value;
            OnPropertyChanged();
         }
      }
      #endregion
   }
}