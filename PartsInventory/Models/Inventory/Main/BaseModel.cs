using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using MongoDB.Bson;

using MVVMLibrary;

namespace PartsInventory.Models.Inventory.Main
{
   public class BaseModel : Model
   {
      #region Local Props
      private ObjectId _id = new();
      #endregion

      #region Constructors
      public BaseModel() { }
      #endregion

      #region Methods

      #endregion

      #region Full Props
      public ObjectId Id
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