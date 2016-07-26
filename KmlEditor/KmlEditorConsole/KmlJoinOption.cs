using CommandLine;
using CommandLine.Text;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KmlEditorConsole
{
    public class KmlJoinOption
    {
        [Option('j', "join", Required = true,
            HelpText = "Join folders and files into a kml file.")]
        public bool join { get; set; }

        [Option('i', "input", Required = true,
            HelpText = "Input folder.")]
        public string inputFolder { get; set; }

        [Option('o', "output", Required = true,
            HelpText = "Output kml file.")]
        public string outputFile { get; set; }

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
