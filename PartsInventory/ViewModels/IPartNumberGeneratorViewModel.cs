using System.Collections.Generic;
using System.Collections.ObjectModel;

using MVVMLibrary;
using PartsInventory.Models.Enums;
using PartsInventory.Models.Inventory.Main;

namespace PartsInventory.ViewModels
{
   public interface IPartNumberGeneratorViewModel
   {
      IMainViewModel MainVM { get; }
      Command AssignToSelectedCmd { get; init; }
      Command ClearCmd { get; init; }
      Command NewCmd { get; init; }
      PartNumber? NewPartNumber { get; set; }
      ObservableCollection<PartModel>? SelectedParts { get; set; }
      PartNumberSubCategory[]? SelectedSubCategories { get; set; }
      PartNumberSubCategory SubCategory { get; set; }
      PartNumberCategory Category { get; set; }

      void PartNumberCreated_PNTemp(object sender, PartNumber e);
      void SelectedPartsChanged_Inv(object sender, IEnumerable<PartModel>? e);
   }
}