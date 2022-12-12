using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Microsoft.Win32;

using MVVMLibrary;

using PartsInventory.Models.API;
using PartsInventory.Models.API.Buffer;
using PartsInventory.Models.Enums;
using PartsInventory.Models.Events;
using PartsInventory.Models.Inventory.Main;
using PartsInventory.Resources.Settings;
using PartsInventory.Utils.Messager;

namespace PartsInventory.ViewModels.Main
{
   public class InvoiceParserViewModel : ViewModel, IInvoiceParserViewModel
   {
      #region Local Props
      private readonly IMainViewModel _mainVM;
      private readonly IOptions<DirSettings> _dirSettings;
      private readonly IOptions<APISettings> _apiSettings;
      private readonly IAPIController _apiController;
      private readonly ILogger<InvoiceParserViewModel> _logger;
      private readonly IMessageService _messageService;
      private readonly IAPIBuffer _apiBuffer;

      public event EventHandler<AddInvoiceToPartsEventArgs> AddToPartsEvent = (s, e) => { };
      //private InvoiceModel? _selectedInvoice = null;
      private ObservableCollection<InvoiceModel>? _selectedInvoices = null;
      private ObservableCollection<InvoicePartModel>? _selectedParts = null;

      private InvoiceModel? _tempInvoice = null;
      private bool _tempExpanded = false;

      private bool _invoicesAdded = false;
      private bool _removeInvoiceParts = false;


      #region Commands
      public Command OpenInvoicesCmd { get; init; }
      public Command OpenInvoiceCmd { get; init; }
      public Command AddToPartsCmd { get; init; }
      public Command ClearTempInvoiceCmd { get; init; }
      public Command AddTempInvoiceCmd { get; init; }
      public Command DeleteInvoiceCmd { get; init; }
      #endregion
      #endregion

      #region Constructors
      public InvoiceParserViewModel(
         IMainViewModel mainVM,
         IOptions<DirSettings> dirSettings,
         IOptions<APISettings> apiSettings,
         IAPIController apiController,
         ILogger<InvoiceParserViewModel> logger,
         IMessageService messageService,
         IAPIBuffer apiBuffer)
      {
         _mainVM = mainVM;
         _dirSettings = dirSettings;
         _apiSettings = apiSettings;
         _apiController = apiController;
         _logger = logger;
         _messageService = messageService;
         _apiBuffer = apiBuffer;

         OpenInvoicesCmd = new(OpenInvoices);
         OpenInvoiceCmd = new(OpenInvoice);
         AddTempInvoiceCmd = new(AddTempInvoice);
         ClearTempInvoiceCmd = new(() => TempInvoice = null);
         AddToPartsCmd = new(AddToParts);
         DeleteInvoiceCmd = new(DeleteInvoice);
         AddToPartsEvent += This_AddToPartsEvent;
      }
      #endregion

      #region Methods
      private void OpenInvoices()
      {
         OpenFileDialog dialog = new()
         {
            Title = "Open Invoices",
            AddExtension = false,
            Multiselect = true,
            InitialDirectory = PathSettings.Default.PartInvoiceDir,
         };

         if (dialog.ShowDialog() == true)
         {
            if (dialog.FileNames.Length == 0)
            {
               _messageService.AddMessage("No invoice files selected.", Severity.Warning);
               return;
            }
            _messageService.AddMessage($"Sending {dialog.FileNames.Length} invoice files to API.");
            Parallel.ForEach(dialog.FileNames, async (file) =>
            {
               await ParseInvoice(file);
            });
         }
      }

      private async void OpenInvoice()
      {
         OpenFileDialog dialog = new()
         {
            Title = "Open Invoices",
            AddExtension = false,
            Multiselect = false,
            InitialDirectory = PathSettings.Default.PartInvoiceDir,
         };

         if (dialog.ShowDialog() == true)
         {
            if (dialog.FileNames.Length == 0)
            {
               _messageService.AddMessage("No invoice files selected.", Severity.Warning);
               return;
            }
            _messageService.AddMessage($"Sending {Path.GetFileName(dialog.FileName)} invoice files to API.");
            await ParseInvoice(dialog.FileName);
         }
      }

      private async Task ParseInvoicesBroke(string[] paths)
      {
         try
         {
            var filteredPaths = paths.ToList().FindAll(path =>
               int.TryParse(Path.GetFileNameWithoutExtension(path), out int orderNum)
               && !MainVM.User.Invoices.Any(inv => inv.OrderNumber == orderNum)
            );
            if (!filteredPaths.Any())
            {
               _messageService.AddMessage("All files are either already parsed or unable to be parsed.", Severity.Warning);
               return;
            }
            var newInvoices = await _apiController.ParseInvoiceFiles(filteredPaths.ToArray());
            if (newInvoices is null)
            {
               _messageService.AddMessage("Unable to parse files..", Severity.Warning);
               return;
            }
            if (!newInvoices.Any())
            {
               _messageService.AddMessage("No files to parse.", Severity.Warning);
               return;
            }
            foreach (var invoice in newInvoices)
            {
               if (await _apiController.AddModelToUser(MainVM.User.Id, invoice.Id, ModelIDSelector.INVOICES))
               {
                  MainVM.User.Invoices.Add(invoice);
                  MainVM.User.InvoiceIDs.Add(invoice.Id);
               }
               else
               {
                  _messageService.AddMessage($"Unable to add invoice {invoice.OrderNumber} to user.", Severity.Error);
               }
            }
         }
         catch (Exception e)
         {
            _messageService.AddMessage($"ERROR - {e.Message}", Severity.Error);
         }
      }

      private async Task ParseInvoices(string[] paths)
      {
         try
         {
            var filteredPaths = paths.ToList().FindAll(path =>
               int.TryParse(Path.GetFileNameWithoutExtension(path), out int orderNum)
               && !MainVM.User.Invoices.Any(inv => inv.OrderNumber == orderNum)
            );
            if (!filteredPaths.Any())
            {
               _messageService.AddMessage("All files are either already parsed or unable to be parsed.", Severity.Warning);
               return;
            }
            foreach (var path in filteredPaths)
            {
               var invoice = await _apiController.ParseInvoiceFile(path);
               if (invoice is null)
               {
                  _messageService.AddMessage("Unable to parse files..", Severity.Warning);
                  continue;
               }
               if (await _apiController.AddModelToUser(MainVM.User.Id, invoice.Id, ModelIDSelector.INVOICES))
               {
                  MainVM.User.Invoices.Add(invoice);
                  MainVM.User.InvoiceIDs.Add(invoice.Id);
               }
               else
               {
                  _messageService.AddMessage($"Unable to add invoice {invoice.OrderNumber} to profile.", Severity.Error);
               }
            }
         }
         catch (Exception e)
         {
            _messageService.AddMessage($"ERROR - {e.Message}", Severity.Error);
         }
      }

      private async Task ParseInvoice(string path)
      {
         try
         {
            var foundInvoice = int.TryParse(Path.GetFileNameWithoutExtension(path), out int orderNum) && MainVM.User.Invoices.FirstOrDefault(inv => inv.OrderNumber == orderNum) is null;
            if (!foundInvoice)
            {
               _messageService.AddMessage("All files are either already parsed or unable to be parsed.", Severity.Warning);
               return;
            }
            TempInvoice = await _apiController.ParseInvoiceFile(path);
            if (TempInvoice is null)
            {
               _messageService.AddMessage("Unable to parse file..", Severity.Warning);
               return;
            }
            _messageService.AddMessage("Successfully parsed invoice.");
         }
         catch (Exception e)
         {
            _messageService.AddMessage($"ERROR - {e.Message}", Severity.Error);
         }
      }

      private async void AddTempInvoice()
      {
         if (TempInvoice is null)
            return;
         if (MainVM.User is null)
            return;

         if (await _apiController.CreateInvoice(TempInvoice))
         {
            if (await _apiController.AddModelToUser(MainVM.User.Id, TempInvoice.Id, ModelIDSelector.INVOICES))
            {
               MainVM.User.Invoices.Add(TempInvoice);
               TempInvoice = null;
               _messageService.AddMessage("Created invoice and added to user.");
               return;
            }
            _messageService.AddMessage("Created invoice but failed to add to user.", Severity.Error);
         }
         _messageService.AddMessage("Unable to create invoice.", Severity.Error);
      }

      private async void AddToParts()
      {
         if (MainVM.User is null || SelectedInvoices is null)
            return;
         var newParts = new List<PartModel>();
         var allparts = SelectedInvoices.SelectMany(x => !x.IsAddedToParts ? x.PartModels : new());
         foreach (var part in allparts)
         {
            if (!part.AddToInventory)
               continue;
            var newPart = PartModel.ConvertInvoicePart(part);
            if (MainVM.User.Parts.FirstOrDefault(p => part.Reference == p.Reference?.ToString() || part.PartNumber == p.PartNumber) is PartModel foundPart)
            {
               newPart = PartModel.CopyTo(foundPart);
               newPart.Quantity = foundPart.Quantity + part.Quantity;
               _apiBuffer.UpdateModel(newPart);
            }
            else
            {
               newParts.Add(newPart);
            }
         }

         await MainVM.AddParts(newParts);
      }

      private void This_AddToPartsEvent(object? sender, AddInvoiceToPartsEventArgs e)
      {
         InvoicesAdded = true;
      }

      public async Task<bool> GetAllInvoicesAsync()
      {
         var invoices = await _apiController.GetInvoices(_mainVM.User.InvoiceIDs);
         if (invoices is null)
            return false;
         _mainVM.User.Invoices = new(invoices);
         return true;
      }

      private async void DeleteInvoice()
      {
         if (SelectedInvoices is null || MainVM.User is null)
            return;
         if (SelectedInvoices.Count == 0)
            return;

         if (await _apiController.DeleteInvoice(SelectedInvoices[0].Id))
         {
            MainVM.User.Invoices.Remove(SelectedInvoices[0]);
            MainVM.User.InvoiceIDs.Remove(SelectedInvoices[0].Id);
            _messageService.AddMessage($"Deleted Invoice {SelectedInvoices[0].OrderNumber}");
            SelectedInvoices.RemoveAt(0);
         }
      }

      public void UpdatePart(InvoicePartModel part)
      {
         var foundInvoice = MainVM.User.Invoices.FirstOrDefault(x => x.PartModels.Contains(part));
         if (foundInvoice != null)
         {
            _apiBuffer.UpdateModel(foundInvoice);
         }
      }
      #endregion

      #region Full Props
      public IMainViewModel MainVM => _mainVM;
      public ObservableCollection<InvoiceModel>? SelectedInvoices
      {
         get => _selectedInvoices;
         set
         {
            _selectedInvoices = value;
            OnPropertyChanged();
         }
      }

      public bool InvoicesAdded
      {
         get => _invoicesAdded;
         set
         {
            _invoicesAdded = value;
            OnPropertyChanged();
         }
      }

      public ObservableCollection<InvoicePartModel>? SelectedParts
      {
         get => _selectedParts;
         set
         {
            _selectedParts = value;
            OnPropertyChanged();
         }
      }

      public InvoiceModel? TempInvoice
      {
         get => _tempInvoice;
         set
         {
            _tempInvoice = value;
            OnPropertyChanged();
         }
      }

      public bool TempExpanded
      {
         get => _tempExpanded;
         set
         {
            _tempExpanded = value;
            OnPropertyChanged();
         }
      }

      public bool RemoveInvoiceParts
      {
         get => _removeInvoiceParts;
         set
         {
            _removeInvoiceParts = value;
            OnPropertyChanged();
         }
      }
      #endregion
   }
}
