using System;

using MVVMLibrary;

using PartsInventory.Models;
using PartsInventory.Models.Inventory;
using PartsInventory.Models.Inventory.Main;

namespace PartsInventory.ViewModels;

public interface IProjectBOMViewModel
{
   IMainViewModel MainVM { get; }
   int CurrentTab { get; set; }
   ProjectModel? Project { get; set; }
   Command AllocateCmd { get; init; }
   Command ClearProjectCmd { get; init; }
   Command ParseProjectCmd { get; init; }

   void Exit(object sender, EventArgs e);
   void Loaded(object sender, EventArgs e);
   //void PartsChanged_Main(object sender, UserModel e);
}