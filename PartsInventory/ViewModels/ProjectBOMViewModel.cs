using MVVMLibrary;
using PartsInventory.Models;
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
      private ProjectModel _proj = new();

      #region Commands

      #endregion
      #endregion

      #region Constructors
      public ProjectBOMViewModel() { }
      #endregion

      #region Methods

      #endregion

      #region Full Props
      public ProjectModel Project
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
