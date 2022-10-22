using CSVParserLibrary;
using CSVParserLibrary.Models;
using PartsInventory.Models.BOM;
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
      public string FilePath { get; set; } = "";
      #endregion

      #region Constructors
      public BOMParser() { }
      public BOMParser(string path) => FilePath = path;
      #endregion

      #region Methods
      public int GetMetadata(BOMModel model, ProjectModel proj, List<string> lines)
      {
         var index = lines.IndexOf("end");
         if (index != -1)
         {
            for (int i = index + 1; i < lines.Count; i++)
            {
               var split = RemoveQuotes(lines[i].Split(",", StringSplitOptions.TrimEntries));
               if (split.Length == 2)
               {
                  switch (split[0])
                  {
                     case "Source":
                        proj.Source = split[1];
                        break;
                     case "PartCount":
                        if (int.TryParse(split[1], out int partCount))
                        {
                           proj.PartCount = partCount;
                        }
                        break;
                     case "Date":
                        if (DateTime.TryParse(split[1], out DateTime date))
                        {
                           proj.Date = date;
                        }
                        break;
                     default:
                        break;
                  }
               }
            }
         }
         return index;
      }

      public BOMModel Parse(ProjectModel proj)
      {
         using StreamReader reader = new(FilePath);
         BOMModel model = new();
         List<string> allLines = new();
         Queue<string> lines = new();
         while (!reader.EndOfStream) allLines.Add(reader.ReadLine());
         int endIndex = GetMetadata(model, proj, allLines);
         allLines.RemoveRange(endIndex, allLines.Count - endIndex);
         lines = new(allLines);
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

      public async Task<BOMModel> ParseAsync(ProjectModel proj)
      {
         return await Task.Run(() =>
         {
            return Parse(proj);
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
