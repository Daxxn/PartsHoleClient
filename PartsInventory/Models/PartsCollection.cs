using MVVMLibrary;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PartsInventory.Models
{
   public class PartsCollection : Model
   {
      #region Local Props
      private ObservableCollection<PartModel> _parts = new();
      private ObservableCollection<InvoiceModel> _invoices = new();
      #endregion

      #region Constructors
      public PartsCollection() { }
      #endregion

      #region Methods
      public void AddInvoices(IList<InvoiceModel> invoices)
      {
         foreach (var invoice in invoices)
         {
            if (!Invoices.Any(inv => inv.OrderNumber == invoice.OrderNumber))
            {
               Invoices.Add(invoice);
               foreach (var part in invoice.Parts)
               {
                  if (Parts.FirstOrDefault(p => p.Equals(part), null) is PartModel pt)
                  {
                     pt.Quantity += part.Quantity;
                  }
                  else
                  {
                     Parts.Add(part);
                  }
               }
            }
         }
      }
      #endregion

      #region Full Props
      public PartModel? this[PartNumber pn]
      {
         get
         {
            return Parts.FirstOrDefault((p) => p.Reference?.Equals(pn) == true);
         }
      }
      public ObservableCollection<PartModel> Parts
      {
         get => _parts;
         set
         {
            _parts = value;
            OnPropertyChanged();
         }
      }

      public ObservableCollection<InvoiceModel> Invoices
      {
         get => _invoices;
         set
         {
            _invoices = value;
            OnPropertyChanged();
         }
      }
      #endregion
   }
}
