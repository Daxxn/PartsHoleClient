using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

using MVVMLibrary;
using PartsInventory.Models.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PartsInventory.Models.Inventory.Main
{
   public class PartNumber : BaseModel, IComparable<PartNumber>
   {
      #region Local Props
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
      private uint _category = 0;
      private uint _subCategory = 0;
      private uint _partId = 0;
      #endregion

      #region Constructors
      public PartNumber() { }
      public PartNumber(string input)
      {
         Parse(input);
      }
      public PartNumber(uint type, uint partId)
      {
         Category = type;
         PartID = partId;
      }
      #endregion

      #region Methods
      public override string ToString()
      {
         return $"{Category:D4}-{PartID:D4}";
      }

      public void Parse(string input)
      {
         if (string.IsNullOrEmpty(input))
            return;

         var spl = input.Split('-');

         if (spl.Length == 2)
         {
            if (uint.TryParse(spl[0], out uint typeNum))
            {
               Category = typeNum;
            }
            if (uint.TryParse(spl[1], out uint partId))
            {
               PartID = partId;
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
         if (pn is null)
            return false;
         return pn.Category == Category && pn.PartID == PartID;
      }

      public int CompareTo(PartNumber? other)
      {
         if (other is null)
            return 1;
         if (other == this)
            return 0;
         if (other.Category > Category)
            return -1;
         return other.PartID.CompareTo(PartID);
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
      /// <summary>
      /// The first 2 numbers of the part number.
      /// <list type="table">
      ///   <listheader>
      ///      <term>(Category</term>
      ///      <term>Sub Category)</term>
      ///      <description>Part ID</description>
      ///   </listheader>
      ///   <item>
      ///      <term>(##</term>
      ///      <term>##)</term>
      ///      <description>####</description>
      ///   </item>
      /// </list>
      /// </summary>
      public uint Category
      {
         get => _category;
         set
         {
            _category = value;
            OnPropertyChanged();
         }
      }

      /// <summary>
      /// Second 2 numbers of the part number.
      /// <list type="table">
      ///   <listheader>
      ///      <term>(Category</term>
      ///      <term>Sub Category)</term>
      ///      <description>Part ID</description>
      ///   </listheader>
      ///   <item>
      ///      <term>(##</term>
      ///      <term>##)</term>
      ///      <description>####</description>
      ///   </item>
      /// </list>
      /// </summary>
      public uint SubCategory
      {
         get => _subCategory;
         set
         {
            _subCategory = value;
            OnPropertyChanged();
         }
      }

      /// <summary>
      /// Unique ID for a <see cref="PartModel"/>
      /// <para/>
      /// NOT the same as the Models <see cref="ObjectId"/>.
      /// </summary>
      public uint PartID
      {
         get => _partId;
         set
         {
            _partId = value;
            OnPropertyChanged();
         }
      }

      /// <summary>
      /// Combined categories. The first 4 numbers of the part number.
      /// <list type="table">
      ///   <listheader>
      ///      <term>Full Category</term>
      ///      <description>Part ID</description>
      ///   </listheader>
      ///   <item>
      ///      <term>####</term>
      ///      <description>####</description>
      ///   </item>
      /// </list>
      /// </summary>
      public uint FullCategory => (uint)(Category * Math.Pow(10, 2)) + SubCategory;
      #endregion
   }
}
