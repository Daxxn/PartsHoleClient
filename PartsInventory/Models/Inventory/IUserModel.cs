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

      /// <summary>
      /// <para>OLD!!!</para>
      /// Checks the all invoice parts against the main parts list.
      /// </summary>
      /// <param name="invoices"></param>
      void AddInvoices(IList<InvoiceModel> invoices);
      /// <summary>
      /// Checks the invoice parts against the main parts list.
      /// </summary>
      /// <param name="invoice"></param>
      /// <returns>All parts with updated quantitys and Ids.</returns>
      IEnumerable<PartModel>? AddInvoice(InvoiceModel invoice);

      /// <summary>
      /// Updates old parts and appends new parts.
      /// </summary>
      /// <param name="parts">updated parts</param>
      void AddUpdatedParts(IEnumerable<PartModel> parts);
   }
}