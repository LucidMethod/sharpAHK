using sharpAHK;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AHKExpressions
{
    public static partial class AHKExpressions
    {
        // #region === Clipboard Extentions ===

        /// <summary>
        /// Write Text to Clipboard
        /// </summary>
        /// <param name="WriteText">Writes Text to Clipboard</param>
        public static void ToClipboard(this string WriteText)
        {
            _AHK ahk = new _AHK();
            ahk.ClipWrite(WriteText);
        }


    }
}
