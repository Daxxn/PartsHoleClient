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
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace PartsInventory.Views
{
   /// <summary>
   /// Interaction logic for PartsInventoryView.xaml
   /// </summary>
   public partial class PartsInventoryView : UserControl
   {
      public PartsInventoryViewModel VM { get; private set; }
      public PartsInventoryView()
      {

         InitializeComponent();
      }

      private void Loaded_Event(object sender, RoutedEventArgs e)
      {
         if (DataContext is not PartsInventoryViewModel vm) throw new Exception("PartsView loaded incorrect view model.");
         VM = vm;
      }
   }
}
