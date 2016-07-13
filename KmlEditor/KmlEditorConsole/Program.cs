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
            String inFile = @"C:\tmp\Test\todalared.kmz";
            String outFile = @"C:\tmp\Test\out.kml";
            KmlEditor kmlEditor = new KmlEditor();
            kmlEditor.openFile(inFile);
        }
    }
}
