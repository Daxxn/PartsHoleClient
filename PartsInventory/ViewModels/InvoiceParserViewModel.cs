using Microsoft.Win32;
using MVVMLibrary;
using PartsInventory.Models;
using PartsInventory.Models.Events;
using PartsInventory.Models.Parsers.DigiKey;
using PartsInventory.Models.Parsers.Mouser;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PartsInventory.ViewModels
{
   public class InvoiceParserViewModel : ViewModel
   {
      #region Local Props
      public event EventHandler<AddInvoiceToPartsEventArgs> AddToPartsEvent;
      private ObservableCollection<OrderModel> _invoices = new();
      private OrderModel? _selectedInvoice = null;

      #region Commands
      public Command OpenInvoicesCmd { get; init; }
      public Command OpenAllInvoicesCmd { get; init; }
      public Command ParseTestCmd { get; init; }
      public Command ClearInvoicesCmd { get; init; }
      public Command AddToPartsCmd { get; init; }
      #endregion
      #endregion

      #region Constructors
      public InvoiceParserViewModel()
      {
         ParseTestCmd = new(ParseTest);
         OpenInvoicesCmd = new(OpenInvoices);
         OpenAllInvoicesCmd = new(OpenAllInvoices);
         ClearInvoicesCmd = new(() => {
            Invoices.Clear();
            SelectedInvoice = null;
         });
         AddToPartsCmd = new(AddToParts);
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
         List<string> paths = new(Directory.GetFiles(PathSettings.Default.DigiKeyDir));
         paths.AddRange(Directory.GetDirectories(PathSettings.Default.MouserDir));

         ParseInvoices(paths.ToArray());

         if (Invoices.Count > 0 && SelectedInvoice is null)
         {
            SelectedInvoice = Invoices[0];
         }
      }
      private void ParseTest()
      {
         DigiKeyParser parser = new(@"C:\Users\Daxxn\Documents\Electrical\PartInvoices\DigiKey\67927544.csv");
         SelectedInvoice = parser.Parse();
      }

      private void ParseInvoices(string[] paths)
      {
         foreach (var path in paths)
         {
            var ext = Path.GetExtension(path);
            if (ext == ".csv")
            {
               DigiKeyParser parser = new(path);
               Invoices.Add(parser.Parse());
            }
            else if (ext == ".pdf")
            {
               MouserParser parser = new(path);
               Invoices.Add(parser.Parse());
            }
         }
      }

      private void AddToParts()
      {
         if (Invoices.Count == 0) return;

         List<PartModel> newParts = new();
         List<uint> invoiceIDs = new();
         foreach (var inv in Invoices)
         {
            invoiceIDs.Add(inv.OrderNumber);
            newParts.AddRange(inv.Parts);
         }
         AddToPartsEvent?.Invoke(this, new(invoiceIDs, newParts));
      }
      #endregion

      #region Full Props
      public ObservableCollection<OrderModel> Invoices
      {
         get => _invoices;
         set
         {
            _invoices = value;
            OnPropertyChanged();
         }
      }

      public OrderModel? SelectedInvoice
      {
         get => _selectedInvoice;
         set
         {
            _selectedInvoice = value;
            OnPropertyChanged();
         }
      }
      #endregion
   }
}
