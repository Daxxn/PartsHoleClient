using System;

using MVVMLibrary;

using PartsInventory.Models;

namespace PartsInventory.ViewModels
{
   public interface IProjectBOMViewModel
   {
      Command AllocateCmd { get; init; }
      PartsCollection? AllParts { get; set; }
      Command ClearProjectCmd { get; init; }
      int CurrentTab { get; set; }
      Command ParseProjectCmd { get; init; }
      ProjectModel? Project { get; set; }

      void Exit(object sender, EventArgs e);
      void Loaded(object sender, EventArgs e);
      void PartsChanged_Main(object sender, PartsCollection e);
   }
}