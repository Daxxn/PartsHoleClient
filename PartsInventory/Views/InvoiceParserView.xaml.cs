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
      private InvoiceParserViewModel VM { get; init; }
      public InvoiceParserView()
      {
         VM = MainViewModel.Instance.InvoiceParserVM;
         DataContext = VM;
         InitializeComponent();
      }
   }
}
