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
        public static void SplitKmlIntoFolders(string kmlFilePath, String outputPath, int folderLevel)
        {
            KmlFile kmlFile = FileHelper.LoadKmlKmzFile(kmlFilePath);
            SplitKmlIntoFolders(kmlFile, outputPath, folderLevel);
        }

        public static void SplitKmlIntoFolders(KmlFile kmlFile, String outputPath, int folderLevel)
        {
            if (kmlFile == null) return;

            FileHelper.CreateDirectoryAndCleanIt(outputPath);

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
        }

        static void ProcessDocument(Document document, string outputPath, int folderLevel, int currentFolderLevel)
        {
            Document newDoc = new Document();
            newDoc.Name = document.Name;
            if (document.Description != null) newDoc.Description = document.Description.Clone();
            if (document.Schemas != null) document.Schemas.ToList().ForEach(s => newDoc.AddSchema(s.Clone()));
            //if (document.Styles != null) document.Styles.ToList().ForEach(s => newDoc.AddStyle(s.Clone()));
            if (document.StyleUrl != null) newDoc.StyleUrl = document.StyleUrl;

            string newOutputPath = Path.Combine(outputPath, FileHelper.RemoveInvalidFilePathCharacters(newDoc.Name));
            Directory.CreateDirectory(newOutputPath);
            string kmlFilePath = Path.Combine(outputPath, FileHelper.RemoveInvalidFilePathCharacters(document.Name) + ".kml");
            FileHelper.SaveToKmlFile(newDoc, kmlFilePath);

            IEnumerable<Feature> features = document.Features;
            features.ToList().ForEach(feature => {
                ProcessFeature(feature, newOutputPath, folderLevel, currentFolderLevel+1, document);
            });
        }

        static void ProcessFolder(Folder folder, string outputPath, int folderLevel, int currentFolderLevel, Document doc)
        {
            string kmlFilePath = Path.Combine(outputPath, FileHelper.RemoveInvalidFilePathCharacters(folder.Name) + ".kml");
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
                    string newOutputPath = Path.Combine(outputPath, FileHelper.RemoveInvalidFilePathCharacters(feature.Name));
                    Directory.CreateDirectory(newOutputPath);
                    ProcessFolder(feature as Folder, newOutputPath, folderLevel, currentFolderLevel + 1, doc);
                    Folder newFolder = new Folder();
                    if(feature.Name != null) newFolder.Name = feature.Name;
                    if(feature.Description != null) newFolder.Description = feature.Description.Clone();
                    newDoc.AddFeature(newFolder);
                }
                else
                {
                    newDoc.AddFeature(feature.Clone());
                }
            });
            FileHelper.SaveToKmlFile(newDoc, kmlFilePath);
        }
    }
}
