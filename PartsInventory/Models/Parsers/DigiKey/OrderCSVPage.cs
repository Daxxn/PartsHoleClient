using CSVParserLibrary.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PartsInventory.Models.Parsers.DigiKey
{
   public class OrderCSVPage : IPage
   {
      #region Local Props
      public IList<string> Props { get; set; } = new List<string>();
      public IList<ILine> Lines { get; set; } = new List<ILine>();
      #endregion

      #region Constructors
      public OrderCSVPage() { }
      #endregion

      #region Methods
      #endregion

      #region Full Props

      #endregion
   }
}
