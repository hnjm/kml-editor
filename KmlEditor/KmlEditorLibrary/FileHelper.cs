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
    public class FileHelper
    {
        /*
         * From: Gary Kindel in http://stackoverflow.com/revisions/8626562/1
         */
        public static string RemoveInvalidFilePathCharacters(string filename, string replaceChar = "")
        {
            string regexSearch = new string(Path.GetInvalidFileNameChars()) + new string(Path.GetInvalidPathChars());
            Regex r = new Regex(string.Format("[{0}]", Regex.Escape(regexSearch)));
            return r.Replace(filename, replaceChar);
        }

        public static KmlFile LoadKmlKmzFile(String kmlKmzFilePath)
        {
            string fileExtension = Path.GetExtension(kmlKmzFilePath);
            using (FileStream fileStream = File.OpenRead(kmlKmzFilePath))
            {
                if (fileExtension.Equals(".kmz", StringComparison.OrdinalIgnoreCase))
                {
                    KmzFile kmzFile = KmzFile.Open(fileStream);
                    string kmlFileString = kmzFile.ReadKml();
                    using (StringReader stringReader = new StringReader(kmlFileString))
                    {
                        KmlFile kmlFile = KmlFile.Load(stringReader);
                        return kmlFile;
                    }
                }
                else
                {
                    KmlFile  kmlFile = KmlFile.Load(fileStream);
                    return kmlFile;
                }
            }
        }

        public static void SaveToKmlFile(Document document, string kmlFilePath)
        {
            KmlFile kmlFile = KmlFile.Create(document, false);
            SaveToKmlFile(kmlFile, kmlFilePath);
        }

        public static void SaveToKmlFile(KmlFile kmlFile, String kmlFilePath)
        {
            using (var stream = System.IO.File.OpenWrite(kmlFilePath))
            {
                kmlFile.Save(stream);
            }
        }

        public static void CreateDirectoryAndCleanIt(string path)
        {
            DirectoryInfo dir = new DirectoryInfo(path);
            if (!dir.Exists)
            {
                dir.Create();
            }
            else
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

        }
    }
}
