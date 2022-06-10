using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CSVParserLibrary;
using CSVParserLibrary.Models;

namespace PartsInventory.Models.Parsers.BOM
{
   public class BOMLine : ILine
   {
      #region Local Props
      public IList<string> Data { get; set; } = new List<string>();
      #endregion

      #region Constructors
      public BOMLine() { }
      public BOMLine(IList<string> data)
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
