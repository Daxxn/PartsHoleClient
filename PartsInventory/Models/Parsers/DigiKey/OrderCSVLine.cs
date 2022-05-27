using CSVParserLibrary.Models;
using System.Collections.Generic;

namespace PartsInventory.Models.Parsers.DigiKey
{
   public class OrderCSVLine : ILine
   {
      #region Local Props
      public IList<string> Data { get; set; } = new List<string>();
      #endregion

      #region Constructors
      public OrderCSVLine() { }
      public OrderCSVLine(IList<string> data)
      {
         Data = data;
      }
      #endregion

      #region Methods

      #endregion

      #region Full Props

      #endregion
   }
}