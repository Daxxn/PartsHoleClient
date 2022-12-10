using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows.Media;

using MVVMLibrary;

using PartsInventory.Models.API;
using PartsInventory.Models.Events;
using PartsInventory.Models.Inventory.Main;

namespace PartsInventory.ViewModels.Main
{
   public class PartsInventoryViewModel : ViewModel, IPartsInventoryViewModel
   {
      #region Local Props
      private readonly IMainViewModel _mainViewModel;
      private readonly IAPIController _apiController;

      public event EventHandler<PartModel> OpenDatasheetEvent = (s, e) => { };
      public event EventHandler<IEnumerable<PartModel>?> SelectedPartsChanged = (s, e) => { };
      private InvoiceModel? _selectedInvoice = null;
      private ObservableCollection<PartModel>? _selectedParts = null;
      private ObservableCollection<BinModel>? _binSearchresults = null;
      private BinModel? _selectedBin = null;
      private string? _binSearchText = null;
      private string? _selectedBinName = null;
      private ObservableCollection<PartNumber>? _pnSearchresults = null;
      private PartNumber? _selectedPartNumber = null;
      private string? _partNumberSearchText = null;

      #region commands
      public Command RemovePartCmd { get; init; }
      #endregion
      #endregion

      #region Constructors
      public PartsInventoryViewModel(IMainViewModel mainViewModel, IAPIController apiController)
      {
         _mainViewModel = mainViewModel;
         _apiController = apiController;

         RemovePartCmd = new(RemovePart);
      }
      #endregion

      #region Methods
      public void NewPartsEventHandler(object sender, AddInvoiceToPartsEventArgs e)
      {
         //MainVM.User.AddInvoices(e.NewInvoices);
      }

      public void OpenDatasheet(object sender, PartModel part)
      {
         OpenDatasheetEvent?.Invoke(sender, part);
      }

      private async void RemovePart()
      {
         if (MainVM.User is null)
            return;
         if (SelectedParts is null)
            return;
         if (SelectedParts.Count != 1)
            return;
         await MainVM.RemovePart(SelectedParts[0]);
      }

      public async void AddBinToPart()
      {
         if (SelectedParts is null) return;
         if (SelectedParts.Count != 1) return;
         if (SelectedBin is null) return;

         SelectedParts[0].BinLocation = SelectedBin;
         if (await _apiController.UpdatePart(SelectedParts[0]))
         {
            BinSearchText = null;
         }
      }

      public async void AddPartNumberToPart()
      {
         if (SelectedParts is null) return;
         if (SelectedParts.Count == 0) return;
         if (SelectedPartNumber is null) return;

         SelectedParts[0].PartNumber = SelectedPartNumber.ToString();
         if (await _apiController.UpdatePart(SelectedParts[0]))
         {
            PartNumberSearchText = null;
         }
      }

      public void UpdateBinSearch()
      {
         if (string.IsNullOrEmpty(BinSearchText))
         {
            BinSearchResults = null;
            return;
         }
         BinSearchResults = new(MainVM.User.Bins.Where(bin => bin.ToString().Contains(BinSearchText)));
      }

      public void UpdatePartNumberSearch()
      {
         if (string.IsNullOrEmpty(PartNumberSearchText))
         {
            PartNumberSearchResults = null;
            return;
         }
         PartNumberSearchResults = new(MainVM.User.PartNumbers.Where(pn =>
         {
            if (!MainVM.User.Parts.Any(part => part?.Reference == pn))
            {
               return pn.ToString().Contains(PartNumberSearchText);
            }
            return false;
         }));
      }
      public void UpdateAPI(BaseModel model)
      {
         MainVM.UpdateAPI(model);
      }
      #endregion

      #region Full Props
      public IMainViewModel MainVM
      {
         get => _mainViewModel;
      }

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

      public string? BinSearchText
      {
         get => _binSearchText;
         set
         {
            _binSearchText = value;
            OnPropertyChanged();
         }
      }

      public ObservableCollection<BinModel>? BinSearchResults
      {
         get => _binSearchresults;
         set
         {
            _binSearchresults = value;
            OnPropertyChanged();
         }
      }

      public string? PartNumberSearchText
      {
         get => _partNumberSearchText;
         set
         {
            _partNumberSearchText = value;
            OnPropertyChanged();
         }
      }

      public ObservableCollection<PartNumber>? PartNumberSearchResults
      {
         get => _pnSearchresults;
         set
         {
            _pnSearchresults = value;
            OnPropertyChanged();
         }
      }

      public PartNumber? SelectedPartNumber
      {
         get => _selectedPartNumber;
         set
         {
            _selectedPartNumber = value;
            OnPropertyChanged();
         }
      }
      #endregion
   }
}
