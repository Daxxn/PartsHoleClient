using PartsInventory.Models.Inventory.Main;

namespace PartsInventory.Models.Inventory
{
   public interface IPartNumber
   {
      uint ID { get; set; }
      uint TypeNum { get; set; }

      int CompareTo(IPartNumber? other);
      bool Equals(object? obj);
      bool Equals(IPartNumber? pn);
      int GetHashCode();
      void Parse(string input);
      string ToString();
   }
}