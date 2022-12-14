using System;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;

using CSVParserLibrary;

using Microsoft.Win32;

using MVVMLibrary;

using PartsInventory.Models;
using PartsInventory.Models.BOM;
using PartsInventory.Models.Inventory;

namespace PartsInventory.ViewModels.Main
{
   public class ProjectBOMViewModel : ViewModel, IProjectBOMViewModel
   {
      #region Local Props
      private readonly IMainViewModel _mainViewModel;
      private readonly IAbstractFactory<ICSVParser> _parserFactory;
      private ProjectModel? _project = null;
      private int _currentTab = 0;
      private ICSVParserOptions _parserOptions;

      #region Commands
      public Command ParseProjectCmd { get; init; }
      public Command ClearProjectCmd { get; init; }
      public Command AllocateCmd { get; init; }
      #endregion
      #endregion

      #region Constructors
      public ProjectBOMViewModel(IMainViewModel mainViewModel, IAbstractFactory<ICSVParser> parserFactory)
      {
         _parserFactory = parserFactory;
         _mainViewModel = mainViewModel;
         ParseProjectCmd = new(ParseProject);
         ClearProjectCmd = new(() => Project = null);
         AllocateCmd = new(Allocate);
         _parserOptions = new CSVParserOptions()
         {
            ExclusionFunctions = new()
                  {
                     { "end-of-parts", (line) => line.Length == 1 && line[0] == "end" },
                     { "source-line", (line) =>
                     {
                        if (line.Length > 1)
                        {
                           if (line[0] == "Source")
                           {
                              Project!.Source = line[1];
                              return true;
                           }
                        }
                        return false;
                     } },
                     { "date-line", (line) =>
                     {
                        if (line.Length > 1)
                        {
                           if (line[0] == "Date")
                           {
                              if (DateTime.TryParse(line[1], out DateTime date))
                              {
                                 Project!.Date = date;
                              }
                              return true;
                           }
                        }
                        return false;
                     } },
                     { "part-count-line", (line) => line.Length == 2 || line[0] == "PartCount" },
                  },
            IgnoreCase = false,
            IgnoreLineParseErrors = true,
         };
      }
      #endregion

      #region Methods
      private void ParseProject()
      {
         CurrentTab = 0;
         OpenFileDialog dialog = new()
         {
            InitialDirectory = PathSettings.Default.BOMs,
            Multiselect = false,
            Title = "Open BOM (.csv)",
            Filter = "BOM|*.csv|All Files|*.*"
         };

         if (dialog.ShowDialog() == true)
         {
            try
            {

               Project = new(dialog.FileName);
               var parser = _parserFactory.Create();
               var result = parser.ParseFile<BOMItemModel>(dialog.FileName, _parserOptions);
               Project.BOM = new()
               {
                  Parts = new(result.Values),
               };
            }
            catch (Exception e)
            {
               MessageBox.Show(e.Message);
            }
         }
      }

      private void Allocate()
      {
         //if (Project is null)
         //   return;
         //if (Project.BOM is null)
         //   return;
         //if (User is null)
         //   return;

         //Project.Parts = new();
         //foreach (var bom in Project.BOM.Parts)
         //{
         //   if (bom.Reference == null)
         //      continue;
         //   var foundParts = User.Parts.Where((p) => p.Reference == bom.Reference).ToArray();
         //   if (foundParts.Length > 0)
         //   {
         //      foreach (var part in foundParts)
         //      {
         //         part.AllocatedQty = bom.Quantity;
         //         Project.Parts.Parts.Add(part);

         //      }
         //   }
         //}
      }

      public async void Loaded(object sender, EventArgs e)
      {
         await Task.Run(() =>
         {
            try
            {
               // Need to add auto-save and auto-load of the last BOM.
            }
            catch (Exception e)
            {
               MessageBox.Show(e.Message);
            }
         });
      }

      public async void Exit(object sender, EventArgs e)
      {
         await Task.Run(() =>
         {
            try
            {

            }
            catch (Exception e)
            {
               MessageBox.Show(e.Message);
            }
         });
      }

      //public void PartsChanged_Main(object sender, UserModel e)
      //{
      //   User = e;
      //}
      #endregion

      #region Full Props
      public IMainViewModel MainVM
      {
         get => _mainViewModel;
      }

      public ProjectModel? Project
      {
         get => _project;
         set
         {
            _project = value;
            OnPropertyChanged();
         }
      }

      public int CurrentTab
      {
         get => _currentTab;
         set
         {
            _currentTab = value;
            OnPropertyChanged();
         }
      }
      #endregion
   }
}
