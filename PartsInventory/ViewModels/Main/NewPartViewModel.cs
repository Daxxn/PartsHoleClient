using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

using MVVMLibrary;

using PartsInventory.Models.Inventory.Main;

namespace PartsInventory.ViewModels.Main
{
   public class NewPartViewModel : ViewModel, INewPartViewModel
   {
      #region Local Props
      private IMainViewModel _mainViewModel;

      private PartModel? _newPart = PartModel.CreateNew();
      private string? _csvLine = "Test";
      #region Commands
      public Command ClearCmd { get; init; }
      public Command ParseCSVCmd { get; init; }
      #endregion
      #endregion

      #region Constructors
      public NewPartViewModel(IMainViewModel mainViewModel)
      {
         _mainViewModel = mainViewModel;

         ClearCmd = new(Clear);
         ParseCSVCmd = new(ParseCSV);
      }
      #endregion

      #region Methods
      public async Task<bool> Submit()
      {
         if (NewPart is null) return false;
         if (NewPart?.CheckPart() == true) return false;
         var success = await _mainViewModel.AddPart(NewPart!);
         if (success)
            NewPart = PartModel.CreateNew();
         return success;
      }

      private void Clear()
      {
         NewPart = PartModel.CreateNew();
      }

      private void ParseCSV()
      {
         try
         {
            if (CSVLine is null) return;
            if (NewPart is null) return;
            var split = CSVLine.Split("\n");
            if (split.Length < 3) return;
            NewPart.ParseRawProps(split);
         }
         catch (Exception e)
         {
            MessageBox.Show(e.Message);
         }
      }
      #region Events

      #endregion
      #endregion

      #region Full Props
      public PartModel? NewPart
      {
         get => _newPart;
         set
         {
            _newPart = value;
            OnPropertyChanged();
         }
      }

      public string? CSVLine
      {
         get => _csvLine;
         set
         {
            _csvLine = value;
            OnPropertyChanged();
         }
      }
      #endregion
   }
}
