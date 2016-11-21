using SharpKml.Dom;
using SharpKml.Engine;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KmlEditorLibrary
{
    public class KmlFileHelper
    {
        public static KmlFile OpenFile(String filePath)
        {
            string fileExtension = System.IO.Path.GetExtension(filePath);
            using (FileStream fileStream = File.OpenRead(filePath))
            {
                if (fileExtension.Equals(".kmz", StringComparison.OrdinalIgnoreCase))
                {
                    KmzFile kmzFile = KmzFile.Open(fileStream);
                    string kmlFileString = kmzFile.ReadKml();
                    using (StringReader stringReader = new StringReader(kmlFileString))
                    {
                        return KmlFile.Load(stringReader);
                    }
                }
                else
                {
                    return KmlFile.Load(fileStream);
                }
            }
        }

        public static void SaveFile(KmlFile kmlFile, String filePath)
        {
            string fileExtension = System.IO.Path.GetExtension(filePath);
            using (FileStream fileStream = File.OpenWrite(filePath))
            {
                if (fileExtension.Equals(".kmz", StringComparison.OrdinalIgnoreCase))
                {
                    KmzFile kmzFile = KmzFile.Create(kmlFile);
                    kmzFile.Save(fileStream);
                }
                else
                {
                    kmlFile.Save(fileStream);
                }
            }
        }

        public static void splitKmlIntoFolders(KmlFile kmlFile, String outputPath, int folderLevel)
        {
            KmlSplitter.SplitKmlIntoFolders(kmlFile, outputPath, folderLevel);
        }

        public static Style FindStyleByStyleURL(KmlFile kmlFile, string styleUrl)
        {
            SharpKml.Dom.Style style = null;
            if (!String.IsNullOrEmpty(styleUrl))
            {
                if (styleUrl.StartsWith("#"))
                {
                    styleUrl = styleUrl.Substring(1);
                }

                SharpKml.Dom.StyleSelector styleSelector = kmlFile.Styles.FirstOrDefault(s => s.Id == styleUrl);
                if (styleSelector != null && styleSelector is StyleMapCollection)
                {
                    StyleMapCollection styleMapCollection = styleSelector as StyleMapCollection;
                    styleMapCollection.ToList().ForEach(element =>
                    {
                        if (element is Pair)
                        {
                            Pair pair = element as Pair;
                            if (pair.State != null && pair.State == StyleState.Highlight)
                            {
                                string styleUrl2 = pair.StyleUrl.OriginalString;
                                if (!String.IsNullOrEmpty(styleUrl2))
                                {
                                    if (styleUrl2.StartsWith("#"))
                                    {
                                        styleUrl2 = styleUrl2.Substring(1);
                                    }
                                    SharpKml.Dom.StyleSelector styleSelector2 = kmlFile.Styles.FirstOrDefault(s => s.Id == styleUrl2);
                                    if (styleSelector2 != null && styleSelector2 is SharpKml.Dom.Style)
                                    {
                                        style = styleSelector2 as SharpKml.Dom.Style;
                                    }
                                }
                            }
                        }
                    });
                }
            }
            return style;
        }

        public static Schema FindSchemaByName(KmlFile kmlFile, String name)
        {
            Document document = (kmlFile.Root as Kml).Feature as Document;
            return document.Schemas.FirstOrDefault(s => s.Name == name);
        }

        public static SimpleField setSimpleField(String name, String fieldType)
        {
            SimpleField simpleField = new SimpleField();
            simpleField.DisplayName = name;
            simpleField.FieldType = fieldType;
            return simpleField;
        }


    }
}
