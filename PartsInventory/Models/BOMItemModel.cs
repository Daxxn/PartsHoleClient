using MVVMLibrary;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PartsInventory.Models
{
   public class BOMItemModel : PartModel
   {
      #region Local Props
      private string _refDes = "";
      private ValueModel _value = new();
      private string _package = "";
      private string _bomName = "BOM-NAME";
      #endregion

      #region Constructors
      public BOMItemModel() { }
      #endregion

      #region Methods
      public new void ParseProp(string prop, string value)
      {
         switch (prop)
         {
            case "Part":
               ReferenceDesignator = value;
               break;
            case "Value":
               Value.RawValue = value;
               break;
            case "Device":
               BOMName = value;
               break;
            case "Package":
               Package = value;
               break;
            case "Description":
               Description = value;
               break;
            default:
               break;
         }
      }
      #endregion

      #region Full Props
      public string ReferenceDesignator
      {
         get => _refDes;
         set
         {
            _refDes = value;
            OnPropertyChanged();
         }
      }

      public ValueModel Value
      {
         get => _value;
         set
         {
            _value = value;
            OnPropertyChanged();
         }
      }

      public string Package
      {
         get => _package;
         set
         {
            _package = value;
            OnPropertyChanged();
         }
      }

      public string BOMName
      {
         get => _bomName;
         set
         {
            _bomName = value;
            OnPropertyChanged();
         }
      }
      #endregion
   }
}
