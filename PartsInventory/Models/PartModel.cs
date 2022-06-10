using MVVMLibrary;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PartsInventory.Models
{
   public class PartModel : Model
   {
      #region Local Props
      private string _supplierPartNumber = "";
      private string _partNumber = "";
      private string? _desc = null;
      private int? _reference = null;
      private uint _qty = 0;
      private decimal _unitPrice = 0;
      //private decimal _extPrice = 0;
      private uint _backorder = 0;

      private string? _datasheet = null;
      private string[]? _tags = null;
      private BinModel _binLocation = new();
      #endregion

      #region Constructors
      public PartModel() { }
      #endregion

      #region Methods
      public void ParseProp(string prop, string value)
      {
         switch (prop)
         {
            case "Quantity":
               Quantity = ParseUint(value);
               break;
            case "Part Number":
               SupplierPartNumber = value;
               break;
            case "Manufacturer Part Number":
               PartNumber = value;
               break;
            case "Description":
               Description = value;
               break;
            case "Customer Reference":
               Reference = ParseInt(value);
               break;
            case "Unit Price":
               UnitPrice = ParseDec(value);
               break;
            case "Backorder":
               Backorder = ParseUint(value);
               break;

            // Dont really need it. The unit price & quantity are enough.
            // Keep for now, just in case.
            //case "Extended Price":
            //   var val = value.Replace("$", "");
            //   ExtendedPrice = ParseDec(val);
            //   break;

            default:
               break;
         }
      }

      public bool Equals(PartModel part)
      {
         if (Reference is not null)
         {
            if (Reference == part.Reference)
            {
               return true;
            }
         }
         if (PartNumber == part.PartNumber)
         {
            return true;
         }
         if (SupplierPartNumber == part.SupplierPartNumber)
         {
            return true;
         }
         return false;
      }

      private static int? ParseInt(string input)
      {
         var success = int.TryParse(input, out int val);
         if (success)
         {
            return val;
         }
         return null;
      }

      private static uint ParseUint(string input)
      {
         var success = uint.TryParse(input, out uint val);
         if (success)
         {
            return val;
         }
         return 0;
      }

      private static decimal ParseDec(string input)
      {
         var success = decimal.TryParse(input, out decimal val);
         if (success)
         {
            return val;
         }
         return 0;
      }
      #endregion

      #region Full Props
      public string SupplierPartNumber
      {
         get => _supplierPartNumber;
         set
         {
            _supplierPartNumber = value;
            OnPropertyChanged();
         }
      }

      public string PartNumber
      {
         get => _partNumber;
         set
         {
            _partNumber = value;
            OnPropertyChanged();
         }
      }

      public string? Description
      {
         get => _desc;
         set
         {
            _desc = value;
            OnPropertyChanged();
         }
      }

      public int? Reference
      {
         get => _reference;
         set
         {
            _reference = value;
            OnPropertyChanged();
         }
      }

      public uint Quantity
      {
         get => _qty;
         set
         {
            _qty = value;
            OnPropertyChanged();
         }
      }

      public decimal UnitPrice
      {
         get => _unitPrice;
         set
         {
            _unitPrice = value;
            OnPropertyChanged();
         }
      }

      public decimal ExtendedPrice
      {
         get => _unitPrice * _qty;
      }

      public uint Backorder
      {
         get => _backorder;
         set
         {
            _backorder = value;
            OnPropertyChanged();
         }
      }

      public string? Datasheet
      {
         get => _datasheet;
         set
         {
            _datasheet = value;
            OnPropertyChanged();
         }
      }

      public BinModel BinLocation
      {
         get => _binLocation;
         set
         {
            _binLocation = value;
            OnPropertyChanged();
         }
      }

      public string[]? Tags
      {
         get => _tags;
         set
         {
            _tags = value;
            OnPropertyChanged();
         }
      }
      #endregion
   }
}
