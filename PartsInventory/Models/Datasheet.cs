using MVVMLibrary;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PartsInventory.Models
{
   public class Datasheet : Model
   {
      #region Local Props
      private Uri? _filePath = null;
      #endregion

      #region Constructors
      public Datasheet() { }
      public Datasheet(string? filePath)
      {
         var opt = new UriCreationOptions()
         {
            DangerousDisablePathAndQueryCanonicalization = false
         };
         if (Uri.TryCreate(filePath, opt, out Uri? uri))
         {
            Path = uri;
         }
      }
      #endregion

      #region Methods

      #endregion

      #region Full Props
      public Uri? Path
      {
         get => _filePath;
         set
         {
            _filePath = value;
            OnPropertyChanged();
            OnPropertyChanged(nameof(Display));
            OnPropertyChanged(nameof(IsGoodPath));
         }
      }

      public string? Display
      {
         get
         {
            if (Path is null) return null;
            if (Path.IsFile) return "File";
            return "Link";
         }
      }

      public bool IsGoodPath
      {
         get
         {
            return Path is not null;
         }
      }
      #endregion
   }
}
