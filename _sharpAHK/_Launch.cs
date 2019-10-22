using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace sharpAHK
{
    public partial class _AHK
    {

        /// <summary>
        /// Parse Application's CommandLine Parameters - Return Parsed Object
        /// </summary>
        /// <returns></returns>
        public static CmdLine CommandLineParams()
        {
            string InLine = "";
            //==== Command Line Parameters =================
            int c = Environment.GetCommandLineArgs().Count();
            if (c > 0)
            {
                for (int i = 0; i <= (c - 1); i++)
                {
                    string[] args = Environment.GetCommandLineArgs();

                    if (InLine != "") { InLine = InLine + "|" + args[i]; }
                    else { InLine = args[i]; }
                }
            }

            _AHK ahk = new _AHK();
            string ThisAppEXE = ahk.StringSplit(InLine, "|", 0);
            string AppName = ahk.StringSplit(InLine, "|", 1);
            string FilePath = ahk.StringSplit(InLine, "|", 2);
            string DirPath = "";

            if (File.Exists(AppName)) { FilePath = AppName; AppName = ""; }

            if (ThisAppEXE == AppName) { AppName = ""; }
            if (ThisAppEXE == FilePath) { FilePath = ""; }

            if (Directory.Exists(FilePath)) { DirPath = FilePath; FilePath = ""; }

            CmdLine cmd = new CmdLine();
            cmd.AppName = AppName;
            cmd.FilePath = FilePath;
            cmd.ThisExePath = ThisAppEXE;
            cmd.DirPath = DirPath;

            return cmd;
        }

        /// <summary>
        /// Contains CommandLine Parameter Details
        /// </summary>
        public class CmdLine
        {
            public string ThisExePath;
            public string AppName;
            public string FilePath;
            public string DirPath;
        }


    }
}
