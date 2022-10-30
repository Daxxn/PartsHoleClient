using PartsInventory.Models;
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
   /// Interaction logic for PassivesView.xaml
   /// </summary>
   public partial class PassivesView : UserControl
   {
      public PassivesViewModel VM { get; set; }
      public PassivesView()
      {
         VM = MainViewModel.Instance.PassivesVM;
         DataContext = VM;
         InitializeComponent();

         VM.NewBookEvent += NewBook_VM;
      }

      private void AddPartsList_SelectionChanged(object sender, SelectionChangedEventArgs e)
      {
         if (sender is ListView lv)
         {
            VM.SelectedAddParts = new();
            if (lv.SelectedItems.Count > 0)
            {
               foreach (var item in lv.SelectedItems)
               {
                  if (item is PartModel part)
                  {
                     VM.SelectedAddParts.Add(part);
                  }
               }
            }
         }
      }

      private void NewBook_VM(object sender, EventArgs e)
      {
         var bookDialog = new PassiveBookDialog();

         bookDialog.Show();
      }
   }
}
