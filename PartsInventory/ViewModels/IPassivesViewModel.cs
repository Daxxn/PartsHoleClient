using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;

using MVVMLibrary;
using PartsInventory.Models.Enums;
using PartsInventory.Models.Events;
using PartsInventory.Models.Inventory;
using PartsInventory.Models.Inventory.Main;
using PartsInventory.Models.Passives;
using PartsInventory.Models.Passives.Book;

namespace PartsInventory.ViewModels;

public interface IPassivesViewModel
{
   IUserModel User { get; }
   int CurrentTabIndex { get; set; }
   Command AddAllPartsCmd { get; init; }
   Command AddBookCmd { get; init; }
   Command AddSelectedPartsCmd { get; init; }
   Command NewBookCmd { get; init; }
   Command ParseAllPartsCmd { get; init; }
   Command SearchCmd { get; init; }
   PassiveSearchProp? SearchProp { get; set; }
   ObservableCollection<IPassive>? SearchResults { get; set; }
   string? SearchText { get; set; }
   ObservableCollection<PartModel>? SelectedAddParts { get; set; }
   PassiveBookModel? SelectedBook { get; set; }
   ObservableCollection<PartModel>? SelectedParts { get; set; }
   string[] StandardPackageSizes { get; }

   event EventHandler<NewBookEventArgs> NewBookEvent;

   void AddNewBook_Book(object sender, PassiveBookModel e);
   //void PartsChanged_Main(object sender, UserModel e);
   void SelectedPartsChanged_Inv(object sender, IEnumerable<PartModel>? e);
}