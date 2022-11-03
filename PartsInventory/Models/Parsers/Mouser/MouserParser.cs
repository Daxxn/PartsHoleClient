using PartsInventory.Models.Enums;
using PartsInventory.Models.Inventory;
using PartsInventory.Models.Inventory.Main;

using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace PartsInventory.Models.Parsers.Mouser
{
   public class MouserParser : IParser
   {
      #region Local Props
      public string FilePath { get; init; }
      #endregion

      #region Constructors
      public MouserParser(string filePath) => FilePath = filePath;
      #endregion

      #region Methods
      /// <summary>
      /// Need to find a better PDF file parser..
      /// </summary>
      /// <returns>Parsed Invoice</returns>
      /// <exception cref="NotImplementedException"></exception>
      public InvoiceModel Parse()
      {
         //InvoiceModel model = new();
         //GetOrderDetails(model, FilePath);

         //PdfLoadedDocument doc = new(FilePath);
         //string text = doc.Pages[0].ExtractText();


         //var lines = GetPartData(text);
         //IList<string>? partNumbers = FindPartNumbers(text);
         //if (partNumbers is null) return model;

         //foreach (var pn in partNumbers)
         //{
         //   model.Parts.Add(new() { PartNumber = pn });
         //}

         //return model;

         throw new NotImplementedException();
      }

      public Task<InvoiceModel> ParseAsync()
      {
         throw new NotImplementedException();
      }

      public void GetOrderDetails(InvoiceModel model, string path)
      {
         model.Path = path;
         var name = Path.GetFileNameWithoutExtension(path);
         var success = uint.TryParse(name, out uint orderNum);
         model.OrderNumber = orderNum;
         model.SupplierType = SupplierType.Mouser;
      }

      private IList<string>? FindPartNumbers(string rawText)
      {
         List<string> output = new();
         Regex rg = new(@"(?<=(\s)MFG Part No: )(\S)*");
         MatchCollection matches = rg.Matches(rawText);
         if (matches.Count() == 0) return null;

         foreach (Match match in matches)
         {
            output.Add(match.Value);
         }
         return output;
      }

      private IList<string>? FindExtendedPrice(string rawText)
      {
         List<string> output = new();
         Regex rg = new(@"(\S)*(\D\W)(\W)*(?=(\s)*(MFG Part No:))");
         MatchCollection matches = rg.Matches(rawText);
         if (matches.Count() == 0) return null;

         foreach (Match match in matches)
         {
            output.Add(match.Value);
         }
         return output;
      }

      private IList<string> GetPartData(string rawText)
      {
         List<string> lines = rawText.Split(new string[] { "\n" }, StringSplitOptions.TrimEntries).ToList();

         foreach (var line in lines)
         {

         }
         return lines;
      }
      #endregion

      #region Full Props

      #endregion
   }
}
