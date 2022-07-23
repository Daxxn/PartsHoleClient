using MVVMLibrary;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PartsInventory.Models.KiCAD
{
   public class ComponentModel : Model
   {
      #region Local Props
      private string _ref = "";
      private string _value = "";
      private string? _footprint = null;
      private string? _datasheet = null;
      private LibSourceModel? _libSrc = null;
      #endregion

      #region Constructors
      public ComponentModel() { }
      #endregion

      #region Methods

      #endregion

      #region Full Props
      public string Ref
      {
         get => _ref;
         set
         {
            _ref = value;
            OnPropertyChanged();
         }
      }

      public string Value
      {
         get => _value;
         set
         {
            _value = value;
            OnPropertyChanged();
         }
      }

      public string? Footprint
      {
         get => _footprint;
         set
         {
            _footprint = value;
            OnPropertyChanged();
         }
      }

      public string? Datasheet
      {
         get => _datasheet;
         set
         {
            _datasheet = value;
            OnPropertyChanged();
         }
      }

      public string? DatasheetDisp
      {
         get
         {
            if (Datasheet == null) return null;
            if (Datasheet.StartsWith("http"))
            {
               return "Link";
            }
            return Path.GetFileNameWithoutExtension(Datasheet);
         }
      }

      public LibSourceModel? LibrarySource
      {
         get => _libSrc;
         set
         {
            _libSrc = value;
            OnPropertyChanged();
         }
      }
      #endregion
   }
}
