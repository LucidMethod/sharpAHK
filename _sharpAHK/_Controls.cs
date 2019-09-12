using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace sharpAHK
{
    public partial class _AHK
    {
        // === Controls ===

        /// <summary>Makes a variety of changes to a control.</summary>
        /// <param name="Cmd">Check|Uncheck|Enable|Disable|Show|Hide|Style,N|ExStyle,N|ShowDropDown|HideDropDown|TabLeft,Count|TabRight,Count|Add,String|Delete,N|Choose,N|EditPaste,String</param>
        /// <param name="Value">See Cmd options for Value Params</param>
        /// <param name="Control">Can be either ClassNN (the classname and instance number of the control) or the name/text of the control, both of which can be determined via Window Spy. When using name/text, the matching behavior is determined by SetTitleMatchMode. If this parameter is blank, the target window's topmost control will be used. To operate upon a control's HWND (window handle), leave the Control parameter blank and specify ahk_id %ControlHwnd% for the WinTitle parameter (this also works on hidden controls even when DetectHiddenWindows is Off) . The HWND of a control is typically retrieved via ControlGet Hwnd, MouseGetPos, or DllCall.</param>
        /// <param name="WinTitle">The title or partial title of the target window (the matching behavior is determined by SetTitleMatchMode). If this and the other 3 parameters are omitted, the Last Found Window will be used. To use a window class, specify ahk_class ExactClassName (shown by Window Spy). To use a process identifier (PID), specify ahk_pid %VarContainingPID%. To use a window group, specify ahk_group GroupName. To use a window's unique ID number, specify ahk_id %VarContainingID%. The search can be narrowed by specifying multiple criteria. For example: My File.txt ahk_class Notepad</param>
        /// <param name="WinText">If present, this parameter must be a substring from a single text element of the target window (as revealed by the included Window Spy utility). Hidden text elements are detected if DetectHiddenText is ON.</param>
        /// <param name="ExcludeTitle">Windows whose titles include this value will not be considered. Note: Due to backward compatibility with .aut scripts, this parameter will be interpreted as a command if it exactly matches the name of a command. To work around this, use the WinActive() function instead.</param>
        /// <param name="ExcludeText">Windows whose text include this value will not be considered.</param>
        public bool Control(string Cmd, string Value = "", string Control = "", string WinTitle = "", string WinText = "", string ExcludeTitle = "", string ExcludeText = "")
        {
            string AHKLine = "Control, " + Cmd + "," + Value + "," + Control + "," + WinTitle + "," + WinText + "," + ExcludeTitle + "," + ExcludeText;  // ahk line to execute
            ErrorLog_Setup(true, "[Control] ErrorLevel Detected"); // ErrorLevel Detection Enabled for this function in AHK 
            Execute(AHKLine);   // execute AHK code and return variable value

            if (!ahkGlobal.ErrorLevel) { return true; } // no error level - return true for success
            return false;  // error level detected - success = false
        }

        /// <summary>Sends a mouse button or mouse wheel event to a control.</summary>
        /// <param name="ControlOrPos"> (Position): Specify the X and Y coordinates relative to the target window's upper left corner. The X coordinate must precede the Y coordinate and there must be at least one space or tab between them. For example: X55 Y33. If there is a control at the specified coordinates, it will be sent the click-event at those exact coordinates. (ClassNN or Text): Specify either ClassNN (the classname and instance number of the control) or the name/text of the control, both of which can be determined via Window Spy. When using name/text, the matching behavior is determined by SetTitleMatchMode.</param>
        /// <param name="WinTitle">The title or partial title of the target window (the matching behavior is determined by SetTitleMatchMode). If this and the other 3 window parameters are omitted, the Last Found Window will be used. If this is the letter A and the other 3 window parameters are omitted, the active window will be used. To use a window class, specify ahk_class ExactClassName (shown by Window Spy). To use a process identifier (PID), specify ahk_pid %VarContainingPID%. To use a window group, specify ahk_group GroupName. To use a window's unique ID number, specify ahk_id %VarContainingID%. The search can be narrowed by specifying multiple criteria.</param>
        /// <param name="WinText">If present, this parameter must be a substring from a single text element of the target window (as revealed by the included Window Spy utility). Hidden text elements are detected if DetectHiddenText is ON.</param>
        /// <param name="WhichButton">The button to click: LEFT, RIGHT, MIDDLE (or just the first letter of each of these). If omitted or blank, the LEFT button will be used.</param>
        /// <param name="ClickCount">The number of clicks to send, which can be an expression. If omitted or blank, 1 click is sent. </param>
        /// <param name="Options">NA: May improve reliability | D: Press the mouse button down but do not release it (i.e. generate a down-event). | U: Release the mouse button (i.e. generate an up-event). | Pos: Specify the word Pos anywhere in Options to unconditionally use the X/Y positioning mode as described in the Control-or-Pos parameter above. | Xn: Specify for n the X position to click at, relative to the control's upper left corner. If unspecified, the click will occur at the horizontal-center of the control. | Yn: Specify for n the Y position to click at, relative to the control's upper left corner. If unspecified, the click will occur at the vertical-center of the control. </param>
        /// <param name="ExcludeTitle">Windows whose titles include this value will not be considered.</param>
        /// <param name="ExcludeText">Windows whose text include this value will not be considered.</param>
        public bool ControlClick(string ControlOrPos = "", string WinTitle = "", string WinText = "", string WhichButton = "", string ClickCount = "", string Options = "", string ExcludeTitle = "", string ExcludeText = "")
        {
            string AHKLine = "ControlClick, " + ControlOrPos + "," + WinTitle + "," + WinText + "," + WhichButton + "," + ClickCount + "," + Options + "," + ExcludeTitle + "," + ExcludeText;  // ahk line to execute
            ErrorLog_Setup(true); // ErrorLevel Detection Enabled for this function in AHK 
            Execute(AHKLine);   // execute AHK code and return variable value

            if (!ahkGlobal.ErrorLevel) { return true; } // no error level - return true for success
            return false;  // error level detected - success = false
        }

        /// <summary>Sends simulated keystrokes to a window or control.</summary>
        /// <param name="Control">Can be either ClassNN (the classname and instance number of the control) or the name/text of the control, both of which can be determined via Window Spy. When using name/text, the matching behavior is determined by SetTitleMatchMode. If this parameter is blank or omitted, the target window's topmost control will be used. If this parameter is ahk_parent, the keystrokes will be sent directly to the control's parent window.</param>
        /// <param name="Keys">The sequence of keys to send (see the Send command for details). To send a literal comma, escape it (`,). The rate at which characters are sent is determined by SetKeyDelay. Unlike the Send command, mouse clicks cannot be sent by ControlSend. Use ControlClick for that.</param>
        /// <param name="WinTitle">The title or partial title of the target window (the matching behavior is determined by SetTitleMatchMode). If this and the next 3 parameters are omitted, the Last Found Window will be used. If this is the letter A and the next 3 parameters are omitted, the active window will be used. To use a window class, specify ahk_class ExactClassName (shown by Window Spy). To use a process identifier (PID), specify ahk_pid %VarContainingPID%. To use a window group, specify ahk_group GroupName. To use a window's unique ID number, specify ahk_id %VarContainingID%. The search can be narrowed by specifying multiple criteria. For example: My File.txt ahk_class Notepad</param>
        /// <param name="WinText">If present, this parameter must be a substring from a single text element of the target window (as revealed by the included Window Spy utility). Hidden text elements are detected if DetectHiddenText is ON.</param>
        /// <param name="ExcludeTitle">Windows whose titles include this value will not be considered.</param>
        /// <param name="ExcludeText">Windows whose text include this value will not be considered.</param>
        public bool ControlSend(string Control = "", string Keys = "", string WinTitle = "", string WinText = "", string ExcludeTitle = "", string ExcludeText = "")
        {
            string AHKLine = "ControlSend, " + Control + "," + Keys + "," + WinTitle + "," + WinText + "," + ExcludeTitle + "," + ExcludeText;  // ahk line to execute
            ErrorLog_Setup(true); // ErrorLevel Detection Enabled for this function in AHK 
            Execute(AHKLine);   // execute AHK code and return variable value

            if (!ahkGlobal.ErrorLevel) { return true; } // no error level - return true for success
            return false;  // error level detected - success = false
        }

        /// <summary>Sends simulated keystrokes to a window or control. ControlSendRaw sends the keystrokes in the Keys parameter exactly as they appear rather than translating {Enter} to an ENTER keystroke, ^c to Control-C, etc.</summary>
        /// <param name="Control">Can be either ClassNN (the classname and instance number of the control) or the name/text of the control, both of which can be determined via Window Spy. When using name/text, the matching behavior is determined by SetTitleMatchMode. If this parameter is blank or omitted, the target window's topmost control will be used. If this parameter is ahk_parent, the keystrokes will be sent directly to the control's parent window.</param>
        /// <param name="Keys">The sequence of keys to send (see the Send command for details). To send a literal comma, escape it (`,). The rate at which characters are sent is determined by SetKeyDelay. Unlike the Send command, mouse clicks cannot be sent by ControlSend. Use ControlClick for that.</param>
        /// <param name="WinTitle">The title or partial title of the target window (the matching behavior is determined by SetTitleMatchMode). If this and the next 3 parameters are omitted, the Last Found Window will be used. If this is the letter A and the next 3 parameters are omitted, the active window will be used. To use a window class, specify ahk_class ExactClassName (shown by Window Spy). To use a process identifier (PID), specify ahk_pid %VarContainingPID%. To use a window group, specify ahk_group GroupName. To use a window's unique ID number, specify ahk_id %VarContainingID%. The search can be narrowed by specifying multiple criteria. For example: My File.txt ahk_class Notepad</param>
        /// <param name="WinText">If present, this parameter must be a substring from a single text element of the target window (as revealed by the included Window Spy utility). Hidden text elements are detected if DetectHiddenText is ON.</param>
        /// <param name="ExcludeTitle">Windows whose titles include this value will not be considered.</param>
        /// <param name="ExcludeText">Windows whose text include this value will not be considered.</param>
        public bool ControlSendRaw(string Control = "", string Keys = "", string WinTitle = "", string WinText = "", string ExcludeTitle = "", string ExcludeText = "")
        {
            string AHKLine = "ControlSendRaw, " + Control + "," + Keys + "," + WinTitle + "," + WinText + "," + ExcludeTitle + "," + ExcludeText;  // ahk line to execute
            ErrorLog_Setup(true); // ErrorLevel Detection Enabled for this function in AHK 
            Execute(AHKLine);   // execute AHK code and return variable value

            if (!ahkGlobal.ErrorLevel) { return true; } // no error level - return true for success
            return false;  // error level detected - success = false
        }

        /// <summary>Sets input focus to a given control on a window.</summary>
        /// <param name="Control">Can be either ClassNN (the classname and instance number of the control) or the name/text of the control, both of which can be determined via Window Spy. When using name/text, the matching behavior is determined by SetTitleMatchMode. If this parameter is blank, the target window's topmost control will be used. To operate upon a control's HWND (window handle), leave the Control parameter blank and specify ahk_id %ControlHwnd% for the WinTitle parameter (this also works on hidden controls even when DetectHiddenWindows is Off) . The HWND of a control is typically retrieved via ControlGet Hwnd, MouseGetPos, or DllCall.</param>
        /// <param name="WinTitle">The title or partial title of the target window (the matching behavior is determined by SetTitleMatchMode). If this and the other 3 parameters are omitted, the Last Found Window will be used. To use a window class, specify ahk_class ExactClassName (shown by Window Spy). To use a process identifier (PID), specify ahk_pid %VarContainingPID%. To use a window group, specify ahk_group GroupName. To use a window's unique ID number, specify ahk_id %VarContainingID%. The search can be narrowed by specifying multiple criteria. For example: My File.txt ahk_class Notepad</param>
        /// <param name="WinText">If present, this parameter must be a substring from a single text element of the target window (as revealed by the included Window Spy utility). Hidden text elements are detected if DetectHiddenText is ON.</param>
        /// <param name="ExcludeTitle">Windows whose titles include this value will not be considered. Note: Due to backward compatibility with .aut scripts, this parameter will be interpreted as a command if it exactly matches the name of a command. To work around this, use the WinActive() function instead.</param>
        /// <param name="ExcludeText">Windows whose text include this value will not be considered.</param>
        public bool ControlFocus(string Control = "", string WinTitle = "", string WinText = "", string ExcludeTitle = "", string ExcludeText = "")
        {
            string AHKLine = "ControlFocus, " + Control + "," + WinTitle + "," + WinText + "," + ExcludeTitle + "," + ExcludeText;  // ahk line to execute
            ErrorLog_Setup(true, "[ControlFocus] ErrorLevel Detected"); // ErrorLevel Detection Enabled for this function in AHK 
            Execute(AHKLine);   // execute AHK code and return variable value

            if (!ahkGlobal.ErrorLevel) { return true; } // no error level - return true for success
            return false;  // error level detected - success = false
        }

        /// <summary>Retrieves various types of information about a control.</summary>
        /// <param name="Cmd">Checked|Enabled|Visible|Tab|FindString,String|Choice|LineCount|CurrentLine|CurrentCol|Line,N|Selected|Style|ExStyle|Hwnd|List,Selected|Focused|Col#|Count|Count Selected|Count Focused|Count Col </param>
        /// <param name="Value">See Cmd Options</param>
        /// <param name="Control">Can be either ClassNN (the classname and instance number of the control) or the name/text of the control, both of which can be determined via Window Spy. When using name/text, the matching behavior is determined by SetTitleMatchMode. If this parameter is blank, the target window's topmost control will be used. To operate upon a control's HWND (window handle), leave the Control parameter blank and specify ahk_id %ControlHwnd% for the WinTitle parameter (this also works on hidden controls even when DetectHiddenWindows is Off) . The HWND of a control is typically retrieved via ControlGet Hwnd, MouseGetPos, or DllCall.</param>
        /// <param name="WinTitle">The title or partial title of the target window (the matching behavior is determined by SetTitleMatchMode). If this and the other 3 parameters are omitted, the Last Found Window will be used. To use a window class, specify ahk_class ExactClassName (shown by Window Spy). To use a process identifier (PID), specify ahk_pid %VarContainingPID%. To use a window group, specify ahk_group GroupName. To use a window's unique ID number, specify ahk_id %VarContainingID%. The search can be narrowed by specifying multiple criteria. For example: My File.txt ahk_class Notepad</param>
        /// <param name="WinText">If present, this parameter must be a substring from a single text element of the target window (as revealed by the included Window Spy utility). Hidden text elements are detected if DetectHiddenText is ON.</param>
        /// <param name="ExcludeTitle">Windows whose titles include this value will not be considered. Note: Due to backward compatibility with .aut scripts, this parameter will be interpreted as a command if it exactly matches the name of a command. To work around this, use the WinActive() function instead.</param>
        /// <param name="ExcludeText">Windows whose text include this value will not be considered.</param>
        public string ControlGet(string Cmd, string Value = "", string Control = "", string WinTitle = "", string WinText = "", string ExcludeTitle = "", string ExcludeText = "")
        {
            string AHKLine = "ControlGet, OutputVar, " + Cmd + "," + Value + "," + Control + "," + WinTitle + "," + WinText + "," + ExcludeTitle + "," + ExcludeText;  // ahk line to execute
            ErrorLog_Setup(true); // ErrorLevel Detection Enabled for this function in AHK 
            string OutVar = Execute(AHKLine, "OutputVar");   // execute AHK code and return variable value 
            return OutVar;


            // v1 ToDo
            /*
                    _ControlGet(string Cmd, string Value = "", object Control = null, object WinTitle = null, string WinText = "", string ExcludeTitle = "", string ExcludeText = "")  // function to get info about a control (controlget function wrapper for ahk dll)
                    {
                        string ControlID = "";
                        string SearchWin = "";

                        if (Control != null && WinTitle != null)
                        {
                            IntPtr hWnd = _WinHwnd(WinTitle);  //returns Window Handle (from either handle or window title)
                            SearchWin = "ahk_id " + hWnd.ToString();
                        }

                        if (Control != null)
                        {
                            string ControlVarType = Control.GetType().ToString();  //determine what kind of variable was passed into function

                            // user passed in the control name/class as string
                            if (ControlVarType == "System.String") { ControlID = Control.ToString(); }

                            // user passed in a control handle, set to the SearchWin parameter to search by the control id
                            if (ControlVarType == "System.IntPtr") { ControlID = ""; SearchWin = "ahk_id " + Control.ToString(); }
                        }


                        if (ahkGlobal.ahkdll == null) { ahkGlobal.ahkdll = new AutoHotkey.Interop.AutoHotkeyEngine(); }
                        var ahkdll = ahkGlobal.ahkdll;

                        string Command = "ControlGet,OutputVar," + Cmd + "," + Value + "," + ControlID + "," + SearchWin + "," + WinText + "," + ExcludeTitle + "," + ExcludeText;
                        ahkdll.ExecRaw(Command);
                        string OutputVar = ahkdll.GetVar("OutputVar");

                        //MsgBox(OutputVar);


                        //   Error Handling
                        ahkGlobal.ErrorLevelMsg = "ControlGet Unable Return Value. Window/Control May Not Exist";
                        ahkGlobal.ErrorLevel = false;
                        string ErrorLevelValue = ahkdll.GetVar("ErrorLevel");
                        if (ErrorLevelValue == "0") { ahkGlobal.ErrorLevelMsg = ""; }
                        if (ErrorLevelValue == "1") { ahkGlobal.ErrorLevel = true; }
                        if (ahkGlobal.Debug) { if (ahkGlobal.ErrorLevel) { MsgBox(ahkGlobal.ErrorLevelMsg, "ErrorLevel"); } } //Display Error Messages in Debug Mode


                        return OutputVar;
                    }
            */

        }

        /// <summary>Retrieves which control of the target window has input focus, if any.</summary>
        /// <param name="WinTitle">The title or partial title of the target window (the matching behavior is determined by SetTitleMatchMode). If this and the other 3 parameters are omitted, the Last Found Window will be used. To use a window class, specify ahk_class ExactClassName (shown by Window Spy). To use a process identifier (PID), specify ahk_pid %VarContainingPID%. To use a window group, specify ahk_group GroupName. To use a window's unique ID number, specify ahk_id %VarContainingID%. The search can be narrowed by specifying multiple criteria. For example: My File.txt ahk_class Notepad</param>
        /// <param name="WinText">If present, this parameter must be a substring from a single text element of the target window (as revealed by the included Window Spy utility). Hidden text elements are detected if DetectHiddenText is ON.</param>
        /// <param name="ExcludeTitle">Windows whose titles include this value will not be considered. Note: Due to backward compatibility with .aut scripts, this parameter will be interpreted as a command if it exactly matches the name of a command. To work around this, use the WinActive() function instead.</param>
        /// <param name="ExcludeText">Windows whose text include this value will not be considered.</param>
        public string ControlGetFocus(string WinTitle = "", string WinText = "", string ExcludeTitle = "", string ExcludeText = "")
        {
            string AHKLine = "ControlGetFocus, OutputVar, " + WinTitle + "," + WinText + "," + ExcludeTitle + "," + ExcludeText;  // ahk line to execute
            ErrorLog_Setup(true); // ErrorLevel Detection Enabled for this function in AHK 
            string OutVar = Execute(AHKLine, "OutputVar");   // execute AHK code and return variable value 
            return OutVar;
        }

        /// <summary>Retrieves the position and size of a control.</summary>
        /// <param name="Control">Can be either ClassNN (the classname and instance number of the control) or the name/text of the control, both of which can be determined via Window Spy. When using name/text, the matching behavior is determined by SetTitleMatchMode. If this parameter is blank, the target window's topmost control will be used. To operate upon a control's HWND (window handle), leave the Control parameter blank and specify ahk_id %ControlHwnd% for the WinTitle parameter (this also works on hidden controls even when DetectHiddenWindows is Off) . The HWND of a control is typically retrieved via ControlGet Hwnd, MouseGetPos, or DllCall.</param>
        /// <param name="WinTitle">The title or partial title of the target window (the matching behavior is determined by SetTitleMatchMode). If this and the other 3 parameters are omitted, the Last Found Window will be used. To use a window class, specify ahk_class ExactClassName (shown by Window Spy). To use a process identifier (PID), specify ahk_pid %VarContainingPID%. To use a window group, specify ahk_group GroupName. To use a window's unique ID number, specify ahk_id %VarContainingID%. The search can be narrowed by specifying multiple criteria. For example: My File.txt ahk_class Notepad</param>
        /// <param name="WinText">If present, this parameter must be a substring from a single text element of the target window (as revealed by the included Window Spy utility). Hidden text elements are detected if DetectHiddenText is ON.</param>
        /// <param name="ExcludeTitle">Windows whose titles include this value will not be considered. Note: Due to backward compatibility with .aut scripts, this parameter will be interpreted as a command if it exactly matches the name of a command. To work around this, use the WinActive() function instead.</param>
        /// <param name="ExcludeText">Windows whose text include this value will not be considered.</param>
        public Rect ControlGetPos(string Control = "", string WinTitle = "", string WinText = "", string ExcludeTitle = "", string ExcludeText = "")
        {
            string AHKLine = "ControlGetPos, OutX, OutY, OutW, OutH, " + Control + "," + WinTitle + "," + WinText + "," + ExcludeTitle + "," + ExcludeText;  // ahk line to execute
            ErrorLog_Setup(true); // ErrorLevel Detection Enabled for this function in AHK 
            string OutX = Execute(AHKLine, "OutX");   // execute AHK code and return variable value 

            string OutY = GetVar("OutY");
            string OutW = GetVar("OutW");
            string OutH = GetVar("OutH");

            //string OutString = "X" + OutX + " Y" + OutY + " W" + OutW + " H" + OutH;

            Rect rec = new Rect();
            rec.Left = ToInt(OutX);
            rec.Top = ToInt(OutY);
            rec.Width = ToInt(OutW);
            rec.Height = ToInt(OutH);

            return rec;


            // v1 ToDo
            /*
                    conInfo _ControlGetPos(object Control = null, object WinTitle = null, string WinText = "", string ExcludeTitle = "", string ExcludeText = "")  //returns wInfo object with control's position 
                    {
                        conInfo con = new conInfo();
                        string ControlID = "";
                        string SearchWin = "";

                        if (Control != null && WinTitle != null)
                        {
                            IntPtr hWnd = _WinHwnd(WinTitle);  //returns Window Handle (from either handle or window title)
                            SearchWin = "ahk_id " + hWnd.ToString();
                        }

                        if (Control != null)
                        {
                            string ControlVarType = Control.GetType().ToString();  //determine what kind of variable was passed into function

                            // user passed in the control name/class as string
                            if (ControlVarType == "System.String") { ControlID = Control.ToString(); }

                            // user passed in a control handle, set to the SearchWin parameter to search by the control id
                            if (ControlVarType == "System.IntPtr") { ControlID = ""; SearchWin = "ahk_id " + Control.ToString(); }
                        }


                        if (ahkGlobal.ahkdll == null) { ahkGlobal.ahkdll = new AutoHotkey.Interop.AutoHotkeyEngine(); }
                        var ahkdll = ahkGlobal.ahkdll;

                        string Command = "ControlGetPos, ControlX, ControlY, ControlW, ControlH," + ControlID + "," + SearchWin + "," + WinText + "," + ExcludeTitle + "," + ExcludeText;
                        ahkdll.ExecRaw(Command);

                        string contX = ahkdll.GetVar("ControlX");
                        string contY = ahkdll.GetVar("ControlY");
                        string contW = ahkdll.GetVar("ControlW");
                        string contH = ahkdll.GetVar("ControlH");

                        if (contX != "")
                        {
                            con.ControlX = Int32.Parse(contX);
                            con.ControlY = Int32.Parse(contY);
                            con.ControlW = Int32.Parse(contW);
                            con.ControlH = Int32.Parse(contH);
                        }

                        return con;
                    }
            */


        }

        /// <summary>
        /// Returns a control's handle (control get wrapper for ahk.dll) 
        /// </summary>
        /// <param name="Control"></param>
        /// <param name="WinTitle"></param>
        /// <param name="WinText"></param>
        /// <param name="ExcludeTitle"></param>
        /// <param name="ExcludeText"></param>
        /// <returns></returns>
        public IntPtr ControlGetHwnd(object Control = null, object WinTitle = null, string WinText = "", string ExcludeTitle = "", string ExcludeText = "")
        {
            string ControlID = "";
            string SearchWin = "";

            if (Control != null && WinTitle != null)
            {
                IntPtr hWnd = WinHwnd(WinTitle);  //returns Window Handle (from either handle or window title)
                SearchWin = "ahk_id " + hWnd.ToString();
            }

            if (Control != null)
            {
                string ControlVarType = Control.GetType().ToString();  //determine what kind of variable was passed into function

                // user passed in the control name/class as string
                if (ControlVarType == "System.String") { ControlID = Control.ToString(); }

                // user passed in a control handle, set to the SearchWin parameter to search by the control id
                if (ControlVarType == "System.IntPtr") { ControlID = ""; SearchWin = "ahk_id " + Control.ToString(); }
            }


            if (ahkGlobal.ahkdll == null) { ahkGlobal.ahkdll = new AutoHotkey.Interop.AutoHotkeyEngine(); }
            var ahkdll = ahkGlobal.ahkdll;

            string Command = "ControlGet,OutputVar,Hwnd,," + ControlID + "," + SearchWin + "," + WinText + "," + ExcludeTitle + "," + ExcludeText;
            ahkdll.ExecRaw(Command);
            string OutputVar = ahkdll.GetVar("OutputVar");

            int ID = Int32.Parse(OutputVar);  // string to int
            IntPtr ControlHwnd = ToIntPtr(ID);  // int to IntPtr


            //MsgBox(OutputVar);

            //   Error Handling
            ahkGlobal.ErrorLevelMsg = "ControlGet Unable Return Value. Window/Control May Not Exist";
            ahkGlobal.ErrorLevel = false;
            string ErrorLevelValue = ahkdll.GetVar("ErrorLevel");
            if (ErrorLevelValue == "0") { ahkGlobal.ErrorLevelMsg = ""; }
            if (ErrorLevelValue == "1") { ahkGlobal.ErrorLevel = true; }
            if (ahkGlobal.Debug) { if (ahkGlobal.ErrorLevel) { MsgBox(ahkGlobal.ErrorLevelMsg, "ErrorLevel"); } } //Display Error Messages in Debug Mode


            return ControlHwnd;
        }

        /// <summary>Retrieves text from a control.</summary>
        /// <param name="Control">Can be either ClassNN (the classname and instance number of the control) or the name/text of the control, both of which can be determined via Window Spy. When using name/text, the matching behavior is determined by SetTitleMatchMode. If this parameter is blank, the target window's topmost control will be used. To operate upon a control's HWND (window handle), leave the Control parameter blank and specify ahk_id %ControlHwnd% for the WinTitle parameter (this also works on hidden controls even when DetectHiddenWindows is Off) . The HWND of a control is typically retrieved via ControlGet Hwnd, MouseGetPos, or DllCall.</param>
        /// <param name="WinTitle">The title or partial title of the target window (the matching behavior is determined by SetTitleMatchMode). If this and the other 3 parameters are omitted, the Last Found Window will be used. To use a window class, specify ahk_class ExactClassName (shown by Window Spy). To use a process identifier (PID), specify ahk_pid %VarContainingPID%. To use a window group, specify ahk_group GroupName. To use a window's unique ID number, specify ahk_id %VarContainingID%. The search can be narrowed by specifying multiple criteria. For example: My File.txt ahk_class Notepad</param>
        /// <param name="WinText">If present, this parameter must be a substring from a single text element of the target window (as revealed by the included Window Spy utility). Hidden text elements are detected if DetectHiddenText is ON.</param>
        /// <param name="ExcludeTitle">Windows whose titles include this value will not be considered. Note: Due to backward compatibility with .aut scripts, this parameter will be interpreted as a command if it exactly matches the name of a command. To work around this, use the WinActive() function instead.</param>
        /// <param name="ExcludeText">Windows whose text include this value will not be considered.</param>
        public string ControlGetText(string Control = "", string WinTitle = "", string WinText = "", string ExcludeTitle = "", string ExcludeText = "")
        {
            string AHKLine = "ControlGetText, OutputVar, " + Control + "," + WinTitle + "," + WinText + "," + ExcludeTitle + "," + ExcludeText;  // ahk line to execute
            ErrorLog_Setup(true); // ErrorLevel Detection Enabled for this function in AHK 
            string OutString = Execute(AHKLine, "OutputVar");   // execute AHK code and return variable value 
            return OutString;

            // v1 ToDo
            /*
                    string _ControlGetText(object Control = null, object WinTitle = null, string WinText = "", string ExcludeTitle = "", string ExcludeText = "")  // gets text for control 
                    {
                        string ControlID = "";
                        string SearchWin = "";

                        if (Control != null && WinTitle != null)
                        {
                            IntPtr hWnd = _WinHwnd(WinTitle);  //returns Window Handle (from either handle or window title)
                            SearchWin = "ahk_id " + hWnd.ToString();
                        }

                        if (Control != null)
                        {
                            string ControlVarType = Control.GetType().ToString();  //determine what kind of variable was passed into function

                            // user passed in the control name/class as string
                            if (ControlVarType == "System.String") { ControlID = Control.ToString(); }

                            // user passed in a control handle, set to the SearchWin parameter to search by the control id
                            if (ControlVarType == "System.IntPtr") { ControlID = ""; SearchWin = "ahk_id " + Control.ToString(); }
                        }


                        if (ahkGlobal.ahkdll == null) { ahkGlobal.ahkdll = new AutoHotkey.Interop.AutoHotkeyEngine(); }
                        var ahkdll = ahkGlobal.ahkdll;

                        ahkdll.SetVar("OutputVar", "");
                        string Command = "ControlGetText, OutputVar, " + ControlID + "," + SearchWin + "," + WinText + "," + ExcludeTitle + "," + ExcludeText;
                        ahkdll.ExecRaw(Command);
                        string WinTextReturn = ahkdll.GetVar("OutputVar");

                        //   Error Handling
                        ahkGlobal.ErrorLevelMsg = "ControlGetText Unable To Read Control Text";
                        ahkGlobal.ErrorLevel = false;
                        string ErrorLevelValue = ahkdll.GetVar("ErrorLevel");
                        if (ErrorLevelValue == "0") { ahkGlobal.ErrorLevelMsg = ""; }
                        if (ErrorLevelValue == "1") { ahkGlobal.ErrorLevel = true; }
                        if (ahkGlobal.Debug) { if (ahkGlobal.ErrorLevel) { MsgBox(ahkGlobal.ErrorLevelMsg, "ErrorLevel"); } } //Display Error Messages in Debug Mode

                        return WinTextReturn;
                    }
            */

        }

        /// <summary>Changes the text of a control</summary>
        /// <param name="Control">Can be either ClassNN (the classname and instance number of the control) or the name/text of the control, both of which can be determined via Window Spy. When using name/text, the matching behavior is determined by SetTitleMatchMode. If this parameter is blank, the target window's topmost control will be used. To operate upon a control's HWND (window handle), leave the Control parameter blank and specify ahk_id %ControlHwnd% for the WinTitle parameter (this also works on hidden controls even when DetectHiddenWindows is Off) . The HWND of a control is typically retrieved via ControlGet Hwnd, MouseGetPos, or DllCall.</param>
        /// <param name="NewText"> </param>
        /// <param name="WinTitle">The title or partial title of the target window (the matching behavior is determined by SetTitleMatchMode). If this and the other 3 parameters are omitted, the Last Found Window will be used. To use a window class, specify ahk_class ExactClassName (shown by Window Spy). To use a process identifier (PID), specify ahk_pid %VarContainingPID%. To use a window group, specify ahk_group GroupName. To use a window's unique ID number, specify ahk_id %VarContainingID%. The search can be narrowed by specifying multiple criteria. For example: My File.txt ahk_class Notepad</param>
        /// <param name="WinText">If present, this parameter must be a substring from a single text element of the target window (as revealed by the included Window Spy utility). Hidden text elements are detected if DetectHiddenText is ON.</param>
        /// <param name="ExcludeTitle">Windows whose titles include this value will not be considered. Note: Due to backward compatibility with .aut scripts, this parameter will be interpreted as a command if it exactly matches the name of a command. To work around this, use the WinActive() function instead.</param>
        /// <param name="ExcludeText">Windows whose text include this value will not be considered.</param>
        public bool ControlSetText(string Control = "", string NewText = "", string WinTitle = "", string WinText = "", string ExcludeTitle = "", string ExcludeText = "")
        {
            string AHKLine = "ControlSetText, " + Control + "," + NewText + "," + WinTitle + "," + WinText + "," + ExcludeTitle + "," + ExcludeText;  // ahk line to execute
            ErrorLog_Setup(true); // ErrorLevel Detection Enabled for this function in AHK 
            Execute(AHKLine);   // execute AHK code and return variable value 

            if (!ahkGlobal.ErrorLevel) { return true; } // no error level - return true for success
            return false;  // error level detected - success = false


            // v1 ToDo
            /*
                    bool _ControlSetText(object Control = null, string NewText = "", object WinTitle = null, string WinText = "", string ExcludeTitle = "", string ExcludeText = "")  // sets new text on control
                    {
                        string ControlID = "";
                        string SearchWin = "";

                        if (Control != null && WinTitle != null)
                        {
                            IntPtr hWnd = _WinHwnd(WinTitle);  //returns Window Handle (from either handle or window title)
                            SearchWin = "ahk_id " + hWnd.ToString();
                        }

                        if (Control != null)
                        {
                            string ControlVarType = Control.GetType().ToString();  //determine what kind of variable was passed into function

                            // user passed in the control name/class as string
                            if (ControlVarType == "System.String") { ControlID = Control.ToString(); }

                            // user passed in a control handle, set to the SearchWin parameter to search by the control id
                            if (ControlVarType == "System.IntPtr") { ControlID = ""; SearchWin = "ahk_id " + Control.ToString(); }
                        }


                        if (ahkGlobal.ahkdll == null) { ahkGlobal.ahkdll = new AutoHotkey.Interop.AutoHotkeyEngine(); }
                        var ahkdll = ahkGlobal.ahkdll;


                        //ahkdll.SetVar("OutputVar", "");
                        string Command = "ControlSetText, " + ControlID + "," + NewText + "," + SearchWin + "," + WinText + "," + ExcludeTitle + "," + ExcludeText;
                        ahkdll.ExecRaw(Command);



                        //   Error Handling
                        ahkGlobal.ErrorLevelMsg = "ControlSetText Failed";
                        ahkGlobal.ErrorLevel = false;
                        string ErrorLevelValue = ahkdll.GetVar("ErrorLevel");
                        if (ErrorLevelValue == "0") { ahkGlobal.ErrorLevelMsg = ""; }
                        if (ErrorLevelValue == "1") { ahkGlobal.ErrorLevel = true; }
                        if (ahkGlobal.Debug) { if (ahkGlobal.ErrorLevel) { MsgBox(ahkGlobal.ErrorLevelMsg, "ErrorLevel"); } } //Display Error Messages in Debug Mode


                        return ahkGlobal.ErrorLevel;
                    }
            */

        }

        /// <summary>Moves or resizes a control.</summary>
        /// <param name="Control">Can be either ClassNN (the classname and instance number of the control) or the name/text of the control, both of which can be determined via Window Spy. When using name/text, the matching behavior is determined by SetTitleMatchMode. If this parameter is blank, the target window's topmost control will be used. To operate upon a control's HWND (window handle), leave the Control parameter blank and specify ahk_id %ControlHwnd% for the WinTitle parameter (this also works on hidden controls even when DetectHiddenWindows is Off) . The HWND of a control is typically retrieved via ControlGet Hwnd, MouseGetPos, or DllCall.</param>
        /// <param name="X">The X and Y coordinates (in pixels) of the upper left corner of Control's new location, which can be expressions. If either coordinate is blank, Control's position in that dimension will not be changed. The coordinates are relative to the upper-left corner of the Control's parent window; ControlGetPos or Window Spy can be used to determine them.</param>
        /// <param name="Y">The X and Y coordinates (in pixels) of the upper left corner of Control's new location, which can be expressions. If either coordinate is blank, Control's position in that dimension will not be changed. The coordinates are relative to the upper-left corner of the Control's parent window; ControlGetPos or Window Spy can be used to determine them.</param>
        /// <param name="W">The new width and height of Control (in pixels), which can be expressions. If either parameter is blank or omitted, Control's size in that dimension will not be changed.</param>
        /// <param name="H">The new width and height of Control (in pixels), which can be expressions. If either parameter is blank or omitted, Control's size in that dimension will not be changed.</param>
        /// <param name="WinTitle">The title or partial title of the target window (the matching behavior is determined by SetTitleMatchMode). If this and the other 3 parameters are omitted, the Last Found Window will be used. To use a window class, specify ahk_class ExactClassName (shown by Window Spy). To use a process identifier (PID), specify ahk_pid %VarContainingPID%. To use a window group, specify ahk_group GroupName. To use a window's unique ID number, specify ahk_id %VarContainingID%. The search can be narrowed by specifying multiple criteria. For example: My File.txt ahk_class Notepad</param>
        /// <param name="WinText">If present, this parameter must be a substring from a single text element of the target window (as revealed by the included Window Spy utility). Hidden text elements are detected if DetectHiddenText is ON.</param>
        /// <param name="ExcludeTitle">Windows whose titles include this value will not be considered. Note: Due to backward compatibility with .aut scripts, this parameter will be interpreted as a command if it exactly matches the name of a command. To work around this, use the WinActive() function instead.</param>
        /// <param name="ExcludeText">Windows whose text include this value will not be considered.</param>
        public bool ControlMove(string Control, string X, string Y, string W, string H, string WinTitle = "", string WinText = "", string ExcludeTitle = "", string ExcludeText = "")
        {
            string AHKLine = "ControlMove, " + Control + "," + X + "," + Y + "," + W + "," + H + "," + WinTitle + "," + WinText + "," + ExcludeTitle + "," + ExcludeText;  // ahk line to execute
            ErrorLog_Setup(true); // ErrorLevel Detection Enabled for this function in AHK 
            Execute(AHKLine);   // execute AHK code and return variable value 

            if (!ahkGlobal.ErrorLevel) { return true; } // no error level - return true for success
            return false;  // error level detected - success = false
        }

        /// <summary>Sets the delay that will occur after each control-modifying command.</summary>
        /// <param name="Delay">Time in milliseconds, which can be an expression. Use -1 for no delay at all and 0 for the smallest possible delay. If unset, the default delay is 20.</param>
        public void SetControlDelay(string Delay)
        {
            string AHKLine = "SetControlDelay, " + Delay;  // ahk line to execute
            ErrorLog_Setup(false); // ErrorLevel Detection Enabled for this function in AHK 
            Execute(AHKLine);   // execute AHK code and return variable value 
        }

        /// <summary>Sends a message to a window or control</summary>
        /// <param name="Msg">The message number to send, which can be an expression. See the message list in AHK documentation to determine the number. </param>
        /// <param name="wParam">The first component of the message, which can be an expression. If blank or omitted, 0 will be sent.</param>
        /// <param name="lParam">The second component of the message, which can be an expression. If blank or omitted, 0 will be sent.</param>
        /// <param name="Control">Can be either ClassNN (the classname and instance number of the control) or the name/text of the control, both of which can be determined via Window Spy. When using name/text, the matching behavior is determined by SetTitleMatchMode. If this parameter is blank, the target window's topmost control will be used. To operate upon a control's HWND (window handle), leave the Control parameter blank and specify ahk_id %ControlHwnd% for the WinTitle parameter (this also works on hidden controls even when DetectHiddenWindows is Off) . The HWND of a control is typically retrieved via ControlGet Hwnd, MouseGetPos, or DllCall.</param>
        /// <param name="WinTitle">The title or partial title of the target window (the matching behavior is determined by SetTitleMatchMode). If this and the other 3 parameters are omitted, the Last Found Window will be used. To use a window class, specify ahk_class ExactClassName (shown by Window Spy). To use a process identifier (PID), specify ahk_pid %VarContainingPID%. To use a window group, specify ahk_group GroupName. To use a window's unique ID number, specify ahk_id %VarContainingID%. The search can be narrowed by specifying multiple criteria. For example: My File.txt ahk_class Notepad</param>
        /// <param name="WinText">If present, this parameter must be a substring from a single text element of the target window (as revealed by the included Window Spy utility). Hidden text elements are detected if DetectHiddenText is ON.</param>
        /// <param name="ExcludeTitle">Windows whose titles include this value will not be considered. Note: Due to backward compatibility with .aut scripts, this parameter will be interpreted as a command if it exactly matches the name of a command. To work around this, use the WinActive() function instead.</param>
        /// <param name="ExcludeText">Windows whose text include this value will not be considered.</param>
        public bool PostMessage(string Msg, string wParam = "", string lParam = "", string Control = "", string WinTitle = "", string WinText = "", string ExcludeTitle = "", string ExcludeText = "")
        {
            string AHKLine = "PostMessage, " + Msg + "," + wParam + "," + lParam + "," + Control + "," + WinTitle + "," + WinText + "," + ExcludeTitle + "," + ExcludeText;  // ahk line to execute
            ErrorLog_Setup(true); // ErrorLevel Detection Enabled for this function in AHK 
            Execute(AHKLine);   // execute AHK code and return variable value 

            if (!ahkGlobal.ErrorLevel) { return true; } // no error level - return true for success
            return false;  // error level detected - success = false
        }

        /// <summary>Sends a message to a window or control (SendMessage additionally waits for acknowledgement).</summary>
        /// <param name="Msg">The message number to send, which can be an expression. See the message list in AHK documentation to determine the number. </param>
        /// <param name="wParam">The first component of the message, which can be an expression. If blank or omitted, 0 will be sent.</param>
        /// <param name="lParam">The second component of the message, which can be an expression. If blank or omitted, 0 will be sent.</param>
        /// <param name="Control">Can be either ClassNN (the classname and instance number of the control) or the name/text of the control, both of which can be determined via Window Spy. When using name/text, the matching behavior is determined by SetTitleMatchMode. If this parameter is blank, the target window's topmost control will be used. To operate upon a control's HWND (window handle), leave the Control parameter blank and specify ahk_id %ControlHwnd% for the WinTitle parameter (this also works on hidden controls even when DetectHiddenWindows is Off) . The HWND of a control is typically retrieved via ControlGet Hwnd, MouseGetPos, or DllCall.</param>
        /// <param name="WinTitle">The title or partial title of the target window (the matching behavior is determined by SetTitleMatchMode). If this and the other 3 parameters are omitted, the Last Found Window will be used. To use a window class, specify ahk_class ExactClassName (shown by Window Spy). To use a process identifier (PID), specify ahk_pid %VarContainingPID%. To use a window group, specify ahk_group GroupName. To use a window's unique ID number, specify ahk_id %VarContainingID%. The search can be narrowed by specifying multiple criteria. For example: My File.txt ahk_class Notepad</param>
        /// <param name="WinText">If present, this parameter must be a substring from a single text element of the target window (as revealed by the included Window Spy utility). Hidden text elements are detected if DetectHiddenText is ON.</param>
        /// <param name="ExcludeTitle">Windows whose titles include this value will not be considered. Note: Due to backward compatibility with .aut scripts, this parameter will be interpreted as a command if it exactly matches the name of a command. To work around this, use the WinActive() function instead.</param>
        /// <param name="ExcludeText">Windows whose text include this value will not be considered.</param>
        public bool SendMessage(string Msg, string wParam = "", string lParam = "", string Control = "", string WinTitle = "", string WinText = "", string ExcludeTitle = "", string ExcludeText = "")
        {
            string AHKLine = "SendMessage, " + Msg + "," + wParam + "," + lParam + "," + Control + "," + WinTitle + "," + WinText + "," + ExcludeTitle + "," + ExcludeText;  // ahk line to execute
            ErrorLog_Setup(true); // ErrorLevel Detection Enabled for this function in AHK 
            Execute(AHKLine);   // execute AHK code and return variable value 

            if (!ahkGlobal.ErrorLevel) { return true; } // no error level - return true for success
            return false;  // error level detected - success = false
        }

        /// <summary>Invokes a menu item from the menu bar of the specified window.</summary>
        /// <param name="WinTitle">The title or partial title of the target window (the matching behavior is determined by SetTitleMatchMode). If this and the other 3 parameters are omitted, the Last Found Window will be used. To use a window class, specify ahk_class ExactClassName (shown by Window Spy). To use a process identifier (PID), specify ahk_pid %VarContainingPID%. To use a window group, specify ahk_group GroupName. To use a window's unique ID number, specify ahk_id %VarContainingID%. The search can be narrowed by specifying multiple criteria. For example: My File.txt ahk_class Notepad</param>
        /// <param name="WinText">If present, this parameter must be a substring from a single text element of the target window (as revealed by the included Window Spy utility). Hidden text elements are detected if DetectHiddenText is ON.</param>
        /// <param name="Menu">The name of the top-level menu, e.g. File, Edit, View. It can also be the position of the desired menu item by using 1& to represent the first menu, 2& the second, and so on. </param>
        /// <param name="SubMenu1">The name of the menu item to select or its position</param>
        /// <param name="SubMenu2">If SubMenu1 itself contains a menu, this is the name of the menu item inside, or its position.</param>
        /// <param name="SubMenu3">If SubMenu2 itself contains a menu, this is the name of the menu item inside, or its position.</param>
        /// <param name="SubMenu4">If SubMenu3 itself contains a menu, this is the name of the menu item inside, or its position.</param>
        /// <param name="SubMenu5">If SubMenu4 itself contains a menu, this is the name of the menu item inside, or its position.</param>
        /// <param name="SubMenu6">If SubMenu5 itself contains a menu, this is the name of the menu item inside, or its position.</param>
        /// <param name="ExcludeTitle">Windows whose titles include this value will not be considered.</param>
        /// <param name="ExcludeText">Windows whose text include this value will not be considered.</param>
        public bool WinMenuSelectItem(string WinTitle, string WinText, string Menu, string SubMenu1 = "", string SubMenu2 = "", string SubMenu3 = "", string SubMenu4 = "", string SubMenu5 = "", string SubMenu6 = "", string ExcludeTitle = "", string ExcludeText = "")
        {
            string AHKLine = "WinMenuSelectItem, " + WinTitle + "," + WinText + "," + Menu + "," + SubMenu1 + "," + SubMenu2 + "," + SubMenu3 + "," + SubMenu4 + "," + SubMenu5 + "," + SubMenu6 + "," + ExcludeTitle + "," + ExcludeText;  // ahk line to execute
            ErrorLog_Setup(true); // ErrorLevel Detection Enabled for this function in AHK 
            Execute(AHKLine);   // execute AHK code and return variable value 

            if (!ahkGlobal.ErrorLevel) { return true; } // no error level - return true for success
            return false;  // error level detected - success = false
        }


    }
}
