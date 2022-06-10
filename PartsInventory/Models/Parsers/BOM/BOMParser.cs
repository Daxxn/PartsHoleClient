using CSVParserLibrary;
using CSVParserLibrary.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PartsInventory.Models.Parsers.BOM
{
   public class BOMParser : ICSVParser
   {
      #region Local Props
      public string FilePath { get; set; }
      #endregion

      #region Constructors
      public BOMParser() { }
      #endregion

      #region Methods
      public void GetOrderDetails(BOMModel model, string path)
      {

      }

      public BOMModel Parse()
      {
         using StreamReader reader = new(FilePath);
         BOMModel model = new();
         GetOrderDetails(model, FilePath);
         Queue<string> lines = new();
         while (!reader.EndOfStream) lines.Enqueue(reader.ReadLine());
         var page = ParseCSV(lines) as BOMPage;

         if (page is null) return model;
         foreach (var line in page.Lines)
         {
            BOMItemModel part = new();
            for (int i = 0; i < page.Props.Count; i++)
            {
               part.ParseProp(page.Props[i], line.Data[i]);
            }
            model.Parts.Add(part);
         }
         return model;
      }

      public async Task<BOMModel> ParseAsync()
      {
         return await Task.Run(() =>
         {
            return Parse();
         });
      }

      public IPage ParseCSV(Queue<string> data)
      {
         BOMPage page = new();
         page.Props = RemoveQuotes(data.Dequeue().Split(',', StringSplitOptions.TrimEntries));
         while (data.Count > 0)
         {
            string[] values = RemoveQuotes(data.Dequeue().Split(',', StringSplitOptions.TrimEntries));
            page.Lines.Add(new BOMLine(values));
         }
         return page;
      }

      public async Task<IPage> ParseCSVAsync(Queue<string> data)
      {
         return await Task.Run(() =>
         {
            return ParseCSV(data);
         });
      }

      private static string[] RemoveQuotes(string[] values)
      {
         for (int i = 0; i < values.Length; i++)
         {
            values[i] = values[i].Replace("\"", "");
         }
         return values;
      }
      #endregion

      #region Full Props

      #endregion
   }
}
