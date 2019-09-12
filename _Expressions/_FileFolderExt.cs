using sharpAHK;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace AHKExpressions
{
    public static partial class AHKExpressions
    {
        // #region === FileFolder ===

        /// <summary>Writes text to the end of a file (first creating the file, if necessary).</summary>
        /// <param name="Text">The text to append to the file. This text may include linefeed characters (`n) to start new lines.</param>
        /// <param name="FileName">The name of the file to be appended, which is assumed to be in %A_WorkingDir% if an absolute path isn't specified.</param>
        /// <param name="Mode">Mode 0 = Default using AHK FileAppend command | Mode 1 = Using StreamWriter to write Text</param>
        public static bool FileAppend(this string Text, string FileName, int Mode = 1)
        {
            _AHK ahk = new _AHK();
            return ahk.FileAppend(Text, FileName, Mode);
        }

        /// <summary>Copies one or more files.</summary>
        /// <param name="SourcePattern">The name of a single file or folder, or a wildcard pattern such as C:\Temp\*.tmp. SourcePattern is assumed to be in WorkingDir if an absolute path isn't specified.</param>
        /// <param name="DestPattern">The name or pattern of the destination, which is assumed to be in %A_WorkingDir% if an absolute path isn't specified.</param>
        /// <param name="OverWrite">Flag determines whether to overwrite files if they already exist. True = OverWrite Existing Files</param>
        /// <param name="ProgressDialog">Option to Display Windows FileCopy Dialog While File Transfers</param>
        public static bool FileCopy(this string SourcePattern, string DestPattern, bool OverWrite = false, bool ProgressDialog = false)
        {
            _AHK ahk = new _AHK();
            return ahk.FileCopy(SourcePattern, DestPattern, OverWrite, ProgressDialog);
        }


        /// <summary>Creates a shortcut (.lnk) file.</summary>
        /// <param name="Target">Name of the file that the shortcut refers to, which should include an absolute path unless the file is integrated with the system (e.g. Notepad.exe).</param>
        /// <param name="LinkFile">Name of the shortcut file to be created, which is assumed to be in %A_WorkingDir% if an absolute path isn't specified. Be sure to include the .lnk extension. If the file already exists, it will be overwritten.</param>
        /// <param name="WorkingDir">Directory that will become Target's current working directory when the shortcut is launched. If blank or omitted, the shortcut will have a blank "Start in" field and the system will provide a default working directory when the shortcut is launched.</param>
        /// <param name="Args">Parameters that will be passed to Target when it is launched. Separate parameters with spaces. If a parameter contains spaces, enclose it in double quotes.</param>
        /// <param name="Description">Comments that describe the shortcut (used by the OS to display a tooltip, etc.)</param>
        /// <param name="IconFile">The full path and name of the icon to be displayed for LinkFile. It must either be an ico file or the very first icon of an EXE or DLL.</param>
        /// <param name="ShortcutKey">A single letter, number, or the name of a single key from the key list (mouse buttons and other non-standard keys might not be supported). Do not include modifier symbols. Currently, all shortcut keys are created as CTRL+ALT shortcuts. For example, if the letter B is specified for this parameter, the shortcut key will be CTRL-ALT-B.</param>
        /// <param name="IconNumber">To use an icon in IconFile other than the first, specify that number here (can be an expression). For example, 2 is the second icon.</param>
        /// <param name="RunState">To have Target launched minimized or maximized, specify one of the following digits: 1 - Normal (this is the default) | 3 - Maximized | 7 - Minimized</param>
        public static bool FileCreateShortcut(this string Target, string LinkFile, string WorkingDir = "", string Args = "", string Description = "", string IconFile = "", string ShortcutKey = "", string IconNumber = "", string RunState = "")
        {
            _AHK ahk = new _AHK();
            return ahk.FileCreateShortcut(Target, LinkFile, WorkingDir, Args, Description, IconFile, ShortcutKey, IconNumber, RunState);
        }


        /// <summary>
        /// Adds shortcut to application for current executable in user's startup directory  
        /// </summary>
        public static void AppShortcutToStartup()
        {
            _AHK ahk = new _AHK();
            ahk.AppShortcutToStartup();
        }

        /// <summary>
        /// Creates URL shortcut on user's pc (default location = desktop)
        /// </summary>
        /// <param name="linkName">Name of URL ShortCut/Site</param>
        /// <param name="linkUrl">URL for new Link</param>
        /// <param name="SaveDir">Directory to save new link to</param>
        public static void UrlShortcutToDesktop(this string linkName, string linkUrl, string SaveDir = "Desktop")
        {
            _AHK ahk = new _AHK();
            ahk.UrlShortcutToDesktop(linkName, linkUrl, SaveDir);
        }


        /// <summary>Deletes one or more files.</summary>
        /// <param name="FilePattern">The name of a single file or a wildcard pattern such as C:\Temp\*.tmp. FilePattern is assumed to be in %A_WorkingDir% if an absolute path isn't specified.</param>
        /// <param name="PromptUser">Option to Prompt User with Yes/No PopUp Dialog Before Deleting (Default = False)</param>
        public static bool FileDelete(this string FilePattern, bool PromptUser = false)
        {
            _AHK ahk = new _AHK();
            return ahk.FileDelete(FilePattern, PromptUser);
        }

        /// <summary>Reports whether a file or folder is read-only, hidden, etc.</summary>
        /// <param name="Filename">The name of the target file, which is assumed to be in %A_WorkingDir% if an absolute path isn't specified.</param>        
        public static string FileGetAttrib(this string Filename)
        {
            _AHK ahk = new _AHK();
            return ahk.FileGetAttrib(Filename);
        }

        /// <summary>Retrieves information about a shortcut (.lnk) file, such as its target file.</summary>
        /// <param name="LinkFile"></param>        
        public static _AHK.shortCutInfo FileGetShortcut(string LinkFile)
        {
            _AHK ahk = new _AHK();
            return FileGetShortcut(LinkFile);
        }

        /// <summary>Read File, Return File Bytes</summary>
        /// <param name="filePath">Path to file to convert to bytes</param>    
        public static byte[] FileBytes(this string filePath)
        {
            _AHK ahk = new _AHK();
            return ahk.FileBytes(filePath);
        }


        /// <summary>Retrieves the size of a file.</summary>
        /// <param name="Filename">The name of the target file, which is assumed to be in %A_WorkingDir% if an absolute path isn't specified.</param>
        /// <param name="Units">If present, this parameter causes the result to be returned in units other than bytes: K = kilobytes | M = megabytes</param>
        public static string FileGetSize(this string Filename, string Units = "")
        {
            _AHK ahk = new _AHK();
            return ahk.FileGetSize(Filename, Units);
        }

        ///// <summary>Retrieves the size of a folder.</summary>
        ///// <param name="DirPath">Directory to poll for current disk size.</param>
        ///// <param name="Units">If present, this parameter causes the result to be returned in units other than bytes: K = kilobytes | M = megabytes</param>
        //public static string DirSize(this string DirPath, string Units = "")
        //{
        //    _AHK ahk = new _AHK();
        //    return ahk.DirSize(DirPath, Units);
        //}


        /// <summary>Retrieves the datetime stamp of a file or folder.</summary>
        /// <param name="Filename">The name of the target file, which is assumed to be in %A_WorkingDir% if an absolute path isn't specified.</param>
        /// <param name="WhichTime">Which timestamp to retrieve: M = Modification time (default if omitted) | C = Creation time | A = Last access time   </param>        
        public static string FileGetTime(this string Filename, string WhichTime = "")
        {
            _AHK ahk = new _AHK();
            return ahk.FileGetTime(Filename, WhichTime);
        }

        /// <summary>Retrieves the version of a file.</summary>
        /// <param name="Filename">The name of the target file, which is assumed to be in %A_WorkingDir% if an absolute path isn't specified.</param>
        public static string FileGetVersion(this string Filename)
        {
            _AHK ahk = new _AHK();
            return ahk.FileGetVersion(Filename);
        }

        //FileInstall - Skipped

        /// <summary>Moves or renames one or more files.</summary>
        /// <param name="SourcePattern">The name of a single file or a wildcard pattern such as C:\Temp\*.tmp. SourcePattern is assumed to be in %A_WorkingDir% if an absolute path isn't specified.</param>
        /// <param name="DestPattern">The name or pattern of the destination, which is assumed to be in %A_WorkingDir% if an absolute path isn't specified. To perform a simple move -- retaining the existing file name(s) -- specify only the folder name as shown in these functionally identical examples: FileMove, C:\*.txt, C:\My Folder</param>
        /// <param name="OverWrite">Determines whether to overwrite files if they already exist</param>
        public static bool FileMove(this string SourcePattern, string DestPattern, bool OverWrite = false)
        {
            _AHK ahk = new _AHK();
            return ahk.FileMove(SourcePattern, DestPattern, OverWrite);

        }

        /// <summary>Renames one or more files (same as FileMove)</summary>
        /// <param name="SourcePattern">The name of a single file or a wildcard pattern such as C:\Temp\*.tmp. SourcePattern is assumed to be in %A_WorkingDir% if an absolute path isn't specified.</param>
        /// <param name="DestPattern">The name or pattern of the destination, which is assumed to be in %A_WorkingDir% if an absolute path isn't specified. To perform a simple move -- retaining the existing file name(s) -- specify only the folder name as shown in these functionally identical examples: FileMove, C:\*.txt, C:\My Folder</param>
        /// <param name="OverWrite">Determines whether to overwrite files if they already exist</param>
        public static bool FileRename(this string SourcePattern, string DestPattern, bool OverWrite = false)
        {
            _AHK ahk = new _AHK();
            return ahk.FileMove(SourcePattern, DestPattern, OverWrite);
        }

        /// <summary>
        /// Loop through file pattern, return matches as list of full file paths
        /// </summary>
        /// <param name="FilePattern"></param>
        /// <param name="IncludeFolders"></param>
        /// <param name="Recurse"></param>
        /// <returns></returns>
        public static List<string> FileListLoop(this string FilePattern, bool IncludeFolders = false, bool Recurse = false)
        {
            _AHK ahk = new _AHK();
            return ahk.FileListLoop(FilePattern, IncludeFolders, Recurse);
        }

        /// <summary>
        /// Loop Through File Pattern/Folder, Return Number of Files Found
        /// </summary>
        /// <param name="Directory">Path to Folder to Search For FileCount</param>
        /// <param name="SearchPattern">File Pattern to Match For FileCount, Default = *.*</param>
        /// <param name="IncludeFolders">Add # of Directories Found to Total File Count</param>
        /// <param name="Recurse">Option to Include Files Found in Subdirectories. Default = true</param>
        /// <returns></returns>
        public static int FileCount(this string Directory, string SearchPattern = "*.*", bool IncludeFolders = false, bool Recurse = true)
        {
            _AHK ahk = new _AHK();
            return ahk.FileCount(Directory, SearchPattern, IncludeFolders, Recurse);
        }

        /// <summary>
        /// Loop Through File Pattern/Folder, Return Number of Directories Found
        /// </summary>
        /// <param name="Directory">Path to Folder to Search For FileCount</param>
        /// <param name="SearchPattern">File Pattern to Match For FileCount, Default = *.*</param>
        /// <param name="Recurse">Option to Include Subdirectories. Default = true</param>
        /// <returns></returns>
        public static int DirCount(this string Directory, string SearchPattern = "*.*", bool Recurse = true)
        {
            _AHK ahk = new _AHK();
            return ahk.DirCount(Directory, SearchPattern, Recurse);
        }

        /// <summary>Reads the specified line from a file and stores the text in a variable.</summary>
        /// <param name="Filename">The name of the file to access, which is assumed to be in %A_WorkingDir% if an absolute path isn't specified.</param>
        /// <param name="LineNum">Which line to read (1 is the first, 2 the second, and so on). This can be an expression.</param>
        public static string FileReadLine(this string Filename, string LineNum)
        {
            _AHK ahk = new _AHK();
            return ahk.FileReadLine(Filename, LineNum);
        }

        /// <summary>Reads a File's Contents into a Variable</summary>
        /// <param name="FilePath">Path to File to Read, Assumed to be in %A_WorkingDir% if Absolute Path isn't Specified</param>
        public static string FileRead(this string FilePath)
        {
            _AHK ahk = new _AHK();
            return ahk.FileRead(FilePath);
        }

        /// <summary>Sends a file or directory to the recycle bin, if possible.</summary>
        /// <param name="FilePattern">The name of a single file or a wildcard pattern such as C:\Temp\*.tmp. FilePattern is assumed to be in %A_WorkingDir% if an absolute path isn't specified. To recycle an entire directory, provide its name without a trailing backslash.</param>
        public static bool FileRecycle(this string FilePattern)
        {
            _AHK ahk = new _AHK();
            return ahk.FileRecycle(FilePattern);
        }

        /// <summary>Empties the recycle bin.</summary>
        /// <param name="DriveLetter">If omitted, the recycle bin for all drives is emptied. Otherwise, specify a drive letter such as C:\</param>        
        public static bool FileRecycleEmpty(this string DriveLetter)
        {
            _AHK ahk = new _AHK();
            return ahk.FileRecycleEmpty(DriveLetter);
        }


        /// <summary>Displays a standard dialog that allows the user to select a folder.</summary>
        /// <param name="StartingFolder">If blank or omitted, the dialog's initial selection will be the user's My Documents folder (or possibly My Computer).</param>
        /// <param name="Options">0: The options are all disabled | 1 (default): A button is provided that allows the user to create new folders. | Add 2 to the above number to provide an edit field that allows the user to type the name of a folder. | Adding 4 ensures that FileSelectFolder will work properly even in a Preinstallation Environment like WinPE or BartPE</param>
        /// <param name="Prompt">Text displayed in the window to instruct the user what to do. If omitted or blank, it will default to "Select Folder - %A_SCRIPTNAME%" (i.e. the name of the current script).</param>
        public static string FileSelectFolder(this string StartingFolder, string Options = "", string Prompt = "")
        {
            _AHK ahk = new _AHK();
            return ahk.FileSelectFolder(StartingFolder, Options, Prompt);
        }

        /// <summary>Changes the attributes of one or more files or folders. Wildcards are supported.</summary>
        /// <param name="Attributes">+ Turn on the attribute | - Turn off the attribute | ^ Toggle the attribute || R = READONLY | A = ARCHIVE | S = SYSTEM | H = HIDDEN | N = NORMAL | O = OFFLINE | T = TEMPORARY</param>
        /// <param name="FilePattern">The name of a single file or folder, or a wildcard pattern such as C:\Temp\*.tmp. FilePattern is assumed to be in %A_WorkingDir% if an absolute path isn't specified. </param>
        /// <param name="OperateOnFolders">0 (default) Folders are not operated upon (only files). | 1 All files and folders that match the wildcard pattern are operated upon. | 2 Only folders are operated upon (no files)</param>
        /// <param name="Recurse">False (default) Subfolders are not recursed into. | True = Subfolders are recursed into so that files and folders contained therein are operated upon if they match FilePattern. </param>
        public static bool FileSetAttrib(this string Attributes, string FilePattern = "", string OperateOnFolders = "0", bool Recurse = false)
        {
            _AHK ahk = new _AHK();
            return ahk.FileSetAttrib(Attributes, FilePattern, OperateOnFolders, Recurse);
        }

        /// <summary>Changes the datetime stamp of one or more files or folders. Wildcards are supported.</summary>
        /// <param name="YYYYMMDDHH24MISS">If blank or omitted, it defaults to the current time. Otherwise, specify the time to use for the operation (see Remarks for the format). Years prior to 1601 are not supported.</param>
        /// <param name="FilePattern">The name of a single file or folder, or a wildcard pattern such as C:\Temp\*.tmp. FilePattern is assumed to be in %A_WorkingDir% if an absolute path isn't specified.</param>
        /// <param name="WhichTime">M = Modification time (this is the default if the parameter is blank or omitted) | C = Creation time | A = Last access time</param>
        /// <param name="OperateOnFolders">0 (default) Folders are not operated upon (only files). | 1 All files and folders that match the wildcard pattern are operated upon. | 2 Only folders are operated upon (no files).</param>
        /// <param name="Recurse">False (default) Subfolders are not recursed into. | True = Subfolders are recursed into so that files and folders contained therein are operated upon if they match FilePattern.</param>        
        public static bool FileSetTime(this string YYYYMMDDHH24MISS, string FilePattern = "", string WhichTime = "", string OperateOnFolders = "", bool Recurse = false)
        {
            _AHK ahk = new _AHK();
            return ahk.FileSetTime(YYYYMMDDHH24MISS, FilePattern, WhichTime, OperateOnFolders, Recurse);
        }

        /// <summary>Checks for the existence of a file or folder.</summary>
        /// <param name="FilePattern">The path, filename, or file pattern to check. FilePattern is assumed to be in %A_WorkingDir% if an absolute path isn't specified.</param>
        public static bool IfExist(this string FilePattern)
        {
            _AHK ahk = new _AHK();
            return ahk.IfExist(FilePattern);
        }

        /// <summary>Checks for the existence of a file or folder.</summary>
        /// <param name="FilePattern">The path, filename, or file pattern to check. FilePattern is assumed to be in %A_WorkingDir% if an absolute path isn't specified.</param>        
        public static bool IfNotExist(this string FilePattern)
        {
            _AHK ahk = new _AHK();
            return ahk.IfNotExist(FilePattern);
        }


        /// <summary>Changes the script's current working directory.</summary>
        /// <param name="DirName">The name of the new working directory, which is assumed to be a subfolder of the current %A_WorkingDir% if an absolute path isn't specified.</param>
        public static bool SetWorkingDir(this string DirName)
        {
            _AHK ahk = new _AHK();
            return ahk.SetWorkingDir(DirName);
        }


        //####  File Path Info (replace SplitPath ahk function)  #####

        /// <summary>Separates a file path - returns file name (with extension)</summary>
        /// <param name="FilePath">File Location to Parse</param>
        /// <param name="CheckIfExists">Option to check to see if FilePath exists - FileName returns blank if file not found</param>
        public static string FileName(this string FilePath, bool CheckIfExists = false)
        {
            _AHK ahk = new _AHK();
            return ahk.FileName(FilePath, CheckIfExists);
        }

        /// <summary>Returns File Name (with Extension)</summary>
        /// <param name="file">File Object to Parse</param>
        public static string FileName(this FileInfo file)
        {
            _AHK ahk = new _AHK();
            return ahk.FileName(file.ToString(), false);
        }

        /// <summary>Separates a file path - returns file name (no extension)</summary>
        /// <param name="FilePath">File Location to Parse</param>
        /// <param name="CheckIfExists">Option to check to see if FilePath exists - FileNameNoExt returns blank if file not found</param>
        public static string FileNameNoExt(this string FilePath, bool CheckIfExists = false)
        {
            _AHK ahk = new _AHK();
            return ahk.FileNameNoExt(FilePath, CheckIfExists);
        }

        /// <summary>Separates a file path - returns file extension (includex '.' prefix)</summary>
        /// <param name="FilePath">File Location to Parse</param>
        /// <param name="CheckIfExists">Option to check to see if FilePath exists - FileExt returns blank if file not found</param>
        /// <param name="RemovePrefix">Option to remove leading . in front of File Extention Return</param>
        public static string FileExt(this string FilePath, bool CheckIfExists = false, bool RemovePrefix = false)
        {
            _AHK ahk = new _AHK();
            return ahk.FileExt(FilePath, CheckIfExists, RemovePrefix);
        }

        /// <summary>Returns File's Parent Directory Path</summary>
        /// <param name="FilePath">File Location to Parse</param>
        /// <param name="CheckIfExists">Option to check to see if FilePath exists - FileDir returns blank if file not found</param>
        public static string FileDir(this string FilePath, bool CheckIfExists = false)  // 
        {
            _AHK ahk = new _AHK();
            return ahk.FileDir(FilePath, CheckIfExists);
        }

        /// <summary>Returns File's Parent Directory Name from Full File Path</summary>
        /// <param name="FilePath">File Location to Parse</param>
        /// <param name="CheckIfExists">Option to check to see if FilePath exists - FileDir returns blank if file not found</param>
        public static string DirName(this string FilePath, bool CheckIfExists = false)
        {
            _AHK ahk = new _AHK();
            return ahk.DirName(FilePath, CheckIfExists);
        }

        // File Info

        /// <summary>Returns name of file size in bytes from file path</summary>
        /// <param name="FilePath">File Location to Parse</param>
        /// <param name="CheckIfExists">Option to check to see if FilePath exists - FileDir returns blank if file not found</param>
        /// <param name="ReturnBytes">Returns Bytes by Default, option to override and return bytes converted to KB/MB/TB</param>
        public static string FileSize(this string FilePath, bool CheckIfExists = false, bool ReturnBytes = true)
        {
            _AHK ahk = new _AHK();
            return ahk.FileSize(FilePath, CheckIfExists, ReturnBytes);
        }

        /// <summary>checks whether a file is Compressed</summary>
        /// <param name="filePath"> </param>
        public static bool isCompressed(this string filePath)
        {
            _AHK ahk = new _AHK();
            return ahk.isCompressed(filePath);
        }

        /// <summary>checks whether a file is encrypted</summary>
        /// <param name="filePath"> </param>
        public static bool isEncrypted(this string filePath)
        {
            _AHK ahk = new _AHK();
            return ahk.isEncrypted(filePath);
        }

        /// <summary>checks whether a file is read only</summary>
        /// <param name="filePath"> </param>
        public static bool isReadOnly(this string filePath)
        {
            _AHK ahk = new _AHK();
            return ahk.isReadOnly(filePath);
        }

        /// <summary>checks whether a file is hidden</summary>
        /// <param name="filePath"> </param>
        public static bool isHidden(this string filePath)
        {
            _AHK ahk = new _AHK();
            return ahk.isHidden(filePath);
        }

        /// <summary>checks whether a file has archive attribute</summary>
        /// <param name="filePath"> </param>
        public static bool isArchive(this string filePath)
        {
            _AHK ahk = new _AHK();
            return ahk.isArchive(filePath);
        }

        /// <summary>checks whether a file is system file</summary>
        /// <param name="filePath"> </param>
        public static bool isSystem(this string filePath)
        {
            _AHK ahk = new _AHK();
            return ahk.isSystem(filePath);
        }

        /// <summary>
        /// Checks FilePath's Extension to see if it matches known video formats
        /// </summary>
        /// <param name="FilePath">File path to check</param>
        /// <returns>Returns True if File is Known Video Format</returns>
        public static bool isVideo(this string FilePath)
        {
            _AHK ahk = new _AHK();
            return ahk.isVideo(FilePath);
        }

        /// <summary>
        /// Checks FilePath's Extension to see if it matches known text file formats
        /// </summary>
        /// <param name="FilePath">File path to check</param>
        /// <returns>Returns True if File is Known Text File Format</returns>
        public static bool isText(this string FilePath)
        {
            _AHK ahk = new _AHK();
            return ahk.isText(FilePath);
        }

        /// <summary>
        /// Checks FilePath's Extension to see if it matches known Image File Formats
        /// </summary>
        /// <param name="FilePath">File path to check</param>
        /// <returns>Returns True if File is Known Image File Format</returns>
        public static bool isImage(this string FilePath)
        {
            _AHK ahk = new _AHK();
            return ahk.isImage(FilePath);
        }



        /// <summary>Returns the timestamp when the file was created</summary>
        /// <param name="FilePath"> </param>
        public static DateTime CreationTime(this string FilePath)
        {
            _AHK ahk = new _AHK();
            return ahk.CreationTime(FilePath);
        }

        /// <summary>Returns the timestamp when the file was written to</summary>
        /// <param name="FilePath"> </param>
        public static DateTime LastWriteTime(this string FilePath)
        {
            _AHK ahk = new _AHK();
            return ahk.LastWriteTime(FilePath);
        }

        /// <summary>Returns the timestamp when the file was last accessed</summary>
        /// <param name="FilePath"> </param>
        public static DateTime LastAccessTime(this string FilePath)
        {
            _AHK ahk = new _AHK();
            return ahk.LastAccessTime(FilePath);
        }


        // File Actions

        /// <summary>Copies a folder along with all its sub-folders and files (similar to xcopy).</summary>
        /// <param name="Source">Name of the source directory (with no trailing backslash), which is assumed to be in %A_WorkingDir% if an absolute path isn't specified.</param>
        /// <param name="Dest">Name of the destination directory (with no trailing baskslash), which is assumed to be in %A_WorkingDir% if an absolute path isn't specified.</param>
        /// <param name="OverWrite">Flag determines whether to overwrite files if they already exist. True = OverWrite Existing Files</param>
        public static bool FileCopyDir(this string Source, string Dest, bool OverWrite = false)
        {
            _AHK ahk = new _AHK();
            return ahk.FileCopyDir(Source, Dest, OverWrite);
        }
        /// <summary>Copies a folder along with all its sub-folders and files (similar to xcopy) (Same as FileCopyDir)</summary>
        /// <param name="Source">Name of the source directory (with no trailing backslash), which is assumed to be in %A_WorkingDir% if an absolute path isn't specified.</param>
        /// <param name="Dest">Name of the destination directory (with no trailing baskslash), which is assumed to be in %A_WorkingDir% if an absolute path isn't specified.</param>
        /// <param name="OverWrite">Flag determines whether to overwrite files if they already exist. True = OverWrite Existing Files</param>
        public static bool CopyDir(this string Source, string Dest, bool OverWrite = false)
        {
            _AHK ahk = new _AHK();
            return ahk.FileCopyDir(Source, Dest, OverWrite);
        }

        /// <summary>Creates a directory/folder.</summary>
        /// <param name="DirName">Name of the directory to create, which is assumed to be in %A_WorkingDir% if an absolute path isn't specified.</param>
        public static bool FileCreateDir(this string DirName)
        {
            _AHK ahk = new _AHK();
            return ahk.FileCreateDir(DirName);
        }
        /// <summary>Creates a directory/folder. (Same As FileCreateDir)</summary>
        /// <param name="DirName">Name of the directory to create, which is assumed to be in %A_WorkingDir% if an absolute path isn't specified.</param>
        public static bool CreateDir(this string DirName)
        {
            _AHK ahk = new _AHK();
            return ahk.FileCreateDir(DirName);
        }


        /// <summary>Moves a folder along with all its sub-folders and files. It can also rename a folder.</summary>
        /// <param name="Source">Name of the source directory (with no trailing backslash), which is assumed to be in %A_WorkingDir% if an absolute path isn't specified.</param>
        /// <param name="Dest">The new path and name of the directory (with no trailing baskslash), which is assumed to be in %A_WorkingDir% if an absolute path isn't specified. For example: D:\My Folder. Note: Dest is the actual path and name that the directory will have after it is moved; it is not the directory into which Source is moved (except for the known limitation mentioned below).</param>
        /// <param name="Flag">0 (default): Do not overwrite existing files. | 1: Overwrite existing files. However, any files or subfolders inside Dest that do not have a counterpart in Source will not be deleted. | 2: The same as mode 1 above except that the limitation is absent. | R: Rename the directory rather than moving it. </param>
        public static bool FileMoveDir(this string Source, string Dest, string Flag = "0")
        {
            _AHK ahk = new _AHK();
            return ahk.FileMoveDir(Source, Dest, Flag);
        }

        /// <summary>Moves a folder along with all its sub-folders and files. It can also rename a folder. (Same as FileMoveDir)</summary>
        /// <param name="Source">Name of the source directory (with no trailing backslash), which is assumed to be in %A_WorkingDir% if an absolute path isn't specified.</param>
        /// <param name="Dest">The new path and name of the directory (with no trailing baskslash), which is assumed to be in %A_WorkingDir% if an absolute path isn't specified. For example: D:\My Folder. Note: Dest is the actual path and name that the directory will have after it is moved; it is not the directory into which Source is moved (except for the known limitation mentioned below).</param>
        /// <param name="Flag">0 (default): Do not overwrite existing files. | 1: Overwrite existing files. However, any files or subfolders inside Dest that do not have a counterpart in Source will not be deleted. | 2: The same as mode 1 above except that the limitation is absent. | R: Rename the directory rather than moving it. </param>
        public static bool MoveDir(this string Source, string Dest, string Flag = "0")
        {
            _AHK ahk = new _AHK();
            return ahk.FileMoveDir(Source, Dest, Flag);
        }

        /// <summary>Deletes a folder.</summary>
        /// <param name="DirName">Name of the directory to delete, which is assumed to be in %A_WorkingDir% if an absolute path isn't specified.</param>
        /// <param name="Recurse">Recurse = False - Do not remove files and sub-directories contained in DirName. In this case, if DirName is not empty, no action will be taken | True = Remove all files and subdirectories.</param>
        public static bool FileRemoveDir(this string DirName, bool Recurse = false)
        {
            _AHK ahk = new _AHK();
            return ahk.FileRemoveDir(DirName, Recurse);
        }

        /// <summary>Opens Directory in Windows Explorer Window (If Found), Returns False if there is an Error / Directory Not Found. If File Path Provided, Opens Dir Containing File</summary>
        /// <param name="DirPath">Path to directory to open in explorer</param>
        /// <param name="CreateIfMissing">Option to Create Missing Directory instead of Returning False, Opens New Dir After Creating</param>
        public static bool OpenDir(this string DirPath, bool CreateIfMissing = false)
        {
            _AHK ahk = new _AHK();
            if (DirPath.IsDir()) { return ahk.OpenDir(DirPath, CreateIfMissing); }
            if (File.Exists(DirPath)) { return ahk.OpenFileDir(DirPath); }
            else return false;
        }

        /// Retired (Redundant)
        ///// <summary>Opens Directory in Windows Explorer containing FilePath</summary>
        ///// <param name="FilePath">Path to file, extracting the folder path to open</param>
        //public static bool OpenFileDir(this string FilePath)
        //{
        //    _AHK ahk = new _AHK();
        //    return ahk.OpenFileDir(FilePath);
        //}

        // File Compare

        #region === File Compare / Hash ===

        /// <summary>
        /// Returns Hash value for File
        /// </summary>
        /// <param name="FilePath"></param>
        /// <returns></returns>
        public static string FileHash(this string FilePath)
        {
            _AHK ahk = new _AHK();
            return ahk.FileHash(FilePath);
        }

        /// <summary>
        /// Compare 2 Files To See if they are the same (either compares Hash or Bytes)
        /// </summary>
        /// <param name="fileOne">First File To Compare</param>
        /// <param name="fileTwo">Second File to Compare</param>
        /// <param name="Hash">Option to Compare File Hashes (Default = True). If False, Compares by Bytes</param>
        /// <returns>Returns True if Files are the Same</returns>
        public static bool FileCompare(this string fileOne, string fileTwo, bool Hash = true)
        {
            _AHK ahk = new _AHK();
            return ahk.FileCompare(fileOne, fileTwo, Hash);
        }

        /// <summary>
        /// Compare two files by bytes - returns true if match found 
        /// </summary>
        /// <param name="file1"></param>
        /// <param name="file2"></param>
        /// <returns></returns>
        public static bool FileCompare_Bytes(this string file1, string file2)
        {
            _AHK ahk = new _AHK();
            return ahk.FileCompare_Bytes(file1, file2);
        }

        /// <summary>
        /// Compares two image paths using hash - returns true if match found
        /// </summary>
        /// <param name="file1"></param>
        /// <param name="file2"></param>
        /// <returns></returns>
        public static bool FileCompare_Hash(this string file1, string file2)
        {
            _AHK ahk = new _AHK();
            return ahk.FileCompare_Hash(file1, file2);
        }


        #endregion





        //====== Dir Print ==============================================

        // string DirectoryFilePaths = ahk.DirPrint("C:\\CCSM", true, "c:\\OutFile.txt", "*.txt", true); 

        /// <summary>
        /// Prints List of Files in Directory to String or New Text File
        /// </summary>
        /// <param name="SearchDir">Directory to Loop through for File Paths</param>
        /// <param name="OutFile">If path provided, writes contents of directory to this to new/existing text file</param> 
        /// <param name="Recurse">Option to Search Files in Subdirectories (aka Recurse) Default = True</param>
        /// <param name="SearchPattern">File pattern to search for</param>
        /// <param name="OverWritePrevious">Option to overwrite previous OutFile if it exists (Default = True)</param>
        /// <param name="OpenAfterWrite">If OutFile path provided, option to open new text file after writing (Default = True)</param>
        /// <returns>Returns string with list of file paths under SearchDir</returns>
        public static string DirPrint(this string SearchDir, string OutFile = "", bool Recurse = true, string SearchPattern = "*.*", bool OverWritePrevious = true, bool OpenAfterWrite = true)
        {
            _AHK ahk = new _AHK();
            return ahk.DirPrint(SearchDir, OutFile, Recurse, SearchPattern, OverWritePrevious, OpenAfterWrite);
        }

        /// <summary>Converts search directory contents to Datatable to display in DataGridView etc</summary>
        /// <param name="SearchDir"> </param>
        /// <param name="Recurse"> </param>
        /// <param name="SearchPattern"> </param>
        public static DataTable GetDirectoryTable(this string SearchDir, bool Recurse = true, string SearchPattern = "*.*")
        {
            _AHK ahk = new _AHK();
            return ahk.GetDirectoryTable(SearchDir, Recurse, SearchPattern);
        }

        /// <summary>Returns Directory size in Formatted Bytes FileSize Text</summary>
        /// <param name="DirPath">Path of Directory to Return Size</param>
        public static string DirSize(this string DirPath, bool FormatBytes = true)
        {
            // 1.
            // Get array of all file names.
            string[] a = Directory.GetFiles(DirPath, "*.*", System.IO.SearchOption.AllDirectories);

            // 2.
            // Calculate total bytes of all files in a loop.
            long b = 0;
            foreach (string name in a)
            {
                // 3.
                // Use FileInfo to get length of each file.
                FileInfo info = new FileInfo(name);
                b += info.Length;
            }

            string size = b.ToString();

            if (FormatBytes)
            {
                _AHK ahk = new _AHK();
                string sizeF = ahk.FormatBytes(b);
                return sizeF;
            }
            return b.ToString();
        }

        /// <summary>Returns true if path is a valid Directory Path</summary>
        /// <param name="FolderPath"> </param>
        public static bool IsDir(this string FolderPath)  // returns true if path is a directory
        {
            _AHK ahk = new _AHK();
            return ahk.isDir(FolderPath);
        }

        /// <summary>Returns the next available file name in a folder, incrementing with "File (FileNumber).ext" Format</summary>
        /// <param name="FilePath">Original File Name</param>
        public static string NextFileName(this string FilePath, int LeadingZeroCount = 0)
        {
            _AHK ahk = new _AHK();
            return ahk.NextFileName(FilePath, LeadingZeroCount);
        }



        // File Lists

        /// <summary>
        /// Returns List<string> of files in directory path
        /// </summary>
        /// <param name="DirPath"> </param>
        /// <param name="SearchPattern"> </param>
        /// <param name="Recurse"> </param>
        /// <param name="FileNameOnly"> </param>
        /// <param name="IncludeExt"> </param>
        public static List<string> FileList(this string DirPath, string SearchPattern = "*.*", bool Recurse = true, bool FileNameOnly = false, bool IncludeExt = true)
        {
            _AHK ahk = new _AHK();
            return ahk.FileList(DirPath, SearchPattern, Recurse, FileNameOnly, IncludeExt);
        }

        /// <summary>
        /// Returns List of Folders in Directory Path
        /// </summary>
        /// <param name="DirPath"> </param>
        /// <param name="SearchPattern"> </param>
        /// <param name="Recurse"> </param>
        /// <param name="FullPathReturn">Option to return either the Full Directory Paths (true) or the Directory name list (false)</param>
        public static List<string> DirList(this string DirPath, string SearchPattern = "*.*", bool Recurse = true, bool FullPathReturn = false)
        {
            _AHK ahk = new _AHK();
            return ahk.DirList(DirPath, SearchPattern, Recurse, FullPathReturn);
        }


        /// <summary>
        /// Format String Removing Illegal Characters - Allowed to Save As File In Windows
        /// </summary>
        /// <param name="FileName"></param>
        /// <param name="ReplaceChar">Character to replace illegal characters with, Default = Space</param>
        /// <returns></returns>
        public static string SafeSaveName(this string FileName, string ReplaceChar = " ")
        {
            _AHK ahk = new _AHK();
            return ahk.SafeSaveName(FileName, ReplaceChar);
        }


        /// <summary>
        /// Parse FileName, Reformatting Words to List Tag List
        /// </summary>
        /// <param name="FilePath">FilePath</param>
        /// <returns></returns>
        public static List<string> FileKeywords(this string FilePath)
        {
            // format keywords
            string keys = FileNameNoExt(FilePath);
            keys = keys.Replace("-", " "); keys = keys.Replace("_", " ");
            keys = keys.Replace(".", " "); keys = keys.Replace(",", " ");
            keys = keys.Replace("`", ""); keys = keys.Replace("'", "");
            keys = keys.Replace("(", " "); keys = keys.Replace(")", " ");
            keys = keys.Replace("!", ""); keys = keys.Replace("#", "");
            keys = keys.Replace("%", ""); keys = keys.Replace("$", "");
            keys = keys.Replace("&", ""); keys = keys.Replace(";", " ");
            keys = keys.Replace("^", ""); keys = keys.Replace("@", "");

            keys = keys.Replace("0", ""); keys = keys.Replace("1", "");
            keys = keys.Replace("2", ""); keys = keys.Replace("3", "");
            keys = keys.Replace("4", ""); keys = keys.Replace("5", "");
            keys = keys.Replace("6", ""); keys = keys.Replace("7", "");
            keys = keys.Replace("8", ""); keys = keys.Replace("9", "");

            keys = keys.Replace("[", ""); keys = keys.Replace("]", "");
            keys = keys.Replace("¦", ""); keys = keys.Replace("˜", "");
            keys = keys.Replace("¿", ""); keys = keys.Replace("+", "");
            keys = keys.Replace("Ñ", "N"); keys = keys.Replace("·", "");
            keys = keys.Replace("Ñ", "N"); keys = keys.Replace("=", "");
            keys = keys.Replace("»", ""); keys = keys.Replace("¬", "");
            keys = keys.Replace("°", ""); keys = keys.Replace("•", "");
            keys = keys.Replace("½", " Half "); keys = keys.Replace("²", "");
            keys = keys.Replace("À", "A"); keys = keys.Replace("  ", " ");
            keys = keys.Trim();
            keys = keys.ToLower(); keys = keys.ToTitleCase();


            List<string> keywords = StringSplit_List(keys, " ");

            return keywords;
        }








        // === File Backup ===

        //private static bool GlobalDebug = false;

        /// <summary>Restores Last Backup File Copy in BackupDir to Original Location</summary>
        /// <param name="FilePath">Path of Original File Previously Backed Up</param>
        /// <param name="Prompt">Option to Prompt User Before Restoring Backup</param>
        /// <param name="BackupDir">Directory To Store Backup File. Default = AppDir\\Backup</param>
        public static bool Restore_Backup(string FilePath, bool Prompt = false, string BackupDir = "\\Backup")
        {
            _AHK ahk = new _AHK();
            return ahk.Restore_Backup(FilePath, Prompt, BackupDir);
        }

        /// <summary>Backup File to Backup Dir with .# ext (also puts \Backup\OriginalName.ext as most recent file copy for opening)</summary>
        /// <param name="FilePath">Path of Original File To Backup</param>
        /// <param name="DeleteOriginal">Option to Delete Original File After Successful Backup</param>
        /// <param name="BackupDir">Directory To Store Backup File. Default = AppDir\\Backup</param>
        /// <returns>Returns True on Successful Backup</returns>
        public static bool Backup_File(string FilePath, bool DeleteOriginal = false, string BackupDir = "\\Backup")
        {
            _AHK ahk = new _AHK();
            return ahk.Backup_File(FilePath, DeleteOriginal, BackupDir);
        }

        /// <summary>Returns Path of the Last Backup File Created for this File Name</summary>
        /// <param name="FilePath">Path of Original File To Backup</param>
        /// <param name="BackupDir">Directory To Store Backup File. Default = AppDir\\Backup</param>
        /// <param name="UseSameNameMostRecent">Overrides Using .# Backup Uses Most Recent Copy of File. Default = True</param>
        public static string Last_Backup_File(string FilePath, string BackupDir = "\\Backup", bool UseSameNameMostRecent = true)
        {
            _AHK ahk = new _AHK();
            return ahk.Last_Backup_File(FilePath, BackupDir, UseSameNameMostRecent);
        }

        /// <summary>Backup File + Display New Backup File Location on GUI</summary>
        /// <param name="FilePath">Path of Original File To Backup</param>
        /// <param name="DisplayControl">WinForm Control Name To Display Backup File Path Returned</param>
        /// <param name="UseSameNameMostRecent">Overrides Using .# Backup Uses Most Recent Copy of File. Default = True</param>
        public static bool Backup_File_Display(string FilePath, Control DisplayControl, bool UseSameNameMostRecent = true)
        {
            _AHK ahk = new _AHK();
            return ahk.Backup_File_Display(FilePath, DisplayControl, UseSameNameMostRecent);
        }







    }
}
