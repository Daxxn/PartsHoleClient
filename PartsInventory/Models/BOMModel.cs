using MVVMLibrary;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PartsInventory.Models
{
   public class BOMModel : Model
   {
      #region Local Props
      private ObservableCollection<BOMItemModel> _parts = new();
      #endregion

      #region Constructors
      public BOMModel() { }
      #endregion

      #region Methods
      #endregion

      #region Full Props
      public ObservableCollection<BOMItemModel> Parts
      {
         get => _parts;
         set
         {
            _parts = value;
            OnPropertyChanged();
         }
      }
      #endregion
   }
}
