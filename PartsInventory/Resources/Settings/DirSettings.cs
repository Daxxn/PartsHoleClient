using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PartsInventory.Resources.Settings
{
   public class DirSettings
   {
      public string PartInvoiceDir { get; set; } = null!;
      public string DigiKeyDir { get; set; } = null!;
      public string MouserDir { get; set; } = null!;
      public string SettingsPath { get; set; } = null!;
      public string DatasheetsPath { get; set; } = null!;
      public string AppDataPath { get; set; } = null!;
      public string AppDataFileName { get; set; } = null!;
      public string BinLocFileName { get; set; } = null!;
      public string KiCADProjects { get; set; } = null!;
      public string BOMs { get; set; } = null!;
      public string ElectricalCalcExe { get; set; } = null!;
      public string SaturnPCBExe { get; set; } = null!;
      public Tools[] ExtraTools { get; set; } = null!;
   }

   public class Tools
   {
      public string Key { get; set; } = null!;
      public string Name { get; set; } = null!;
      public string Path { get; set; } = null!;
   }
}
