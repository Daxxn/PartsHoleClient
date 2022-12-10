using System;
using System.Collections.Generic;

using MongoDB.Bson;

using PartsInventory.Models.Enums;

namespace PartsInventory.Models.Inventory.Main
{
   public class PartNumber : BaseModel, IComparable<PartNumber>, IModel
   {
      #region Local Props
      public static Dictionary<PartNumberCategory, PartNumberSubCategory[]> SubTypeDisplay = new()
      {
         { PartNumberCategory.Other, new PartNumberSubCategory[] { PartNumberSubCategory.Other } },
         { PartNumberCategory.Passives, new PartNumberSubCategory[] { PartNumberSubCategory.Resistor, PartNumberSubCategory.capacitor, PartNumberSubCategory.Inductor, PartNumberSubCategory.Ferrites, PartNumberSubCategory.Crystal, PartNumberSubCategory.Resonator } },
         { PartNumberCategory.Protection, new PartNumberSubCategory[] { PartNumberSubCategory.Fuse, PartNumberSubCategory.CircuitBreaker, PartNumberSubCategory.Varistor, PartNumberSubCategory.PTCFuse } },
         { PartNumberCategory.Mechanical, new PartNumberSubCategory[] { PartNumberSubCategory.Screw, PartNumberSubCategory.Nut, PartNumberSubCategory.Standoff } },
         { PartNumberCategory.Connector, new PartNumberSubCategory[] { PartNumberSubCategory.PinHeader, PartNumberSubCategory.PinSocket, PartNumberSubCategory.TerminalBlock, PartNumberSubCategory.DSub, PartNumberSubCategory.Circular, PartNumberSubCategory.FlatFlex, PartNumberSubCategory.Audio, PartNumberSubCategory.CONN_USB, PartNumberSubCategory.BarrelJack, PartNumberSubCategory.MOLEX, PartNumberSubCategory.Programming, PartNumberSubCategory.PCIe } },
         { PartNumberCategory.Lighting, new PartNumberSubCategory[] { PartNumberSubCategory.LED, PartNumberSubCategory.Fillament, PartNumberSubCategory.Fluorescent } },
         { PartNumberCategory.Diode, new PartNumberSubCategory[] { PartNumberSubCategory.GeneralPurpose, PartNumberSubCategory.Schottky, PartNumberSubCategory.Zener, PartNumberSubCategory.TVS } },
         { PartNumberCategory.Transistor, new PartNumberSubCategory[] { PartNumberSubCategory.NPN, PartNumberSubCategory.PNP, PartNumberSubCategory.Nch, PartNumberSubCategory.Pch } },
         { PartNumberCategory.IC, new PartNumberSubCategory[] {PartNumberSubCategory.MicroController, PartNumberSubCategory.Logic, PartNumberSubCategory.OPAMP, PartNumberSubCategory.LinearReg, PartNumberSubCategory.SwitchingReg, PartNumberSubCategory.Interface, PartNumberSubCategory.AnalogLogic, PartNumberSubCategory.IC_USB, PartNumberSubCategory.ADC, PartNumberSubCategory.DAC, PartNumberSubCategory.ROM, PartNumberSubCategory.EEPROM, PartNumberSubCategory.Memory, PartNumberSubCategory.Processor, PartNumberSubCategory.Sensing, PartNumberSubCategory.CurrentSense } },
         { PartNumberCategory.Display, new PartNumberSubCategory[] {PartNumberSubCategory.LCDCharacter, PartNumberSubCategory.LCDPanel, PartNumberSubCategory.SevenSegment, PartNumberSubCategory.BarGraph, PartNumberSubCategory.OLED} },
         { PartNumberCategory.ElectroMechanical, new PartNumberSubCategory[] { PartNumberSubCategory.Relay, PartNumberSubCategory.Contactor, PartNumberSubCategory.Mic, PartNumberSubCategory.Speaker, PartNumberSubCategory.Buzzer, PartNumberSubCategory.Motor } },
         { PartNumberCategory.Switch_Input, new PartNumberSubCategory[] { PartNumberSubCategory.Tactile, PartNumberSubCategory.Toggle, PartNumberSubCategory.DIP, PartNumberSubCategory.Limit, PartNumberSubCategory.Rotary, PartNumberSubCategory.Slide, PartNumberSubCategory.Rocker, PartNumberSubCategory.RotaryEncoder, PartNumberSubCategory.Potentiometer, PartNumberSubCategory.Keypad, PartNumberSubCategory.Keylock, PartNumberSubCategory.Navigation } }
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
      private PartNumber(uint category, uint partId)
      {
         FullCategory = category;
         PartID = partId;
      }
      public PartNumber(uint category, uint subCategory, uint partId)
      {
         Category = category;
         SubCategory = subCategory;
         PartID = partId;
      }
      #endregion

      #region Methods
      public override string ToString()
      {
         return $"{Category:D2}{SubCategory:D2}-{PartID:D4}";
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
               FullCategory = typeNum;
            }
            if (uint.TryParse(spl[1], out uint partId))
            {
               PartID = partId;
            }
         }
      }

      public static PartNumber Create(PartNumberSubCategory subCategory)
      {
         return new((uint)subCategory, 0);
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
      public uint FullCategory
      {
         get => (uint)(Category * Math.Pow(10, 2)) + SubCategory;
         set
         {
            var str = $"{value:D4}";
            if (uint.TryParse(str.Substring(0, 2), out uint category))
            {
               _category = category;
            }
            if (uint.TryParse(str.Substring(2, 2), out uint subCategory))
            {
               _subCategory = subCategory;
            }
            OnPropertyChanged(nameof(Category));
            OnPropertyChanged(nameof(SubCategory));
         }
      }
      #endregion
   }
}
