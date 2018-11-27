using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows.Forms;
using AHKExpressions;

namespace sharpAHK
{
    public partial class _AHK
    {
        // === Built in Variables ===

        #region === Convert Variable Types ===

        /// <summary>Converts String/Int To Bool Variable Type</summary>
        /// <param name="TrueFalseVar">String/Int to Convert</param>
        /// <returns>Returns BOOL Variable Type From Input String/Int</returns>
        public bool ToBool(object TrueFalseVar)
        {
            string VarType = TrueFalseVar.GetType().ToString();  //determine what kind of variable was passed into function

            if (VarType == "System.Int32")
            {
                if (TrueFalseVar.ToString() == "1") { return true; }
                if (TrueFalseVar.ToString() == "0") { return false; }
            }

            if (VarType == "System.String")
            {
                if (TrueFalseVar.ToString().ToUpper() == "TRUE") { return true; }
                else { return false; }
            }

            MsgBox(VarType + " Not Setup For ToBool() Conversion");
            return false;
        }

        /// <summary>Converts String/Bool To Int Variable Type</summary>
        /// <param name="Input">String/Bool/IntPtr to Convert</param>
        /// <returns>Returns INT Variable Type From Input String/Bool/IntPtr</returns>
        public int ToInt(object Input)
        {
            if (Input == null) { return -1; }
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
                if (Input == "") { return 0; }

                try
                {
                    string value = Regex.Replace(Input.ToString(), "[A-Za-z ]", "");

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
        /// <param name="Input"> </param>
        /// <returns>Returns IntPtr Variable Type From Input String/Int</returns>
        public IntPtr ToIntPtr(object Input)
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

        /// <summary>
        /// Convert IntPtr to String (Untested)
        /// </summary>
        /// <param name="encoding"></param>
        /// <param name="ptr"></param>
        /// <param name="length"></param>
        /// <returns></returns>
        public static string IntPtr_ToString(Encoding encoding, IntPtr ptr, int length)
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
        public DateTime ToDateTime(object TimeString)
        {
            string VarType = TimeString.GetType().ToString();  //determine what kind of variable was passed into function

            if (VarType == "System.String" || VarType == "System.Int32")
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
        /// <param name="fileBytes">FileSize on Bytes</param>
        /// <returns>Returns string with FileSize with Units (KB/MB/TB etc)</returns>
        public string FormatBytes(object fileBytes)
        {
            //public string FormatBytes(Int64 bytes)

            Int64 bytes;

            string type = fileBytes.GetType().ToString();

            if (type == "System.String")
            {
                bool result = Int64.TryParse((string)fileBytes, out bytes);
            }
            else if (type == "System.Int32")
            {
                bytes = Convert.ToInt64(fileBytes);
            }
            else
            {
                bytes = (Int64)fileBytes;
            }



            if (bytes >= 0x1000000000000000) { return ((double)(bytes >> 50) / 1024).ToString("0.## EB"); }
            if (bytes >= 0x4000000000000) { return ((double)(bytes >> 40) / 1024).ToString("0.## PB"); }
            if (bytes >= 0x10000000000) { return ((double)(bytes >> 30) / 1024).ToString("0.## TB"); }
            if (bytes >= 0x40000000) { return ((double)(bytes >> 20) / 1024).ToString("0.## GB"); }
            if (bytes >= 0x100000) { return ((double)(bytes >> 10) / 1024).ToString("0.## MB"); }
            if (bytes >= 0x400) { return ((double)(bytes) / 1024).ToString("0.##") + " KB"; }
            return bytes.ToString("0 Bytes");
        }


        /// <summary>
        /// Converts Formated FileSize (ex: 42 MB) and Converts to Bytes
        /// </summary>
        /// <param name="FormattedFileSize">string with file size, ex: 32 KB/MB/GB/TB</param>
        /// <returns></returns>
        public int ToBytes(string FormattedFileSize)
        {
            string size = FormattedFileSize;
            double Bytes = 0;

            if (size.ToUpper().Contains("EB"))
            {
                string bytes = size.ToUpper().Replace(" EB", "").Trim();
                Bytes = Convert.ToDouble(bytes);
                Bytes = Bytes * 1024.0 * 1024.0 * 1024.0 * 1024.0 * 1024.0 * 1024.0;
            }
            if (size.ToUpper().Contains("PB"))
            {
                string bytes = size.ToUpper().Replace(" PB", "").Trim();
                Bytes = Convert.ToDouble(bytes);
                Bytes = Bytes * 1024.0 * 1024.0 * 1024.0 * 1024.0 * 1024.0;
            }
            if (size.ToUpper().Contains("TB"))
            {
                string bytes = size.ToUpper().Replace(" TB", "").Trim();
                Bytes = Convert.ToDouble(bytes);
                Bytes = Bytes * 1024.0 * 1024.0 * 1024.0 * 1024.0;
            }
            if (size.ToUpper().Contains("GB"))
            {
                string bytes = size.ToUpper().Replace(" GB", "").Trim();
                Bytes = Convert.ToDouble(bytes);
                Bytes = Bytes * 1024.0 * 1024.0 * 1024.0;
            }
            if (size.ToUpper().Contains("MB"))
            {
                string bytes = size.ToUpper().Replace(" MB", "").Trim();
                Bytes = Convert.ToDouble(bytes);
                Bytes = Bytes * 1024.0 * 1024.0;
            }
            if (size.ToUpper().Contains("KB"))
            {
                string bytes = size.ToUpper().Replace(" KB", "").Trim();
                Bytes = Convert.ToDouble(bytes);
                Bytes = Bytes * 1024.0;
            }

            return (int)Bytes;
        }



        /// <summary>
        /// Convert Bytes to Megabytes
        /// </summary>
        /// <param name="bytes"></param>
        /// <returns></returns>
        public double Bytes_To_MB(long bytes)
        {
            return (bytes / 1024f) / 1024f;
        }


        /// <summary>
        /// Convert Kilobytes To Megabytes
        /// </summary>
        /// <param name="kilobytes"></param>
        /// <returns></returns>
        public double KB_To_MB(long kilobytes)
        {
            return kilobytes / 1024f;
        }

        /// <summary>
        /// Convert Megabytes To Gigabytes
        /// </summary>
        /// <param name="megabytes"></param>
        /// <returns></returns>
        public double MB_To_GB(double megabytes)
        {
            // 1024 megabyte in a gigabyte
            return megabytes / 1024.0;
        }

        /// <summary>
        /// Convert Megabytes To Terabytes
        /// </summary>
        /// <param name="megabytes"></param>
        /// <returns></returns>
        public double MB_To_TB(double megabytes)
        {
            // 1024 * 1024 megabytes in a terabyte
            return megabytes / (1024.0 * 1024.0);
        }

        /// <summary>
        /// Convert Gigabytes To Megabytes
        /// </summary>
        /// <param name="gigabytes"></param>
        /// <returns></returns>
        public double GB_To_MB(double gigabytes)
        {
            // 1024 gigabytes in a terabyte
            return gigabytes * 1024.0;
        }

        /// <summary>
        /// Convert Gigabytes To Terabytes
        /// </summary>
        /// <param name="gigabytes"></param>
        /// <returns></returns>
        public double GB_To_TB(double gigabytes)
        {
            // 1024 gigabytes in a terabyte
            return gigabytes / 1024.0;
        }

        /// <summary>
        /// Convert Terabytes To Megabytes
        /// </summary>
        /// <param name="terabytes"></param>
        /// <returns></returns>
        public double TB_To_MB(double terabytes)
        {
            // 1024 * 1024 megabytes in a terabyte
            return terabytes * (1024.0 * 1024.0);
        }

        /// <summary>
        /// Convert Terabytes To Gigabytes
        /// </summary>
        /// <param name="terabytes"></param>
        /// <returns></returns>
        public double TB_To_GB(double terabytes)
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
        public string VarType(object Object, bool DisplayVarType = false)
        {
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



        #endregion


        #region #### Script Properties ###############################

        /// <summary>
        /// Directory path for the current application's exe. Used to find/create subfolders under your project
        /// </summary>
        /// <param name="openDir">Option to Open AppDir Folder (Default = False)</param>
        /// <returns>Returns path to current application's exe</returns>
        public string AppDir(bool openDir = false)  // returns the application directory for the application currently executing
        {
            string dir = System.IO.Path.GetDirectoryName(Application.ExecutablePath);

            if (openDir) { OpenDir(dir); }

            return dir;
        }

        /// <summary>Returns the app name from the assembly info of application running</summary>
        public string AppName()
        {
            return Assembly.GetCallingAssembly().GetName().Name.ToString();
        }

        /// <summary>
        /// Returns path to user's desktop directory
        /// </summary>
        /// <param name="openDir">Option to Open Desktop Directory (Default = False - Only Returns Path)</param>
        /// <returns>Returns Directory Path to User's Desktop</returns>
        public string DesktopDir(bool openDir = false)
        {
            string desktop = Environment.GetFolderPath(Environment.SpecialFolder.Desktop);

            if (openDir) { OpenDir(desktop); }

            return desktop;
        }

        /// <summary>
        /// Returns path to user's startup directory
        /// </summary>
        /// <param name="openDir">Option to Open User's StartUp Dir (Default = False - Only Returns Path)</param>
        /// <returns>Returns path to user's startup dir</returns>
        public string StartUpDir(bool openDir = false)
        {
            //startUpFolderPath
            string startDir = Environment.GetFolderPath(Environment.SpecialFolder.Startup);

            if (openDir) { OpenDir(startDir); }

            return startDir;
        }

        /// <summary>Returns user's computer name</summary>
        public string ComputerName()
        {
            return ahkReturn("OutputVar = %A_ComputerName%");
        }

        /// <summary>Returns user's Windows User name</summary>
        public string UserName()
        {
            return ahkReturn("OutputVar = %A_UserName%");
        }

        /// <summary>The script's current working directory, which is where files will be accessed by default. The final backslash is not included unless it is the root directory. Two examples: C:\ and C:\My Documents. Use SetWorkingDir to change the working directory.</summary>
        public string A_WorkingDir(bool openDir = false)
        {
            string dir = ahkReturn("OutputVar = %A_WorkingDir%");

            if (openDir) { OpenDir(dir); }

            return dir;
        }

        /// <summary>The full path of the directory where the current script is located. For backward compatibility with AutoIt v2, the final backslash is included only for .aut scripts (even for root directories).</summary>
        public string A_ScriptDir(bool openDir = false)
        {
            string dir = ahkReturn("OutputVar = %A_ScriptDir%");

            if (openDir) { OpenDir(dir); }

            return dir;
        }
        /// <summary>The file name of the current script, without its path, e.g. MyScript.ahk.</summary>
        public string A_ScriptName()
        {
            return ahkReturn("OutputVar = %A_ScriptName%");
        }
        /// <summary>The combination of A_ScriptDir and A_ScriptName - gives the complete file specification of the script, e.g. C:\My Documents\My Script.ahk</summary>
        public string A_ScriptFullPath()
        {
            return ahkReturn("OutputVar = %A_ScriptFullPath%");
        }
        /// <summary>The number of the currently executing line within the script (or one of its #Include files). This line number will match the one shown by ListLines; it can be useful for error reporting such as this example: MsgBox Could not write to log file (line number %A_LineNumber%).</summary>
        public int A_LineNumber()
        {
            return ahkReturnInt("OutputVar = %A_LineNumber%");
        }
        /// <summary>The full path and name of the file to which A_LineNumber belongs, which will be the same as A_ScriptFullPath unless the line belongs to one of a non-compiled script's #Include files.</summary>
        public string A_LineFile()
        {
            return ahkReturn("OutputVar = %A_LineFile%");
        }
        /// <summary>The name of the user-defined function that is currently executing (blank if none); for example: MyFunction.</summary>
        public string A_ThisFunc()
        {
            return ahkReturn("OutputVar = %A_ThisFunc%");
        }
        /// <summary>The name of the label (subroutine) that is currently executing (blank if none); for example: MyLabel. It is updated whenever the script executes Gosub/Return or Goto. It is also updated for automatically-called labels such as timers, GUI threads, menu items, hotkeys, hotstrings, OnClipboardChange, and OnExit, However, A_ThisLabel is not updated when execution "falls into" a label from above; when that happens, A_ThisLabel retains its previous value. See also: A_ThisHotkey and IsLabel()</summary>
        public string A_ThisLabel()
        {
            return ahkReturn("OutputVar = %A_ThisLabel%");
        }
        /// <summary>In versions prior to 1.0.22, this variable is blank. Otherwise, it contains the version of AutoHotkey that is running the script, such as 1.0.22. In the case of a compiled script, the version that was originally used to compile it is reported. The formatting of the version number allows a script to check whether A_AhkVersion is greater than some minimum version number with > or >= as in this example: if A_AhkVersion >= 1.0.25.07</summary>
        public string A_AhkVersion()
        {
            return ahkReturn("OutputVar = %A_AhkVersion%");
        }
        /// <summary>For non-compiled scripts: The full path and name of the EXE file that is actually running the current script. For example: C:\Program Files\AutoHotkey\AutoHotkey.exe</summary>
        public string A_AhkPath()
        {
            return ahkReturn("OutputVar = %A_AhkPath%");
        }
        /// <summary>Contains 1 if the script is running as a compiled EXE and nothing if it is not.</summary>
        public bool A_IsCompiled()
        {
            return ahkReturnBool("OutputVar = %A_IsCompiled%");
        }
        /// <summary>The most recent reason the script was asked to terminate. This variable is blank unless the script has an OnExit subroutine and that subroutine is currently running or has been called at least once by an exit attempt. See OnExit for details.</summary>
        public string A_ExitReason()
        {
            return ahkReturn("OutputVar = %A_ExitReason%");
        }

        /// <summary>
        /// Exit Application
        /// </summary>
        public void ExitApp()
        {
            Application.Exit();
        }


        #endregion


        #region #### Date and Time ###############################

        /// <summary>Current 4-digit year (e.g. 2004). Synonymous with A_Year.</summary>
        public int A_YYYY()
        {
            return ahkReturnInt("OutputVar = %A_YYYY%");
        }
        /// <summary>Current 2-digit month (01-12). Synonymous with A_Mon.</summary>
        public int A_MM()
        {
            return ahkReturnInt("OutputVar = %A_MM%");
        }
        /// <summary>Current 2-digit day of the month (01-31). Synonymous with A_MDay.</summary>
        public int A_DD()
        {
            return ahkReturnInt("OutputVar = %A_DD%");
        }
        /// <summary>Current month's full name in the current user's language, e.g. July</summary>
        public string A_MMMM()
        {
            return ahkReturn("OutputVar = %A_MMMM%");
        }
        /// <summary>Current month's abbreviation in the current user's language, e.g. Jul</summary>
        public string A_MMM()
        {
            return ahkReturn("OutputVar = %A_MMM%");
        }
        /// <summary>Current day of the week's full name in the current user's language, e.g. Sunday</summary>
        public string A_DDDD()
        {
            return ahkReturn("OutputVar = %A_DDDD%");
        }
        /// <summary>Current day of the week's 3-letter abbreviation in the current user's language, e.g. Sun</summary>
        public string A_DDD()
        {
            return ahkReturn("OutputVar = %A_DDD%");
        }
        /// <summary>Current 1-digit day of the week (1-7). 1 is Sunday in all locales.</summary>
        public int A_WDay()
        {
            return ahkReturnInt("OutputVar = %A_WDay%");
        }
        /// <summary>Current day of the year (1-366). The value is not zero-padded, e.g. 9 is retrieved, not 009.</summary>
        public int A_YDay()
        {
            return ahkReturnInt("OutputVar = %A_YDay%");
        }
        /// <summary>Current year and week number (e.g. 200453) according to ISO 8601.</summary>
        public int A_YWeek()
        {
            return ahkReturnInt("OutputVar = %A_YWeek%");
        }
        /// <summary>Current 2-digit hour (00-23) in 24-hour time (for example, 17 is 5pm).</summary>
        public string A_Hour()
        {
            return ahkReturn("OutputVar = %A_Hour%");
        }
        /// <summary>Current 2-digit minute (00-59).</summary>
        public int A_Min()
        {
            return ahkReturnInt("OutputVar = %A_Min%");
        }
        /// <summary>Current 2-digit second (00-59).</summary>
        public int A_Sec()
        {
            return ahkReturnInt("OutputVar = %A_Sec%");
        }
        /// <summary>Current 3-digit millisecond (000-999).</summary>
        public int A_MSec()
        {
            return ahkReturnInt("OutputVar = %A_MSec%");
        }
        /// <summary>The current local time in YYYYMMDDHH24MISS format.</summary>
        public string A_Now()
        {
            return ahkReturn("OutputVar = %A_Now%");
        }
        /// <summary>The current Coordinated Universal Time (UTC) in YYYYMMDDHH24MISS format. UTC is essentially the same as Greenwich Mean Time (GMT).</summary>
        public string A_NowUTC()
        {
            return ahkReturn("OutputVar = %A_NowUTC%");
        }
        /// <summary>The Number of Milliseconds Since Computer Rebooted</summary>
        public int A_TickCount()
        {
            return ahkReturnInt("OutputVar = %A_TickCount%");
        }

        #endregion

        #region === AHK Return ===

        /// <summary>
        /// Execute AHK Command That Populates 'OutputVar', Returns Value From AHK Session
        /// </summary>
        /// <param name="ahkCommand">AutoHotkey Command to Execute</param>
        /// <param name="outVarName">Variable to return value from in AHK Session, Default = "OutputVar"</param>
        /// <returns>Returns Variable Value from AHK Session</returns>
        public string ahkReturn(string ahkCommand = "OutputVar = %A_TickCount%", string outVarName = "OutputVar")
        {
            if (ahkGlobal.ahkdll == null) { ahkGlobal.ahkdll = new AutoHotkey.Interop.AutoHotkeyEngine(); }
            var ahkdll = ahkGlobal.ahkdll;

            ErrorLog_Setup(false); // ErrorLevel Detection Disabled for this function in AHK 

            ahkdll.ExecRaw(ahkCommand);
            return ahkdll.GetVar(outVarName);
        }

        /// <summary>
        /// Returns AHK OutputVar Contents as Bool
        /// </summary>
        /// <param name="ahkCommand">AutoHotkey Command to Execute</param>
        /// <param name="outVarName">Variable to return value from in AHK Session, Default = "OutputVar"</param>
        /// <returns>Returns True/False From AHK Command as Bool</returns>
        public bool ahkReturnBool(string ahkCommand = "OutputVar = %A_IsSuspended%", string outVarName = "OutputVar")
        {
            string OutVar = ahkReturn(ahkCommand, outVarName);

            // convert return value to bool 
            if (OutVar != "1") { return true; }
            return false;
        }

        /// <summary>
        /// Returns AHK OutputVar Contents as Int
        /// </summary>
        /// <param name="ahkCommand">AutoHotkey Command to Execute</param>
        /// <param name="outVarName">Variable to return value from in AHK Session, Default = "OutputVar"</param>
        /// <returns>Returns Value from AHK Command, Converts to Integer</returns>
        public int ahkReturnInt(string ahkCommand = "OutputVar = %A_IsSuspended%", string outVarName = "OutputVar")
        {
            string OutVar = ahkReturn(ahkCommand, outVarName);

            // convert return value to int
            return ToInt(OutVar);
        }


        #endregion

        #region #### Script Settings ###############################

        /// <summary>Returns TRUE if script is suspended, otherwise FALSE</summary>
        public bool A_IsSuspended()
        {
            return ahkReturnBool("OutputVar = %A_IsSuspended%");
        }
        /// <summary>Returns TRUE if the thread immediately underneath the current thread is paused. Otherwise returns FALSE.</summary>
        public bool A_IsPaused()
        {
            return ahkReturnBool("OutputVar = %A_IsPaused%");
        }
        /// <summary>Returns TRUE if the thread immediately underneath the current thread is paused. Otherwise returns FALSE.</summary>
        public bool A_IsCritical()
        {
            return ahkReturnBool("OutputVar = %A_IsCritical%");
        }
        /// <summary>Synonymous with A_NumBatchLines - The current value as set by SetBatchLines. Examples: 200 or 10ms (depending on format)</summary>
        public string A_BatchLines()
        {
            return ahkReturn("OutputVar = %A_BatchLines%");
        }
        /// <summary>The current mode set by SetTitleMatchMode: 1, 2, 3, or RegEx.</summary>
        public string A_TitleMatchMode()
        {
            return ahkReturn("OutputVar = %A_TitleMatchMode%");
        }
        /// <summary>The current match speed (fast or slow) set by SetTitleMatchMode.</summary>
        public string A_TitleMatchModeSpeed()
        {
            return ahkReturn("OutputVar = %A_TitleMatchModeSpeed%");
        }
        /// <summary>The current mode (On or Off) set by DetectHiddenWindows.</summary>
        public bool A_DetectHiddenWindows()
        {
            string returnVal = ahkReturn("OutputVar = %A_DetectHiddenWindows%");
            if (returnVal == "On") { return true; }
            return false;
            //return ahkReturn("OutputVar = %A_DetectHiddenWindows%");
        }
        /// <summary>The current mode (On or Off) set by DetectHiddenText.</summary>
        public bool A_DetectHiddenText()
        {
            string returnVal = ahkReturn("OutputVar = %A_DetectHiddenText%");
            if (returnVal == "On") { return true; }
            return false;
            //return ahkReturn("OutputVar = %A_DetectHiddenText%");
        }
        /// <summary>The current mode (On or Off) set by AutoTrim.</summary>
        public string A_AutoTrim()
        {
            return ahkReturn("OutputVar = %A_AutoTrim%");
        }
        /// <summary>The current mode (On, Off, or Locale) set by StringCaseSense.</summary>
        public string A_StringCaseSense()
        {
            return ahkReturn("OutputVar = %A_StringCaseSense%");
        }
        /// <summary>The current integer format (H or D) set by SetFormat.</summary>
        public string A_FormatInteger()
        {
            return ahkReturn("OutputVar = %A_FormatInteger%");
        }
        /// <summary>The current floating point number format set by SetFormat.</summary>
        public string A_FormatFloat()
        {
            return ahkReturn("OutputVar = %A_FormatFloat%");
        }
        /// <summary>The current delay set by SetKeyDelay (always decimal, not hex). This delay is for the traditional SendEvent mode, not SendPlay.</summary>
        public string A_KeyDelay()
        {
            return ahkReturn("OutputVar = %A_KeyDelay%");
        }
        /// <summary>The current delay set by SetWinDelay (always decimal, not hex).</summary>
        public string A_WinDelay()
        {
            return ahkReturn("OutputVar = %A_WinDelay%");
        }
        /// <summary>The current delay set by SetControlDelay (always decimal, not hex)</summary>
        public string A_ControlDelay()
        {
            return ahkReturn("OutputVar = %A_ControlDelay%");
        }
        /// <summary>The current delay set by SetMouseDelay (always decimal, not hex). This delay is for the traditional SendEvent mode, not SendPlay.</summary>
        public string A_MouseDelay()
        {
            return ahkReturn("OutputVar = %A_MouseDelay%");
        }
        /// <summary>The current speed set by SetDefaultMouseSpeed (always decimal, not hex).</summary>
        public string A_DefaultMouseSpeed()
        {
            return ahkReturn("OutputVar = %A_DefaultMouseSpeed%");
        }
        /// <summary>Contains 1 if the tray icon is currently hidden or 0 otherwise. The icon can be hidden via #NoTrayIcon or the Menu command.</summary>
        public string A_IconHidden()
        {
            return ahkReturn("OutputVar = %A_IconHidden%");
        }
        /// <summary>Blank unless a custom tooltip for the tray icon has been specified via Menu, Tray, Tip -- in which case it's the text of the tip.</summary>
        public string A_IconTip()
        {
            return ahkReturn("OutputVar = %A_IconTip%");
        }
        /// <summary>Blank unless a custom tray icon has been specified via Menu, tray, icon -- in which case it's the full path and name of the icon's file.</summary>
        public string A_IconFile()
        {
            return ahkReturn("OutputVar = %A_IconFile%");
        }
        /// <summary>Blank if A_IconFile is blank. Otherwise, it's the number of the icon in A_IconFile (typically 1)</summary>
        public string A_IconNumber()
        {
            return ahkReturn("OutputVar = %A_IconNumber%");
        }

        #endregion

        #region #### User Idle Time ###############################

        /// <summary>The number of milliseconds that have elapsed since the system last received keyboard, mouse, or other input. This is useful for determining whether the user is away. This variable will be blank unless the operating system is Windows 2000, XP, or beyond. Physical input from the user as well as artificial input generated by any program or script (such as the Send or MouseMove commands) will reset this value back to zero. Since this value tends to increase by increments of 10, do not check whether it is equal to another value. Instead, check whether it is greater or less than another value. For example: IfGreater, A_TimeIdle, 600000, MsgBox, The last keyboard or mouse activity was at least 10 minutes ago.</summary>
        public string A_TimeIdle()
        {
            return ahkReturn("OutputVar = %A_TimeIdle%");
        }
        /// <summary>Similar to above but ignores artificial keystrokes and/or mouse clicks whenever the corresponding hook (keyboard or mouse) is installed; that is, it responds only to physical events. (This prevents simulated keystrokes and mouse clicks from falsely indicating that a user is present.) If neither hook is installed, this variable is equivalent to A_TimeIdle. If only one hook is installed, only its type of physical input affects A_TimeIdlePhysical (the other/non-installed hook's input, both physical and artificial, has no effect)</summary>
        public string A_TimeIdlePhysical()
        {
            return ahkReturn("OutputVar = %A_TimeIdlePhysical%");
        }


        #endregion


        #region #### GUI Windows and Menu Bars ###############################


        /// <summary>The GUI window number that launched the current thread. This variable is blank unless a Gui control, menu bar item, or event such as GuiClose/GuiEscape launched the current thread.</summary>
        public string A_Gui()
        {
            string AHKLine = "OutputVar = %A_Gui%";  // ahk line to execute
            ErrorLog_Setup(false); // ErrorLevel Detection Disabled for this function in AHK 
            string OutVar = Execute(AHKLine, "OutputVar");   // execute AHK code and return variable value 
            return OutVar;
        }
        /// <summary>The name of the variable associated with the GUI control that launched the current thread. If that control lacks an associated variable, A_GuiControl instead contains the first 63 characters of the control's text/caption (this is most often used to avoid giving each button a variable name). A_GuiControl is blank whenever: 1) A_Gui is blank; 2) a GUI menu bar item or event such as GuiClose/GuiEscape launched the current thread; 3) the control lacks an associated variable and has no caption; or 4) The control that originally launched the current thread no longer exists (perhaps due to Gui Destroy).</summary>
        public string A_GuiControl()
        {
            string AHKLine = "OutputVar = %A_GuiControl%";  // ahk line to execute
            ErrorLog_Setup(false); // ErrorLevel Detection Disabled for this function in AHK 
            string OutVar = Execute(AHKLine, "OutputVar");   // execute AHK code and return variable value 
            return OutVar;
        }
        /// <summary>Contain the GUI window's width when referenced in a GuiSize subroutine. They apply to the window's client area, which is the area excluding title bar, menu bar, and borders.</summary>
        public string A_GuiWidth()
        {
            string AHKLine = "OutputVar = %A_GuiWidth%";  // ahk line to execute
            ErrorLog_Setup(false); // ErrorLevel Detection Disabled for this function in AHK 
            string OutVar = Execute(AHKLine, "OutputVar");   // execute AHK code and return variable value 
            return OutVar;
        }
        /// <summary>Contain the GUI window's height when referenced in a GuiSize subroutine. They apply to the window's client area, which is the area excluding title bar, menu bar, and borders.</summary>
        public string A_GuiHeight()
        {
            string AHKLine = "OutputVar = %A_GuiHeight%";  // ahk line to execute
            ErrorLog_Setup(false); // ErrorLevel Detection Disabled for this function in AHK 
            string OutVar = Execute(AHKLine, "OutputVar");   // execute AHK code and return variable value 
            return OutVar;
        }
        /// <summary>Contains the X coordinate for GuiContextMenu and GuiDropFiles events. Coordinates are relative to the upper-left corner of the window.</summary>
        public string A_GuiX()
        {
            string AHKLine = "OutputVar = %A_GuiX%";  // ahk line to execute
            ErrorLog_Setup(false); // ErrorLevel Detection Disabled for this function in AHK 
            string OutVar = Execute(AHKLine, "OutputVar");   // execute AHK code and return variable value 
            return OutVar;
        }
        /// <summary>Contains the Y coordinate for GuiContextMenu and GuiDropFiles events. Coordinates are relative to the upper-left corner of the window.</summary>
        public string A_GuiY()
        {
            string AHKLine = "OutputVar = %A_GuiY%";  // ahk line to execute
            ErrorLog_Setup(false); // ErrorLevel Detection Disabled for this function in AHK 
            string OutVar = Execute(AHKLine, "OutputVar");   // execute AHK code and return variable value 
            return OutVar;
        }
        /// <summary>The type of event that launched the current thread. If the thread was not launched via GUI action, this variable is blank.</summary>
        public string A_GuiEvent()
        {
            string AHKLine = "OutputVar = %A_GuiEvent%";  // ahk line to execute
            ErrorLog_Setup(false); // ErrorLevel Detection Disabled for this function in AHK 
            string OutVar = Execute(AHKLine, "OutputVar");   // execute AHK code and return variable value 
            return OutVar;
        }
        /// <summary>Contains additional information about the following events: The OnClipboardChange label | Mouse wheel hotkeys (WheelDown/Up/Left/Right)  |  RegisterCallback() | GUI events, namely GuiContextMenu, GuiDropFiles, ListBox, ListView, TreeView, and StatusBar. If there is no additional information for an event, A_EventInfo contains 0.</summary>
        public string A_EventInfo()
        {
            string AHKLine = "OutputVar = %A_EventInfo%";  // ahk line to execute
            ErrorLog_Setup(false); // ErrorLevel Detection Disabled for this function in AHK 
            string OutVar = Execute(AHKLine, "OutputVar");   // execute AHK code and return variable value 
            return OutVar;
        }


        #endregion


        #region #### Hotkeys, Hotstrings, and Custom Menu Items ###############################

        /// <summary>The name of the most recently selected custom menu item (blank if none).</summary>
        public string A_ThisMenuItem()
        {
            string AHKLine = "OutputVar := A_ThisMenuItem";  // ahk line to execute
            ErrorLog_Setup(false); // ErrorLevel Detection Disabled for this function in AHK 
            string OutVar = Execute(AHKLine, "OutputVar");   // execute AHK code and return variable value 
            return OutVar;
        }
        /// <summary>The name of the menu from which A_ThisMenuItem was selected.</summary>
        public string A_ThisMenu()
        {
            string AHKLine = "OutputVar := A_ThisMenu";  // ahk line to execute
            ErrorLog_Setup(false); // ErrorLevel Detection Disabled for this function in AHK 
            string OutVar = Execute(AHKLine, "OutputVar");   // execute AHK code and return variable value 
            return OutVar;
        }
        /// <summary>A number indicating the current position of A_ThisMenuItem within A_ThisMenu. The first item in the menu is 1, the second is 2, and so on. Menu separator lines are counted. This variable is blank if A_ThisMenuItem is blank or no longer exists within A_ThisMenu. It is also blank if A_ThisMenu itself no longer exists.</summary>
        public string A_ThisMenuItemPos()
        {
            string AHKLine = "OutputVar := A_ThisMenuItemPos";  // ahk line to execute
            ErrorLog_Setup(false); // ErrorLevel Detection Disabled for this function in AHK 
            string OutVar = Execute(AHKLine, "OutputVar");   // execute AHK code and return variable value 
            return OutVar;
        }
        /// <summary>The key name of the most recently executed hotkey or non-auto-replace hotstring (blank if none), e.g. #z. This value will change if the current thread is interrupted by another hotkey, so be sure to copy it into another variable immediately if you need the original value for later use in a subroutine.</summary>
        public string A_ThisHotkey()
        {
            string AHKLine = "OutputVar := A_ThisHotkey";  // ahk line to execute
            ErrorLog_Setup(false); // ErrorLevel Detection Disabled for this function in AHK 
            string OutVar = Execute(AHKLine, "OutputVar");   // execute AHK code and return variable value 
            return OutVar;
        }
        /// <summary>Same as A_ThisHotkey except for the previous hotkey. It will be blank if none.</summary>
        public string A_PriorHotkey()
        {
            string AHKLine = "OutputVar := A_PriorHotkey";  // ahk line to execute
            ErrorLog_Setup(false); // ErrorLevel Detection Disabled for this function in AHK 
            string OutVar = Execute(AHKLine, "OutputVar");   // execute AHK code and return variable value 
            return OutVar;
        }
        /// <summary>The number of milliseconds that have elapsed since A_ThisHotkey was pressed. It will be -1 whenever A_ThisHotkey is blank.</summary>
        public string A_TimeSinceThisHotkey()
        {
            string AHKLine = "OutputVar := A_TimeSinceThisHotkey";  // ahk line to execute
            ErrorLog_Setup(false); // ErrorLevel Detection Disabled for this function in AHK 
            string OutVar = Execute(AHKLine, "OutputVar");   // execute AHK code and return variable value 
            return OutVar;
        }
        /// <summary>The number of milliseconds that have elapsed since A_PriorHotkey was pressed. It will be -1 whenever A_PriorHotkey is blank.</summary>
        public string A_TimeSincePriorHotkey()
        {
            string AHKLine = "OutputVar := A_TimeSincePriorHotkey";  // ahk line to execute
            ErrorLog_Setup(false); // ErrorLevel Detection Disabled for this function in AHK 
            string OutVar = Execute(AHKLine, "OutputVar");   // execute AHK code and return variable value 
            return OutVar;
        }
        /// <summary>The ending character that was pressed by the user to trigger the most recent non-auto-replace hotstring. If no ending character was required (due to the * option), this variable will be blank.</summary>
        public string A_EndChar()
        {
            string AHKLine = "OutputVar := A_EndChar";  // ahk line to execute
            ErrorLog_Setup(false); // ErrorLevel Detection Disabled for this function in AHK 
            string OutVar = Execute(AHKLine, "OutputVar");   // execute AHK code and return variable value 
            return OutVar;
        }


        #endregion


        #region #### Operating System and User Info ###############################

        /// <summary>Contains the same string as the environment's ComSpec variable (e.g. C:\Windows\system32\cmd.exe). Often used with Run/RunWait.</summary>
        public string ComSpec()
        {
            if (ahkGlobal.ahkdll == null) { ahkGlobal.ahkdll = new AutoHotkey.Interop.AutoHotkeyEngine(); }
            var ahkdll = ahkGlobal.ahkdll;

            ErrorLog_Setup(false); // ErrorLevel Detection Disabled for this function in AHK 

            string Command = "OutputVar = %ComSpec%";  // ahk line to execute
            ahkdll.ExecRaw(Command);
            return ahkdll.GetVar("OutputVar");
        }
        /// <summary>The full path and name of the folder designated to hold temporary files (e.g. C:\DOCUME~1\UserName\LOCALS~1\Temp). It is retrieved from one of the following locations (in order): 1) the environment variables TMP, TEMP, or USERPROFILE; 2) the Windows directory. On Windows 9x, A_WorkingDir is returned if neither TMP nor TEMP exists.</summary>
        public string A_Temp(bool openDir = false)
        {
            if (ahkGlobal.ahkdll == null) { ahkGlobal.ahkdll = new AutoHotkey.Interop.AutoHotkeyEngine(); }
            var ahkdll = ahkGlobal.ahkdll;

            ErrorLog_Setup(false); // ErrorLevel Detection Disabled for this function in AHK 

            string Command = "OutputVar = %A_Temp%";  // ahk line to execute
            ahkdll.ExecRaw(Command);
            string OutVar = ahkdll.GetVar("OutputVar");

            if (openDir) { OpenDir(OutVar); } // Option to Open Folder
            return OutVar;
        }
        /// <summary>The type of Operating System being run.  Either WIN32_WINDOWS (i.e. Windows 95/98/ME) or WIN32_NT (i.e. Windows NT4/2000/XP/2003/Vista).</summary>
        public string A_OSType()
        {
            if (ahkGlobal.ahkdll == null) { ahkGlobal.ahkdll = new AutoHotkey.Interop.AutoHotkeyEngine(); }
            var ahkdll = ahkGlobal.ahkdll;

            ErrorLog_Setup(false); // ErrorLevel Detection Disabled for this function in AHK 

            string Command = "OutputVar = %A_OSType%";  // ahk line to execute
            ahkdll.ExecRaw(Command);
            return ahkdll.GetVar("OutputVar");
        }
        /// <summary>One of the following strings: WIN_VISTA [requires v1.0.44.13+], WIN_2003, WIN_XP, WIN_2000, WIN_NT4, WIN_95, WIN_98, WIN_ME.</summary>
        public string A_OSVersion()
        {
            if (ahkGlobal.ahkdll == null) { ahkGlobal.ahkdll = new AutoHotkey.Interop.AutoHotkeyEngine(); }
            var ahkdll = ahkGlobal.ahkdll;

            ErrorLog_Setup(false); // ErrorLevel Detection Disabled for this function in AHK 

            string Command = "OutputVar = %A_OSVersion%";  // ahk line to execute
            ahkdll.ExecRaw(Command);
            return ahkdll.GetVar("OutputVar");
        }
        /// <summary>The system's default language, which is one of these 4-digit codes.</summary>
        public string A_Language()
        {
            if (ahkGlobal.ahkdll == null) { ahkGlobal.ahkdll = new AutoHotkey.Interop.AutoHotkeyEngine(); }
            var ahkdll = ahkGlobal.ahkdll;

            ErrorLog_Setup(false); // ErrorLevel Detection Disabled for this function in AHK 

            string Command = "OutputVar = %A_Language%";  // ahk line to execute
            ahkdll.ExecRaw(Command);
            return ahkdll.GetVar("OutputVar");
        }
        /// <summary>The name of the computer as seen on the network.</summary>
        public string A_ComputerName()
        {
            if (ahkGlobal.ahkdll == null) { ahkGlobal.ahkdll = new AutoHotkey.Interop.AutoHotkeyEngine(); }
            var ahkdll = ahkGlobal.ahkdll;

            ErrorLog_Setup(false); // ErrorLevel Detection Disabled for this function in AHK 

            string Command = "OutputVar = %A_ComputerName%";  // ahk line to execute
            ahkdll.ExecRaw(Command);
            return ahkdll.GetVar("OutputVar");
        }
        /// <summary>The logon name of the user who launched this script.</summary>
        public string A_UserName()
        {
            if (ahkGlobal.ahkdll == null) { ahkGlobal.ahkdll = new AutoHotkey.Interop.AutoHotkeyEngine(); }
            var ahkdll = ahkGlobal.ahkdll;

            ErrorLog_Setup(false); // ErrorLevel Detection Disabled for this function in AHK 

            string Command = "OutputVar = %A_UserName%";  // ahk line to execute
            ahkdll.ExecRaw(Command);
            return ahkdll.GetVar("OutputVar");
        }
        /// <summary>The Windows directory. For example: C:\Windows</summary>
        public string A_WinDir(bool openDir = false)
        {
            if (ahkGlobal.ahkdll == null) { ahkGlobal.ahkdll = new AutoHotkey.Interop.AutoHotkeyEngine(); }
            var ahkdll = ahkGlobal.ahkdll;

            ErrorLog_Setup(false); // ErrorLevel Detection Disabled for this function in AHK 

            string Command = "OutputVar = %A_WinDir%";  // ahk line to execute
            ahkdll.ExecRaw(Command);
            string OutVar = ahkdll.GetVar("OutputVar");

            if (openDir) { OpenDir(OutVar); } // Option to Open Folder
            return OutVar;
        }
        /// <summary>The Program Files directory (e.g. C:\Program Files).</summary>
        public string A_ProgramFiles(bool openDir = false)
        {
            if (ahkGlobal.ahkdll == null) { ahkGlobal.ahkdll = new AutoHotkey.Interop.AutoHotkeyEngine(); }
            var ahkdll = ahkGlobal.ahkdll;

            ErrorLog_Setup(false); // ErrorLevel Detection Disabled for this function in AHK 

            string Command = "OutputVar = %A_ProgramFiles%";  // ahk line to execute
            ahkdll.ExecRaw(Command);
            string OutVar = ahkdll.GetVar("OutputVar");

            if (openDir) { OpenDir(OutVar); } // Option to Open Folder
            return OutVar;
        }
        /// <summary>The full path and name of the folder containing the current user's application-specific data. For example: C:\Documents and Settings\Username\Application Data</summary>
        public string A_AppData(bool openDir = false)
        {
            if (ahkGlobal.ahkdll == null) { ahkGlobal.ahkdll = new AutoHotkey.Interop.AutoHotkeyEngine(); }
            var ahkdll = ahkGlobal.ahkdll;

            ErrorLog_Setup(false); // ErrorLevel Detection Disabled for this function in AHK 

            string Command = "OutputVar = %A_AppData%";  // ahk line to execute
            ahkdll.ExecRaw(Command);
            string OutVar = ahkdll.GetVar("OutputVar");

            if (openDir) { OpenDir(OutVar); } // Option to Open Folder
            return OutVar;
        }
        /// <summary>The full path and name of the folder containing the all-users application-specific data.</summary>
        public string A_AppDataCommon(bool openDir = false)
        {
            if (ahkGlobal.ahkdll == null) { ahkGlobal.ahkdll = new AutoHotkey.Interop.AutoHotkeyEngine(); }
            var ahkdll = ahkGlobal.ahkdll;

            ErrorLog_Setup(false); // ErrorLevel Detection Disabled for this function in AHK 

            string Command = "OutputVar = %A_AppDataCommon%";  // ahk line to execute
            ahkdll.ExecRaw(Command);
            string OutVar = ahkdll.GetVar("OutputVar");

            if (openDir) { OpenDir(OutVar); } // Option to Open Folder
            return OutVar;
        }
        /// <summary>A_Desktop The full path and name of the folder containing the current user's desktop files.</summary>
        public string A_Desktop(bool openDir = false)
        {
            if (ahkGlobal.ahkdll == null) { ahkGlobal.ahkdll = new AutoHotkey.Interop.AutoHotkeyEngine(); }
            var ahkdll = ahkGlobal.ahkdll;

            ErrorLog_Setup(false); // ErrorLevel Detection Disabled for this function in AHK 

            string Command = "OutputVar = %A_Desktop%";  // ahk line to execute
            ahkdll.ExecRaw(Command);
            string OutVar = ahkdll.GetVar("OutputVar");

            if (openDir) { OpenDir(OutVar); } // Option to Open Folder
            return OutVar;
        }
        /// <summary>The full path and name of the folder containing the all-users desktop files.</summary>
        public string A_DesktopCommon(bool openDir = false)
        {
            if (ahkGlobal.ahkdll == null) { ahkGlobal.ahkdll = new AutoHotkey.Interop.AutoHotkeyEngine(); }
            var ahkdll = ahkGlobal.ahkdll;

            ErrorLog_Setup(false); // ErrorLevel Detection Disabled for this function in AHK 

            string Command = "OutputVar = %A_DesktopCommon%";  // ahk line to execute
            ahkdll.ExecRaw(Command);
            string OutVar = ahkdll.GetVar("OutputVar");

            if (openDir) { OpenDir(OutVar); } // Option to Open Folder
            return OutVar;
        }
        /// <summary>The full path and name of the current user's Start Menu folder.</summary>
        public string A_StartMenu(bool openDir = false)
        {
            if (ahkGlobal.ahkdll == null) { ahkGlobal.ahkdll = new AutoHotkey.Interop.AutoHotkeyEngine(); }
            var ahkdll = ahkGlobal.ahkdll;

            ErrorLog_Setup(false); // ErrorLevel Detection Disabled for this function in AHK 

            string Command = "OutputVar = %A_StartMenu%";  // ahk line to execute
            ahkdll.ExecRaw(Command);
            string OutVar = ahkdll.GetVar("OutputVar");

            if (openDir) { OpenDir(OutVar); } // Option to Open Folder
            return OutVar;
        }
        /// <summary>The full path and name of the all-users Start Menu folder.</summary>
        public string A_StartMenuCommon(bool openDir = false)
        {
            if (ahkGlobal.ahkdll == null) { ahkGlobal.ahkdll = new AutoHotkey.Interop.AutoHotkeyEngine(); }
            var ahkdll = ahkGlobal.ahkdll;

            ErrorLog_Setup(false); // ErrorLevel Detection Disabled for this function in AHK 

            string Command = "OutputVar = %A_StartMenuCommon%";  // ahk line to execute
            ahkdll.ExecRaw(Command);
            string OutVar = ahkdll.GetVar("OutputVar");

            if (openDir) { OpenDir(OutVar); } // Option to Open Folder
            return OutVar;
        }
        /// <summary>The full path and name of the Programs folder in the current user's Start Menu.</summary>
        public string A_Programs(bool openDir = false)
        {
            if (ahkGlobal.ahkdll == null) { ahkGlobal.ahkdll = new AutoHotkey.Interop.AutoHotkeyEngine(); }
            var ahkdll = ahkGlobal.ahkdll;

            ErrorLog_Setup(false); // ErrorLevel Detection Disabled for this function in AHK 

            string Command = "OutputVar = %A_Programs%";  // ahk line to execute
            ahkdll.ExecRaw(Command);
            string OutVar = ahkdll.GetVar("OutputVar");

            if (openDir) { OpenDir(OutVar); } // Option to Open Folder
            return OutVar;
        }
        /// <summary>The full path and name of the Programs folder in the all-users Start Menu.</summary>
        public string A_ProgramsCommon(bool openDir = false)
        {
            if (ahkGlobal.ahkdll == null) { ahkGlobal.ahkdll = new AutoHotkey.Interop.AutoHotkeyEngine(); }
            var ahkdll = ahkGlobal.ahkdll;

            ErrorLog_Setup(false); // ErrorLevel Detection Disabled for this function in AHK 

            string Command = "OutputVar = %A_ProgramsCommon%";  // ahk line to execute
            ahkdll.ExecRaw(Command);
            string OutVar = ahkdll.GetVar("OutputVar");

            if (openDir) { OpenDir(OutVar); } // Option to Open Folder
            return OutVar;
        }
        /// <summary>The full path and name of the Startup folder in the current user's Start Menu.</summary>
        public string A_Startup(bool openDir = false)
        {
            if (ahkGlobal.ahkdll == null) { ahkGlobal.ahkdll = new AutoHotkey.Interop.AutoHotkeyEngine(); }
            var ahkdll = ahkGlobal.ahkdll;

            ErrorLog_Setup(false); // ErrorLevel Detection Disabled for this function in AHK 

            string Command = "OutputVar = %A_Startup%";  // ahk line to execute
            ahkdll.ExecRaw(Command);
            string OutVar = ahkdll.GetVar("OutputVar");

            if (openDir) { OpenDir(OutVar); } // Option to Open Folder
            return OutVar;
        }
        /// <summary>The full path and name of the Startup folder in the all-users Start Menu.</summary>
        public string A_StartupCommon(bool openDir = false)
        {
            if (ahkGlobal.ahkdll == null) { ahkGlobal.ahkdll = new AutoHotkey.Interop.AutoHotkeyEngine(); }
            var ahkdll = ahkGlobal.ahkdll;

            ErrorLog_Setup(false); // ErrorLevel Detection Disabled for this function in AHK 

            string Command = "OutputVar = %A_StartupCommon%";  // ahk line to execute
            ahkdll.ExecRaw(Command);
            string OutVar = ahkdll.GetVar("OutputVar");

            if (openDir) { OpenDir(OutVar); } // Option to Open Folder
            return OutVar;
        }
        /// <summary>The full path and name of the current user's "My Documents" folder. Unlike most of the similar variables, if the folder is the root of a drive, the final backslash is not included. For example, it would contain M: rather than M:\</summary>
        public string A_MyDocuments(bool openDir = false)
        {
            if (ahkGlobal.ahkdll == null) { ahkGlobal.ahkdll = new AutoHotkey.Interop.AutoHotkeyEngine(); }
            var ahkdll = ahkGlobal.ahkdll;

            ErrorLog_Setup(false); // ErrorLevel Detection Disabled for this function in AHK 

            string Command = "OutputVar = %A_MyDocuments%";  // ahk line to execute
            ahkdll.ExecRaw(Command);
            string OutVar = ahkdll.GetVar("OutputVar");

            if (openDir) { OpenDir(OutVar); } // Option to Open Folder
            return OutVar;
        }
        /// <summary>If the current user has admin rights, returns TRUE. Otherwise, returns FALSE. Under Windows 95/98/Me, this variable always returns True</summary>
        public bool A_IsAdmin()
        {
            if (ahkGlobal.ahkdll == null) { ahkGlobal.ahkdll = new AutoHotkey.Interop.AutoHotkeyEngine(); }
            var ahkdll = ahkGlobal.ahkdll;

            ErrorLog_Setup(false); // ErrorLevel Detection Disabled for this function in AHK 

            string Command = "OutputVar = %A_IsAdmin%";  // ahk line to execute
            ahkdll.ExecRaw(Command);

            string OutVar = ahkdll.GetVar("OutputVar");
            if (OutVar == "1") { return true; }
            return false;
        }

        /// <summary>Returns Width of Primary Monitor in Pixels (e.g. 1024 and 768).</summary>
        public int ScreenWidth()
        {
            int outV = ToInt(A_ScreenWidth());
            return outV;
        }

        /// <summary>Returns Height of Primary Monitor, in Pixels (e.g. 1024 and 768)</summary>
        /// <returns>Returns Width as Int</returns>
        public int ScreenHeight()
        {
            int outV = ToInt(A_ScreenHeight());
            return outV;
        }

        /// <summary>Returns Width of Primary Monitor in Pixels (e.g. 1024 and 768)</summary>
        /// <returns>Returns Width as String</returns>
        public string A_ScreenWidth()
        {
            if (ahkGlobal.ahkdll == null) { ahkGlobal.ahkdll = new AutoHotkey.Interop.AutoHotkeyEngine(); }
            var ahkdll = ahkGlobal.ahkdll;

            ErrorLog_Setup(false); // ErrorLevel Detection Disabled for this function in AHK 

            string Command = "OutputVar = %A_ScreenWidth%";  // ahk line to execute
            ahkdll.ExecRaw(Command);
            return ahkdll.GetVar("OutputVar");
        }
        /// <summary>Returns Height of Primary Monitor, in Pixels (e.g. 1024 and 768)</summary>
        /// <returns>Returns Width as String</returns>
        public string A_ScreenHeight()
        {
            if (ahkGlobal.ahkdll == null) { ahkGlobal.ahkdll = new AutoHotkey.Interop.AutoHotkeyEngine(); }
            var ahkdll = ahkGlobal.ahkdll;

            ErrorLog_Setup(false); // ErrorLevel Detection Disabled for this function in AHK 

            string Command = "OutputVar = %A_ScreenHeight%";  // ahk line to execute
            ahkdll.ExecRaw(Command);
            return ahkdll.GetVar("OutputVar");
        }


        /// <summary>The IP addresses of the first 4 network adapters in the computer.</summary>
        public string A_IPAddress1()
        {
            if (ahkGlobal.ahkdll == null) { ahkGlobal.ahkdll = new AutoHotkey.Interop.AutoHotkeyEngine(); }
            var ahkdll = ahkGlobal.ahkdll;

            ErrorLog_Setup(false); // ErrorLevel Detection Disabled for this function in AHK 

            string Command = "OutputVar = %A_IPAddress1%";  // ahk line to execute
            ahkdll.ExecRaw(Command);
            return ahkdll.GetVar("OutputVar");
        }
        /// <summary>The IP addresses of the first 4 network adapters in the computer.</summary>
        public string A_IPAddress2()
        {
            if (ahkGlobal.ahkdll == null) { ahkGlobal.ahkdll = new AutoHotkey.Interop.AutoHotkeyEngine(); }
            var ahkdll = ahkGlobal.ahkdll;

            ErrorLog_Setup(false); // ErrorLevel Detection Disabled for this function in AHK 

            string Command = "OutputVar = %A_IPAddress2%";  // ahk line to execute
            ahkdll.ExecRaw(Command);
            return ahkdll.GetVar("OutputVar");
        }
        /// <summary>The IP addresses of the first 4 network adapters in the computer.</summary>
        public string A_IPAddress3()
        {
            if (ahkGlobal.ahkdll == null) { ahkGlobal.ahkdll = new AutoHotkey.Interop.AutoHotkeyEngine(); }
            var ahkdll = ahkGlobal.ahkdll;

            ErrorLog_Setup(false); // ErrorLevel Detection Disabled for this function in AHK 

            string Command = "OutputVar = %A_IPAddress3%";  // ahk line to execute
            ahkdll.ExecRaw(Command);
            return ahkdll.GetVar("OutputVar");
        }
        /// <summary>The IP addresses of the first 4 network adapters in the computer.</summary>
        public string A_IPAddress4()
        {
            if (ahkGlobal.ahkdll == null) { ahkGlobal.ahkdll = new AutoHotkey.Interop.AutoHotkeyEngine(); }
            var ahkdll = ahkGlobal.ahkdll;

            ErrorLog_Setup(false); // ErrorLevel Detection Disabled for this function in AHK 

            string Command = "OutputVar = %A_IPAddress4%";  // ahk line to execute
            ahkdll.ExecRaw(Command);
            return ahkdll.GetVar("OutputVar");
        }


        //To discover the dimensions of other monitors in a multi-monitor system, use SysGet.

        //To instead discover the width and height of the entire desktop (even if it spans multiple monitors), use the following example (but on Windows 95/NT, both of the below variables will be set to 0):
        //SysGet, VirtualWidth, 78
        //SysGet, VirtualHeight, 79

        //In addition, use SysGet to discover the work area of a monitor, which can be smaller than the monitor's total area because the taskbar and other registered desktop toolbars are excluded.

        #endregion


        #region #### Misc. ###############################

        /// <summary>This variable contains a single space character. See AutoTrim for details.</summary>
        public string A_Space()
        {
            if (ahkGlobal.ahkdll == null) { ahkGlobal.ahkdll = new AutoHotkey.Interop.AutoHotkeyEngine(); }
            var ahkdll = ahkGlobal.ahkdll;

            ErrorLog_Setup(false); // ErrorLevel Detection Disabled for this function in AHK 

            string Command = "OutputVar = %A_Space%";  // ahk line to execute
            ahkdll.ExecRaw(Command);
            return ahkdll.GetVar("OutputVar");
        }
        /// <summary>This variable contains a single tab character. See AutoTrim for details.</summary>
        public string A_Tab()
        {
            if (ahkGlobal.ahkdll == null) { ahkGlobal.ahkdll = new AutoHotkey.Interop.AutoHotkeyEngine(); }
            var ahkdll = ahkGlobal.ahkdll;

            ErrorLog_Setup(false); // ErrorLevel Detection Disabled for this function in AHK 

            string Command = "OutputVar = %A_Tab%";  // ahk line to execute
            ahkdll.ExecRaw(Command);
            return ahkdll.GetVar("OutputVar");
        }
        /// <summary>The type of mouse cursor currently being displayed. It will be one of the following words: AppStarting, Arrow, Cross, Help, IBeam, Icon, No, Size, SizeAll, SizeNESW, SizeNS, SizeNWSE, SizeWE, UpArrow, Wait, Unknown. The acronyms used with the size-type cursors are compass directions, e.g. NESW = NorthEast+SouthWest. The hand-shaped cursors (pointing and grabbing) are classified as Unknown.</summary>
        public string A_Cursor()
        {
            if (ahkGlobal.ahkdll == null) { ahkGlobal.ahkdll = new AutoHotkey.Interop.AutoHotkeyEngine(); }
            var ahkdll = ahkGlobal.ahkdll;

            ErrorLog_Setup(false); // ErrorLevel Detection Disabled for this function in AHK 

            string Command = "OutputVar = %A_Cursor%";  // ahk line to execute
            ahkdll.ExecRaw(Command);
            return ahkdll.GetVar("OutputVar");
        }
        /// <summary>The current X coordinate of the caret (text insertion point). The coordinates are relative to the active window unless CoordMode is used to make them relative to the entire screen. If there is no active window or the caret position cannot be determined, these variables are blank.</summary>
        public string A_CaretX()
        {
            if (ahkGlobal.ahkdll == null) { ahkGlobal.ahkdll = new AutoHotkey.Interop.AutoHotkeyEngine(); }
            var ahkdll = ahkGlobal.ahkdll;

            ErrorLog_Setup(false); // ErrorLevel Detection Disabled for this function in AHK 

            string Command = "OutputVar = %A_CaretX%";  // ahk line to execute
            ahkdll.ExecRaw(Command);
            return ahkdll.GetVar("OutputVar");
        }
        /// <summary>The current Y coordinate of the caret (text insertion point). The coordinates are relative to the active window unless CoordMode is used to make them relative to the entire screen. If there is no active window or the caret position cannot be determined, these variables are blank.</summary>
        public string A_CaretY()
        {
            if (ahkGlobal.ahkdll == null) { ahkGlobal.ahkdll = new AutoHotkey.Interop.AutoHotkeyEngine(); }
            var ahkdll = ahkGlobal.ahkdll;

            ErrorLog_Setup(false); // ErrorLevel Detection Disabled for this function in AHK 

            string Command = "OutputVar = %A_CaretY%";  // ahk line to execute
            ahkdll.ExecRaw(Command);
            return ahkdll.GetVar("OutputVar");
        }
        /// <summary>Value of Last Error Level Set by AHK</summary>
        public string ErrorLevel()
        {
            if (ahkGlobal.ahkdll == null) { ahkGlobal.ahkdll = new AutoHotkey.Interop.AutoHotkeyEngine(); }
            var ahkdll = ahkGlobal.ahkdll;

            ErrorLog_Setup(false); // ErrorLevel Detection Disabled for this function in AHK 

            string Command = "OutputVar = %ErrorLevel%";  // ahk line to execute
            ahkdll.ExecRaw(Command);
            return ahkdll.GetVar("OutputVar");
        }
        /// <summary>The result from the OS's GetLastError() function. For details, see DllCall() and Run/RunWait.</summary>
        public string A_LastError()
        {
            if (ahkGlobal.ahkdll == null) { ahkGlobal.ahkdll = new AutoHotkey.Interop.AutoHotkeyEngine(); }
            var ahkdll = ahkGlobal.ahkdll;

            ErrorLog_Setup(false); // ErrorLevel Detection Disabled for this function in AHK 

            string Command = "OutputVar = %A_LastError%";  // ahk line to execute
            ahkdll.ExecRaw(Command);
            return ahkdll.GetVar("OutputVar");
        }


        #endregion



    }
}
