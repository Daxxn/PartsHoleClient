using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PartsInventory.Models.API.Interfaces
{
   public interface IApiConverter<T>
   {
      string _id { get; set; }

      T ToModel();
      //object FromModel(T model);
   }
}
