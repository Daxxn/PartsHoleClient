using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using MongoDB.Bson;

using PartsInventory.Models.API.Interfaces;
using PartsInventory.Models.Enums;
using PartsInventory.Models.Inventory.Main;

namespace PartsInventory.Models.API.Models
{
   public class InvoiceApiModel : IApiConverter<InvoiceModel>
   {
      #region Local Props
      public string Id { get; set; }
      public uint OrderNumber { get; set; }
      public IEnumerable<string> Parts { get; set; }
      public string Path { get; set; }
      public decimal SubTotal { get; set; }
      public SupplierType? SupplierType { get; set; }
      #endregion

      #region Constructors
      public InvoiceApiModel() { }
      #endregion

      #region Methods
      public InvoiceModel ToModel()
      {
         return new()
         {
            Id = Id,
            OrderNumber = OrderNumber,
            PartIDs = Parts,
            Path = Path,
            SubTotal = SubTotal,
            SupplierType = SupplierType,
         };
      }

      public static InvoiceApiModel FromModel(InvoiceModel model)
      {
         return new()
         {
            Id = model.Id,
            OrderNumber = model.OrderNumber,
            Path = model.Path,
            SubTotal = model.SubTotal,
            SupplierType = model.SupplierType,
            Parts = model.Parts.Select(x => x.Id),
         };
      }
      #endregion

      #region Full Props

      #endregion
   }
}
