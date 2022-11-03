using MVVMLibrary;
using PartsInventory.Models.Inventory;
using PartsInventory.Models.Inventory.Main;

using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PartsInventory.ViewModels.Testing
{
   public class PartSearchViewModel : ViewModel, IPartSearchViewModel
   {
      #region Local Props
      private UserModel? _allParts = null;
      private ObservableCollection<PartModel> _searchParts = new();
      private ObservableCollection<PartModel>? _selectedParts = null;

      private string? _searchText = null;

      private bool _matchCase = true;

      #region Commands
      public Command SearchCmd { get; init; }
      #endregion
      #endregion

      #region Constructors
      public PartSearchViewModel()
      {
         SearchCmd = new(Search);
      }
      #endregion

      #region Methods
      private void Search()
      {
         if (string.IsNullOrEmpty(SearchText))
            return;
         if (AllParts is null)
            return;

         SearchText = SearchText.Trim();

         SearchParts.Clear();

         foreach (var part in AllParts.Parts)
         {
            if (part.Search(SearchText, MatchCase))
            {
               SearchParts.Add(part);
            }
         }
      }
      #endregion

      #region Full Props
      public UserModel? AllParts
      {
         get => _allParts;
         set
         {
            _allParts = value;
            OnPropertyChanged();
         }
      }

      public ObservableCollection<PartModel> SearchParts
      {
         get => _searchParts;
         set
         {
            _searchParts = value;
            OnPropertyChanged();
         }
      }

      public ObservableCollection<PartModel>? SelectedParts
      {
         get => _selectedParts;
         set
         {
            _selectedParts = value;
            OnPropertyChanged();
         }
      }

      public string? SearchText
      {
         get => _searchText;
         set
         {
            _searchText = value;
            OnPropertyChanged();
         }
      }

      public bool MatchCase
      {
         get => _matchCase;
         set
         {
            _matchCase = value;
            OnPropertyChanged();
         }
      }
      #endregion
   }
}
