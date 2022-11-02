using System;

using MVVMLibrary;

using PartsInventory.Models;

namespace PartsInventory.ViewModels
{
   public interface IPartNumberTemplateViewModel
   {
      Command CreatePartNumCmd { get; init; }
      PartNumber PartNumber { get; set; }
      Command SelectTypeNumCmd { get; init; }

      event EventHandler<PartNumber> CreatePartNumber;

      void CreatePartNum();
      void SelectTypeNum(object param);
   }
}