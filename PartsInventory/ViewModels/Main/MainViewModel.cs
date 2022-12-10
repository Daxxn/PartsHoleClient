using System;
using System.Threading.Tasks;
using System.Windows;

using Microsoft.Extensions.Options;
using Microsoft.Win32;

using MongoDB.Bson;

using MVVMLibrary;

using PartsInventory.Models.API;
using PartsInventory.Models.API.Buffer;
using PartsInventory.Models.Enums;
using PartsInventory.Models.Inventory;
using PartsInventory.Models.Inventory.Main;
using PartsInventory.Resources.Settings;
using PartsInventory.Utils.Messager;

namespace PartsInventory.ViewModels.Main
{
   public class MainViewModel : ViewModel, IMainViewModel
   {
      #region Local Props
      private readonly IPartNumberTemplateViewModel _partNumTempVM;
      private readonly IPassivesViewModel _passivesVM;
      private readonly IPassiveBookViewModel _bookVM;
      private readonly IOptions<DirSettings> _dirSettings;
      private readonly IOptions<APISettings> _apiSettings;
      private readonly IAPIController _apiController;
      private readonly IAPIBuffer _apiBuffer;
      private readonly IMessageService _messageService;
      private IUserModel _user;

      private int _msgCount = 0;

      #region Events
      #endregion

      #region Commands
      public Command SaveCmd { get; init; }
      public Command OpenCmd { get; init; }
      public Command GetUserTestAsyncCmd { get; init; }
      public Command SendFileTestCmd { get; init; }
      public Command AddBinTestCmd { get; init; }
      public Command GetBinTestCmd { get; init; }
      public Command AddMsgCmd { get; init; }
      #endregion
      #endregion

      #region Constructors
      public MainViewModel(
         IPartNumberTemplateViewModel partNumTempVM,
         IPassivesViewModel passivesVM,
         IPassiveBookViewModel bookVM,
         IOptions<DirSettings> dirSettings,
         IOptions<APISettings> apiSettings,
         IAPIController apiController,
         IAPIBuffer apiBuffer,
         IUserModel user,
         IMessageService messageService
         )
      {
         _partNumTempVM = partNumTempVM;
         _passivesVM = passivesVM;
         _bookVM = bookVM;
         _dirSettings = dirSettings;
         _apiSettings = apiSettings;
         _apiController = apiController;
         _apiBuffer = apiBuffer;
         _user = user;
         _messageService = messageService;

         SaveCmd = new(Save);
         OpenCmd = new(Open);
         AddMsgCmd = new(() => _messageService.AddMessage($"Message Test {_msgCount++}", Severity.Info));

         GetUserTestAsyncCmd = new Command(GetUserTestAsync);
         SendFileTestCmd = new(SendFileTest);
         AddBinTestCmd = new(AddBinTest);
         GetBinTestCmd = new(GetBinTest);

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
            await _apiController.AddModelToUser(User.Id, part.Id, ModelIDSelector.PARTS);
            User.Parts.Add(part);
         }
         return success;
      }

      public async Task RemovePart(PartModel part)
      {
         var success = await _apiController.DeletePart(part.Id);
         if (success)
         {
            if (await _apiController.RemoveModelFromUser(User.Id, part.Id, ModelIDSelector.PARTS))
            {
               User.Parts.Remove(part);
            }
         }
      }

      public async void GetUserFromAPI()
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
         // Get UserData from API:
         await UpdateUserData();
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
         // Get UserData from API:
         await UpdateUserData();
      }

      public async Task<bool> UpdateUserData()
      {
         var data = await _apiController.GetUserData(User);
         if (data is null)
            return false;
         User.Parts = data.Parts != null ? new(data.ToParts()!) : new();
         User.Invoices = data.Invoices != null ? new(data.ToInvoices()!) : new();
         User.Bins = data.Bins != null ? new(data.ToBins()!) : new();
         User.PartNumbers = data.PartNumbers != null ? new(data.PartNumbers!) : new();
         return true;
      }

      private async void AddBinTest()
      {
         var newBinID = ObjectId.GenerateNewId().ToString();
         var newBin = new BinModel()
         {
            Id = newBinID,
            Name = "Test BIN",
            Horizontal = 10,
            Vertical = 42,
            IsBook = false,
         };
         if (!await _apiController.CreateBin(newBin))
         {
            MessageBox.Show("failed to create BIN.", "Error");
         }
      }

      private async void GetBinTest()
      {
         var foundBin = await _apiController.GetBin("637a138439776a35867213dc");
      }

      public void UpdateAPI(BaseModel model)
      {
         _apiBuffer.UpdateModel(model);
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

      public IMessageService MessageService
      {
         get => _messageService;
      }
      #endregion
   }
}
