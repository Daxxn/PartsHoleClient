using MVVMLibrary;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PartsInventory.Models
{
   public class RevisionModel : Model
   {
      #region Local Props
      private ObservableCollection<uint> _values = new();
      #endregion

      #region Constructors
      public RevisionModel() { }
      #endregion

      #region Methods
      public static RevisionModel? Parse(string input)
      {
         if (string.IsNullOrEmpty(input)) return null;
         RevisionModel rev = new();
         if (input.StartsWith("REV"))
         {
            input = input.Replace("REV", "");
         }

         if (input.Contains('.'))
         {
            string[] spl = input.Split('.', StringSplitOptions.TrimEntries);
            foreach (var n in spl)
            {
               if (uint.TryParse(n, out uint num))
               {
                  rev.Values.Add(num);
               }
               else
               {
                  rev.Values.Add(0);
               }
            }
         }
         else
         {
            if (uint.TryParse(input, out uint num))
            {
               rev.Values.Add(num);
            }
            else
            {
               rev.Values.Add(0);
            }
         }
         return rev;
      }
      public override string ToString()
      {
         StringBuilder sb = new StringBuilder("REV");
         if (Values.Count > 0)
         {
            for (int i = 0; i < Values.Count; i++)
            {
               sb.Append(Values[i]);
               if (Values.Count - 1 >= i)
               {
                  sb.Append('.');
               }
            }
         }
         else
         {
            sb.Append("00");
         }
         return sb.ToString();
      }
      #endregion

      #region Full Props
      public ObservableCollection<uint> Values
      {
         get => _values;
         set
         {
            _values = value;
            OnPropertyChanged();
         }
      }
      #endregion
   }
}
