using PartsInventory.ViewModels;
using System;
using System.Collections.Generic;
using System.IO;
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
   /// Interaction logic for DatasheetView.xaml
   /// </summary>
   public partial class DatasheetView : UserControl
   {
      public DatasheetViewModel VM { get; private set; }
      public DatasheetView()
      {
         InitializeComponent();
      }

      private void Loaded_Event(object sender, RoutedEventArgs e)
      {
         if (DataContext is not DatasheetViewModel vm) throw new Exception("DatasheetView loaded incorrect view model.");
         VM = vm;
         VM.DocLoadedEvent += VM_DocLoadedEvent;
      }

      private void VM_DocLoadedEvent(object? sender, Stream doc)
      {
         DatasheetViewer.Load(doc);
      }
   }
}
