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
    public class KmlJoiner
    {
        static KmlFile loadKmlFile(String filePath)
        {
            KmlFile kmlFile = null;
            using (FileStream fileStream = File.OpenRead(filePath))
            {
                kmlFile = KmlFile.Load(fileStream);
            }
            return kmlFile;
        }

        public static void saveKml(KmlFile kmlFile, String kmlFilePath)
        {
            using (var stream = System.IO.File.OpenWrite(kmlFilePath))
            {
                kmlFile.Save(stream);
            }
        }

        public static void JoinFoldersIntoKml(String inputPath, String outFile)
        {
            KmlFile kmlOut = JoinFoldersIntoKml(inputPath);
            saveKml(kmlOut, outFile);
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

            KmlFile rootKmlFile = loadKmlFile(kmlf.FullName);
            if (!(rootKmlFile.Root is Document)) throw new Exception();
            Document rootDoc = (Document) rootKmlFile.Root;

            dir.EnumerateDirectories().ToList().ForEach(f => { processFolder(rootDoc, f); });
            KmlFile kmlOut = KmlFile.Create(rootDoc, false);
            return kmlOut;
        }

        static void processFolder(Container parentContainer, DirectoryInfo directory)
        {
            FileInfo kmlf = directory.EnumerateFiles().ToList().FirstOrDefault(f => ".kml".Equals(f.Extension, StringComparison.OrdinalIgnoreCase));
            if (kmlf == null) throw new Exception();

            KmlFile rootKmlFile = loadKmlFile(kmlf.FullName);
            if (!(rootKmlFile.Root is Document)) throw new Exception();
            Document doc = (Document)rootKmlFile.Root;
            Folder folder = new Folder();
            folder.Name = doc.Name;
            parentContainer.AddFeature(folder);
            doc.Features.ToList().ForEach(feature => folder.AddFeature(feature.Clone()));
            directory.EnumerateDirectories().ToList().ForEach(f => { processFolder(folder, f); });
        }
    }
}
