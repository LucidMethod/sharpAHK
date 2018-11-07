using sharpAHK;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AHKExpressions
{
    public static partial class StringExtension
    {
        // #region === Clipboard Extentions ===


        /// <summary>
        /// Returns Contents of the OS's Clipboard. Writes to Clipboard if Value Provided in WriteText Param
        /// </summary>
        /// <param name="WriteText">If Provided, Writes Text to Clipboard Instead of Returning Clipboard Contents</param>
        /// <returns>Returns Contents of Clipboard with Option to Write New Clipboard Value</returns>
        public static string Clipboard(string WriteText = "")
        {
            _AHK ahk = new _AHK();
            return ahk.Clipboard(WriteText);
        }

        /// <summary>
        /// Returns Entire Contents of Clipboard (Such as Formatting and Text)
        /// </summary>
        /// <returns>Returns String with Entire Contents of Clipboard</returns>
        public static string ClipboardAll()
        {
            _AHK ahk = new _AHK();
            return ahk.ClipboardAll();
        }

        /// <summary>
        /// Waits for Clipboard To Contain value, returns Clipboard contents
        /// </summary>
        /// <param name="SecondsToWait">Seconds to Wait Before ClipWait Times Out</param>
        /// <returns></returns>
        public static string ClipWait(int SecondsToWait = 5)
        {
            _AHK ahk = new _AHK();
            return ahk.ClipWait(SecondsToWait);
        }

        /// <summary>
        /// Clears Contents of Clipboard
        /// </summary>
        public static void ClipboardClear()
        {
            _AHK ahk = new _AHK();
            ahk.ClipboardClear();
        }

        /// <summary>
        /// Write Text to Clipboard (Blank to Clear)
        /// </summary>
        /// <param name="WriteText">Writes Text to Clipboard</param>
        public static void ClipWrite(string WriteText = "")
        {
            _AHK ahk = new _AHK();
            ahk.ClipWrite(WriteText);
        }

        /// <summary>
        /// Adds One or More Files to Windows Clipboard, Ready to Paste in Explorer
        /// </summary>
        /// <param name="fileList">FileList can be a Single File Path (as string) or List of File Paths (strings)</param>
        /// <returns></returns>
        public static bool Clipboard_Files(object fileList)
        {
            _AHK ahk = new _AHK();
            return ahk.Clipboard_Files(fileList);
        }

    }
}
