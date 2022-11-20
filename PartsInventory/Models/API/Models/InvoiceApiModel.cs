using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

using PartsInventory.Models.API.Interfaces;
using PartsInventory.Models.Enums;
using PartsInventory.Models.Inventory.Main;

namespace PartsInventory.Models.API.Models
{
   public class InvoiceApiModel : IApiConverter<InvoiceModel>
   {
      #region Local Props
      public string _id { get; set; } = null!;
      public uint OrderNumber { get; set; }
      public IEnumerable<DigiKeyPartModel> Parts { get; set; } = null!;
      public string Path { get; set; } = null!;
      public decimal SubTotal { get; set; }
      public int? SupplierType { get; set; }
      public bool IsAddedToParts { get; set; }
      #endregion

      #region Constructors
      public InvoiceApiModel() { }
      #endregion

      #region Methods
      public InvoiceModel ToModel()
      {
         return new()
         {
            Id = _id,
            OrderNumber = OrderNumber,
            Parts = new(Parts),
            Path = Path,
            SubTotal = SubTotal,
            SupplierType = (SupplierType)(SupplierType ?? 2),
            IsAddedToParts = IsAddedToParts
         };
      }

      public static InvoiceApiModel FromModel(InvoiceModel model)
      {
         return new()
         {
            _id = model.Id,
            OrderNumber = model.OrderNumber,
            Path = model.Path,
            SubTotal = model.SubTotal,
            SupplierType = (int?)model.SupplierType,
            Parts = model.Parts,
            IsAddedToParts = model.IsAddedToParts,
         };
      }
      #endregion

      #region Full Props

      #endregion
   }
}
