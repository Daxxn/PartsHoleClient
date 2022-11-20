using Microsoft.Extensions.Options;
using Microsoft.Win32;
using MVVMLibrary;

using PartsInventory.Models.API;
using PartsInventory.Models.Events;
using PartsInventory.Models.Inventory;
using PartsInventory.Models.Inventory.Main;
using PartsInventory.Resources.Settings;

using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace PartsInventory.ViewModels.Main
{
   public class InvoiceParserViewModel : ViewModel, IInvoiceParserViewModel
   {
      #region Local Props
      private readonly IMainViewModel _mainVM;
      private readonly IOptions<DirSettings> _dirSettings;
      private readonly IOptions<APISettings> _apiSettings;
      private readonly IAPIController _apiController;

      public event EventHandler<AddInvoiceToPartsEventArgs> AddToPartsEvent = (s, e) => { };
      //private InvoiceModel? _selectedInvoice = null;
      private ObservableCollection<InvoiceModel>? _selectedInvoices = null;

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
         IAPIController apiController)
      {
         _mainVM = mainVM;
         _dirSettings = dirSettings;
         _apiSettings = apiSettings;
         _apiController = apiController;

         ParseTestCmd = new(ParseTest);
         OpenInvoicesCmd = new(OpenInvoices);
         OpenAllInvoicesCmd = new(OpenAllInvoices);
         //ClearInvoicesCmd = new(() =>
         //{
         //   Invoices.Clear();
         //   SelectedInvoice = null;
         //});
         ClearInvoicesCmd = new();
         AddToPartsCmd = new(AddToParts);
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
            ParseInvoices(dialog.FileNames);
         }
      }

      private void OpenAllInvoices()
      {
         List<string> paths = new(
            Directory.GetFiles(
               Path.Combine(_dirSettings.Value.PartInvoiceDir, _dirSettings.Value.DigiKeyDir)
               ));
         //paths.AddRange(Directory.GetDirectories(PathSettings.Default.MouserDir));

         ParseInvoices(paths.ToArray());

         if (MainVM.User.Invoices.Count > 0 && SelectedInvoices is null)
         {
            SelectedInvoices = new()
            {
               MainVM.User.Invoices[0]
            };
         }
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
            var invoice = _apiController.ParseFileTest(dialog.FileName);
            if (invoice is null)
               return;
            if (await _apiController.AddInvoiceToUser(MainVM.User.Id, invoice.Id))
            {
               MainVM.User.Invoices.Add(invoice);
               MainVM.User.InvoiceIDs.Add(invoice.Id);
            }
         }
      }

      private void ParseInvoices(string[] paths)
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

      private async Task<bool> AddInvoice(InvoiceModel invoice)
      {
         throw new NotImplementedException("Part of old busted calls to the API.");
         //invoice.IsAddedToParts = true;
         //if (await _apiController.UpdateInvoice(invoice))
         //{
         //   var updatedParts = await _apiController.UpdateParts(invoice.Parts);
         //   if (updatedParts?.All(x => x == true) == true)
         //   {
         //      invoice.IsAddedToParts = true;
         //      return true;
         //   }
         //}
         //return false;
      }

      private void This_AddToPartsEvent(object? sender, AddInvoiceToPartsEventArgs e)
      {
         InvoicesAdded = true;
      }

      public async Task<bool> GetAllInvoicesAsync()
      {
         var invoices = await _apiController.GetInvoices(_mainVM.User.InvoiceIDs);
         if (invoices is null) return false;
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
      #endregion
   }
}
