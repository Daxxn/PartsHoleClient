using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.Xml;
using System.Text;
using System.Threading.Tasks;
using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Bson;

using PartsInventory.Models.API.Interfaces;
using PartsInventory.Models.Inventory;
using PartsInventory.Models.Inventory.Main;

namespace PartsInventory.Models.API.Models
{
   public class PartApiModel : IApiConverter<PartModel>
   {
      #region Local Props
      public string _id { get; set; } = null!;
      public string SupplierPartNumber { get; set; } = null!;
      public string PartNumber { get; set; } = null!;
      public string? Description { get; set; }
      public string? Reference { get; set; }
      public uint Quantity { get; set; }
      public uint AllocatedQty { get; set; }
      public uint Slippage { get; set; }
      public decimal UnitPrice { get; set; }
      public uint Backorder { get; set; }

      public string? Datasheet { get; set; }
      public string[]? Tags { get; set; }
      public string? BinLocationId { get; set; }
      #endregion

      #region Constructors
      public PartApiModel() { }
      #endregion

      #region Methods
      public PartModel ToModel()
      {
         var reference = Reference is null ? new() : new PartNumber(Reference);
         var datasheet = new Datasheet(Datasheet);
         return new PartModel()
         {
            Id = _id,
            AllocatedQty = AllocatedQty,
            Backorder = Backorder,
            UnitPrice = UnitPrice,
            Datasheet = datasheet,
            Description = Description,
            PartNumber = PartNumber,
            Quantity = Quantity,
            Reference = reference,
            Slippage = Slippage,
            SupplierPartNumber = SupplierPartNumber,
            Tags = Tags,
            BinLocationId = BinLocationId
         };
      }

      public static PartApiModel FromModel(PartModel model)
      {
         return new()
         {
            _id = model.Id,
            AllocatedQty = model.AllocatedQty,
            Backorder = model.Backorder,
            UnitPrice = model.UnitPrice,
            Datasheet = model.Datasheet?.Path?.AbsolutePath,
            Description = model.Description,
            PartNumber = model.PartNumber,
            Quantity = model.Quantity,
            Reference = model.Reference?.ToString(),
            Slippage = model.Slippage,
            SupplierPartNumber = model.SupplierPartNumber,
            Tags = model.Tags,
            BinLocationId = model.BinLocation.Id,
         };
      }
      #endregion

      #region Full Props

      #endregion
   }
}
