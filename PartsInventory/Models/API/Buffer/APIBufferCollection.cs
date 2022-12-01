using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using PartsInventory.Models.Enums;
using PartsInventory.Models.Inventory;
using PartsInventory.Models.Inventory.Main;

namespace PartsInventory.Models.API.Buffer
{
   public class APIBufferCollection
   {
      #region Local Props
      private Dictionary<string, PartModel> PartModelBuffer { get; set; }
      private Dictionary<string, PartNumber> PartNumberBuffer { get; set; }
      private Dictionary<string, InvoiceModel> InvoiceModelBuffer { get; set; }
      private Dictionary<string, BinModel> BinModelBuffer { get; set; }

      #endregion

      #region Constructors
      public APIBufferCollection() { }
      #endregion

      #region Methods
      public Dictionary<string, PartModel> GetPartModelBuffer()
      {
         return PartModelBuffer;
      }
      public Dictionary<string, PartNumber> GetPartNumberBuffer()
      {
         return PartNumberBuffer;
      }
      public Dictionary<string, InvoiceModel> GetInvoiceModelBuffer()
      {
         return InvoiceModelBuffer;
      }
      public Dictionary<string, BinModel> GetBinModelBuffer()
      {
         return BinModelBuffer;
      }


      #endregion

      #region Full Props

      #endregion
   }
}
