using System;
using System.Collections.Generic;
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
      public SupplierType SupplierType { get; set; }
      #endregion

      #region Constructors
      public InvoiceApiModel() { }
      #endregion

      #region Methods
      public InvoiceModel Convert()
      {
         return new()
         {
            Id = new(Id),
            OrderNumber = OrderNumber,
            PartIDs = Parts,
            Path = Path,
            SubTotal = SubTotal,
            SupplierType = SupplierType,
         };
      }
      #endregion

      #region Full Props

      #endregion
   }
}
