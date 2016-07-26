using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Diagnostics;

namespace KmlEditorConsoleTest
{
    [TestClass]
    public class KmlEditorConsoleUnitTest
    {
        string TestArguments(string arguments)
        {
            Process process = new Process();
            process.StartInfo.FileName = "KmlEditorConsole.exe";
            process.StartInfo.Arguments = arguments;
            process.StartInfo.RedirectStandardOutput = true;
            process.StartInfo.RedirectStandardInput = true;
            process.StartInfo.UseShellExecute = false;
            //* Set your output and error (asynchronous) handlers
            //process.OutputDataReceived += (s, e) => Console.WriteLine(e.Data);
            //process.ErrorDataReceived += (s, e) => Console.WriteLine(e.Data);
            process.Start();
            // You can then use prop.StandardInput and prop.StandardOutput for testing.
            process.WaitForExit();
            int code = process.ExitCode;
            //* Read the output (or the error)
            string output = process.StandardOutput.ReadToEnd();
            return output;
        }

        [TestMethod]
        public void TestKmlSplitterNoArguments()
        {
            String[] args = { };
            KmlEditorConsole.Program.Main(args);
        }

        [TestMethod]
        public void TestKmlSplitterOnlyS()
        {
            String[] args = { "-s" };
            KmlEditorConsole.Program.Main(args);
        }

        [TestMethod]
        public void TestKmlSplitterOnlyS1()
        {
            String[] args = { "-s", "-i"};
            KmlEditorConsole.Program.Main(args);
        }

        [TestMethod]
        public void TestKmlSplitterSFinal()
        {
            String[] args = { "-s",
                "-i", "C:\\tmp\\Test\\Ferromapas (en Edición).kml",
                "-o", "C:\\tmp\\output",
                "-l", "4"};
            KmlEditorConsole.Program.Main(args);
        }

        [TestMethod]
        public void TestKmlJoinnerOnlyJ()
        {
            String[] args = { "-j" };
            KmlEditorConsole.Program.Main(args);
        }

        [TestMethod]
        public void TestKmlJoinnerOnlyJ1()
        {
            String[] args = { "-j", "-i" };
            KmlEditorConsole.Program.Main(args);
        }

        [TestMethod]
        public void TestKmlJoinnerJFinal()
        {
            String[] args = { "-j",
                "-i", "C:\\tmp\\output",
                "-o", "C:\\tmp\\Test\\out.kml"};
            KmlEditorConsole.Program.Main(args);
        }
    }
}
