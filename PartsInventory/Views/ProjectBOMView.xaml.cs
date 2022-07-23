using PartsInventory.Models.KiCAD;
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
   /// Interaction logic for ProjectBOMView.xaml
   /// </summary>
   public partial class ProjectBOMView : UserControl
   {
      private ProjectBOMViewModel VM { get; set; }
      public ProjectBOMView()
      {
         VM = MainViewModel.Instance.ProjectBOMVM;
         DataContext = VM;
         InitializeComponent();
      }

      private void Datasheet_Click(object sender, RoutedEventArgs e)
      {
         if (sender is Button btn)
         {
            if (btn.DataContext is ComponentModel comp)
            {
               if (!string.IsNullOrEmpty(comp.Datasheet))
               {
                  ProcessStartInfo proc = new()
                  {
                     FileName = @"C:\Program Files\Google\Chrome\Application\chrome.exe",
                     Arguments = comp.Datasheet,
                  };
                  Process.Start(proc);
               }
            }
         }
      }
   }
}
