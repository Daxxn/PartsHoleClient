using MVVMLibrary;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PartsInventory.Models.Passives
{
   public class Resistor : Passive
   {
      #region Local Props
      private string? _powerRating = null;
      #endregion

      #region Constructors
      public Resistor() { }
      #endregion

      #region Methods
      public override void ParseDesc(string? desc)
      {
         if (string.IsNullOrEmpty(desc)) return;

         var split = desc.Split(' ', StringSplitOptions.RemoveEmptyEntries);
         if (split.Length == 0) return;

         for (int i = 0; i < split.Length; i++)
         {
            if (split[i] == "OHM")
            {
               if (i - 1 < split.Length && i - 1 > 0)
               {
                  Value = ParseValue(split[i - 1]);
               }
            }
            else if (split[i].Contains('%'))
            {
               if (double.TryParse(split[i][..^1], out double tol))
               {
                  Tolerance = tol;
               }
            }
            else if (split[i].EndsWith('W'))
            {
               PowerRating = split[i];
            }
         }

         ParsePackage(split[^1]);
      }

      private double ParseValue(string value)
      {
         if (string.IsNullOrEmpty(value)) return 0;
         if (value.EndsWith('K'))
         {
            if(double.TryParse(value.AsSpan(0, value.Length - 1), out double val))
            {
               return val * 1000;
            }
         }
         else if (value.EndsWith('M'))
         {
            if (double.TryParse(value.AsSpan(0, value.Length - 1), out double val))
            {
               return val * 1000000;
            }
         }
         else if (value.EndsWith('m'))
         {
            if (double.TryParse(value.AsSpan(0, value.Length - 1), out double val))
            {
               return val * 0.001;
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
      public string? PowerRating
      {
         get => _powerRating;
         set
         {
            _powerRating = value;
            OnPropertyChanged();
         }
      }
      public new char UnitLetter => 'Ω';
      public new char TypeLetter => 'R';
      #endregion
   }
}
