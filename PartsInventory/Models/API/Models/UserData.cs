using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Navigation;

using PartsInventory.Models.API;
using PartsInventory.Models.API.Interfaces;
using PartsInventory.Models.Inventory.Main;

namespace PartsInventory.Models.API.Models;

public class UserData : IUserData
{
   #region Local Props
   public IEnumerable<PartApiModel>? Parts { get; set; } = null!;
   public IEnumerable<InvoiceApiModel>? Invoices { get; set; } = null!;
   public IEnumerable<BinApiModel>? Bins { get; set; } = null!;
   #endregion

   #region Constructors
   public UserData() { }

   public IEnumerable<PartModel>? ToParts()
   {
      var parts = Parts?.Select(x => x.ToModel()).ToList();
      if (parts != null)
      {
         if (Bins?.Any() == true)
         {
            foreach (var part in parts)
            {
               if (part.BinLocationId != null)
               {
                  part.BinLocation = Bins.FirstOrDefault(x => x._id == part.BinLocationId)?.ToModel() ?? new();
               }
            }
         }
      }
      return parts;
   }

   public IEnumerable<InvoiceModel>? ToInvoices()
   {
      return Invoices?.Select(x => x.ToModel());
   }

   public IEnumerable<BinModel>? ToBins()
   {
      return Bins?.Select(x => x.ToModel());
   }
   #endregion

   #region Methods

   #endregion

   #region Full Props

   #endregion
}
