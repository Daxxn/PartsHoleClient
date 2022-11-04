using MVVMLibrary;
using PartsInventory.Models.Inventory;
using PartsInventory.Models.Inventory.Main;

using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PartsInventory.ViewModels.Main
{
   public class PartSearchViewModel : ViewModel, IPartSearchViewModel
   {
      #region Local Props
      private IUserModel _user;
      private ObservableCollection<PartModel> _searchParts = new();
      private ObservableCollection<PartModel>? _selectedParts = null;

      private string? _searchText = null;

      private bool _matchCase = true;

      #region Commands
      public Command SearchCmd { get; init; }
      #endregion
      #endregion

      #region Constructors
      public PartSearchViewModel(IUserModel user)
      {
         _user = user;
         SearchCmd = new(Search);
      }
      #endregion

      #region Methods
      private void Search()
      {
         if (string.IsNullOrEmpty(SearchText))
            return;
         if (User is null)
            return;

         SearchText = SearchText.Trim();

         SearchParts.Clear();

         foreach (var part in User.Parts)
         {
            if (part.Search(SearchText, MatchCase))
            {
               SearchParts.Add(part);
            }
         }
      }
      #endregion

      #region Full Props
      public IUserModel User
      {
         get => _user;
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
