using System.Collections.Generic;
using System.Collections.ObjectModel;
using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Bson;

using PartsInventory.Models.Inventory.Main;
using PartsInventory.Models.Passives;

namespace PartsInventory.Models.Inventory
{
   public interface IUserModel
   {
      [BsonId]
      [BsonRepresentation(BsonType.ObjectId)]
      string Id { get; set; }

      string UserName { get; set; }
      string? Email { get; set; }
      string AuthID { get; set; }
      IEnumerable<string> InvoiceIDs { get; set; }
      IEnumerable<string> PartIDs { get; set; }

      PartModel? this[PartNumber pn] { get; }

      ObservableCollection<InvoiceModel> Invoices { get; set; }
      ObservableCollection<PartModel> Parts { get; set; }
      PassivesCollection Passives { get; set; }

      void AddInvoices(IList<InvoiceModel> invoices);
   }
}