using MVVMLibrary;

using PartsInventory.Models.Events;
using PartsInventory.Models.Passives.Book;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PartsInventory.ViewModels.New
{
   public class PassiveBookViewModel : ViewModel, IPassiveBookViewModel
   {
      #region Local Props
      public event EventHandler<PassiveBookModel> AddNewBookEvent;

      private PassiveBookModel _book = new();
      private ObservableCollection<ValueModel>? _selectedValues = null;
      private bool _addZero = false;

      #region Commands
      public Command GenerateCmd { get; init; }
      #endregion
      #endregion

      #region Constructors
      public PassiveBookViewModel()
      {
         AddNewBookEvent += AddNewBookEvent_this;
         GenerateCmd = new(() => Book.GenerateValues(AddZero));
      }
      #endregion

      #region Methods

      private void AddNewBookEvent_this(object? sender, IEnumerable<ValueModel> e)
      {
      }
      public void RemoveValue(ValueModel value)
      {
         Book.Remove(value);
      }

      public void AddAbove(ValueModel value)
      {
         Book.Insert(value.Index, new(value.Index));
      }

      public void AddBelow(ValueModel value)
      {
         Book.Insert(value.Index + 1, new(value.Index + 1));
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

      public bool AddZero
      {
         get => _addZero;
         set
         {
            _addZero = value;
            OnPropertyChanged();
         }
      }

      public string[] StandardPackageSizes
      {
         get => Models.Constants.StandardSMDPackages;
      }
      #endregion
   }
}
