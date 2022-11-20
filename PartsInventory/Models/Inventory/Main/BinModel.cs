using JsonReaderLibrary;
using MVVMLibrary;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;
using System.Windows.Markup;

namespace PartsInventory.Models.Inventory.Main
{
   public class BinModel : BaseModel
   {
      #region Local Props
      private int _horz = 0;
      private int _vert = 0;
      private string _name = "BIN";
      private bool _isBook = false;
      #endregion

      #region Constructors
      public BinModel() { }
      public BinModel(string name) => Name = name;
      public BinModel(string name, bool isBook)
      {
         Name = name;
         IsBook = isBook;
      }
      public BinModel(string name, int horz, int vert)
      {
         Name = name;
         Horizontal = horz;
         Vertical = vert;
      }
      #endregion

      #region Methods
      public override string ToString()
      {
         return $"{Name} - {(IsBook ? "Book" : $"{Horizontal} - {Vertical}")}";
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

      public bool IsBook
      {
         get => _isBook;
         set
         {
            _isBook = value;
            OnPropertyChanged();
         }
      }
      #endregion
   }
}
