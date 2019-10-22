using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace sharpAHK
{
    public partial class _AHK
    {
        #region === Process Management ===

        /// <summary>Performs one of the following operations on a process: checks if it exists; changes its priority; closes it; waits for it to close.</summary>
        /// <param name="Cmd">Exist | Close | List | Priority | Wait | WaitClose</param>
        /// <param name="PIDorName">This parameter can be either a number (the PID) or a process name as described below. It can also be left blank to change the priority of the script itself.</param>
        /// <param name="Param3">See AHK Documentation For Options</param>
        public bool process(string Cmd, string PIDorName, string Param3 = "")
        {
            string AHKLine = "Process, " + Cmd + "," + PIDorName + "," + Param3;  // ahk line to execute
            ErrorLog_Setup(true); // ErrorLevel Detection Enabled for this function in AHK 
            Execute(AHKLine);   // execute AHK code and return variable value

            if (!ahkGlobal.ErrorLevel) { return true; } // no error level - return true for success
            return false;  // error level detected - success = false
        }

        /// <summary>Runs an external program.</summary>
        /// <param name="Target">A document, URL, executable file (.exe, .com, .bat, etc.), shortcut (.lnk), or system verb to launch (see remarks). If Target is a local file and no path was specified with it, A_WorkingDir will be searched first. If no matching file is found there, the system will search for and launch the file if it is integrated ("known"), e.g. by being contained in one of the PATH folders. To pass parameters, add them immediately after the program or document name. If a parameter contains spaces, it is safest to enclose it in double quotes (even though it may work without them in some cases).</param>
        /// <param name="WorkingDir">The working directory for the launched item. Do not enclose the name in double quotes even if it contains spaces. If omitted, the script's own working directory (A_WorkingDir) will be used. </param>
        /// <param name="MinMaxHideUseErrorLevel">Max: launch maximized | Min: launch minimized | Hide: launch hidden | UseErrorLevel: If the launch fails, this option skips the warning dialog, sets ErrorLevel to the word ERROR, and allows the current thread to continue. </param>
        /// <returns>Returns OutputVarPID as string</returns>
        public string Run(string Target, string WorkingDir = "", string MinMaxHideUseErrorLevel = "")
        {
            if (Target == null) { return ""; }

            string target = Target.Replace(",", "`,");
            string workingDir = WorkingDir.Replace(",", "`,");

            string AHKLine = "Run, " + target + "," + workingDir + "," + MinMaxHideUseErrorLevel + ", OutputVarPID";  // ahk line to execute
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
        public string RunWait(string Target, string WorkingDir = "", string MinMaxHideUseErrorLevel = "")
        {
            string target = Target.Replace(",", "`,");
            string workingDir = WorkingDir.Replace(",", "`,");

            string AHKLine = "RunWait, " + target + "," + workingDir + "," + MinMaxHideUseErrorLevel + ", OutputVarPID";  // ahk line to execute
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

        /// <summary>Specifies a set of user credentials to use for all subsequent uses of Run and RunWait. Requires Windows 2000/XP or later.</summary>
        /// <param name="User">If this and the other parameters are all omitted, the RunAs feature will be turned off, which restores Run and RunWait to their default behavior. Otherwise, this is the username under which new processes will be created. </param>
        /// <param name="Password">User's password.</param>
        /// <param name="Domain">User's domain. To use a local account, leave this blank. If that fails to work, try using @YourComputerName.</param>
        public void RunAs(string User = "", string Password = "", string Domain = "")
        {
            ErrorLog_Setup(false);
            Execute("RunAs, " + User + "," + Password + "," + Domain);
        }

        /// <summary>Shuts down, restarts, or logs off the system.</summary>
        /// <param name="Code">A combination of shutdown codes:  LogOff = 0 | ShutDown = 1 | Reboot = 2 | Force = 4 | Power Down = 8</param>
        public void Shutdown(string Code)
        {
            ErrorLog_Setup(false);
            Execute("Shutdown, " + Code);
        }

        /// <summary>
        /// Returns exe path of window by WinTitle or WinHandle
        /// </summary>
        /// <param name="WinTitle"></param>
        /// <returns></returns>
        public string ProcessPath(object WinTitle)
        {
            IntPtr hWnd = WinHwnd(WinTitle);  //returns Window Handle (from either handle or window title)

            uint pid = 0;
            _GetWindowThreadProcessId(hWnd, out pid);
            if (hWnd != IntPtr.Zero)
            {
                if (pid != 0)
                {
                    var process = Process.GetProcessById((int)pid);
                    if (process != null)
                    {
                        return process.MainModule.FileName.ToString();
                    }
                }
            }
            return "";
        }


        #endregion


        /// <summary>
        /// Either Activate or Launch Application
        /// </summary>
        /// <param name="WinTitle">WinTitle to Activate / Look For</param>
        /// <param name="Path">Application Path to Launch</param>
        public void RunOrActivate(string WinTitle, string Path)
        {
            if (WinExist(WinTitle)) { WinRestore(WinTitle); WinActivate(WinTitle); } else { Run(Path); }
        }
            

    }
}
