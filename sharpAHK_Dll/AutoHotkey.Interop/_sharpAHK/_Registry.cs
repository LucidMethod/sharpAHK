using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace sharpAHK
{
    public partial class _AHK
    {
        #region === Registry ===

        /// <summary>Deletes a subkey or value from the registry.</summary>
        /// <param name="RootKey">Must be either HKEY_LOCAL_MACHINE, HKEY_USERS, HKEY_CURRENT_USER, HKEY_CLASSES_ROOT, or HKEY_CURRENT_CONFIG (or the abbreviations for each of these, such as HKLM). To access a remote registry, prepend the computer name and a colon, as in this example: \\workstation01:HKEY_LOCAL_MACHINE</param>
        /// <param name="SubKey">The name of the subkey (e.g. Software\SomeApplication).</param>
        /// <param name="ValueName">The name of the value to delete. If omitted, the entire SubKey will be deleted. To delete Subkey's default value -- which is the value displayed as "(Default)" by RegEdit -- use the phrase AHK_DEFAULT for this parameter. </param>
        /// <returns>Returns True if No ErrorLevel</returns>
        public bool RegDelete(string RootKey, string SubKey, string ValueName = "")
        {
            ErrorLog_Setup(true);
            Execute("RegDelete, " + RootKey + "," + SubKey + "," + ValueName);

            if (!ahkGlobal.ErrorLevel) { return true; } // no error level - return true for success
            return false;  // error level detected - success = false
        }

        /// <summary>Reads a value from the registry.</summary>
        /// <param name="RootKey">Must be either HKEY_LOCAL_MACHINE, HKEY_USERS, HKEY_CURRENT_USER, HKEY_CLASSES_ROOT, or HKEY_CURRENT_CONFIG (or the abbreviations for each of these, such as HKLM). To access a remote registry, prepend the computer name and a colon, as in this example: \\workstation01:HKEY_LOCAL_MACHINE</param>
        /// <param name="SubKey">The name of the subkey (e.g. Software\SomeApplication).</param>
        /// <param name="ValueName">The name of the value to retrieve. If omitted, Subkey's default value will be retrieved, which is the value displayed as "(Default)" by RegEdit. If there is no default value (that is, if RegEdit displays "value not set"), OutputVar is made blank and ErrorLevel is set to 1.</param>
        public string RegRead(string RootKey, string SubKey, string ValueName = "")
        {
            string AHKLine = "RegRead, OutputVar, " + RootKey + "," + SubKey + "," + ValueName;  // ahk line to execute
            ErrorLog_Setup(true); // ErrorLevel Detection Enabled for this function in AHK 
            string OutVar = Execute(AHKLine, "OutputVar");   // execute AHK code and return variable value 
            return OutVar;
        }

        /// <summary>Writes a value to the registry.</summary>
        /// <param name="ValueType">Must be either REG_SZ, REG_EXPAND_SZ, REG_MULTI_SZ, REG_DWORD, or REG_BINARY. </param>
        /// <param name="RootKey">Must be either HKEY_LOCAL_MACHINE, HKEY_USERS, HKEY_CURRENT_USER, HKEY_CLASSES_ROOT, or HKEY_CURRENT_CONFIG (or the abbreviations for each of these, such as HKLM). To access a remote registry, prepend the computer name and a colon, as in this example: \\workstation01:HKEY_LOCAL_MACHINE</param>
        /// <param name="SubKey">The name of the subkey (e.g. Software\SomeApplication). If SubKey does not exist, it is created (along with its ancestors, if necessary). If SubKey is left blank, the value is written directly into RootKey (though some operating systems might refuse to write in HKEY_CURRENT_USER's top level).</param>
        /// <param name="ValueName">The name of the value that will be written to. If blank or omitted, Subkey's default value will be used, which is the value displayed as "(Default)" by RegEdit.</param>
        /// <param name="Value">The value to be written. If omitted, it will default to an empty (blank) string, or 0, depending on ValueType. If the text is long, it can be broken up into several shorter lines by means of a continuation section, which might improve readability and maintainability.</param>
        /// <returns>Returns True if No ErrorLevel</returns>
        public bool RegWrite(string ValueType, string RootKey, string SubKey, string ValueName = "", string Value = "")
        {
            string AHKLine = "RegWrite, " + ValueType + "," + RootKey + "," + SubKey + "," + ValueName + "," + Value;  // ahk line to execute
            ErrorLog_Setup(true); // ErrorLevel Detection Enabled for this function in AHK 
            Execute(AHKLine);   // execute AHK code and return variable value

            if (!ahkGlobal.ErrorLevel) { return true; } // no error level - return true for success
            return false;  // error level detected - success = false
        }


        #endregion
    }
}
