using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace sharpAHK
{
    public partial class _AHK
    {
        // #region === Drive ===

        /// <summary>
        /// Options for AHK Drive Command
        /// </summary>		
        public enum DriveOpts
        {
            Label,
            Lock,
            Unlock,
            Eject
        }

        /// <summary>
        /// Options for AHK DriveGet_List Command
        /// </summary>		
        public enum DriveType
        {
            ALL,
            CDROM,
            REMOVABLE,
            FIXED,
            NETWORK,
            RAMDISK,
            UNKNOWN
        }

        /// <summary>Ejects/retracts the tray in a CD or DVD drive, or sets a drive's volume label.</summary>
        /// <param name="SubCommand">Label|Lock|Unlock|Eject, Drive, 1</param>
        /// <param name="DriveLetter">Drive Letter To Act Upon | ex: D:</param>
        /// <param name="Value">1 to Retract/Close Tray</param>
        public bool Drive(DriveOpts SubCommand, string DriveLetter = "", string Value = "")
        {
            string cmd = "";
            if (SubCommand == DriveOpts.Label) { cmd = "Label"; }
            if (SubCommand == DriveOpts.Lock) { cmd = "Lock"; }
            if (SubCommand == DriveOpts.Unlock) { cmd = "Unlock"; }
            if (SubCommand == DriveOpts.Eject) { cmd = "Eject"; }

            string AHKLine = "Drive, " + cmd + ", " + DriveLetter + ", " + Value;  // ahk line to execute
            //MsgBox(AHKLine);

            ErrorLog_Setup(true); // ErrorLevel Detection Enabled for this function in AHK 
            Execute(AHKLine);   // execute AHK code and return variable value
            return ahkGlobal.ErrorLevel;
        }

        /// <summary>Ejects the tray in a CD or DVD drive</summary>
        /// <param name="DriveLetter">Drive Letter To Act Upon, Ex: D:</param>
        public bool Drive_Eject(string DriveLetter = "")
        {
            string AHKLine = "Drive, Eject," + DriveLetter + ", 0";  // ahk line to execute
            ErrorLog_Setup(true); // ErrorLevel Detection Enabled for this function in AHK 
            Execute(AHKLine);   // execute AHK code and return variable value
            return ahkGlobal.ErrorLevel;
        }

        /// <summary>Retracts the tray in a CD or DVD drive</summary>
        /// <param name="DriveLetter">Drive Letter To Act Upon, Ex: D:</param>
        public bool Drive_Close(string DriveLetter = "")
        {
            string AHKLine = "Drive, Eject," + DriveLetter + ", 1";  // ahk line to execute
            ErrorLog_Setup(true); // ErrorLevel Detection Enabled for this function in AHK 
            Execute(AHKLine);   // execute AHK code and return variable value
            return ahkGlobal.ErrorLevel;
        }

        /// <summary>Changes Drive's volume label to be NewLabel (if NewLabel is omitted, the drive will have no label). Drive is the drive letter followed by a colon and an optional backslash (might also work on UNCs and mapped drives). For example: Drive, Label, C:, Seagate200</summary>
        /// <param name="DriveLetter">Drive Letter To Act Upon, Ex: D:</param>
        /// <param name="NewLabel">New Drive Label - if Omitted, Drive With Have No Label</param>
        public bool Drive_Label(string DriveLetter, string NewLabel = "")
        {
            string AHKLine = "Drive, Label," + DriveLetter + ", " + NewLabel;  // ahk line to execute
            ErrorLog_Setup(true); // ErrorLevel Detection Enabled for this function in AHK 
            Execute(AHKLine);   // execute AHK code and return variable value
            return ahkGlobal.ErrorLevel;
        }

        /// <summary>Prevents a drive's eject feature from working. For example: "Drive, Lock, D:". Most drives cannot be "locked open". However, locking the drive while it is open will probably result in it becoming locked the moment it is closed. This command has no effect on drives that do not support locking (such as most read-only drives), nor is it likely to work on non-IDE drives on Windows 95/98/Me. If a drive is locked by a script and that script exits, the drive will stay locked until another script or program unlocks it, or the system is restarted. If the specified drive does not exist or does not support the locking feature, ErrorLevel is set to 1. Otherwise, it is set to 0.</summary>
        /// <param name="DriveLetter">Drive Letter To Act Upon, Ex: D:</param>
        public bool Drive_Lock(string DriveLetter)
        {
            string AHKLine = "Drive, Lock," + DriveLetter;  // ahk line to execute
            ErrorLog_Setup(true); // ErrorLevel Detection Enabled for this function in AHK 
            Execute(AHKLine);   // execute AHK code and return variable value
            return ahkGlobal.ErrorLevel;
        }

        /// <summary>Reverses the above. On Window NT/2000/XP or later, Unlock needs to be executed multiple times if the drive was locked multiple times (at least for some drives). For example, if "Drive, Lock, D:" was executed three times, three executions of "Drive, Unlock, D:" might be needed to unlock it. Because of this and the fact that there is no way to determine whether a drive is currently locked, it is often useful to keep track of its lock-state in a variable.</summary>
        /// <param name="DriveLetter">Drive Letter To Act Upon, Ex: D:</param>
        public bool Drive_Unlock(string DriveLetter)
        {
            string AHKLine = "Drive, Unlock," + DriveLetter;  // ahk line to execute
            ErrorLog_Setup(true); // ErrorLevel Detection Enabled for this function in AHK 
            Execute(AHKLine);   // execute AHK code and return variable value
            return ahkGlobal.ErrorLevel;
        }

        /// <summary>Retrieves the free disk space of a drive, in Megabytes.</summary>
        /// <param name="DriveLetter">Drive Letter To Return Free Space Value, Ex: C:\</param>
        /// <returns>Returns Space Free in MB</returns>
        public string DriveSpaceFree(string DriveLetter)
        {
            string AHKLine = "DriveSpaceFree, OutputVar," + DriveLetter;  // ahk line to execute
            ErrorLog_Setup(false); // ErrorLevel Detection Enabled for this function in AHK 
            string OutVar = Execute(AHKLine, "OutputVar");   // execute AHK code and return variable value

            //int OutputVar = Int32.Parse(OutVar);  //convert output to int
            return OutVar;
        }

        /// <summary>Retrieves various types of information about the computer's drive(s).</summary>
        /// <param name="Cmd">List|Capacity|Filesystem|Drive|Label|Serial|Type|Status|StatusCD</param>
        /// <param name="Value">Type|Path|Drive|Drive|Path|Path|Drive</param>
        public string DriveGet(string Cmd, string Value = "")
        {
            string AHKLine = "DriveGet, OutputVar, " + Cmd + ", " + Value;  // ahk line to execute
            ErrorLog_Setup(true); // ErrorLevel Detection Enabled for this function in AHK 
            return Execute(AHKLine, "OutputVar");   // execute AHK code and return variable value
        }

        /// <summary>Returns string of letters, one character for each drive letter in the system. For example: ACDEZ. If Type is omitted, all drive types are retrieved. Otherwise, Type should be one of the following words to retrieve only a specific type of drive: CDROM, REMOVABLE, FIXED, NETWORK, RAMDISK, UNKNOWN.</summary>
        /// <param name="Type">All Drive Types Returned If Blank || CDROM|REMOVABLE|FIXED|NETWORK|RAMDISK|UNKNOWN</param>
        public string DriveGet_List(DriveType Type = DriveType.ALL)
        {
            string type = "";
            if (Type == DriveType.CDROM) { type = "CDROM"; }
            if (Type == DriveType.REMOVABLE) { type = "REMOVABLE"; }
            if (Type == DriveType.FIXED) { type = "FIXED"; }
            if (Type == DriveType.NETWORK) { type = "NETWORK"; }
            if (Type == DriveType.RAMDISK) { type = "RAMDISK"; }
            if (Type == DriveType.UNKNOWN) { type = "UNKNOWN"; }

            string AHKLine = "DriveGet, OutputVar, List," + type;  // ahk line to execute
            ErrorLog_Setup(true); // ErrorLevel Detection Enabled for this function in AHK 
            string returnVal = Execute(AHKLine, "OutputVar");   // execute AHK code and return variable value
            return returnVal;
        }

        /// <summary>Retrieves the total capacity of Path (e.g. C:\) in megabytes.</summary>
        /// <param name="Path">Drive Path To Check Total Capacity Of</param>
        public string DriveGet_Capacity(string Path = @"C:\")
        {
            string AHKLine = "DriveGet, OutputVar, Capacity, " + Path;  // ahk line to execute
            ErrorLog_Setup(true); // ErrorLevel Detection Enabled for this function in AHK 
            string returnVal = Execute(AHKLine, "OutputVar");   // execute AHK code and return variable value
            return returnVal;
        }

        /// <summary>Retrieves the type of Drive's file system, where Drive is the drive letter followed by a colon and an optional backslash, or a UNC name such \\server1\share1. OutputVar will be set to one of the following words: FAT, FAT32, NTFS, CDFS (typically indicates a CD), UDF (typically indicates a DVD). OutputVar will be made blank and ErrorLevel set to 1 if the drive does not contain formatted media.</summary>
        /// <param name="Path">Drive Path To Check FileSystem Type</param>
        public string DriveGet_FileSystem(string Path = @"C:\")
        {
            string AHKLine = "DriveGet, OutputVar, FileSystem, " + Path;  // ahk line to execute
            ErrorLog_Setup(true); // ErrorLevel Detection Enabled for this function in AHK 
            string returnVal = Execute(AHKLine, "OutputVar");   // execute AHK code and return variable value
            return returnVal;
        }

        /// <summary>Retrieves Drive's volume label, where Drive is the drive letter followed by a colon and an optional backslash, or a UNC name such \\server1\share1.</summary>
        /// <param name="Path">Drive Path To Return Label From</param>
        public string DriveGet_Label(string Path = @"C:\")
        {
            string AHKLine = "DriveGet, OutputVar, Label, " + Path;  // ahk line to execute
            ErrorLog_Setup(true); // ErrorLevel Detection Enabled for this function in AHK 
            string returnVal = Execute(AHKLine, "OutputVar");   // execute AHK code and return variable value
            return returnVal;
        }

        /// <summary>Retrieves Drive's volume serial number expressed as decimal integer, where Drive is the drive letter followed by a colon and an optional backslash, or a UNC name such \\server1\share1.</summary>
        /// <param name="Path">Drive Path To Return Info From</param>
        public string DriveGet_Serial(string Path = @"C:\")
        {
            string AHKLine = "DriveGet, OutputVar, Serial, " + Path;  // ahk line to execute
            ErrorLog_Setup(true); // ErrorLevel Detection Enabled for this function in AHK 
            string returnVal = Execute(AHKLine, "OutputVar");   // execute AHK code and return variable value
            return returnVal;
        }

        /// <summary>Retrieves Path's drive type, which is one of the following words: Unknown, Removable, Fixed, Network, CDROM, RAMDisk.</summary>
        /// <param name="Path">Drive Path To Return Info From</param>
        public string DriveGet_Type(string Path = @"C:\")
        {
            string AHKLine = "DriveGet, OutputVar, Type, " + Path;  // ahk line to execute
            ErrorLog_Setup(true); // ErrorLevel Detection Enabled for this function in AHK 
            string returnVal = Execute(AHKLine, "OutputVar");   // execute AHK code and return variable value
            return returnVal;
        }

        /// <summary>Retrieves Path's status, which is one of the following words: Unknown (might indicate unformatted/RAW), Ready, NotReady (typical for removable drives that don't contain media), Invalid (Path does not exist or is a network drive that is presently inaccessible, etc.)</summary>
        /// <param name="Path">Drive Path To Return Info From</param>
        public string DriveGet_Status(string Path = @"C:\")
        {
            string AHKLine = "DriveGet, OutputVar, Status, " + Path;  // ahk line to execute
            ErrorLog_Setup(true); // ErrorLevel Detection Enabled for this function in AHK 
            string returnVal = Execute(AHKLine, "OutputVar");   // execute AHK code and return variable value
            return returnVal;
        }

        /// <summary>Retrieves the media status of a CD or DVD drive, where Drive is the drive letter followed by a colon (if Drive is omitted, the default CD/DVD drive will be used).</summary>
        /// <param name="Path">Drive Path To Return Info From</param>
        public string DriveGet_StatusCD(string Path = @"D:\")
        {
            string AHKLine = "DriveGet, OutputVar, StatusCD, " + Path;  // ahk line to execute
            ErrorLog_Setup(true); // ErrorLevel Detection Enabled for this function in AHK 
            string returnVal = Execute(AHKLine, "OutputVar");   // execute AHK code and return variable value
            return returnVal;
        }



    }
}
