using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using CSVParserLibrary.Models;
using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Bson;

namespace PartsInventory.Models.Inventory.Main;

public class DigiKeyPartModel
{
   #region Local Props
   [BsonId]
   [BsonRepresentation(BsonType.ObjectId)]
   public string Id { get; set; } = null!;
   public int Quantity { get; set; }
   public string PartNumber { get; set; } = null!;
   public string ManufacturerPartNumber { get; set; } = null!;
   public string Description { get; set; } = null!;
   public string CustomerReference { get; set; } = null!;
   public int Backorder { get; set; }
   public decimal UnitPrice { get; set; }
   #endregion

   #region Constructors
   public DigiKeyPartModel() => Id = ObjectId.GenerateNewId().ToString();
   #endregion

   #region Methods
   public override string ToString() =>
      $"{(Id is null ? "" : "'ID'")} {ManufacturerPartNumber} {PartNumber} {CustomerReference} {Quantity} {UnitPrice} {Description}";
   #endregion

   #region Other Props
   public decimal ExtendedPrice => UnitPrice * Quantity;
   #endregion
}
