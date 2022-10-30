using MVVMLibrary;
using PartsInventory.Models;
using PartsInventory.Models.Enums;
using PartsInventory.Models.Passives;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace PartsInventory.ViewModels
{
   public class PassivesViewModel : ViewModel
   {
      #region Local Props
      private PartsCollection? _parts = null;
      private ObservableCollection<PartModel>? _selectedParts = null;
      private ObservableCollection<PartModel>? _selectedAddParts = null;
      private ObservableCollection<IPassive>? _searchResults = null;
      private PassiveSearchProp? _searchProp = null;
      private string? _searchText = null;
      private int _currentTabIndex = 0;

      #region Commands
      public Command SearchCmd { get; init; }
      public Command AddAllPartsCmd { get; init; }
      public Command AddSelectedPartsCmd { get; init; }
      public Command ParseAllPartsCmd { get; init; }
      #endregion
      #endregion

      #region Constructors
      public PassivesViewModel()
      {
         AddAllPartsCmd = new(AddAllParts);
         AddSelectedPartsCmd = new(AddSelectedParts);
         ParseAllPartsCmd = new(ParseAllParts);
         SearchCmd = new(Search);
      }
      #endregion

      #region Methods
      private void AddAllParts()
      {
         if (Parts is null) return;
         if (SelectedParts is null) return;
         foreach (var part in SelectedParts)
         {
            IPassive? newPassive = Passive.ConvertPartAuto(part);
            if (newPassive is null) continue;
            Parts.Passives.Add(newPassive);
         }
      }

      private void AddSelectedParts()
      {
         if (Parts is null) return;
         if (SelectedAddParts is null) return;
         foreach (var part in SelectedAddParts)
         {
            IPassive? newPassive = Passive.ConvertPartAuto(part);
            if (newPassive is null) continue;
            Parts.Passives.Add(newPassive);
         }
      }

      private void ParseAllParts()
      {
         if (Parts is null) return;
         var passives = Parts.Passives;
         if (passives.Capacitors.Count > 0 || passives.Resistors.Count > 0 || passives.Inductors.Count > 0)
         {
            var result = MessageBox.Show("There is currently passives saved. Overwrite??", "Hold on..", MessageBoxButton.YesNo);
            if (result == MessageBoxResult.No) return;
         }

         Parts.Passives = new();

         foreach (var part in Parts.Parts)
         {
            IPassive? newPassive = Passive.ConvertPartAuto(part);
            if (newPassive is null) continue;
            Parts.Passives.Add(newPassive);
         }
      }

      private void Search()
      {
         if (SearchProp is null) return;
         var results = SearchPassives(Parts.Passives.GetPassivesList(CurrentTabIndex));
         if (results is null) return;
         SearchResults = new(results);
      }

      private IEnumerable<IPassive>? SearchPassives(IEnumerable<IPassive> list) => SearchProp switch
      {
         PassiveSearchProp.Value => double.TryParse(SearchText, out double val) ? list.Where(x => x.Value == val) : null,
         PassiveSearchProp.Tolerance => double.TryParse(SearchText, out double val) ? list.Where(x => x.Tolerance == val) : null,
         PassiveSearchProp.Desc => SearchText != null ? list.Where(x => x.Description.Contains(SearchText)) : null,
         _ => null
      };


      //private void SearchPassives(IEnumerable<IPassive> list)
      //{
      //   switch (SearchProp)
      //   {
      //      case PassiveSearchProp.Value:
      //         if (double.TryParse(SearchText, out double searchVal))
      //         {
      //            SearchResults = new(list.Where((p) => p.Value == searchVal));
      //         }
      //         break;
      //      default:
      //         break;
      //   }
      //}

      public void PartsChanged_Main(object sender, PartsCollection e)
      {
         Parts = e;
      }

      public void SelectedPartsChanged_Inv(object sender, IEnumerable<PartModel>? e)
      {
         if (e is null)
         {
            SelectedParts = null;
            return;
         }
         SelectedParts = new(e);
      }
      #endregion

      #region Full Props
      public PartsCollection? Parts
      {
         get => _parts;
         set
         {
            _parts = value;
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

      public ObservableCollection<PartModel>? SelectedAddParts
      {
         get => _selectedAddParts;
         set
         {
            _selectedAddParts = value;
            OnPropertyChanged();
         }
      }

      public ObservableCollection<IPassive>? SearchResults
      {
         get => _searchResults;
         set
         {
            _searchResults = value;
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

      public PassiveSearchProp? SearchProp
      {
         get => _searchProp;
         set
         {
            _searchProp = value;
            OnPropertyChanged();
         }
      }

      public int CurrentTabIndex
      {
         get => _currentTabIndex;
         set
         {
            _currentTabIndex = value;
            OnPropertyChanged();
         }
      }
      #endregion
   }
}
