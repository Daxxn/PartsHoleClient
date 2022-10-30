using PartsInventory.Models.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PartsInventory.Models.Passives
{
   public interface IPassive
   {
      double Value { get; set; }
      double Tolerance { get; set; }
      string ToleranceDisplay { get; }
      string? Description { get; set; }
      uint Quantity { get; set; }
      uint AllocatedQty { get; set; }
      char UnitLetter { get; }
      char TypeLetter { get; }
      string? ManufacturerNumber { get; set; }
      PartNumber? PartNumber { get; set; }
      BinModel? BinLocation { get; set; }
      string? PackageName { get; set; }
      public PackageType PackageType { get; set; }

      void ParseDesc(string? desc);
   }
}
