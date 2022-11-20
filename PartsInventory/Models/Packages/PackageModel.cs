using MVVMLibrary;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PartsInventory.Models.Packages
{
   public class PackageModel : Model
   {
      #region Local Props
      private string _name = "NO_PKG";
      private uint _pinCount = 0;
      private string _subLib = "UNKNOWN";
      private string? _dims = null;
      #endregion

      #region Constructors
      public PackageModel() { }
      public PackageModel(string data)
      {
         ParseData(data);
      }
      #endregion

      #region Methods
      public static PackageModel Parse(string input)
      {
         var split = input.Split(':');
         if (split.Length == 2)
         {
            if (split[1].Contains('_'))
            {
               var nameSplit = split[1].Split('_', StringSplitOptions.RemoveEmptyEntries);
               if (nameSplit.Length > 1)
               {
                  return new()
                  {
                     SubLibrary = split[0],
                     Name = nameSplit[0],
                     Dims = string.Join(' ', nameSplit[1..])
                  };
               }
            }
            else
            {
               return new()
               {
                  SubLibrary = split[0],
                  Name = split[1],
               };
            }
         }
         return new()
         {
            Name = input,
         };
      }

      public void ParseData(string data)
      {
         var split = data.Split(':');
         if (split.Length == 2)
         {
            if (split[1].Contains('_'))
            {
               var nameSplit = split[1].Split('_', StringSplitOptions.RemoveEmptyEntries);
               if (nameSplit.Length > 1)
               {
                  SubLibrary = split[0];
                  Name = nameSplit[0];
                  Dims = string.Join(' ', nameSplit[1..]);
                  return;
               }
            }
            else
            {
               SubLibrary = split[0];
               Name = split[1];
               return;
            }
         }
         Name = data;
      }

      public override string ToString()
      {
         return $"{Name} {SubLibrary} {PinCount} {Dims}";
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

      public uint PinCount
      {
         get => _pinCount;
         set
         {
            _pinCount = value;
            OnPropertyChanged();
         }
      }

      public string SubLibrary
      {
         get => _subLib;
         set
         {
            _subLib = value;
            OnPropertyChanged();
         }
      }

      public string? Dims
      {
         get => _dims;
         set
         {
            _dims = value;
            OnPropertyChanged();
         }
      }
      #endregion
   }
}
