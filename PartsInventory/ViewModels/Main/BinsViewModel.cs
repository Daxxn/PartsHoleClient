using MVVMLibrary;

using PartsInventory.Models.API;
using PartsInventory.Models.Inventory.Main;

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

      #region Commands
      public Command NewBinCmd { get; init; }
      public Command RemoveBinsCmd { get; init; }
      #endregion
      #endregion

      #region Constructors
      public BinsViewModel(IMainViewModel mainVM, IAPIController apiController)
      {
         _mainVM = mainVM;
         _apiController = apiController;

         #region Init Commands
         NewBinCmd = new Command(NewBin);
         RemoveBinsCmd = new Command(RemoveBins);
         #endregion
      }
      #endregion

      #region Methods

      private void NewBin()
      {

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
      #endregion
   }
}
