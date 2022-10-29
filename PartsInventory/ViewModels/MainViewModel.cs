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
      private PartsInventoryViewModel _partsInventoryVM = new();
      private InvoiceParserViewModel _invoiceParserVM = new();
      private PackageViewModel _packageVM = new();
      private ProjectBOMViewModel _projectBOMVM = new();
      private PartNumberGeneratorViewModel _partNumGenVM = new();
      private PartNumberTemplateViewModel _partNumTempVM = new();

      #region Events
      public static EventHandler<PartsCollection> PartsChangedEvent;
      #endregion

      #region Commands
      public Command SaveCmd { get; init; }
      public Command OpenCmd { get; init; }
      #endregion
      #endregion

      #region Constructors
      private MainViewModel()
      {
         InvoiceParserVM.AddToPartsEvent += PartsInventoryVM.NewPartsEventHandler;
         SaveCmd = new(Save);
         OpenCmd = new(Open);

         PartsChangedEvent += ProjectBOMVM.PartsChanged_Main;
         PartsChangedEvent += PartsInventoryVM.PartsChanged_Main;
         PartsChangedEvent += PartNumGenVM.PartsChanged_Main;

         PartsInventoryVM.SelectedPartChanged += PartNumGenVM.SelectedPartChanged_Inv;
         PartNumTempVM.CreatePartNumber += PartNumGenVM.PartNumberCreated_PNTemp;
      }
      #endregion

      #region Methods
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
               //PartsInventoryVM.PartsCollection = parts;
               PartsChangedEvent?.Invoke(this, parts);
            }
         }
         catch (Exception e)
         {
            MessageBox.Show(e.Message);
         }
      }
      #endregion

      #region Full Props
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

      public ProjectBOMViewModel ProjectBOMVM
      {
         get => _projectBOMVM;
         set
         {
            _projectBOMVM = value;
            OnPropertyChanged();
         }
      }

      public PartNumberGeneratorViewModel PartNumGenVM
      {
         get => _partNumGenVM;
         set
         {
            _partNumGenVM = value;
            OnPropertyChanged();
         }
      }

      public PartNumberTemplateViewModel PartNumTempVM
      {
         get => _partNumTempVM;
         set
         {
            _partNumTempVM = value;
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
