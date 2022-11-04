using JsonReaderLibrary;

using Microsoft.Extensions.Options;

using MVVMLibrary;

using PartsInventory.Models.API;
using PartsInventory.Models.Inventory;
using PartsInventory.Models.Inventory.Main;
using PartsInventory.Resources.Settings;

using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace PartsInventory.ViewModels.Main
{
   public class MainViewModel : ViewModel, IMainViewModel
   {
      #region Local Props
      private readonly IPartsInventoryViewModel _partsInventoryVM;
      private readonly IInvoiceParserViewModel _invoiceParserVM;
      private readonly IPackageViewModel _packageVM;
      private readonly IProjectBOMViewModel _projectBOMVM;
      private readonly IPartNumberGeneratorViewModel _partNumGenVM;
      private readonly IPartNumberTemplateViewModel _partNumTempVM;
      private readonly IPassivesViewModel _passivesVM;
      private readonly IPassiveBookViewModel _bookVM;
      private readonly IOptions<DirSettings> _dirSettings;
      private readonly IOptions<APISettings> _apiSettings;
      private readonly IAPIController _apiController;
      private UserModel? _user = null;

      #region Events
      public static EventHandler<UserModel> PartsChangedEvent;
      #endregion

      #region Commands
      public Command SaveCmd { get; init; }
      public Command OpenCmd { get; init; }
      public Command GetUserTestAsyncCmd { get; init; }
      #endregion
      #endregion

      #region Constructors
      public MainViewModel(
         IPartsInventoryViewModel partsVM,
         IInvoiceParserViewModel parserVM,
         IPackageViewModel packageVM,
         IProjectBOMViewModel bomVM,
         IPartNumberGeneratorViewModel partNumGenVM,
         IPartNumberTemplateViewModel partNumTempVM,
         IPassivesViewModel passivesVM,
         IPassiveBookViewModel bookVM,
         IOptions<DirSettings> dirSettings,
         IOptions<APISettings> apiSettings,
         IAPIController apiController
         )
      {
         _partsInventoryVM = partsVM;
         _invoiceParserVM = parserVM;
         _packageVM = packageVM;
         _projectBOMVM = bomVM;
         _partNumGenVM = partNumGenVM;
         _partNumTempVM = partNumTempVM;
         _passivesVM = passivesVM;
         _bookVM = bookVM;
         _dirSettings = dirSettings;
         _apiSettings = apiSettings;
         _apiController = apiController;
         SaveCmd = new(Save);
         OpenCmd = new(Open);

         GetUserTestAsyncCmd = new Command(GetUserTestAsync);

         _invoiceParserVM.AddToPartsEvent += _partsInventoryVM.NewPartsEventHandler;

         PartsChangedEvent += _projectBOMVM.PartsChanged_Main;
         PartsChangedEvent += _partsInventoryVM.PartsChanged_Main;
         PartsChangedEvent += _partNumGenVM.PartsChanged_Main;
         PartsChangedEvent += _passivesVM.PartsChanged_Main;

         _partsInventoryVM.SelectedPartsChanged += _partNumGenVM.SelectedPartsChanged_Inv;
         _partsInventoryVM.SelectedPartsChanged += _passivesVM.SelectedPartsChanged_Inv;
         _partNumTempVM.CreatePartNumber += _partNumGenVM.PartNumberCreated_PNTemp;
         _passivesVM.NewBookEvent += _bookVM.NewBook_Psv;
         _bookVM.AddNewBookEvent += _passivesVM.AddNewBook_Book;
      }
      #endregion

      #region Methods
      public void Save()
      {
         try
         {
            // Replaced with API
            //var partsSavePath = Path.Combine(_dirSettings.Value.AppDataPath, _dirSettings.Value.AppDataFileName);
            //if (_partsInventoryVM.PartsCollection is not null)
            //{
            //   JsonReader.SaveJsonFile(partsSavePath, _partsInventoryVM.PartsCollection, true);
            //}


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
            // Replaced with API
            //var partsSavePath = Path.Combine(_dirSettings.Value.AppDataPath, _dirSettings.Value.AppDataFileName);
            //if (File.Exists(partsSavePath))
            //{
            //   var parts = JsonReader.OpenJsonFile<PartsCollection>(partsSavePath);
            //   PartsChangedEvent?.Invoke(this, parts);
            //}

            // Try to get Auth0 token...
            // For now, just using the dev user.
            //var devObjId = "636015e41a792e2787223cfa";

            // Get UserModel from API:
            //APIController.Get<UserModel>($"{_apiSettings.Value.UserEndpoint}/{devObjId}")
         }
         catch (Exception e)
         {
            MessageBox.Show(e.Message);
         }
      }

      private async void GetUserTestAsync()
      {
         // Try to get Auth0 token...
         // For now, just using the dev user.
         var devObjId = "636015e41a792e2787223cfa";

         var devUser = new UserModel()
         {
            Id = devObjId
         };

         // Get UserModel from API:

         _partsInventoryVM.PartsCollection = await _apiController.GetUser(devUser);
         if (_partsInventoryVM.PartsCollection is null)
            return;
         var data = await _apiController.GetUserData(_partsInventoryVM.PartsCollection);
         if (data is null)
            return;
         _partsInventoryVM.PartsCollection.Parts = data.Parts != null ? new(data.ToParts()!) : new();
         _partsInventoryVM.PartsCollection.Invoices = data.Invoices != null ? new(data.ToInvoices()!) : new();
      }
      #endregion

      #region Full Props

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

      public UserModel? User
      {
         get => _user;
         set
         {
            _user = value;
            OnPropertyChanged();
         }
      }
      #endregion
   }
}
