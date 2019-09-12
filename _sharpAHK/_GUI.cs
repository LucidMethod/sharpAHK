using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace sharpAHK
{
    public partial class _AHK
    {
        #region === GUI ===


        /// <summary>Updates Text on DisplayControl (works on multi-thread)</summary>
        /// <param name="Text">Text to populate on Control (TextBox/Label/Etc)</param>
        /// <param name="DisplayControl">WinForm Control to Update (TextBox/Label/etc - any that acces Text param</param>
        public void DisplayText(string Text, Control DisplayControl = null)
        {
            if (DisplayControl != null)
            {
                try  // try to update control from another thread
                {
                    MethodInvoker inv = delegate { DisplayControl.Text = Text; };
                    Form formName = DisplayControl.FindForm();  // determine which form this control belongs to 
                    formName.Invoke(inv);
                }
                catch // otherwise update control from current thread
                {
                    DisplayControl.Text = Text;
                }
            }
        }




        //==== AHK GUI Functions ===========

        /// <summary>Creates and manages windows and controls. Such windows can be used as data entry forms or custom user interfaces.</summary>
        /// <param name="subCommand">See AHK Documentation For Options</param>
        /// <param name="Param2">See AHK Documentation For Options</param>
        /// <param name="Param3">See AHK Documentation For Options</param>
        /// <param name="Param4">See AHK Documentation For Options</param>
        public void Gui(string subCommand, string Param2 = "", string Param3 = "", string Param4 = "")
        {
            string AHKLine = "Gui, " + subCommand + "," + Param2 + "," + Param3 + "," + Param4;  // ahk line to execute
            ErrorLog_Setup(false); // ErrorLevel Detection Enabled for this function in AHK 
            Execute(AHKLine);   // execute AHK code and return variable value 
        }

        /// <summary>Makes a variety of changes to a control in a GUI window.</summary>
        /// <param name="subCommand">See AHK Documentation For Options</param>
        /// <param name="ControlID">See AHK Documentation For Options</param>
        /// <param name="Param3">See AHK Documentation For Options</param>
        public bool GuiControl(string subCommand, string ControlID, string Param3 = "")
        {
            string AHKLine = "GuiControl, " + subCommand + "," + ControlID + "," + Param3;  // ahk line to execute
            ErrorLog_Setup(true, "[GuiControl] ErrorLevel Detected"); // ErrorLevel Detection Enabled for this function in AHK 
            Execute(AHKLine);   // execute AHK code and return variable value

            if (!ahkGlobal.ErrorLevel) { return true; } // no error level - return true for success
            return false;  // error level detected - success = false
        }

        /// <summary>Retrieves various types of information about a control in a GUI window.</summary>
        /// <param name="subCommand">See AHK Documentation For Options</param>
        /// <param name="ControlID">See AHK Documentation For Options</param>
        /// <param name="Param3">See AHK Documentation For Options</param>        
        public string GuiControlGet(string subCommand = "", string ControlID = "", string Param3 = "")
        {
            string AHKLine = "GuiControlGet, OutputVar" + subCommand + "," + ControlID + "," + Param3;  // ahk line to execute
            ErrorLog_Setup(true, "[GuiControlGet] ErrorLevel Detected"); // ErrorLevel Detection Enabled for this function in AHK 
            string OutputVar = Execute(AHKLine, "OutputVar");   // execute AHK code and return variable value
            return OutputVar;
        }

        //#InstallKeybdHook has no affect when attempting to run during script use

        /// <summary>Disables or enables the user's ability to interact with the computer via keyboard and mouse.</summary>
        /// <param name="Mode">Mode 1: One of the following words: On: The user is prevented from interacting with the computer (mouse and keyboard input has no effect). | Off: Input is re-enabled. || Mode 2: This mode operates independently of the other two. | Send: The user's keyboard and mouse input is ignored while a Send or SendRaw is in progress (the traditional SendEvent mode only). | This prevents the user's mouse movements and clicks from disrupting the simulated mouse events. | SendAndMouse: A combination of the above two modes.  </param>
        public void BlockInput(string Mode)
        {
            ErrorLog_Setup(false);
            Execute("BlockInput," + Mode);
        }

        /// <summary>Displays script info and a history of the most recent keystrokes and mouse clicks.</summary>
        public void KeyHistory()
        {
            ErrorLog_Setup(false);
            Execute("KeyHistory");
        }


        /// <summary>Checks if a keyboard key or mouse/joystick button is down or up. Also retrieves joystick status.</summary>
        /// <param name="KeyName">This can be just about any single character from the keyboard or one of the key names from the key list, such as a mouse/joystick button (though mouse button state usually cannot be detected on Windows 95/98/Me). Examples: B, 5, LWin, RControl, Alt, Enter, Escape, LButton, MButton, Joy1. Alternatively, an explicit virtual key code such as vkFF may be specified. This is useful in the rare case where a key has no name. The virtual key code of such a key can be determined by following the steps at the bottom fo the key list page. </param>
        /// <param name="Mode">P: Retrieve the physical state | T: Retrieve the toggle state. If omitted, the mode will default to that which retrieves the logical state of the key. This is the state that the OS and the active window believe the key to be in, but is not necessarily the same as the physical state. </param>
        public string GetKeyState(string KeyName, string Mode = "")
        {
            string AHKLine = "GetKeyState, OutputVar" + KeyName + "," + Mode;  // ahk line to execute
            ErrorLog_Setup(true); // ErrorLevel Detection Enabled for this function in AHK 
            string OutVar = Execute(AHKLine, "OutputVar");   // execute AHK code and return variable value 
            return OutVar;
        }


        /// <summary>Returns true if key is currently pressed down</summary>
        /// <param name="Key">This can be just about any single character from the keyboard or one of the key names from the key list, such as a mouse/joystick button (though mouse button state usually cannot be detected on Windows 95/98/Me). Examples: B, 5, LWin, RControl, Alt, Enter, Escape, LButton, MButton, Joy1. Alternatively, an explicit virtual key code such as vkFF may be specified. This is useful in the rare case where a key has no name. The virtual key code of such a key can be determined by following the steps at the bottom fo the key list page. </param>
        public bool KeyDown(string Key)
        {
            string AHKLine = "GetKeyState, OutputVar" + Key + ", P";  // ahk line to execute
            ErrorLog_Setup(true); // ErrorLevel Detection Enabled for this function in AHK 
            string OutVar = Execute(AHKLine, "OutputVar");   // execute AHK code and return variable value 

            if (OutVar == "U") { return false; }
            if (OutVar == "D") { return true; }
            return false;
        }


        /// <summary>Waits for a key or mouse/joystick button to be released or pressed down.</summary>
        /// <param name="KeyName">This can be just about any single character from the keyboard or one of the key names from the key list, such as a mouse/joystick button (though mouse button state usually cannot be detected on Windows 95/98/Me). Examples: B, 5, LWin, RControl, Alt, Enter, Escape, LButton, MButton, Joy1. Alternatively, an explicit virtual key code such as vkFF may be specified. This is useful in the rare case where a key has no name. The virtual key code of such a key can be determined by following the steps at the bottom fo the key list page. </param>
        /// <param name="Options">D: Wait for the key to be pushed down. | L: Check the logical state of the key, which is the state that the OS and the active window believe the key to be in (not necessarily the same as the physical state). This option is ignored for joystick buttons. | T: Timeout (e.g. T3). The number of seconds to wait before timing out and setting ErrorLevel to 1. If the key or button achieves the specified state, the command will not wait for the timeout to expire. Instead, it will immediately set ErrorLevel to 0 and the script will continue executing. If this parameter is blank, the command will wait indefinitely for the specified key or mouse/joystick button to be physically released by the user. However, if the keyboard hook is not installed and KeyName is a keyboard key released artificially by means such as the Send command, the key will be seen as having been physically released. The same is true for mouse buttons when the mouse hook is not installed. </param>
        public bool KeyWait(string KeyName, string Options = "")
        {
            string AHKLine = "KeyWait, " + KeyName + "," + Options;  // ahk line to execute
            ErrorLog_Setup(true); // ErrorLevel Detection Enabled for this function in AHK 
            Execute(AHKLine);   // execute AHK code and return variable value

            if (!ahkGlobal.ErrorLevel) { return true; } // no error level - return true for success
            return false;  // error level detected - success = false
        }

        /// <summary>Waits for the user to type a string.</summary>
        /// <param name="Options">B: Backspace is ignored. | C: Case sensitive. | I: Ignore input generated by any AutoHotkey script, such as the SendEvent command. | L: Length limit (e.g. L5). The maximum allowed length of the input. | M: Modified keystrokes such as Control-A through Control-Z are recognized and transcribed if they correspond to real ASCII characters. | T: Timeout (e.g. T3). The number of seconds to wait before terminating the Input and setting ErrorLevel to the word Timeout. | V: Visible. Normally, the user's input is blocked (hidden from the system). Use this option to have the user's keystrokes sent to the active window. | *: Wildcard (find anywhere). Normally, what the user types must exactly match one of the MatchList phrases for a match to occur. Use this option to find a match more often by searching the entire length of the input text.</param>
        /// <param name="EndKeys">A list of zero or more keys, any one of which terminates the Input when pressed (the EndKey itself is not written to OutputVar). When an Input is terminated this way, ErrorLevel is set to the word EndKey followed by a colon and the name of the EndKey.</param>
        /// <param name="MatchList">A comma-separated list of key phrases, any of which will cause the Input to be terminated (in which case ErrorLevel will be set to the word Match). The entirety of what the user types must exactly match one of the phrases for a match to occur (unless the * option is present). In addition, any spaces or tabs around the delimiting commas are significant, meaning that they are part of the match string. For example, if MatchList is "ABC , XYZ ", the user must type a space after ABC or before XYZ to cause a match.</param>
        public string Input(string Options = "", string EndKeys = "", string MatchList = "")
        {
            string AHKLine = "Input, OutputVar, " + Options + "," + EndKeys + "," + MatchList;  // ahk line to execute
            ErrorLog_Setup(true); // ErrorLevel Detection Enabled for this function in AHK 
            string OutVar = Execute(AHKLine, "OutputVar");   // execute AHK code and return variable value 
            return OutVar;
        }

        /// <summary>Displays an input box to ask the user to enter a string.</summary>
        /// <param name="Title">The title of the input box. If blank or omitted, it defaults to the name of the script. </param>
        /// <param name="Prompt">The text of the input box, which is usually a message to the user indicating what kind of input is expected. If Prompt is long, it can be broken up into several shorter lines by means of a continuation section, which might improve readability and maintainability.</param>
        /// <param name="Hide">If this parameter is the word HIDE, the user's input will be masked, which is useful for passwords.</param>
        /// <param name="Width">If this parameter is blank or omitted, the starting width of the window will be 375. This parameter can be an expression.</param>
        /// <param name="Height">If this parameter is blank or omitted, the starting height of the window will be 189. This parameter can be an expression.</param>
        /// <param name="X">The X and Y coordinates of the window (use 0,0 to move it to the upper left corner of the desktop), which can be expressions. If either coordinate is blank or omitted, the dialog will be centered in that dimension. Either coordinate can be negative to position the window partially or entirely off the desktop.</param>
        /// <param name="Y">The X and Y coordinates of the window (use 0,0 to move it to the upper left corner of the desktop), which can be expressions. If either coordinate is blank or omitted, the dialog will be centered in that dimension. Either coordinate can be negative to position the window partially or entirely off the desktop.</param>
        /// <param name="Font">Not yet implemented (leave blank). In the future it might accept something like verdana:8</param>
        /// <param name="TimeOut">Timeout in seconds (can contain a decimal point or be an expression).  If this value exceeds 2147483 (24.8 days), it will be set to 2147483. After the timeout has elapsed, the InputBox window will be automatically closed and ErrorLevel will be set to 2. OutputVar will still be set to what the user entered.</param>
        /// <param name="Default">A string that will appear in the InputBox's edit field when the dialog first appears. The user can change it by backspacing or other means. </param>
        public string InputBox(string Title = "", string Prompt = "", bool Hide = false, string Width = "", string Height = "", string X = "", string Y = "", string Font = "", string TimeOut = "", string Default = "")
        {
            string hide = ""; if (Hide) { hide = "HIDE"; }
            string AHKLine = "InputBox, OutputVar, " + Title + "," + Prompt + "," + hide + "," + Width + "," + Height + "," + X + "," + Y + "," + Font + "," + TimeOut + "," + Default;  // ahk line to execute
            ErrorLog_Setup(true); // ErrorLevel Detection Enabled for this function in AHK 
            string OutVar = Execute(AHKLine, "OutputVar");   // execute AHK code and return variable value 
            return OutVar;

            // v1 ToDo
            /*
                    public DialogResult _InputBox(string title, string promptText, ref string value, string OKButton = "OK", string CancelButton = "Cancel")  // user input box that promps user for input
                    {
                        //EX: 
                        //string value = "New List Name";
                        //if (ahk.InputBox("Enter New List Name: ", "", ref value) == DialogResult.OK)
                        //{
                        //    string UserEntry = value;
                        //    bool Inserted = sqlite.InsertListItem(ahkGlobal.UserDb, "UserLists", UserEntry, "", "", "0");
                        //    Load_UserList_InGrid(ahkGlobal.UserDb, UserEntry);
                        //    PopulateListDDL();
                        //}


                        Form DialogForm = new Form();
                        Label label = new Label();
                        TextBox textBox = new TextBox();
                        Button buttonOk = new Button();
                        Button buttonCancel = new Button();


                        DialogForm.Text = title;
                        label.Text = promptText;
                        textBox.Text = value;

                        buttonOk.Text = OKButton;
                        buttonCancel.Text = CancelButton;
                        buttonOk.DialogResult = DialogResult.OK;
                        buttonCancel.DialogResult = DialogResult.Cancel;

                        label.SetBounds(9, 20, 372, 13);
                        textBox.SetBounds(12, 36, 372, 20);
                        buttonOk.SetBounds(228, 72, 75, 23);
                        buttonCancel.SetBounds(309, 72, 75, 23);

                        label.AutoSize = true;
                        textBox.Anchor = textBox.Anchor | AnchorStyles.Right;
                        buttonOk.Anchor = AnchorStyles.Bottom | AnchorStyles.Right;
                        buttonCancel.Anchor = AnchorStyles.Bottom | AnchorStyles.Right;

                        DialogForm.ClientSize = new Size(396, 107);
                        DialogForm.Controls.AddRange(new Control[] { label, textBox, buttonOk, buttonCancel });
                        DialogForm.ClientSize = new Size(Math.Max(300, label.Right + 10), DialogForm.ClientSize.Height);
                        DialogForm.FormBorderStyle = FormBorderStyle.FixedDialog;
                        DialogForm.StartPosition = FormStartPosition.CenterScreen;
                        DialogForm.TopMost = true;
                        DialogForm.MinimizeBox = false;
                        DialogForm.MaximizeBox = false;
                        DialogForm.AcceptButton = buttonOk;
                        DialogForm.CancelButton = buttonCancel;


                        DialogResult dialogResult = DialogForm.ShowDialog();
                        value = textBox.Text;
                        return dialogResult;
                    }
            */

        }

        /// <summary>
        /// v2 Input Box
        /// </summary>
        /// <param name="title"></param>
        /// <param name="promptText"></param>
        /// <param name="value"></param>
        /// <param name="OKButton"></param>
        /// <param name="CancelButton"></param>
        /// <returns></returns>
        public DialogResult _InputBox(string title, string promptText, ref string value, string OKButton = "OK", string CancelButton = "Cancel")  // user input box that promps user for input
        {
            //EX: 
            //string value = "New List Name";
            //if (ahk.InputBox("Enter New List Name: ", "", ref value) == DialogResult.OK)
            //{
            //    string UserEntry = value;
            //    bool Inserted = sqlite.InsertListItem(ahkGlobal.UserDb, "UserLists", UserEntry, "", "", "0");
            //    Load_UserList_InGrid(ahkGlobal.UserDb, UserEntry);
            //    PopulateListDDL();
            //}


            Form DialogForm = new Form();
            Label label = new Label();
            TextBox textBox = new TextBox();
            Button buttonOk = new Button();
            Button buttonCancel = new Button();


            DialogForm.Text = title;
            label.Text = promptText;
            textBox.Text = value;

            buttonOk.Text = OKButton;
            buttonCancel.Text = CancelButton;
            buttonOk.DialogResult = DialogResult.OK;
            buttonCancel.DialogResult = DialogResult.Cancel;

            label.SetBounds(9, 20, 372, 13);
            textBox.SetBounds(12, 36, 372, 20);
            buttonOk.SetBounds(228, 72, 75, 23);
            buttonCancel.SetBounds(309, 72, 75, 23);

            label.AutoSize = true;
            textBox.Anchor = textBox.Anchor | AnchorStyles.Right;
            buttonOk.Anchor = AnchorStyles.Bottom | AnchorStyles.Right;
            buttonCancel.Anchor = AnchorStyles.Bottom | AnchorStyles.Right;

            DialogForm.ClientSize = new Size(396, 107);
            DialogForm.Controls.AddRange(new Control[] { label, textBox, buttonOk, buttonCancel });
            DialogForm.ClientSize = new Size(Math.Max(300, label.Right + 10), DialogForm.ClientSize.Height);
            DialogForm.FormBorderStyle = FormBorderStyle.FixedDialog;
            DialogForm.StartPosition = FormStartPosition.CenterScreen;
            DialogForm.TopMost = true;
            DialogForm.MinimizeBox = false;
            DialogForm.MaximizeBox = false;
            DialogForm.AcceptButton = buttonOk;
            DialogForm.CancelButton = buttonCancel;


            DialogResult dialogResult = DialogForm.ShowDialog();
            value = textBox.Text;
            return dialogResult;
        }

        /// <summary>Displays the specified text in a small window containing one or more buttons (such as Yes and No) - returns True if Clicked, False if timeout reached</summary>
        /// <param name="Text_Or_Options">If all parameters are omitted, MsgBox will display "Press OK to continue." Otherwise, this parameter is the text displayed inside the message box. OPTIONS Indicates the type of message box and the possible button combinations. More than one option can be added by combining values: || Ok - 0 | Ok/Cancel - 1 | Abort/Retry/Ignore - 2 | Yes/No/Cancel - 3 | Yes/No - 4 | Retry/Cancel - 5 | Cancel/Try Again/Continue - 6 | Add Help Button - 16384 | Icon Stop/Error - 16 | Icon Question - 32 | Icon Exclamation - 48 | Icon Asterisk - 64 | 2nd Button Default - 256 | 3rd Button Default - 512 | System Modal (always on top) - 4096 | Task Modal - 8192 | Show on Default Desktop - 131072 | AlwaysOnTop (no title bar) - 262144 | Text Right Justified - 524288 | Right to Left Reading - 1048576</param>
        /// <param name="Title">The title of the message box window. If omitted or blank, it defaults to the name of the script (without path). </param>
        /// <param name="Text">This parameter is the text displayed inside the message box to instruct the user what to do, or to present information.</param>
        /// <param name="TimeOut">(optional) Timeout in seconds (can contain a decimal point but cannot be an expression).  If this value exceeds 2147483 (24.8 days), it will be set to 2147483.  After the timeout has elapsed the message box will be automatically closed and the IfMsgBox command will see the value TIMEOUT.</param>
        /// <returns>R</returns>
        private string msgBox(string Text_Or_Options, string Title = "", string Text = "", string TimeOut = "")
        {
            if (Title != "" || Text != "" || TimeOut != "")  // user provided options in first param - checks for TimeOut event to return false if reached
            {
                string AHKLine = "MsgBox, " + Text_Or_Options + "," + Title + "," + Text + "," + TimeOut; // ahk line to execute
                //AHKLine = AHKLine + Environment.NewLine + "TimedOut = false" + Environment.NewLine + "IfMsgBox, TimeOut" + Environment.NewLine + "TimedOut = true";

                ErrorLog_Setup(false); // ErrorLevel Detection Not Enabled for this function in AHK 
                Execute(AHKLine);


                // check to see if messagebox returned a value based on user clicking a messagebox option
                AHKLine = "IfMsgBox, No\r\nOutputVar = No\r\nIfMsgBox, Yes\r\nOutputVar = Yes\r\nIfMsgBox, OK\r\nOutputVar = Ok\r\nIfMsgBox, Cancel\r\nOutputVar = Cancel\r\nIfMsgBox, Abort\r\nOutputVar = Abort\r\nIfMsgBox, Ignore\r\nOutputVar = Ignore\r\nIfMsgBox, Retry\r\nOutputVar = Retry\r\nIfMsgBox, Continue\r\nOutputVar = Continue\r\nIfMsgBox, TryAgain\r\nOutputVar = TryAgain\r\nIfMsgBox, Timeout\r\nOutputVar = Timeout";
                string OutVar = Execute(AHKLine, "OutputVar");   // execute AHK code and return variable value 
                return OutVar;


                //Function("MsgBox", Text_Or_Options, Title, Text, TimeOut); 
                //Function("MsgBox", Text, "Title"); 


                //string OutVar = Execute(AHKLine, "TimedOut");   // execute AHK code and return variable value 
                //if (OutVar == "true") { return "TimedOut = False"; }  // timed out - return false
                //if (OutVar == "false") { return "TimedOut = True"; }  // did not time out - return true
            }


            if (Title == "" && Text == "" && TimeOut == "")  // no options provided - just display message text
            {
                MessageBox.Show(Text_Or_Options, "", MessageBoxButtons.OK, MessageBoxIcon.None, MessageBoxDefaultButton.Button1, (MessageBoxOptions)0x40000);
                ErrorLog_Setup(false); // ErrorLevel Detection Not Enabled for this function in AHK 
                //string AHKLine = "MsgBox, " + Text_Or_Options; // ahk line to execute
                //Execute(AHKLine);   // execute AHK code and return variable value 
                return "";
            }

            return "";
        }

        /// <summary>
        /// Behaves Same as MsgBox Command, Options are PreDefined Here for Faster Config
        /// </summary>
        /// <param name="Text">Text to Display in MessageBox</param>
        /// <param name="Title">MsgBox Title</param>
        /// <param name="Button">Button Options (Default = OK)</param>
        /// <param name="Icon">Button Icon (Default = None)</param>
        /// <param name="TimeOut">Seconds to Wait on User Input Before Timeout (Default = -1 Which Disables TimeOut)</param>
        /// <returns>Returns Button Text User Clicked or 'TimeOut' if User Didn't Click Before TimeOut Reached</returns>
        public string MsgBox(object Text, string Title = "MsgBox Display", MsgBoxButtons Button = MsgBoxButtons.OK, MsgBoxIcon Icon = MsgBoxIcon.None, int TimeOut = -1)
        {
            try
            {
                MessageBox.Show(Text.ToString());
            }
            catch
            {
                MessageBox.Show("MessageBox Error.. My Bad");
            }
            return "";

            // add option values together
            int add = (int)Button;
            int add2 = (int)Icon;
            add += add2;

            string timeOut = TimeOut.ToString();
            if (TimeOut == -1) { timeOut = ""; }

            string userText = Text.ToString().Replace(",", "`,");
            //Title = Title.Replace(",", "`,");
            //Text_Or_Options = Text_Or_Options.Replace("\n", "`n");

            return msgBox(add.ToString(), Title, userText, timeOut);
        }

        /// <summary>
        /// MsgBox Button Options
        /// </summary>		
        public enum MsgBoxButtons
        {
            OK = 0,
            OK_Cancel = 1,
            Abort_Retry_Ignore = 2,
            Yes_No_Cancel = 3,
            Yes_No = 4,
            Retry_Cancel = 5,
            Cancel_TryAgain_Continue = 6,
            Add_HelpButton = 16384
        }

        /// <summary>
        /// MsgBox Icon Options
        /// </summary>		
        public enum MsgBoxIcon
        {
            None = 0,
            StopError = 16,
            Question = 32,
            Exclamation = 48,
            Asterisk = 64
        }


        /// No Longer Needed - MsgBox Command Returns Clicked Text Value
        /// <summary>Checks which button was pushed by the user during the most recent MsgBox command. **Returns Button Name pressed by last MsgBox command**</summary>
        /// <returns>Returns the Button Name Clicked By User on Last MsgBox Prompt</returns>
        public string MsgBox_Clicked()
        {
            string AHKLine = "IfMsgBox, No\r\nOutputVar = No\r\nIfMsgBox, Yes\r\nOutputVar = Yes\r\nIfMsgBox, OK\r\nOutputVar = Ok\r\nIfMsgBox, Cancel\r\nOutputVar = Cancel\r\nIfMsgBox, Abort\r\nOutputVar = Abort\r\nIfMsgBox, Ignore\r\nOutputVar = Ignore\r\nIfMsgBox, Retry\r\nOutputVar = Retry\r\nIfMsgBox, Continue\r\nOutputVar = Continue\r\nIfMsgBox, TryAgain\r\nOutputVar = TryAgain\r\nIfMsgBox, Timeout\r\nOutputVar = Timeout"; // ahk line to execute
            ErrorLog_Setup(false); // ErrorLevel Detection Enabled for this function in AHK 
            string OutVar = Execute(AHKLine, "OutputVar");   // execute AHK code and return variable value 
            return OutVar;
        }

        /// <summary>Checks to see if ButtonName (ex: Ok, No, Cancel) was pushed by the user during the most recent MsgBox command. Returns True if Specific ButtonName was pushed</summary>
        /// <param name="ButtonName">Name of the MsgBox Button User May Have Clicked (Ex: Ok/Yes/No/Cancel) - Returns True if Button Was Clicked</param>
        public bool IfMsgBox(string ButtonName = "Ok")
        {
            string AHKLine = "IfMsgBox, No\r\nOutputVar = No\r\nIfMsgBox, Yes\r\nOutputVar = Yes\r\nIfMsgBox, OK\r\nOutputVar = Ok\r\nIfMsgBox, Cancel\r\nOutputVar = Cancel\r\nIfMsgBox, Abort\r\nOutputVar = Abort\r\nIfMsgBox, Ignore\r\nOutputVar = Ignore\r\nIfMsgBox, Retry\r\nOutputVar = Retry\r\nIfMsgBox, Continue\r\nOutputVar = Continue\r\nIfMsgBox, TryAgain\r\nOutputVar = TryAgain\r\nIfMsgBox, Timeout\r\nOutputVar = Timeout"; // ahk line to execute
            ErrorLog_Setup(false); // ErrorLevel Detection Enabled for this function in AHK 
            string OutVar = Execute(AHKLine, "OutputVar");   // execute AHK code and return variable value 

            // check to see if button user was looking for was clicked
            if (OutVar.ToUpper() == ButtonName.ToUpper()) { return true; }

            // user's button name wasn't clicked - return false
            return false;
        }

        /// <summary>yes/no user prompt</summary>
        /// <param name="Question"> </param>
        /// <param name=" Title"> </param>
        public DialogResult YesNoBox(string Question, string Title)
        {
            //// EX: 
            //var ResultValue = ahk.YesNoBox("Delete " + FileName + "?", "Delete File?");
            //if (ResultValue.ToString() == "Yes") { ahk.FileDelete(FilePath); }


            DialogResult result = MessageBox.Show(Question, Title, MessageBoxButtons.YesNo);
            return result;
        }

        /// <summary>
        /// Yes/No Dialog that Returns TRUE if Yes Clicked
        /// </summary>
        /// <param name="Question"></param>
        /// <param name="Title"></param>
        /// <returns></returns>
        public bool YesNo(string Question, string Title = "PROMPT")
        {
            DialogResult result = YesNoBox(Question, Title);
            if (result.ToString().ToUpper() == "NO") { return false; }
            return true;
        }



        /// <summary>yes/no/cancel prompt for user input</summary>
        /// <param name="Question"> </param>
        /// <param name=" Title"> </param>
        public DialogResult YesNoCancelBox(string Question, string Title)  // yes/no/cancel prompt for user input
        {
            //// EX: 
            //var ResultValue = ahk.YesNoCancelBox("Delete ?", "Delete File?");
            //if (ResultValue.ToString() == "Cancel") { ahk.MsgBox("Canceled"); }


            DialogResult result = MessageBox.Show(Question, Title, MessageBoxButtons.YesNoCancel);
            return result;
        }

        /// <summary>Select Color Dialog</summary>
        public Color Color_Dialog()
        {
            //// select color dialog
            //Color SelectedColor = ahk.Color_Dialog();

            //// update control if value returned
            //if (SelectedColor != Color.Empty)
            //{
            //    dataGridView1.BackgroundColor = SelectedColor;
            //}


            ColorDialog colorDialog1 = new ColorDialog();

            if (colorDialog1.ShowDialog() == System.Windows.Forms.DialogResult.OK) { return colorDialog1.Color; }

            return Color.Empty; // no value selected
        }

        /// <summary>Popup dialog to select folder path</summary>
        public string Select_Folder_Dialog()
        {
            FolderBrowserDialog folderBrowserDialog1 = new FolderBrowserDialog();

            DialogResult result = folderBrowserDialog1.ShowDialog();
            if (result == DialogResult.OK)
            {
                //string[] files = Directory.GetFiles(folderBrowserDialog1.SelectedPath);
                //MessageBox.Show("Files found: " + files.Length.ToString(), "Message");
                return folderBrowserDialog1.SelectedPath;
            }

            return "";
        }

        /// <summary>Open File Dialog</summary>
        /// <param name="InitialDir"> </param>
        /// <param name="DefaultExt"> </param>
        /// <param name="Filter"> </param>
        /// <param name="Title"> </param>
        public string Open_File_Dialog(string InitialDir = @"C:\", string DefaultExt = "txt", string Filter = "Text Files (*.txt)|*.txt|All Files (*.*)|*.*", string Title = "Browse Files")
        {
            string SelectedFile = "";

            OpenFileDialog openFileDialog1 = new OpenFileDialog();

            openFileDialog1.InitialDirectory = InitialDir;
            openFileDialog1.Title = Title;

            openFileDialog1.CheckFileExists = true;
            openFileDialog1.CheckPathExists = true;

            openFileDialog1.DefaultExt = DefaultExt;
            openFileDialog1.Filter = Filter;
            openFileDialog1.FilterIndex = 2;
            openFileDialog1.RestoreDirectory = true;

            openFileDialog1.ReadOnlyChecked = true;
            openFileDialog1.ShowReadOnly = true;

            if (openFileDialog1.ShowDialog() == DialogResult.OK)
            {
                SelectedFile = openFileDialog1.FileName;
                //MsgBox(SelectedFile); 
                return SelectedFile;
            }

            return SelectedFile;
        }

        private List<string> Open_File_Dialog_Multiple()  // Select Multiple Files From Open File Dialog
        {
            OpenFileDialog openFileDialog1 = new OpenFileDialog();
            List<string> SelectedFiles = new List<string>();

            openFileDialog1.Filter =
                "Images (*.BMP;*.JPG;*.GIF,*.PNG,*.TIFF)|*.BMP;*.JPG;*.GIF;*.PNG;*.TIFF|" +
                "All files (*.*)|*.*";

            openFileDialog1.Multiselect = true;
            openFileDialog1.Title = "Select Photos";

            DialogResult dr = openFileDialog1.ShowDialog();
            if (dr == System.Windows.Forms.DialogResult.OK)
            {
                foreach (String file in openFileDialog1.FileNames)
                {
                    SelectedFiles.Add(file);
                }
            }
            return SelectedFiles;
        }

        /// <summary>Save File Prompt</summary>
        /// <param name="InitialDirectory"> </param>
        /// <param name="DefaultExt"> </param>
        /// <param name="Filter"> </param>
        /// <param name="Title"> </param>
        /// <param name="FileMustExist"> </param>
        public string Save_File_Dialog(string InitialDirectory = @"C:\", string DefaultExt = "txt", string Filter = "Text Files (*.txt)|*.txt|All Files (*.*)|*.*", string Title = "Save File", bool FileMustExist = false)
        {
            string SaveFileName = "";
            SaveFileDialog saveFileDialog1 = new SaveFileDialog();
            saveFileDialog1.InitialDirectory = InitialDirectory;
            if (InitialDirectory != "") { saveFileDialog1.InitialDirectory = InitialDirectory; }
            saveFileDialog1.Title = "Save text Files";
            saveFileDialog1.CheckFileExists = FileMustExist;
            saveFileDialog1.CheckPathExists = FileMustExist;
            saveFileDialog1.DefaultExt = DefaultExt;
            saveFileDialog1.Filter = Filter;
            saveFileDialog1.FilterIndex = 2;
            saveFileDialog1.RestoreDirectory = true;

            if (saveFileDialog1.ShowDialog() == DialogResult.OK)
            {
                SaveFileName = saveFileDialog1.FileName;
                MsgBox("Save File Path: " + SaveFileName);
                return SaveFileName;
            }

            return SaveFileName;
        }

        /// <summary>Save File As Prompt</summary>
        /// <param name="InitialDirectory"> </param>
        /// <param name="DefaultExt"> </param>
        /// <param name="Filter"> </param>
        /// <param name="Title"> </param>
        public string Save_File_As_Dialog(string InitialDirectory = @"C:\", string DefaultExt = "txt", string Filter = "Text Files (*.txt)|*.txt|All Files (*.*)|*.*", string Title = "Save File As")
        {
            string SaveFileName = "";
            SaveFileDialog saveFileDialog1 = new SaveFileDialog();
            saveFileDialog1.InitialDirectory = InitialDirectory;
            if (InitialDirectory != "") { saveFileDialog1.InitialDirectory = InitialDirectory; }
            saveFileDialog1.Title = Title;
            saveFileDialog1.CheckFileExists = false;
            saveFileDialog1.CheckPathExists = false;
            saveFileDialog1.DefaultExt = DefaultExt;
            saveFileDialog1.Filter = Filter;
            saveFileDialog1.FilterIndex = 2;
            saveFileDialog1.RestoreDirectory = true;

            if (saveFileDialog1.ShowDialog() == DialogResult.OK)
            {
                SaveFileName = saveFileDialog1.FileName;
                //MsgBox("Save File Path: " + SaveFileName);
                return SaveFileName;
            }

            return SaveFileName;
        }








        /// <summary>Creates or updates a window containing a progress bar or an image.</summary>
        /// <param name="Param1">If the progress window already exists: If Param1 is the word OFF, the window is destroyed. If Param1 is the word SHOW, the window is shown if it is currently hidden. Otherwise, if Param1 is an pure number, its bar's position is changed to that value. If Param1 is blank, its bar position will be unchanged but its text will be updated to reflect any new strings provided in SubText, MainText, and WinTitle. In both of these modes, if the window doesn't yet exist, it will be created with the defaults for all options.</param>
        /// <param name="SubText">The text to display below the image or bar indicator. Although word-wrapping will occur, to begin a new line explicitly, use linefeed (`n). To set an existing window's text to be blank, specify %A_Space%. For the purpose of auto-calculating the window's height, blank lines can be reserved in a way similar to MainText below.</param>
        /// <param name="MainText">The text to display above the image or bar indicator (its font is semi-bold). Although word-wrapping will occur, to begin a new line explicitly, use linefeed (`n). If blank or omitted, no space will be reserved in the window for MainText. To reserve space for single line to be added later, or to set an existing window's text to be blank, specify %A_Space%. To reserve extra lines beyond the first, append one or more linefeeds (`n). Once the height of MainText's control area has been set, it cannot be changed without recreating the window. </param>
        /// <param name="WinTitle">The title of the window. If omitted and the window is being newly created, the title defaults to the name of the script (without path). If the B (borderless) option has been specified, there will be no visible title bar but the window can still be referred to by this title in commands such as WinMove.</param>
        /// <param name="FontName">The name of the font to use for both MainText and SubText. The font table lists the fonts included with the various versions of Windows. If unspecified or if the font cannot be found, the system's default GUI font will be used. See AHK documentation for how to change the size, weight, and color of the font.</param>
        public void Progress(string Param1 = "", string SubText = "", string MainText = "", string WinTitle = "", string FontName = "")
        {
            string AHKLine = "Progress, " + Param1 + "," + SubText + "," + MainText + "," + WinTitle + "," + FontName; // ahk line to execute
            ErrorLog_Setup(false); // ErrorLevel Detection Enabled for this function in AHK 
            Execute(AHKLine);   // execute AHK code and return variable value 
        }

        /// <summary>Creates or updates a window containing a progress bar or an image.</summary>
        /// <param name="ImagePath">If this is the word OFF, the window is destroyed. If this is the word SHOW, the window is shown if it is currently hidden. Otherwise, this is the file name of the BMP, GIF, or JPG image to display (to display other file formats such as PNG, TIF, and ICO, consider using the Gui command to create a window containing a picture control). ImageFile is assumed to be in %A_WorkingDir% if an absolute path isn't specified. If ImageFile and Options are blank and the window already exists, its image will be unchanged but its text will be updated to reflect any new strings provided in SubText, MainText, and WinTitle. For newly created windows, if ImageFile is blank or there is a problem loading the image, the window will be displayed without the picture.</param>
        /// <param name="Options">See AHK Documentation For More Info - A string of zero or more options from the list of options. || A - Window will NOT be always-on-top | B - Borderless | M - Window will be moveable by user (except if borderless) | T - the window will be given a button in the task bar and it will be unowned. | Hn - specify n for the height of the window's client area | Wn - specify n for the width of the client window's area | Xn - specify n for the window's x coordinate | Yn - specify n for the window's y cooridnate | Hide - window will be initially hidden</param>
        /// <param name="SubText">The text to display below the image or bar indicator. Although word-wrapping will occur, to begin a new line explicitly, use linefeed (`n). To set an existing window's text to be blank, specify %A_Space%. For the purpose of auto-calculating the window's height, blank lines can be reserved in a way similar to MainText below.</param>
        /// <param name="MainText">The text to display above the image or bar indicator (its font is semi-bold). Although word-wrapping will occur, to begin a new line explicitly, use linefeed (`n). If blank or omitted, no space will be reserved in the window for MainText. To reserve space for single line to be added later, or to set an existing window's text to be blank, specify %A_Space%. To reserve extra lines beyond the first, append one or more linefeeds (`n). Once the height of MainText's control area has been set, it cannot be changed without recreating the window. </param>
        /// <param name="WinTitle">The title of the window. If omitted and the window is being newly created, the title defaults to the name of the script (without path). If the B (borderless) option has been specified, there will be no visible title bar but the window can still be referred to by this title in commands such as WinMove.</param>
        /// <param name="FontName">The name of the font to use for both MainText and SubText. The font table lists the fonts included with the various versions of Windows. If unspecified or if the font cannot be found, the system's default GUI font will be used. See AHK documentation for how to change the size, weight, and color of the font.</param>
        public void SplashImage(string ImagePath = "", string Options = "", string SubText = "", string MainText = "", string WinTitle = "", string FontName = "")
        {
            string AHKLine = "SplashImage, " + ImagePath + "," + Options + "," + SubText + "," + MainText + "," + WinTitle + "," + FontName; // ahk line to execute
            ErrorLog_Setup(false); // ErrorLevel Detection Enabled for this function in AHK 
            Execute(AHKLine);   // execute AHK code and return variable value 
        }


        /// <summary>
        /// Displays a SplashImage previously hidden (created with "Hide" Option enabled)
        /// </summary>
        /// <param name="DisplayTimeMS">Option to Hide Splash Image after X milliseconds (if greater than 0)</param>
        public void SplashImageShow(int DisplayTimeMS = 0)
        {
            string AHKLine = "SplashImage, Show"; // ahk line to execute
            ErrorLog_Setup(false); // ErrorLevel Detection Enabled for this function in AHK 
            Execute(AHKLine);   // execute AHK code and return variable value 

            if (DisplayTimeMS > 0)
            {
                Sleep(DisplayTimeMS);
                SplashImageOff();
            }
        }

        /// <summary>Command to remove an existing Splash Image.</summary>
        public void SplashImageOff()
        {
            string AHKLine = "SplashImage, Off"; // ahk line to execute
            ErrorLog_Setup(false); // ErrorLevel Detection Enabled for this function in AHK 
            Execute(AHKLine);   // execute AHK code and return variable value 
        }

        /// <summary>Command to remove an existing splash window.</summary>
        public void SplashTextOff()
        {
            string AHKLine = "SplashTextOff"; // ahk line to execute
            ErrorLog_Setup(false); // ErrorLevel Detection Enabled for this function in AHK 
            Execute(AHKLine);   // execute AHK code and return variable value 
        }

        /// <summary>Creates a customizable text popup window.</summary>
        /// <param name="Width">The width in pixels of the Window. Default 200. This parameter can be an expression.</param>
        /// <param name="Height">The height in pixels of the window (not including its title bar). Default 0 (i.e. just the title bar will be shown). This parameter can be an expression.</param>
        /// <param name="Title">The title of the window. Default empty (blank). </param>
        /// <param name="Text">The text of the window. Default empty (blank). If Text is long, it can be broken up into several shorter lines by means of a continuation section, which might improve readability and maintainability.</param>
        /// <param name="DisplayTimeMS">Option to Hide Splash Text after X milliseconds (if greater than 0)</param>
        public void SplashTextOn(string Width = "", string Height = "", string Title = "", string Text = "", int DisplayTimeMS = 0)
        {
            string AHKLine = "SplashTextOn, " + Width + "," + Height + "," + Title + "," + Text; // ahk line to execute
            ErrorLog_Setup(false); // ErrorLevel Detection Enabled for this function in AHK 
            Execute(AHKLine);   // execute AHK code and return variable value 

            if (DisplayTimeMS > 0)
            {
                Sleep(DisplayTimeMS);
                SplashTextOff();
            }
        }

        /// <summary>Creates an always-on-top window anywhere on the screen.</summary>
        /// <param name="Text">If blank or omitted, the existing tooltip (if any) will be hidden. Otherwise, this parameter is the text to display in the tooltip. To create a multi-line tooltip, use the linefeed character (`n) in between each line, e.g. Line1`nLine2. If Text is long, it can be broken up into several shorter lines by means of a continuation section, which might improve readability and maintainability. </param>
        /// <param name="DisplayTime">Time (Milliseconds) to Display ToolTip on Screen</param>
        /// <param name="X">The X and Y position of the tooltip relative to the active window (use "CoordMode, ToolTip" to change to screen coordinates). If the coordinates are omitted, the tooltip will be shown near the mouse cursor. X and Y can be expressions.</param>
        /// <param name="Y">The X and Y position of the tooltip relative to the active window (use "CoordMode, ToolTip" to change to screen coordinates). If the coordinates are omitted, the tooltip will be shown near the mouse cursor. X and Y can be expressions.</param>
        /// <param name="WhichToolTip">Omit this parameter if you don't need multiple tooltips to appear simultaneously. Otherwise, this is a number between 1 and 20 to indicate which tooltip window to operate upon. If unspecified, that number is 1 (the first). This parameter can be an expression.</param>
        public void ToolTip(string Text = "", string DisplayTime = "", string X = "", string Y = "", string WhichToolTip = "")
        {
            string AHKLine = "ToolTip, " + Text + "," + X + "," + Y + "," + WhichToolTip; // ahk line to execute
            ErrorLog_Setup(false); // ErrorLevel Detection Enabled for this function in AHK 
            Execute(AHKLine);   // execute AHK code and return variable value 

            //=== Option To Close ToolTip After X Milliseconds ====

            if (DisplayTime != "")
            {
                int Time = Int32.Parse(DisplayTime);  // string to int

                // Create a timer with a two second interval.
                aTimer = new System.Timers.Timer();
                aTimer.Interval = Time;

                // Hook up the Elapsed event for the timer. 
                aTimer.Elapsed += KillToolTip;

                // Have the timer fire repeated events (true is the default)
                aTimer.AutoReset = false;

                // Start the timer
                aTimer.Enabled = true;
            }
        }

        /// <summary>Timer used to destroy tooltip after X milliseconds</summary>
        private static System.Timers.Timer aTimer;  // 

        /// <summary>Event Used to Close ToolTip after X Milliseconds</summary>
        private void KillToolTip(Object source, System.Timers.ElapsedEventArgs e)
        {
            ToolTip();
            aTimer.Enabled = false;
            aTimer.Close();
        }

        /// <summary>Creates a balloon message window near the tray icon. Requires Windows 2000/XP or later.</summary>
        /// <param name="Title">If all parameters are omitted, any TrayTip window currently displayed will be removed. Otherwise, this parameter is the title of the window, which can be up to 73 characters long (characters beyond this length are not shown). If Title is blank, the title line will be entirely omitted from the balloon window, making it vertically shorter.</param>
        /// <param name="Text">If this parameter is omitted or blank, any TrayTip window currently displayed will be removed. Otherwise, specify the message to display, which appears beneath Title. Only the first 265 characters of Text will be displayed. Carriage return (`r) or linefeed (`n) may be used to create multiple lines of text. For example: Line1`nLine2 </param>
        /// <param name="Seconds">The approximate number of seconds to display the window, after which it will be automatically removed by the OS. Specifying a number less than 10 or greater than 30 will usually cause the minimum (10) or maximum (30) display time to be used instead. If blank or omitted, the minimum time will usually be used. This parameter can be an expression.</param>
        /// <param name="Options">1: Info icon | 2: Warning icon | 3: Error icon | If omitted, it defaults to 0, which is no icon. This parameter can be an expression. </param>
        public void TrayTip(string Title = "", string Text = "", string Seconds = "", string Options = "")
        {
            string AHKLine = "TrayTip, " + Title + "," + Text + "," + Seconds + "," + Options; // ahk line to execute
            ErrorLog_Setup(false); // ErrorLevel Detection Enabled for this function in AHK 
            Execute(AHKLine);   // execute AHK code and return variable value 
        }

        /// <summary>
        /// Updated Version of TrayTip - Displays Popup on Bottom Right of Primary Display
        /// </summary>
        /// <param name="text">Text to Display</param>
        /// <param name="title">Notification Title</param>
        public void Notify(string text, string title = "")
        {
            // https://msdn.microsoft.com/en-us/library/system.windows.forms.notifyicon.aspx


            var notification = new System.Windows.Forms.NotifyIcon()
            {
                Visible = true,
                Icon = System.Drawing.SystemIcons.Information,
                Text = text,
                BalloonTipIcon = System.Windows.Forms.ToolTipIcon.Info,
                BalloonTipTitle = title,
                BalloonTipText = text
            };

            // Display for 5 seconds.
            notification.ShowBalloonTip(5000);

            // This will let the balloon close after it's 5 second timeout
            // for demonstration purposes. Comment this out to see what happens
            // when dispose is called while a balloon is still visible.
            //Thread.Sleep(10000);

            // The notification should be disposed when you don't need it anymore,
            // but doing so will immediately close the balloon if it's visible.
            notification.Dispose();
        }



        /// <summary>Sends simulated keystrokes and mouse clicks to the active window. Modifiers include: ! (ALT) | + (SHIFT) | ^ (CONTROL) | # (WIN) | {!} - {^} (Literal Key Use) | {F1} - {F24} | {Enter} | {Space} | {Tab} | {Up} | {PgDn} | {Backspace} | {Control Down} | {NumpadLeft} | {PrintScreen}</summary>
        /// <param name="Keys">The sequence of keys to send. As with other commands, the comma in front of the first parameter is optional.</param>
        public void Send(string Keys)
        {
            ErrorLog_Setup(false);
            Execute("Send, " + Keys);
        }

        /// <summary>Sends simulated keystrokes and mouse clicks to the active window. The SendRaw command sends the keystrokes exactly as they appear rather than translating {Enter} to an ENTER keystroke, ^c to Control-C, etc.</summary>
        /// <param name="Keys">The sequence of keys to send. As with other commands, the comma in front of the first parameter is optional.</param>
        public void SendRaw(string Keys)
        {
            ErrorLog_Setup(false);
            Execute("SendRaw, " + Keys);
        }

        /// <summary>Sends simulated keystrokes and mouse clicks to the active window. SendInput is generally the preferred method to send keystrokes and mouse clicks because of its superior speed and reliability. Under most conditions, SendInput is nearly instantaneous, even when sending long strings.</summary>
        /// <param name="Keys">The sequence of keys to send. As with other commands, the comma in front of the first parameter is optional.</param>
        public void SendInput(string Keys)
        {
            ErrorLog_Setup(false);
            Execute("SendInput, " + Keys);

            // v1 ToDo
            /*
                    //==== Send Key Strokes ======================
                    public void _SendInput(string Keys, int Mode = 1)  // sends input from application to another application (2 different modes to sending - 1 or 2)
                    {
                        if (Mode == 1)  // AHK Send Input
                        {
                            if (ahkGlobal.ahkdll == null) { ahkGlobal.ahkdll = new AutoHotkey.Interop.AutoHotkeyEngine(); }
                            var ahkdll = ahkGlobal.ahkdll;

                            string Command = "SendInput " + Keys;
                            ahkdll.ExecRaw(Command);
                        }

                        if (Mode == 2)  // C# Send Input
                        {
                            SendKeys.SendWait(Keys);
                        }
                    }
            */

        }

        /// <summary>Sends simulated keystrokes and mouse clicks to the active window. SendPlay's biggest advantage is its ability to "play back" keystrokes and mouse clicks in a broader variety of games than the other modes.</summary>
        /// <param name="Keys">The sequence of keys to send. As with other commands, the comma in front of the first parameter is optional.</param>
        public void SendPlay(string Keys)
        {
            ErrorLog_Setup(false);
            Execute("SendPlay, " + Keys);
        }

        /// <summary>Sends simulated keystrokes and mouse clicks to the active window.</summary>
        /// <param name="Keys">The sequence of keys to send. As with other commands, the comma in front of the first parameter is optional.</param>
        /// <param name="KeyDelay">Amount of time (ms) to pause in between key sends. Default is -1 (fastest possible)</param>
        public void SendEvent(string Keys, string keyDelay = "-1")
        {
            KeyDelay(keyDelay);  // set the key delay between keystrokes, -1 = no delay

            ErrorLog_Setup(false);
            Execute("SendEvent, " + Keys);
        }

        /// <summary>Makes Send synonymous with SendInput or SendPlay rather than the default (SendEvent). Also makes Click and MouseMove/Click/Drag use the specified method.</summary>
        /// <param name="Mode">Event | Input | InputThenPlay | Play</param>
        public void SendMode(string Mode)
        {
            ErrorLog_Setup(false);
            Execute("SendMode " + Mode);
        }

        /// <summary>Sets the delay that will occur after each keystroke sent by Send and ControlSend.</summary>
        /// <param name="Delay">Time in milliseconds, which can be an expression. Use -1 for no delay at all and 0 for the smallest possible delay (however, if the Play parameter is present, both 0 and -1 produce no delay). Leave this parameter blank to retain the current Delay.</param>
        /// <param name="PressDuration">Certain games and other specialized applications may require a delay inside each keystroke; that is, after the press of the key but before its release. Use -1 for no delay at all (default) and 0 for the smallest possible delay (however, if the Play parameter is present, both 0 and -1 produce no delay). Omit this parameter to leave the current PressDuration unchanged. Note: PressDuration also produces a delay after any change to the modifier key state (CTRL, ALT, SHIFT, and WIN) needed to support the keys being sent.</param>
        /// <param name="Play">The word Play applies the above settings to the SendPlay mode rather than the traditional SendEvent mode. If a script never uses this parameter, the delay is always -1/-1 for SendPlay.</param>
        public void KeyDelay(string Delay = "", string PressDuration = "", string Play = "")
        {
            ErrorLog_Setup(false);
            Execute("SetKeyDelay, " + Delay + "," + PressDuration + "," + Play);
        }

        /// <summary>Sets the state of the Capslock key. Can also force the key to stay on or off.</summary>
        /// <param name="OnState">Default = On || OnState = True (On): Turns on the key and removes the AlwaysOn/Off attribute of the key (if present). | OnState = False (Off): Turns off the key and removes the AlwaysOn/Off attribute of the key (if present). | AlwaysOn: Forces the key to stay on permanently | AlwaysOff: Forces the key to stay off permanently </param>
        public void CapsLock(bool OnState = true)
        {
            string State = "On";
            if (!OnState) { State = "Off"; }

            ErrorLog_Setup(false);
            Execute("SetCapsLockState, " + State);
        }

        /// <summary>Sets the state of the NumLock key. Can also force the key to stay on or off.</summary>
        /// <param name="OnState">Default = AlwaysOn || On (True): Turns on the key and removes the AlwaysOn/Off attribute of the key (if present). | Off (False): Turns off the key and removes the AlwaysOn/Off attribute of the key (if present). | AlwaysOn: Forces the key to stay on permanently | AlwaysOff: Forces the key to stay off permanently </param>
        public void NumLock(bool OnState = true)
        {
            string State = "On";
            if (!OnState) { State = "Off"; }

            ErrorLog_Setup(false);
            Execute("SetNumLockState, " + State);
        }

        /// <summary>Sets the state of the ScrollLock key. Can also force the key to stay on or off.</summary>
        /// <param name="OnState">Default = AlwaysOn || On (True): Turns on the key and removes the AlwaysOn/Off attribute of the key (if present). | Off (False): Turns off the key and removes the AlwaysOn/Off attribute of the key (if present). | AlwaysOn: Forces the key to stay on permanently | AlwaysOff: Forces the key to stay off permanently </param>
        public void ScrollLock(bool OnState = true)
        {
            string State = "On";
            if (!OnState) { State = "Off"; }

            ErrorLog_Setup(false);
            Execute("SetScrollLockState, " + State);
        }

        /// <summary>Whether to restore the state of CapsLock after a Send.</summary>
        /// <param name="OnState">On (True): This is the initial setting for all scripts: The CapsLock key will be restored to its former value if Send needed to change it temporarily for its operation. | Off (True): The state of the CapsLock key is not changed at all. As a result, Send will invert the case of the characters if Capslock happens to be ON during the operation. </param>
        public void SetStoreCapslockMode(bool OnState = true)
        {
            string State = "On";
            if (!OnState) { State = "Off"; }

            ErrorLog_Setup(false);
            Execute("SetStoreCapslockMode, " + State);
        }


        #endregion
    }
}
