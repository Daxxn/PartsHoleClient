using System.Collections.ObjectModel;

using MVVMLibrary;

using PartsInventory.Models;

namespace PartsInventory.ViewModels
{
   public interface IPartSearchViewModel
   {
      PartsCollection? AllParts { get; set; }
      bool MatchCase { get; set; }
      Command SearchCmd { get; init; }
      ObservableCollection<PartModel> SearchParts { get; set; }
      string? SearchText { get; set; }
      ObservableCollection<PartModel>? SelectedParts { get; set; }
   }
}