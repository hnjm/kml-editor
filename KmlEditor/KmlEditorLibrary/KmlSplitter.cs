using SharpKml.Dom;
using SharpKml.Engine;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace KmlEditorLibrary
{
    public class KmlSplitter
    {
        public static void SplitKmlIntoFolders(KmlFile kmlFile, String outputPath, int folderLevel)
        {
            if (kmlFile == null) return;

            // check if the folder exists
            DirectoryInfo dir = new DirectoryInfo(outputPath);
            if (!dir.Exists)
            {
                dir.Create();
            } else
            { 
                foreach (FileInfo fi in dir.GetFiles())
                {
                    fi.Delete();
                }

                foreach (DirectoryInfo di in dir.GetDirectories())
                {
                    di.Delete(true);
                }
            }

            Kml kml = kmlFile.Root as Kml;
            if (kml != null)
            {
                Feature feature = kml.Feature;
                ProcessFeature(feature, outputPath, folderLevel, 0, null);
            }
        }

        static void ProcessFeature(Feature feature, string outputPath, int folderLevel, int currentFolderLevel, Document doc)
        {
            if (feature is Document)
            {
                ProcessDocument(feature as Document, outputPath, folderLevel, currentFolderLevel);
            }
            else if (feature is Folder)
            {
                ProcessFolder(feature as Folder, outputPath, folderLevel, currentFolderLevel, doc);
            }
            else if (feature is Container)
            {
                ProcessContainer(feature as Container, outputPath, folderLevel, currentFolderLevel);
            }
        }

        static void ProcessContainer(Container container, string outputPath, int folderLevel, int currentFolderLevel)
        {
            if (container is Document)
            {
                ProcessDocument(container as Document, outputPath, folderLevel, currentFolderLevel);
            }
        }

        static void ProcessDocument(Document document, string outputPath, int folderLevel, int currentFolderLevel)
        {
            IEnumerable<Feature> features = document.Features;
            features.ToList().ForEach(feature => {
                ProcessFeature(feature, outputPath, folderLevel, currentFolderLevel, document);
            });
        }

        static string RemoveInvalidFilePathCharacters(string filename, string replaceChar = "")
        {
            string regexSearch = new string(Path.GetInvalidFileNameChars()) + new string(Path.GetInvalidPathChars());
            Regex r = new Regex(string.Format("[{0}]", Regex.Escape(regexSearch)));
            return r.Replace(filename, replaceChar);
        }

        static void ProcessFolder(Folder folder, string outputPath, int folderLevel, int currentFolderLevel, Document doc)
        {
            string kmlFilePath = Path.Combine(outputPath, RemoveInvalidFilePathCharacters(folder.Name) + ".kml");
            Document newDoc = new Document();
            newDoc.Name = folder.Name;
            if (folder.Description != null) newDoc.Description = folder.Description.Clone();
            if(doc.Schemas != null) doc.Schemas.ToList().ForEach(s => newDoc.AddSchema(s.Clone()));
            //if (doc.Styles != null) doc.Styles.ToList().ForEach(s => newDoc.AddStyle(s.Clone()));
            if (doc.StyleUrl != null) newDoc.StyleUrl = doc.StyleUrl;

            IEnumerable<Feature> features = folder.Features;
            features.ToList().ForEach(feature =>
            {
                if (feature is Folder && folderLevel > currentFolderLevel)
                {
                    string newOutputPath = Path.Combine(outputPath, RemoveInvalidFilePathCharacters(feature.Name));
                    Directory.CreateDirectory(newOutputPath);
                    ProcessFolder(feature as Folder, newOutputPath, folderLevel, currentFolderLevel + 1, doc);
                }
                else
                {
                    newDoc.AddFeature(feature.Clone());
                }
            });
            using (var stream = System.IO.File.OpenWrite(kmlFilePath))
            {
                KmlFile kmlOut = KmlFile.Create(newDoc, false);
                kmlOut.Save(stream);
            }
            /*

            if (folderLevel > currentFolderLevel) {
                string newOutputPath = Path.Combine(outputPath, folder.Name);
                Directory.CreateDirectory(newOutputPath);
                string folderInformationFile = Path.Combine(newOutputPath, "folderDescription.txt");
                using (StreamWriter outputFile = new StreamWriter(folderInformationFile))
                {
                    if(folder.Description != null)
                    {
                        String description = folder.Description.Text;
                        outputFile.WriteLine(folder.Description.Text);
                    }
                }
                Document doc = new Document();
                doc.Name = folder.Name;
                //d.Schemas.ToList().ForEach(s => doc.AddSchema(s.Clone()));
                doc.AddFeature(folder.Clone());

                KmlFile kmlOut = KmlFile.Create(doc, false);
                string folderFileName = Path.Combine(outputPath, "folder.kml");

                IEnumerable<Feature> features = folder.Features;
                features.ToList().ForEach(feature =>
                {
                    if (feature is Folder)
                    {
                        ProcessFolder(feature as Folder, newOutputPath, folderLevel, currentFolderLevel + 1);
                    } else
                    {
                        doc.AddFeature(feature.Clone());
                    }
                });
                using (var stream = System.IO.File.OpenWrite(folderFileName))
                {
                    kmlOut.Save(stream);
                }
            }
            else
            {
                string folderFileName = Path.Combine(outputPath, "folder.kml");
                Document doc = new Document();
                doc.Name = folder.Name;
                //d.Schemas.ToList().ForEach(s => doc.AddSchema(s.Clone()));
                doc.AddFeature(folder.Clone());
                KmlFile kmlOut = KmlFile.Create(doc, false);
                using (var stream = System.IO.File.OpenWrite(folderFileName))
                {
                    kmlOut.Save(stream);
                }
            }*/
        }
    }
}
