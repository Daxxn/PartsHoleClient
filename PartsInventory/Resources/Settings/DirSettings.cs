using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PartsInventory.Resources.Settings
{
   public class DirSettings
   {
      public string PartInvoiceDir { get; set; }
      public string DigiKeyDir { get; set; }
      public string MouserDir { get; set; }
      public string SettingsPath { get; set; }
      public string DatasheetsPath { get; set; }
      public string AppDataPath { get; set; }
      public string AppDataFileName { get; set; }
      public string BinLocFileName { get; set; }
      public string KiCADProjects { get; set; }
      public string BOMs { get; set; }
      public string ElectricalCalcExe { get; set; }
      public string SaturnPCBExe { get; set; }
      public Tools[] ExtraTools { get; set; }
   }

   public class Tools
   {
      public string Key { get; set; }
      public string Name { get; set; }
      public string Path { get; set; }
   }
}
