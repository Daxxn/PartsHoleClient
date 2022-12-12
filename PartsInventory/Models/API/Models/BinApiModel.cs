using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

using PartsInventory.Models.API.Interfaces;
using PartsInventory.Models.Inventory.Main;

namespace PartsInventory.Models.API.Models
{
   public class BinApiModel : IApiConverter<BinModel>
   {
      #region Local Props
      [BsonId]
      [BsonRepresentation(BsonType.ObjectId)]
      public string Id { get; set; } = null!;
      public string Name { get; set; } = null!;
      public int Horizontal { get; set; }
      public int Vertical { get; set; }
      public bool IsBook { get; set; }
      #endregion

      #region Constructors
      public BinApiModel() { }
      #endregion

      #region Methods
      public BinModel ToModel()
      {
         return new()
         {
            Id = Id,
            Name = Name,
            Horizontal = Horizontal,
            Vertical = Vertical,
            IsBook = IsBook
         };
      }

      public static BinApiModel FromModel(BinModel bin)
      {
         return new()
         {
            Id = bin.Id,
            Name = bin.Name,
            Horizontal = bin.Horizontal,
            Vertical = bin.Vertical,
            IsBook = bin.IsBook,
         };
      }
      #endregion

      #region Full Props

      #endregion
   }
}