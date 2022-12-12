using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using CSVParserLibrary.Models;
using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Bson;
using System.Text.Json.Serialization;

namespace PartsInventory.Models.Inventory.Main;

public class InvoicePartModel : BaseModel
{
   #region Local Props
   public uint Quantity { get; set; }
   public string SupplierPartNumber { get; set; } = null!;
   public string PartNumber { get; set; } = null!;
   public string Description { get; set; } = null!;
   public string Reference { get; set; } = null!;
   public uint Backorder { get; set; }
   public decimal UnitPrice { get; set; }
   private bool _addtoInventory = true;
   #endregion

   #region Constructors
   public InvoicePartModel() => Id = ObjectId.GenerateNewId().ToString();
   #endregion

   #region Methods
   public override string ToString() =>
      $"{(Id is null ? "" : "'ID'")} {PartNumber} {SupplierPartNumber} {Reference} {Quantity} {UnitPrice} {Description}";
   #endregion

   #region Other Props
   [JsonIgnore]
   public decimal ExtendedPrice => UnitPrice * Quantity;

   public bool AddToInventory
   {
      get => _addtoInventory;
      set
      {
         _addtoInventory = value;
         OnPropertyChanged();
      }
   }
   #endregion
}
