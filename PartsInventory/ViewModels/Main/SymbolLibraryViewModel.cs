using System.Collections.ObjectModel;
using System.Windows;

using KiCADParserLibrary.Symbols.VModels;

using Microsoft.Extensions.Options;
using Microsoft.Win32;

using MVVMLibrary;

using PartsInventory.Models;
using PartsInventory.Resources.Settings;
using PartsInventory.Utils.Messager;

namespace PartsInventory.ViewModels.Main;

public class SymbolLibraryViewModel : ViewModel, ISymbolLibraryViewModel
{
   #region Local Props
   private readonly IAbstractFactory<ISymbolParser> _parserFactory;

   private string? _libraryDir = null;
   private ObservableCollection<Library>? _libraries = null;
   #region Commands
   public Command BrowseLibraryCmd { get; init; }
   public Command SyncLibrariesCmd { get; init; }
   #endregion
   #endregion

   #region Constructors
   public SymbolLibraryViewModel(IOptions<DirSettings> options, IMainViewModel mainVM, IAbstractFactory<ISymbolParser> parserFactory)
   {
      MainVM = mainVM;
      LibraryDirectory = options.Value.Symbols;
      _parserFactory = parserFactory;

      BrowseLibraryCmd = new(BrowseLibrary);
      SyncLibrariesCmd = new Command(SyncLibraries);
   }
   #endregion

   #region Methods
   private void BrowseLibrary()
   {
      MessageBox.Show("Not worth it right now. The folder browser is busted.\nThe idiots at microsoft never set it up to work with WPF and .NET 6.\n\nFuture Cody is going to have to build a custom browser. HAHA!!");
   }
   private void SyncLibraries()
   {
      var parser = _parserFactory.Create();
   }
   #region Events

   #endregion
   #endregion

   #region Full Props
   public IMainViewModel MainVM { get; }
   public ObservableCollection<Library>? Libraries
   {
      get => _libraries;
      set
      {
         _libraries = value;
         OnPropertyChanged();
      }
   }

   public string? LibraryDirectory
   {
      get => _libraryDir;
      set
      {
         _libraryDir = value;
         OnPropertyChanged();
      }
   }
   #endregion
}
