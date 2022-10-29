using MVVMLibrary;
using PartsInventory.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PartsInventory.ViewModels
{
   public class PartNumberGeneratorViewModel : ViewModel
   {
      #region Local Props
      private PartModel? _selectedPart = null;
      private PartsCollection? _allParts = null;
      private PartNumber? _newPartNum = new();

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
      private void New()
      {
         if (AllParts is null) return;
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

      private void AssignToSelectedPart()
      {
         if (SelectedPart is null) return;
         if (NewPartNumber is null) return;
         SelectedPart.Reference = NewPartNumber;
         NewPartNumber = new();
      }

      #region Events
      public void SelectedPartChanged_Inv(object sender, PartModel? e)
      {
         SelectedPart = e;
      }

      public void PartsChanged_Main(object sender, PartsCollection e)
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
      public PartModel? SelectedPart
      {
         get => _selectedPart;
         set
         {
            _selectedPart = value;
            OnPropertyChanged();
         }
      }

      public PartsCollection? AllParts
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
