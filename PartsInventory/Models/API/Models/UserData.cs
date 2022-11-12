using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using PartsInventory.Models.API;
using PartsInventory.Models.API.Interfaces;
using PartsInventory.Models.Inventory.Main;

namespace PartsInventory.Models.API.Models;

public class UserData : IUserData
{
   #region Local Props
   public IEnumerable<PartApiModel>? Parts { get; set; } = null!;
   public IEnumerable<InvoiceApiModel>? Invoices { get; set; } = null!;
   #endregion

   #region Constructors
   public UserData() { }

   public IEnumerable<PartModel>? ToParts()
   {
      return Parts?.Select(x => x.ToModel());
   }

   public IEnumerable<InvoiceModel>? ToInvoices(IEnumerable<PartModel> parts)
   {
      //var invoices = Invoices?.Select(x => x.ToModel());
      var invoices = new List<InvoiceModel>();
      if (Invoices is null)
         return null;
      foreach (var i in Invoices)
      {
         invoices.Add(i.ToModel());
      }
      foreach (var inv in invoices)
      {
         var foundParts = parts.Where(p => inv.PartIDs.Contains(p.Id));
         inv.Parts = new(foundParts);
      }
      return invoices;
   }
   #endregion

   #region Methods

   #endregion

   #region Full Props

   #endregion
}
