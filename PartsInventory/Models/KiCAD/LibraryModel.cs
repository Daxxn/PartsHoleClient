using MVVMLibrary;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PartsInventory.Models.KiCAD
{
   public class LibraryModel : Model
   {
      #region Local Props
      private string _name = "";
      private string _path = "";
      #endregion

      #region Constructors
      public LibraryModel() { }
      #endregion

      #region Methods

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

      public string Path
      {
         get => _path;
         set
         {
            _path = value;
            OnPropertyChanged();
         }
      }
      #endregion
   }
}
