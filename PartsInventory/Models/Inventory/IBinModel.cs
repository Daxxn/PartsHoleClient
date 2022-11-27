namespace PartsInventory.Models.Inventory
{
   public interface IBinModel
   {
      int Horizontal { get; set; }
      bool IsBook { get; set; }
      string Name { get; set; }
      int Vertical { get; set; }

      string ToString();
   }
}