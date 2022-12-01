using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PartsInventory.Models.API.Buffer
{
   public class APIBufferCollection2
   {
      #region Local Props
      private Dictionary<string, object> Buffer { get; set; } = new();
      #endregion

      #region Constructors
      public APIBufferCollection2() { }
      #endregion

      #region Methods
      public void AddModel(string id, object model)
      {
         if (Buffer.ContainsKey(id))
         {
            Buffer[id] = model;
         }
         Buffer.Add(id, model);
      }

      public IEnumerable<object> GetAllModels() => Buffer.Values;

      public object? GetModel(string id) => Buffer.TryGetValue(id, out object? obj) ? obj : null;
      #endregion

      #region Full Props

      #endregion
   }
}
