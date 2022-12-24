using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Threading.Tasks;

using MVVMLibrary;

using PartsInventory.Models.Inventory;
using PartsInventory.Models.Inventory.Main;

using IModel = PartsInventory.Models.Inventory.IModel;

namespace PartsInventory.ViewModels;

public interface IMainViewModel
{
   IUserModel User { get; set; }
   public ObservableCollection<PartModel>? SelectedParts { get; set; }
   string AspectRatio { get; set; }
   double MonitorSize { get; set; }
   Command OpenCmd { get; init; }
   Command SaveCmd { get; init; }
   Command SendFileTestCmd { get; init; }
   void Open();
   void Save();

   Task<bool> AddPart(PartModel part);
   Task<bool> AddParts(IEnumerable<PartModel> parts);
   Task RemovePart(PartModel part);
   void GetUserTestAsync();

   /// <summary>
   /// Updates the API with any minor changes to a model.
   /// <para/>
   /// Minor changes like property updates ONLY
   /// </summary>
   /// <param name="model">Model to update.</param>
   void UpdateAPI(IModel model);
}