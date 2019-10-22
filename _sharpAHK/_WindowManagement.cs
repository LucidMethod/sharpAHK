using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.Drawing.Imaging;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace sharpAHK
{
    public partial class _AHK
    {
        // === Windows Management ===


        /// <summary>Makes a window stay on top of all other windows. Use ON to turn on the setting, OFF to turn it off, or TOGGLE to set it to the opposite of its current state. If omitted, it defaults to TOGGLE. The word Topmost can be used in place of AlwaysOnTop.</summary>
        /// <param name="WinTitle">The title or partial title of the target window (the matching behavior is determined by SetTitleMatchMode). If this and the other 3 parameters are omitted, the Last Found Window will be used. To use a window class, specify ahk_class ExactClassName (shown by Window Spy). To use a process identifier (PID), specify ahk_pid %VarContainingPID%. To use a window group, specify ahk_group GroupName. To use a window's unique ID number, specify ahk_id %VarContainingID%. The search can be narrowed by specifying multiple criteria. For example: My File.txt ahk_class Notepad</param>
        /// <param name="On">Turn AlwaysOnTop On/Off</param>
        /// <param name="Toggle">Option to Toggle Current AlwaysOnTop State</param>
        /// <param name="WinText">If present, this parameter must be a substring from a single text element of the target window (as revealed by the included Window Spy utility). Hidden text elements are detected if DetectHiddenText is ON.</param>
        /// <param name="ExcludeTitle">Windows whose titles include this value will not be considered. Note: Due to backward compatibility with .aut scripts, this parameter will be interpreted as a command if it exactly matches the name of a command. To work around this, use the WinActive() function instead.</param>
        /// <param name="ExcludeText">Windows whose text include this value will not be considered.</param>
        public void AlwaysOnTop(string WinTitle, bool On = true, bool Toggle = false, string WinText = "", string ExcludeTitle = "", string ExcludeText = "")
        {
            string Option = "On";
            if (!On) { Option = "Off"; }
            if (Toggle) { Option = "Toggle"; }
            string AHKLine = "WinSet, AlwaysOnTop, " + Option + "," + WinTitle + "," + WinText + "," + ExcludeTitle + "," + ExcludeText;  // ahk line to execute
            ErrorLog_Setup(false); // ErrorLevel Detection Enabled for this function in AHK 
            Execute(AHKLine);   // execute AHK code and return variable value

            // v1 ToDo
            /*
                    public void WinAlwaysOnTop(object WinTitle, bool SetAlwaysOnTop = true)  // Set Window to be alway on top of other windows on screen (use SetAlwaysOnTop = false to toggle back to normal view mode)
                    {
                        IntPtr hWnd = _WinHwnd(WinTitle);  //returns Window Handle (from either handle or window title)

                        if (SetAlwaysOnTop == true)
                        {
                            _SetWindowPos(hWnd, HWND_TOPMOST, 0, 0, 0, 0, TOPMOST_FLAGS);
                        }

                        if (SetAlwaysOnTop == false)
                        {
                            _SetWindowPos(hWnd, HWND_NOTOPMOST, 0, 0, 0, 0, TOPMOST_FLAGS);
                        }

                    }
             */


        }

        /// <summary>Sends a window to the bottom of stack; that is, beneath all other windows. The effect is similar to pressing Alt-Escape.</summary>
        /// <param name="WinTitle">The title or partial title of the target window (the matching behavior is determined by SetTitleMatchMode). If this and the other 3 parameters are omitted, the Last Found Window will be used. To use a window class, specify ahk_class ExactClassName (shown by Window Spy). To use a process identifier (PID), specify ahk_pid %VarContainingPID%. To use a window group, specify ahk_group GroupName. To use a window's unique ID number, specify ahk_id %VarContainingID%. The search can be narrowed by specifying multiple criteria. For example: My File.txt ahk_class Notepad</param>
        /// <param name="WinText">If present, this parameter must be a substring from a single text element of the target window (as revealed by the included Window Spy utility). Hidden text elements are detected if DetectHiddenText is ON.</param>
        /// <param name="ExcludeTitle">Windows whose titles include this value will not be considered. Note: Due to backward compatibility with .aut scripts, this parameter will be interpreted as a command if it exactly matches the name of a command. To work around this, use the WinActive() function instead.</param>
        /// <param name="ExcludeText">Windows whose text include this value will not be considered.</param>
        public void WinSetBottom(string WinTitle = "", string WinText = "", string ExcludeTitle = "", string ExcludeText = "")
        {
            string AHKLine = "WinSet, Bottom,, " + WinTitle + "," + WinText + "," + ExcludeTitle + "," + ExcludeText;  // ahk line to execute
            ErrorLog_Setup(false); // ErrorLevel Detection Enabled for this function in AHK 
            Execute(AHKLine);   // execute AHK code and return variable value
        }

        /// <summary>Brings a window to the top of the stack without explicitly activating it. However, the system default settings will probably cause it to activate in most cases. In addition, this command may have no effect due to the operating system's protection against applications that try to steal focus from the user (it may depend on factors such as what type of window is currently active and what the user is currently doing). One possible work-around is to make the window briefly AlwaysOnTop, then turn off AlwaysOnTop.</summary>
        /// <param name="WinTitle">The title or partial title of the target window (the matching behavior is determined by SetTitleMatchMode). If this and the other 3 parameters are omitted, the Last Found Window will be used. To use a window class, specify ahk_class ExactClassName (shown by Window Spy). To use a process identifier (PID), specify ahk_pid %VarContainingPID%. To use a window group, specify ahk_group GroupName. To use a window's unique ID number, specify ahk_id %VarContainingID%. The search can be narrowed by specifying multiple criteria. For example: My File.txt ahk_class Notepad</param>
        /// <param name="WinText">If present, this parameter must be a substring from a single text element of the target window (as revealed by the included Window Spy utility). Hidden text elements are detected if DetectHiddenText is ON.</param>
        /// <param name="ExcludeTitle">Windows whose titles include this value will not be considered. Note: Due to backward compatibility with .aut scripts, this parameter will be interpreted as a command if it exactly matches the name of a command. To work around this, use the WinActive() function instead.</param>
        /// <param name="ExcludeText">Windows whose text include this value will not be considered.</param>
        public void WinSetTop(string WinTitle = "", string WinText = "", string ExcludeTitle = "", string ExcludeText = "")
        {
            string AHKLine = "WinSet, Top,, " + WinTitle + "," + WinText + "," + ExcludeTitle + "," + ExcludeText;  // ahk line to execute
            ErrorLog_Setup(false); // ErrorLevel Detection Enabled for this function in AHK 
            Execute(AHKLine);   // execute AHK code and return variable value
        }

        /// <summary>Enables a window. When a window is disabled, the user cannot move it or interact with its controls. In addition, disabled windows are omitted from the alt-tab list.</summary>
        /// <param name="WinTitle">The title or partial title of the target window (the matching behavior is determined by SetTitleMatchMode). If this and the other 3 parameters are omitted, the Last Found Window will be used. To use a window class, specify ahk_class ExactClassName (shown by Window Spy). To use a process identifier (PID), specify ahk_pid %VarContainingPID%. To use a window group, specify ahk_group GroupName. To use a window's unique ID number, specify ahk_id %VarContainingID%. The search can be narrowed by specifying multiple criteria. For example: My File.txt ahk_class Notepad</param>
        /// <param name="WinText">If present, this parameter must be a substring from a single text element of the target window (as revealed by the included Window Spy utility). Hidden text elements are detected if DetectHiddenText is ON.</param>
        /// <param name="ExcludeTitle">Windows whose titles include this value will not be considered. Note: Due to backward compatibility with .aut scripts, this parameter will be interpreted as a command if it exactly matches the name of a command. To work around this, use the WinActive() function instead.</param>
        /// <param name="ExcludeText">Windows whose text include this value will not be considered.</param>
        public void WinSetEnable(string WinTitle = "", string WinText = "", string ExcludeTitle = "", string ExcludeText = "")
        {
            string AHKLine = "WinSet, Enable,, " + WinTitle + "," + WinText + "," + ExcludeTitle + "," + ExcludeText;  // ahk line to execute
            ErrorLog_Setup(false); // ErrorLevel Detection Enabled for this function in AHK 
            Execute(AHKLine);   // execute AHK code and return variable value
        }

        /// <summary>Disables a window. When a window is disabled, the user cannot move it or interact with its controls. In addition, disabled windows are omitted from the alt-tab list.</summary>
        /// <param name="WinTitle">The title or partial title of the target window (the matching behavior is determined by SetTitleMatchMode). If this and the other 3 parameters are omitted, the Last Found Window will be used. To use a window class, specify ahk_class ExactClassName (shown by Window Spy). To use a process identifier (PID), specify ahk_pid %VarContainingPID%. To use a window group, specify ahk_group GroupName. To use a window's unique ID number, specify ahk_id %VarContainingID%. The search can be narrowed by specifying multiple criteria. For example: My File.txt ahk_class Notepad</param>
        /// <param name="WinText">If present, this parameter must be a substring from a single text element of the target window (as revealed by the included Window Spy utility). Hidden text elements are detected if DetectHiddenText is ON.</param>
        /// <param name="ExcludeTitle">Windows whose titles include this value will not be considered. Note: Due to backward compatibility with .aut scripts, this parameter will be interpreted as a command if it exactly matches the name of a command. To work around this, use the WinActive() function instead.</param>
        /// <param name="ExcludeText">Windows whose text include this value will not be considered.</param>
        public void WinSetDisable(string WinTitle = "", string WinText = "", string ExcludeTitle = "", string ExcludeText = "")
        {
            string AHKLine = "WinSet, Disable,, " + WinTitle + "," + WinText + "," + ExcludeTitle + "," + ExcludeText;  // ahk line to execute
            ErrorLog_Setup(false); // ErrorLevel Detection Enabled for this function in AHK 
            Execute(AHKLine);   // execute AHK code and return variable value
        }

        /// <summary>Attempts to update the appearance/contents of a window by informing the OS that the window's rectangle needs to be redrawn. If this method does not work for a particular window, try WinMove.</summary>
        /// <param name="WinTitle">The title or partial title of the target window (the matching behavior is determined by SetTitleMatchMode). If this and the other 3 parameters are omitted, the Last Found Window will be used. To use a window class, specify ahk_class ExactClassName (shown by Window Spy). To use a process identifier (PID), specify ahk_pid %VarContainingPID%. To use a window group, specify ahk_group GroupName. To use a window's unique ID number, specify ahk_id %VarContainingID%. The search can be narrowed by specifying multiple criteria. For example: My File.txt ahk_class Notepad</param>
        /// <param name="WinText">If present, this parameter must be a substring from a single text element of the target window (as revealed by the included Window Spy utility). Hidden text elements are detected if DetectHiddenText is ON.</param>
        /// <param name="ExcludeTitle">Windows whose titles include this value will not be considered. Note: Due to backward compatibility with .aut scripts, this parameter will be interpreted as a command if it exactly matches the name of a command. To work around this, use the WinActive() function instead.</param>
        /// <param name="ExcludeText">Windows whose text include this value will not be considered.</param>
        public void WinSetRedraw(string WinTitle = "", string WinText = "", string ExcludeTitle = "", string ExcludeText = "")
        {
            string AHKLine = "WinSet, Redraw,, " + WinTitle + "," + WinText + "," + ExcludeTitle + "," + ExcludeText;  // ahk line to execute
            ErrorLog_Setup(false); // ErrorLevel Detection Enabled for this function in AHK 
            Execute(AHKLine);   // execute AHK code and return variable value
        }

        /// <summary>Determines whether invisible text in a window is "seen" for the purpose of finding the window. This affects commands such as IfWinExist and WinActivate.</summary>
        /// <param name="On">True (Default): Hidden text will be detected. False: This is the default for AutoIt v2 scripts. Hidden text is not detected.</param>
        public void DetectHiddenText(bool On = true)
        {
            string Val = "On"; if (!On) { Val = "Off"; }

            string AHKLine = "DetectHiddenText, " + Val;  // ahk line to execute
            ErrorLog_Setup(false); // ErrorLevel Detection Enabled for this function in AHK 
            Execute(AHKLine);   // execute AHK code and return variable value
        }

        /// <summary>Determines whether invisible windows are "seen" by the script.</summary>
        /// <param name="On">True (Default): Hidden windows are detected. False: This is the default. Hidden windows are not detected, except by the WinShow command. </param>
        public void DetectHiddenWindows(bool On = true)
        {
            string Val = "On"; if (!On) { Val = "Off"; }

            string AHKLine = "DetectHiddenWindows, " + Val;  // ahk line to execute
            ErrorLog_Setup(false); // ErrorLevel Detection Enabled for this function in AHK 
            Execute(AHKLine);   // execute AHK code and return variable value
        }

        /// <summary>Checks if the specified window exists and is currently active (foremost).</summary>
        /// <param name="WinTitle">The title or partial title of the target window (the matching behavior is determined by SetTitleMatchMode). If this and the other 3 parameters are omitted, the Last Found Window will be used. To use a window class, specify ahk_class ExactClassName (shown by Window Spy). To use a process identifier (PID), specify ahk_pid %VarContainingPID%. To use a window group, specify ahk_group GroupName. To use a window's unique ID number, specify ahk_id %VarContainingID%. The search can be narrowed by specifying multiple criteria. For example: My File.txt ahk_class Notepad</param>
        /// <param name="WinText">If present, this parameter must be a substring from a single text element of the target window (as revealed by the included Window Spy utility). Hidden text elements are detected if DetectHiddenText is ON.</param>
        /// <param name="ExcludeTitle">Windows whose titles include this value will not be considered. Note: Due to backward compatibility with .aut scripts, this parameter will be interpreted as a command if it exactly matches the name of a command. To work around this, use the WinActive() function instead.</param>
        /// <param name="ExcludeText">Windows whose text include this value will not be considered.</param>
        public bool WinActive(string WinTitle = "", string WinText = "", string ExcludeTitle = "", string ExcludeText = "")
        {
            string ahkString = @"
                OutputVar = false
                IfWinActive, " + WinTitle + "," + WinText + "," + ExcludeTitle + "," + ExcludeText + @" 
                    OutputVar = true
                ";

            ErrorLog_Setup(false); // ErrorLevel Detection Enabled for this function in AHK 
            string OutputVar = Execute(ahkString, "OutputVar");   // execute AHK code and return variable value

            // return bool value indicating if window was found to be active
            bool ReturnVal = false;
            if (OutputVar == "true") { ReturnVal = true; }
            return ReturnVal;
        }

        /// <summary>Checks if the specified window exists and is currently active (foremost).</summary>
        /// <param name="WinTitle">The title or partial title of the target window (the matching behavior is determined by SetTitleMatchMode). If this and the other 3 parameters are omitted, the Last Found Window will be used. To use a window class, specify ahk_class ExactClassName (shown by Window Spy). To use a process identifier (PID), specify ahk_pid %VarContainingPID%. To use a window group, specify ahk_group GroupName. To use a window's unique ID number, specify ahk_id %VarContainingID%. The search can be narrowed by specifying multiple criteria. For example: My File.txt ahk_class Notepad</param>
        /// <param name="WinText">If present, this parameter must be a substring from a single text element of the target window (as revealed by the included Window Spy utility). Hidden text elements are detected if DetectHiddenText is ON.</param>
        /// <param name="ExcludeTitle">Windows whose titles include this value will not be considered. Note: Due to backward compatibility with .aut scripts, this parameter will be interpreted as a command if it exactly matches the name of a command. To work around this, use the WinActive() function instead.</param>
        /// <param name="ExcludeText">Windows whose text include this value will not be considered.</param>
        public bool WinNotActive(string WinTitle = "", string WinText = "", string ExcludeTitle = "", string ExcludeText = "")
        {
            string ahkString = @"
                OutputVar = false
                IfWinNotActive, " + WinTitle + "," + WinText + "," + ExcludeTitle + "," + ExcludeText + @" 
                    OutputVar = true
                ";

            ErrorLog_Setup(false); // ErrorLevel Detection Enabled for this function in AHK 
            string OutputVar = Execute(ahkString, "OutputVar");   // execute AHK code and return variable value

            // return bool value indicating if window was found to be active
            bool ReturnVal = false; if (OutputVar == "true") { ReturnVal = true; }
            return ReturnVal;
        }

        /// <summary>Checks if the specified window exists.</summary>
        /// <param name="WinTitle">The title or partial title of the target window (the matching behavior is determined by SetTitleMatchMode). If this and the other 3 parameters are omitted, the Last Found Window will be used. To use a window class, specify ahk_class ExactClassName (shown by Window Spy). To use a process identifier (PID), specify ahk_pid %VarContainingPID%. To use a window group, specify ahk_group GroupName. To use a window's unique ID number, specify ahk_id %VarContainingID%. The search can be narrowed by specifying multiple criteria. For example: My File.txt ahk_class Notepad</param>
        /// <param name="WinText">If present, this parameter must be a substring from a single text element of the target window (as revealed by the included Window Spy utility). Hidden text elements are detected if DetectHiddenText is ON.</param>
        /// <param name="ExcludeTitle">Windows whose titles include this value will not be considered. Note: Due to backward compatibility with .aut scripts, this parameter will be interpreted as a command if it exactly matches the name of a command. To work around this, use the WinActive() function instead.</param>
        /// <param name="ExcludeText">Windows whose text include this value will not be considered.</param>
        public bool WinExist(string WinTitle = "", string WinText = "", string ExcludeTitle = "", string ExcludeText = "")
        {
            string ahkString = @"
                OutputVar = false
                IfWinExist, " + WinTitle + "," + WinText + "," + ExcludeTitle + "," + ExcludeText + @" 
                    OutputVar = true
                ";

            ErrorLog_Setup(false); // ErrorLevel Detection Enabled for this function in AHK 
            string OutputVar = Execute(ahkString, "OutputVar");   // execute AHK code and return variable value

            // return bool value indicating if window exists
            bool ReturnVal = false; if (OutputVar == "true") { ReturnVal = true; }
            return ReturnVal;

            // v1 ToDo
            /*
                    public bool _WinExist(string WinTitle)  // returns true/false if the window title currently exists
                    {
                        //=== Create List of All Window Titles ===============================

                        Dictionary<string, string> WinList = new Dictionary<string, string>();

                        WinList.Clear();

                        Process[] processlist = Process.GetProcesses();

                        foreach (Process process in processlist)
                        {
                            if (!String.IsNullOrEmpty(process.MainWindowTitle))
                            {
                                //Console.WriteLine("Process: {0} ID: {1} Window title: {2}", process.ProcessName, process.Id, process.MainWindowTitle);
                                //MsgBox("ProcessName: " + process.ProcessName + Environment.NewLine + "ProcessID: " + process.Id + Environment.NewLine + "WinTitle: " + process.MainWindowTitle);

                                // add to list if not already in the list
                                if (!WinList.ContainsKey(process.MainWindowTitle))
                                {
                                    WinList.Add(process.MainWindowTitle, process.MainWindowTitle);
                                }
                            }
                        }

                        //=== Check to see if User's Window Title is in List of Window Titles
                        if (WinList.ContainsKey(WinTitle))
                        {
                            return true;
                        }

                        return false;
                    }
            */

        }

        /// <summary>Checks if the specified window exists.</summary>
        /// <param name="WinTitle">The title or partial title of the target window (the matching behavior is determined by SetTitleMatchMode). If this and the other 3 parameters are omitted, the Last Found Window will be used. To use a window class, specify ahk_class ExactClassName (shown by Window Spy). To use a process identifier (PID), specify ahk_pid %VarContainingPID%. To use a window group, specify ahk_group GroupName. To use a window's unique ID number, specify ahk_id %VarContainingID%. The search can be narrowed by specifying multiple criteria. For example: My File.txt ahk_class Notepad</param>
        /// <param name="WinText">If present, this parameter must be a substring from a single text element of the target window (as revealed by the included Window Spy utility). Hidden text elements are detected if DetectHiddenText is ON.</param>
        /// <param name="ExcludeTitle">Windows whose titles include this value will not be considered. Note: Due to backward compatibility with .aut scripts, this parameter will be interpreted as a command if it exactly matches the name of a command. To work around this, use the WinActive() function instead.</param>
        /// <param name="ExcludeText">Windows whose text include this value will not be considered.</param>
        public bool WinNotExist(string WinTitle = "", string WinText = "", string ExcludeTitle = "", string ExcludeText = "")
        {
            string ahkString = @"
                OutputVar = false
                IfWinNotExist, " + WinTitle + "," + WinText + "," + ExcludeTitle + "," + ExcludeText + @" 
                    OutputVar = true
                ";

            ErrorLog_Setup(false); // ErrorLevel Detection Enabled for this function in AHK 
            string OutputVar = Execute(ahkString, "OutputVar");   // execute AHK code and return variable value

            // return bool value indicating if window exists
            bool ReturnVal = false; if (OutputVar == "true") { ReturnVal = true; }
            return ReturnVal;
        }

        /// <summary>Sets the matching behavior of the WinTitle parameter in commands such as WinWait.</summary>
        /// <param name="Mode">One of the following digits or the word RegEx: 1: A window's title must start with the specified WinTitle to be a match. 2: A window's title can contain WinTitle anywhere inside it to be a match. 3: A window's title must exactly match WinTitle to be a match.</param>
        public void SetTitleMatchMode(string Mode)
        {
            string AHKLine = "SetTitleMatchMode, " + Mode;  // ahk line to execute
            ErrorLog_Setup(false); // ErrorLevel Detection Enabled for this function in AHK 
            Execute(AHKLine);   // execute AHK code and return variable value 
        }

        /// <summary>Sets the delay that will occur after each windowing command, such as WinActivate.</summary>
        /// <param name="Delay">Time in milliseconds, which can be an expression. Use -1 for no delay at all and 0 for the smallest possible delay. If unset, the default delay is 100.</param>
        public void SetWinDelay(string Delay)
        {
            string AHKLine = "SetWinDelay, " + Delay;  // ahk line to execute
            ErrorLog_Setup(false); // ErrorLevel Detection Enabled for this function in AHK 
            Execute(AHKLine);   // execute AHK code and return variable value 
        }

        /// <summary>Retrieves the text from a standard status bar control.</summary>
        /// <param name="PartNum">Which part number of the bar to retrieve, which can be an expression. Default 1, which is usually the part that contains the text of interest.</param>
        /// <param name="WinTitle">The title or partial title of the target window (the matching behavior is determined by SetTitleMatchMode). If this and the other 3 parameters are omitted, the Last Found Window will be used. To use a window class, specify ahk_class ExactClassName (shown by Window Spy). To use a process identifier (PID), specify ahk_pid %VarContainingPID%. To use a window group, specify ahk_group GroupName. To use a window's unique ID number, specify ahk_id %VarContainingID%. The search can be narrowed by specifying multiple criteria. For example: My File.txt ahk_class Notepad</param>
        /// <param name="WinText">If present, this parameter must be a substring from a single text element of the target window (as revealed by the included Window Spy utility). Hidden text elements are detected if DetectHiddenText is ON.</param>
        /// <param name="ExcludeTitle">Windows whose titles include this value will not be considered. Note: Due to backward compatibility with .aut scripts, this parameter will be interpreted as a command if it exactly matches the name of a command. To work around this, use the WinActive() function instead.</param>
        /// <param name="ExcludeText">Windows whose text include this value will not be considered.</param>
        public string StatusBarGetText(string PartNum = "", string WinTitle = "", string WinText = "", string ExcludeTitle = "", string ExcludeText = "")
        {
            string AHKLine = "StatusBarGetText, OutputVar, " + PartNum + "," + WinTitle + "," + WinText + "," + ExcludeTitle + "," + ExcludeText;  // ahk line to execute
            ErrorLog_Setup(false); // ErrorLevel Detection Enabled for this function in AHK 
            string OutVar = Execute(AHKLine, "OutputVar");   // execute AHK code and return variable value 
            return OutVar;

            // v1 ToDo

            /*
                    public string StatusBarGetText(int Part = 1, object WinTitle = null, string WinText = "", string ExcludeTitle = "", string ExcludeText = "")  // returns text from statusbar control on another application
                    {
                        IntPtr hWnd = _WinHwnd(WinTitle);  //returns Window Handle (from either handle or window title)

                        if (ahkGlobal.ahkdll == null) { ahkGlobal.ahkdll = new AutoHotkey.Interop.AutoHotkeyEngine(); }
                        var ahkdll = ahkGlobal.ahkdll;

                        ahkdll.SetVar("OutputText", "");
                        string Command = "StatusBarGetText, OutputText," + Part + ", ahk_id " + hWnd.ToString() + "," + WinText + "," + ExcludeTitle + "," + ExcludeText;
                        ahkdll.ExecRaw(Command);
                        string OutputText = ahkdll.GetVar("OutputText");


                        //   Error Handling
                        ahkGlobal.ErrorLevelMsg = "There Was A Problem Reading StatusBar";
                        ahkGlobal.ErrorLevel = false;
                        string ErrorLevelValue = ahkdll.GetVar("ErrorLevel");
                        if (ErrorLevelValue == "0") { ahkGlobal.ErrorLevelMsg = ""; }
                        if (ErrorLevelValue == "1") { ahkGlobal.ErrorLevel = true; }
                        if (ahkGlobal.Debug) { if (ahkGlobal.ErrorLevel) { MsgBox(ahkGlobal.ErrorLevelMsg, "ErrorLevel"); } } //Display Error Messages in Debug Mode


                        return OutputText;
                    }
            */

        }

        /// <summary>Waits until a window's status bar contains the specified string.</summary>
        /// <param name="BarText">The text or partial text for the which the command will wait to appear. Default is blank (empty), which means to wait for the status bar to become blank. The text is case sensitive and the matching behavior is determined by SetTitleMatchMode, similar to WinTitle below. To instead wait for the bar's text to change, either use StatusBarGetText in a loop, or use the RegEx example at the bottom of this page.</param>
        /// <param name="Seconds">The number of seconds (can contain a decimal point or be an expression) to wait before timing out, in which case ErrorLevel will be set to 1. Default is blank, which means wait indefinitely. Specifying 0 is the same as specifying 0.5.</param>
        /// <param name="PartNum">Which part number of the bar to retrieve, which can be an expression. Default 1, which is usually the part that contains the text of interest.</param>
        /// <param name="WinTitle">The title or partial title of the target window (the matching behavior is determined by SetTitleMatchMode). If this and the other 3 parameters are omitted, the Last Found Window will be used. To use a window class, specify ahk_class ExactClassName (shown by Window Spy). To use a process identifier (PID), specify ahk_pid %VarContainingPID%. To use a window group, specify ahk_group GroupName. To use a window's unique ID number, specify ahk_id %VarContainingID%. The search can be narrowed by specifying multiple criteria. For example: My File.txt ahk_class Notepad</param>
        /// <param name="WinText">If present, this parameter must be a substring from a single text element of the target window (as revealed by the included Window Spy utility). Hidden text elements are detected if DetectHiddenText is ON.</param>
        /// <param name="ExcludeTitle">Windows whose titles include this value will not be considered. Note: Due to backward compatibility with .aut scripts, this parameter will be interpreted as a command if it exactly matches the name of a command. To work around this, use the WinActive() function instead.</param>
        /// <param name="ExcludeText">Windows whose text include this value will not be considered.</param>
        public bool StatusBarWait(string BarText = "", int Seconds = 10, string PartNum = "", string WinTitle = "", string WinText = "", string ExcludeTitle = "", string ExcludeText = "")
        {
            string AHKLine = "StatusBarWait, " + BarText + "," + Seconds.ToString() + "," + PartNum + "," + WinTitle + "," + WinText + "," + ExcludeTitle + "," + ExcludeText;  // ahk line to execute
            ErrorLog_Setup(true); // ErrorLevel Detection Enabled for this function in AHK 
            Execute(AHKLine);   // execute AHK code and return variable value

            if (!ahkGlobal.ErrorLevel) { return true; } // no error level - return true for success
            return false;  // error level detected - success = false


            // v1 ToDo
            /*
                    public bool _StatusBarWait(string BarText = "", int Seconds = 5, int Part = 1, object WinTitle = null, string WinText = "", int Interval = 50, string ExcludeTitle = "", string ExcludeText = "")  // waits until statusbar text contains string before continuing
                    {
                        IntPtr hWnd = _WinHwnd(WinTitle);  //returns Window Handle (from either handle or window title)
                        if (ahkGlobal.ahkdll == null) { ahkGlobal.ahkdll = new AutoHotkey.Interop.AutoHotkeyEngine(); }
                        var ahkdll = ahkGlobal.ahkdll;

                        string Command = "StatusBarWait," + BarText + "," + Seconds + "," + Part + ", ahk_id " + hWnd.ToString() + "," + WinText + "," + Interval + "," + ExcludeTitle + "," + ExcludeText;
                        ahkdll.ExecRaw(Command);

                        //   Error Handling
                        ahkGlobal.ErrorLevel = false;
                        string ErrorLevelValue = ahkdll.GetVar("ErrorLevel");
                        if (ErrorLevelValue == "0")
                        { ahkGlobal.ErrorLevelMsg = ""; ahkGlobal.ErrorLevel = false; }
                        if (ErrorLevelValue == "1")
                        { ahkGlobal.ErrorLevelMsg = "StatusBarWait Timed Out"; ahkGlobal.ErrorLevel = true; }
                        if (ErrorLevelValue == "2")
                        { ahkGlobal.ErrorLevelMsg = "StatusBarWait Could Not Access StatusBar"; ahkGlobal.ErrorLevel = true; }

                        if (ahkGlobal.Debug) { if (ahkGlobal.ErrorLevel) { MsgBox(ahkGlobal.ErrorLevelMsg, "ErrorLevel"); } } //Display Error Messages in Debug Mode


                        bool Success = false;
                        if (!ahkGlobal.ErrorLevel) { Success = true; }

                        return Success;
                    }
            */

        }

        /// <summary>Activates the specified window (makes it foremost).</summary>
        /// <param name="WinTitle">The title or partial title of the target window (the matching behavior is determined by SetTitleMatchMode). If this and the other 3 parameters are omitted, the Last Found Window will be used. To use a window class, specify ahk_class ExactClassName (shown by Window Spy). To use a process identifier (PID), specify ahk_pid %VarContainingPID%. To use a window group, specify ahk_group GroupName. To use a window's unique ID number, specify ahk_id %VarContainingID%. The search can be narrowed by specifying multiple criteria. For example: My File.txt ahk_class Notepad</param>
        /// <param name="WinText">If present, this parameter must be a substring from a single text element of the target window (as revealed by the included Window Spy utility). Hidden text elements are detected if DetectHiddenText is ON.</param>
        /// <param name="ExcludeTitle">Windows whose titles include this value will not be considered. Note: Due to backward compatibility with .aut scripts, this parameter will be interpreted as a command if it exactly matches the name of a command. To work around this, use the WinActive() function instead.</param>
        /// <param name="ExcludeText">Windows whose text include this value will not be considered.</param>
        public void WinActivate(string WinTitle = "", string WinText = "", string ExcludeTitle = "", string ExcludeText = "")
        {
            ErrorLog_Setup(false);
            Execute("WinActivate, " + WinTitle + "," + WinText + "," + ExcludeTitle + "," + ExcludeText);

            // v1 ToDo

            //IntPtr hWnd = WinHwnd(WinTitle);  //returns Window Handle (from either handle or window title)
            //// Activate by Process Name
            //if (!hWnd.Equals(IntPtr.Zero))
            //{
            //    SetForegroundWindow(hWnd);
            //}

        }

        /// <summary>Same as WinActivate except that it activates the bottommost (least recently active) matching window rather than the topmost.</summary>
        /// <param name="WinTitle">The title or partial title of the target window (the matching behavior is determined by SetTitleMatchMode). If this and the other 3 parameters are omitted, the Last Found Window will be used. To use a window class, specify ahk_class ExactClassName (shown by Window Spy). To use a process identifier (PID), specify ahk_pid %VarContainingPID%. To use a window group, specify ahk_group GroupName. To use a window's unique ID number, specify ahk_id %VarContainingID%. The search can be narrowed by specifying multiple criteria. For example: My File.txt ahk_class Notepad</param>
        /// <param name="WinText">If present, this parameter must be a substring from a single text element of the target window (as revealed by the included Window Spy utility). Hidden text elements are detected if DetectHiddenText is ON.</param>
        /// <param name="ExcludeTitle">Windows whose titles include this value will not be considered. Note: Due to backward compatibility with .aut scripts, this parameter will be interpreted as a command if it exactly matches the name of a command. To work around this, use the WinActive() function instead.</param>
        /// <param name="ExcludeText">Windows whose text include this value will not be considered.</param>
        public void WinActivateBottom(string WinTitle = "", string WinText = "", string ExcludeTitle = "", string ExcludeText = "")
        {
            ErrorLog_Setup(false);
            Execute("WinActivateBottom, " + WinTitle + "," + WinText + "," + ExcludeTitle + "," + ExcludeText);
        }

        /// <summary>Closes the specified window.</summary>
        /// <param name="WinTitle">The title or partial title of the target window (the matching behavior is determined by SetTitleMatchMode). If this and the other 3 parameters are omitted, the Last Found Window will be used. To use a window class, specify ahk_class ExactClassName (shown by Window Spy). To use a process identifier (PID), specify ahk_pid %VarContainingPID%. To use a window group, specify ahk_group GroupName. To use a window's unique ID number, specify ahk_id %VarContainingID%. The search can be narrowed by specifying multiple criteria. For example: My File.txt ahk_class Notepad</param>
        /// <param name="WinText">If present, this parameter must be a substring from a single text element of the target window (as revealed by the included Window Spy utility). Hidden text elements are detected if DetectHiddenText is ON.</param>
        /// <param name="SecondsToWait">If omitted or blank, the command will not wait at all. If 0, it will wait 500ms. Otherwise, it will wait the indicated number of seconds (can contain a decimal point or be an expression) for the window to close. If the window does not close within that period, the script will continue. ErrorLevel is not set by this command, so use IfWinExist or WinWaitClose if you need to determine for certain that a window is closed. While the command is in a waiting state, new threads can be launched via hotkey, custom menu item, or timer.</param>
        /// <param name="ExcludeTitle">Windows whose titles include this value will not be considered. Note: Due to backward compatibility with .aut scripts, this parameter will be interpreted as a command if it exactly matches the name of a command. To work around this, use the WinActive() function instead.</param>
        /// <param name="ExcludeText">Windows whose text include this value will not be considered.</param>
        public void WinClose(string WinTitle = "", string WinText = "", string SecondsToWait = "", string ExcludeTitle = "", string ExcludeText = "")
        {
            ErrorLog_Setup(false);
            Execute("WinClose, " + WinTitle + "," + WinText + "," + SecondsToWait + "," + ExcludeTitle + "," + ExcludeText);

            // v1 ToDo
            /*
                        IntPtr hWnd = _WinHwnd(WinTitle);  //returns Window Handle (from either handle or window title)
                        _SendMessage(hWnd, WM_CLOSE, IntPtr.Zero, IntPtr.Zero);
             */
        }

        /// <summary>Waits until the specified window does not exist.</summary>
        /// <param name="WinTitle">The title or partial title of the target window (the matching behavior is determined by SetTitleMatchMode). If this and the other 3 parameters are omitted, the Last Found Window will be used. To use a window class, specify ahk_class ExactClassName (shown by Window Spy). To use a process identifier (PID), specify ahk_pid %VarContainingPID%. To use a window group, specify ahk_group GroupName. To use a window's unique ID number, specify ahk_id %VarContainingID%. The search can be narrowed by specifying multiple criteria. For example: My File.txt ahk_class Notepad</param>
        /// <param name="WinText">If present, this parameter must be a substring from a single text element of the target window (as revealed by the included Window Spy utility). Hidden text elements are detected if DetectHiddenText is ON.</param>
        /// <param name="Seconds">How many seconds to wait before timing out and setting ErrorLevel to 1. Leave blank to wait indefinitely. Specifying 0 is the same as specifying 0.5. This parameter can be an expression.</param>
        /// <param name="ExcludeTitle">Windows whose titles include this value will not be considered. Note: Due to backward compatibility with .aut scripts, this parameter will be interpreted as a command if it exactly matches the name of a command. To work around this, use the WinActive() function instead.</param>
        /// <param name="ExcludeText">Windows whose text include this value will not be considered.</param>
        public bool WinWaitClose(string WinTitle = "", string WinText = "", string Seconds = "", string ExcludeTitle = "", string ExcludeText = "")
        {
            string AHKLine = "WinWaitClose, " + WinTitle + "," + WinText + "," + Seconds + "," + ExcludeTitle + "," + ExcludeText;  // ahk line to execute
            ErrorLog_Setup(true); // ErrorLevel Detection Enabled for this function in AHK 
            Execute(AHKLine);   // execute AHK code and return variable value

            if (!ahkGlobal.ErrorLevel) { return true; } // no error level - return true for success
            return false;  // error level detected - success = false

            // v1 ToDo
            /*            
                   //public bool WinWaitClose(object WinTitle = null, string WinText = "", int Seconds = 5, string ExcludeTitle = "", string ExcludeText = "")  // waits for windows to close (pass wintitle or handle)
                    {
                        IntPtr hWnd = _WinHwnd(WinTitle);  //returns Window Handle (from either handle or window title)
                        if (ahkGlobal.ahkdll == null) { ahkGlobal.ahkdll = new AutoHotkey.Interop.AutoHotkeyEngine(); }
                        var ahkdll = ahkGlobal.ahkdll;

                        string Command = "WinWaitClose, ahk_id " + hWnd + "," + WinText + "," + Seconds + "," + ExcludeTitle + "," + ExcludeText;
                        ahkdll.ExecRaw(Command);



                        //   Error Handling
                        ahkGlobal.ErrorLevelMsg = "WinWaitClose Timed Out (" + Seconds + " Seconds - " + WinTitle + " Still Running)";
                        ahkGlobal.ErrorLevel = false;
                        string ErrorLevelValue = ahkdll.GetVar("ErrorLevel");
                        if (ErrorLevelValue == "0") { ahkGlobal.ErrorLevelMsg = ""; }
                        if (ErrorLevelValue == "1") { ahkGlobal.ErrorLevel = true; }
                        if (ahkGlobal.Debug) { if (ahkGlobal.ErrorLevel) { MsgBox(ahkGlobal.ErrorLevelMsg, "ErrorLevel"); } } //Display Error Messages in Debug Mode


                        if (ahkGlobal.ErrorLevel == false) { return true; }

                        return false;
                    }

            */

        }

        /// <summary>Retrieves the specified window's unique ID, process ID, process name, or a list of its controls. It can also retrieve a list of all windows matching the specified criteria.</summary>
        /// <param name="Cmd">ID | IDLast | PID | ProcessName | Count | List | MinMax | ControlList | ControlListHwnd | Transparent | Transcolor | Style | ExStyle</param>
        /// <param name="WinTitle">The title or partial title of the target window (the matching behavior is determined by SetTitleMatchMode). If this and the other 3 parameters are omitted, the Last Found Window will be used. To use a window class, specify ahk_class ExactClassName (shown by Window Spy). To use a process identifier (PID), specify ahk_pid %VarContainingPID%. To use a window group, specify ahk_group GroupName. To use a window's unique ID number, specify ahk_id %VarContainingID%. The search can be narrowed by specifying multiple criteria. For example: My File.txt ahk_class Notepad</param>
        /// <param name="WinText">If present, this parameter must be a substring from a single text element of the target window (as revealed by the included Window Spy utility). Hidden text elements are detected if DetectHiddenText is ON.</param>
        /// <param name="ExcludeTitle">Windows whose titles include this value will not be considered. Note: Due to backward compatibility with .aut scripts, this parameter will be interpreted as a command if it exactly matches the name of a command. To work around this, use the WinActive() function instead.</param>
        /// <param name="ExcludeText">Windows whose text include this value will not be considered.</param>
        public string _WinGet(string Cmd = "", string WinTitle = "", string WinText = "", string ExcludeTitle = "", string ExcludeText = "")
        {
            string AHKLine = "WinGet, OutputVar, " + Cmd + "," + WinTitle + "," + WinText + "," + ExcludeTitle + "," + ExcludeText;  // ahk line to execute
            ErrorLog_Setup(false); // ErrorLevel Detection Enabled for this function in AHK 
            string OutVar = Execute(AHKLine, "OutputVar");   // execute AHK code and return variable value 
            return OutVar;
        }

        public string WinGet(WinGetCmd Cmd, string WinTitle = "", string WinText = "", string ExcludeTitle = "", string ExcludeText = "")
        {
            string AHKLine = "WinGet, OutputVar, " + Cmd.ToString() + "," + WinTitle + "," + WinText + "," + ExcludeTitle + "," + ExcludeText;  // ahk line to execute
            ErrorLog_Setup(false); // ErrorLevel Detection Enabled for this function in AHK 
            string OutVar = Execute(AHKLine, "OutputVar");   // execute AHK code and return variable value 
            return OutVar;
        }

        /// <summary>
        /// Options for WinGet Command
        /// </summary>
        public enum WinGetCmd
        {
            /// <summary>
            /// Retrieves the unique ID number (HWND/handle) of a window. If there is no matching window, OutputVar is made blank. The functions WinExist() and WinActive() can also be used to retrieve the ID of a window; for example, WinExist("A") is a fast way to get the ID of the active window. To discover the HWND of a control (for use with Post/SendMessage or DllCall), use ControlGet Hwnd or MouseGetPos.
            /// </summary>
            ID,
            /// <summary>
            /// Same as above except it retrieves the ID of the last/bottommost window if there is more than one match. If there is only one match, it performs identically to ID. This concept is similar to that used by WinActivateBottom.
            /// </summary>
            IDLast,
            /// <summary>
            /// Retrieves the Process ID (PID) of a window.
            /// </summary>
            PID,
            /// <summary>
            /// Retrieves the name of the process (e.g. notepad.exe) that owns a window. If there are no matching windows, OutputVar is made blank.
            /// </summary>
            ProcessName,
            /// <summary>
            /// Retrieves the number of existing windows that match the specified WinTitle, WinText, ExcludeTitle, and ExcludeText (0 if none). To count all windows on the system, omit all four title/text parameters. Hidden windows are included only if DetectHiddenWindows has been turned on. 
            /// </summary>
            Count,
            /// <summary>
            /// Retrieves the unique ID numbers of all existing windows that match the specified WinTitle, WinText, ExcludeTitle, and ExcludeText (to retrieve all windows on the entire system, omit all four title/text parameters). Each ID number is stored in an array element whose name begins with OutputVar's own name, while OutputVar itself is set to the number of retrieved items (0 if none). For example, if OutputVar is MyArray and two matching windows are discovered, MyArray1 will be set to the ID of the first window, MyArray2 will be set to the ID of the second window, and MyArray itself will be set to the number 2. Windows are retrieved in order from topmost to bottommost (according to how they are stacked on the desktop). Hidden windows are included only if DetectHiddenWindows has been turned on. Within a function, to create an array that is global instead of local, declare MyArray as a global variable prior to using this command (the converse is true for assume-global functions).
            /// </summary>
            List,
            /// <summary>
            /// Retrieves the minimized/maximized state for a window. OuputVar is made blank if no matching window exists; otherwise, it is set to one of the following numbers:
            /// -1: The window is minimized (WinRestore can unminimize it). 
            /// 1: The window is maximized(WinRestore can unmaximize it).
            /// 0: The window is neither minimized nor maximized.
            /// </summary>
            MinMax,
            /// <summary>
            /// Retrieves the control names for all controls in a window. If no matching window exists or there are no controls in the window, OutputVar is made blank. Otherwise, each control name consists of its class name followed immediately by its sequence number (ClassNN), as shown by Window Spy.
            /// Each item except the last is terminated by a linefeed (`n). To examine the individual control names one by one, use a parsing loop as shown in the examples section below.
            /// Controls are sorted according to their Z-order, which is usually the same order as TAB key navigation if the window supports tabbing.
            /// The control currently under the mouse cursor can be retrieved via MouseGetPos.
            /// </summary>
            ControlList,
            /// <summary>
            /// Same as above except it retrieves the window handle (HWND) of each control rather than its ClassNN.
            /// </summary>
            ControlListHwnd,
            /// <summary>
            /// Retrieves the degree of transparency of a window (see WinSet for how to set transparency). OutputVar is made blank if: 1) the OS is older than Windows XP; 2) there are no matching windows; 3) the window has no transparency level; or 4) other conditions (caused by OS behavior) such as the window having been minimized, restored, and/or resized since it was made transparent. Otherwise, a number between 0 and 255 is stored, where 0 indicates an invisible window and 255 indicates an opaque window. 
            /// </summary>
            Transparent,
            /// <summary>
            /// Retrieves the color that is marked transparent in a window (see WinSet for how to set the TransColor). OutputVar is made blank if: 1) the OS is older than Windows XP; 2) there are no matching windows; 3) the window has no transparent color; or 4) other conditions (caused by OS behavior) such as the window having been minimized, restored, and/or resized since it was made transparent. Otherwise, a six-digit hexadecimal RGB color is stored, e.g. 0x00CC99.
            /// </summary>
            TransColor,
            /// <summary>
            /// Retrieves an 8-digit hexadecimal number representing style or extended style (respectively) of a window. If there are no matching windows, OutputVar is made blank. The following example determines whether a window has the WS_DISABLED style:
            /// </summary>
            Style,
            /// <summary>
            /// Retrieves an 8-digit hexadecimal number representing style or extended style (respectively) of a window. If there are no matching windows, OutputVar is made blank. The following example determines whether a window has the WS_DISABLED style:
            /// </summary>
            ExStyle
        }

        /// <summary>
        /// Returns List of WinGet Commands (WinGetCmd)
        /// </summary>
        /// <returns>Returns List of WinGet Commands (WinGetCmd)</returns>
        public List<string> Commands_WinGetCmd()
        {
            List<string> rlist = new List<string>();
            Array list = Enum.GetValues(typeof(_AHK.WinGetCmd));
            foreach (var item in list) { rlist.Add(item.ToString()); }
            return rlist;
        }

        /// <summary>Retrieves the specified window's unique AHK ID</summary>
        /// <param name="WinTitle">The title or partial title of the target window (the matching behavior is determined by SetTitleMatchMode). If this and the other 3 parameters are omitted, the Last Found Window will be used. To use a window class, specify ahk_class ExactClassName (shown by Window Spy). To use a process identifier (PID), specify ahk_pid %VarContainingPID%. To use a window group, specify ahk_group GroupName. To use a window's unique ID number, specify ahk_id %VarContainingID%. The search can be narrowed by specifying multiple criteria. For example: My File.txt ahk_class Notepad</param>
        /// <param name="WinText">If present, this parameter must be a substring from a single text element of the target window (as revealed by the included Window Spy utility). Hidden text elements are detected if DetectHiddenText is ON.</param>
        /// <param name="ExcludeTitle">Windows whose titles include this value will not be considered. Note: Due to backward compatibility with .aut scripts, this parameter will be interpreted as a command if it exactly matches the name of a command. To work around this, use the WinActive() function instead.</param>
        /// <param name="ExcludeText">Windows whose text include this value will not be considered.</param>
        public string WinGetID(string WinTitle = "", string WinText = "", string ExcludeTitle = "", string ExcludeText = "")
        {
            string AHKLine = "WinGet, OutputVar, ID," + WinTitle + "," + WinText + "," + ExcludeTitle + "," + ExcludeText;  // ahk line to execute
            ErrorLog_Setup(false); // ErrorLevel Detection Enabled for this function in AHK 
            string OutVar = Execute(AHKLine, "OutputVar");   // execute AHK code and return variable value   
            OutVar = "ahk_ID " + OutVar;
            return OutVar;
        }

        /// <summary>Retrieves the specified window's Process ID</summary>
        /// <param name="WinTitle">The title or partial title of the target window (the matching behavior is determined by SetTitleMatchMode). If this and the other 3 parameters are omitted, the Last Found Window will be used. To use a window class, specify ahk_class ExactClassName (shown by Window Spy). To use a process identifier (PID), specify ahk_pid %VarContainingPID%. To use a window group, specify ahk_group GroupName. To use a window's unique ID number, specify ahk_id %VarContainingID%. The search can be narrowed by specifying multiple criteria. For example: My File.txt ahk_class Notepad</param>
        /// <param name="WinText">If present, this parameter must be a substring from a single text element of the target window (as revealed by the included Window Spy utility). Hidden text elements are detected if DetectHiddenText is ON.</param>
        /// <param name="ExcludeTitle">Windows whose titles include this value will not be considered. Note: Due to backward compatibility with .aut scripts, this parameter will be interpreted as a command if it exactly matches the name of a command. To work around this, use the WinActive() function instead.</param>
        /// <param name="ExcludeText">Windows whose text include this value will not be considered.</param>
        public string WinGetPID(string WinTitle = "", string WinText = "", string ExcludeTitle = "", string ExcludeText = "")
        {
            string AHKLine = "WinGet, OutputVar, PID," + WinTitle + "," + WinText + "," + ExcludeTitle + "," + ExcludeText;  // ahk line to execute
            ErrorLog_Setup(false); // ErrorLevel Detection Enabled for this function in AHK 
            string OutVar = Execute(AHKLine, "OutputVar");   // execute AHK code and return variable value 
            OutVar = "ahk_PID " + OutVar;
            return OutVar;
        }

        /// <summary>
        /// Returns the Active Window Handle
        /// </summary>
        /// <param name="ahkMethod">Option to switch between AHK Method and C# (C# ver Returned Matching ID Used in Win Functions)</param>
        /// <returns>Returns Handle of Active Window</returns>
        public string WinGetActiveID(bool ahkMethod = false)
        {
            string winHandle = "";

            if (ahkMethod)
            {
                winHandle = "ahk_ID " + WinGet(_AHK.WinGetCmd.ID, WinGetActiveTitle(true));
                return winHandle;
            }

            // Method #2 (Seems to work better with Window Interaction Functions)

            const int nChars = 256;
            StringBuilder Buff = new StringBuilder(nChars);
            IntPtr handle = GetForegroundWindow();
            winHandle = "ahk_ID " + handle.ToString();
            return winHandle;
        }

        /// <summary>Combines the functions of WinGetActiveTitle and WinGetPos into one command.</summary>
        /// <returns>Returns winInfo Object with Window Dimentions etc</returns>
        public winInfo WinGetActiveStats()
        {
            string AHKLine = "WinGetActiveStats, Title, Width, Height, X, Y";  // ahk line to execute
            ErrorLog_Setup(false); // ErrorLevel Detection Enabled for this function in AHK 
            Execute(AHKLine);   // execute AHK code and return variable value             

            //return value from ahk session
            string W = GetVar("Width");
            string H = GetVar("Height");
            string X = GetVar("X");
            string Y = GetVar("Y");

            winInfo wI = new winInfo();   // define object reference
            wI.WinTitle = GetVar("Title");
            wI.WinX = Int32.Parse(X);
            wI.WinY = Int32.Parse(Y);
            wI.WinW = Int32.Parse(W);
            wI.WinH = Int32.Parse(H);

            return wI;
        }

        /// <summary>Retrieves the title of the active window.</summary>
        public string WinGetActiveTitle(bool ahkMethod = true)
        {
            //bool ahkMethod = true;

            if (ahkMethod)
            {
                string AHKLine = "WinGetActiveTitle, OutputVar";  // ahk line to execute
                ErrorLog_Setup(false); // ErrorLevel Detection Enabled for this function in AHK 
                string OutVar = Execute(AHKLine, "OutputVar");   // execute AHK code and return variable value 
                return OutVar;
            }


            //Returns Active Window Title as String
            try
            {
                const int nChars = 256;
                StringBuilder Buff = new StringBuilder(nChars);
                IntPtr handle = GetForegroundWindow();
                if (GetWindowText(handle, Buff, nChars) > 0) { return Buff.ToString(); }
            }
            catch(Exception ex) { MsgBox(ex.ToString()); }

            return null;
        }

        /// <summary>Retrieves the specified window's class name.</summary>
        /// <param name="WinTitle">The title or partial title of the target window (the matching behavior is determined by SetTitleMatchMode). If this and the other 3 parameters are omitted, the Last Found Window will be used. To use a window class, specify ahk_class ExactClassName (shown by Window Spy). To use a process identifier (PID), specify ahk_pid %VarContainingPID%. To use a window group, specify ahk_group GroupName. To use a window's unique ID number, specify ahk_id %VarContainingID%. The search can be narrowed by specifying multiple criteria. For example: My File.txt ahk_class Notepad</param>
        /// <param name="WinText">If present, this parameter must be a substring from a single text element of the target window (as revealed by the included Window Spy utility). Hidden text elements are detected if DetectHiddenText is ON.</param>
        /// <param name="ExcludeTitle">Windows whose titles include this value will not be considered. Note: Due to backward compatibility with .aut scripts, this parameter will be interpreted as a command if it exactly matches the name of a command. To work around this, use the WinActive() function instead.</param>
        /// <param name="ExcludeText">Windows whose text include this value will not be considered.</param>
        public string WinGetClass(string WinTitle = "", string WinText = "", string ExcludeTitle = "", string ExcludeText = "")
        {
            string AHKLine = "WinGetClass, OutputVar, " + WinTitle + "," + WinText + "," + ExcludeTitle + "," + ExcludeText;  // ahk line to execute
            ErrorLog_Setup(false); // ErrorLevel Detection Enabled for this function in AHK 
            string OutVar = Execute(AHKLine, "OutputVar");   // execute AHK code and return variable value 
            return OutVar;

            // v1 ToDo
            /*
                    private string _WinGetClass(object WinTitle)  // returns Class Name from Handle
                    {
                        IntPtr hWnd = _WinHwnd(WinTitle);  //returns Window Handle (from either handle or window title)

                        StringBuilder ClassName = new StringBuilder(256);
                        var nRet = _GetClassName(hWnd, ClassName, ClassName.Capacity);
                        return ClassName.ToString();
                    }
            */
        }

        /// <summary>Retrieves the position and size of the specified window.</summary>
        /// <param name="WinTitle">The title or partial title of the target window (the matching behavior is determined by SetTitleMatchMode). If this and the other 3 parameters are omitted, the Last Found Window will be used. To use a window class, specify ahk_class ExactClassName (shown by Window Spy). To use a process identifier (PID), specify ahk_pid %VarContainingPID%. To use a window group, specify ahk_group GroupName. To use a window's unique ID number, specify ahk_id %VarContainingID%. The search can be narrowed by specifying multiple criteria. For example: My File.txt ahk_class Notepad</param>
        /// <param name="WinText">If present, this parameter must be a substring from a single text element of the target window (as revealed by the included Window Spy utility). Hidden text elements are detected if DetectHiddenText is ON.</param>
        /// <param name="ExcludeTitle">Windows whose titles include this value will not be considered. Note: Due to backward compatibility with .aut scripts, this parameter will be interpreted as a command if it exactly matches the name of a command. To work around this, use the WinActive() function instead.</param>
        /// <param name="ExcludeText">Windows whose text include this value will not be considered.</param>        
        public winInfo WinGetPos(string WinTitle = "", string WinText = "", string ExcludeTitle = "", string ExcludeText = "")
        {
            string AHKLine = "WinGetPos, OutX, OutY, OutW, OutH, " + WinTitle + "," + WinText + "," + ExcludeTitle + "," + ExcludeText;  // ahk line to execute
            ErrorLog_Setup(false); // ErrorLevel Detection Enabled for this function in AHK 
            Execute(AHKLine);   // execute AHK code and return variable value             

            //return value from ahk session
            string OutX = GetVar("OutX");
            string OutY = GetVar("OutY");
            string OutW = GetVar("OutW");
            string OutH = GetVar("OutH");

            winInfo wI = new winInfo();   // define new object reference
            try
            {
                wI.WinX = Int32.Parse(OutX);
                wI.WinY = Int32.Parse(OutY);
                wI.WinW = Int32.Parse(OutW);
                wI.WinH = Int32.Parse(OutH);
            }
            catch
            {

            }

            return wI;

            // v1 

            //// based on the window handle/window title, return a rectangle object populated with top/left/w/h
            ////AHK.AHK.wInfo WinCoords = ahk.WinGetPos(Window);
            ////ahk.MsgBox("X = " + WinCoords.WinX + Environment.NewLine + "Y = " + WinCoords.WinY + Environment.NewLine + "W = " + WinCoords.WinWidth + Environment.NewLine + "H = " + WinCoords.WinHeight);

            //winInfo info = new winInfo(); // declare the object to return populated
            //Rect ReturnRect = new Rect(); // declare the object to return populated
            //IntPtr hWnd = _WinHwnd(WinTitle);  //returns Window Handle (from either handle or window title)

            //_GetWindowRect(hWnd, ref ReturnRect);   //return the window dimensions

            //// set the values / calculate width/height
            //int x = ReturnRect.Left;
            //int y = ReturnRect.Top;

            //int Width = ReturnRect.Right - ReturnRect.Left;
            //int Height = ReturnRect.Bottom - ReturnRect.Top;


            //info.WinX = ReturnRect.Left;
            //info.WinY = ReturnRect.Top;
            //info.WinW = Width;
            //info.WinH = Height;


            ////MessageBox.Show("Width = " + Width.ToString() + Environment.NewLine + "Height = " + Height.ToString() + Environment.NewLine + "X = " + x.ToString() + "   Y = " + y.ToString());
            //return info;


        }

        /// <summary>Retrieves the text from the specified window.</summary>
        /// <param name="WinTitle">The title or partial title of the target window (the matching behavior is determined by SetTitleMatchMode). If this and the other 3 parameters are omitted, the Last Found Window will be used. To use a window class, specify ahk_class ExactClassName (shown by Window Spy). To use a process identifier (PID), specify ahk_pid %VarContainingPID%. To use a window group, specify ahk_group GroupName. To use a window's unique ID number, specify ahk_id %VarContainingID%. The search can be narrowed by specifying multiple criteria. For example: My File.txt ahk_class Notepad</param>
        /// <param name="WinText">If present, this parameter must be a substring from a single text element of the target window (as revealed by the included Window Spy utility). Hidden text elements are detected if DetectHiddenText is ON.</param>
        /// <param name="ExcludeTitle">Windows whose titles include this value will not be considered. Note: Due to backward compatibility with .aut scripts, this parameter will be interpreted as a command if it exactly matches the name of a command. To work around this, use the WinActive() function instead.</param>
        /// <param name="ExcludeText">Windows whose text include this value will not be considered.</param>        
        public string WinGetText(string WinTitle = "", string WinText = "", string ExcludeTitle = "", string ExcludeText = "")
        {
            string AHKLine = "WinGetText, OutputVar, " + WinTitle + "," + WinText + "," + ExcludeTitle + "," + ExcludeText;  // ahk line to execute
            ErrorLog_Setup(false); // ErrorLevel Detection Enabled for this function in AHK 
            string OutVar = Execute(AHKLine, "OutputVar");   // execute AHK code and return variable value 
            return OutVar;

            // v1 ToDo
            /*
                    public string _WinGetText(object WinTitle = null, string WinText = "", string ExcludeTitle = "", string ExcludeText = "")  // returns visible text from window
                    {
                        if (ahkGlobal.ahkdll == null) { ahkGlobal.ahkdll = new AutoHotkey.Interop.AutoHotkeyEngine(); }
                        var ahkdll = ahkGlobal.ahkdll;
                        IntPtr hWnd = _WinHwnd(WinTitle);  //returns Window Handle (from either handle or window title)

                        ahkdll.SetVar("WinText", "");
                        string Command = "WinGetText, WinText, ahk_id " + hWnd.ToString() + "," + WinText + "," + ExcludeTitle + "," + ExcludeText;
                        ahkdll.ExecRaw(Command);
                        string WinTextReturn = ahkdll.GetVar("WinText");

                        return WinTextReturn;
                    }
            */

            // v2 ToDo
            /*
                    public string _WinText(IntPtr hWnd)  // returns text from window (by win handle)
                    {
                        //StringBuilder builder = new StringBuilder(500);
                        //int handle = hWnd.ToInt32(); 
                        ////Get the text from the active window into the stringbuilder
                        //SendMessage(handle, WM_GETTEXT, builder.Capacity, builder);
                        //return builder.ToString();

                        int handle = hWnd.ToInt32();
                        const int WM_GETTEXT = 0x0D;
                        StringBuilder sb = new StringBuilder();
                        int retVal = _SendMessage(handle, WM_GETTEXT, 100, sb);
                        //MessageBox.Show(sb.ToString());

                        return sb.ToString();
                    }
            */

        }

        /// <summary>Retrieves the title of the specified window.</summary>
        /// <param name="WinTitle">The title or partial title of the target window (the matching behavior is determined by SetTitleMatchMode). If this and the other 3 parameters are omitted, the Last Found Window will be used. To use a window class, specify ahk_class ExactClassName (shown by Window Spy). To use a process identifier (PID), specify ahk_pid %VarContainingPID%. To use a window group, specify ahk_group GroupName. To use a window's unique ID number, specify ahk_id %VarContainingID%. The search can be narrowed by specifying multiple criteria. For example: My File.txt ahk_class Notepad</param>
        /// <param name="WinText">If present, this parameter must be a substring from a single text element of the target window (as revealed by the included Window Spy utility). Hidden text elements are detected if DetectHiddenText is ON.</param>
        /// <param name="ExcludeTitle">Windows whose titles include this value will not be considered. Note: Due to backward compatibility with .aut scripts, this parameter will be interpreted as a command if it exactly matches the name of a command. To work around this, use the WinActive() function instead.</param>
        /// <param name="ExcludeText">Windows whose text include this value will not be considered.</param>        
        public string WinGetTitle(string WinTitle = "", string WinText = "", string ExcludeTitle = "", string ExcludeText = "")
        {
            string AHKLine = "WinGetTitle, OutputVar, " + WinTitle + "," + WinText + "," + ExcludeTitle + "," + ExcludeText;  // ahk line to execute
            ErrorLog_Setup(false); // ErrorLevel Detection Enabled for this function in AHK 
            string OutVar = Execute(AHKLine, "OutputVar");   // execute AHK code and return variable value 
            return OutVar;

            // v1 ToDo
            /*
                    public string _WinGetTitle(object WinTitle = null)   // returns Window Title from handle, if blank it grabs the active window title
                    {
                        IntPtr hWnd = new IntPtr(0); // declare IntPtr (assign tmp value)

                        if (WinTitle == null)
                        {
                            hWnd = _WinGetActiveID(); // grabs active window handle if no name is provided
                        }
                        if (WinTitle != null)
                        {
                            hWnd = _WinHwnd(WinTitle);  //returns Window Handle (from either handle or window title)
                        }


                        int handle = hWnd.ToInt32(); // convert to int
                        // get window title from handle
                        StringBuilder title = new StringBuilder(256);
                        _GetWindowText(handle, title, 256);
                        return title.ToString();
                    }
            */

        }

        /// <summary>Hides the specified window.</summary>
        /// <param name="WinTitle">The title or partial title of the target window (the matching behavior is determined by SetTitleMatchMode). If this and the other 3 parameters are omitted, the Last Found Window will be used. To use a window class, specify ahk_class ExactClassName (shown by Window Spy). To use a process identifier (PID), specify ahk_pid %VarContainingPID%. To use a window group, specify ahk_group GroupName. To use a window's unique ID number, specify ahk_id %VarContainingID%. The search can be narrowed by specifying multiple criteria. For example: My File.txt ahk_class Notepad</param>
        /// <param name="WinText">If present, this parameter must be a substring from a single text element of the target window (as revealed by the included Window Spy utility). Hidden text elements are detected if DetectHiddenText is ON.</param>
        /// <param name="ExcludeTitle">Windows whose titles include this value will not be considered. Note: Due to backward compatibility with .aut scripts, this parameter will be interpreted as a command if it exactly matches the name of a command. To work around this, use the WinActive() function instead.</param>
        /// <param name="ExcludeText">Windows whose text include this value will not be considered.</param>        
        public void WinHide(string WinTitle = "", string WinText = "", string ExcludeTitle = "", string ExcludeText = "")
        {
            string AHKLine = "WinHide, " + WinTitle + "," + WinText + "," + ExcludeTitle + "," + ExcludeText;  // ahk line to execute
            ErrorLog_Setup(false); // ErrorLevel Detection Enabled for this function in AHK 
            Execute(AHKLine);   // execute AHK code and return variable value 

            // v1 ToDo
            /*
                    public void WinHide(object WinTitle = null, string WinText = "", string ExcludeTitle = "", string ExcludeText = "")  // hides window (if currently visible)
                    {
                        IntPtr hWnd = _WinHwnd(WinTitle);  //returns Window Handle (from either handle or window title)
                        if (ahkGlobal.ahkdll == null) { ahkGlobal.ahkdll = new AutoHotkey.Interop.AutoHotkeyEngine(); }
                        var ahkdll = ahkGlobal.ahkdll;

                        string Command = "WinHide, ahk_id " + hWnd.ToString() + "," + WinText + "," + ExcludeTitle + "," + ExcludeText;
                        ahkdll.ExecRaw(Command);
                    }
            */
        }

        /// <summary>Unhides the specified window.</summary>
        /// <param name="WinTitle">The title or partial title of the target window (the matching behavior is determined by SetTitleMatchMode). If this and the other 3 parameters are omitted, the Last Found Window will be used. To use a window class, specify ahk_class ExactClassName (shown by Window Spy). To use a process identifier (PID), specify ahk_pid %VarContainingPID%. To use a window group, specify ahk_group GroupName. To use a window's unique ID number, specify ahk_id %VarContainingID%. The search can be narrowed by specifying multiple criteria. For example: My File.txt ahk_class Notepad</param>
        /// <param name="WinText">If present, this parameter must be a substring from a single text element of the target window (as revealed by the included Window Spy utility). Hidden text elements are detected if DetectHiddenText is ON.</param>
        /// <param name="ExcludeTitle">Windows whose titles include this value will not be considered. Note: Due to backward compatibility with .aut scripts, this parameter will be interpreted as a command if it exactly matches the name of a command. To work around this, use the WinActive() function instead.</param>
        /// <param name="ExcludeText">Windows whose text include this value will not be considered.</param>        
        public void WinShow(string WinTitle = "", string WinText = "", string ExcludeTitle = "", string ExcludeText = "")
        {
            ErrorLog_Setup(false);
            Execute("WinShow, " + WinTitle + "," + WinText + "," + ExcludeTitle + "," + ExcludeText);

            // v1 ToDo
            /*
                    public void _WinShow(object WinTitle = null, string WinText = "", string ExcludeTitle = "", string ExcludeText = "")  // shows a window (if already hidden)
                    {
                        IntPtr hWnd = _WinHwnd(WinTitle);  //returns Window Handle (from either handle or window title)
                        if (ahkGlobal.ahkdll == null) { ahkGlobal.ahkdll = new AutoHotkey.Interop.AutoHotkeyEngine(); }
                        var ahkdll = ahkGlobal.ahkdll;

                        string Command = "WinShow, ahk_id " + hWnd.ToString() + "," + WinText + "," + ExcludeTitle + "," + ExcludeText;
                        ahkdll.ExecRaw(Command);
                    }
            */
        }

        /// <summary>Enlarges the specified window to its maximum size.</summary>
        /// <param name="WinTitle">The title or partial title of the target window (the matching behavior is determined by SetTitleMatchMode). If this and the other 3 parameters are omitted, the Last Found Window will be used. To use a window class, specify ahk_class ExactClassName (shown by Window Spy). To use a process identifier (PID), specify ahk_pid %VarContainingPID%. To use a window group, specify ahk_group GroupName. To use a window's unique ID number, specify ahk_id %VarContainingID%. The search can be narrowed by specifying multiple criteria. For example: My File.txt ahk_class Notepad</param>
        /// <param name="WinText">If present, this parameter must be a substring from a single text element of the target window (as revealed by the included Window Spy utility). Hidden text elements are detected if DetectHiddenText is ON.</param>
        /// <param name="ExcludeTitle">Windows whose titles include this value will not be considered. Note: Due to backward compatibility with .aut scripts, this parameter will be interpreted as a command if it exactly matches the name of a command. To work around this, use the WinActive() function instead.</param>
        /// <param name="ExcludeText">Windows whose text include this value will not be considered.</param>        
        public void WinMaximize(string WinTitle = "", string WinText = "", string ExcludeTitle = "", string ExcludeText = "")
        {
            ErrorLog_Setup(false);
            Execute("WinMaximize, " + WinTitle + "," + WinText + "," + ExcludeTitle + "," + ExcludeText);

            // v1 ToDo
            /*
                    public void WinMaximize(object WinTitle)  // maximize window
                    {
                        IntPtr hWnd = _WinHwnd(WinTitle);  //returns Window Handle (from either handle or window title)
                        _ShowWindowAsync(hWnd, 3); //maximize window
                    }
            */

        }

        /// <summary>Collapses the specified window into a button on the task bar.</summary>
        /// <param name="WinTitle">The title or partial title of the target window (the matching behavior is determined by SetTitleMatchMode). If this and the other 3 parameters are omitted, the Last Found Window will be used. To use a window class, specify ahk_class ExactClassName (shown by Window Spy). To use a process identifier (PID), specify ahk_pid %VarContainingPID%. To use a window group, specify ahk_group GroupName. To use a window's unique ID number, specify ahk_id %VarContainingID%. The search can be narrowed by specifying multiple criteria. For example: My File.txt ahk_class Notepad</param>
        /// <param name="WinText">If present, this parameter must be a substring from a single text element of the target window (as revealed by the included Window Spy utility). Hidden text elements are detected if DetectHiddenText is ON.</param>
        /// <param name="ExcludeTitle">Windows whose titles include this value will not be considered. Note: Due to backward compatibility with .aut scripts, this parameter will be interpreted as a command if it exactly matches the name of a command. To work around this, use the WinActive() function instead.</param>
        /// <param name="ExcludeText">Windows whose text include this value will not be considered.</param>        
        public void WinMinimize(string WinTitle = "", string WinText = "", string ExcludeTitle = "", string ExcludeText = "")
        {
            ErrorLog_Setup(false);
            Execute("WinMinimize, " + WinTitle + "," + WinText + "," + ExcludeTitle + "," + ExcludeText);

            // v1 ToDo
            /*
                    public void WinMinimize(object WinTitle)   // minimize window
                    {
                        IntPtr hWnd = _WinHwnd(WinTitle);  //returns Window Handle (from either handle or window title)
                        _ShowWindowAsync(hWnd, 2);  //minimize window
                    }
            */

        }

        /// <summary>Forces the specified window to close.</summary>
        /// <param name="WinTitle">The title or partial title of the target window (the matching behavior is determined by SetTitleMatchMode). If this and the other 3 parameters are omitted, the Last Found Window will be used. To use a window class, specify ahk_class ExactClassName (shown by Window Spy). To use a process identifier (PID), specify ahk_pid %VarContainingPID%. To use a window group, specify ahk_group GroupName. To use a window's unique ID number, specify ahk_id %VarContainingID%. The search can be narrowed by specifying multiple criteria. For example: My File.txt ahk_class Notepad</param>
        /// <param name="WinText">If present, this parameter must be a substring from a single text element of the target window (as revealed by the included Window Spy utility). Hidden text elements are detected if DetectHiddenText is ON.</param>
        /// <param name="SecondsToWait"> </param> 
        /// <param name="ExcludeTitle">Windows whose titles include this value will not be considered. Note: Due to backward compatibility with .aut scripts, this parameter will be interpreted as a command if it exactly matches the name of a command. To work around this, use the WinActive() function instead.</param>
        /// <param name="ExcludeText">Windows whose text include this value will not be considered.</param>/// 
        public void WinKill(string WinTitle = "", string WinText = "", string SecondsToWait = "", string ExcludeTitle = "", string ExcludeText = "")
        {
            ErrorLog_Setup(false);
            Execute("WinKill, " + WinTitle + "," + WinText + "," + SecondsToWait + "," + ExcludeTitle + "," + ExcludeText);

            // v1 ToDo
            /*
                    public void _WinKill(string WinTitle, string WinText = "", string SecondsToWait = "", string ExcludeTitle = "", string ExcludeText = "")  // Forces the specified window to close.
                    {
                        //IntPtr hWnd = WinHwnd(WinTitle);  //returns Window Handle (from either handle or window title)
                        if (ahkGlobal.ahkdll == null) { ahkGlobal.ahkdll = new AutoHotkey.Interop.AutoHotkeyEngine(); }
                        var ahkdll = ahkGlobal.ahkdll;

                        ahkdll.ExecRaw("WinKill, " + WinTitle + "," + WinText + "," + SecondsToWait + "," + ExcludeTitle + "," + ExcludeText);
                    }
            */

        }

        /// <summary>Minimizes all windows.</summary>
        public void WinMinimizeAll()
        {
            ErrorLog_Setup(false);
            Execute("WinMinimizeAll");
        }

        /// <summary>Unminimizes all windows.</summary>
        public void WinMinimizeAllUndo()
        {
            ErrorLog_Setup(false);
            Execute("WinMinimizeAllUndo");
        }

        /// <summary>Changes the position and/or size of the specified window.</summary>
        /// <param name="WinTitle">The title or partial title of the target window (the matching behavior is determined by SetTitleMatchMode). If this and the other 3 parameters are omitted, the Last Found Window will be used. To use a window class, specify ahk_class ExactClassName (shown by Window Spy). To use a process identifier (PID), specify ahk_pid %VarContainingPID%. To use a window group, specify ahk_group GroupName. To use a window's unique ID number, specify ahk_id %VarContainingID%. The search can be narrowed by specifying multiple criteria. For example: My File.txt ahk_class Notepad</param>
        /// <param name="WinText">If present, this parameter must be a substring from a single text element of the target window (as revealed by the included Window Spy utility). Hidden text elements are detected if DetectHiddenText is ON.</param>
        /// <param name="X">The X coordinates (in pixels) of the upper left corner of the target window's new location, which can be expressions. The upper-left pixel of the screen is at 0, 0. If these are the only parameters given with the command, the Last Found Window will be used as the target window.</param>
        /// <param name="Y">The Y coordinates (in pixels) of the upper left corner of the target window's new location, which can be expressions. The upper-left pixel of the screen is at 0, 0. If these are the only parameters given with the command, the Last Found Window will be used as the target window.</param>
        /// <param name="Width">The new width and height of the window (in pixels), which can be expressions. If either is omitted, blank, or the word DEFAULT, the size in that dimension will not be changed.</param>
        /// <param name="Height">The new width and height of the window (in pixels), which can be expressions. If either is omitted, blank, or the word DEFAULT, the size in that dimension will not be changed.</param>
        /// <param name="ExcludeTitle">Windows whose titles include this value will not be considered. Note: Due to backward compatibility with .aut scripts, this parameter will be interpreted as a command if it exactly matches the name of a command. To work around this, use the WinActive() function instead.</param>
        /// <param name="ExcludeText">Windows whose text include this value will not be considered.</param>/// 
        public void WinMove(string WinTitle, string WinText = "", object X = null, object Y = null, object Width = null, object Height = null, string ExcludeTitle = "", string ExcludeText = "")
        {
            ErrorLog_Setup(false);
            Execute("WinMove, " + WinTitle + "," + WinText + "," + X.ToString() + "," + Y + "," + Width.ToString() + "," + Height.ToString() + "," + ExcludeTitle + "," + ExcludeText);


            // v1 ToDo
            /*
                        IntPtr hWnd = _WinHwnd(WinTitle);  //returns Window Handle (from either handle or window title)

                        const short SWP_NOMOVE = 0X2;
                        const short SWP_NOSIZE = 1;
                        const short SWP_NOZORDER = 0X4;
                        const int SWP_SHOWWINDOW = 0x0040;

                        //hWnd Insert After (2nd parameter in SetWindowPos)
                        IntPtr HWND_TOPMOST = new IntPtr(-1);
                        IntPtr HWND_NOTOPMOST = new IntPtr(-2);
                        IntPtr HWND_TOP = new IntPtr(0);
                        IntPtr HWND_BOTTOM = new IntPtr(1);

                        uint NOSIZE = 0x0001;
                        uint NOMOVE = 0x0002;
                        uint NOZORDER = 0x0004;
                        uint NOREDRAW = 0x0008;
                        uint NOACTIVATE = 0x0010;
                        uint DRAWFRAME = 0x0020;
                        uint FRAMECHANGED = 0x0020;
                        uint SHOWWINDOW = 0x0040;
                        uint HIDEWINDOW = 0x0080;
                        uint NOCOPYBITS = 0x0100;
                        uint NOOWNERZORDER = 0x0200;
                        uint NOREPOSITION = 0x0200;
                        uint NOSENDCHANGING = 0x0400;
                        uint DEFERERASE = 0x2000;
                        uint ASYNCWINDOWPOS = 0x4000;

                        winInfo WinSize = WinGetPos(WinTitle.ToString()); // current w/h used if not provided by user

                        if (w == 0)  // use original width if not provided
                        {
                            w = WinSize.WinW;
                        }
                        if (h == 0)  // use original height if not provided
                        {
                            h = WinSize.WinH;
                        }

                        _SetWindowPos(hWnd, HWND_TOPMOST, x, y, w, h, SHOWWINDOW);
            */
        }

        /// <summary>Unminimizes or unmaximizes the specified window if it is minimized or maximized.</summary>
        /// <param name="WinTitle">The title or partial title of the target window (the matching behavior is determined by SetTitleMatchMode). If this and the other 3 parameters are omitted, the Last Found Window will be used. To use a window class, specify ahk_class ExactClassName (shown by Window Spy). To use a process identifier (PID), specify ahk_pid %VarContainingPID%. To use a window group, specify ahk_group GroupName. To use a window's unique ID number, specify ahk_id %VarContainingID%. The search can be narrowed by specifying multiple criteria. For example: My File.txt ahk_class Notepad</param>
        /// <param name="WinText">If present, this parameter must be a substring from a single text element of the target window (as revealed by the included Window Spy utility). Hidden text elements are detected if DetectHiddenText is ON.</param>
        /// <param name="ExcludeTitle">Windows whose titles include this value will not be considered. Note: Due to backward compatibility with .aut scripts, this parameter will be interpreted as a command if it exactly matches the name of a command. To work around this, use the WinActive() function instead.</param>
        /// <param name="ExcludeText">Windows whose text include this value will not be considered.</param>/// 
        public void WinRestore(string WinTitle = "", string WinText = "", string ExcludeTitle = "", string ExcludeText = "")
        {
            ErrorLog_Setup(false);
            Execute("WinRestore, " + WinTitle + "," + WinText + "," + ExcludeTitle + "," + ExcludeText);

            // v1 ToDo
            /*
                    public void _WinRestore(object WinTitle)  // Unminimizes or unmaximizes the specified window if it is minimized or maximized.
                    {
                        IntPtr hWnd = _WinHwnd(WinTitle);  //returns Window Handle (from either handle or window title)
                        _ShowWindowAsync(hWnd, 1);
                    }
            */
        }

        /// <summary>Makes a variety of changes to the specified window, such as "always on top" and transparency.</summary>
        /// <param name="Attribute">AlwaysOnTop | Bottom | Top | Disable | Enable | Redraw | Style | Region | Transparent | Transcolor </param>
        /// <param name="Value">See AHK Documentation For Options</param>
        /// <param name="WinTitle">The title or partial title of the target window (the matching behavior is determined by SetTitleMatchMode). If this and the other 3 parameters are omitted, the Last Found Window will be used. To use a window class, specify ahk_class ExactClassName (shown by Window Spy). To use a process identifier (PID), specify ahk_pid %VarContainingPID%. To use a window group, specify ahk_group GroupName. To use a window's unique ID number, specify ahk_id %VarContainingID%. The search can be narrowed by specifying multiple criteria. For example: My File.txt ahk_class Notepad</param>
        /// <param name="WinText">If present, this parameter must be a substring from a single text element of the target window (as revealed by the included Window Spy utility). Hidden text elements are detected if DetectHiddenText is ON.</param>
        /// <param name="ExcludeTitle">Windows whose titles include this value will not be considered. Note: Due to backward compatibility with .aut scripts, this parameter will be interpreted as a command if it exactly matches the name of a command. To work around this, use the WinActive() function instead.</param>
        /// <param name="ExcludeText">Windows whose text include this value will not be considered.</param>        
        public bool WinSet(string Attribute, string Value, string WinTitle = "", string WinText = "", string ExcludeTitle = "", string ExcludeText = "")
        {
            string AHKLine = "WinSet, " + Attribute + "," + Value + "," + WinTitle + "," + WinText + "," + ExcludeTitle + "," + ExcludeText;  // ahk line to execute
            ErrorLog_Setup(true); // ErrorLevel Detection Enabled for this function in AHK 
            Execute(AHKLine);   // execute AHK code and return variable value

            if (!ahkGlobal.ErrorLevel) { return true; } // no error level - return true for success
            return false;  // error level detected - success = false
        }

        /// <summary>Changes the title of the specified window.</summary>
        /// <param name="WinTitle">The title or partial title of the target window (the matching behavior is determined by SetTitleMatchMode). If this and the other 3 parameters are omitted, the Last Found Window will be used. To use a window class, specify ahk_class ExactClassName (shown by Window Spy). To use a process identifier (PID), specify ahk_pid %VarContainingPID%. To use a window group, specify ahk_group GroupName. To use a window's unique ID number, specify ahk_id %VarContainingID%. The search can be narrowed by specifying multiple criteria. For example: My File.txt ahk_class Notepad</param>
        /// <param name="WinText">If present, this parameter must be a substring from a single text element of the target window (as revealed by the included Window Spy utility). Hidden text elements are detected if DetectHiddenText is ON.</param>
        /// <param name="NewTitle">The new title for the window. If this is the only parameter given, the Last Found Window will be used.</param>
        /// <param name="ExcludeTitle">Windows whose titles include this value will not be considered. Note: Due to backward compatibility with .aut scripts, this parameter will be interpreted as a command if it exactly matches the name of a command. To work around this, use the WinActive() function instead.</param>
        /// <param name="ExcludeText">Windows whose text include this value will not be considered.</param>        
        public void WinSetTitle(string WinTitle, string WinText, string NewTitle, string ExcludeTitle = "", string ExcludeText = "")
        {
            ErrorLog_Setup(false);
            Execute("WinSetTitle, " + WinTitle + "," + WinText + "," + NewTitle + "," + ExcludeTitle + "," + ExcludeText);
        }

        /// <summary>Waits until the specified window exists.</summary>
        /// <param name="WinTitle">The title or partial title of the target window (the matching behavior is determined by SetTitleMatchMode). If this and the other 3 parameters are omitted, the Last Found Window will be used. To use a window class, specify ahk_class ExactClassName (shown by Window Spy). To use a process identifier (PID), specify ahk_pid %VarContainingPID%. To use a window group, specify ahk_group GroupName. To use a window's unique ID number, specify ahk_id %VarContainingID%. The search can be narrowed by specifying multiple criteria. For example: My File.txt ahk_class Notepad</param>
        /// <param name="WinText">If present, this parameter must be a substring from a single text element of the target window (as revealed by the included Window Spy utility). Hidden text elements are detected if DetectHiddenText is ON.</param>
        /// <param name="Seconds">How many seconds to wait before timing out and setting ErrorLevel to 1. Leave blank to wait indefinitely. Specifying 0 is the same as specifying 0.5. This parameter can be an expression.</param>
        /// <param name="ExcludeTitle">Windows whose titles include this value will not be considered. Note: Due to backward compatibility with .aut scripts, this parameter will be interpreted as a command if it exactly matches the name of a command. To work around this, use the WinActive() function instead.</param>
        /// <param name="ExcludeText">Windows whose text include this value will not be considered.</param> 
        public bool WinWait(string WinTitle = "", string WinText = "", string Seconds = "", string ExcludeTitle = "", string ExcludeText = "")
        {
            string AHKLine = "WinWait, " + WinTitle + "," + WinText + "," + Seconds + "," + ExcludeTitle + "," + ExcludeText;  // ahk line to execute
            ErrorLog_Setup(true, "[WinWait] Command Timed Out"); // ErrorLevel Detection Enabled for this function in AHK 
            Execute(AHKLine);   // execute AHK code and return variable value

            if (!ahkGlobal.ErrorLevel) { return true; } // no error level - return true for success
            return false;  // error level detected - success = false

            // v1 ToDo
            /*
                    public bool _WinWait(object WinTitle = null, string WinText = "", int Seconds = 5, string ExcludeTitle = "", string ExcludeText = "")  // waits for window title to exist
                    {
                        IntPtr hWnd = _WinHwnd(WinTitle);  //returns Window Handle (from either handle or window title)
                        if (ahkGlobal.ahkdll == null) { ahkGlobal.ahkdll = new AutoHotkey.Interop.AutoHotkeyEngine(); }
                        var ahkdll = ahkGlobal.ahkdll;

                        string Command = "WinWait, ahk_id " + hWnd.ToString() + "," + WinText + "," + Seconds + "," + ExcludeTitle + "," + ExcludeText;
                        ahkdll.ExecRaw(Command);

                        //   Error Handling
                        ahkGlobal.ErrorLevelMsg = "WinWait Timed Out.";
                        ahkGlobal.ErrorLevel = false;
                        string ErrorLevelValue = ahkdll.GetVar("ErrorLevel");
                        if (ErrorLevelValue == "0") { ahkGlobal.ErrorLevelMsg = ""; }
                        if (ErrorLevelValue == "1") { ahkGlobal.ErrorLevel = true; }
                        if (ahkGlobal.Debug) { if (ahkGlobal.ErrorLevel) { MsgBox(ahkGlobal.ErrorLevelMsg, "ErrorLevel"); } } //Display Error Messages in Debug Mode


                        bool WinFound = false;
                        if (ahkGlobal.ErrorLevel == false) { WinFound = true; }

                        return WinFound;
                    }
            */

        }

        /// <summary>Waits until the specified window is active.</summary>
        /// <param name="WinTitle">The title or partial title of the target window (the matching behavior is determined by SetTitleMatchMode). If this and the other 3 parameters are omitted, the Last Found Window will be used. To use a window class, specify ahk_class ExactClassName (shown by Window Spy). To use a process identifier (PID), specify ahk_pid %VarContainingPID%. To use a window group, specify ahk_group GroupName. To use a window's unique ID number, specify ahk_id %VarContainingID%. The search can be narrowed by specifying multiple criteria. For example: My File.txt ahk_class Notepad</param>
        /// <param name="WinText">If present, this parameter must be a substring from a single text element of the target window (as revealed by the included Window Spy utility). Hidden text elements are detected if DetectHiddenText is ON.</param>
        /// <param name="Seconds">How many seconds to wait before timing out and setting ErrorLevel to 1. Leave blank to wait indefinitely. Specifying 0 is the same as specifying 0.5. This parameter can be an expression.</param>
        /// <param name="ExcludeTitle">Windows whose titles include this value will not be considered. Note: Due to backward compatibility with .aut scripts, this parameter will be interpreted as a command if it exactly matches the name of a command. To work around this, use the WinActive() function instead.</param>
        /// <param name="ExcludeText">Windows whose text include this value will not be considered.</param> 
        public bool WinWaitActive(string WinTitle = "", string WinText = "", string Seconds = "", string ExcludeTitle = "", string ExcludeText = "")
        {
            string AHKLine = "WinWaitActive, " + WinTitle + "," + WinText + "," + Seconds + "," + ExcludeTitle + "," + ExcludeText;  // ahk line to execute
            ErrorLog_Setup(true, "[WinWaitActive] Command Timed Out"); // ErrorLevel Detection Enabled for this function in AHK 
            Execute(AHKLine);   // execute AHK code and return variable value

            if (!ahkGlobal.ErrorLevel) { return true; } // no error level - return true for success
            return false;  // error level detected - success = false
        }

        /// <summary>Waits until the specified window is not active.</summary>
        /// <param name="WinTitle">The title or partial title of the target window (the matching behavior is determined by SetTitleMatchMode). If this and the other 3 parameters are omitted, the Last Found Window will be used. To use a window class, specify ahk_class ExactClassName (shown by Window Spy). To use a process identifier (PID), specify ahk_pid %VarContainingPID%. To use a window group, specify ahk_group GroupName. To use a window's unique ID number, specify ahk_id %VarContainingID%. The search can be narrowed by specifying multiple criteria. For example: My File.txt ahk_class Notepad</param>
        /// <param name="WinText">If present, this parameter must be a substring from a single text element of the target window (as revealed by the included Window Spy utility). Hidden text elements are detected if DetectHiddenText is ON.</param>
        /// <param name="Seconds">How many seconds to wait before timing out and setting ErrorLevel to 1. Leave blank to wait indefinitely. Specifying 0 is the same as specifying 0.5. This parameter can be an expression.</param>
        /// <param name="ExcludeTitle">Windows whose titles include this value will not be considered. Note: Due to backward compatibility with .aut scripts, this parameter will be interpreted as a command if it exactly matches the name of a command. To work around this, use the WinActive() function instead.</param>
        /// <param name="ExcludeText">Windows whose text include this value will not be considered.</param> 
        public bool WinWaitNotActive(string WinTitle = "", string WinText = "", string Seconds = "", string ExcludeTitle = "", string ExcludeText = "")
        {
            string AHKLine = "WinWaitNotActive, " + WinTitle + "," + WinText + "," + Seconds + "," + ExcludeTitle + "," + ExcludeText;  // ahk line to execute
            ErrorLog_Setup(true, "[WinWaitNotActive] Command Timed Out"); // ErrorLevel Detection Enabled for this function in AHK 
            Execute(AHKLine);   // execute AHK code and return variable value

            if (!ahkGlobal.ErrorLevel) { return true; } // no error level - return true for success
            return false;  // error level detected - success = false
        }

        /// <summary>Retrieves the minimized/maximized state for a window. returns true if window is minimized</summary>
        /// <param name="WinTitle">The title or partial title of the target window (the matching behavior is determined by SetTitleMatchMode). If this and the other 3 parameters are omitted, the Last Found Window will be used. To use a window class, specify ahk_class ExactClassName (shown by Window Spy). To use a process identifier (PID), specify ahk_pid %VarContainingPID%. To use a window group, specify ahk_group GroupName. To use a window's unique ID number, specify ahk_id %VarContainingID%. The search can be narrowed by specifying multiple criteria. For example: My File.txt ahk_class Notepad</param>
        /// <param name="WinText">If present, this parameter must be a substring from a single text element of the target window (as revealed by the included Window Spy utility). Hidden text elements are detected if DetectHiddenText is ON.</param>
        /// <param name="ExcludeTitle">Windows whose titles include this value will not be considered. Note: Due to backward compatibility with .aut scripts, this parameter will be interpreted as a command if it exactly matches the name of a command. To work around this, use the WinActive() function instead.</param>
        /// <param name="ExcludeText">Windows whose text include this value will not be considered.</param>
        public bool IsMinimized(string WinTitle = "", string WinText = "", string ExcludeTitle = "", string ExcludeText = "")
        {
            // OuputVar is made blank if no matching window exists; otherwise, it is set to one of the following numbers:
            //-1: The window is minimized (WinRestore can unminimize it). 
            //1: The window is maximized (WinRestore can unmaximize it).
            //0: The window is neither minimized nor maximized.

            string MinMax = WinGet(_AHK.WinGetCmd.MinMax, WinTitle, WinText, ExcludeTitle, ExcludeText);
            if (MinMax == "-1") { return true; }
            return false;

            // v1 ToDo
            /*
                    public bool IsMinimized(object WinTitle)  // returns true if window is minimized
                    {
                        IntPtr hWnd = _WinHwnd(WinTitle);  //returns Window Handle (from either handle or window title)

                        WINDOWPLACEMENT placement = new WINDOWPLACEMENT();
                        _GetWindowPlacement(hWnd, ref placement);
                        switch (placement.showCmd)
                        {
                            case 1:
                                //Console.WriteLine("Normal");
                                break;
                            case 2:
                                return true;
                            //Console.WriteLine("Minimized");
                            case 3:
                                //Console.WriteLine("Maximized");
                                break;
                        }

                        return false;
                    }
            */

        }

        /// <summary>Retrieves the minimized/maximized state for a window. returns true if window is maximized</summary>
        /// <param name="WinTitle">The title or partial title of the target window (the matching behavior is determined by SetTitleMatchMode). If this and the other 3 parameters are omitted, the Last Found Window will be used. To use a window class, specify ahk_class ExactClassName (shown by Window Spy). To use a process identifier (PID), specify ahk_pid %VarContainingPID%. To use a window group, specify ahk_group GroupName. To use a window's unique ID number, specify ahk_id %VarContainingID%. The search can be narrowed by specifying multiple criteria. For example: My File.txt ahk_class Notepad</param>
        /// <param name="WinText">If present, this parameter must be a substring from a single text element of the target window (as revealed by the included Window Spy utility). Hidden text elements are detected if DetectHiddenText is ON.</param>
        /// <param name="ExcludeTitle">Windows whose titles include this value will not be considered. Note: Due to backward compatibility with .aut scripts, this parameter will be interpreted as a command if it exactly matches the name of a command. To work around this, use the WinActive() function instead.</param>
        /// <param name="ExcludeText">Windows whose text include this value will not be considered.</param>
        public bool IsMaximized(string WinTitle = "", string WinText = "", string ExcludeTitle = "", string ExcludeText = "")
        {
            // OuputVar is made blank if no matching window exists; otherwise, it is set to one of the following numbers:
            //-1: The window is minimized (WinRestore can unminimize it). 
            //1: The window is maximized (WinRestore can unmaximize it).
            //0: The window is neither minimized nor maximized.

            string MinMax = WinGet(_AHK.WinGetCmd.MinMax, WinTitle, WinText, ExcludeTitle, ExcludeText);
            if (MinMax == "1") { return true; }
            return false;

            // v1 ToDo
            /*
                    public bool IsMaximized(object WinTitle)  // returns true if window is maximized
                    {
                        IntPtr hWnd = _WinHwnd(WinTitle);  //returns Window Handle (from either handle or window title)

                        WINDOWPLACEMENT placement = new WINDOWPLACEMENT();
                        _GetWindowPlacement(hWnd, ref placement);
                        switch (placement.showCmd)
                        {
                            case 1:
                                return false;
                            //Console.WriteLine("Normal");
                            case 2:
                                return false;
                            //Console.WriteLine("Minimized");
                            case 3:
                                return true;
                            //Console.WriteLine("Maximized");
                        }

                        return false;
                    }
             */

        }

        /// <summary>Retrieves the minimized/maximized state for a window. returns true if window is NEITHER minimized nor maximized</summary>
        /// <param name="WinTitle">The title or partial title of the target window (the matching behavior is determined by SetTitleMatchMode). If this and the other 3 parameters are omitted, the Last Found Window will be used. To use a window class, specify ahk_class ExactClassName (shown by Window Spy). To use a process identifier (PID), specify ahk_pid %VarContainingPID%. To use a window group, specify ahk_group GroupName. To use a window's unique ID number, specify ahk_id %VarContainingID%. The search can be narrowed by specifying multiple criteria. For example: My File.txt ahk_class Notepad</param>
        /// <param name="WinText">If present, this parameter must be a substring from a single text element of the target window (as revealed by the included Window Spy utility). Hidden text elements are detected if DetectHiddenText is ON.</param>
        /// <param name="ExcludeTitle">Windows whose titles include this value will not be considered. Note: Due to backward compatibility with .aut scripts, this parameter will be interpreted as a command if it exactly matches the name of a command. To work around this, use the WinActive() function instead.</param>
        /// <param name="ExcludeText">Windows whose text include this value will not be considered.</param>
        public bool NotMinOrMax(string WinTitle = "", string WinText = "", string ExcludeTitle = "", string ExcludeText = "")
        {
            // OuputVar is made blank if no matching window exists; otherwise, it is set to one of the following numbers:
            //-1: The window is minimized (WinRestore can unminimize it). 
            //1: The window is maximized (WinRestore can unmaximize it).
            //0: The window is neither minimized nor maximized.

            string MinMax = WinGet(_AHK.WinGetCmd.MinMax, WinTitle, WinText, ExcludeTitle, ExcludeText);
            if (MinMax == "0") { return true; }
            return false;
        }

        /// <summary>
        /// Returns List of Window Titles in Current Processes
        /// </summary>
        /// <returns>List of Window Titles Currently Running</returns>
        public List<string> WinList()
        {
            List<string> WinList = new List<string>();
            foreach (Process process in Process.GetProcesses())
            {
                if (!String.IsNullOrEmpty(process.MainWindowTitle))
                {
                    //Console.WriteLine("Process: {0} ID: {1} Window title: {2}", process.ProcessName, process.Id, process.MainWindowTitle);
                    //MsgBox("ProcessName: " + process.ProcessName + Environment.NewLine + "ProcessID: " + process.Id + Environment.NewLine + "WinTitle: " + process.MainWindowTitle);
                    // add to list if not already in the list
                    WinList.Add(process.MainWindowTitle);
                }
            }
            return WinList;
        }



        /// <summary>
        /// Returns Window Handle (from either handle or window title)
        /// </summary>
        /// <param name="WinTitle"></param>
        /// <returns></returns>
        private IntPtr WinHwnd(object WinTitle)
        {
            string VarType = WinTitle.GetType().ToString();  //determine what kind of variable was passed into function

            IntPtr hWnd = new IntPtr(0); // declare IntPtr (assign tmp value)

            // user passed in the Window Title as string
            if (VarType == "System.String")
            {
                hWnd = WinGetHwnd(WinTitle.ToString());  //grab window title handle
            }

            // user passed in the handle, activate window
            if (VarType == "System.IntPtr")
            {
                if (!WinTitle.Equals(IntPtr.Zero))
                {
                    hWnd = (IntPtr)WinTitle; //declare the object as an IntPtr variable
                }
            }

            return hWnd;
        }
        private IntPtr WinGetHwnd(string WindowTitle = "A")  // returns window handle, default "A" returns active window handle
        {
            IntPtr hWnd = IntPtr.Zero;

            // c# version - works
            foreach (Process pList in Process.GetProcesses())
            {
                if (pList.MainWindowTitle.Contains(WindowTitle))
                {
                    hWnd = pList.MainWindowHandle;
                }
            }
            return hWnd; //Should contain the handle but may be zero if the title doesn't match


            //// grabs ahk id
            //if (ahkGlobal.ahkdll == null) { ahkGlobal.ahkdll = new AutoHotkey.Interop.AutoHotkeyEngine(); }
            //var ahkdll = ahkGlobal.ahkdll;

            //string WinTitle = "Untitled - Notepad";
            //string Command = "WinGet, active_id, ID, " + WinTitle;
            ////string Command = "WinID := WinGetID(\"" + WindowTitle + "\")"; 
            //ahkdll.ExecRaw(Command);
            //string OutPutVar = ahkdll.GetVar("active_id");
            //MsgBox(OutPutVar);

            //return hWnd;
            ////hWnd = ToIntPtr(OutPutVar); 

            //return hWnd; 
        }
        private IntPtr HwndFromProcessName(string ProcessName)  // returns the handle associated with a process name
        {
            IntPtr hWnd = IntPtr.Zero;
            Process[] processes = Process.GetProcessesByName(ProcessName);
            try
            {
                Process lol = processes[0];
                hWnd = lol.MainWindowHandle;
            }
            catch { }
            //int handle = hWnd.ToInt32();
            return hWnd; //Should contain the handle but may be zero if the title doesn't match
        }


        /// <summary>
        /// Populate wInfo object from the Window Title
        /// </summary>
        /// <param name="WinTitle"></param>
        /// <returns></returns>
        public winInfo Return_wInfo(object WinTitle, bool WindowScreenCap = true)
        {
            //public wInfo MouseGetPos()  // gets the current mouse position, returns wInfo object populated (includes control info/handles)

            if (ahkGlobal.ahkdll == null) { ahkGlobal.ahkdll = new AutoHotkey.Interop.AutoHotkeyEngine(); }
            var ahkdll = ahkGlobal.ahkdll;

            winInfo info = new winInfo(); // declare the object to return populated

            //ahkdll.SetVar("OutputVar", "");
            string Command = "MouseGetPos, MouseX, MouseY, WinID, ControlID, 3";
            ahkdll.ExecRaw(Command);

            //info.MouseXPos = Int32.Parse(ahkdll.GetVar("MouseX"));
            //info.MouseYPos = Int32.Parse(ahkdll.GetVar("MouseY"));

            ////// convert string to IntPtr
            ////string winHwnd = ahkdll.GetVar("WinID");
            ////if (winHwnd != "")
            ////{
            ////    int WinID = Int32.Parse(winHwnd);  // string to int
            ////    info.WinHwnd = ToIntPtr(WinID);  // int to IntPtr
            ////}

            ////// convert string to IntPtr
            ////string ControlHwnd = ahkdll.GetVar("ControlID");
            ////if (ControlHwnd != "")
            ////{
            ////    int cID = Int32.Parse(ControlHwnd);  // string to int
            ////    info.ControlHwnd = ToIntPtr(cID);  // int to IntPtr
            ////}

            //info. = WinHwnd(WinTitle);
            //info.WinClass = WinGetClass(WinTitle);   // win class


            //// based on the window handle/window title, return a rectangle object populated with top/left/w/h
            //AHK.AHK.wInfo WinCoords = ahk.WinGetPos(Window);
            //ahk.MsgBox("X = " + WinCoords.WinX + Environment.NewLine + "Y = " + WinCoords.WinY + Environment.NewLine + "W = " + WinCoords.WinWidth + Environment.NewLine + "H = " + WinCoords.WinHeight);

            Rect ReturnRect = new Rect(); // declare the object to return populated
            IntPtr hWnd = WinGetHwnd(WinTitle.ToString());  //returns Window Handle (from either handle or window title)

            //return the window dimensions
            //try { _GetWindowRect(hWnd, ref ReturnRect); } catch { }

            winInfo winif = WinGetPos(WinTitle.ToString());

            info.WinX = winif.WinX;
            info.WinY = winif.WinY;
            info.WinW = winif.WinW;
            info.WinH = winif.WinH;


            // Window Coordinates on Screen
            //info.WinX = ReturnRect.Left;
            //info.WinY = ReturnRect.Top;
            //info.WinW = ReturnRect.Right - ReturnRect.Left;
            //info.WinH = ReturnRect.Bottom - ReturnRect.Top;

            // extract icon from exe path, convert to Image
            try { info.WinIcon = Icon.ExtractAssociatedIcon(WinTitle.ToString()).ToBitmap(); } catch { }
            
            if (WindowScreenCap)
            {
                //==============================================================
                // Take ScreenShot of Window
                //==============================================================
                var bmpScreenshot = new Bitmap(Screen.PrimaryScreen.Bounds.Width,
                                               Screen.PrimaryScreen.Bounds.Height,
                                               PixelFormat.Format32bppArgb);

                // Create a graphics object from the bitmap.
                var gfxScreenshot = Graphics.FromImage(bmpScreenshot);

                // Take the screenshot from the upper left corner to the right bottom corner.
                gfxScreenshot.CopyFromScreen(info.WinX,
                                            info.WinY,
                                            info.WinW,
                                            info.WinH,
                                            Screen.PrimaryScreen.Bounds.Size,
                                            CopyPixelOperation.SourceCopy);

                // Save the screenshot to the specified path that the user has chosen.
                //bmpScreenshot.Save(SaveFile, ImageFormat.Png);
                info.WinScreenShot = bmpScreenshot;
            }


            return info;
        }



        // Win Actions

        public enum WinActions
        {
            AlwaysOnTop,
            AlwaysOnTopOff,
            WinSetBottom,
            WinSetTop,
            WinSetEnable,
            WinSetDisable,
            WinSetRedraw,
            WinActivate,
            WinActivateBottom,
            WinClose,
            WinHide,
            WinShow,
            WinMinimize,
            WinMaximize,
            WinKill,
            WinMinimizeAll,
            WinMinimizeAllUndo,
            WinRestore
        }

        /// <summary>
        /// Returns List of Window Actions
        /// </summary>
        /// <returns>Returns List of WinGet Commands (WinGetCmd)</returns>
        public List<string> WinActionsList()
        {
            List<string> rlist = new List<string>();
            Array list = Enum.GetValues(typeof(_AHK.WinActions));
            foreach (var item in list) { rlist.Add(item.ToString()); }
            return rlist;
        }


        /// <summary>
        /// Window Actions by WinTitle
        /// </summary>
        /// <param name="WinTitle">Window Title / AHK ID</param>
        /// <param name="Action">WinAction to Perform on WinTitle</param>
        public void WinAction(string WinTitle, WinActions Action)
        {
            if (Action == WinActions.AlwaysOnTop) { AlwaysOnTop(WinTitle, true); }
            if (Action == WinActions.AlwaysOnTopOff) { AlwaysOnTop(WinTitle, false); }
            if (Action == WinActions.WinSetBottom) { WinSetBottom(WinTitle); }
            if (Action == WinActions.WinSetTop) { WinSetTop(WinTitle); }
            if (Action == WinActions.WinSetEnable) { WinSetEnable(WinTitle); }
            if (Action == WinActions.WinSetDisable) { WinSetDisable(WinTitle); }
            if (Action == WinActions.WinSetRedraw) { WinSetRedraw(WinTitle); }
            if (Action == WinActions.WinActivate) { WinActivate(WinTitle); }
            if (Action == WinActions.WinActivateBottom) { WinActivateBottom(WinTitle); }
            if (Action == WinActions.WinClose) { WinClose(WinTitle); }
            if (Action == WinActions.WinHide) { WinHide(WinTitle); }
            if (Action == WinActions.WinShow) { WinShow(WinTitle); }
            if (Action == WinActions.WinMinimize) { WinMinimize(WinTitle); }
            if (Action == WinActions.WinMaximize) { WinMaximize(WinTitle); }
            if (Action == WinActions.WinKill) { WinKill(WinTitle); }
            if (Action == WinActions.WinMinimizeAll) { WinMinimizeAll(); }
            if (Action == WinActions.WinMinimizeAllUndo) { WinMinimizeAllUndo(); }
            if (Action == WinActions.WinRestore) { WinRestore(WinTitle); }
        }
        /// <summary>
        /// Window Actions by WinTitle
        /// </summary>
        /// <param name="WinTitle">Window Title / AHK ID</param>
        /// <param name="Action">WinAction to Perform on WinTitle</param>
        public void WinAction(string WinTitle, string Action)
        {
            if (Action.ToUpper() == WinActions.AlwaysOnTop.ToString().ToUpper()) { AlwaysOnTop(WinTitle, true); }
            if (Action.ToUpper() == WinActions.AlwaysOnTopOff.ToString().ToUpper()) { AlwaysOnTop(WinTitle, false); }
            if (Action.ToUpper() == WinActions.WinSetBottom.ToString().ToUpper()) { WinSetBottom(WinTitle); }
            if (Action.ToUpper() == WinActions.WinSetTop.ToString().ToUpper()) { WinSetTop(WinTitle); }
            if (Action.ToUpper() == WinActions.WinSetEnable.ToString().ToUpper()) { WinSetEnable(WinTitle); }
            if (Action.ToUpper() == WinActions.WinSetDisable.ToString().ToUpper()) { WinSetDisable(WinTitle); }
            if (Action.ToUpper() == WinActions.WinSetRedraw.ToString().ToUpper()) { WinSetRedraw(WinTitle); }
            if (Action.ToUpper() == WinActions.WinActivate.ToString().ToUpper()) { WinActivate(WinTitle); }
            if (Action.ToUpper() == WinActions.WinActivateBottom.ToString().ToUpper()) { WinActivateBottom(WinTitle); }
            if (Action.ToUpper() == WinActions.WinClose.ToString().ToUpper()) { WinClose(WinTitle); }
            if (Action.ToUpper() == WinActions.WinHide.ToString().ToUpper()) { WinHide(WinTitle); }
            if (Action.ToUpper() == WinActions.WinShow.ToString().ToUpper()) { WinShow(WinTitle); }
            if (Action.ToUpper() == WinActions.WinMinimize.ToString().ToUpper()) { WinMinimize(WinTitle); }
            if (Action.ToUpper() == WinActions.WinMaximize.ToString().ToUpper()) { WinMaximize(WinTitle); }
            if (Action.ToUpper() == WinActions.WinKill.ToString().ToUpper()) { WinKill(WinTitle); }
            if (Action.ToUpper() == WinActions.WinMinimizeAll.ToString().ToUpper()) { WinMinimizeAll(); }
            if (Action.ToUpper() == WinActions.WinMinimizeAllUndo.ToString().ToUpper()) { WinMinimizeAllUndo(); }
            if (Action.ToUpper() == WinActions.WinRestore.ToString().ToUpper()) { WinRestore(WinTitle); }
        }


    }
}
