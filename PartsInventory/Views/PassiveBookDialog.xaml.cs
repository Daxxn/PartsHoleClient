using PartsInventory.Models.Passives.Book;
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
using System.Windows.Shapes;

namespace PartsInventory.Views
{
   /// <summary>
   /// Interaction logic for PassiveBookDialog.xaml
   /// </summary>
   public partial class PassiveBookDialog : Window
   {
      public PassiveBookViewModel VM { get; set; }
      public PassiveBookDialog()
      {
         VM = MainViewModel.Instance.BookVM;
         DataContext = VM;
         InitializeComponent();
         //PackageSizeList.ItemsSource = SmdPackages;
      }

      private void ValueList_SelectionChanged(object sender, SelectionChangedEventArgs e)
      {

         if (sender is ListView lv)
         {
            VM.SelectedValues = new();
            foreach (var item in lv.SelectedItems)
            {
               if (item is ValueModel model)
               {
                  VM.SelectedValues.Add(model);
               }
            }
         }
      }

      private void Remove_Click(object sender, RoutedEventArgs e)
      {
         if (sender is Button btn)
         {
            if (btn.DataContext is ValueModel val)
            {
               VM.RemoveValue(val);
            }
         }
      }

      private void AddAbove_Click(object sender, RoutedEventArgs e)
      {
         if (sender is Button btn)
         {
            if (btn.DataContext is ValueModel val)
            {
               VM.AddAbove(val);
            }
         }
      }

      private void AddBelow_Click(object sender, RoutedEventArgs e)
      {
         if (sender is Button btn)
         {
            if (btn.DataContext is ValueModel val)
            {
               VM.AddBelow(val);
            }
         }
      }

      private void SmdDisplay_Loaded(object sender, RoutedEventArgs e)
      {

      }
   }
}
