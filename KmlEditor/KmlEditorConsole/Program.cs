using KmlEditorLibrary;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KmlEditorConsole
{
    class Program
    {
        static void Main(string[] args)
        {
            /*
            String inFile = @"C:\tmp\Test\Ferromapas (en Edición).kml";
            String outFile = @"C:\tmp\Test\out.kml";
            KmlEditor kmlEditor = new KmlEditor();
            kmlEditor.openFile(inFile);
            kmlEditor.splitKmlIntoFolders("C:\\tmp\\output", 3);
            kmlEditor.closeFile();
            */

            string inFile = "C:\\tmp\\output";
            string outKml = "C:\\tmp\\Test\\out.kml";
            KmlJoiner.JoinFoldersIntoKml(inFile, outKml);
        }
    }
}
