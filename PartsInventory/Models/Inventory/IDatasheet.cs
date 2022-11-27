using System;

namespace PartsInventory.Models.Inventory.Main
{
   public interface IDatasheet
   {
      string? Display { get; }
      bool IsGoodPath { get; }
      Uri? Path { get; set; }
   }
}