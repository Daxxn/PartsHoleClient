using JsonReaderLibrary;
using MVVMLibrary;
using PartsInventory.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace PartsInventory.ViewModels
{
   public class MainViewModel : ViewModel
   {
      #region Local Props
      private static MainViewModel _instance = new();
      private DatasheetViewModel _datasheetVM = new();
      private PartsInventoryViewModel _partsInventoryVM = new();
      private InvoiceParserViewModel _invoiceParserVM = new();
      private PackageViewModel _packageVM = new();

      #region Commands
      public Command SaveCmd { get; init; }
      public Command OpenCmd { get; init; }
      #endregion
      #endregion

      #region Constructors
      private MainViewModel()
      {
         InvoiceParserVM.AddToPartsEvent += PartsInventoryVM.NewPartsEventHandler;
         PartsInventoryVM.OpenDatasheetEvent += DatasheetVM.OpenDatasheetEventHandler;
         SaveCmd = new(Save);
         OpenCmd = new(Open);
      }
      #endregion

      #region Methods
      public ViewModel GetViewModel(int index)
      {
         if (index == 0)
         {
            return PartsInventoryVM;
         }
         else if (index == 1)
         {
            return InvoiceParserVM;
         }
         else if (index == 2)
         {
            return DatasheetVM;
         }
         else throw new ArgumentException("Index is not a valid view model.");
      }

      public void Save()
      {
         try
         {
            var partsSavePath = Path.Combine(PathSettings.Default.AppDataPath, PathSettings.Default.AppDataFileName);
            if (PartsInventoryVM.PartsCollection is not null)
            {
               JsonReader.SaveJsonFile(partsSavePath, PartsInventoryVM.PartsCollection, true);
            }
         }
         catch (Exception e)
         {
            MessageBox.Show(e.Message);
         }
      }

      public void Open()
      {
         try
         {
            var partsSavePath = Path.Combine(PathSettings.Default.AppDataPath, PathSettings.Default.AppDataFileName);
            if (File.Exists(partsSavePath))
            {
               var parts = JsonReader.OpenJsonFile<PartsCollection>(partsSavePath);
               PartsInventoryVM.PartsCollection = parts;
            }
         }
         catch (Exception e)
         {
            MessageBox.Show(e.Message);
         }
      }
      #endregion

      #region Full Props
      public DatasheetViewModel DatasheetVM
      {
         get => _datasheetVM;
         set
         {
            _datasheetVM = value;
            OnPropertyChanged();
         }
      }

      public PartsInventoryViewModel PartsInventoryVM
      {
         get => _partsInventoryVM;
         set
         {
            _partsInventoryVM = value;
            OnPropertyChanged();
         }
      }

      public InvoiceParserViewModel InvoiceParserVM
      {
         get => _invoiceParserVM;
         set
         {
            _invoiceParserVM = value;
            OnPropertyChanged();
         }
      }

      public PackageViewModel PackageVM
      {
         get => _packageVM;
         set
         {
            _packageVM = value;
            OnPropertyChanged();
         }
      }

      public static MainViewModel Instance
      {
         get
         {
            if (_instance is null) _instance = new MainViewModel();
            return _instance;
         }
      }
      #endregion
   }
}
