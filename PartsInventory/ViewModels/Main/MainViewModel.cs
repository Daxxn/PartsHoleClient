using JsonReaderLibrary;

using Microsoft.Extensions.Options;
using Microsoft.Win32;

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
      private readonly IPartNumberGeneratorViewModel _partNumGenVM;
      private readonly IPartNumberTemplateViewModel _partNumTempVM;
      private readonly IPassivesViewModel _passivesVM;
      private readonly IPassiveBookViewModel _bookVM;
      private readonly IOptions<DirSettings> _dirSettings;
      private readonly IOptions<APISettings> _apiSettings;
      private readonly IAPIController _apiController;
      private IUserModel _user;

      #region Events
      #endregion

      #region Commands
      public Command SaveCmd { get; init; }
      public Command OpenCmd { get; init; }
      public Command GetUserTestAsyncCmd { get; init; }
      public Command SendFileTestCmd { get; init; }
      #endregion
      #endregion

      #region Constructors
      public MainViewModel(
         IPartNumberGeneratorViewModel partNumGenVM,
         IPartNumberTemplateViewModel partNumTempVM,
         IPassivesViewModel passivesVM,
         IPassiveBookViewModel bookVM,
         IOptions<DirSettings> dirSettings,
         IOptions<APISettings> apiSettings,
         IAPIController apiController,
         IUserModel user
         )
      {
         _partNumGenVM = partNumGenVM;
         _partNumTempVM = partNumTempVM;
         _passivesVM = passivesVM;
         _bookVM = bookVM;
         _dirSettings = dirSettings;
         _apiSettings = apiSettings;
         _apiController = apiController;
         _user = user;

         SaveCmd = new(Save);
         OpenCmd = new(Open);
         SendFileTestCmd = new(SendFileTest);

         GetUserTestAsyncCmd = new Command(GetUserTestAsync);

         //_partsInventoryVM.SelectedPartsChanged += _partNumGenVM.SelectedPartsChanged_Inv;
         //_partsInventoryVM.SelectedPartsChanged += _passivesVM.SelectedPartsChanged_Inv;
         _partNumTempVM.CreatePartNumber += _partNumGenVM.PartNumberCreated_PNTemp;
         _passivesVM.NewBookEvent += _bookVM.NewBook_Psv;
         _bookVM.AddNewBookEvent += _passivesVM.AddNewBook_Book;
      }
      #endregion

      #region Methods
      private void SendFileTest()
      {
         try
         {
            OpenFileDialog dialog = new()
            {
               CheckFileExists= true,
               DefaultExt = ".csv",
               Filter = "*.csv|CSV File|*.*|All Files"
            };

            if (dialog.ShowDialog() == true)
            {
               var newInvoice = _apiController.ParseFileTest(dialog.FileName);
               MessageBox.Show(newInvoice?.ToString() ?? "Nothing returned", "Response:");
            }
         }
         catch (Exception e)
         {
            MessageBox.Show(e.Message, "Error");
         }
      }

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

      public async Task<bool> AddPart(PartModel part)
      {
         var success = await _apiController.CreatePart(part);
         if (success)
         {
            await _apiController.AddPartToUser(User.Id, part.Id);
            User.Parts.Add(part);
         }
         return success;
      }

      public async Task RemovePart(PartModel part)
      {
         var success = await _apiController.DeletePart(part.Id);
         if (success)
         {
            User.Parts.Remove(part);
         }
      }

      public async void GetUserTestAsync()
      {
         // Try to get Auth0 token...
         // For now, just using the dev user.
         var devObjId = "636015e41a792e2787223cfa";

         var devUser = new UserModel()
         {
            Id = devObjId
         };

         // Get UserModel from API:

         var tempUser = await _apiController.GetUser(devUser);
         if (tempUser == null)
         {
            MessageBox.Show("Login Failure.", "Error");
            return;
         }
         User = tempUser;
         var data = await _apiController.GetUserData(User);
         if (data is null)
            return;
         User.Parts = data.Parts != null ? new(data.ToParts()!) : new();
         User.Invoices = data.Invoices != null ? new(data.ToInvoices()!) : new();
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

      public IUserModel User
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
