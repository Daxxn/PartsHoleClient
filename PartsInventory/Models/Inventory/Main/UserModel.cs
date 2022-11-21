using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

using MVVMLibrary;

using PartsInventory.Models.Extensions;
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
   public class UserModel : BaseModel, IUserModel
   {
      #region Local Props
      private string _userName = null!;
      private string? _email = null!;
      public string AuthID { get; set; } = null!;

      private ObservableCollection<PartModel> _parts = new();
      private ObservableCollection<InvoiceModel> _invoices = new();
      private ObservableCollection<BinModel> _bins = new();

      private PassivesCollection _passives = new();

      public List<string> InvoiceIDs { get; set; } = null!;
      public List<string> PartIDs { get; set; } = null!;
      public List<string> BinIDs { get; set; } = null!;
      #endregion

      #region Constructors
      public UserModel() { }
      #endregion

      #region Methods
      public void AddInvoices(IList<InvoiceModel> invoices)
      {
         foreach (var invoice in invoices)
         {
            AddInvoice(invoice);
         }
      }

      public IEnumerable<PartModel>? AddInvoice(InvoiceModel invoice)
      {
         throw new NotImplementedException("Part of old busted calls to the API.");
         //if (!invoice.IsAddedToParts)
         //{
         //   var foundParts = new List<PartModel>();
         //   foreach (var part in invoice.Parts)
         //   {
         //      if (Parts.FirstOrDefault(p => p?.EqualsPartNumber(part) == true, null) is PartModel pt)
         //      {
         //         part.Id = pt.Id;
         //         pt.Quantity += part.Quantity;
         //         foundParts.Add(pt);
         //      }
         //      else
         //      {
         //         part.Id = ObjectId.GenerateNewId().ToString();
         //         //Parts.Add(part);
         //         foundParts.Add(part);
         //      }
         //   }
         //   return foundParts;
         //}
         //return null;
      }

      public void AddUpdatedParts(IEnumerable<PartModel> parts)
      {
         Parts.MergeAdd((a, b) => a.Id == b.Id, parts);
      }

      public void SyncIDs()
      {
         PartIDs = Parts.Select(p => p.Id).ToList();
         InvoiceIDs = Invoices.Select(p => p.Id).ToList();
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

      public ObservableCollection<BinModel> Bins
      {
         get => _bins;
         set
         {
            _bins = value;
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
