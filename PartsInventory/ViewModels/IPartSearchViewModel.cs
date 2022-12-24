using System.Collections.ObjectModel;

using MVVMLibrary;
using PartsInventory.Models.Inventory;
using PartsInventory.Models.Inventory.Main;

namespace PartsInventory.ViewModels;

public interface IPartSearchViewModel
{
   IUserModel User { get; }
   //UserModel? AllParts { get; set; }
   bool MatchCase { get; set; }
   Command SearchCmd { get; init; }
   ObservableCollection<PartModel> SearchParts { get; set; }
   string? SearchText { get; set; }
   ObservableCollection<PartModel>? SelectedParts { get; set; }
}