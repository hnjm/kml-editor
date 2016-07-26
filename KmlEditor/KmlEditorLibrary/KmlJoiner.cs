using SharpKml.Dom;
using SharpKml.Engine;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace KmlEditorLibrary
{
    public class KmlJoiner
    {
        public static void JoinFoldersIntoKml(String inputPath, String outputFile)
        {
            KmlFile kmlFile = JoinFoldersIntoKml(inputPath);
            FileHelper.SaveToKmlFile(kmlFile, outputFile);
        }

        public static KmlFile JoinFoldersIntoKml(String inputPath)
        {
            DirectoryInfo dir = new DirectoryInfo(inputPath);
            if (!dir.Exists)
            {
                throw new DirectoryNotFoundException();
            }

            FileInfo kmlf = dir.EnumerateFiles().ToList().FirstOrDefault(f => ".kml".Equals(f.Extension, StringComparison.OrdinalIgnoreCase));
            if (kmlf == null) throw new Exception();

            KmlFile rootKmlFile = FileHelper.LoadKmlKmzFile(kmlf.FullName);
            if (!(rootKmlFile.Root is Document)) throw new Exception();
            Document rootDoc = (Document) (rootKmlFile.Root.Clone());

            dir.EnumerateDirectories().ToList().ForEach(f => { processFolder(rootDoc, f); });
            KmlFile kmlOut = KmlFile.Create(rootDoc, false);
            return kmlOut;
        }

        static void processFolder(Container parentContainer, DirectoryInfo directory)
        {
            FileInfo kmlf = directory.EnumerateFiles().ToList().FirstOrDefault(f => ".kml".Equals(f.Extension, StringComparison.OrdinalIgnoreCase));
            if (kmlf == null) throw new Exception();

            KmlFile rootKmlFile = FileHelper.LoadKmlKmzFile(kmlf.FullName);
            if (!(rootKmlFile.Root is Document)) throw new Exception();
            Document doc = (Document)rootKmlFile.Root;
            Folder folder = (Folder)parentContainer.Features.FirstOrDefault(c => c is Folder && doc.Name.Equals(c.Name, StringComparison.OrdinalIgnoreCase));
            if (folder == null)
            {
                folder = new Folder();
                parentContainer.AddFeature(folder);
            }
            folder.Name = doc.Name;
            if (doc.Description != null) folder.Description = doc.Description;
            List<DirectoryInfo> directories = directory.EnumerateDirectories().ToList();
            doc.Features.ToList().ForEach(feature => folder.AddFeature(feature.Clone()));
            directory.EnumerateDirectories().ToList().ForEach(f => { processFolder(folder, f); });
        }
    }
}
