using MVVMLibrary;
using PartsInventory.Models.Passives;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace PartsInventory.Models
{
   public class Capacitor : Passive
   {
      #region Local Props
      private double _voltageRating = 0;
      #endregion

      #region Constructors
      public Capacitor() { }
      #endregion

      #region Methods
      public override void ParseDesc(string? desc)
      {
         if (string.IsNullOrEmpty(desc)) return;

         var split = desc.Split(' ', StringSplitOptions.RemoveEmptyEntries);
         if (split.Length == 0) return;

         for (int i = 0; i < split.Length; i++)
         {
            if (split[i].EndsWith('F'))
            {
               Value = ParseValue(split[i][..^1]);
            }
            else if (split[i].EndsWith('V'))
            {
               if (double.TryParse(split[i][..^1], out double volt))
               {
                  VoltageRating = volt;
               }
            }
            else if (split[i].Contains('%'))
            {
               if (double.TryParse(split[i][..^1], out double tr))
               {
                  Tolerance = tr;
               }
            }
         }

         ParsePackage(split[^1]);
      }

      private double ParseValue(string value)
      {
         if (string.IsNullOrEmpty(value)) return 0;
         value = value.ToLower();
         if (value.EndsWith('m'))
         {
            if (double.TryParse(value.AsSpan(0, value.Length - 1), out double val))
            {
               return val * 0.001;
            }
         }
         else if (value.EndsWith('u'))
         {
            if (double.TryParse(value.AsSpan(0, value.Length - 1), out double val))
            {
               return val * 0.000001;
            }
         }
         else if (value.EndsWith('n'))
         {
            if (double.TryParse(value.AsSpan(0, value.Length - 1), out double val))
            {
               return val * 0.000000001;
            }
         }
         else if (value.EndsWith('p'))
         {
            if (double.TryParse(value.AsSpan(0, value.Length - 1), out double val))
            {
               return val * 0.000000000001;
            }
         }
         else
         {
            if (double.TryParse(value, out double val))
            {
               return val;
            }
         }
         return 0;
      }
      #endregion

      #region Full Props
      public double VoltageRating
      {
         get => _voltageRating;
         set
         {
            _voltageRating = value;
            OnPropertyChanged();
         }
      }
      public new char UnitLetter => 'F';
      public new char TypeLetter => 'C';
      #endregion
   }
}
