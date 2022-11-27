namespace PartsInventory.Models
{
   public interface IAbstractFactory<T>
   {
      T Create();
   }
}