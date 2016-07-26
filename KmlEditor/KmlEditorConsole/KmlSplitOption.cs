using CommandLine;
using CommandLine.Text;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KmlEditorConsole
{
    class KmlSplitOption
    {
        [Option('s', "split", Required = true,
            HelpText = "Kml or kmz file to be splitted.")]
        public bool split { get; set; }

        [Option('i', "input", Required = true, 
            HelpText = "Kml or kmz file to be splitted.")]
        public string file { get; set; }

        [Option('l', "level", Required = true,
            HelpText = "Folder level.")]
        public int folderLevel { get; set; }

        [Option('o', "output", Required = true,
            HelpText = "Folder output.")]
        public string outputFolder { get; set; }

        [ParserState]
        public IParserState LastParserState { get; set; }

        [HelpOption]
        public string GetUsage()
        {
            return HelpText.AutoBuild(this,
              (HelpText current) => HelpText.DefaultParsingErrorsHandler(this, current));
        }
    }
}
