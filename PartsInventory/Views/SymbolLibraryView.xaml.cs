using System.Windows.Controls;

using PartsInventory.ViewModels;

namespace PartsInventory.Views
{
   /// <summary>
   /// Interaction logic for SymbolLibraryView.xaml
   /// </summary>
   public partial class SymbolLibraryView : UserControl
   {
      private readonly ISymbolLibraryViewModel _vm;
      public SymbolLibraryView(ISymbolLibraryViewModel vm)
      {
         _vm = vm;
         DataContext = _vm;
         InitializeComponent();
      }
   }
}
