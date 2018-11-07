using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace AHKExpressions
{
    public static partial class StringExtension
    {
        //#region === Dialogs ===

        /// <summary>
        /// Behaves Same as MsgBox Command, Options are PreDefined Here for Faster Config
        /// </summary>
        /// <param name="Text">Text to Display in MessageBox</param>
        /// <param name="Title">MsgBox Title</param>
        /// <param name="Button">Button Options (Default = OK)</param>
        /// <param name="Icon">Button Icon (Default = None)</param>
        /// <param name="TimeOut">Seconds to Wait on User Input Before Timeout (Default = -1 Which Disables TimeOut)</param>
        /// <returns>Returns Button Text User Clicked or 'TimeOut' if User Didn't Click Before TimeOut Reached</returns>
        public static string MsgBox(this string Text)
        {
            MessageBox.Show(Text.ToString());
            return "";
        }

        /// <summary>yes/no user prompt</summary>
        /// <param name="Question"> </param>
        /// <param name=" Title"> </param>
        public static DialogResult YesNoBox(this string Question, string Title)
        {
            //// EX: 
            //var ResultValue = ahk.YesNoBox("Delete " + FileName + "?", "Delete File?");
            //if (ResultValue.ToString() == "Yes") { ahk.FileDelete(FilePath); }


            DialogResult result = MessageBox.Show(Question, Title, MessageBoxButtons.YesNo);
            return result;
        }

        /// <summary>yes/no/cancel prompt for user input</summary>
        /// <param name="Question"> </param>
        /// <param name=" Title"> </param>
        public static DialogResult YesNoCancelBox(this string Question, string Title)  // yes/no/cancel prompt for user input
        {
            //// EX: 
            //var ResultValue = ahk.YesNoCancelBox("Delete ?", "Delete File?");
            //if (ResultValue.ToString() == "Cancel") { ahk.MsgBox("Canceled"); }


            DialogResult result = MessageBox.Show(Question, Title, MessageBoxButtons.YesNoCancel);
            return result;
        }

        

    }
}
