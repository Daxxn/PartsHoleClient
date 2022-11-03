using PartsInventory.Models.Inventory;
using PartsInventory.Models.Inventory.Main;
using PartsInventory.ViewModels;
using PartsInventory.ViewModels.Main;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace PartsInventory.Views;

public partial class PartSearchDialog : Window
{
   private readonly IPartSearchViewModel VM;
   public PartSearchDialog(IPartSearchViewModel vm)
   {
      VM = vm;
      DataContext = VM;
      InitializeComponent();
   }

   private void DataGrid_SelectionChanged(object sender, SelectionChangedEventArgs e)
   {
      if (sender is DataGrid dg)
      {
         VM.SelectedParts = new();
         foreach (var part in dg.SelectedItems)
         {
            if (part is PartModel pm)
            {
               VM.SelectedParts.Add(pm);
            }
         }
      }
   }
}
