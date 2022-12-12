using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PartsInventory.Models.API
{
   public class APIException
   {
      #region Local Props
      public string? Type { get; set; }
      public string? Title { get; set; }
      public int Status { get; set; }
      public string? TraceId { get; set; }
      public Dictionary<string, IEnumerable<string>>? Errors { get; set; }
      #endregion

      #region Methods

      #endregion

      #region Full Props

      #endregion
   }
}
