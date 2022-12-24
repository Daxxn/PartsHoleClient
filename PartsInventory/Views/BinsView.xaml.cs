using System.Linq;
using System.Windows.Controls;

using PartsInventory.Models.Inventory.Main;
using PartsInventory.ViewModels;

namespace PartsInventory.Views;

public partial class BinsView : UserControl
{
   private readonly IBinsViewModel _vm;
   public BinsView(IBinsViewModel vm)
   {
      _vm = vm;
      DataContext = _vm;
      InitializeComponent();
   }

   private void DataGrid_SelectionChanged(object sender, SelectionChangedEventArgs e)
   {
      if (sender is DataGrid dg)
      {
         if (dg.SelectedItems.Count > 0)
         {
            _vm.SelectedBins = new(dg.SelectedItems.Cast<BinModel>());
         }
         else
         {
            _vm.SelectedBins = null;
         }
      }
   }
}
