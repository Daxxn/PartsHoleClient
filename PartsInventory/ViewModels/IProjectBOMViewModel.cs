using System;

using MVVMLibrary;

using PartsInventory.Models;
using PartsInventory.Models.Inventory;
using PartsInventory.Models.Inventory.Main;

namespace PartsInventory.ViewModels
{
   public interface IProjectBOMViewModel
   {
      Command AllocateCmd { get; init; }
      UserModel? AllParts { get; set; }
      Command ClearProjectCmd { get; init; }
      int CurrentTab { get; set; }
      Command ParseProjectCmd { get; init; }
      ProjectModel? Project { get; set; }

      void Exit(object sender, EventArgs e);
      void Loaded(object sender, EventArgs e);
      void PartsChanged_Main(object sender, UserModel e);
   }
}