using MVVMLibrary;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PartsInventory.Models.KiCAD
{
   public class KicadModel : Model
   {
      #region Local Props
      private string _kicadPath = "";
      private ObservableCollection<ComponentModel> _components = new();
      private Dictionary<string, string> _textVars = new();
      private ObservableCollection<LibraryModel> _libraries = new();
      private RevisionModel? _rev = null;
      #endregion

      #region Constructors
      public KicadModel() { }
      #endregion

      #region Methods

      #endregion

      #region Full Props
      public string KiCADFilePath
      {
         get => _kicadPath;
         set
         {
            _kicadPath = value;
            OnPropertyChanged();
         }
      }

      public string Name
      {
         get
         {
            if (KiCADFilePath == null) return "";
            return Path.GetFileNameWithoutExtension(KiCADFilePath);
         }
      }

      public ObservableCollection<ComponentModel> Components
      {
         get => _components;
         set
         {
            _components = value;
            OnPropertyChanged();
         }
      }

      public Dictionary<string, string> TextVariables
      {
         get => _textVars;
         set
         {
            _textVars = value;
            OnPropertyChanged();
         }
      }

      public ObservableCollection<LibraryModel> Libraries
      {
         get => _libraries;
         set
         {
            _libraries = value;
            OnPropertyChanged();
         }
      }

      public RevisionModel? REV
      {
         get => _rev;
         set
         {
            _rev = value;
            OnPropertyChanged();
         }
      }
      #endregion
   }
}
