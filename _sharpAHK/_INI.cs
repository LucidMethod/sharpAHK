using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace sharpAHK
{
    public partial class _AHK
    {
        #region === INI ===

        /// <summary>Deletes a value from a standard format .ini file.</summary>
        /// <param name="IniPath">The name of the .ini file, which is assumed to be in %A_WorkingDir% if an absolute path isn't specified.</param>
        /// <param name="Section">The section name in the .ini file, which is the heading phrase that appears in square brackets (do not include the brackets in this parameter). </param>
        /// <param name="Key">The key name in the .ini file. If omitted, the entire Section will be deleted.</param>
        public bool IniDelete(string IniPath, string Section, string Key = "")
        {
            string filename = IniPath.Replace(",", "`,");
            string section = Section.Replace(",", "`,");
            string key = Key.Replace(",", "`,");

            string AHKLine = "IniDelete, " + filename + "," + section + "," + key;  // ahk line to execute
            ErrorLog_Setup(true, "Error Deleting INI Key In: " + filename); // ErrorLevel Detection Enabled for this function in AHK 
            Execute(AHKLine);   // execute AHK code and return variable value

            if (!ahkGlobal.ErrorLevel) { return true; } // no error level - return true for success
            else { throw new System.InvalidOperationException(ahkGlobal.ErrorLevelMsg); }  // throw exception on AHK error
            //return false;  // error level detected - success = false
        }

        /// <summary>Reads a value from a standard format .ini file.</summary>
        /// <param name="IniPath">The name of the .ini file, which is assumed to be in %A_WorkingDir% if an absolute path isn't specified.</param>
        /// <param name="Section">The section name in the .ini file, which is the heading phrase that appears in square brackets (do not include the brackets in this parameter).</param>
        /// <param name="Key">The key name in the .ini file.</param>        
        /// <param name="Default">The value to store in OutputVar if the requested key is not found. If omitted, it defaults to the word ERROR. To store a blank value (empty string), specify %A_Space%.</param>
        public string IniRead(string IniPath, string Section, string Key, string Default = "")
        {
            string filename = IniPath.Replace(",", "`,");
            string section = Section.Replace(",", "`,");
            string key = Key.Replace(",", "`,");
            string DEfault = Default.Replace(",", "`,");

            //string fileName = StringReplace(Filename, "%", "`%", "ALL"); // fix illegal ahk chars

            string AHKLine = "IniRead, OutputVar, " + filename + "," + section + "," + key + "," + DEfault;  // ahk line to execute
            ErrorLog_Setup(true, "Error Reading INI: " + filename); // ErrorLevel Detection Enabled for this function in AHK 
            string OutVar = Execute(AHKLine, "OutputVar");   // execute AHK code and return variable value 

            if (ahkGlobal.ErrorLevel) { throw new System.InvalidOperationException(ahkGlobal.ErrorLevelMsg); }  // throw exception on AHK error
            return OutVar;
        }

        /// <summary>Writes a value to a standard format .ini file.</summary>
        /// <param name="Value">The string or number that will be written to the right of Key's equal sign (=). If the text is long, it can be broken up into several shorter lines by means of a continuation section, which might improve readability and maintainability.</param>
        /// <param name="IniPath">The name of the .ini file, which is assumed to be in %A_WorkingDir% if an absolute path isn't specified.</param>
        /// <param name="Section">The section name in the .ini file, which is the heading phrase that appears in square brackets (do not include the brackets in this parameter).</param>
        /// <param name="Key">The key name in the .ini file.</param>        
        public bool IniWrite(object Value, string IniPath, string Section, string Key)
        {
            if (Value == null) { Value = ""; }
            //throw new System.InvalidOperationException("Value is NULL");
            if (IniPath == null) { throw new System.InvalidOperationException("IniPath Cannot be Null"); }

            string filename = IniPath.Replace(",", "`,");
            string section = Section.Replace(",", "`,");
            string key = Key.Replace(",", "`,");
            string value = Value.ToString().Replace(",", "`,");

            string AHKLine = "IniWrite, " + value + "," + filename + "," + section + "," + key;  // ahk line to execute
            ErrorLog_Setup(true, "Error Deleting INI Key In: " + filename); // ErrorLevel Detection Enabled for this function in AHK 
            Execute(AHKLine);   // execute AHK code and return variable value

            if (!ahkGlobal.ErrorLevel) { return true; } // no error level - return true for success
            else { throw new System.InvalidOperationException(ahkGlobal.ErrorLevelMsg + " | " + ahkGlobal.ErrorLevelValue); }
            //return false;  // error level detected - success = false
        }

        /// <summary>
        /// Reads .URL File and Returns Web Address
        /// </summary>
        /// <param name="URLFile">FilePath to .URL File to Read</param>
        /// <returns>Returns Link Found in .URL File</returns>
        public string URL_Read(string URLFile)
        {
            string URL = IniRead(URLFile, "InternetShortcut", "URL");
            return URL;
        }

        /// <summary>
        /// Writes .URL File Link to WebSite
        /// </summary>
        /// <param name="linkName">Name of the Site to Save Link To</param>
        /// <param name="SaveDir">Directory to Save New URL File To</param>
        /// <param name="linkURL">WebSite Address to Write to new URL File</param>
        public void URL_Write(string linkName, string SaveDir, string linkURL) // write URL link file to local folder
        {
            IniWrite(linkURL, SaveDir + "\\" + linkName + ".url", "InternetShortcut", "URL");
        }


        #endregion
    }
}
