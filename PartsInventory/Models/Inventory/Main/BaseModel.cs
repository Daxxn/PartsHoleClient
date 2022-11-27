using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

using MVVMLibrary;

namespace PartsInventory.Models.Inventory.Main;

public class BaseModel : Model
{
   #region Local Props
   [BsonId]
   [BsonRepresentation(BsonType.ObjectId)]
   [JsonPropertyName("_id")]
   public string Id { get; set; }
   #endregion

   #region Constructors
   public BaseModel() => Id = ObjectId.GenerateNewId().ToString();
   #endregion

   #region Methods

   #endregion
}