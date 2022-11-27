using System.Text.Json;

namespace PartsInventory.Models.API
{
   internal class BsonNamingPolicy : JsonNamingPolicy
   {
      public override string ConvertName(string name)
      {
         return name == "Id" ? "id" : name;
      }
   }
}