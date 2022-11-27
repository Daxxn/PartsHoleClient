using System.Collections.ObjectModel;

using MVVMLibrary;

using PartsInventory.Models.Inventory.Main;

namespace PartsInventory.ViewModels.Main
{
   public interface IBinsViewModel
   {
      IMainViewModel MainVM { get; }
      Command CreateNewBinCmd { get; init; }
      Command RemoveBinsCmd { get; init; }
      ObservableCollection<BinModel>? SelectedBins { get; set; }
   }
}