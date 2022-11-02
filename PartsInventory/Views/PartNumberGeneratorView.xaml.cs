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

namespace PartsInventory.Views;

public partial class PartNumberGeneratorView : UserControl
{
   private readonly IPartNumberGeneratorViewModel VM;
   private readonly IPartNumberTemplateViewModel _templateVM;
   public PartNumberGeneratorView(
      IPartNumberGeneratorViewModel vm,
      IPartNumberTemplateViewModel tempVM
      )
   {
      VM = vm;
      _templateVM = tempVM;
      DataContext = VM;
      InitializeComponent();
   }

   private void OpenTemplateView_Click(object sender, RoutedEventArgs e)
   {
      var dialog = new PartNumberTemplateDialog(_templateVM);
      dialog.ShowDialog();
   }
}
