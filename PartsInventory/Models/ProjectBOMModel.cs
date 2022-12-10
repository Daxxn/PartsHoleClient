using MVVMLibrary;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PartsInventory.Models.KiCAD;
using PartsInventory.Models.BOM;
using System.IO;
using PartsInventory.Models.Inventory;
using PartsInventory.Models.Inventory.Main;
using System.Collections.ObjectModel;

namespace PartsInventory.Models
{
   public class ProjectModel : Model
   {
      #region Local Props
      private string _name = "New Project";
      private ObservableCollection<BOMModel>? _boms = null;
      private RevisionModel _rev = new();
      #endregion

      #region Constructors
      public ProjectModel() { }
      #endregion

      #region Methods
      #endregion

      #region Full Props
      public string Name
      {
         get => _name;
         set
         {
            _name = value;
            OnPropertyChanged();
         }
      }

      public ObservableCollection<BOMModel>? BOMs
      {
         get => _boms;
         set
         {
            _boms = value;
            OnPropertyChanged();
         }
      }

      public RevisionModel REV
      {
         get => _rev;
         set
         {
            _rev = value;
            OnPropertyChanged();
         }
      }
      #endregion
   }
}
