using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

using MVVMLibrary;

namespace PartsInventory.Models.Inventory.Main
{
   public class BaseModel : Model
   {
      #region Local Props
      private string _id = null!;
      #endregion

      #region Constructors
      public BaseModel() { }
      #endregion

      #region Methods

      #endregion

      #region Full Props
      [BsonId]
      [BsonRepresentation(BsonType.ObjectId)]
      public string Id
      {
         get => _id;
         set
         {
            _id = value;
            OnPropertyChanged();
         }
      }
      #endregion
   }
}