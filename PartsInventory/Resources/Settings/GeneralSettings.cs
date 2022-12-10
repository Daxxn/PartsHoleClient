using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace PartsInventory.Resources.Settings;

public class GeneralSettings
{
   private double _messageInterval = 10;
   public int MonitorSize { get; set; }
   public string AspectRatio { get; set; } = "16/9";
   public int CurrentMainTab { get; set; }
   public int CurrentPassivesTab { get; set; }
   public double MessageInterval { get => _messageInterval * 1000; set => _messageInterval = value; }
   public double APIUpdateInterval { get; set; } = 10;
   [JsonIgnore]
   public double APIUpdateIntervalms { get => APIUpdateInterval * 1000 * 60; }
   public TabSettings TabIndecies { get; set; } = null!;
}

public class TabSettings
{
   public int Main { get; set; }
   public int Passives { get; set; }
}
