using PartsInventory.Models;
using PartsInventory.ViewModels;
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

namespace PartsInventory.Views
{
   /// <summary>
   /// Interaction logic for PartSearchDialog.xaml
   /// </summary>
   public partial class PartSearchDialog : Window
   {
      public PartSearchViewModel VM { get; set; }
      public PartSearchDialog()
      {
         VM = new();
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
}
