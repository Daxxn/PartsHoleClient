using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

using MVVMLibrary;
using PartsInventory.Models.Passives;
using PartsInventory.Models.Passives.Book;

using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PartsInventory.Models.Inventory.Main
{
   public class UserModel : BaseModel
   {
      #region Local Props
      private string _userName = null!;
      private string? _email = null!;
      public string AuthID { get; set; }

      private ObservableCollection<PartModel> _parts = new();
      private ObservableCollection<InvoiceModel> _invoices = new();

      private PassivesCollection _passives = new();

      public IEnumerable<string> InvoiceIDs { get; set; } = null!;
      public IEnumerable<string> PartIDs { get; set; } = null!;
      #endregion

      #region Constructors
      public UserModel() { }
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
                  if (Parts.FirstOrDefault(p => p?.Equals(part) == true, null) is PartModel pt)
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
      public string UserName
      {
         get => _userName;
         set
         {
            _userName = value;
            OnPropertyChanged();
         }
      }

      public string? Email
      {
         get => _email;
         set
         {
            _email = value;
            OnPropertyChanged();
         }
      }

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

      public PassivesCollection Passives
      {
         get => _passives;
         set
         {
            _passives = value;
            OnPropertyChanged();
         }
      }
      #endregion
   }
}
