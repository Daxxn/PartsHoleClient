using System.Collections.Generic;

using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

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
      public string Id { get; set; } = null!;
      public string UserName { get; set; } = null!;
      public string AuthID { get; set; } = null!;
      public string? Email { get; set; }
      public List<string> Invoices { get; set; } = null!;
      public List<string> Parts { get; set; } = null!;
      public List<string> Bins { get; set; } = null!;
      public List<string> PartNumbers { get; set; } = null!;
      #endregion

      #region Constructors
      public UserApiModel() { }
      #endregion

      #region Methods
      public IUserModel ToModel()
      {
         return new UserModel()
         {
            Id = Id,
            AuthID = AuthID,
            Email = Email,
            UserName = UserName,
            InvoiceIDs = Invoices,
            PartIDs = Parts,
            BinIDs = Bins,
            PartNumberIDs = PartNumbers,
         };
      }

      public static UserApiModel FromModel(IUserModel model)
      {
         return new UserApiModel()
         {
            Id = model.Id,
            UserName = model.UserName,
            Email = model.Email,
            AuthID = model.AuthID,
            Invoices = model.InvoiceIDs,
            Parts = model.PartIDs,
            Bins = model.BinIDs,
            PartNumbers = model.PartNumberIDs,
         };
      }
      #endregion
   }
}
