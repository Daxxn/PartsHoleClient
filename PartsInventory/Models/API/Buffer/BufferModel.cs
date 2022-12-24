using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using PartsInventory.Models.Inventory;
using PartsInventory.Models.Inventory.Main;

namespace PartsInventory.Models.API.Buffer;

public class BufferModel
{
   #region Local Props
   public int AttemptCount { get; set; } = 0;
   public IModel Model { get; set; }
   public bool InProgress { get; set; } = false;
   #endregion

   #region Constructors
   public BufferModel(IModel model)
   {
      Model = model;
   }
   #endregion

   #region Methods
   public void Attempting()
   {
      InProgress = true;
      AttemptCount++;
   }
   #endregion

   #region Full Props

   #endregion
}
