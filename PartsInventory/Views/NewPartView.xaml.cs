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

using PartsInventory.ViewModels;

namespace PartsInventory.Views;

public partial class NewPartView : Window
{
   private const string PropsText =
      "The parser is configured to accept new lines (\\n) as delimiters.\n"
      + "Add \"{NA}\" to skip over a Property."
      + "Tags Format: [\"<value>\",\"<value>\",...]"
      + "  Property Order:\n\n"
      + "Quantity\nPart #\nSupplier Part #\nMy Part #\nDescription\n"
      + "Unit Price\nDatasheet\nTags";
   private readonly INewPartViewModel _newPartViewModel;
   public NewPartView(INewPartViewModel newPartVeiwModel)
   {
      _newPartViewModel = newPartVeiwModel;
      DataContext = _newPartViewModel;
      InitializeComponent();
      PropsDisplay.Text = PropsText;
   }

   private async void Submit_Click(object sender, RoutedEventArgs e)
   {
      await _newPartViewModel.Submit();
   }
}
