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
   /// Interaction logic for PackageView.xaml
   /// </summary>
   public partial class PackageView : UserControl
   {
      private PackageViewModel VM { get; init; }
      public PackageView()
      {
         VM = MainViewModel.Instance.PackageVM;
         DataContext = VM;
         InitializeComponent();
      }
   }
}
