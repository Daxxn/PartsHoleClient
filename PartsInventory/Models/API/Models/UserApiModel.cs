using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Bson;

using PartsInventory.Models.API.Interfaces;
using PartsInventory.Models.Inventory;
using PartsInventory.Models.Inventory.Main;

namespace PartsInventory.Models.API.Models
{
   public class UserApiModel : IApiConverter<IUserModel>
   {
      #region Local Props
      [BsonId]
      [BsonRepresentation(BsonType.ObjectId)]
      public string _id { get; set; } = null!;
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
      public IUserModel ToModel()
      {
         return new UserModel()
         {
            Id = _id,
            InvoiceIDs = Invoices,
            PartIDs = Parts,
            AuthID = AuthID,
            Email = Email,
            UserName = UserName,
         };
      }

      public static UserApiModel FromModel(IUserModel model)
      {
         return new UserApiModel()
         {
            _id = model.Id,
            UserName = model.UserName,
            Email = model.Email,
            AuthID = model.AuthID,
            Invoices = model.InvoiceIDs,
            Parts = model.PartIDs,
         };
      }
      #endregion
   }
}
