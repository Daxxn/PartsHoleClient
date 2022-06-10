using MVVMLibrary;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PartsInventory.Models
{
   public class ProjectModel : Model
   {
      #region Local Props
      private string _name = "New Project";
      private RevisionModel _rev = new();
      private BOMModel _bom = new();
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

      public RevisionModel REV
      {
         get => _rev;
         set
         {
            _rev = value;
            OnPropertyChanged();
         }
      }

      public BOMModel BOM
      {
         get => _bom;
         set
         {
            _bom = value;
            OnPropertyChanged();
         }
      }
      #endregion
   }
}
