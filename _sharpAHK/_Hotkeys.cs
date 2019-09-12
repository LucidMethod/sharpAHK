using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace sharpAHK
{
    public partial class _AHK
    {
        // Example Use:

        // Define Hotkey Combinations and Actions to Execute
        //public void Setup_Hotkeys()
        //{
        ////== Setup Hotkeys and Save To Initialize ==
        //ahk.Add_Hotkey("F4", "HotkeyAction");
        //ahk.Add_Hotkey("F5", "Hotkey_Pressed");
        //ahk.Add_Hotkey("F6", "Hotkey_Pressed");
        //ahk.Save_Hotkeys(this);
        //}

        ////== Add functions to respond to pressed hotkeys
        //public void HotkeyAction()
        //{
        //    ahk.MsgBox("Function Fired - No Parameters");
        //}
        //public void Hotkey_Pressed(string Hotkey)
        //{
        //    ahk.MsgBox("Function Fired - Hotkey = " + Hotkey);
        //}


        /// <summary>Intializes Hotkey Communication with Main Application - Allows for AutoHotkey to execute functions in Main Application (non-AHK Code like C#)</summary>
        /// <param name="form">Name of the Form Setting Up the Hotkey. Ex: Hotkey_Setup(this);</param>
        private void Hotkey_Setup(Form form)
        {
            //you can have AutoHotkey communicate with the hosting environment 
            // 1 - Create Handler for your ahk code 
            // 2 - Initalize Pipes Module, passing in your handler
            // 3 - Use 'SendPipeMessage(string)' from your AHK code
            var ipcHandler = new Func<string, string>(fromAhk =>
            {
                //MessageBox.Show("ipcHander: " + fromAhk); 
                //Console.WriteLine("received message from ahk " + fromAhk);
                //System.Threading.Thread.Sleep(3000); //simulating lots of work

                string PressedHK = Execute("HK := A_ThisHotkey", "HK");



                string[] parameters = { PressedHK };

                object OBJ = form;
                var objType = OBJ.GetType();
                var method = objType.GetMethod(fromAhk);

                try
                {
                    method.Invoke(OBJ, parameters); // try executing function with 1 parameter passed back - the pressed hotkey detected - ex: HotkeyActions(string HotkeyPressed)
                }
                catch  // if that fails
                {
                    try
                    {
                        parameters = null;
                        method.Invoke(OBJ, parameters);  // execute function without any parameters = ex: FunctionName()
                    }
                    catch
                    {
                        MsgBox("Failed To Execute Method Using Hotkey: " + PressedHK + " - Can Only Contain 1 Parameter.");
                    }
                }

                return PressedHK;
            });


            //create an autohtkey engine.
            if (ahkGlobal.ahkdll == null) { ahkGlobal.ahkdll = new AutoHotkey.Interop.AutoHotkeyEngine(); }
            var ahkdll = ahkGlobal.ahkdll;

            //the initalize pipes module only needs to be called once per application
            if (!HKInit)
            {
                ahkdll.InitalizePipesModule(ipcHandler);
                HKInit = true;
            }
        }

        bool HKInit = false;  // stores bool indicating whether user has initiated using hotkeys in this AHK session
        string hkList;  // holds list of hotkey definitions added with Add_Hotkey command - populated before Save_Hotkeys is run
        string hkListDisp;  // holds list of hotkey definitions added with Add_Hotkey command - populated before Save_Hotkeys is run - formated to display as text output when saving

        /// <summary>Add Hotkey Command The Triggers a Function/Method In Your Main Application</summary>
        /// <param name="Hotkey">Combination of keys to pressed to call executing a function/method</param>
        /// <param name="Method">Function/Method in Main Application To Run When Hotkey Is Pressed</param>
        public void Add_Hotkey(string Hotkey, string Method)
        {
            if (hkList != null)
            {
                hkList = hkList + "\n\n" + Hotkey + @"::SendPipeMessage(""" + Method + @""")";
                hkListDisp = hkListDisp + "\n\n" + Hotkey + " : " + Method;
            }
            if (hkList == null)
            {
                hkList = Hotkey + @"::SendPipeMessage(""" + Method + @""")";
                hkListDisp = "[Hotkey Definitions]\n\n" + Hotkey + " : " + Method;
            }

            //string Script = @"F5::SendPipeMessage(""TEST"")" + "\n" + @"F6::SendPipeMessage(""TestFunc"")";
            //ahk.Load_Functions(Script);
        }

        /// <summary>Save Command That Initializes Hotkeys Added using Add_Hotkey()</summary>
        /// <param name="ThisForm">Name of the Form Setting Up the Hotkey. Ex: Save_Hotkeys(this);</param>
        public void Save_Hotkeys(Form ThisForm)
        {
            ErrorLog_Setup(false);
            Hotkey_Setup(ThisForm);
            Load_ahkString(hkList);
        }


        /// <summary>Creates, modifies, enables, or disables a hotkey while the script is running.</summary>
        /// <param name="KeyName">Name of the hotkey's activation key, including any modifier symbols. For example, specify #c for the Win+C hotkey. If KeyName already exists as a hotkey, that hotkey will be updated with the values of the command's other parameters. KeyName can also be the name of an existing hotkey label (i.e. a double-colon label), which will cause that hotkey to be updated with the values of the command's other parameters. When specifying an existing hotkey, KeyName is not case sensitive. However, the names of keys must be spelled the same as in the existing hotkey (e.g. Esc is not the same as Escape for this purpose). Also, the order of modifier symbols such as ^!+# does not matter. The current IfWin setting determines the variant of a hotkey upon which the Hotkey command will operate. If the variant does not yet exist, it will be created. When a hotkey is first created -- either by the Hotkey command or a double-colon label in the script -- its key name and the ordering of its modifier symbols becomes the permanent name of that hotkey as reflected by A_ThisHotkey. This name does not change even if the Hotkey command later accesses the hotkey with a different symbol ordering. </param>
        /// <param name="Label">The label name whose contents will be executed (as a new thread) when the hotkey is pressed. Both normal labels and hotkey/hotstring labels can be used. The trailing colon(s) should not be included. If Label is dynamic (e.g. %VarContainingLabelName%), IsLabel(VarContainingLabelName) may be called beforehand to verify that the label exists. This parameter can be left blank if KeyName already exists as a hotkey, in which case its label will not be changed. This is useful to change only the hotkey's Options. If the label is specified but the hotkey is disabled from a previous use of this command, the hotkey will remain disabled. To prevent this, include the word ON in Options. This parameter can also be one of the following special values: On: The hotkey becomes enabled. No action is taken if the hotkey is already On. Off: The hotkey becomes disabled. No action is taken if the hotkey is already Off. Toggle: The hotkey is set to the opposite state (enabled or disabled). AltTab (and others): These are special Alt-Tab hotkey actions that are described here. Note: The current IfWin setting determines the variant of a hotkey upon which On/Off/Toggle will operate.</param>
        /// <param name="Options">A string of zero or more of the following letters with optional spaces in between. For example: UseErrorLevel B0 UseErrorLevel: If the command encounters a problem, this option skips the warning dialog, sets ErrorLevel to one of the codes from the table below, then allows the current thread to continue. On: Enables the hotkey if it is currently disabled. Off: Disables the hotkey if it is currently enabled. This is typically used to create a hotkey in an initially-disabled state. B or B0: Specify the letter B to buffer the hotkey as described in #MaxThreadsBuffer. Specify B0 (B with the number 0) to disable this type of buffering. Pn: Specify the letter P followed by the hotkey's thread priority. If the P option is omitted when creating a hotkey, 0 will be used. Tn: Specify the letter T followed by a the number of threads to allow for this hotkey as described in #MaxThreadsPerHotkey. For example: T5 If either or both of the B and T option letters are omitted and the hotkey already exists, those options will not be changed. But if the hotkey does not yet exist -- that is, it is about to be created by this command -- the options will default to those most recently in effect. For example, the instance of #MaxThreadsBuffer that occurs closest to the bottom of the script will be used. If #MaxThreadsBuffer does not appear in the script, its default setting (OFF in this case) will be used. This behavior also applies to #IfWin: the bottommost occurrence applies to newly created hotkeys unless "Hotkey IfWin" has executed since the script started. Note: The current IfWin setting determines the variant of a hotkey upon which the Hotkey command will operate. If the variant does not yet exist, it will be created.</param>
        public void Hotkey(string KeyName, string Label = "", string Options = "")
        {
            ErrorLog_Setup(true);
            Execute("Hotkey, " + KeyName + "," + Label + "," + Options);
        }

        /// <summary>Context sensitive Hotkeys, enabled when specific window is active.</summary>
        /// <param name="KeyName">Name of the hotkey's activation key, including any modifier symbols. For example, specify #c for the Win+C hotkey. If KeyName already exists as a hotkey, that hotkey will be updated with the values of the command's other parameters. KeyName can also be the name of an existing hotkey label (i.e. a double-colon label), which will cause that hotkey to be updated with the values of the command's other parameters. When specifying an existing hotkey, KeyName is not case sensitive. However, the names of keys must be spelled the same as in the existing hotkey (e.g. Esc is not the same as Escape for this purpose). Also, the order of modifier symbols such as ^!+# does not matter. The current IfWin setting determines the variant of a hotkey upon which the Hotkey command will operate. If the variant does not yet exist, it will be created. When a hotkey is first created -- either by the Hotkey command or a double-colon label in the script -- its key name and the ordering of its modifier symbols becomes the permanent name of that hotkey as reflected by A_ThisHotkey. This name does not change even if the Hotkey command later accesses the hotkey with a different symbol ordering. </param>
        /// <param name="Label">The label name whose contents will be executed (as a new thread) when the hotkey is pressed. Both normal labels and hotkey/hotstring labels can be used. The trailing colon(s) should not be included. If Label is dynamic (e.g. %VarContainingLabelName%), IsLabel(VarContainingLabelName) may be called beforehand to verify that the label exists. This parameter can be left blank if KeyName already exists as a hotkey, in which case its label will not be changed. This is useful to change only the hotkey's Options. If the label is specified but the hotkey is disabled from a previous use of this command, the hotkey will remain disabled. To prevent this, include the word ON in Options. This parameter can also be one of the following special values: On: The hotkey becomes enabled. No action is taken if the hotkey is already On. Off: The hotkey becomes disabled. No action is taken if the hotkey is already Off. Toggle: The hotkey is set to the opposite state (enabled or disabled). AltTab (and others): These are special Alt-Tab hotkey actions that are described here. Note: The current IfWin setting determines the variant of a hotkey upon which On/Off/Toggle will operate.</param>
        /// <param name="WinTitle">The title or partial title of the target window (the matching behavior is determined by SetTitleMatchMode). If this and the other 3 parameters are omitted, the Last Found Window will be used. To use a window class, specify ahk_class ExactClassName (shown by Window Spy). To use a process identifier (PID), specify ahk_pid %VarContainingPID%. To use a window group, specify ahk_group GroupName. To use a window's unique ID number, specify ahk_id %VarContainingID%. The search can be narrowed by specifying multiple criteria. For example: My File.txt ahk_class Notepad</param>
        /// <param name="WinText">If present, this parameter must be a substring from a single text element of the target window (as revealed by the included Window Spy utility). Hidden text elements are detected if DetectHiddenText is ON.</param>
        public void Hotkey_IfWinActive(string KeyName, string Label, string WinTitle = "", string WinText = "")
        {
            // same as Hotkey command - added for cooresponding var names
            ErrorLog_Setup(true);
            Execute("Hotkey, IfWinActive," + WinTitle + "," + WinText + "\n" + "Hotkey, " + KeyName + ", " + Label);
        }

        /// <summary>Context sensitive Hotkeys, enabled when specific window exists.</summary>
        /// <param name="KeyName">Name of the hotkey's activation key, including any modifier symbols. For example, specify #c for the Win+C hotkey. If KeyName already exists as a hotkey, that hotkey will be updated with the values of the command's other parameters. KeyName can also be the name of an existing hotkey label (i.e. a double-colon label), which will cause that hotkey to be updated with the values of the command's other parameters. When specifying an existing hotkey, KeyName is not case sensitive. However, the names of keys must be spelled the same as in the existing hotkey (e.g. Esc is not the same as Escape for this purpose). Also, the order of modifier symbols such as ^!+# does not matter. The current IfWin setting determines the variant of a hotkey upon which the Hotkey command will operate. If the variant does not yet exist, it will be created. When a hotkey is first created -- either by the Hotkey command or a double-colon label in the script -- its key name and the ordering of its modifier symbols becomes the permanent name of that hotkey as reflected by A_ThisHotkey. This name does not change even if the Hotkey command later accesses the hotkey with a different symbol ordering. </param>
        /// <param name="Label">The label name whose contents will be executed (as a new thread) when the hotkey is pressed. Both normal labels and hotkey/hotstring labels can be used. The trailing colon(s) should not be included. If Label is dynamic (e.g. %VarContainingLabelName%), IsLabel(VarContainingLabelName) may be called beforehand to verify that the label exists. This parameter can be left blank if KeyName already exists as a hotkey, in which case its label will not be changed. This is useful to change only the hotkey's Options. If the label is specified but the hotkey is disabled from a previous use of this command, the hotkey will remain disabled. To prevent this, include the word ON in Options. This parameter can also be one of the following special values: On: The hotkey becomes enabled. No action is taken if the hotkey is already On. Off: The hotkey becomes disabled. No action is taken if the hotkey is already Off. Toggle: The hotkey is set to the opposite state (enabled or disabled). AltTab (and others): These are special Alt-Tab hotkey actions that are described here. Note: The current IfWin setting determines the variant of a hotkey upon which On/Off/Toggle will operate.</param>
        /// <param name="WinTitle">The title or partial title of the target window (the matching behavior is determined by SetTitleMatchMode). If this and the other 3 parameters are omitted, the Last Found Window will be used. To use a window class, specify ahk_class ExactClassName (shown by Window Spy). To use a process identifier (PID), specify ahk_pid %VarContainingPID%. To use a window group, specify ahk_group GroupName. To use a window's unique ID number, specify ahk_id %VarContainingID%. The search can be narrowed by specifying multiple criteria. For example: My File.txt ahk_class Notepad</param>
        /// <param name="WinText">If present, this parameter must be a substring from a single text element of the target window (as revealed by the included Window Spy utility). Hidden text elements are detected if DetectHiddenText is ON.</param>
        public void Hotkey_IfWinExist(string KeyName, string Label, string WinTitle = "", string WinText = "")
        {
            // same as Hotkey command - added for cooresponding var names
            ErrorLog_Setup(true);
            Execute("Hotkey, IfWinExist," + WinTitle + "," + WinText + "\n" + "Hotkey, " + KeyName + ", " + Label);
        }

        /// <summary>Displays the hotkeys in use by the current script, whether their subroutines are currently running, and whether or not they use the keyboard or mouse hook.</summary>
        public void ListHotkeys()
        {
            ErrorLog_Setup(false);
            Execute("ListHotkeys");
        }


    }
}
