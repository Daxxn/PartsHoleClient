using JsonReaderLibrary;
using PartsInventory.Models;
using PartsInventory.ViewModels;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;

namespace PartsInventory
{
   /// <summary>
   /// Interaction logic for App.xaml
   /// </summary>
   public partial class App : Application
   {
      protected override void OnStartup(StartupEventArgs e)
      {
         MainViewModel.Instance.Open();
         DatasheetFinder.OnStartup();
         base.OnStartup(e);
      }
      protected override void OnExit(ExitEventArgs e)
      {
         MainViewModel.Instance.Save();
         Settings.Default.Save();
         PathSettings.Default.Save();
         base.OnExit(e);
      }
   }
}
