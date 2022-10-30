using MVVMLibrary;
using PartsInventory.Models.Events;
using PartsInventory.Models.Passives.Book;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PartsInventory.ViewModels
{
   public class PassiveBookViewModel : ViewModel
   {
      #region Local Props
      private PassiveBookModel _book = new();
      private ObservableCollection<ValueModel>? _selectedValues = null;

      #region Commands
      public Command GenerateCmd { get; init; }
      #endregion
      #endregion

      #region Constructors
      public PassiveBookViewModel()
      {
         GenerateCmd = new(() => Book.GenerateValues());
      }
      #endregion

      #region Methods
      public void RemoveValue(ValueModel value)
      {
         Book.Remove(value);
      }

      public void AddAbove(ValueModel value)
      {
         Book.Insert(value.Index - 1, new(value.Index - 1));
      }

      public void AddBelow(ValueModel value)
      {
         Book.Insert(value.Index, new(value.Index));
      }

      #region Events
      public void NewBook_Psv(object sender, NewBookEventArgs e)
      {
         Book = new(e.Type);
      }
      #endregion
      #endregion

      #region Full Props
      public PassiveBookModel Book
      {
         get => _book;
         set
         {
            _book = value;
            OnPropertyChanged();
         }
      }

      public ObservableCollection<ValueModel>? SelectedValues
      {
         get => _selectedValues;
         set
         {
            _selectedValues = value;
            OnPropertyChanged();
         }
      }
      #endregion
   }
}
