using MVVMLibrary;


using PartsInventory.Models.API;
using PartsInventory.Models.Inventory.Main;
using PartsInventory.Models.Inventory.Enums;

using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PartsInventory.ViewModels.Main
{
   public class BinsViewModel : ViewModel, IBinsViewModel
   {
      #region Local Props
      private readonly IMainViewModel _mainVM;
      private readonly IAPIController _apiController;

      private ObservableCollection<BinModel>? _selectedBins = null;
      private BinModel _newBin = new();

      #region Commands
      public Command CreateNewBinCmd { get; init; }
      public Command RemoveBinsCmd { get; init; }
      #endregion
      #endregion

      #region Constructors
      public BinsViewModel(IMainViewModel mainVM, IAPIController apiController)
      {
         _mainVM = mainVM;
         _apiController = apiController;

         #region Init Commands
         CreateNewBinCmd = new Command(CreateNewBin);
         RemoveBinsCmd = new Command(RemoveBins);
         #endregion
      }
      #endregion

      #region Methods
      private async void CreateNewBin()
      {
         if (await _apiController.CreateBin(NewBin))
         {
            await _apiController.AddModelToUser(MainVM.User.Id, NewBin.Id, ModelIDSelector.BINS);
         }
         _mainVM.User.Bins.Add(NewBin);
         NewBin = new();
      }

      private void RemoveBins()
      {

      }
      #region Events

      #endregion
      #endregion

      #region Full Props
      public IMainViewModel MainVM
      {
         get => _mainVM;
      }

      public ObservableCollection<BinModel>? SelectedBins
      {
         get => _selectedBins;
         set
         {
            _selectedBins = value;
            OnPropertyChanged();
         }
      }

      public BinModel NewBin
      {
         get => _newBin;
         set
         {
            _newBin = value;
            OnPropertyChanged();
         }
      }
      #endregion
   }
}
