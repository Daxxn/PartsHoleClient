using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using PartsInventory.Models.API.Models;

namespace PartsInventory.Models.API.RequestModels
{
   public class UserDataRequestModel
   {
      public RequestUserModel userTest { get; set; } = null!;

      public static UserDataRequestModel Create(UserApiModel user)
      {
         return new()
         {
            userTest = new()
            {
               id = user.Id,
               authID = user.AuthID,
               email = user.Email,
               userName = user.UserName,
               parts = user.Parts,
               invoices = user.Invoices
            }
         };
      }
   }

   public class RequestUserModel
   {
      public string id { get; set; } = null!;
      public string userName { get; set; } = null!;
      public string authID { get; set; } = null!;
      public string? email { get; set; }
      public IEnumerable<string> parts { get; set; } = null!;
      public IEnumerable<string> invoices { get; set; } = null!;


   }
}
