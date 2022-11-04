using MVVMLibrary;
using PartsInventory.Models.Events;
using PartsInventory.Models.Inventory;
using PartsInventory.Models.Inventory.Main;

using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PartsInventory.ViewModels.Main
{
   public class PartsInventoryViewModel : ViewModel, IPartsInventoryViewModel
   {
      #region Local Props
      private readonly IMainViewModel _mainViewModel;

      public event EventHandler<PartModel> OpenDatasheetEvent = (s, e) => { };
      public event EventHandler<IEnumerable<PartModel>?> SelectedPartsChanged = (s, e) => { };
      private InvoiceModel? _selectedInvoice = null;
      private ObservableCollection<PartModel>? _selectedParts = null;
      private BinModel? _selectedBin = null;
      private string? _selectedBinName = null;

      #region commands
      public Command AddPartCmd { get; init; }
      public Command RemovePartCmd { get; init; }
      #endregion
      #endregion

      #region Constructors
      public PartsInventoryViewModel(IMainViewModel mainViewModel)
      {
         _mainViewModel = mainViewModel;
         AddPartCmd = new(AddPart);
         RemovePartCmd = new(RemovePart);
      }
      #endregion

      #region Methods
      public void NewPartsEventHandler(object sender, AddInvoiceToPartsEventArgs e)
      {
         MainVM.User.AddInvoices(e.NewInvoices);
      }

      public void OpenDatasheet(object sender, PartModel part)
      {
         OpenDatasheetEvent?.Invoke(sender, part);
      }

      private void AddPart()
      {
         if (MainVM.User is null)
            return;
         MainVM.User.Parts.Add(new());
      }

      private void RemovePart()
      {
         if (MainVM.User is null || SelectedParts is null)
            return;

         foreach (var part in SelectedParts)
         {
            MainVM.User.Parts.Remove(part as PartModel);
         }
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
      #endregion
   }
}
