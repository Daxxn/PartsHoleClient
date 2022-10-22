using CSVParserLibrary;
using Microsoft.Win32;
using MVVMLibrary;
using PartsInventory.Models;
using PartsInventory.Models.BOM;
using PartsInventory.Models.KiCAD;
using PartsInventory.Models.Parsers.BOM;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace PartsInventory.ViewModels
{
   public class ProjectBOMViewModel : ViewModel
   {
      #region Local Props
      private ProjectModel? _project = null;

      #region Commands
      public Command ParseProjectCmd { get; init; }
      public Command ClearProjectCmd { get; init; }
      #endregion
      #endregion

      #region Constructors
      public ProjectBOMViewModel()
      {
         ParseProjectCmd = new(ParseProject);
         ClearProjectCmd = new(() => Project = null);
      }
      #endregion

      #region Methods
      private void ParseProject()
      {
         OpenFileDialog dialog = new()
         {
            InitialDirectory = PathSettings.Default.BOMs,
            Multiselect = false,
            Title = "Open KiCAD Project (.xml)",
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
      #endregion
   }
}
