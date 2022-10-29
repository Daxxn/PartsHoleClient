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
   /// Interaction logic for PartNumberTemplateDialog.xaml
   /// </summary>
   public partial class PartNumberTemplateDialog : Window
   {
      public PartNumberTemplateViewModel VM { get; set; }
      public PartNumberTemplateDialog()
      {
         VM = MainViewModel.Instance.PartNumTempVM;
         DataContext = VM;
         InitializeComponent();

         VM.CreatePartNumber += VM_CreatePartNumber;
      }

      private void VM_CreatePartNumber(object? sender, Models.PartNumber e)
      {
         Close();
      }
   }
}
