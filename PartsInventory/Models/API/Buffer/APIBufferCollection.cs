using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using PartsInventory.Models.Inventory.Main;

namespace PartsInventory.Models.API.Buffer
{
   public class APIBufferCollection
   {
      #region Local Props
      private Dictionary<PriorityKey, BaseModel> Buffer { get; set; } = new();
      #endregion

      #region Constructors
      public APIBufferCollection() { }
      #endregion

      #region Methods
      public void AddModel(string id, BaseModel model)
      {
         if (Buffer.ContainsKey(new(id)))
         {
            Buffer.Remove(new(id));
            Buffer.Add(new(id, Buffer.Count - 1), model);
            return;
         }
         Buffer.Add(new(id), model);
      }

      public List<KeyValuePair<PriorityKey, BaseModel>> GetAllModels() => Buffer.OrderBy((kv) => kv.Key.Priority).ToList();

      public object? GetModel(string id) => Buffer.TryGetValue(new(id), out BaseModel? obj) ? obj : null;

      public bool RemoveModel(string id) => Buffer.Remove(new(id));
      #endregion

      #region Full Props

      #endregion
   }
}
