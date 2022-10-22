using MVVMLibrary;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PartsInventory.Models.BOM
{
   public class BOMModel : Model
   {
      #region Local Props
      private ProjectModel _proj = null;
      private string? _source = null;
      private int? _partCount = null;
      private DateTime? _date = null;
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

      public string? Source
      {
         get => _source;
         set
         {
            _source = value;
            OnPropertyChanged();
         }
      }

      public int? PartCount
      {
         get => _partCount;
         set
         {
            _partCount = value;
            OnPropertyChanged();
         }
      }

      public DateTime? Date
      {
         get => _date;
         set
         {
            _date = value;
            OnPropertyChanged();
         }
      }
      #endregion
   }
}
