using MVVMLibrary;
using PartsInventory.Models;
using PartsInventory.Models.Events;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PartsInventory.ViewModels.Testing
{
   public class PartsInventoryViewModel : ViewModel, IPartsInventoryViewModel
   {
      #region Local Props
      public event EventHandler<PartModel> OpenDatasheetEvent = (s, e) => { };
      public event EventHandler<IEnumerable<PartModel>?> SelectedPartsChanged = (s, e) => { };
      private PartsCollection? _partsCollection = null;
      private InvoiceModel? _selectedInvoice = null;
      //private PartModel? _selectedPart = null;
      private ObservableCollection<PartModel>? _selectedParts = null;
      private BinModel? _selectedBin = null;
      private string? _selectedBinName = null;

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
         if (PartsCollection is null)
            PartsCollection = new();
         PartsCollection.AddInvoices(e.NewInvoices);
      }

      public void OpenDatasheet(object sender, PartModel part)
      {
         OpenDatasheetEvent?.Invoke(sender, part);
      }

      private void AddPart()
      {
         if (PartsCollection is null)
            return;
         PartsCollection.Parts.Add(new());
      }

      private void RemovePart()
      {
         if (PartsCollection is null || SelectedParts is null)
            return;

         //PartsCollection.Parts.Remove(SelectedPart);
         foreach (var part in SelectedParts)
         {
            PartsCollection.Parts.Remove(part);
         }
      }

      public void PartsChanged_Main(object sender, PartsCollection e)
      {
         PartsCollection = e;
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

      //public PartModel? SelectedPart
      //{
      //   get => _selectedPart;
      //   set
      //   {
      //      _selectedPart = value;
      //      OnPropertyChanged();
      //      SelectedPartChanged?.Invoke(this, value);
      //   }
      //}

      public ObservableCollection<PartModel>? SelectedParts
      {
         get => _selectedParts;
         set
         {
            _selectedParts = value;
            SelectedPartsChanged?.Invoke(this, value);
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

      public BinModel? SelectedBin
      {
         get => _selectedBin;
         set
         {
            _selectedBin = value;
            OnPropertyChanged();
         }
      }

      public string? SelectedBinName
      {
         get => _selectedBinName;
         set
         {
            _selectedBinName = value;
            OnPropertyChanged();
         }
      }
      #endregion
   }
}
