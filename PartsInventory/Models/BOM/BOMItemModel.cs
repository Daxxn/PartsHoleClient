using CSVParserLibrary.Models;

using MVVMLibrary;

using PartsInventory.Models.Enums;
using PartsInventory.Models.Inventory;
using PartsInventory.Models.Inventory.Main;
using PartsInventory.Models.Packages;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PartsInventory.Models.BOM
{
    public class BOMItemModel : PartModel
   {
      #region Local Props
      private static readonly Dictionary<string, PartType> PartTypeDecoder = new()
      {
         { "A", PartType.Arduino },
         { "AT", PartType.Attenuator },
         { "BR", PartType.BridgeRectifier },
         { "B", PartType.Battery },
         { "C", PartType.Capacitor },
         { "CN", PartType.CapacitorNetwork },
         { "D", PartType.Diode },
         { "DN", PartType.DiodeNetwork },
         { "DL", PartType.DelayLine },
         { "DS", PartType.Display },
         { "F", PartType.Fuse },
         { "FB", PartType.FerriteBead },
         { "FD", PartType.Fiducial },
         { "FL", PartType.Filter },
         { "H", PartType.Hardware },
         { "HY", PartType.DirectionalCoupler },
         { "IR", PartType.InfraredDiode },
         { "J", PartType.Connector },
         { "JP", PartType.Jumper },
         { "RL", PartType.Relay },
         { "L", PartType.Inductor },
         { "LS", PartType.Speaker },
         { "M", PartType.Motor },
         { "MK", PartType.Microphone },
         { "OP", PartType.OptoIsolator },
         { "PS", PartType.PowerSupply },
         { "Q", PartType.Transistor },
         { "R", PartType.Resistor },
         { "RC", PartType.RotaryEncoder },
         { "RN", PartType.ResistorNetwork },
         { "RT", PartType.Thermistor },
         { "RV", PartType.Varistor },
         { "RS", PartType.SenseResistor },
         { "S", PartType.Switch },
         { "T", PartType.Transformer },
         { "TC", PartType.Thermocouple },
         { "TP", PartType.TestPoint },
         { "TN", PartType.Tuner },
         { "U", PartType.IC },
         { "V", PartType.VacuumTube },
         { "VR", PartType.VoltageRegulator },
         { "SK", PartType.Socket },
         { "Y", PartType.Oscillator },

         { "XTAL", PartType.Crystal },
         { "USB", PartType.USBConnector }
      };
      private string _refDes = "";
      private ValueModel _value = new();
      private PackageModel? _package = null;
      private string _library = "No Lib";
      private PartType _type = PartType.Unknown;
      private string? _symbol = null;
      #endregion

      #region Constructors
      public BOMItemModel() { }
      #endregion

      #region Methods
      public new void ParseProp(string prop, string value)
      {
         switch (prop)
         {
            case "Ref":
               ReferenceDesignator = value;
               if (value.Length <= 0) break;
               var refDes = GetRefDesType(value);
               if (PartTypeDecoder.ContainsKey(refDes))
               {
                  Type = PartTypeDecoder[refDes];
               }
               break;
            case "Value":
               Value.RawValue = value;
               break;
            case "Footprint":
               Package = PackageModel.Parse(value);
               break;
            case "DatasheetUrl":
               DatasheetUrl = new(value);
               break;
            case "Pins":
               if (uint.TryParse(value, out uint pinCount))
               {
                  Package ??= new();
                  Package.PinCount = pinCount;
               }
               break;
            case "Library":
               if (!string.IsNullOrEmpty(value))
               {
                  Library = value;
               }
               break;
            case "Symbol":
               if (!string.IsNullOrEmpty(value))
               {
                  Symbol = value;
               }
               break;
            case "SupplierPartNumber":
               Reference = new(value);
               break;
            default:
               break;
         }
      }

      private string GetRefDesType(string input)
      {
         StringBuilder b = new();
         foreach (var c in input)
         {
            if (char.IsLetter(c))
            {
               b.Append(c);
            }
            else break;
         }
         return b.ToString();
      }

      public override string ToString()
      {
         return $"{ReferenceDesignator} {Reference} {Value} {Library}";
      }
      #endregion

      #region Full Props
      [CSVProperty("Ref")]
      public string ReferenceDesignator
      {
         get => _refDes;
         set
         {
            _refDes = value;
            if (value.Length <= 0)
               return;
            var refDes = GetRefDesType(value);
            if (PartTypeDecoder.ContainsKey(refDes))
            {
               Type = PartTypeDecoder[refDes];
            }
            OnPropertyChanged();
         }
      }

      [CSVProperty("Value")]
      public ValueModel Value
      {
         get => _value;
         set
         {
            _value = value;
            OnPropertyChanged();
         }
      }

      [CSVProperty("Footprint")]
      public PackageModel? Package
      {
         get => _package;
         set
         {
            _package = value;
            OnPropertyChanged();
         }
      }

      [CSVProperty("Library")]
      public string Library
      {
         get => _library;
         set
         {
            _library = value;
            OnPropertyChanged();
         }
      }

      public PartType Type
      {
         get => _type;
         set
         {
            _type = value;
            OnPropertyChanged();
         }
      }

      [CSVProperty("Symbol")]
      public string? Symbol
      {
         get => _symbol;
         set
         {
            _symbol = value;
            OnPropertyChanged();
         }
      }
      #endregion
   }
}
