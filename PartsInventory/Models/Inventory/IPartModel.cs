using PartsInventory.Models.Inventory.Main;

namespace PartsInventory.Models.Inventory
{
   public interface IPartModel
   {
      uint AllocatedQty { get; set; }
      uint Backorder { get; set; }
      IBinModel BinLocation { get; set; }
      IDatasheet? Datasheet { get; set; }
      string? Description { get; set; }
      decimal ExtendedPrice { get; }
      string PartNumber { get; set; }
      uint Quantity { get; set; }
      IPartNumber? Reference { get; set; }
      uint Slippage { get; set; }
      string SupplierPartNumber { get; set; }
      string[]? Tags { get; set; }
      decimal UnitPrice { get; set; }

      bool Equals(IPartModel part);
      void ParseProp(string prop, string value);
      bool Search(string text, bool matchCase);
      string ToString();
   }
}