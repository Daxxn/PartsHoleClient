using MVVMLibrary;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PartsInventory.Models.Inventory.Main
{
   public class PartModel : BaseModel
   {
      #region Local Props
      private string _supplierPartNumber = "";
      private string _partNumber = "";
      private string? _desc = null;
      private PartNumber? _reference = null;
      private uint _qty = 0;
      private uint _alloc = 0;
      private uint _slip = 0;
      private decimal _unitPrice = 0;
      //private decimal _extPrice = 0;
      private uint _backorder = 0;

      private Datasheet? _datasheet = null;
      private string[]? _tags = null;
      private BinModel _binLocation = new();
      #endregion

      #region Constructors
      public PartModel() { }
      #endregion

      #region Methods
      public void ParseProp(string prop, string value)
      {
         switch (prop.ToLower())
         {
            case "quantity":
               Quantity = ParseUint(value);
               break;
            case "part number":
               SupplierPartNumber = value;
               break;
            case "manufacturer part number":
               PartNumber = value;
               break;
            case "description":
               Description = value;
               break;
            case "customer reference":
               Reference = new(value);
               break;
            case "unit price":
               UnitPrice = ParseDec(value);
               break;
            case "backorder":
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
         if (part.Reference is null)
            return false;
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

      public bool Search(string text, bool matchCase)
      {
         if (matchCase)
            text = text.ToLower();
         var reference = ConvertCase(Reference?.ToString(), matchCase);
         if (reference?.Contains(text) == true)
            return true;
         var pn = ConvertCase(PartNumber, matchCase);
         if (pn?.Contains(text) == true)
            return true;
         var supplPN = ConvertCase(SupplierPartNumber, matchCase);
         if (supplPN?.Contains(text) == true)
            return true;

         return false;
      }

      private static string? ConvertCase(string? input, bool matchCase)
      {
         return matchCase && input is not null ? input.ToLower() : input;
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

      public override string ToString()
      {
         return $"{Reference} {PartNumber} {SupplierPartNumber} - {Quantity} {AllocatedQty}";
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

      public PartNumber? Reference
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

      public uint AllocatedQty
      {
         get => _alloc;
         set
         {
            _alloc = value;
            OnPropertyChanged();
         }
      }

      public uint Slippage
      {
         get => _slip;
         set
         {
            _slip = value;
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

      public Datasheet? Datasheet
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
