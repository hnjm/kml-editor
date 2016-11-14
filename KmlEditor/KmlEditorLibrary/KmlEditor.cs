using SharpKml.Engine;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KmlEditorLibrary
{
    public class KmlEditor
    {
        KmlFile _kmlFile = null;
        public KmlFile kmlFile {
            get { return _kmlFile; }
        }

        public void openFile(String filePath)
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
                        _kmlFile = KmlFile.Load(stringReader);
                    }
                }
                else
                {
                    _kmlFile = KmlFile.Load(fileStream);
                }
            }
        }

        public void closeFile()
        {
            _kmlFile = null;
        }

        public void splitKmlIntoFolders(String outputPath, int folderLevel)
        {
            KmlSplitter.SplitKmlIntoFolders(_kmlFile, outputPath, folderLevel);
        }
    }
}
