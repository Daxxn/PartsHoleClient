using CSVParserLibrary;
using CSVParserLibrary.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PartsInventory.Models.Parsers.DigiKey
{
   public class DigiKeyParser : ICSVParser, IParser
   {
      #region Local Props
      public string FilePath { get; set; }
      #endregion

      #region Constructors
      public DigiKeyParser(string filePath) => FilePath = filePath;
      #endregion

      #region Methods
      public IPage ParseCSV(Queue<string> data)
      {
         OrderCSVPage page = new();
         var props = RemoveQuotes(data.Dequeue().Split(',', StringSplitOptions.TrimEntries));
         page.Props = props;
         while(data.Count > 0)
         {
            string[] values = RemoveQuotes(data.Dequeue().Split(',', StringSplitOptions.TrimEntries));
            page.Lines.Add(new OrderCSVLine(values));
         }
         return page;
      }

      public Task<IPage> ParseCSVAsync(Queue<string> data)
      {
         throw new NotImplementedException();
      }

      public InvoiceModel Parse()
      {
         using StreamReader reader = new(FilePath);
         InvoiceModel model = new();
         GetOrderDetails(model, FilePath);
         Queue<string> lines = new();
         while (!reader.EndOfStream) lines.Enqueue(reader.ReadLine());
         var page = ParseCSV(lines) as OrderCSVPage;

         var subTotalLine = page.Lines[^1];
         page.Lines.RemoveAt(page.Lines.Count - 1);
         if (decimal.TryParse(subTotalLine.Data[^1].Replace("$", ""), out decimal subTot))
         {
            model.SubTotal = subTot;
         }

         foreach (var line in page.Lines)
         {
            PartModel part = new();
            for (int i = 0; i < page.Props.Count; i++)
            {
               part.ParseProp(page.Props[i], line.Data[i]);
            }
            part.Datasheet = new(DatasheetFinder.SearchDatasheetsTest(part.PartNumber));
            model.Parts.Add(part);
         }

         return model;
      }

      public Task<InvoiceModel> ParseAsync()
      {
         throw new NotImplementedException();
      }

      private static string[] RemoveQuotes(string[] values)
      {
         for (int i = 0; i < values.Length; i++)
         {
            values[i] = values[i].Replace("\"", "");
         }
         return values;
      }

      public void GetOrderDetails(InvoiceModel model, string path)
      {
         model.Path = path;
         model.SupplierType = SupplierType.DigiKey;
         var name = Path.GetFileNameWithoutExtension(path);
         if (uint.TryParse(name, out uint orderNum)) model.OrderNumber = orderNum;
      }
      #endregion

      #region Full Props

      #endregion
   }
}
