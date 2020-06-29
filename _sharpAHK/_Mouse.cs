using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace sharpAHK
{
    public partial class _AHK
    {
        // === Mouse Control ===

        /// <summary>Clicks a mouse button at the specified coordinates.</summary>
        /// <param name="Command">Ex: Right 44, 55 | Down | 44, 55</param>
        /// <example>ahk.Click("Right 45, 55");</example>
        public void Click(string Command = "")
        {
            ErrorLog_Setup(false);
            Execute("Click " + Command);
        }

        /// <summary>Sets coordinate mode for various commands to be relative to either the active window or the screen.</summary>
        /// <param name="Mode">ToolTip (Affects ToolTip)| Pixel (Affects PixelGetColor, PixelSearch, and ImageSearch)| Mouse (Affects MouseGetPos, Click, and MouseMove/Click/Drag)| Caret (Affects the built-in variables A_CaretX and A_CaretY)| Menu (Affects the "Menu Show" command when coordinates are specified for it)</param>
        /// <param name="RelativeToScreen"> True: Coordinates are relative to the desktop (entire screen). | False: Coordinates are relative to the active window</param>
        public void CoordMode(string Mode = "Mouse", bool RelativeToScreen = true)
        {
            string relative = "Screen";
            if (!RelativeToScreen) { relative = "Relative"; }

            if (Mode.ToUpper() == "MOUSE")
            {
                _MouseCoordMode = relative; // store mouse coord mode to access value through MouseCoodMode()
            }

            ErrorLog_Setup(false);
            Execute("CoordMode, " + Mode + ", " + relative);
        }

        /// <summary>Sets coordinate mode for mouse commands to be relative to either the active window or the screen. Affects MouseGetPos, Click, and MouseMove/Click/Drag.</summary>
        /// <param name="RelativeToScreen"> Leave BLANK to simply return current Mouse CoordMode. | "True" to set the CoordMode relative to Screen. | "False" (Default) to set CoordMode relative to Window</param>
        public string MouseCoordMode(string RelativeToScreen = "")
        {
            if (RelativeToScreen != "")
            {
                bool relativeToScreen = false;
                if (RelativeToScreen.ToUpper() == "SCREEN" || RelativeToScreen.ToUpper() == "TRUE") { relativeToScreen = true; }
                if (RelativeToScreen.ToUpper() == "RELATIVE" || RelativeToScreen.ToUpper() == "FALSE") { relativeToScreen = false; }

                if (relativeToScreen) { _MouseCoordMode = "Screen"; }
                if (!relativeToScreen) { _MouseCoordMode = "Relative"; }

                CoordMode("Mouse", relativeToScreen);
            }

            return _MouseCoordMode;
        }

        /// <summary>
        /// Tracks current mouse cood mode value to return from MouseCoodMode()
        /// </summary>
        private static string _MouseCoordMode = "Relative";

        [DefaultValue(Left)]
        public enum MouseButton
        {
            Left,
            Right,
            CenterWheel
        }

        [DefaultValue(None)]
        public enum MouseState
        {
            None,
            Down,
            Up
        }



        /// <summary>Clicks or holds down a mouse button, or turns the mouse wheel.</summary>
        /// <param name="button">The button to click: Left (default), Right, Middle (or just the first letter of each of these); or the fourth or fifth mouse button (X1 or X2), which are supported on Windows 2000/XP or later. For example: MouseClick, X1. This parameter may be omitted, in which case it defaults to Left. | Rotate the mouse wheel: On Windows NT/2000/XP or later, specify WheelUp or WU to turn the wheel upward (away from you); specify WheelDown or WD to turn the wheel downward (toward you). In v1.0.48+, specify WheelLeft (or WL) or WheelRight (or WR) to push the wheel left or right, respectively (but these have no effect on operating systems older than Windows Vista). ClickCount is the number of notches to turn the wheel.</param>
        /// <param name="X">The x/y coordinates to which the mouse cursor is moved prior to clicking, which can be expressions. Coordinates are relative to the active window unless CoordMode was used to change that. If omitted, the cursor's current position is used.</param>
        /// <param name="Y">The x/y coordinates to which the mouse cursor is moved prior to clicking, which can be expressions. Coordinates are relative to the active window unless CoordMode was used to change that. If omitted, the cursor's current position is used.</param>
        /// <param name="ClickCount">The number of times to click the mouse, which can be an expression.  If omitted, the button is clicked once. </param>
        /// <param name="Speed">The speed to move the mouse in the range 0 (fastest) to 100 (slowest), which can be an expression.  Note: a speed of 0 will move the mouse instantly.  If omitted, the default speed (as set by SetDefaultMouseSpeed or 2 otherwise) will be used.</param>
        /// <param name="D_U">D = Press the mouse button down but do not release it (i.e. generate a down-event). | U = Release the mouse button (i.e. generate an up-event). </param>
        /// <param name="R">If this parameter is the letter R, the X and Y coordinates will be treated as offsets from the current mouse position. In other words, the cursor will be moved from its current position by X pixels to the right (left if negative) and Y pixels down (up if negative).</param>
        public void MouseClick(MouseButton button = MouseButton.Left, object X = null, object Y = null, bool ScreenCoordMode = true, object ClickCount = null, object Speed = null, MouseState state = MouseState.None, bool Relative = false)
        {
            if (X == null) { X = ""; }
            if (Y == null) { Y = ""; }
            if (ClickCount == null) { ClickCount = "1"; }
            if (Speed == null) { Speed = ""; }

            string R = "";
            if (Relative) { R = "R"; }

            string clickCount = "1";
            string speed = "";

            if (ClickCount != null) { clickCount = ClickCount.ToString(); }
            if (Speed != null) { speed = Speed.ToString(); }

            //if (ScreenCoordMode) { CoordMode("Mouse", true); }
            //else { CoordMode("Mouse", false); }

            string D_U = "";
            if (state.ToString() == "None") { D_U = ""; }
            if (state.ToString() == "Down") { D_U = "D"; }
            if (state.ToString() == "Up") { D_U = "U"; }

            ErrorLog_Setup(false);
            Execute("MouseClick, " + button.ToString() + "," + X.ToString() + "," + Y.ToString() + "," + clickCount.ToString() + "," + speed + "," + D_U + "," + R);

            // v1 ToDo
            /*
                public void _MouseClick(int xpos, int ypos, string Button = "Left")  // simulates left/right mouse click on screen
                    {
                        _SetCursorPos(xpos, ypos);

                        Button = Button.ToLower();  // convert options to lower case

                        if (Button == "left")
                        {
                            mouse_event(MOUSEEVENTF_LEFTDOWN, xpos, ypos, 0, 0);
                            mouse_event(MOUSEEVENTF_LEFTUP, xpos, ypos, 0, 0);
                        }
                        if (Button == "right")
                        {
                            mouse_event(MOUSEEVENTF_RIGHTDOWN, xpos, ypos, 0, 0);
                            mouse_event(MOUSEEVENTF_RIGHTUP, xpos, ypos, 0, 0);
                        }
                    }
             */

        }

        /// <summary>Clicks and holds the specified mouse button, moves the mouse to the destination coordinates, then releases the button.</summary>
        /// <param name="WhichButton">The button to click: Left, Right, Middle (or just the first letter of each of these). The fourth and fifth mouse buttons are supported on Windows 2000/XP or later: Specify X1 for the fourth button and X2 for the fifth. For example: MouseClickDrag, X1, ...</param>
        /// <param name="X1">The x/y coordinates of the drag's starting position, which can be expressions (the mouse will be moved to these coordinates right before the drag is started). Coordinates are relative to the active window unless CoordMode was used to change that. If omitted, the mouse's current position is used.</param>
        /// <param name="Y1">The x/y coordinates of the drag's starting position, which can be expressions (the mouse will be moved to these coordinates right before the drag is started). Coordinates are relative to the active window unless CoordMode was used to change that. If omitted, the mouse's current position is used.</param>
        /// <param name="X2">The x/y coordinates to drag the mouse to (that is, while the button is held down), which can be expressions. Coordinates are relative to the active window unless CoordMode was used to change that. </param>
        /// <param name="Y2">The x/y coordinates to drag the mouse to (that is, while the button is held down), which can be expressions. Coordinates are relative to the active window unless CoordMode was used to change that. </param>
        /// <param name="Speed">The speed to move the mouse in the range 0 (fastest) to 100 (slowest), which can be an expression.  Note: a speed of 0 will move the mouse instantly.  If omitted, the default speed (as set by SetDefaultMouseSpeed or 2 otherwise) will be used.</param>
        /// <param name="R">If this parameter is the letter R, the X1 and Y1 coordinates will be treated as offsets from the current mouse position. In other words, the cursor will be moved from its current position by X1 pixels to the right (left if negative) and Y1 pixels down (up if negative). Similarly, the X2 and Y2 coordinates will be treated as offsets from the X1 and Y1 coordinates. For example, the following would first move the cursor down and to the right by 5 pixels from its starting position, and then drag it from that position down and to the right by 10 pixels: MouseClickDrag, Left, 5, 5, 10, 10, , R </param>
        public void MouseClickDrag(string WhichButton, string X1, string Y1, string X2, string Y2, string Speed = "", string R = "")
        {
            ErrorLog_Setup(false);
            Execute("MouseClickDrag, " + WhichButton + "," + X1 + "," + Y1 + "," + X2 + "," + Y2 + "," + Speed + "," + R);
        }

        /// <summary>Retrieves the current position of the mouse cursor, and optionally which window and control it is hovering over.</summary>
        /// <param name="Opt">BLANK or 1 Populate ControlClassNN, 2 and 3 Populate ControlHwnd | 1: Uses a simpler method to determine OutputVarControl. This method correctly retrieves the active/topmost child window of an Multiple Document Interface (MDI) application such as SysEdit or TextPad. However, it is less accurate for other purposes such as detecting controls inside a GroupBox control. | 2: Stores the control's HWND in OutputVarControl rather than the control's ClassNN. | 3: A combination of 1 and 2 </param>
        public mousePos MouseGetPos(string Opt = "")
        {
            string mouseCoodMode = MouseCoordMode(); // store the current mouse mode value

            MouseCoordMode("Relative"); // sets mouse coodinates relative to active window
            string AHKLine = "MouseGetPos, OutXPos, OutYPos, OutWin, OutControl, " + Opt;  // ahk line to execute
            ErrorLog_Setup(true); // ErrorLevel Detection Enabled for this function in AHK 
            Execute(AHKLine);   // execute AHK code and return variable value 

            //return value from ahk session
            string OutXPos = GetVar("OutXPos");
            string OutYPos = GetVar("OutYPos");

            // populate object to return values to user
            mousePos mouseP = new mousePos();
            mouseP.X_Window = Int32.Parse(OutXPos);
            mouseP.Y_Window = Int32.Parse(OutYPos);
            mouseP.WinHwnd = GetVar("OutWin");


            MouseCoordMode("Screen"); // sets mouse coodinates relative to screen
            AHKLine = "MouseGetPos, OutXPos, OutYPos, OutWin, OutControl, " + Opt;  // ahk line to execute
            ErrorLog_Setup(true); // ErrorLevel Detection Enabled for this function in AHK 
            Execute(AHKLine);   // execute AHK code and return variable value 

            //return value from ahk session
            OutXPos = GetVar("OutXPos");
            OutYPos = GetVar("OutYPos");

            // populate object to return values to user
            mouseP.X_Screen = Int32.Parse(OutXPos);
            mouseP.Y_Screen = Int32.Parse(OutYPos);

            if (mouseP.WinHwnd == "" || mouseP.WinHwnd == null)
                mouseP.WinHwnd = GetVar("OutWin");


            if (Opt == "" || Opt == "1")
                mouseP.ControlClassNN = GetVar("OutControl");

            if (Opt == "2" || Opt == "3")
                mouseP.ControlHwnd = GetVar("OutControl");

            return mouseP;



            // v1 ToDo
            /*
                    //==== Get Mouse Pos =============
                    //EX: 
                    //  var MousePos = ahk.GetMousePos();
                    //  string MouseX = MousePos.XPos.ToString();
                    //  string MouseY = MousePos.YPos.ToString();

                    public wInfo _GetMousePos()  // gets the current mouse position, returns wInfo object populated
                    {
                        wInfo MousePos = new wInfo();
                        MousePos.MouseXPos = Cursor.Position.X;
                        MousePos.MouseYPos = Cursor.Position.Y;

                        return MousePos;
                    }
            */

            // v2 ToDo
            /*
                    public wInfo _MouseGetPos()  // gets the current mouse position, returns wInfo object populated (includes control info/handles)
                    {
                        if (ahkGlobal.ahkdll == null) { ahkGlobal.ahkdll = new AutoHotkey.Interop.AutoHotkeyEngine(); }
                        var ahkdll = ahkGlobal.ahkdll;

                        wInfo mouseInfo = new wInfo();

                        //ahkdll.SetVar("OutputVar", "");
                        string Command = "MouseGetPos, MouseX, MouseY, WinID, ControlID, 3";
                        ahkdll.ExecRaw(Command);

                        mouseInfo.MouseXPos = Int32.Parse(ahkdll.GetVar("MouseX"));
                        mouseInfo.MouseYPos = Int32.Parse(ahkdll.GetVar("MouseY"));

                        // convert string to IntPtr
                        string WinHwnd = ahkdll.GetVar("WinID");
                        if (WinHwnd != "")
                        {
                            int WinID = Int32.Parse(WinHwnd);  // string to int
                            mouseInfo.WinHwnd = _ToIntPtr(WinID);  // int to IntPtr
                        }

                        // convert string to IntPtr
                        string ControlHwnd = ahkdll.GetVar("ControlID");
                        if (ControlHwnd != "")
                        {
                            int cID = Int32.Parse(ControlHwnd);  // string to int
                            mouseInfo.ControlHwnd = _ToIntPtr(cID);  // int to IntPtr
                        }

                        //MsgBox("X=" + mouseInfo.MouseXPos.ToString());
                        //MsgBox("Y=" + mouseInfo.MouseYPos.ToString());
                        //MsgBox("WinHwnd=" + mouseInfo.WinHwnd.ToString());
                        //MsgBox("ControlHwnd=" + mouseInfo.ControlHwnd.ToString());

                        _ControlGetPos(null, mouseInfo.ControlHwnd);


                        return mouseInfo;
                    }
            */



        }

        /// <summary>Moves the mouse cursor.</summary>
        /// <param name="X">The x/y coordinates to move the mouse to, which can be expressions. Coordinates are relative to the active window unless CoordMode was used to change that.</param>
        /// <param name="Y">The x/y coordinates to move the mouse to, which can be expressions. Coordinates are relative to the active window unless CoordMode was used to change that.</param>
        /// <param name="Speed">The speed to move the mouse in the range 0 (fastest) to 100 (slowest), which can be an expression.  Note: a speed of 0 will move the mouse instantly.  If omitted, the default speed (as set by SetDefaultMouseSpeed or 2 otherwise) will be used.</param>
        /// <param name="OffSetFromCurrentPos">If this parameter is True, the X and Y coordinates will be treated as offsets from the current mouse position. In other words, the cursor will be moved from its current position by X pixels to the right (left if negative) and Y pixels down (up if negative).</param>
        public void MouseMove(int X, int Y, bool ScreenCoordMode = true, int Speed = 2, bool OffSetFromCurrentPos = false)
        {
            if (ScreenCoordMode) { CoordMode("Mouse", true); }
            else { CoordMode("Mouse", false); }

            string R = "";
            if (OffSetFromCurrentPos) { R = "R"; }

            ErrorLog_Setup(false);
            Execute("MouseMove, " + X.ToString() + "," + Y.ToString() + "," + Speed.ToString() + "," + R);


            // v1 ToDo
            /*
                    public void _MouseMove(int xpos, int ypos)  //moves mouse cursor to position on screen
                    {
                        _SetCursorPos(xpos, ypos);
                    }
            */


        }

        /// <summary>Sets the mouse speed that will be used if unspecified in Click and MouseMove/Click/Drag.</summary>
        /// <param name="Speed">The speed to move the mouse in the range 0 (fastest) to 100 (slowest).  Note: a speed of 0 will move the mouse instantly. This parameter can be an expression.</param>
        public void SetDefaultMouseSpeed(string Speed)
        {
            ErrorLog_Setup(false);
            Execute("SetDefaultMouseSpeed, " + Speed);
        }

        /// <summary>Sets the delay that will occur after each mouse movement or click.</summary>
        /// <param name="Delay">Time in milliseconds, which can be an expression. Use -1 for no delay at all and 0 for the smallest possible delay (however, if the Play parameter is present, both 0 and -1 produce no delay). If unset, the default delay is 10 for the traditional SendEvent mode and -1 for SendPlay mode.</param>
        /// <param name="Play">The word Play applies the delay to the SendPlay mode rather than the traditional Send/SendEvent mode. If a script never uses this parameter, the delay is always -1 for SendPlay.</param>
        public void SetMouseDelay(string Delay, string Play = "")
        {
            ErrorLog_Setup(false);
            Execute("SetDefaultMouseSpeed, " + Delay + "," + Play);
        }

    }
}
