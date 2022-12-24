using System.Collections.ObjectModel;

using KiCADParserLibrary.Symbols.VModels;

using MVVMLibrary;

namespace PartsInventory.ViewModels;

public interface ISymbolLibraryViewModel
{
   IMainViewModel MainVM { get; }
   ObservableCollection<Library>? Libraries { get; set; }
   Command SyncLibrariesCmd { get; init; }
}