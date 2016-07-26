using KmlEditorLibrary;
using System;


namespace KmlEditorConsole
{
    public class Program
    {
        public static void Main(string[] args)
        {
            MainOption mainOption = new MainOption();
            CommandLine.Parser parser = new CommandLine.Parser(s =>
            {
                s.IgnoreUnknownArguments = true;
            });
            if (parser.ParseArguments(args, mainOption))
            {
                if (mainOption.split) {
                    KmlSplitOption kmlSplitOption = new KmlSplitOption();
                    if (parser.ParseArguments(args, kmlSplitOption))
                    {
                        String output = "Split file '" + kmlSplitOption.file + "' to folder '" + kmlSplitOption.outputFolder + "' folderLevel:" + kmlSplitOption.folderLevel;
                        Console.WriteLine(output);
                        KmlSplitter.SplitKmlIntoFolders(kmlSplitOption.file, kmlSplitOption.outputFolder, kmlSplitOption.folderLevel);
                    } else {
                        Console.WriteLine(kmlSplitOption.GetUsage());
                    }
                } else if (mainOption.join)
                {
                    KmlJoinOption kmlJoinOption = new KmlJoinOption();
                    if (parser.ParseArguments(args, kmlJoinOption))
                    {
                        String output = "Join folder '" + kmlJoinOption.inputFolder + "' to file '" + kmlJoinOption.outputFile + "'";
                        Console.WriteLine(output);
                        KmlJoiner.JoinFoldersIntoKml(kmlJoinOption.inputFolder, kmlJoinOption.outputFile);
                    }
                    else
                    {
                        Console.WriteLine(kmlJoinOption.GetUsage());                        
                    }
                }
                else
                {
                    Console.WriteLine(mainOption.GetUsage());
                }
            }
            else
            {
                Console.WriteLine(mainOption.GetUsage());                
            }
        }
    }
}
