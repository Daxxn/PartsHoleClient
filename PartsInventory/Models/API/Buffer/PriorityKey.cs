using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PartsInventory.Models.API.Buffer;

public struct PriorityKey : IComparable<PriorityKey>
{
   public string ID { get; set; }
   public int Priority { get; set; }

   public PriorityKey(string id, int priority)
   {
      ID = id;
      Priority = priority;
   }
   public PriorityKey(string id)
   {
      ID = id;
      Priority = 0;
   }

   public int CompareTo(PriorityKey other) =>
      other.ID == ID ? 0 : ID.CompareTo(other.ID);
}
