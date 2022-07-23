using Microsoft.Win32;
using MVVMLibrary;
using PartsInventory.Models;
using PartsInventory.Models.KiCAD;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PartsInventory.ViewModels
{
   public class ProjectBOMViewModel : ViewModel
   {
      #region Local Props
      private KicadModel? _proj = null;

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
            InitialDirectory = PathSettings.Default.KiCADProjects,
            Multiselect = false,
            Title = "Open KiCAD Project (.xml)",
            Filter = "Project File|*.xml|All Files|*.*"
         };

         if (dialog.ShowDialog() == true)
         {
            Project = ProjectParser.Parse(dialog.FileName);
         }
      }
      #endregion

      #region Full Props
      public KicadModel? Project
      {
         get => _proj;
         set
         {
            _proj = value;
            OnPropertyChanged();
         }
      }
      #endregion
   }
}
