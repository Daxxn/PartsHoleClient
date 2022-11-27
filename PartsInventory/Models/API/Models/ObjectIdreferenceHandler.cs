using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

using MongoDB.Bson;

namespace PartsInventory.Models.API.Models
{
   public class ObjectIdreferenceHandler : ReferenceHandler
   {
      public override ReferenceResolver CreateResolver()
      {
         return new ObjectIdReferenceResolver();
      }
   }

   public class ObjectIdReferenceResolver : ReferenceResolver
   {
      public override void AddReference(string referenceId, object value)
      {
      }

      public override string GetReference(object value, out bool alreadyExists)
      {
         throw new NotImplementedException();
      }

      public override object ResolveReference(string referenceId)
      {
         if (referenceId == "id")
         {
            return new ObjectId();
         }
         return new object();
      }
   }
}
