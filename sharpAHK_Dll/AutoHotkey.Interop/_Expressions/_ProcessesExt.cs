using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AHKExpressions
{
    public static partial class StringExtension
    {
        // === Process ===

        /// <summary>Runs an external program.</summary>
        /// <param name="Target">A document, URL, executable file (.exe, .com, .bat, etc.), shortcut (.lnk), or system verb to launch (see remarks). If Target is a local file and no path was specified with it, A_WorkingDir will be searched first. If no matching file is found there, the system will search for and launch the file if it is integrated ("known"), e.g. by being contained in one of the PATH folders. To pass parameters, add them immediately after the program or document name. If a parameter contains spaces, it is safest to enclose it in double quotes (even though it may work without them in some cases).</param>
        /// <param name="WorkingDir">The working directory for the launched item. Do not enclose the name in double quotes even if it contains spaces. If omitted, the script's own working directory (A_WorkingDir) will be used. </param>
        /// <param name="MinMaxHideUseErrorLevel">Max: launch maximized | Min: launch minimized | Hide: launch hidden | UseErrorLevel: If the launch fails, this option skips the warning dialog, sets ErrorLevel to the word ERROR, and allows the current thread to continue. </param>
        /// <returns>Returns OutputVarPID as string</returns>
        public static string Run(this string Target, string WorkingDir = "", string MinMaxHideUseErrorLevel = "")
        {
            string AHKLine = "Run, " + Target + "," + WorkingDir + "," + MinMaxHideUseErrorLevel + ", OutputVarPID";  // ahk line to execute
            ErrorLog_Setup(true); // ErrorLevel Detection Enabled for this function in AHK 
            string OutVar = Execute(AHKLine, "OutputVarPID");   // execute AHK code and return variable value 
            return OutVar;

            // v1 ToDo
            /*
                    public IntPtr _Run(string Target, string Arguments = "", bool Hidden = false)  // run application path. includes win tools (Notepad, cmd, calc, word, paint, regedit, notepad++ as TARGET to launch)
                    {
                        IntPtr ApphWnd = new IntPtr(0);

                        if (Target.ToUpper() == "NOTEPAD") { Target = Environment.SystemDirectory + "\\notepad.exe"; }
                        if (Target.ToUpper() == "CMD" || Target.ToUpper() == "COMMAND") { Target = Environment.SystemDirectory + "\\cmd.exe"; }
                        if (Target.ToUpper() == "CALC" || Target.ToUpper() == "CALCULATOR") { Target = Environment.SystemDirectory + "\\calc.exe"; }
                        if (Target.ToUpper() == "WORD") { Target = Environment.SystemDirectory + "\\word.exe"; }
                        if (Target.ToUpper() == "PAINT") { Target = Environment.SystemDirectory + "\\paint.exe"; }
                        if (Target.ToUpper() == "REGEDIT") { Target = Environment.SystemDirectory + "\\regedit.exe"; }

                        if (Target.ToUpper() == "NOTEPAD++")
                        {
                            Target = @"C:\Google Drive\AHK\Lib\Tool_Library\Office\Notepad++\App\Notepad++\Notepad++.exe";
                            if (!File.Exists(Target)) { Target = @"C:\Users\jason\Google Drive\AHK\Lib\Tool_Library\Office\Notepad++\App\Notepad++\Notepad++.exe"; }
                        }

                        if (Target.ToUpper().Contains("HTTP://"))  //user trying to open url
                        {
                            Process proc;
                            proc = System.Diagnostics.Process.Start(Target);
                            proc.WaitForInputIdle();
                            try { ApphWnd = proc.MainWindowHandle; }
                            catch { }
                            return ApphWnd;
                        }

                        if (File.Exists(Target))
                        {
                            FileInfo info = new FileInfo(Target);

                            // check to see if target is a directory, if so 
                            FileAttributes attr = File.GetAttributes(Target);
                            if ((attr & FileAttributes.Directory) == FileAttributes.Directory)
                            {
                                //it's a directory
                                OpenDir(Target);
                                return ApphWnd;
                            }


                            ProcessStartInfo startInfo = new ProcessStartInfo();
                            startInfo.WorkingDirectory = info.Directory.ToString();
                            startInfo.FileName = Target;
                            startInfo.Arguments = Arguments;

                            if (Hidden == true)
                            {
                                startInfo.WindowStyle = ProcessWindowStyle.Hidden;
                            }

                            Process proc;
                            proc = Process.Start(startInfo);
                            try { proc.WaitForInputIdle(); }
                            catch { }

                            try { ApphWnd = proc.MainWindowHandle; }
                            catch { }
                            return ApphWnd;
                        }

                        return ApphWnd;
                    }
            */



        }

        /// <summary>Runs an external program. RunWait will wait until the program finishes before continuing.</summary>
        /// <param name="Target">A document, URL, executable file (.exe, .com, .bat, etc.), shortcut (.lnk), or system verb to launch (see remarks). If Target is a local file and no path was specified with it, A_WorkingDir will be searched first. If no matching file is found there, the system will search for and launch the file if it is integrated ("known"), e.g. by being contained in one of the PATH folders. To pass parameters, add them immediately after the program or document name. If a parameter contains spaces, it is safest to enclose it in double quotes (even though it may work without them in some cases).</param>
        /// <param name="WorkingDir">The working directory for the launched item. Do not enclose the name in double quotes even if it contains spaces. If omitted, the script's own working directory (A_WorkingDir) will be used. </param>
        /// <param name="MinMaxHideUseErrorLevel">Max: launch maximized | Min: launch minimized | Hide: launch hidden | UseErrorLevel: If the launch fails, this option skips the warning dialog, sets ErrorLevel to the word ERROR, and allows the current thread to continue. </param>
        /// <returns>Returns OutputVarPID as string</returns>
        public static string RunWait(this string Target, string WorkingDir = "", string MinMaxHideUseErrorLevel = "")
        {
            string AHKLine = "RunWait, " + Target + "," + WorkingDir + "," + MinMaxHideUseErrorLevel + ", OutputVarPID";  // ahk line to execute
            ErrorLog_Setup(true); // ErrorLevel Detection Enabled for this function in AHK 
            string OutVar = Execute(AHKLine, "OutputVarPID");   // execute AHK code and return variable value 

            // returns OutputVarPID
            return OutVar;

            // v1 ToDo
            /*
                    public bool _RunWait(string ProcessPath, string Arguments = "", bool Hidden = false)  // runs application and waits for the application to end before continuing
                    {
                        if (File.Exists(ProcessPath))
                        {
                            Process process = new Process();
                            process.StartInfo.FileName = ProcessPath;
                            process.StartInfo.Arguments = Arguments;
                            process.StartInfo.ErrorDialog = true;
                            //process.StartInfo.WindowStyle = ProcessWindowStyle.Minimized;
                            if (Hidden == true)
                            {
                                process.StartInfo.WindowStyle = ProcessWindowStyle.Hidden;
                            }
                            process.Start();
                            process.WaitForExit();
                            return true;
                        }

                        return false;
                    }
            */

        }


        /// <summary>Waits the specified amount of time before continuing.</summary>
        /// <param name="DelayInMilliseconds">The amount of time to pause (in milliseconds) between 0 and 2147483647 (24 days), which can be an expression.</param>        
        public static void Sleep(this object DelayInMilliseconds)
        {
            string AHKLine = "Sleep, " + DelayInMilliseconds.ToString();  // ahk line to execute
            ErrorLog_Setup(false); // ErrorLevel Detection Enabled for this function in AHK 
            Execute(AHKLine);   // execute AHK code and return variable value 

            // v1 ToDo
            /*
                    _Sleep(int SleepTime)  // sleeps / idles for x seconds before continuing
                    {
                        Thread.Sleep(SleepTime);
                    }
            */

        }




    }
}
