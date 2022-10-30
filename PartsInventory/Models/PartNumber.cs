using MVVMLibrary;
using PartsInventory.Models.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PartsInventory.Models
{
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

      public bool Equals(PartNumber? pn)
      {
         if (pn is null) return false;
         return pn.TypeNum == TypeNum && pn.ID == ID;
      }

      public int CompareTo(PartNumber? other)
      {
         if (other is null) return 1;
         if (other == this) return 0;
         if (other.TypeNum > TypeNum) return -1;
         return other.ID.CompareTo(ID);
      }

      public override int GetHashCode()
      {
         return base.GetHashCode();
      }

      public static bool operator ==(PartNumber? left, PartNumber? right)
      {
         if (left is null)
         {
            return right is null;
         }

         return left.Equals(right);
      }

      public static bool operator !=(PartNumber left, PartNumber right)
      {
         return !(left == right);
      }

      public static bool operator <(PartNumber left, PartNumber right)
      {
         return left is null ? right is not null : left.CompareTo(right) < 0;
      }

      public static bool operator <=(PartNumber left, PartNumber right)
      {
         return left is null || left.CompareTo(right) <= 0;
      }

      public static bool operator >(PartNumber left, PartNumber right)
      {
         return left is not null && left.CompareTo(right) > 0;
      }

      public static bool operator >=(PartNumber left, PartNumber right)
      {
         return left is null ? right is null : left.CompareTo(right) >= 0;
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
