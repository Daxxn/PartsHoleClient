using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using PartsInventory.Models.API;
using PartsInventory.Models.Inventory.Main;

namespace PartsInventory.Models.API.Models;

public class UserData : IUserData
{
   #region Local Props
   public List<PartModel> Parts { get; set; } = null!;
   public List<InvoiceModel> Invoices { get; set; } = null!;
   #endregion

   #region Constructors
   public UserData() { }
   #endregion

   #region Methods

   #endregion

   #region Full Props

   #endregion
}
