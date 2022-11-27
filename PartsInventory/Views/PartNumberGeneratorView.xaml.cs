using System.Windows;
using System.Windows.Controls;

using PartsInventory.ViewModels;

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
