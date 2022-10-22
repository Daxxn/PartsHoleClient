using MVVMLibrary;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PartsInventory.Models.KiCAD;
using PartsInventory.Models.BOM;
using System.IO;

namespace PartsInventory.Models
{
   public class ProjectModel : Model
   {
      #region Local Props
      private string _name = "New Project";
      private string? _rev = null;
      private BOMModel _bom = new();
      private string? _source = null;
      private int? _partCount = null;
      private DateTime? _date = null;
      #endregion

      #region Constructors
      public ProjectModel(string filePath)
      {
         Source = filePath;
         ParseFileName(filePath);
      }
      #endregion

      #region Methods
      public void ParseFileName(string input)
      {
         if (!File.Exists(input)) throw new FileNotFoundException(input);

         string fileName = Path.GetFileNameWithoutExtension(input).Replace("-BOM", "");

         var split = fileName.Split('-', StringSplitOptions.TrimEntries).ToList();
         if (split.Count > 1)
         {
            for (int i = 0; i < split.Count; i++)
            {
               if (split[i].Contains("REV"))
               {
                  REV = split[i];
                  split.RemoveAt(i);
                  Name = string.Join('-', split);
                  return;
               }
            }
         }
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

      public string? REV
      {
         get => _rev;
         set
         {
            _rev = value;
            OnPropertyChanged();
         }
      }

      public BOMModel BOM
      {
         get => _bom;
         set
         {
            _bom = value;
            OnPropertyChanged();
         }
      }

      public string? Source
      {
         get => _source;
         set
         {
            _source = value;
            OnPropertyChanged();
         }
      }

      public int? PartCount
      {
         get => _partCount;
         set
         {
            _partCount = value;
            OnPropertyChanged();
         }
      }

      public DateTime? Date
      {
         get => _date;
         set
         {
            _date = value;
            OnPropertyChanged();
         }
      }
      #endregion
   }
}
