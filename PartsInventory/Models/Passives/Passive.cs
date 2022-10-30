using MVVMLibrary;
using PartsInventory.Models.Enums;
using PartsInventory.Models.Packages;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PartsInventory.Models.Passives
{
   public abstract class Passive : Model, IPassive
   {
      #region Local Props
      protected readonly string[] PTHIndicators = new string[]
      {
         "AXIAL", "RADIAL", "PTH"
      };
      private double _value = 0;
      private double _tolerance = 1;
      private string? _desc = null;
      private uint _qty = 0;
      private string? _mfgNum = null;
      private PartNumber? _partNum = null;
      private uint _alloc = 0;
      private BinModel? _bin = null;
      private string? _packageName = null;
      private PackageType _packageType = PackageType.UNK;
      #endregion

      #region Constructors
      public Passive() { }
      #endregion

      #region Methods
      /// <summary>
      /// Converts the part from <see cref="PartModel"/> to <see cref="IPassive"/> based on the <c>TModel</c> type provided.
      /// 
      /// Including applicable properties. Also tries to parse the description to find the passives value.
      /// </summary>
      /// <typeparam name="TModel">IPassive type to instanciate.</typeparam>
      /// <param name="part">Part to convert</param>
      /// <returns>Created passive with parsed value</returns>
      public static TModel ConvertPart<TModel>(PartModel part) where TModel : IPassive, new()
      {
         var passive = new TModel
         {
            ManufacturerNumber = part.PartNumber,
            PartNumber = part.Reference,
            Description = part.Description,
            Quantity = part.Quantity,
            AllocatedQty = part.AllocatedQty,
            BinLocation = part.BinLocation,
         };
         passive.ParseDesc(part.Description);
         return passive;
      }

      public static IPassive? ConvertPartAuto(PartModel part)
      {
         if (part.Description?.StartsWith("RES") == true)
         {
            var passive = new Resistor
            {
               ManufacturerNumber = part.PartNumber,
               PartNumber = part.Reference,
               Description = part.Description,
               Quantity = part.Quantity,
            };
            passive.ParseDesc(part.Description);
            return passive;
         }
         else if (part.Description?.StartsWith("CAP") == true)
         {
            var passive = new Capacitor
            {
               ManufacturerNumber = part.PartNumber,
               PartNumber = part.Reference,
               Description = part.Description,
               Quantity = part.Quantity,
            };
            passive.ParseDesc(part.Description);
            return passive;
         }
         else if (part.Description?.Contains(" IND ") == true)
         {
            var passive = new Inductor
            {
               ManufacturerNumber = part.PartNumber,
               PartNumber = part.Reference,
               Description = part.Description,
               Quantity = part.Quantity,
            };
            passive.ParseDesc(part.Description);
            return passive;
         }
         else return null;
      }

      /// <summary>
      /// Tries to parse the value of the passive from the description string.
      /// </summary>
      /// <param name="desc">Part description</param>
      /// <returns>Parsed double or 0 if unable.</returns>
      public abstract void ParseDesc(string? desc);

      protected void ParsePackage(string str)
      {
         if (str.Length == 4)
         {
            if (str.All((c) => char.IsNumber(c)))
            {
               PackageName = str;
               PackageType = PackageType.SMD;
            }
         }
         else if (str == "SMD")
         {
            PackageType = PackageType.SMD;
         }
         else if (PTHIndicators.Contains(str))
         {
            PackageType = PackageType.PTH;
         }
      }
      #endregion

      #region Full Props
      public double Value
      {
         get => _value;
         set
         {
            _value = value;
            OnPropertyChanged();
         }
      }

      public double Tolerance
      {
         get => _tolerance;
         set
         {
            _tolerance = value;
            OnPropertyChanged();
            OnPropertyChanged(nameof(ToleranceDisplay));
         }
      }

      public string ToleranceDisplay
      {
         get => $"{Tolerance}%";
      }

      public string? Description
      {
         get => _desc;
         set
         {
            _desc = value;
            OnPropertyChanged();
         }
      }

      public uint Quantity
      {
         get => _qty;
         set
         {
            _qty = value;
            OnPropertyChanged();
         }
      }

      public uint AllocatedQty
      {
         get => _alloc;
         set
         {
            _alloc = value;
            OnPropertyChanged();
         }
      }

      public string? ManufacturerNumber
      {
         get => _mfgNum;
         set
         {
            _mfgNum = value;
            OnPropertyChanged();
         }
      }

      public PartNumber? PartNumber
      {
         get => _partNum;
         set
         {
            _partNum = value;
            OnPropertyChanged();
         }
      }
      public BinModel? BinLocation
      {
         get => _bin;
         set
         {
            _bin = value;
            OnPropertyChanged();
         }
      }

      public string? PackageName
      {
         get => _packageName;
         set
         {
            _packageName = value;
            OnPropertyChanged();
         }
      }

      public PackageType PackageType
      {
         get => _packageType;
         set
         {
            _packageType = value;
            OnPropertyChanged();
         }
      }

      public char UnitLetter => '\0';
      public char TypeLetter => '\0';
      #endregion
   }
}
