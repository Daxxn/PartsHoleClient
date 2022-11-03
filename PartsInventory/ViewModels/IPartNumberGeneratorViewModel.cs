using System.Collections.Generic;
using System.Collections.ObjectModel;

using MVVMLibrary;
using PartsInventory.Models.Enums;
using PartsInventory.Models.Inventory;
using PartsInventory.Models.Inventory.Main;

namespace PartsInventory.ViewModels
{
   public interface IPartNumberGeneratorViewModel
   {
      UserModel? AllParts { get; set; }
      Command AssignToSelectedCmd { get; init; }
      Command ClearCmd { get; init; }
      Command NewCmd { get; init; }
      PartNumber? NewPartNumber { get; set; }
      ObservableCollection<PartModel>? SelectedParts { get; set; }
      PartNumberSubTypes[]? SelectedSubTypes { get; set; }
      PartNumberSubTypes SubType { get; set; }
      PartNumberType Type { get; set; }

      void PartNumberCreated_PNTemp(object sender, PartNumber e);
      void PartsChanged_Main(object sender, UserModel e);
      void SelectedPartsChanged_Inv(object sender, IEnumerable<PartModel>? e);
   }
}