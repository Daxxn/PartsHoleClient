using MVVMLibrary;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PartsInventory.Models
{
   #region Enums
   public enum PartNumberType
   {
      Other,

      Passives,
      Protection,
      Mechanical,
      Connector,
      Lighting,
      Diode,
      Transistor,
      IC,
      Display,
      ElectroMechanical,
      Switch_Input
   }

   public enum PartNumberSubTypes
   {
      Other = 0,
      Resistor = 0101,
      capacitor = 0102,
      Inductor = 0103,
      Ferrites = 0104,
      Crystal = 0105,
      Resonator = 0106,
      Fuse = 0201,
      CircuitBreaker = 0202,
      Varistor = 0203,
      PTCFuse = 0204,
      Screw = 0301,
      Nut = 0302,
      Standoff = 0303,
      PinHeader = 0401,
      PinSocket = 0402,
      TerminalBlock = 0403,
      DSub = 0404,
      Circular = 0405,
      FlatFlex = 0406,
      Audio = 0407,
      USB = 08,
      BarrelJack = 0409,
      MOLEX = 0410,
      Programming = 0411,
      PCIe = 0412,
      LED = 0501,
      Fillament = 0502,
      Fluorescent = 0503,
      GeneralPurpose = 0601,
      Schottky = 0602,
      Zener = 0603,
      TVS = 0604,
      NPN = 0701,
      PNP = 0702,
      Nch = 0703,
      Pch = 0704,
      MicroController = 0801,
      Logic = 0802,
      OPAMP = 0803,
      LinearReg = 0804,
      SwitchingReg = 0805,
      Interface = 0806,
      AnalogLogic = 0807,
      ADC = 0809,
      DAC = 0810,
      ROM = 0811,
      EEPROM = 0812,
      Memory = 0813,
      Processor = 0814,
      Sensing = 0815,
      CurrentSense = 0816,
      LCDCharacter = 0901,
      LCDPanel = 0902,
      SevenSegment = 0903,
      BarGraph = 0904,
      OLED = 0905,
      Relay = 1001,
      Contactor = 1002,
      Mic = 1003,
      Speaker = 1004,
      Buzzer = 1005,
      Motor = 1006,
      Tactile = 1101,
      Toggle = 1102,
      DIP = 1103,
      Limit = 1104,
      Rotary = 1105,
      Slide = 1106,
      Rocker = 1107,
      RotaryEncoder = 1108,
      Potentiometer = 1109,
      Keypad = 1110,
      Keylock = 1111,
      Navigation = 1112
   }
   #endregion

   public class PartNumber : Model, IComparable<PartNumber>
   {
      #region Local Props
      //private static Dictionary<PartNumberType, uint> SubTypeIDs = new()
      //{
      //   { PartNumberType.Passives, 6 },
      //   { PartNumberType.Protection, 4 },
      //   { PartNumberType.Mechanical, 3 },
      //   { PartNumberType.Connector, 12 },
      //   { PartNumberType.Lighting, 3 },
      //   { PartNumberType.Diode, 4 },
      //   { PartNumberType.Transistor, 5 },
      //   { PartNumberType.IC, 16 },
      //   { PartNumberType.Display, 5 },
      //   { PartNumberType.ElectroMechanical, 6 },
      //   { PartNumberType.Switch_Input, 12 },
      //};
      public static Dictionary<PartNumberType, PartNumberSubTypes[]> SubTypeDisplay = new()
      {
         { PartNumberType.Passives, new PartNumberSubTypes[] { PartNumberSubTypes.Resistor, PartNumberSubTypes.capacitor, PartNumberSubTypes.Inductor, PartNumberSubTypes.Ferrites, PartNumberSubTypes.Crystal, PartNumberSubTypes.Resonator } },
         { PartNumberType.Protection, new PartNumberSubTypes[] { PartNumberSubTypes.Fuse, PartNumberSubTypes.CircuitBreaker, PartNumberSubTypes.Varistor, PartNumberSubTypes.PTCFuse } },
         { PartNumberType.Mechanical, new PartNumberSubTypes[] { PartNumberSubTypes.Screw, PartNumberSubTypes.Nut, PartNumberSubTypes.Standoff } },
         { PartNumberType.Connector, new PartNumberSubTypes[] { PartNumberSubTypes.PinHeader, PartNumberSubTypes.PinSocket, PartNumberSubTypes.TerminalBlock, PartNumberSubTypes.DSub, PartNumberSubTypes.Circular, PartNumberSubTypes.FlatFlex, PartNumberSubTypes.Audio, PartNumberSubTypes.USB, PartNumberSubTypes.BarrelJack, PartNumberSubTypes.MOLEX, PartNumberSubTypes.Programming, PartNumberSubTypes.PCIe } },
         { PartNumberType.Lighting, new PartNumberSubTypes[] { PartNumberSubTypes.LED, PartNumberSubTypes.Fillament, PartNumberSubTypes.Fluorescent } },
         { PartNumberType.Diode, new PartNumberSubTypes[] { PartNumberSubTypes.GeneralPurpose, PartNumberSubTypes.Schottky, PartNumberSubTypes.Zener, PartNumberSubTypes.TVS } },
         { PartNumberType.Transistor, new PartNumberSubTypes[] { PartNumberSubTypes.NPN, PartNumberSubTypes.PNP, PartNumberSubTypes.Nch, PartNumberSubTypes.Pch } },
         { PartNumberType.IC, new PartNumberSubTypes[] {PartNumberSubTypes.MicroController, PartNumberSubTypes.Logic, PartNumberSubTypes.OPAMP, PartNumberSubTypes.LinearReg, PartNumberSubTypes.SwitchingReg, PartNumberSubTypes.Interface, PartNumberSubTypes.AnalogLogic, PartNumberSubTypes.USB, PartNumberSubTypes.ADC, PartNumberSubTypes.DAC, PartNumberSubTypes.ROM, PartNumberSubTypes.EEPROM, PartNumberSubTypes.Memory, PartNumberSubTypes.Processor, PartNumberSubTypes.Sensing, PartNumberSubTypes.CurrentSense } },
         { PartNumberType.Display, new PartNumberSubTypes[] {PartNumberSubTypes.LCDCharacter, PartNumberSubTypes.LCDPanel, PartNumberSubTypes.SevenSegment, PartNumberSubTypes.BarGraph, PartNumberSubTypes.OLED} },
         { PartNumberType.ElectroMechanical, new PartNumberSubTypes[] { PartNumberSubTypes.Relay, PartNumberSubTypes.Contactor, PartNumberSubTypes.Mic, PartNumberSubTypes.Speaker, PartNumberSubTypes.Buzzer, PartNumberSubTypes.Motor } },
         { PartNumberType.Switch_Input, new PartNumberSubTypes[] { PartNumberSubTypes.Tactile, PartNumberSubTypes.Toggle, PartNumberSubTypes.DIP, PartNumberSubTypes.Limit, PartNumberSubTypes.Rotary, PartNumberSubTypes.Slide, PartNumberSubTypes.Rocker, PartNumberSubTypes.RotaryEncoder, PartNumberSubTypes.Potentiometer, PartNumberSubTypes.Keypad, PartNumberSubTypes.Keylock, PartNumberSubTypes.Navigation } }
      };
      private uint _typeNum = 0;
      private uint _id = 0;
      #endregion

      #region Constructors
      public PartNumber() { }
      public PartNumber(string input)
      {
         Parse(input);
      }
      public PartNumber(uint type, uint id)
      {
         TypeNum = type;
         ID = id;
      }
      #endregion

      #region Methods
      public override string ToString()
      {
         return $"{TypeNum:D4}-{ID:D4}";
      }

      public void Parse(string input)
      {
         if (string.IsNullOrEmpty(input)) return;

         var spl = input.Split('-');

         if (spl.Length == 2)
         {
            if (uint.TryParse(spl[0], out uint typeNum))
            {
               TypeNum = typeNum;
            }
            if (uint.TryParse(spl[1], out uint id))
            {
               ID = id;
            }
         }
      }

      public static PartNumber Create(PartNumberType type, PartNumberSubTypes subType)
      {
         return new((uint)subType, 0);
      }

      public override bool Equals(object? obj)
      {
         if (obj is PartNumber pn)
            return Equals(pn);
         return base.Equals(obj);
      }

      public bool Equals(PartNumber pn)
      {
         return pn.TypeNum == TypeNum && pn.ID == ID;
      }

      public int CompareTo(PartNumber? other)
      {
         if (other == null) return 1;
         if (other == this) return 0;
         if (other.TypeNum > TypeNum) return -1;
         return other.ID.CompareTo(ID);
      }
      #endregion

      #region Full Props
      public uint TypeNum
      {
         get => _typeNum;
         set
         {
            _typeNum = value;
            OnPropertyChanged();
         }
      }

      public uint ID
      {
         get => _id;
         set
         {
            _id = value;
            OnPropertyChanged();
         }
      }
      #endregion
   }
}
