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

      public event EventHandler<AddInvoiceToPartsEventArgs> AddToPartsEvent = (s, e) => { };
      //private InvoiceModel? _selectedInvoice = null;
      private ObservableCollection<InvoiceModel>? _selectedInvoices = null;
      private ObservableCollection<DigiKeyPartModel>? _selectedParts = null;

      private bool _invoicesAdded = false;

      #region Commands
      public Command OpenInvoicesCmd { get; init; }
      public Command OpenAllInvoicesCmd { get; init; }
      public Command ParseTestCmd { get; init; }
      public Command ClearInvoicesCmd { get; init; }
      public Command AddToPartsCmd { get; init; }
      #endregion
      #endregion

      #region Constructors
      public InvoiceParserViewModel(
         IMainViewModel mainVM,
         IOptions<DirSettings> dirSettings,
         IOptions<APISettings> apiSettings,
         IAPIController apiController,
         ILogger<InvoiceParserViewModel> logger,
         IMessageService messageService)
      {
         _mainVM = mainVM;
         _dirSettings = dirSettings;
         _apiSettings = apiSettings;
         _apiController = apiController;
         _logger = logger;
         _messageService = messageService;

         ParseTestCmd = new(ParseTest);
         OpenInvoicesCmd = new(OpenInvoices);
         OpenAllInvoicesCmd = new(OpenAllInvoices);
         ClearInvoicesCmd = new();
         AddToPartsCmd = new(AddToParts);
         AddToPartsEvent += This_AddToPartsEvent;
      }
      #endregion

      #region Methods
      private async void OpenInvoices()
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
            await ParseInvoices(dialog.FileNames);
         }
      }

      private async void OpenAllInvoices()
      {
         var filePaths = Directory.GetFiles(
               Path.Combine(_dirSettings.Value.PartInvoiceDir, _dirSettings.Value.DigiKeyDir));
         await ParseInvoices(filePaths);
         //paths.AddRange(Directory.GetDirectories(PathSettings.Default.MouserDir));

         //ParseInvoicesOLD(paths.ToArray());

         //if (MainVM.User.Invoices.Count > 0 && SelectedInvoices is null)
         //{
         //   SelectedInvoices = new()
         //   {
         //      MainVM.User.Invoices[0]
         //   };
         //}
      }

      private async void ParseTest()
      {
         OpenFileDialog dialog = new()
         {
            InitialDirectory = PathSettings.Default.DigiKeyDir,
            Multiselect = false,
            CheckFileExists = true,
            Title = "Select Invoice (.csv)",
            Filter = "Invoice|*.csv|All Files|*.*"
         };
         if (dialog.ShowDialog() == true)
         {
            _messageService.AddMessage($"Starting File Parse : {Path.GetFileName(dialog.FileName)}");
            var invoice = _apiController.ParseFileTest(dialog.FileName);
            if (invoice is null)
               return;
            if (await _apiController.AddModelToUser(MainVM.User.Id, invoice.Id, ModelIDSelector.INVOICES))
            {
               MainVM.User.Invoices.Add(invoice);
               MainVM.User.InvoiceIDs.Add(invoice.Id);
            }
         }
      }

      private void ParseInvoicesOLD(string[] paths)
      {
         foreach (var path in paths)
         {
            var name = Path.GetFileNameWithoutExtension(path);
            if (int.TryParse(name, out int orderNum))
            {
               if (!MainVM.User.Invoices.Any(inv => inv.OrderNumber == orderNum))
               {
                  var ext = Path.GetExtension(path);
                  if (ext == ".csv")
                  {
                     _apiController.ParseFileTest(path);
                     //DigiKeyParser parser = new(path);

                     //MainVM.User.Invoices.Add(parser.Parse());
                     //InvoicesAdded = false;
                  }
                  // Not working. Switching over to EXCEL parser.
                  else if (ext == ".xls")
                  {
                     //MouserParser parser = new(path);
                     //MainVM.User.Invoices.Add(parser.Parse());
                     //InvoicesAdded = false;
                  }
               }
            }
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

      private void AddToParts()
      {
         throw new NotImplementedException("Part of old busted calls to the API.");
         //foreach (var invoice in MainVM.User.Invoices)
         //{
         //   if (MainVM.User.Invoices.Count == 0)
         //      return;
         //   //AddToPartsEvent?.Invoke(this, new(MainVM.User.Invoices));

         //   // Adds all the parts from the invoice to the parts inventory,
         //   var foundParts = MainVM.User.AddInvoice(invoice);
         //   if (foundParts is null)
         //      continue;
         //   invoice.PartIDs = foundParts.Select(x => x.Id);
         //   // then updates the server with the new/updated parts.
         //   if (await AddInvoice(invoice))
         //   {
         //      //MainVM.User.AddUpdatedParts(invoice.Parts);
         //      await _apiController.UpdateUser(MainVM.User);
         //   }
         //}
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

      public ObservableCollection<DigiKeyPartModel>? SelectedParts
      {
         get => _selectedParts;
         set
         {
            _selectedParts = value;
            OnPropertyChanged();
         }
      }
      #endregion
   }
}
