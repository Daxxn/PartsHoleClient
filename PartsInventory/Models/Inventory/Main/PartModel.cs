﻿using System;
using System.Text.Json.Serialization;

using CSVParserLibrary.Models;

using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace PartsInventory.Models.Inventory.Main
{
   public class PartModel : BaseModel, IModel
   {
      #region Local Props
      public const string BlankPartNumber = "BLANK_PART";
      private string _supplierPartNumber = "";
      private string _partNumber = "";
      private string? _desc = null;
      private PartNumber? _reference = null;
      private uint _qty = 0;
      private uint _alloc = 0;
      private uint _slip = 0;
      private decimal _unitPrice = 0;
      private uint _backorder = 0;

      private Datasheet? _datasheetUrl = null;
      private string[]? _tags = null;

      [BsonRepresentation(BsonType.ObjectId)]
      public string? BinLocationId { get; set; }
      private BinModel _binLocation = new();
      #endregion

      #region Constructors
      public PartModel() { }
      #endregion

      #region Methods
      public static PartModel CreateNew()
      {
         return new()
         {
            Id = ObjectId.GenerateNewId().ToString(),
            PartNumber = BlankPartNumber,
         };
      }

      public static PartModel ConvertInvoicePart(InvoicePartModel invoicePart)
      {
         return new()
         {
            Quantity = invoicePart.Quantity,
            Backorder = invoicePart.Backorder,
            Description = invoicePart.Description,
            PartNumber = invoicePart.PartNumber,
            Reference = new(invoicePart.Reference),
            SupplierPartNumber = invoicePart.SupplierPartNumber,
            UnitPrice = invoicePart.UnitPrice,
         };
      }

      public static PartModel CopyTo(PartModel part)
      {
         return part.MemberwiseClone() is PartModel p ? p : new();
      }

      /// <summary>
      /// Parses the <paramref name="lines"/> from the NewPartView.
      /// </summary>
      /// <param name="lines"><see cref="Array"/> of <see langword="string"/>s to parse.</param>
      public void ParseRawProps(string[] lines)
      {
         for (int i = 0; i < lines.Length; i++)
         {
            if (string.IsNullOrEmpty(lines[i]))
               continue;
            if (lines[i][0] == '{')
            {
               if (lines[i] == "{NA}")
                  continue;
            }
            switch (i)
            {
               case 0:
                  Quantity = ParseUint(lines[i]);
                  break;
               case 1:
                  PartNumber = lines[i].Trim();
                  break;
               case 2:
                  SupplierPartNumber = lines[i].Trim();
                  break;
               case 3:
                  Reference = new(lines[i].Trim());
                  break;
               case 4:
                  Description = lines[i].Trim();
                  break;
               case 5:
                  UnitPrice = ParseDec(lines[i]);
                  break;
               case 6:
                  DatasheetUrl = new(lines[i].Trim());
                  break;
               case 7:
               // TODO - Add tags later
               // The tag system isnt even started yet. GAWD!! theres too much to do...
               default:
                  break;
            }
         }
      }

      /// <summary>
      /// Confirms the part can be created in the database.
      /// </summary>
      /// <returns>True if able.</returns>
      public bool CheckPart()
      {
         return !string.IsNullOrEmpty(SupplierPartNumber)
            && !string.IsNullOrEmpty(PartNumber)
            && Quantity != 0;
      }

      /// <summary>
      /// Checks equality.
      /// Favors <see cref="Reference"/> match.
      /// </summary>
      /// <param name="part"><see cref="PartModel"/> to check against</param>
      /// <returns></returns>
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

      /// <summary>
      /// Checks equality specifically for <see cref="PartNumber"/> and <see cref="SupplierPartNumber"/>.
      /// </summary>
      /// <param name="part"><see cref="PartModel"/> to check against.</param>
      /// <returns>True if <see cref="PartNumber"/> and <see cref="SupplierPartNumber"/> are equal.</returns>
      public bool EqualsPartNumber(PartModel part)
      {
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

      [CSVProperty("SupplierPartNumber")]
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

      [CSVProperty("DatasheetUrl")]
      [JsonIgnore]
      public Datasheet? DatasheetUrl
      {
         get => _datasheetUrl;
         set
         {
            _datasheetUrl = value;
            OnPropertyChanged();
         }
      }

      public string? Datasheet
      {
         get => _datasheetUrl?.Display;
         set => _datasheetUrl = new(value);
      }

      public BinModel BinLocation
      {
         get => _binLocation;
         set
         {
            _binLocation = value;
            BinLocationId = value.Id;
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
