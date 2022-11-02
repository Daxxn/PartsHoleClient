using System.Collections.Generic;
using System.Collections.ObjectModel;

using MVVMLibrary;

using PartsInventory.Models;
using PartsInventory.Models.Enums;

namespace PartsInventory.ViewModels
{
   public interface IPartNumberGeneratorViewModel
   {
      PartsCollection? AllParts { get; set; }
      Command AssignToSelectedCmd { get; init; }
      Command ClearCmd { get; init; }
      Command NewCmd { get; init; }
      PartNumber? NewPartNumber { get; set; }
      ObservableCollection<PartModel>? SelectedParts { get; set; }
      PartNumberSubTypes[]? SelectedSubTypes { get; set; }
      PartNumberSubTypes SubType { get; set; }
      PartNumberType Type { get; set; }

      void PartNumberCreated_PNTemp(object sender, PartNumber e);
      void PartsChanged_Main(object sender, PartsCollection e);
      void SelectedPartsChanged_Inv(object sender, IEnumerable<PartModel>? e);
   }
}