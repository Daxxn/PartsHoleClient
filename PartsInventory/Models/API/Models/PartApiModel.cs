using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.Xml;
using System.Text;
using System.Threading.Tasks;
using PartsInventory.Models.API.Interfaces;
using PartsInventory.Models.Inventory;
using PartsInventory.Models.Inventory.Main;

namespace PartsInventory.Models.API.Models
{
   public class PartApiModel : IApiConverter<PartModel>
   {
      #region Local Props
      public string Id { get; set; } = null!;
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
      public string? BinLocation { get; set; }
      #endregion

      #region Constructors
      public PartApiModel() { }
      #endregion

      #region Methods
      public PartModel Convert()
      {
         var reference = Reference is null ? new() : new PartNumber(Reference);
         var datasheet = new Datasheet(Datasheet);
         return new PartModel()
         {
            Id = new(Id),
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
         };
      }
      #endregion

      #region Full Props

      #endregion
   }
}
