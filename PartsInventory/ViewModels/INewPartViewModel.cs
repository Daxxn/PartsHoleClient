using System.Threading.Tasks;

using MVVMLibrary;

using PartsInventory.Models.Inventory.Main;

namespace PartsInventory.ViewModels
{
   public interface INewPartViewModel
   {
      Command ClearCmd { get; init; }
      Command ParseCSVCmd { get; init; }
      PartModel? NewPart { get; set; }
      string? CSVLine { get; set; }

      Task<bool> Submit();
   }
}