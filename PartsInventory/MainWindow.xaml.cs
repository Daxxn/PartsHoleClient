﻿using PartsInventory.ViewModels;
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
      private MainViewModel VM { get; init; }
      public MainWindow()
      {
         VM = MainViewModel.Instance;
         DataContext = VM;
         InitializeComponent();
      }

      private void ElectricalCalc_Click(object sender, RoutedEventArgs e)
      {
         Process.Start(PathSettings.Default.ElectricalCalcExe);
      }

      private void Saturn_Click(object sender, RoutedEventArgs e)
      {
         Process.Start(PathSettings.Default.SaturnPCBExe);
      }
   }
}
