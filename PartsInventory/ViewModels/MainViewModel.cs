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
      private PassivesViewModel _passivesVM = new();
      private PassiveBookViewModel _bookVM = new();

      private double _monSize = Settings.Default.MonitorSize;
      private string _aspectRatio = Settings.Default.AspectRatio;

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
         PartsChangedEvent += PassivesVM.PartsChanged_Main;

         PartsInventoryVM.SelectedPartsChanged += PartNumGenVM.SelectedPartsChanged_Inv;
         PartsInventoryVM.SelectedPartsChanged += PassivesVM.SelectedPartsChanged_Inv;
         PartNumTempVM.CreatePartNumber += PartNumGenVM.PartNumberCreated_PNTemp;
         PassivesVM.NewBookEvent += BookVM.NewBook_Psv;
         BookVM.AddNewBookEvent += PassivesVM.AddNewBook_Book;
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

      public PassivesViewModel PassivesVM
      {
         get => _passivesVM;
         set
         {
            _passivesVM = value;
            OnPropertyChanged();
         }
      }

      public PassiveBookViewModel BookVM
      {
         get => _bookVM;
         set
         {
            _bookVM = value;
            OnPropertyChanged();
         }
      }

      public double MonitorSize
      {
         get => Settings.Default.MonitorSize;
         set
         {
            Settings.Default.MonitorSize = value;
            OnPropertyChanged();
         }
      }

      public string AspectRatio
      {
         get => Settings.Default.AspectRatio;
         set
         {
            Settings.Default.AspectRatio = value;
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
