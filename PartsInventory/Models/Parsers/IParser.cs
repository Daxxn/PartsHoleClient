using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PartsInventory.Models.Parsers
{
   public interface IParser
   {
      OrderModel Parse();
      Task<OrderModel> ParseAsync();

      void GetOrderDetails(OrderModel model, string path);
   }
}
