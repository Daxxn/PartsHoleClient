using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PartsInventory.Models.API.Models
{
   public interface IApiModel<T>
   {
      string Id { get; set; }

      T Convert();
   }
}
