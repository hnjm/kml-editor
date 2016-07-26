using CommandLine;
using CommandLine.Text;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KmlEditorConsole
{
    public class MainOption
    {
        [Option('s', "split", Required = false,
            HelpText = "Split a kml or kmz file into folders and files")]
        public bool split { get; set; }

        [Option('j', "join", Required = false,
            HelpText = "Join folders and files into a kml file")]
        public bool join { get; set; }

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
