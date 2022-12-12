using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Windows;

using MVVMLibrary;

using PartsInventory.Models.API;
using PartsInventory.Models.Enums;
using PartsInventory.Models.Extensions;
using PartsInventory.Models.Inventory.Main;

namespace PartsInventory.ViewModels.Main
{
   public class PartNumberGeneratorViewModel : ViewModel, IPartNumberGeneratorViewModel
   {
      #region Local Props
      private readonly IMainViewModel _mainVM;
      private readonly IAPIController _apiController;

      private PartModel? _selectedPart = null;
      private ObservableCollection<PartModel>? _selectedParts = null;
      private PartNumber? _newPartNum = new();
      private PartNumber? _selectedPartNumber = null;

      private PartNumberCategory _category = PartNumberCategory.Other;
      private PartNumberSubCategory _subCategory = PartNumberSubCategory.Other;

      private PartNumberSubCategory[]? _selectedSubCategories = PartNumber.SubTypeDisplay[PartNumberCategory.Other];

      #region Commands
      public Command NewCmd { get; init; }
      public Command ClearCmd { get; init; }
      public Command AssignToSelectedCmd { get; init; }
      public Command RemoveCmd { get; init; }
      #endregion
      #endregion

      #region Constructors
      public PartNumberGeneratorViewModel(IMainViewModel mainVM, IPartsInventoryViewModel partsVM, IAPIController apiController)
      {
         _mainVM = mainVM;
         _apiController = apiController;

         partsVM.SelectedPartsChanged += SelectedPartsChanged_Inv;
         NewCmd = new(New);
         ClearCmd = new(() => NewPartNumber = null);
         AssignToSelectedCmd = new(AssignToSelectedPart);
         RemoveCmd = new(Remove);
      }
      #endregion

      #region Methods
      /// <summary>
      /// Requests a new <see cref="PartNumber"/> from the API.
      /// </summary>
      private async void New()
      {
         if (Category == PartNumberCategory.Other)
            return;
         if (SubCategory == PartNumberSubCategory.Other)
            return;
         var newPartNumber = await _apiController.NewPartNumber(_mainVM.User.Id, (uint)SubCategory);
         if (newPartNumber == null)
         {
            MessageBox.Show("Aborting part number generation.", "Warning");
            return;
         }
         MainVM.User.PartNumbers.Add(newPartNumber);
         NewPartNumber = new();
      }

      /// <summary>
      /// Assigns the newly generated <see cref="PartNumber"/> to the selected <see cref="PartModel"/> and updates the database.
      /// </summary>
      private async void AssignToSelectedPart()
      {
         if (SelectedPart is null)
            return;
         if (NewPartNumber is null)
            return;
         SelectedPart.Reference = NewPartNumber;
         if (await _apiController.UpdatePart(SelectedPart))
         {
            NewPartNumber = new PartNumber();
         }
      }

      private async void Remove()
      {
         if (SelectedPartNumber is null) return;
         if (await _apiController.DeletePartNumber(SelectedPartNumber.Id))
         {
            MainVM.User.PartNumbers.Remove(SelectedPartNumber);
            MainVM.User.PartNumberIDs.Remove(SelectedPartNumber.Id);
            MainVM.User.Parts.ForEach(part =>
            {
               if (part.Reference?.Equals(SelectedPartNumber) == true)
               {
                  part.Reference = null;
               }
            });
            //MainVM.User.PartModels.Where(x => x.Reference?.Equals(SelectedPartNumber) == true).ToList().ForEach(part => part.Reference = null);
            SelectedPartNumber= null;
         }
      }

      #region Events
      /// <summary>
      /// Triggered when the selected parts in the <see cref="IPartsInventoryViewModel"/> change.
      /// </summary>
      /// <param name="sender">The <see cref="IPartsInventoryViewModel"/></param>
      /// <param name="e">The list of selected <see cref="PartModel"/>s.</param>
      public void SelectedPartsChanged_Inv(object sender, IEnumerable<PartModel>? e)
      {
         if (e is null)
         {
            SelectedParts = null;
            return;
         }
         SelectedParts = new(e);
      }

      /// <summary>
      /// !! OLD !!
      /// <para/>
      /// Triggered when a new <see cref="PartNumber"/> is created.
      /// </summary>
      public void PartNumberCreated_PNTemp(object sender, PartNumber e)
      {
         NewPartNumber = e;
      }
      #endregion
      #endregion

      #region Full Props
      public IMainViewModel MainVM
      {
         get => _mainVM;
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

      public PartModel? SelectedPart
      {
         get => _selectedPart;
         set
         {
            _selectedPart = value;
            OnPropertyChanged();
         }
      }

      public PartNumber? SelectedPartNumber
      {
         get => _selectedPartNumber;
         set
         {
            _selectedPartNumber = value;
            OnPropertyChanged();
         }
      }

      public PartNumber? NewPartNumber
      {
         get => _newPartNum;
         set
         {
            _newPartNum = value;
            OnPropertyChanged();
         }
      }

      public PartNumberCategory Category
      {
         get => _category;
         set
         {
            _category = value;
            // TODO - Separate into its own class.
            SelectedSubCategories = PartNumber.SubTypeDisplay[value];
            SubCategory = SelectedSubCategories[0];
            OnPropertyChanged();
            OnPropertyChanged(nameof(SelectedSubCategories));
            OnPropertyChanged(nameof(SubCategory));
         }
      }

      public PartNumberSubCategory SubCategory
      {
         get => _subCategory;
         set
         {
            _subCategory = value;
            NewPartNumber = PartNumber.Create(value);
            OnPropertyChanged();
            OnPropertyChanged(nameof(NewPartNumber));
         }
      }

      public PartNumberSubCategory[]? SelectedSubCategories
      {
         get => _selectedSubCategories;
         set
         {
            _selectedSubCategories = value;
            OnPropertyChanged();
         }
      }
      #endregion
   }
}
