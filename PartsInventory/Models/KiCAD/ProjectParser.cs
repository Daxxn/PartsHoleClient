using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Xml;

namespace PartsInventory.Models.KiCAD
{
   public static class ProjectParser
   {
      #region Local Props

      #endregion

      #region Methods
      public static KicadModel? Parse(string projPath)
      {
         XmlDocument doc = new();
         doc.Load(projPath);
         var rootNode = doc.DocumentElement;

         if (rootNode is null) return null;

         KicadModel bom = new()
         {
            KiCADFilePath = projPath
         };

         var textVars = rootNode.SelectNodes("descendant::textvar");

         if (textVars is not null)
         {
            if (textVars.Count > 0)
            {
               foreach (XmlNode txt in textVars)
               {
                  var name = txt.Attributes?.GetNamedItem("name")?.Value;
                  if (name != null)
                  {
                     bom.TextVariables.Add(name, txt.InnerText);
                  }
               }
            }
         }

         var components = rootNode.SelectNodes("descendant::comp");

         if (components is null) return null;

         foreach (XmlNode comp in components)
         {
            string? refDes = comp.Attributes?.GetNamedItem("ref")?.Value;
            if (refDes != null)
            {
               ComponentModel c = new()
               {
                  Ref = refDes,
                  LibrarySource = ParseLibSource(comp.SelectSingleNode("descendant::libsource"))
               };
               XmlNode? valueNode = comp.SelectSingleNode("descendant::value");
               XmlNode? footprintNode = comp.SelectSingleNode("descendant::footprint");
               XmlNode? datasheetNode = comp.SelectSingleNode("descendant::datasheet");

               if (valueNode != null)
               {
                  string? value = ParseValueRegex(valueNode, bom);
                  if (value != null)
                  {
                     c.Value = value;
                  }
               }

               if (footprintNode != null)
               {
                  string? fp = footprintNode.InnerText;
                  if (fp != null)
                  {
                     c.Footprint = fp;
                  }
               }

               if (datasheetNode != null)
               {
                  string? ds = datasheetNode.InnerText;
                  if (ds != null)
                  {
                     c.Datasheet = ds;
                  }
               }

               bom.Components.Add(c);
            }
         }

         var libs = rootNode.SelectNodes("descendant::library");

         if (libs != null)
         {
            foreach (XmlNode lib in libs)
            {
               string? name = lib.Attributes?.GetNamedItem("logical")?.Value;
               string? value = lib.SelectSingleNode("uri")?.InnerText;

               if (name != null)
               {
                  bom.Libraries.Add(new()
                  {
                     Name = name,
                     Path = value ?? "BADPATH"
                  });
               }
            }
         }

         var sheets = rootNode.SelectNodes("descendant::sheet");
         if (sheets != null)
         {
            bom.REV = RevisionModel.Parse(ParseREV(sheets));
         }

         return bom;
      }

      public static LibSourceModel? ParseLibSource(XmlNode? libSrcNode)
      {
         if (libSrcNode is null) return null;

         var attrs = libSrcNode.Attributes;
         string? lib = attrs?.GetNamedItem("lib")?.Value;
         if (lib != null)
         {
            string? part = attrs?.GetNamedItem("part")?.Value;
            string? desc = attrs?.GetNamedItem("description")?.Value;
            LibSourceModel libSrc = new()
            {
               Library = lib,
               Part = part ?? "NULL",
               Description = desc ?? "No Description",
            };
            return libSrc;
         }
         return null;
      }

      public static string? ParseValueRegex(XmlNode? valueNode, KicadModel bom)
      {
         if (valueNode is null) return null;

         var value = valueNode.InnerText;

         if (value.Contains("${"))
         {
            Regex reg = new("(?<=\\$\\{).+(?=\\})");
            MatchCollection matches = reg.Matches(value);

            if (matches.Count > 0)
            {
               foreach (Match m in matches)
               {
                  if (bom.TextVariables.ContainsKey(m.Value))
                  {
                     value = m.Result(bom.TextVariables[m.Value]);
                  }
               }
            }
         }

         return value;
      }

      public static string ParseREV(XmlNodeList? sheetNodes)
      {
         if (sheetNodes is null) return "REV";
         if (sheetNodes.Count == 0) return "REV";

         foreach (XmlNode node in sheetNodes)
         {
            var revNode = node.SelectSingleNode("descendant::rev");
            if (revNode != null)
            {
               if (!string.IsNullOrEmpty(revNode.InnerText)) return revNode.InnerText;
            }
         }
         return "REV";
      }
      #endregion

      #region Full Props

      #endregion
   }
}
