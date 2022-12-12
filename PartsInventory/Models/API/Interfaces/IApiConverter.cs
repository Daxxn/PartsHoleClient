using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PartsInventory.Models.API.Interfaces
{
   public interface IApiConverter<T>
   {
      string Id { get; set; }

      T ToModel();
      //object FromModel(T model);
   }
}
