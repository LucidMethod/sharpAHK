using sharpAHK;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace AHKExpressions
{
    public static partial class AHKExpressions
    {
        /// <summary>Converts String/Int To Bool Variable Type</summary>
        /// <param name="TrueFalseVar">String/Int to Convert</param>
        /// <returns>Returns BOOL Variable Type From Input String/Int</returns>
        public static bool ToBool(this object TrueFalseVar)
        {
            if (TrueFalseVar == null) { return false; }
            _AHK ahk = new _AHK();
            return ahk.ToBool(TrueFalseVar);
        }

        /// <summary>Converts String/Bool To Int Variable Type</summary>
        /// <param name="Input">String/Bool/IntPtr to Convert</param>
        /// <returns>Returns INT Variable Type From Input String/Bool/IntPtr</returns>
        public static int ToInt(this object Input)
        {

            //MsgBox("Input: " + Input.ToString());

            string VarType = Input.GetType().ToString();  //determine what kind of variable was passed into function
            int Out = 0;

            if (VarType == "System.Int32")
            {
                Out = (int)Input;
                return Out;
            }

            if (VarType == "System.Boolean")
            {
                string InputBoolString = Input.ToString();
                if (InputBoolString == "True")
                {
                    return 1;
                }
                else
                {
                    return 0;
                }
            }

            if (VarType == "System.String")
            {
                if (Input.ToString() == "") { return 0; }

                try
                {
                    string value = Regex.Replace(Input.ToString(), "[A-Za-z ]", "");
                    //Out = Int32.Parse(value.ToString());  // string to int

                    // if there is a period, adjust to keep int conversion from failing
                    if (value.ToString().Contains("."))
                    {
                        // convert string to double
                        double oOut = Convert.ToDouble(value);

                        // rount double to int
                        Out = (int)Math.Round(oOut);

                        // return int
                        return Out;
                    }
                    else
                    {
                        Out = Int32.Parse(value.ToString());  // string to int
                    }

                    return Out;
                }
                catch { return 0; }
            }

            if (VarType == "System.IntPtr")
            {
                Out = (int)Input;
                return Out;
            }

            if (VarType == "System.Double")
            {
                double val = (double)Input;
                Out = (int)Math.Round(val);
                //Out = Convert.ToInt32(Input);
                return Out;
            }

            if (VarType == "System.Long")
            {
                Out = Convert.ToInt32(Input);
                return Out;
            }

            return Out;
        }

        /// <summary>convert string / int to IntPtr</summary>
        /// <param name="object Input"> </param>
        /// <returns>Returns IntPtr Variable Type From Input String/Int</returns>
        public static IntPtr ToIntPtr(this object Input)
        {
            string VarType = Input.GetType().ToString();  //determine what kind of variable was passed into function

            IntPtr nIntPtr = new IntPtr(0);

            if (VarType == "System.Int32" || VarType == "System.String") //String/Int32 to IntPtr
            {
                int ConvertInt = Int32.Parse(Input.ToString());  // convert object to int
                IntPtr myPtr = new IntPtr(ConvertInt); //convert Int to IntPtr
                return myPtr;
            }

            //if (VarType == "System.IntPtr")

            return nIntPtr;
        }

        // untested
        public static string IntPtr_ToString(this Encoding encoding, IntPtr ptr, int length)
        {
            //	null pointer = null string
            if (ptr == IntPtr.Zero)
                return null;

            //	0 length string = string.Empty
            if (length == 0)
                return string.Empty;

            byte[] buff = new byte[length];
            Marshal.Copy(ptr, buff, 0, length);

            //	We don't want to carry over the Trailing null
            if (buff[buff.Length - 1] == 0)
                length--;

            return encoding.GetString(buff, 0, length);
        }


        /// <summary>Converts String/Int To DateTime Format</summary>
        /// <param name="TimeString">FileSize on Bytes</param>
        /// <returns>Returns DateTime Variable Type From TimeString</returns>
        public static DateTime ToDateTime(this object TimeString)
        {
            string VarType = TimeString.GetType().ToString();  //determine what kind of variable was passed into function

            if (VarType == "System.String" || VarType == "System.Int32" || VarType == "System.DBNull")
            {
                DateTime enteredDate = new DateTime(1900, 1, 1);
                try
                {
                    enteredDate = DateTime.Parse(TimeString.ToString());
                }
                catch { }

                return enteredDate;
            }

            if (VarType == "System.DateTime") { return (DateTime)TimeString; }

            MsgBox("Unable To Format This VarType Yet: " + VarType);
            return DateTime.Now;
        }


        #region === Convert Bytes ===



        /// <summary>Returns a human-readable size discriptor for up 64-bit length fields (adds kb/mb/tb to return)</summary>
        /// <param name="bytes">FileSize on Bytes</param>
        /// <returns>Returns string with FileSize with Units (KB/MB/TB etc)</returns>
        public static string FormatBytes(this Int64 bytes)
        {
            if (bytes >= 0x1000000000000000) { return ((double)(bytes >> 50) / 1024).ToString("0.## EB"); }
            if (bytes >= 0x4000000000000) { return ((double)(bytes >> 40) / 1024).ToString("0.## PB"); }
            if (bytes >= 0x10000000000) { return ((double)(bytes >> 30) / 1024).ToString("0.## TB"); }
            if (bytes >= 0x40000000) { return ((double)(bytes >> 20) / 1024).ToString("0.## GB"); }
            if (bytes >= 0x100000) { return ((double)(bytes >> 10) / 1024).ToString("0.## MB"); }
            if (bytes >= 0x400) { return ((double)(bytes) / 1024).ToString("0.##") + " KB"; }
            return bytes.ToString("0 Bytes");
        }

        /// <summary>
        /// Convert Bytes to Megabytes
        /// </summary>
        /// <param name="bytes"></param>
        /// <returns></returns>
        public static double Bytes_To_MB(this long bytes)
        {
            return (bytes / 1024f) / 1024f;
        }

        /// <summary>
        /// Convert Kilobytes To Megabytes
        /// </summary>
        /// <param name="kilobytes"></param>
        /// <returns></returns>
        public static double KB_To_MB(this long kilobytes)
        {
            return kilobytes / 1024f;
        }

        /// <summary>
        /// Convert Megabytes To Gigabytes
        /// </summary>
        /// <param name="megabytes"></param>
        /// <returns></returns>
        public static double MB_To_GB(this double megabytes)
        {
            // 1024 megabyte in a gigabyte
            return megabytes / 1024.0;
        }

        /// <summary>
        /// Convert Megabytes To Terabytes
        /// </summary>
        /// <param name="megabytes"></param>
        /// <returns></returns>
        public static double MB_To_TB(this double megabytes)
        {
            // 1024 * 1024 megabytes in a terabyte
            return megabytes / (1024.0 * 1024.0);
        }

        /// <summary>
        /// Convert Gigabytes To Megabytes
        /// </summary>
        /// <param name="gigabytes"></param>
        /// <returns></returns>
        public static double GB_To_MB(this double gigabytes)
        {
            // 1024 gigabytes in a terabyte
            return gigabytes * 1024.0;
        }

        /// <summary>
        /// Convert Gigabytes To Terabytes
        /// </summary>
        /// <param name="gigabytes"></param>
        /// <returns></returns>
        public static double GB_To_TB(this double gigabytes)
        {
            // 1024 gigabytes in a terabyte
            return gigabytes / 1024.0;
        }

        /// <summary>
        /// Convert Terabytes To Megabytes
        /// </summary>
        /// <param name="terabytes"></param>
        /// <returns></returns>
        public static double TB_To_MB(this double terabytes)
        {
            // 1024 * 1024 megabytes in a terabyte
            return terabytes * (1024.0 * 1024.0);
        }

        /// <summary>
        /// Convert Terabytes To Gigabytes
        /// </summary>
        /// <param name="terabytes"></param>
        /// <returns></returns>
        public static double TB_To_GB(this double terabytes)
        {
            // 1024 gigabytes in a terabyte
            return terabytes * 1024.0;
        }




        #endregion


        /// <summary>
        /// Returns String With Variable Type Passed Into Object Parameter, Option to Display Type In MessageBox
        /// </summary>
        /// <param name="Object">Object To Check For Variable Type</param>
        /// <param name="DisplayVarType">Option to Display Results in MessageBox (Default = False)</param>
        /// <returns></returns>
        public static string VarType(this object Object, bool DisplayVarType = false)
        {
            if (Object == null) { return ""; }

            string VarType = Object.GetType().ToString();  //determine what kind of variable was passed into function

            //### CONTROLS #########################
            //System.Windows.Forms.Button
            //System.Windows.Forms.CheckBox
            //System.Windows.Forms.DataGridView
            //System.Windows.Forms.ListBox
            //System.Windows.Forms.PictureBox
            //System.Windows.Forms.TabControl
            //System.Windows.Forms.TabPage
            //System.Windows.Forms.TableLayoutPanel
            //System.Windows.Forms.TextBox
            //System.Windows.Forms.ToolStripMenuItem
            //System.Windows.Forms.TreeView

            //ScintillaNET.Scintilla
            //TreeViewFast.Controls.TreeViewFast


            //### VARIABLES #########################
            // System.String (string)
            // System.Int32 (int)
            // System.Int64 (long)
            // System.Collections.Generic.List`1(System.String)  (List<string>)
            // System.Collections.Generic.List`1(System.Int32)   (List<int>)

            if (DisplayVarType) { MsgBox(VarType); }

            return VarType;
        }


    }
}
