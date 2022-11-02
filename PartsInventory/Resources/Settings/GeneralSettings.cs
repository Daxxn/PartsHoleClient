using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PartsInventory.Resources.Settings
{
   public class GeneralSettings
   {
      public int MonitorSize { get; set; }
      public string AspectRatio { get; set; } = "16/9";
      public int CurrentMainTab { get; set; }
      public int CurrentPassivesTab { get; set; }
      public TabSettings TabIndecies { get; set; } = null!;
   }

   public class TabSettings
   {
      public int Main { get; set; }
      public int Passives { get; set; }
   }
}
