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
        // === Clipboard ===

        /// <summary>
        /// Returns Contents of the OS's Clipboard. Writes to Clipboard if Value Provided in WriteText Param
        /// </summary>
        /// <param name="WriteText">If Provided, Writes Text to Clipboard Instead of Returning Clipboard Contents</param>
        /// <returns>Returns Contents of Clipboard with Option to Write New Clipboard Value</returns>
        public string Clipboard(string WriteText = "")
        {
            if (WriteText != "") { SetVar("clipboard", WriteText); return WriteText; }  // user provided value to Write to Clipboard

            bool AHKMethod = true;

            if (AHKMethod)
            {
                string AHKLine = "OutputVar = %Clipboard%";  // ahk line to execute
                ErrorLog_Setup(false); // ErrorLevel Detection Disabled for this function in AHK 
                string OutVar = Execute(AHKLine, "OutputVar");   // execute AHK code and return variable value 
                return OutVar;
            }
            else  // untested alternative clipboard function 
            {
                string clipboard = "";
                try
                {
                    IDataObject ClipData = System.Windows.Forms.Clipboard.GetDataObject();

                    if (ClipData.GetDataPresent(DataFormats.Text))
                    {
                        clipboard = System.Windows.Forms.Clipboard.GetData(DataFormats.Text).ToString();
                    }
                }
                catch { }
                return clipboard;
            }
        }

        /// <summary>
        /// Returns Entire Contents of Clipboard (Such as Formatting and Text)
        /// </summary>
        /// <returns>Returns String with Entire Contents of Clipboard</returns>
        public string ClipboardAll()
        {
            string AHKLine = "OutputVar = %ClipboardAll%";  // ahk line to execute
            ErrorLog_Setup(false); // ErrorLevel Detection Disabled for this function in AHK 
            string OutVar = Execute(AHKLine, "OutputVar");   // execute AHK code and return variable value 
            return OutVar;
        }

        /// <summary>
        /// Waits for Clipboard To Contain value, returns Clipboard contents
        /// </summary>
        /// <param name="SecondsToWait">Seconds to Wait Before ClipWait Times Out</param>
        /// <param name="AnyData">If False   is More Selective, Waiting for Files/Text To Exist</param>
        /// <returns></returns>
        public string ClipWait(int SecondsToWait = 5)
        {
            string clipboard = "";
            SetVar("clipboard", clipboard);  // clear out clipboard

            //if (AnyData) { AnyDataOnClipboard = 1; }
            //string AHKLine = @"ClipWait, " + SecondsToWait + "," + AnyDataOnClipboard;  // ahk line to execute
            //ErrorLog_Setup(true); // ErrorLevel Detection Enabled for this function in AHK 

            do   // loop until clipboard isn't empty
            {
                clipboard = Clipboard(); // returns clipboard value

            } while (clipboard == "");

            return clipboard;
        }

        /// <summary>
        /// Clears Contents of Clipboard
        /// </summary>
        public void ClipboardClear()
        {
            SetVar("clipboard", "");
        }

        /// <summary>
        /// Write Text to Clipboard (Blank to Clear)
        /// </summary>
        /// <param name="WriteText">Writes Text to Clipboard</param>
        public void ClipWrite(string WriteText = "")
        {
            SetVar("clipboard", WriteText);

            // v1 ToDo
            /*
                    _ClipboardWrite(string WriteText)  // writes text to user's clipboard
                    {
                        System.Windows.Forms.Clipboard.SetDataObject(WriteText);
                    }
            */

        }

        /// <summary>
        /// Adds One or More Files to Windows Clipboard, Ready to Paste in Explorer
        /// </summary>
        /// <param name="fileList">FileList can be a Single File Path (as string) or List of File Paths (strings)</param>
        /// <returns></returns>
        public bool Clipboard_Files(object fileList)
        {
            // determine if FilePath (string) or List of Files was passed in
            string VarType = fileList.GetType().ToString();
            //MsgBox(VarType);
            bool isFile = false; bool isList = false;
            if (VarType == "System.String") { isFile = true; }
            if (VarType == "System.Collections.Generic.List`1[System.String]") { isList = true; }

            System.Collections.Specialized.StringCollection paths = new System.Collections.Specialized.StringCollection();

            if (isList) // list of files passed in as string list
            {
                List<string> files = fileList as List<string>;
                foreach (var item in files)
                {
                    paths.Add(fileList.ToString());
                }
                try { System.Windows.Forms.Clipboard.SetFileDropList(paths); return true; }
                catch { return false; }
            }

            if (isFile)  // single file passed in as string
            {
                paths.Add(fileList.ToString());
                try { System.Windows.Forms.Clipboard.SetFileDropList(paths); return true; }
                catch { return false; }
            }

            return false;
        }


    }
}
