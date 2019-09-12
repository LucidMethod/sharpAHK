using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace AHKExpressions
{
    public static partial class AHKExpressions
    {
        //#region === Dialogs ===

        /// <summary>
        /// Behaves Same as MsgBox Command, Options are PreDefined Here for Faster Config
        /// </summary>
        /// <param name="Text">Text to Display in MessageBox</param>
        public static void MsgBox(this string Text)
        {
            MessageBox.Show(Text.ToString());
        }

        /// <summary>yes/no user prompt</summary>
        /// <param name="Question"> </param>
        /// <param name=" Title"> </param>
        public static DialogResult YesNoBox(this string Question, string Title)
        {
            //// EX: 
            //var ResultValue = ahk.YesNoBox("Delete " + FileName + "?", "Delete File?");
            //if (ResultValue.ToString() == "Yes") { ahk.FileDelete(FilePath); }

            return MessageBox.Show(Question, Title, MessageBoxButtons.YesNo);
        }

        /// <summary>yes/no/cancel prompt for user input</summary>
        /// <param name="Question"> </param>
        /// <param name=" Title"> </param>
        public static DialogResult YesNoCancelBox(this string Question, string Title)  // yes/no/cancel prompt for user input
        {
            //// EX: 
            //var ResultValue = ahk.YesNoCancelBox("Delete ?", "Delete File?");
            //if (ResultValue.ToString() == "Cancel") { ahk.MsgBox("Canceled"); }

            return MessageBox.Show(Question, Title, MessageBoxButtons.YesNoCancel);
        }

        

    }
}
