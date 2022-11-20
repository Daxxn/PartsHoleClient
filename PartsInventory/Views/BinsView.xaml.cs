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

using PartsInventory.ViewModels.Main;

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
}
