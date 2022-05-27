using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace PartsInventory.Models
{
   public static class DatasheetFinder
   {
      #region Local Props
      public static List<string> DatasheetPaths { get; set; } = new();
      private static readonly List<string> punctuation = new()
      {
         "-", "_", "/"
      };
      #endregion

      #region Methods
      public static void OnStartup()
      {
         if (Directory.Exists(PathSettings.Default.DatasheetsPath))
         {
            DatasheetPaths = new(Directory.GetFiles(PathSettings.Default.DatasheetsPath));
         }
      }

      public static string? SearchDatasheets(string partNumber)
      {
         if (partNumber.Length == 0) return null;

         foreach (var datasheet in DatasheetPaths)
         {
            var name = Path.GetFileNameWithoutExtension(datasheet);
            if (name.Contains(partNumber))
            {
               return datasheet;
            }
         }
         return null;
      }

      //public static string? SearchDatasheetsTest(string partNumber)
      //{
      //   if (partNumber.Length == 0) return null;

      //   foreach (var datasheet in DatasheetPaths)
      //   {
      //      var name = Path.GetFileNameWithoutExtension(datasheet);
      //      uint matchCount = 0;
      //      for (int i = 0; i < partNumber.Length; i++)
      //      {
      //         if (i >= name.Length || i >= partNumber.Length) break;
      //         if (name[i] == partNumber[i])
      //         {
      //            matchCount++;
      //         }
      //      }
      //      if ((float)matchCount / (float)partNumber.Length > 0.5f)
      //      {
      //         return datasheet;
      //      }
      //   }
      //   return null;
      //}

      public static string? SearchDatasheetsTest(string partNumber)
      {
         var pn = SplitPunctuation(partNumber);
         Regex reg;
         if (pn.Length < partNumber.Length)
         {
            reg = new(@$"{pn}", RegexOptions.IgnoreCase);
         }
         else
         {
            reg = new(@$"{partNumber[..(partNumber.Length / 2)]}", RegexOptions.IgnoreCase);
         }
         foreach (var datasheet in DatasheetPaths)
         {
            MatchCollection matches = reg.Matches(datasheet);
            if (matches.Count > 0)
            {
               return datasheet;
            }
         }
         return null;
      }

      private static string SplitPunctuation(string partNumber)
      {
         StringBuilder output = new();
         for (int i = 0; i < partNumber.Length; i++)
         {
            if (punctuation.Contains(partNumber[i].ToString()))
            {
               return output.ToString();
            }
            output.Append(partNumber[i]);
         }
         return output.ToString();
      }
      #endregion

      #region Full Props

      #endregion
   }
}
