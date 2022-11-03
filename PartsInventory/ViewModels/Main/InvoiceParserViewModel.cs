using Microsoft.Win32;
using MVVMLibrary;
using PartsInventory.Models.Events;
using PartsInventory.Models.Inventory;
using PartsInventory.Models.Inventory.Main;
using PartsInventory.Models.Parsers.DigiKey;
using PartsInventory.Models.Parsers.Mouser;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PartsInventory.ViewModels.Main
{
   public class InvoiceParserViewModel : ViewModel, IInvoiceParserViewModel
   {
      #region Local Props
      public event EventHandler<AddInvoiceToPartsEventArgs> AddToPartsEvent = (s, e) => { };
      private ObservableCollection<InvoiceModel> _invoices = new();
      private InvoiceModel? _selectedInvoice = null;

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
      public InvoiceParserViewModel()
      {
         ParseTestCmd = new(ParseTest);
         OpenInvoicesCmd = new(OpenInvoices);
         OpenAllInvoicesCmd = new(OpenAllInvoices);
         ClearInvoicesCmd = new(() =>
         {
            Invoices.Clear();
            SelectedInvoice = null;
         });
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
         List<string> paths = new(Directory.GetFiles(Path.Combine(PathSettings.Default.PartInvoiceDir, PathSettings.Default.DigiKeyDir)));
         //paths.AddRange(Directory.GetDirectories(PathSettings.Default.MouserDir));

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
            var name = Path.GetFileNameWithoutExtension(path);
            if (int.TryParse(name, out int orderNum))
            {
               if (!Invoices.Any(inv => inv.OrderNumber == orderNum))
               {
                  var ext = Path.GetExtension(path);
                  if (ext == ".csv")
                  {
                     DigiKeyParser parser = new(path);
                     Invoices.Add(parser.Parse());
                     InvoicesAdded = false;
                  }
                  // Not working. Switching over to EXCEL parser.
                  else if (ext == ".pdf")
                  {
                     MouserParser parser = new(path);
                     Invoices.Add(parser.Parse());
                     InvoicesAdded = false;
                  }
               }
            }
         }
      }

      private void AddToParts()
      {
         if (Invoices.Count == 0)
            return;
         AddToPartsEvent?.Invoke(this, new(Invoices));
      }

      private void This_AddToPartsEvent(object? sender, AddInvoiceToPartsEventArgs e)
      {
         InvoicesAdded = true;
      }
      #endregion

      #region Full Props
      public ObservableCollection<InvoiceModel> Invoices
      {
         get => _invoices;
         set
         {
            _invoices = value;
            OnPropertyChanged();
         }
      }

      public InvoiceModel? SelectedInvoice
      {
         get => _selectedInvoice;
         set
         {
            _selectedInvoice = value;
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
