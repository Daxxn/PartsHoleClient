using Microsoft.Extensions.Options;

using PartsInventory.Models.API;
using PartsInventory.Resources.Settings;
using PartsInventory.ViewModels;
using PartsInventory.Views;

using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Security.Cryptography.Xml;
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

namespace PartsInventory
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
   {

      private readonly IMainViewModel VM;
      private readonly IOptions<DirSettings> _dirSettings;
      private readonly NewPartView _newPartView;
      private readonly PartNumberTemplateDialog _partNumberTempDialog;
      private readonly PartSearchDialog _searchDialog;

      public MainWindow(
         IMainViewModel mainVM,
         IOptions<DirSettings> dirSettings,
         PartsInventoryView partsView,
         InvoiceParserView invoiceView,
         ProjectBOMView bomView,
         PartNumberGeneratorView pnView,
         PassivesView passView,
         PackageView pkgView,
         BinsView binsView,
         NewPartView newPartView,
         PartNumberTemplateDialog partNumTempDialog,
         PartSearchDialog searchDialog)
      {
         VM = mainVM;
         _dirSettings = dirSettings;
         _newPartView = newPartView;
         _partNumberTempDialog = partNumTempDialog;
         _searchDialog = searchDialog;
         DataContext = VM;
         InitializeComponent();

         Loaded += MainWindow_Loaded;

         PartsViewTab.Content = partsView;
         InvoiceViewTab.Content = invoiceView;
         BomViewTab.Content = bomView;
         PartNumViewTab.Content = pnView;
         PassivesViewTab.Content = passView;
         PackagesViewTab.Content = pkgView;
         BinsViewTab.Content = binsView;
      }

      private void MainWindow_Loaded(object sender, RoutedEventArgs e)
      {
         VM.Open();
      }

      private void ElectricalCalc_Click(object sender, RoutedEventArgs e)
      {
         Process.Start(_dirSettings.Value.ElectricalCalcExe);
      }

      private void Saturn_Click(object sender, RoutedEventArgs e)
      {
         Process.Start(_dirSettings.Value.SaturnPCBExe);
      }

      private async void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
      {
         VM.Save();
         _newPartView.Close();
         _partNumberTempDialog.Close();
         _searchDialog.Close();
         await App.AppHost!.StopAsync();
      }

      private void Window_Loaded(object sender, RoutedEventArgs e)
      {
         VM.GetUserTestAsync();
      }
   }
}
