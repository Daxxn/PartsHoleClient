﻿using MVVMLibrary;
using PartsInventory.Models.Enums;
using PartsInventory.Models.Inventory;
using PartsInventory.Models.Inventory.Main;

using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace PartsInventory.ViewModels.Main
{
   public class PartNumberGeneratorViewModel : ViewModel, IPartNumberGeneratorViewModel
   {
      #region Local Props
      private ObservableCollection<PartModel>? _selectedParts = null;
      private UserModel? _allParts = null;
      private PartNumber? _newPartNum = null;

      private PartNumberType _type = PartNumberType.Other;
      private PartNumberSubTypes _subType = PartNumberSubTypes.Other;

      private PartNumberSubTypes[]? _selectedSubTypes = null;

      #region Commands
      public Command NewCmd { get; init; }
      public Command ClearCmd { get; init; }
      public Command AssignToSelectedCmd { get; init; }
      #endregion
      #endregion

      #region Constructors
      public PartNumberGeneratorViewModel()
      {
         NewCmd = new(New);
         ClearCmd = new(() => NewPartNumber = null);
         AssignToSelectedCmd = new(AssignToSelectedPart);
      }
      #endregion

      #region Methods
      /// <summary>
      /// Replace. Theres an issues with the incremetation. It goes up a few numbers then it starts randomly repeating numbers.
      /// </summary>
      private void New()
      {
         if (AllParts is null)
            return;
         NewPartNumber = PartNumber.Create(Type, SubType);
         var matchingTypes = AllParts.Parts.Where((p) => p.Reference.TypeNum == NewPartNumber.TypeNum).ToArray();

         if (matchingTypes != null)
         {
            if (matchingTypes.Length == 0)
            {
               NewPartNumber.ID = 1;
            }
            else if (matchingTypes.Length > 1)
            {
               var ordered = matchingTypes.OrderBy((p) => p.Reference.TypeNum).ToArray();
               for (uint i = 0; i < ordered.Length; i++)
               {
                  if (ordered[i].Reference.ID != i + 1)
                  {
                     NewPartNumber.ID = i;
                  }
               }
               NewPartNumber.ID = ordered[^1].Reference.ID + 1;
            }
            else
            {
               NewPartNumber.ID = matchingTypes[0].Reference.ID + 1;
            }
         }
      }

      /// <summary>
      /// Assigns the newly generated part number to the selected part.
      /// </summary>
      private void AssignToSelectedPart()
      {
         if (SelectedParts is null)
            return;
         if (NewPartNumber is null)
            return;
         if (SelectedParts.Count != 1)
         {
            MessageBox.Show("Unable to assign part number.\nOnly one part can be selected.", "Warning");
            return;
         }
         // TODO - replace with DI interfaces.
         SelectedParts[0].Reference = NewPartNumber as PartNumber;
         NewPartNumber = new PartNumber();
      }

      #region Events
      public void SelectedPartsChanged_Inv(object sender, IEnumerable<PartModel>? e)
      {
         if (e is null)
         {
            SelectedParts = null;
            return;
         }
         SelectedParts = new(e);
      }

      public void PartsChanged_Main(object sender, UserModel e)
      {
         AllParts = e;
      }

      public void PartNumberCreated_PNTemp(object sender, PartNumber e)
      {
         NewPartNumber = e;
      }
      #endregion
      #endregion

      #region Full Props
      public ObservableCollection<PartModel>? SelectedParts
      {
         get => _selectedParts;
         set
         {
            _selectedParts = value;
            OnPropertyChanged();
         }
      }

      public UserModel? AllParts
      {
         get => _allParts;
         set
         {
            _allParts = value;
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

      public PartNumberType Type
      {
         get => _type;
         set
         {
            _type = value;
            // TODO - Separate into its own class.
            SelectedSubTypes = PartNumber.SubTypeDisplay[value];
            OnPropertyChanged();
            OnPropertyChanged(nameof(SelectedSubTypes));
         }
      }

      public PartNumberSubTypes SubType
      {
         get => _subType;
         set
         {
            _subType = value;
            OnPropertyChanged();
         }
      }

      public PartNumberSubTypes[]? SelectedSubTypes
      {
         get => _selectedSubTypes;
         set
         {
            _selectedSubTypes = value;
            OnPropertyChanged();
         }
      }
      #endregion
   }
}
