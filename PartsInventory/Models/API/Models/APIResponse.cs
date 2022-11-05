using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PartsInventory.Models.API.Models
{
   public class APIResponse<T>
   {
      #region Local Props
      public string Method { get; set; } = "GET";
      public T? Body { get; set; }
      public string? Message { get; set; }
      #endregion

      #region Constructors
      public APIResponse() { }
      public APIResponse(T body, string method, string message)
      {
         Method = method;
         Body = body;
         Message = message;
      }
      public APIResponse(string method, string message)
      {
         Method = method;
         Message = message;
      }
      public APIResponse(T body, string method)
      {
         Body = body;
         Method = method;
         Message = "Success";
      }
      #endregion

      #region Methods

      #endregion

      #region Full Props

      #endregion
   }
}
