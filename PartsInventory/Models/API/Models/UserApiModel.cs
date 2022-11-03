using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using PartsInventory.Models.API.Interfaces;
using PartsInventory.Models.Inventory.Main;

namespace PartsInventory.Models.API.Models
{
   public class UserApiModel : IApiConverter<UserModel>
   {
      #region Local Props
      public string Id { get; set; } = null!;
      public string UserName { get; set; } = null!;
      public string AuthID { get; set; } = null!;
      public string? Email { get; set; }
      public IEnumerable<string> Invoices { get; set; } = null!;
      public IEnumerable<string> Parts { get; set; } = null!;
      #endregion

      #region Constructors
      public UserApiModel() { }
      #endregion

      #region Methods
      public UserModel Convert()
      {
         return new()
         {
            Id = new(Id),
            InvoiceIDs = Invoices,
            PartIDs = Parts,
            Email = Email,
            UserName = UserName,
         };
      }

      #endregion

      #region Full Props

      #endregion
   }
}
