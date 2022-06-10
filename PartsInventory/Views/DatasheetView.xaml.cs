﻿using PartsInventory.ViewModels;
using System;
using System.Collections.Generic;
using System.IO;
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
   /// Interaction logic for DatasheetView.xaml
   /// </summary>
   public partial class DatasheetView : UserControl
   {
      private DatasheetViewModel VM { get; init; }
      public DatasheetView()
      {
         VM = MainViewModel.Instance.DatasheetVM;
         DataContext = VM;
         InitializeComponent();
         VM.DocLoadedEvent += VM_DocLoadedEvent;
      }

      private void VM_DocLoadedEvent(object? sender, Stream doc)
      {
         DatasheetViewer.Load(doc);
      }
   }
}
