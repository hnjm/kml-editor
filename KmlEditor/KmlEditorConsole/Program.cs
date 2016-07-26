using KmlEditorLibrary;
using System;


namespace KmlEditorConsole
{
    class Program
    {
        static void Main(string[] args)
        {
            MainOption mainOption = new MainOption();
            if (CommandLine.Parser.Default.ParseArguments(args, mainOption))
            {
                if (mainOption.split) {
                    KmlSplitOption kmlSplitOption = new KmlSplitOption();
                    if (CommandLine.Parser.Default.ParseArguments(args, kmlSplitOption))
                    {
                        Console.WriteLine("Split file '" + kmlSplitOption.file + "' to folder '" + kmlSplitOption.outputFolder + "' folderLevel:" + kmlSplitOption.folderLevel);
                        KmlSplitter.SplitKmlIntoFolders(kmlSplitOption.file, kmlSplitOption.outputFolder, kmlSplitOption.folderLevel);
                    } else {
                        kmlSplitOption.GetUsage();
                    }
                } else if (mainOption.join)
                {
                    KmlJoinOption kmlJoinOption = new KmlJoinOption();
                    if (CommandLine.Parser.Default.ParseArguments(args, kmlJoinOption))
                    {
                        Console.WriteLine("Join folder '" + kmlJoinOption.inputFolder + "' to file '" + kmlJoinOption.outputFile + "'");
                        KmlJoiner.JoinFoldersIntoKml(kmlJoinOption.inputFolder, kmlJoinOption.outputFile);
                    }
                    else
                    {
                        kmlJoinOption.GetUsage();
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
