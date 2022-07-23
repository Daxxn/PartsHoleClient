using Microsoft.Win32;
using PartsInventory.Models;
using PartsInventory.ViewModels;
using System;
using System.Collections.Generic;
using System.Diagnostics;
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
      private PartsInventoryViewModel VM { get; init; }
      public PartsInventoryView()
      {
         VM = MainViewModel.Instance.PartsInventoryVM;
         DataContext = VM;
         InitializeComponent();
      }

      private void Datasheet_Click(object sender, RoutedEventArgs e)
      {
         if (sender is Button btn)
         {
            if (btn.DataContext is PartModel part)
            {
               if (!string.IsNullOrEmpty(part.Datasheet))
               {
                  ProcessStartInfo proc = new()
                  {
                     FileName = @"C:\Program Files\Google\Chrome\Application\chrome.exe",
                     Arguments = part.Datasheet,
                  };
                  Process.Start(proc);
               }
            }
         }
      }

      private void BrowseDatasheet_Click(object sender, RoutedEventArgs e)
      {
         if (sender is Button btn)
         {
            if (btn.DataContext is PartModel part)
            {
               if (part is null) return;
               OpenFileDialog dialog = new()
               {
                  Title = $"Browse for {part.PartNumber} Datasheet",
                  Filter = "PDF|*.pdf|All Files|*.*",
                  InitialDirectory = PathSettings.Default.DatasheetsPath,
                  Multiselect = false,
                  DereferenceLinks = true
               };

               if (dialog.ShowDialog() == true)
               {
                  part.Datasheet = dialog.FileName;
                  VM.OpenDatasheet(sender, part);
               }
            }
         }
      }

      private void BinEdit_Click(object sender, RoutedEventArgs e)
      {
         if (sender is Button btn)
         {
            if (btn.DataContext is BinModel bin)
            {
               VM.SelectedBin = bin;
            }
         }
      }
   }
}
