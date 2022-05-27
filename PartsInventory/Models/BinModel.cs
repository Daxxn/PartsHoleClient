using MVVMLibrary;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PartsInventory.Models
{
   public class BinModel : Model
   {
      #region Local Props
      public static Dictionary<string, Dictionary<int, BinModel>> Bins { get; private set; } = new Dictionary<string, Dictionary<int, BinModel>>();
      private int _horz = 0;
      private int _vert = 0;
      private string _name = "BIN";
      #endregion

      #region Constructors
      public BinModel() { }
      #endregion

      #region Methods
      public static BinModel AddBin(string name, int horz, int vert)
      {
         BinModel newBin = new();
         if (!Bins.ContainsKey(name)) Bins.Add(name, new Dictionary<int, BinModel>());
         int posHash = GetPositionHash(horz, vert);
         if (!Bins[name].ContainsKey(posHash))
         {
            newBin = new()
            {
               Name = name,
               Horizontal = horz,
               Vertical = vert,
            };
            Bins[name].Add(posHash, newBin);
         }
         else
         {
            newBin = Bins[name][posHash];
         }
         return newBin;
      }

      public bool CheckPosition(int horz, int vert)
      {
         return Horizontal == horz && Vertical == vert;
      }

      private static int GetPositionHash(int horz, int vert)
      {
         return horz << 32 & vert;
      }
      #endregion

      #region Full Props
      public string Name
      {
         get => _name;
         set
         {
            _name = value;
            OnPropertyChanged();
         }
      }

      public int Horizontal
      {
         get => _horz;
         set
         {
            _horz = value;
            OnPropertyChanged();
         }
      }

      public int Vertical
      {
         get => _vert;
         set
         {
            _vert = value;
            OnPropertyChanged();
         }
      }

      public int PositionHash
      {
         get => GetPositionHash(Horizontal, Vertical);
      }
      #endregion
   }
}
