using CSVParserLibrary;
using Microsoft.Win32;
using MVVMLibrary;
using PartsInventory.Models;
using PartsInventory.Models.BOM;
using PartsInventory.Models.Inventory;
using PartsInventory.Models.Inventory.Main;
using PartsInventory.Models.KiCAD;
using PartsInventory.Models.Parsers.BOM;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace PartsInventory.ViewModels.Testing
{
   public class ProjectBOMViewModel : ViewModel, IProjectBOMViewModel
   {
      #region Local Props
      private UserModel? _allParts = null;
      private ProjectModel? _project = null;
      private int _currentTab = 0;

      #region Commands
      public Command ParseProjectCmd { get; init; }
      public Command ClearProjectCmd { get; init; }
      public Command AllocateCmd { get; init; }
      #endregion
      #endregion

      #region Constructors
      public ProjectBOMViewModel()
      {
         ParseProjectCmd = new(ParseProject);
         ClearProjectCmd = new(() => Project = null);
         AllocateCmd = new(Allocate);
      }
      #endregion

      #region Methods
      private void ParseProject()
      {
         CurrentTab = 0;
         OpenFileDialog dialog = new()
         {
            InitialDirectory = PathSettings.Default.BOMs,
            Multiselect = false,
            Title = "Open BOM (.csv)",
            Filter = "BOM File|*.csv|All Files|*.*"
         };

         if (dialog.ShowDialog() == true)
         {
            try
            {
               var parser = new BOMParser(dialog.FileName);
               Project = new(dialog.FileName);
               Project.BOM = parser.Parse(Project);
            }
            catch (Exception e)
            {
               MessageBox.Show(e.Message);
            }
         }
      }

      private void Allocate()
      {
         if (Project is null)
            return;
         if (Project.BOM is null)
            return;
         if (AllParts is null)
            return;

         Project.Parts = new();
         foreach (var bom in Project.BOM.Parts)
         {
            if (bom.Reference == null)
               continue;
            var foundParts = AllParts.Parts.Where((p) => p.Reference == bom.Reference).ToArray();
            if (foundParts.Length > 0)
            {
               foreach (var part in foundParts)
               {
                  part.AllocatedQty = bom.Quantity;
                  Project.Parts.Parts.Add(part);

               }
            }
         }
      }

      public async void Loaded(object sender, EventArgs e)
      {
         await Task.Run(() =>
         {
            try
            {
               // Need to add auto-save and auto-load of the last BOM.
               // Also need to replace the settings with the Settings Library.
            }
            catch (Exception e)
            {
               MessageBox.Show(e.Message);
            }
         });
      }

      public async void Exit(object sender, EventArgs e)
      {
         await Task.Run(() =>
         {
            try
            {

            }
            catch (Exception e)
            {
               MessageBox.Show(e.Message);
            }
         });
      }

      public void PartsChanged_Main(object sender, UserModel e)
      {
         AllParts = e;
      }
      #endregion

      #region Full Props
      public ProjectModel? Project
      {
         get => _project;
         set
         {
            _project = value;
            OnPropertyChanged();
         }
      }

      public int CurrentTab
      {
         get => _currentTab;
         set
         {
            _currentTab = value;
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
      #endregion
   }
}
