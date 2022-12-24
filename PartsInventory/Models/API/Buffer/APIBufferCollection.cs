using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using PartsInventory.Models.Inventory.Main;

namespace PartsInventory.Models.API.Buffer;

//public class APIBufferCollection
//{
//   #region Local Props
//   public BufferModel? this[string id]
//   {
//      get
//      {
//         var newKey = new PriorityKey(id);
//         return Buffer.ContainsKey(newKey) ? Buffer[newKey] : null;
//      }
//      set
//      {
//         if (value is null)
//            return;
//         Buffer[new(id)] = value;
//      }
//   }
//   public BufferModel? this[PriorityKey key]
//   {
//      get
//      {
//         return Buffer.ContainsKey(key) ? Buffer[key] : null;
//      }
//      set
//      {
//         if (value is null)
//            return;
//         Buffer[key] = value;
//      }
//   }
//   private Dictionary<PriorityKey, BufferModel> Buffer { get; set; } = new();
//   #endregion

//   #region Constructors
//   public APIBufferCollection() { }
//   #endregion

//   #region Methods
//   public void AddModel(string id, BaseModel model)
//   {
//      if (Buffer.ContainsKey(new(id)))
//      {
//         Buffer.Remove(new(id));
//         Buffer.Add(new(id, Buffer.Count - 1), new(model));
//         return;
//      }
//      Buffer.Add(new(id), new(model));
//   }

//   public List<KeyValuePair<PriorityKey, BufferModel>> GetAllModels() => Buffer.OrderBy((kv) => kv.Key.Priority).ToList();

//   public BufferModel? GetModel(string id) => Buffer.TryGetValue(new(id), out BufferModel obj) ? obj : null;

//   public bool RemoveModel(string id) => Buffer.Remove(new(id));
//   #endregion

//   #region Full Props
//   public int Count => Buffer.Count;
//   #endregion
//}
