using MVVMLibrary;
using PartsInventory.ViewModels.Main;

namespace PartsInventory.ViewModels
{
   public interface IMainViewModel
   {
      string AspectRatio { get; set; }
      double MonitorSize { get; set; }
      Command OpenCmd { get; init; }
      Command SaveCmd { get; init; }

      void Open();
      void Save();
   }
}