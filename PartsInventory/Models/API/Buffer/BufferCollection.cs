using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

using PartsInventory.Models.Inventory;

namespace PartsInventory.Models.API.Buffer;

public class BufferCollection : IEnumerable<BufferModel>
{
   #region Local Props
   private Dictionary<string, BufferModel> _buffer = new();
   public int Count => _buffer.Count;
   #endregion

   #region Constructors
   public BufferCollection() { }
   #endregion

   #region Methods
   public void AddModel(IModel model)
   {
      if (_buffer.ContainsKey(model.Id))
      {
         _buffer[model.Id] = new(model);
         return;
      }
      _buffer.Add(model.Id, new(model));
   }
   public bool RemoveModel(string id)
   {
      return _buffer.Remove(id);
   }
   public IEnumerator<BufferModel> GetEnumerator() => _buffer.Values.GetEnumerator();
   IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();
   #endregion

   #region Full Props
   #endregion
}
