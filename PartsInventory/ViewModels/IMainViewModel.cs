using System.Threading.Tasks;

using MVVMLibrary;

using PartsInventory.Models.Inventory;
using PartsInventory.Models.Inventory.Main;

namespace PartsInventory.ViewModels
{
   public interface IMainViewModel
   {
      IUserModel User { get; set; }
      string AspectRatio { get; set; }
      double MonitorSize { get; set; }
      Command OpenCmd { get; init; }
      Command SaveCmd { get; init; }

      void Open();
      void Save();

      Task<bool> AddPart(PartModel part);
      Task RemovePart(PartModel part);
      void GetUserTestAsync();
   }
}