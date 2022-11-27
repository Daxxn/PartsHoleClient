using System;
using System.Collections.ObjectModel;

using MVVMLibrary;

using PartsInventory.Models.Events;
using PartsInventory.Models.Passives.Book;

namespace PartsInventory.ViewModels
{
   public interface IPassiveBookViewModel
   {
      bool AddZero { get; set; }
      PassiveBookModel Book { get; set; }
      Command GenerateCmd { get; init; }
      ObservableCollection<ValueModel>? SelectedValues { get; set; }
      string[] StandardPackageSizes { get; }

      event EventHandler<PassiveBookModel> AddNewBookEvent;

      void AddAbove(ValueModel value);
      void AddBelow(ValueModel value);
      void NewBook_Psv(object sender, NewBookEventArgs e);
      void RemoveValue(ValueModel value);
   }
}