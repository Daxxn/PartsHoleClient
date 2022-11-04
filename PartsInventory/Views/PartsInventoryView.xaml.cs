using Microsoft.Win32;

using PartsInventory.Models.Inventory;
using PartsInventory.Models.Inventory.Main;
using PartsInventory.ViewModels;
using PartsInventory.ViewModels.Main;

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

namespace PartsInventory.Views;

public partial class PartsInventoryView : UserControl
{
   private readonly IPartsInventoryViewModel VM;
   private readonly NewPartView _newPartView;
   public PartsInventoryView(IPartsInventoryViewModel vm, NewPartView newPartView)
   {
      VM = vm;
      DataContext = VM;
      _newPartView = newPartView;
      InitializeComponent();
   }

   private void Datasheet_Click(object sender, RoutedEventArgs e)
   {
      if (sender is Button btn)
      {
         if (btn.DataContext is PartModel part)
         {
            if (part.Datasheet?.Path is null)
               return;
            if (!part.Datasheet.Path.IsFile)
            {
               ProcessStartInfo proc = new()
               {
                  //FileName = @"C:\Program Files\Google\Chrome\Application\chrome.exe",
                  FileName = "explorer.exe",
                  Arguments = part.Datasheet.Path.AbsoluteUri,
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
            if (part is null)
               return;
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
               part.Datasheet = new(dialog.FileName);
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

   private void PartsList_SelectionChanged(object sender, SelectionChangedEventArgs e)
   {
      if (sender is DataGrid dg)
      {
         PartModel[] temp = new PartModel[dg.SelectedItems.Count];
         for (int i = 0; i < dg.SelectedItems.Count; i++)
         {
            if (dg.SelectedItems[i] is PartModel part)
            {
               temp[i] = part;
            }
         }
         VM.SelectedParts = new(temp);
      }
   }

   private void AddPart_Click(object sender, RoutedEventArgs e)
   {
      _newPartView.Show();

   }
}
