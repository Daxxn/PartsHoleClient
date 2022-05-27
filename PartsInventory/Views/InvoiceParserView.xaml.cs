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
   /// Interaction logic for InvoiceParserView.xaml
   /// </summary>
   public partial class InvoiceParserView : UserControl
   {
      public InvoiceParserViewModel VM { get; private set; }
      public InvoiceParserView()
      {
         InitializeComponent();
      }

      private void Loaded_Event(object sender, RoutedEventArgs e)
      {
         if (DataContext is not InvoiceParserViewModel vm) throw new Exception("InvoiceView loaded incorrect view model.");
         VM = vm;
      }
   }
}
