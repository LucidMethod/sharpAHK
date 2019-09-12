using Microsoft.VisualBasic.Devices;
using Microsoft.VisualBasic.FileIO;
using System;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Security.Cryptography;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace sharpAHK
{
    public partial class _AHK 
    {

        #region === Backup ===

        private bool GlobalDebug = false;

        /// <summary>Restores Last Backup File Copy in BackupDir to Original Location</summary>
        /// <param name="FilePath">Path of Original File Previously Backed Up</param>
        /// <param name="Prompt">Option to Prompt User Before Restoring Backup</param>
        /// <param name="BackupDir">Directory To Store Backup File. Default = AppDir\\Backup</param>
        public bool Restore_Backup(string FilePath, bool Prompt = false, string BackupDir = "\\Backup")
        {
            if (Prompt)  // option to prompt user before restoring file from backup 
            {
                bool RestoreNow = false;

                string fileName = FileName(FilePath);
                var ResultValue = YesNoBox("Restore " + fileName + "?", "Restore Backup Now?");
                if (ResultValue.ToString() == "Yes") { RestoreNow = true; }

                if (!RestoreNow) { return false; }
            }


            string RestoreFile = Last_Backup_File(FilePath, BackupDir, false);

            Backup_File(FilePath);  // backup current file before restoring previous

            bool Restored = FileCopy(RestoreFile, FilePath, true);

            if (GlobalDebug)
            {
                if (!Restored) { MsgBox("Error Restoring: " + RestoreFile + "To: " + FilePath); }
                if (Restored) { MsgBox("Restored: " + RestoreFile + "To: " + FilePath); }
            }

            return Restored;
        }

        /// <summary>Backup File to Backup Dir with .# ext (also puts \Backup\OriginalName.ext as most recent file copy for opening)</summary>
        /// <param name="FilePath">Path of Original File To Backup</param>
        /// <param name="DeleteOriginal">Option to Delete Original File After Successful Backup</param>
        /// <param name="BackupDir">Directory To Store Backup File. Default = AppDir\\Backup</param>
        /// <returns>Returns True on Successful Backup</returns>
        public bool Backup_File(string FilePath, bool DeleteOriginal = false, string BackupDir = "\\Backup")
        {
            string SaveDir = FileDir(FilePath) + "\\Backup";

            if (BackupDir != "" && BackupDir != "\\Backup") { SaveDir = BackupDir; }

            FileCreateDir(SaveDir); // ensure backup directory is created

            int FileCount = 1;

            string newFileName = FileName(FilePath);
            string BackupFileName = SaveDir + "\\" + newFileName;

            bool FileBackedUp = false;
            bool FreeNameFound = false;
            do
            {
                string saveBackupFileName = BackupFileName + "." + FileCount;

                if (File.Exists(saveBackupFileName)) { FreeNameFound = false; FileCount++; continue; }

                if (!File.Exists(saveBackupFileName))
                {
                    FreeNameFound = true;
                    FileBackedUp = FileCopy(FilePath, saveBackupFileName, true);
                    FileCopy(FilePath, BackupFileName, true);
                }


            } while (FreeNameFound == false);


            if (GlobalDebug) { if (!FileBackedUp) { MsgBox("Error Copying " + FilePath + Environment.NewLine + "To BackupDir: " + SaveDir); } }

            if (DeleteOriginal)
            {
                if (FileBackedUp) { FileDelete(FilePath); }
            }

            return FileBackedUp;
        }

        /// <summary>Returns Path of the Last Backup File Created for this File Name</summary>
        /// <param name="FilePath">Path of Original File To Backup</param>
        /// <param name="BackupDir">Directory To Store Backup File. Default = AppDir\\Backup</param>
        /// <param name="UseSameNameMostRecent">Overrides Using .# Backup Uses Most Recent Copy of File. Default = True</param>
        public string Last_Backup_File(string FilePath, string BackupDir = "\\Backup", bool UseSameNameMostRecent = true)
        {
            string SaveDir = FileDir(FilePath) + "\\Backup";
            if (BackupDir != "" && BackupDir != "\\Backup") { SaveDir = BackupDir; }

            List<string> matches = FileList(SaveDir, "*.*", false);

            string newFileName = FileName(FilePath);
            string BackupFileName = SaveDir + "\\" + newFileName;

            int FileCount = 1;
            bool FoundLastFile = false;
            string LastBackFile = "";

            do
            {
                // overrides the .number backup - uses most recent copy of file (overwrites every backup)
                if (UseSameNameMostRecent) { return BackupFileName; }

                string saveBackupFileName = BackupFileName + "." + FileCount;

                if (File.Exists(saveBackupFileName)) { FoundLastFile = false; LastBackFile = saveBackupFileName; FileCount++; continue; }

                if (!File.Exists(saveBackupFileName))
                {
                    return LastBackFile;
                }

                FoundLastFile = true;

            } while (FoundLastFile == false);


            if (GlobalDebug) { MsgBox("Found " + matches.Count().ToString() + " Backup Matches"); }


            return LastBackFile;
        }

        /// <summary>Backup File + Display New Backup File Location on GUI</summary>
        /// <param name="FilePath">Path of Original File To Backup</param>
        /// <param name="DisplayControl">WinForm Control Name To Display Backup File Path Returned</param>
        /// <param name="UseSameNameMostRecent">Overrides Using .# Backup Uses Most Recent Copy of File. Default = True</param>
        public bool Backup_File_Display(string FilePath, Control DisplayControl, bool UseSameNameMostRecent = true)
        {
            if (!File.Exists(FilePath)) { MsgBox(FilePath + " Not Found - Unable to Back Up File!"); return false; }

            bool BackedUp = Backup_File(FilePath);  // backup design file before changing

            string LatestBackupFile = Last_Backup_File(FilePath, "", UseSameNameMostRecent);  // return the name of the last backup file for this project file

            DisplayControl.Text = LatestBackupFile;

            return BackedUp;
        }


        #endregion


        #region === File ===

        /// <summary>Writes text to the end of a file (first creating the file, if necessary).</summary>
        /// <param name="Text">The text to append to the file. This text may include linefeed characters (`n) to start new lines.</param>
        /// <param name="FileName">The name of the file to be appended, which is assumed to be in %A_WorkingDir% if an absolute path isn't specified.</param>
        /// <param name="Mode">Mode 0 = Default using AHK FileAppend command | Mode 1 = Using StreamWriter to write Text</param>
        public bool FileAppend(string Text, string FileName, int Mode = 1)
        {
            if (Mode == 0) // Default mode using AHK FileAppend Command
            {
                string AHKLine = "FileAppend, " + Text + ", " + FileName;  // ahk line to execute
                ErrorLog_Setup(true); // ErrorLevel Detection Enabled for this function in AHK 
                Execute(AHKLine);   // execute AHK code and return variable value
                if (!ahkGlobal.ErrorLevel) { return true; } // no error level - return true for success
            }

            if (Mode == 1) // Optional mode to write using C# stream writer
            {
                try
                {
                    using (StreamWriter writer = new StreamWriter(FileName, true))
                    {
                        writer.WriteLine(Text);
                    }
                    return true;
                }
                catch (IOException ex)
                {
                    return false;
                }
                catch (ArgumentException e)
                {
                    ahkGlobal.ErrorLevel = true;
                    MessageBox.Show("Unable To Write to " + FileName + "\n\r\n\r" + e.ToString());
                    return false;
                }
            }


            return false;  // error level detected - success = false
        }

        /// <summary>Copies one or more files.</summary>
        /// <param name="SourcePattern">The name of a single file or folder, or a wildcard pattern such as C:\Temp\*.tmp. SourcePattern is assumed to be in WorkingDir if an absolute path isn't specified.</param>
        /// <param name="DestPattern">The name or pattern of the destination, which is assumed to be in %A_WorkingDir% if an absolute path isn't specified.</param>
        /// <param name="OverWrite">Flag determines whether to overwrite files if they already exist. True = OverWrite Existing Files</param>
        /// <param name="ProgressDialog">Option to Display Windows FileCopy Dialog While File Transfers</param>
        public bool FileCopy(string SourcePattern, string DestPattern, bool OverWrite = false, bool ProgressDialog = false)
        {
            //=== AHK FileCopy Method (Default Method) ====
            if (!ProgressDialog)
            {
                string sourcePattern = SourcePattern.Replace(",", "`,");
                string destPattern = DestPattern.Replace(",", "`,");

                int overWrite = 0; if (OverWrite) { overWrite = 1; }  // change overwrite bool to 1/0 
                string AHKLine = "FileCopy, " + sourcePattern + ", " + destPattern + ", " + overWrite;  // ahk line to execute
                ErrorLog_Setup(true, "Error Copying [ELV] Files:"); // ErrorLevel Detection Enabled for this function in AHK 
                Execute(AHKLine);   // execute AHK code and return variable value            

                if (!ahkGlobal.ErrorLevel) { return true; } // no error level - return true for success
                return false;  // error level detected - success = false
            }

            //=== C# FileCopy Method - With Progress Dialog ===
            else
            {
                try
                {
                    Computer comp = new Computer();
                    comp.FileSystem.CopyFile(SourcePattern, DestPattern, Microsoft.VisualBasic.FileIO.UIOption.AllDialogs, UICancelOption.DoNothing);
                }
                catch (Exception ex)
                {
                    MsgBox(ex.ToString());
                    return false;
                }
                return true;
            }

            // Alternative FileCopy Method in C# (No Progress Dialog)
            /*
                        //=== C# FileCopy Method ===
                        try { File.Copy(SourcePattern, DestPattern, OverWrite); }
                        catch (Exception ex)
                        {
                            MsgBox(ex.ToString()); 
                            return false;
                        }
                        return true;
            */
        }

        /// <summary>
        /// Function used by FileCopy function to copy list of files to new location
        /// </summary>
        /// <param name="source"></param>
        /// <param name="target"></param>
        /// <param name="OverWrite"></param>
        private static void _CopyAll(DirectoryInfo source, DirectoryInfo target, bool OverWrite = true)
        {
            Directory.CreateDirectory(target.FullName);

            // Copy each file into the new directory.
            foreach (FileInfo fi in source.GetFiles())
            {
                Console.WriteLine(@"Copying {0}\{1}", target.FullName, fi.Name);
                fi.CopyTo(Path.Combine(target.FullName, fi.Name), OverWrite);
            }

            // Copy each subdirectory using recursion.
            foreach (DirectoryInfo diSourceSubDir in source.GetDirectories())
            {
                DirectoryInfo nextTargetSubDir =
                    target.CreateSubdirectory(diSourceSubDir.Name);
                _CopyAll(diSourceSubDir, nextTargetSubDir);
            }
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
        public bool FileCreateShortcut(string Target, string LinkFile, string WorkingDir = "", string Args = "", string Description = "", string IconFile = "", string ShortcutKey = "", string IconNumber = "", string RunState = "")
        {
            string AHKLine = "FileCreateShortcut, " + Target + "," + LinkFile + "," + WorkingDir + "," + Args + "," + Description + "," + IconFile + "," + ShortcutKey + "," + IconNumber + "," + RunState;  // ahk line to execute
            ErrorLog_Setup(true, "Error Creating Shortcut For: " + Target); // ErrorLevel Detection Enabled for this function in AHK 
            Execute(AHKLine);   // execute AHK code and return variable value 

            if (!ahkGlobal.ErrorLevel) { return true; } // no error level - return true for success
            return false;  // error level detected - success = false

            // v1 ToDo
            /*
                    void _appShortcutToDesktop(string linkName)  // creates a link from the current application to the user's desktop as an app link
                    {
                        string deskDir = Environment.GetFolderPath(Environment.SpecialFolder.DesktopDirectory);

                        string LinkFile = deskDir + "\\" + linkName + ".url";
                        FileDelete(LinkFile);

                        string app = System.IO.Path.GetDirectoryName(Environment.GetCommandLineArgs()[0]) + "\\" + Assembly.GetCallingAssembly().GetName().Name.ToString() + ".exe";

                        using (StreamWriter writer = new StreamWriter(deskDir + "\\" + linkName + ".url"))
                        {
                            writer.WriteLine("[InternetShortcut]");
                            writer.WriteLine("URL=file:///" + app);
                            writer.WriteLine("IconIndex=0");
                            string icon = app.Replace('\\', '/');
                            writer.WriteLine("IconFile=" + icon);
                            writer.Flush();
                        }

                        MsgBox(app);
                    }
             */

        }

        /// <summary>
        /// Adds shortcut to application for current executable in user's startup directory  
        /// </summary>
        public void AppShortcutToStartup()
        {
            string AppName = Assembly.GetCallingAssembly().GetName().Name.ToString();
            string ShortCutPath = Environment.GetFolderPath(Environment.SpecialFolder.DesktopDirectory) + "\\" + AppName + ".lnk";
            string AppPath = System.IO.Path.GetDirectoryName(Environment.GetCommandLineArgs()[0]) + "\\" + Assembly.GetCallingAssembly().GetName().Name.ToString() + ".exe";
            string IconPath = "shell32.dll";
            string IconNum = "1";

            Type t = Type.GetTypeFromCLSID(new Guid("72C24DD5-D70A-438B-8A42-98424B88AFB8")); //Windows Script Host Shell Object
            dynamic shell = Activator.CreateInstance(t);
            try
            {
                var lnk = shell.CreateShortcut(ShortCutPath);
                try
                {
                    lnk.TargetPath = AppPath;
                    lnk.IconLocation = IconPath + "," + IconNum;
                    lnk.Save();
                }
                finally
                {
                    Marshal.FinalReleaseComObject(lnk);
                }
            }
            finally
            {
                Marshal.FinalReleaseComObject(shell);
            }
        }

        /// <summary>
        /// Creates URL shortcut on user's pc (default location = desktop)
        /// </summary>
        /// <param name="linkName">Name of URL ShortCut/Site</param>
        /// <param name="linkUrl">URL for new Link</param>
        /// <param name="SaveDir">Directory to save new link to</param>
        public void UrlShortcutToDesktop(string linkName, string linkUrl, string SaveDir = "Desktop")
        {
            string deskDir = Environment.GetFolderPath(Environment.SpecialFolder.DesktopDirectory);

            if (SaveDir.ToUpper() == "DESKTOP") { deskDir = Environment.GetFolderPath(Environment.SpecialFolder.DesktopDirectory); }
            else { deskDir = SaveDir; }

            using (StreamWriter writer = new StreamWriter(deskDir + "\\" + linkName + ".url"))
            {
                writer.WriteLine("[InternetShortcut]");
                writer.WriteLine("URL=" + linkUrl);
                writer.Flush();
            }
        }

        /// <summary>Deletes one or more files.</summary>
        /// <param name="FilePattern">The name of a single file or a wildcard pattern such as C:\Temp\*.tmp. FilePattern is assumed to be in %A_WorkingDir% if an absolute path isn't specified.</param>
        /// <param name="PromptUser">Option to Prompt User with Yes/No PopUp Dialog Before Deleting (Default = False)</param>
        public bool FileDelete(string FilePattern, bool PromptUser = false)
        {
            if (PromptUser)
            {
                var ResultValue = YesNoBox("Delete " + FilePattern + "?", "Delete File?");
                if (ResultValue.ToString() != "Yes") { return false; }
            }


            string filePattern = FilePattern.Replace(",", "`,");
            string AHKLine = "FileDelete, " + filePattern;  // ahk line to execute
            ErrorLog_Setup(true, "Error Deleting: " + filePattern); // ErrorLevel Detection Enabled for this function in AHK 
            Execute(AHKLine);   // execute AHK code and return variable value 

            if (!ahkGlobal.ErrorLevel) { return true; } // no error level - return true for success
            return false;  // error level detected - success = false


            // v1 ToDo
            /*
                            try
                            {
                                File.Delete(FileToDelete); // Try to delete the file.
                                return true;
                            }
                            catch (IOException ex)
                            {
                                if (Debug) { _MsgBox("Unable to Delete " + FileToDelete + Environment.NewLine + ex.ToString()); }
                                return false;  // We could not delete the file.
                            }
            */

        }

        /// <summary>Reports whether a file or folder is read-only, hidden, etc.</summary>
        /// <param name="Filename">The name of the target file, which is assumed to be in %A_WorkingDir% if an absolute path isn't specified.</param>        
        public string FileGetAttrib(string Filename = "")
        {
            string filename = Filename.Replace(",", "`,");
            string AHKLine = "FileGetAttrib, OutputVar, " + filename;  // ahk line to execute
            ErrorLog_Setup(true, "Error Returning Attributes: " + filename); // ErrorLevel Detection Enabled for this function in AHK 
            string OutVar = Execute(AHKLine, "OutputVar");   // execute AHK code and return variable value 
            return OutVar;
        }

        /// <summary>Retrieves information about a shortcut (.lnk) file, such as its target file.</summary>
        /// <param name="LinkFile"></param>        
        public shortCutInfo FileGetShortcut(string LinkFile)
        {
            string AHKLine = "FileGetShortcut, " + LinkFile + ",OutTarget,OutDir,OutArgs,OutDescription,OutIcon,OutIconNum,OutRunState";  // ahk line to execute
            ErrorLog_Setup(true, "Error Reading Shortcut: " + LinkFile); // ErrorLevel Detection Enabled for this function in AHK 
            Execute(AHKLine);   // execute AHK code and return variable value 

            shortCutInfo sInfo = new shortCutInfo();

            //return value from ahk session - populate return object
            string OutputVar = "";
            sInfo.Target = GetVar("OutTarget");
            sInfo.Dir = GetVar("OutDir");
            sInfo.Args = GetVar("OutArgs");
            sInfo.Description = GetVar("OutDescription");
            sInfo.Icon = GetVar("OutIcon");
            sInfo.IconNum = GetVar("OutIconNum");
            sInfo.RunState = GetVar("OutRunState");

            if (sInfo.RunState == "1") { sInfo.RunState = "Normal"; }
            if (sInfo.RunState == "3") { sInfo.RunState = "Maximized"; }
            if (sInfo.RunState == "7") { sInfo.RunState = "Minimized"; }

            // return all variables in 1 string
            //OutputVar = OutputVar + " | " + OutTarget + " | " + OutDir + " | " + OutArgs + " | " + OutDescription + " | " + OutIcon + " | " + OutIconNum + " | " + OutRunState;
            //return OutputVar; 

            return sInfo;
        }

        /// <summary>Object contains return values from FileGetShortcut command</summary>
        public struct shortCutInfo
        {
            public string Target { get; set; }
            public string Dir { get; set; }
            public string Args { get; set; }
            public string Description { get; set; }
            public string Icon { get; set; }
            public string IconNum { get; set; }
            public string RunState { get; set; }
        }

        /// <summary>Read File, Return File Bytes</summary>
        /// <param name="filePath">Path to file to convert to bytes</param>    
        public byte[] FileBytes(string filePath)
        {
            // convert file to binary to store in sql db
            byte[] file;
            using (var stream = new FileStream(filePath, FileMode.Open, FileAccess.Read))
            {
                using (var reader = new BinaryReader(stream))
                {
                    file = reader.ReadBytes((int)stream.Length);
                }
            }
            return file;
        }


        /// <summary>Retrieves the size of a file.</summary>
        /// <param name="Filename">The name of the target file, which is assumed to be in %A_WorkingDir% if an absolute path isn't specified.</param>
        /// <param name="Units">If present, this parameter causes the result to be returned in units other than bytes: K = kilobytes | M = megabytes</param>
        public string FileGetSize(string Filename = "", string Units = "")
        {
            string filename = Filename.Replace(",", "`,");
            string AHKLine = "FileGetSize, OutputVar, " + filename + "," + Units;  // ahk line to execute
            ErrorLog_Setup(true, "Error Returning Size For : " + filename); // ErrorLevel Detection Enabled for this function in AHK 
            string OutVar = Execute(AHKLine, "OutputVar");   // execute AHK code and return variable value 
            return OutVar;
        }

        ///// <summary>Retrieves the size of a folder.</summary>
        ///// <param name="DirPath">Directory to poll for current disk size.</param>
        ///// <param name="Units">If present, this parameter causes the result to be returned in units other than bytes: K = kilobytes | M = megabytes</param>
        //public string DirSize(string DirPath, string Units = "")
        //{
        //    string AHKLine = "Loop, %" + DirPath + "%\\*.*, , 1`nOutputVar += %A_LoopFileSize%";

        //    ErrorLog_Setup(true, "Error Returning Size For : " + DirPath); // ErrorLevel Detection Enabled for this function in AHK 
        //    string OutVar = Execute(AHKLine, "OutputVar");   // execute AHK code and return variable value 
        //    return OutVar;
        //}


        /// <summary>
        /// Returns Directory size in Formatted Bytes FileSize Text
        /// </summary>
        /// <param name="DirPath">Path of Directory to Return Size</param>
        /// <param name="FormatBytes">Option to Convert Return Value from Bytes to Formated. Ex: 4MB</param>
        /// <param name="Recursive">Option to Search SubDirs for Total Folder Size</param>
        /// <returns></returns>
        public string DirSize(string DirPath, bool FormatBytes = true, bool Recursive = true)
        {
            if (!Directory.Exists(DirPath)) { return ""; }

            // 1.
            // Get array of all file names.
            string[] a = Directory.GetFiles(DirPath, "*.*", System.IO.SearchOption.TopDirectoryOnly);
            if (Recursive) { a = Directory.GetFiles(DirPath, "*.*", System.IO.SearchOption.AllDirectories); }

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


        /// <summary>
        /// Returns List of Folders with FolderSize 
        /// </summary>
        /// <param name="RootDir"></param>
        /// <param name="FormatBytes"></param>
        /// <param name="Recursive"></param>
        /// <param name="OutFile"></param>
        /// <returns></returns>
        public string DirSizeReport(string RootDir, bool FormatBytes = true, bool Recursive = false, string OutFile = "")
        {
            string Out = "DIRSIZE REPORT\n\r\n\r";

            List<string> dirs = DirList(RootDir, "*.*", Recursive, true);

            foreach(string dir in dirs)
            {
                string size = DirSize(dir, FormatBytes, true);
                Out = Out + "\n" + dir + " (" + size + ")\n\r";
            }

            if (OutFile != "")
            {
                FileDelete(OutFile);
                FileAppend(Out, OutFile);
                Run(OutFile);
            }

            return Out;
        }


        /// <summary>Retrieves the datetime stamp of a file or folder.</summary>
        /// <param name="Filename">The name of the target file, which is assumed to be in %A_WorkingDir% if an absolute path isn't specified.</param>
        /// <param name="WhichTime">Which timestamp to retrieve: M = Modification time (default if omitted) | C = Creation time | A = Last access time   </param>        
        public string FileGetTime(string Filename = "", string WhichTime = "")
        {
            string filename = Filename.Replace(",", "`,");
            string AHKLine = "FileGetTime, OutputVar, " + filename + "," + WhichTime;  // ahk line to execute
            ErrorLog_Setup(true, "Error Returning File Time : " + filename); // ErrorLevel Detection Enabled for this function in AHK 
            string OutVar = Execute(AHKLine, "OutputVar");   // execute AHK code and return variable value 
            return OutVar;
        }

        /// <summary>Retrieves the version of a file.</summary>
        /// <param name="Filename">The name of the target file, which is assumed to be in %A_WorkingDir% if an absolute path isn't specified.</param>
        public string FileGetVersion(string Filename = "")
        {
            string AHKLine = "FileGetVersion, OutputVar, " + Filename;  // ahk line to execute
            ErrorLog_Setup(true, "Error Returning File Version: " + Filename); // ErrorLevel Detection Enabled for this function in AHK 
            string OutVar = Execute(AHKLine, "OutputVar");   // execute AHK code and return variable value 
            return OutVar;
        }

        //FileInstall - Skipped

        /// <summary>Moves or renames one or more files.</summary>
        /// <param name="SourcePattern">The name of a single file or a wildcard pattern such as C:\Temp\*.tmp. SourcePattern is assumed to be in %A_WorkingDir% if an absolute path isn't specified.</param>
        /// <param name="DestPattern">The name or pattern of the destination, which is assumed to be in %A_WorkingDir% if an absolute path isn't specified. To perform a simple move -- retaining the existing file name(s) -- specify only the folder name as shown in these functionally identical examples: FileMove, C:\*.txt, C:\My Folder</param>
        /// <param name="OverWrite">Determines whether to overwrite files if they already exist</param>
        public bool FileMove(string SourcePattern, string DestPattern, bool OverWrite = false)
        {
            // change overwrite bool to 1/0 
            int overWrite = 0; if (OverWrite) { overWrite = 1; }

            string source = SourcePattern.Replace(",", "`,");
            string dest = DestPattern.Replace(",", "`,");

            //string source = StringReplace(SourcePattern, ",", "`,", "ALL"); // fix illegal ahk chars

            string AHKLine = "FileMove, " + source + ", " + dest + ", " + overWrite;  // ahk line to execute
            ErrorLog_Setup(true, "Error Moving [ELV] Files"); // ErrorLevel Detection Enabled for this function in AHK 
            Execute(AHKLine);   // execute AHK code and return variable value

            if (!ahkGlobal.ErrorLevel) { return true; } // no error level - return true for success
            return false;  // error level detected - success = false

            // v1 ToDo
            /*
                    bool _FileMove(string FileToMove, string FileDestination, bool OverWrite = true)  // moves file from one location to another
                    {
                        if (File.Exists(FileToMove))
                        {
                            if (OverWrite == true)  // delete the existing destination file in order to overwrite with new file
                            {
                                if (File.Exists(FileDestination))
                                {
                                    File.Delete(FileDestination); // Try to delete the file.
                                }
                            }

                            try
                            {
                                File.Move(FileToMove, FileDestination); // Try to move
                                return true;
                            }
                            catch (IOException ex)
                            {
                                //Console.WriteLine(ex); // Write error
                                return false;
                            }
                        }

                        return false;
                    }
            */
        }

        /// <summary>Renames one or more files (same as FileMove)</summary>
        /// <param name="SourcePattern">The name of a single file or a wildcard pattern such as C:\Temp\*.tmp. SourcePattern is assumed to be in %A_WorkingDir% if an absolute path isn't specified.</param>
        /// <param name="DestPattern">The name or pattern of the destination, which is assumed to be in %A_WorkingDir% if an absolute path isn't specified. To perform a simple move -- retaining the existing file name(s) -- specify only the folder name as shown in these functionally identical examples: FileMove, C:\*.txt, C:\My Folder</param>
        /// <param name="OverWrite">Determines whether to overwrite files if they already exist</param>
        public bool FileRename(string SourcePattern, string DestPattern, bool OverWrite = false)
        {
            return FileMove(SourcePattern, DestPattern, OverWrite);

            // v1 ToDo
            /*
                    bool _FileRename(string FileToRename, string NewFileName, bool OverWrite = true)  // renames file on pc
                    {
                        // same as FileMove -- another copy

                        if (File.Exists(FileToRename))
                        {
                            if (OverWrite == true)  // delete the existing destination file in order to overwrite with new file
                            {
                                if (File.Exists(NewFileName))
                                {
                                    File.Delete(NewFileName); // Try to delete the file.
                                }
                            }

                            try
                            {
                                File.Move(FileToRename, NewFileName); // Try to move
                                return true;
                            }
                            catch (IOException ex)
                            {
                                //Console.WriteLine(ex); // Write error
                                return false;
                            }
                        }

                        return false;
                    }
            */

        }

        /// <summary>
        /// Rename Folder Path
        /// </summary>
        /// <param name="SourceDir"></param>
        /// <param name="DestDir"></param>
        /// <returns></returns>
        public bool DirRename(string SourceDir, string DestDir)
        {
            try
            {
                Directory.Move(SourceDir, DestDir);
                return true;
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
                return false;
            }
        }

        /// <summary>
        /// Moves all files in subfolders to the RootDirPath, removes empty folders
        /// </summary>
        /// <param name="RootDirPath">Top Folder Containing Subfolders to Move All Files Into</param>
        /// <param name="OverWriteDupes">Option to OverWrite Existing Files in RootDirPath with Same Name</param>
        public void FileMoveToRoot(string RootDirPath, bool OverWriteDupes = true)
        {
            if (!Directory.Exists(RootDirPath)) { return; }

            string[] a = Directory.GetFiles(RootDirPath, "*.*", System.IO.SearchOption.AllDirectories);

            foreach(string file in a)
            {
                FileMove(file, RootDirPath + "\\" + FileName(file), OverWriteDupes);
            }

            RemoveEmptyDirs(RootDirPath);
        }

        /// <summary>
        /// Loops through RootDirPath and Removes SubFolders that don't contain any files
        /// </summary>
        /// <param name="RootDirPath"></param>
        /// <returns></returns>
        public int RemoveEmptyDirs(string RootDirPath)
        {
            int removed = 0;

            List<string> dirs = DirList(RootDirPath, "*.*", true, true);
            if (dirs != null && dirs.Count > 0)
            {
                foreach (string dir in dirs)
                {
                    List<string> files = FileList(dir, "*.*", false);
                    if (files != null)
                    {
                        if (files.Count == 0) { FileRemoveDir(dir, true); removed++; }
                    }
                }
            }

            dirs = DirList(RootDirPath, "*.*", true, true);
            if (dirs != null && dirs.Count == 0)
            {
                List<string> files = FileList(RootDirPath, "*.*", true, false, true);
                if (files != null && files.Count == 0)
                {
                    FileRemoveDir(RootDirPath); removed++;
                }
            }

            return removed;
        }


        /// <summary>
        /// Searches Directory for Specific File Format, Returns True if Located in Dir
        /// </summary>
        /// <param name="RootDirPath">Directory to Search</param>
        /// <param name="Format">File Format To Search For (ex: *.txt)</param>
        /// <returns></returns>
        public bool DirContainsFormat(string RootDirPath, string Format = "*.rar")
        {
            if (!Directory.Exists(RootDirPath)) { return false; }
            List<string> files = FileList(RootDirPath, Format, true);
            if (files.Count > 0) { return true; }
            return false;
        }


        /// <summary>
        /// Loop through file pattern, return matches as list of full file paths
        /// </summary>
        /// <param name="FilePattern"></param>
        /// <param name="IncludeFolders"></param>
        /// <param name="Recurse"></param>
        /// <returns></returns>
        public List<string> FileListLoop(string FilePattern, bool IncludeFolders = false, bool Recurse = false)
        {
            // change overwrite bool to 1/0 
            int recurse = 0; if (Recurse) { recurse = 1; }
            int includeFolders = 0; if (IncludeFolders) { includeFolders = 1; }

            string AHKLine = "FileList =\n\rLoop, " + FilePattern + ", " + includeFolders + ", " + recurse + "\n\r{\n\rif FileList != \n\rFileList = %FileList%`n%A_LoopFileFullPath%\n\rif FileList =\n\rFileList = %A_LoopFileFullPath%\n\r}\n\r";
            ErrorLog_Setup(true, "Error Moving [ELV] Files"); // ErrorLevel Detection Enabled for this function in AHK 
            string fileList = Execute(AHKLine, "FileList");   // execute AHK code and return variable value

            List<string> files = Text_ToList(fileList, true, true, false);

            //MsgBox(fileList); 

            //if (!ahkGlobal.ErrorLevel) { return true; } // no error level - return true for success
            return files;
        }

        /// <summary>
        /// Loop Through File Pattern/Folder, Return Number of Files Found
        /// </summary>
        /// <param name="Directory">Path to Folder to Search For FileCount</param>
        /// <param name="SearchPattern">File Pattern to Match For FileCount, Default = *.*</param>
        /// <param name="IncludeFolders">Add # of Directories Found to Total File Count</param>
        /// <param name="Recurse">Option to Include Files Found in Subdirectories. Default = true</param>
        /// <returns></returns>
        public int FileCount(string Directory, string SearchPattern = "*.*", bool IncludeFolders = false, bool Recurse = true)
        {
            if (Directory.Trim() == "") { return 0; }

            string[] filelist = null;

            if (Recurse == false)
            {
                filelist = System.IO.Directory.GetFiles(Directory, SearchPattern, System.IO.SearchOption.TopDirectoryOnly);  // no recurse
            }

            if (Recurse == true)
            {
                filelist = System.IO.Directory.GetFiles(Directory, SearchPattern, System.IO.SearchOption.AllDirectories);  // recurse
            }

            int dirCount = 0;
            int totalCount = filelist.Count();
            if (IncludeFolders)  // option to add folders to total count found in directory
            {
                dirCount = DirCount(Directory, SearchPattern, Recurse);
                totalCount += dirCount;
            }

            return totalCount;
        }

        /// <summary>
        /// Loop Through File Pattern/Folder, Return Number of Directories Found
        /// </summary>
        /// <param name="Directory">Path to Folder to Search For FileCount</param>
        /// <param name="SearchPattern">File Pattern to Match For FileCount, Default = *.*</param>
        /// <param name="Recurse">Option to Include Subdirectories. Default = true</param>
        /// <returns></returns>
        public int DirCount(string Directory, string SearchPattern = "*.*", bool Recurse = true)
        {
            string[] dirlist = null;

            if (Recurse == false)
            {
                dirlist = System.IO.Directory.GetDirectories(Directory, SearchPattern, System.IO.SearchOption.TopDirectoryOnly);
            }

            if (Recurse == true)
            {
                dirlist = System.IO.Directory.GetDirectories(Directory, SearchPattern, System.IO.SearchOption.AllDirectories);
            }

            return dirlist.Count();
        }

        /// <summary>Reads the specified line from a file and stores the text in a variable.</summary>
        /// <param name="Filename">The name of the file to access, which is assumed to be in %A_WorkingDir% if an absolute path isn't specified.</param>
        /// <param name="LineNum">Which line to read (1 is the first, 2 the second, and so on). This can be an expression.</param>
        public string FileReadLine(string Filename, string LineNum)
        {
            string AHKLine = "FileReadLine, OutputVar, " + Filename + "," + LineNum;  // ahk line to execute
            ErrorLog_Setup(true, "Error Reading Line: " + LineNum + " From " + Filename); // ErrorLevel Detection Enabled for this function in AHK 
            string OutVar = Execute(AHKLine, "OutputVar");   // execute AHK code and return variable value 
            return OutVar;

            // v1 ToDo
            /*
                    string _FileReadLine(string FileToRead, int LineNumber)  // reads specific line number in text file to string
                    {
                        if (File.Exists(FileToRead))
                        {
                            string ReturnText = "";

                            // Read every line in the file.
                            using (StreamReader reader = new StreamReader(FileToRead))
                            {
                                string line;
                                int IndexCounter = 0;

                                while ((line = reader.ReadLine()) != null)
                                {
                                    IndexCounter++;

                                    if (IndexCounter == LineNumber)
                                    {
                                        ReturnText = line;
                                        return ReturnText;
                                    }

                                }
                            }

                            return ReturnText;
                        }

                        return "UNABLE TO LOCATE " + FileToRead;
                    }
            */

        }

        /// <summary>Reads a File's Contents into a Variable</summary>
        /// <param name="FilePath">Path to File to Read, Assumed to be in %A_WorkingDir% if Absolute Path isn't Specified</param>
        public string FileRead(string FilePath)
        {
            if (!File.Exists(FilePath)) { return ""; }

            string AHKLine = "FileRead, OutputVar, " + FilePath;  // ahk line to execute
            ErrorLog_Setup(true, "Error Reading: " + FilePath); // ErrorLevel Detection Enabled for this function in AHK 
            string OutVar = Execute(AHKLine, "OutputVar");   // execute AHK code and return variable value 
            return OutVar;

            // v1 ToDo
            /*
                    string _FileRead(string FileToRead)  // reads contents of text file to string
                    {
                        if (File.Exists(FileToRead))
                        {
                            string ReturnText = File.ReadAllText(FileToRead);
                            return ReturnText;
                        }

                        return "UNABLE TO LOCATE " + FileToRead;
                    }
            */

        }

        /// <summary>Sends a file or directory to the recycle bin, if possible.</summary>
        /// <param name="FilePattern">The name of a single file or a wildcard pattern such as C:\Temp\*.tmp. FilePattern is assumed to be in %A_WorkingDir% if an absolute path isn't specified. To recycle an entire directory, provide its name without a trailing backslash.</param>
        public bool FileRecycle(string FilePattern)
        {
            string AHKLine = "FileRecycle, " + FilePattern;  // ahk line to execute
            ErrorLog_Setup(true, "Error Recycling:" + FilePattern); // ErrorLevel Detection Enabled for this function in AHK 
            Execute(AHKLine);   // execute AHK code and return variable value

            if (!ahkGlobal.ErrorLevel) { return true; } // no error level - return true for success
            return false;  // error level detected - success = false
        }

        /// <summary>Empties the recycle bin.</summary>
        /// <param name="DriveLetter">If omitted, the recycle bin for all drives is emptied. Otherwise, specify a drive letter such as C:\</param>        
        public bool FileRecycleEmpty(string DriveLetter = "")
        {
            string AHKLine = "FileRecycleEmpty, " + DriveLetter;  // ahk line to execute
            ErrorLog_Setup(true, "Error Emptying Recycle :" + DriveLetter); // ErrorLevel Detection Enabled for this function in AHK 
            Execute(AHKLine);   // execute AHK code and return variable value

            if (!ahkGlobal.ErrorLevel) { return true; } // no error level - return true for success
            return false;  // error level detected - success = false
        }

        /// <summary>Displays a standard dialog that allows the user to open or save file(s).</summary>
        /// <param name="Options">M: Multi-select. | S: Save button. | 1: File Must Exist | 2: Path Must Exist | 8: Prompt to Create New File | 16: Prompt to OverWrite File | 32: Shortcuts (.lnk files) are selected as-is rather than being resolved to their targets.</param>
        /// <param name="RootDirFileName">RootDir: The root (starting) directory. | Filename: The default filename to initially show in the dialog's edit field.</param>
        /// <param name="Prompt">Text displayed in the window to instruct the user what to do. If omitted or blank, it will default to "Select File - %A_SCRIPTNAME%" (i.e. the name of the current script).</param>
        /// <param name="Filter">Indicates which types of files are shown by the dialog. Example: Documents (*.txt)</param>
        public string FileSelectFile(string Options = "", string RootDirFileName = "", string Prompt = "", string Filter = "")
        {
            string AHKLine = "FileSelectFile, OutputVar, " + Options + ", " + RootDirFileName + ", " + Prompt + ", " + Filter;  // ahk line to execute
            ErrorLog_Setup(true, "ErrorLevel Detected - User Likely Pressed Cancel"); // ErrorLevel Detection Enabled for this function in AHK 
            string OutVar = Execute(AHKLine, "OutputVar");   // execute AHK code and return variable value 
            return OutVar;
        }

        /// <summary>Displays a standard dialog that allows the user to select a folder.</summary>
        /// <param name="StartingFolder">If blank or omitted, the dialog's initial selection will be the user's My Documents folder (or possibly My Computer).</param>
        /// <param name="Options">0: The options are all disabled | 1 (default): A button is provided that allows the user to create new folders. | Add 2 to the above number to provide an edit field that allows the user to type the name of a folder. | Adding 4 ensures that FileSelectFolder will work properly even in a Preinstallation Environment like WinPE or BartPE</param>
        /// <param name="Prompt">Text displayed in the window to instruct the user what to do. If omitted or blank, it will default to "Select Folder - %A_SCRIPTNAME%" (i.e. the name of the current script).</param>
        public string FileSelectFolder(string StartingFolder = "", string Options = "", string Prompt = "")
        {
            string AHKLine = "Gui +OwnDialogs\n\rFileSelectFolder, OutputVar, " + StartingFolder + "," + Options + "," + Prompt;  // ahk line to execute
            ErrorLog_Setup(true, "ErrorLevel Detected - User Likely Pressed Cancel"); // ErrorLevel Detection Enabled for this function in AHK 
            string OutVar = Execute(AHKLine, "OutputVar");   // execute AHK code and return variable value 
            return OutVar;
        }

        /// <summary>Changes the attributes of one or more files or folders. Wildcards are supported.</summary>
        /// <param name="Attributes">+ Turn on the attribute | - Turn off the attribute | ^ Toggle the attribute || R = READONLY | A = ARCHIVE | S = SYSTEM | H = HIDDEN | N = NORMAL | O = OFFLINE | T = TEMPORARY</param>
        /// <param name="FilePattern">The name of a single file or folder, or a wildcard pattern such as C:\Temp\*.tmp. FilePattern is assumed to be in %A_WorkingDir% if an absolute path isn't specified. </param>
        /// <param name="OperateOnFolders">0 (default) Folders are not operated upon (only files). | 1 All files and folders that match the wildcard pattern are operated upon. | 2 Only folders are operated upon (no files)</param>
        /// <param name="Recurse">False (default) Subfolders are not recursed into. | True = Subfolders are recursed into so that files and folders contained therein are operated upon if they match FilePattern. </param>
        public bool FileSetAttrib(string Attributes, string FilePattern = "", string OperateOnFolders = "0", bool Recurse = false)
        {
            string recurse = "0"; if (Recurse) { recurse = "1"; }
            string AHKLine = "FileSetAttrib, " + FilePattern + "," + FilePattern + "," + OperateOnFolders + "," + recurse;  // ahk line to execute
            ErrorLog_Setup(true, "Error Changing [ELV] Files"); // ErrorLevel Detection Enabled for this function in AHK 
            Execute(AHKLine);   // execute AHK code and return variable value

            if (!ahkGlobal.ErrorLevel) { return true; } // no error level - return true for success
            return false;  // error level detected - success = false
        }

        /// <summary>Changes the datetime stamp of one or more files or folders. Wildcards are supported.</summary>
        /// <param name="YYYYMMDDHH24MISS">If blank or omitted, it defaults to the current time. Otherwise, specify the time to use for the operation (see Remarks for the format). Years prior to 1601 are not supported.</param>
        /// <param name="FilePattern">The name of a single file or folder, or a wildcard pattern such as C:\Temp\*.tmp. FilePattern is assumed to be in %A_WorkingDir% if an absolute path isn't specified.</param>
        /// <param name="WhichTime">M = Modification time (this is the default if the parameter is blank or omitted) | C = Creation time | A = Last access time</param>
        /// <param name="OperateOnFolders">0 (default) Folders are not operated upon (only files). | 1 All files and folders that match the wildcard pattern are operated upon. | 2 Only folders are operated upon (no files).</param>
        /// <param name="Recurse">False (default) Subfolders are not recursed into. | True = Subfolders are recursed into so that files and folders contained therein are operated upon if they match FilePattern.</param>        
        public bool FileSetTime(string YYYYMMDDHH24MISS = "", string FilePattern = "", string WhichTime = "", string OperateOnFolders = "", bool Recurse = false)
        {
            string recurse = "0"; if (Recurse) { recurse = "1"; }
            string AHKLine = "FileSetTime, " + YYYYMMDDHH24MISS + "," + FilePattern + "," + WhichTime + "," + OperateOnFolders + "," + recurse;  // ahk line to execute
            ErrorLog_Setup(true, "Error Changing [ELV] Files"); // ErrorLevel Detection Enabled for this function in AHK 
            Execute(AHKLine);   // execute AHK code and return variable value

            if (!ahkGlobal.ErrorLevel) { return true; } // no error level - return true for success
            return false;  // error level detected - success = false
        }

        /// <summary>Checks for the existence of a file or folder.</summary>
        /// <param name="FilePattern">The path, filename, or file pattern to check. FilePattern is assumed to be in %A_WorkingDir% if an absolute path isn't specified.</param>
        public bool IfExist(string FilePattern)
        {
            string AHKLine = "IfExist, " + FilePattern;  // ahk line to execute
            ErrorLog_Setup(false); // ErrorLevel Detection Enabled for this function in AHK 

            //create an autohtkey engine.
            if (ahkGlobal.ahkdll == null) { ahkGlobal.ahkdll = new AutoHotkey.Interop.AutoHotkeyEngine(); }
            var ahkdll = ahkGlobal.ahkdll;

            // execute a function in AHK
            var add5Results = ahkdll.Eval("FileExist(\"" + FilePattern + "\")");

            // if anything but blank is returned, a file was found
            bool found = false;
            if (add5Results.ToString() != "") { found = true; }

            return found;

            // v1 ToDo
            /*
                    bool _IfExist(string path)  // check to see if file/folder exists
                    {
                        if (File.Exists(path))
                        {
                            // This path is a file
                            return true;
                        }
                        else if (Directory.Exists(path))
                        {
                            // This path is a directory
                            return true;
                        }
                        else
                        {
                            return false;
                        }
                    }
            */
        }

        /// <summary>Checks for the existence of a file or folder.</summary>
        /// <param name="FilePattern">The path, filename, or file pattern to check. FilePattern is assumed to be in %A_WorkingDir% if an absolute path isn't specified.</param>        
        public bool IfNotExist(string FilePattern)
        {
            string AHKLine = "IfNotExist, " + FilePattern;  // ahk line to execute
            ErrorLog_Setup(false); // ErrorLevel Detection Enabled for this function in AHK             

            //create an autohtkey engine.
            if (ahkGlobal.ahkdll == null) { ahkGlobal.ahkdll = new AutoHotkey.Interop.AutoHotkeyEngine(); }
            var ahkdll = ahkGlobal.ahkdll;

            // execute a function in AHK
            var add5Results = ahkdll.Eval("FileExist(\"" + FilePattern + "\")");

            // if anything but blank is returned, a file was found
            bool notFound = true;
            if (add5Results.ToString() != "") { notFound = false; }

            return notFound;


            // v1 ToDo
            /*
                    bool _IfNotExist(string path)  // check to see if file/folder exists
                    {
                        if (File.Exists(path))
                        {
                            // This path is a file 
                            return false;
                        }
                        else if (Directory.Exists(path))
                        {
                            // This path is a directory
                            return false;
                        }
                        else
                        {
                            return true;
                        }
                    }
            */


        }


        /// <summary>Changes the script's current working directory.</summary>
        /// <param name="DirName">The name of the new working directory, which is assumed to be a subfolder of the current %A_WorkingDir% if an absolute path isn't specified.</param>
        public bool SetWorkingDir(string DirName)
        {
            string AHKLine = "SetWorkingDir, " + DirName;  // ahk line to execute
            ErrorLog_Setup(true, "Error Setting WorkingDir: " + DirName); // ErrorLevel Detection Enabled for this function in AHK 
            Execute(AHKLine);   // execute AHK code and return variable value

            if (!ahkGlobal.ErrorLevel) { return true; } // no error level - return true for success
            return false;  // error level detected - success = false
        }

        ///// <summary>Separates a file path - returns file name (with extension)</summary>
        ///// <param name="InputVar">Name of the variable containing the file name to be analyzed.</param>
        //public string SplitPath_FileName(string InputVar)
        //{
        //    string AHKLine = "InputVar = " + InputVar + Environment.NewLine + "SplitPath, InputVar, OutFileName, OutDir, OutExtension, OutNameNoExt, OutDrive";  // ahk line to execute
        //    ErrorLog_Setup(false); // ErrorLevel Detection Enabled for this function in AHK 
        //    string OutVar = Execute(AHKLine, "OutFileName");   // execute AHK code and return variable value 
        //    return OutVar;             
        //}

        ///// <summary>Separates a file path - returns file directory path</summary>
        ///// <param name="InputVar">Name of the variable containing the file name to be analyzed.</param>
        //public string SplitPath_FileDir(string InputVar)
        //{
        //    string AHKLine = "InputVar = " + InputVar + Environment.NewLine + "SplitPath, InputVar, OutFileName, OutDir, OutExtension, OutNameNoExt, OutDrive";  // ahk line to execute
        //    ErrorLog_Setup(false); // ErrorLevel Detection Enabled for this function in AHK 
        //    string OutVar = Execute(AHKLine, "OutDir");   // execute AHK code and return variable value 
        //    return OutVar;             
        //}

        ///// <summary>Separates a file path - returns file extension</summary>
        ///// <param name="InputVar">Name of the variable containing the file name to be analyzed.</param>
        //public string SplitPath_FileExt(string InputVar)
        //{
        //    string AHKLine = "InputVar = " + InputVar + Environment.NewLine + "SplitPath, InputVar, OutFileName, OutDir, OutExtension, OutNameNoExt, OutDrive";  // ahk line to execute
        //    ErrorLog_Setup(false); // ErrorLevel Detection Enabled for this function in AHK 
        //    string OutVar = Execute(AHKLine, "OutExtension");   // execute AHK code and return variable value 
        //    return OutVar;             
        //}

        ///// <summary>Separates a file path - returns file name (no extension)</summary>
        ///// <param name="InputVar">Name of the variable containing the file name to be analyzed.</param>
        //public string SplitPath_FileNameNoExt(string InputVar)
        //{
        //    string AHKLine = "InputVar = " + InputVar + Environment.NewLine + "SplitPath, InputVar, OutFileName, OutDir, OutExtension, OutNameNoExt, OutDrive";  // ahk line to execute
        //    ErrorLog_Setup(false); // ErrorLevel Detection Enabled for this function in AHK 
        //    string OutVar = Execute(AHKLine, "OutNameNoExt");   // execute AHK code and return variable value 
        //    return OutVar;             
        //}

        ///// <summary>Separates a file path - returns file drive letter</summary>
        ///// <param name="InputVar">Name of the variable containing the file name to be analyzed.</param>
        //public string SplitPath_FileDrive(string InputVar)
        //{
        //    string AHKLine = "InputVar = " + InputVar + Environment.NewLine + "SplitPath, InputVar, OutFileName, OutDir, OutExtension, OutNameNoExt, OutDrive";  // ahk line to execute
        //    ErrorLog_Setup(false); // ErrorLevel Detection Enabled for this function in AHK 
        //    string OutVar = Execute(AHKLine, "OutDrive");   // execute AHK code and return variable value 
        //    return OutVar;             
        //}

        // ToDo - Create FileDrive !!!

        //####  File Path Info (replace SplitPath ahk function)  #####

        /// <summary>Separates a file path - returns file name (with extension)</summary>
        /// <param name="FilePath">File Location to Parse</param>
        /// <param name="CheckIfExists">Option to check to see if FilePath exists - FileName returns blank if file not found</param>
        public string FileName(string FilePath, bool CheckIfExists = false)
        {
            if (FilePath == null || FilePath.Trim() == "") { return ""; }  // don't check if blank value passed in

            if (FilePath.Contains("/"))  // ex: https://www.thetvdb.com/banners/posters/73141-6.jpg
            {
                return StringSplit(FilePath, "/", 0, true);
            }

            if (CheckIfExists)
            {
                if (File.Exists(FilePath))
                {
                    System.IO.FileInfo fileinfo = new System.IO.FileInfo(FilePath); //retrieve info about each file
                    return fileinfo.Name.ToString();
                }
                return "";
            }
            else
            {
                if (File.Exists(FilePath))
                {
                    System.IO.FileInfo fileinfo = new System.IO.FileInfo(FilePath); //retrieve info about each file
                    return fileinfo.Name.ToString();
                }
                else
                {
                    return StringSplit(FilePath, @"\", 0, true); 
                }
            }
        }

        /// <summary>
        /// Returns File's Drive Letter
        /// </summary>
        /// <param name="FilePath"></param>
        /// <returns></returns>
        public string FileDrive(string FilePath)
        {
            if (FilePath == null || FilePath.Trim() == "") { return ""; }  // don't check if blank value passed in
            return StringSplit(FilePath, ":", 0);
        }

        /// <summary>Separates a file path - returns file name (no extension)</summary>
        /// <param name="FilePath">File Location to Parse</param>
        /// <param name="CheckIfExists">Option to check to see if FilePath exists - FileNameNoExt returns blank if file not found</param>
        public string FileNameNoExt(string FilePath, bool CheckIfExists = false)
        {
            if (FilePath == null || FilePath.Trim() == "") { return ""; }  // don't check if blank value passed in

            if (CheckIfExists)
            {
                if (File.Exists(FilePath))
                {
                    System.IO.FileInfo fileinfo = new System.IO.FileInfo(FilePath); //retrieve info about each file
                    return fileinfo.Name.Replace(fileinfo.Extension, "");
                }
                return "";
            }
            else
            {
                //System.IO.FileInfo fileinfo = new System.IO.FileInfo(FilePath); //retrieve info about each file

                string seg = StringSplit(FilePath, @"\", 0, true, true);
                string ext = StringSplit(seg, ".", 0, true);

                return StringReplace(seg, "." + ext);
            }
        }

        /// <summary>Separates a file path - returns file extension (Includes '.' Prefix)</summary>
        /// <param name="FilePath">File Location to Parse</param>
        /// <param name="CheckIfExists">Option to check to see if FilePath exists - FileExt returns blank if file not found</param>
        /// <param name="RemovePrefix">Option to remove leading . in front of File Extention Return</param>
        public string FileExt(string FilePath, bool CheckIfExists = false, bool RemovePrefix = false)
        {
            if (FilePath == null || FilePath.Trim() == "") { return ""; }  // don't check if blank value passed in

            if (CheckIfExists)
            {
                if (File.Exists(FilePath))
                {
                    System.IO.FileInfo fileinfo = new System.IO.FileInfo(FilePath); //retrieve info about each file

                    //option to remove leading . in front of extention
                    string ext = fileinfo.Extension;
                    if (RemovePrefix) { ext = StringReplace(ext, ".", ""); }

                    return ext;
                }
                return "";
            }
            else
            {
                System.IO.FileInfo fileinfo = new System.IO.FileInfo(FilePath); //retrieve info about each file

                //option to remove leading . in front of extention
                string ext = fileinfo.Extension;
                if (RemovePrefix)
                {
                    ext = StringReplace(ext, ".", "");
                }

                return ext;
            }
        }

        /// <summary>Returns File's Parent Directory Path</summary>
        /// <param name="FilePath">File Location to Parse</param>
        /// <param name="CheckIfExists">Option to check to see if FilePath exists - FileDir returns blank if file not found</param>
        public string FileDir(string FilePath, bool CheckIfExists = false)  // 
        {
            if (FilePath == null || FilePath.Trim() == "") { return ""; }  // don't check if blank value passed in

            if (CheckIfExists)
            {
                if (File.Exists(FilePath))
                {
                    try
                    {
                        System.IO.FileInfo fileinfo = new System.IO.FileInfo(FilePath); //retrieve info about each file
                        return fileinfo.Directory.ToString();
                    }
                    catch
                    { }

                }
                return "";
            }
            else
            {
                try
                {
                    System.IO.FileInfo fileinfo = new System.IO.FileInfo(FilePath); //retrieve info about each file
                    return fileinfo.Directory.ToString();
                }
                catch
                {
                    return "";
                }
            }
        }

        /// <summary>Returns File's Parent Directory Name from Full File Path</summary>
        /// <param name="FilePath">File Location to Parse</param>
        /// <param name="CheckIfExists">Option to check to see if FilePath exists - FileDir returns blank if file not found</param>
        public string DirName(string FilePath, bool CheckIfExists = false)
        {
            if (FilePath == null || FilePath.Trim() == "") { return ""; }  // don't check if blank value passed in

            if (CheckIfExists)
            {
                // if filePath is a directory, just return that dir name
                if (IsDir(FilePath))
                {
                    string DirName = "";
                    string[] words = FilePath.Split('\\');
                    foreach (string word in words) { DirName = word; }
                    return DirName;
                }

                if (File.Exists(FilePath))
                {
                    System.IO.FileInfo fileinfo = new System.IO.FileInfo(FilePath); //retrieve info about each file
                    string s = fileinfo.DirectoryName.ToString();
                    string DirName = "";
                    string[] words = s.Split('\\');
                    foreach (string word in words) { DirName = word; }
                    return DirName;
                }
                return "";
            }
            else
            {

                // if filePath is a directory, just return that dir name
                if (IsDir(FilePath))
                {
                    string DirName = "";
                    string[] words = FilePath.Split('\\');
                    foreach (string word in words) { DirName = word; }
                    return DirName;
                }
                else
                {
                    System.IO.FileInfo fileinfo = new System.IO.FileInfo(FilePath); //retrieve info about each file
                    string DirName = "";
                    if (fileinfo.DirectoryName != null)
                    {
                        string s = fileinfo.DirectoryName.ToString();
                        string[] words = s.Split('\\');
                        foreach (string word in words) { DirName = word; }
                    }
                    return DirName;
                }
            }
        }


        /// <summary>
        /// Returns File's Last Modified Date 
        /// </summary>
        /// <param name="FilePath"></param>
        /// <returns></returns>
        public DateTime FileModified(string FilePath)
        {
            FileInfo info = new FileInfo(FilePath);  // Get Attributes for file.
            return info.LastWriteTime;
        }

        // File Info

        /// <summary>Returns name of file size in bytes from file path</summary>
        /// <param name="FilePath">File Location to Parse</param>
        /// <param name="CheckIfExists">Option to check to see if FilePath exists - FileDir returns blank if file not found</param>
        /// <param name="ReturnBytes">Returns Bytes by Default, option to override and return bytes converted to KB/MB/TB</param>
        public string FileSize(string FilePath, bool CheckIfExists = false, bool ReturnBytes = true)
        {
            if (FilePath == null || FilePath.Trim() == "") { return "0"; }  // don't check if blank value passed in

            if (CheckIfExists)
            {
                if (File.Exists(FilePath))
                {
                    System.IO.FileInfo fileinfo = new System.IO.FileInfo(FilePath); //retrieve info about each file

                    // option to return plain english representation of bytes 
                    if (!ReturnBytes)
                    {
                        return FormatBytes(fileinfo.Length);  // convert bytes to Text representation (adds kb/mb/tb to return)
                    }

                    return fileinfo.Length.ToString();
                }
                return "";
            }
            else
            {
                System.IO.FileInfo fileinfo = new System.IO.FileInfo(FilePath); //retrieve info about each file

                // option to return plain english representation of bytes 
                if (!ReturnBytes)
                {
                    return FormatBytes(fileinfo.Length);  // convert bytes to Text representation (adds kb/mb/tb to return)
                }

                return fileinfo.Length.ToString();
            }

        }

        /// <summary>checks whether a file is Compressed</summary>
        /// <param name="filePath"> </param>
        public bool isCompressed(string filePath)
        {
            if (filePath == null || filePath.Trim() == "") { return false; }  // don't check if blank value passed in

            bool isCompressed = ((File.GetAttributes(filePath) & FileAttributes.Compressed) == FileAttributes.Compressed);
            return isCompressed;
        }

        /// <summary>checks whether a file is encrypted</summary>
        /// <param name="filePath"> </param>
        public bool isEncrypted(string filePath)
        {
            if (filePath == null || filePath.Trim() == "") { return false; }  // don't check if blank value passed in

            bool isEncrypted = ((File.GetAttributes(filePath) & FileAttributes.Encrypted) == FileAttributes.Encrypted);
            return isEncrypted;
        }

        /// <summary>checks whether a file is read only</summary>
        /// <param name="filePath"> </param>
        public bool isReadOnly(string filePath)
        {
            if (filePath == null || filePath.Trim() == "") { return false; }  // don't check if blank value passed in

            bool isReadOnly = ((File.GetAttributes(filePath) & FileAttributes.ReadOnly) == FileAttributes.ReadOnly);
            return isReadOnly;
        }

        /// <summary>checks whether a file is hidden</summary>
        /// <param name="filePath"> </param>
        public bool isHidden(string filePath)
        {
            bool isHidden = ((File.GetAttributes(filePath) & FileAttributes.Hidden) == FileAttributes.Hidden);
            return isHidden;
        }

        /// <summary>checks whether a file has archive attribute</summary>
        /// <param name="filePath"> </param>
        public bool isArchive(string filePath)
        {
            if (filePath == null || filePath.Trim() == "") { return false; }  // don't check if blank value passed in

            bool isArchive = ((File.GetAttributes(filePath) & FileAttributes.Archive) == FileAttributes.Archive);
            return isArchive;
        }

        /// <summary>checks whether a file is system file</summary>
        /// <param name="filePath"> </param>
        public bool isSystem(string filePath)
        {
            if (filePath == null || filePath.Trim() == "") { return false; }  // don't check if blank value passed in

            bool isSystem = ((File.GetAttributes(filePath) & FileAttributes.System) == FileAttributes.System);
            return isSystem;
        }

        /// <summary>
        /// Checks FilePath's Extension to see if it matches known video formats
        /// </summary>
        /// <param name="FilePath">File path to check</param>
        /// <returns>Returns True if File is Known Video Format</returns>
        public bool isVideo(string FilePath)
        {
            if (FilePath == null || FilePath.Trim() == "") { return false; }  // don't check if blank value passed in

            try
            {
                string FileExt = Path.GetExtension(FilePath);

                // check to see if format is video
                if (FileExt.ToUpper() == ".FLV") { return true; }
                if (FileExt.ToUpper() == ".MP4") { return true; }
                if (FileExt.ToUpper() == ".MPG") { return true; }
                if (FileExt.ToUpper() == ".AVI") { return true; }
                if (FileExt.ToUpper() == ".MKV") { return true; }
                if (FileExt.ToUpper() == ".MPEG") { return true; }
                if (FileExt.ToUpper() == ".DIVX") { return true; }
                if (FileExt.ToUpper() == ".MOV") { return true; }
                if (FileExt.ToUpper() == ".M4V") { return true; }
                if (FileExt.ToUpper() == ".ASF") { return true; }
                if (FileExt.ToUpper() == ".WMV") { return true; }
                if (FileExt.ToUpper() == ".VOB") { return true; }
                return false;
            }
            catch
            {
                return false;
            }
        }

        /// <summary>
        /// Checks FilePath's Extension to see if it matches known text file formats
        /// </summary>
        /// <param name="FilePath">File path to check</param>
        /// <returns>Returns True if File is Known Text File Format</returns>
        public bool isText(string FilePath)
        {
            if (FilePath == null || FilePath.Trim() == "") { return false; }  // don't check if blank value passed in

            try
            {
                string FileExt = Path.GetExtension(FilePath);

                // check to see if format is text

                if (FileExt.ToUpper() == ".CS") { return true; }
                if (FileExt.ToUpper() == ".TXT") { return true; }
                if (FileExt.ToUpper() == ".INI") { return true; }
                if (FileExt.ToUpper() == ".AHK") { return true; }
                if (FileExt.ToUpper() == ".CONFIG") { return true; }
                if (FileExt.ToUpper() == ".SETTINGS") { return true; }
                if (FileExt.ToUpper() == "1") { return true; }
                if (FileExt.ToUpper() == "2") { return true; }
                if (FileExt.ToUpper() == "3") { return true; }

                return false;
            }
            catch
            {
                return false;
            }

        }

        /// <summary>
        /// Checks FilePath's Extension to see if it matches known Image File Formats
        /// </summary>
        /// <param name="FilePath">File path to check</param>
        /// <returns>Returns True if File is Known Image File Format</returns>
        public bool isImage(string FilePath)
        {
            if (FilePath == null || FilePath.Trim() == "") { return false; }  // don't check if blank value passed in

            try
            {
                string FileExt = Path.GetExtension(FilePath);

                if (FileExt.ToUpper() == ".PNG") { return true; }
                if (FileExt.ToUpper() == ".JPG") { return true; }
                if (FileExt.ToUpper() == ".JPEG") { return true; }
                if (FileExt.ToUpper() == ".GIF") { return true; }
                if (FileExt.ToUpper() == ".ICO") { return true; }
                if (FileExt.ToUpper() == ".ICON") { return true; }

                return false;
            }
            catch
            {
                return false;
            }
        }


        /// <summary>
        /// Checks File/DirPath and Returns True if File
        /// </summary>
        /// <param name="Path">File or Folder Path to Check</param>
        /// <returns>Returns True if Path leads to File instead of Directory</returns>
        public bool isFile(string Path)
        {
            try
            {
                // get the file attributes for file or directory
                FileAttributes attr = File.GetAttributes(Path);

                if (attr.HasFlag(FileAttributes.Directory))
                    return false;
                else
                    return true;
            }
            catch
            {
                return false;
            }
        }

        /// <summary>
        /// Checks File/DirPath and Returns True if Directory
        /// </summary>
        /// <param name="Path">File or Folder Path to Check</param>
        /// <returns>Returns True if Path leads to Directory instead of File</returns>
        public bool isDir(string Path)
        {
            try
            {
                // get the file attributes for file or directory
                FileAttributes attr = File.GetAttributes(Path);

                if (attr.HasFlag(FileAttributes.Directory))
                    return true;
                else
                    return false;
            }
            catch
            {
                return false;
            }

        }


        /// <summary>Returns the timestamp when the file was created</summary>
        /// <param name="FilePath"> </param>
        public DateTime CreationTime(string FilePath)
        {
            if (FilePath == null || FilePath.Trim() == "") { return new DateTime(); }  // don't check if blank value passed in

            DateTime creationTime = File.GetCreationTime(FilePath);
            return creationTime;
        }

        /// <summary>Returns the timestamp when the file was written to</summary>
        /// <param name="FilePath"> </param>
        public DateTime LastWriteTime(string FilePath)
        {
            if (FilePath == null || FilePath.Trim() == "") { return new DateTime(); }  // don't check if blank value passed in

            DateTime lastWriteTime = File.GetLastWriteTime(FilePath);
            return lastWriteTime;
        }

        /// <summary>Returns the timestamp when the file was last accessed</summary>
        /// <param name="FilePath"> </param>
        public DateTime LastAccessTime(string FilePath)
        {
            if (FilePath == null || FilePath.Trim() == "") { return new DateTime(); }  // don't check if blank value passed in

            DateTime lastAccessTime = File.GetLastAccessTime(FilePath);
            return lastAccessTime;
        }


        // File Actions

        /// <summary>Copies a folder along with all its sub-folders and files (similar to xcopy).</summary>
        /// <param name="Source">Name of the source directory (with no trailing backslash), which is assumed to be in %A_WorkingDir% if an absolute path isn't specified.</param>
        /// <param name="Dest">Name of the destination directory (with no trailing baskslash), which is assumed to be in %A_WorkingDir% if an absolute path isn't specified.</param>
        /// <param name="OverWrite">Flag determines whether to overwrite files if they already exist. True = OverWrite Existing Files</param>
        public bool FileCopyDir(string Source, string Dest, bool OverWrite = false)
        {
            string source = Source.Replace(",", "`,");
            string dest = Dest.Replace(",", "`,");

            // change overwrite bool to 1/0 
            int overWrite = 0; if (OverWrite) { overWrite = 1; }

            string AHKLine = "FileCopyDir, " + source + ", " + dest + ", " + overWrite;  // ahk line to execute
            ErrorLog_Setup(true, "Error Copying " + source); // ErrorLevel Detection Enabled for this function in AHK 
            Execute(AHKLine);   // execute AHK code and return variable value 

            if (!ahkGlobal.ErrorLevel) { return true; } // no error level - return true for success
            return false;  // error level detected - success = false

            // v1 ToDo
            /*
                    void _FileCopyDir(string SourceDir, string DestinationDir, bool OverWrite = true)  // copy directory from one path to destination directory
                    {
                        DirectoryInfo diSource = new DirectoryInfo(SourceDir);
                        DirectoryInfo diTarget = new DirectoryInfo(DestinationDir);

                        _CopyAll(diSource, diTarget, OverWrite);
                    }
            */

        }

        /// <summary>Creates a directory/folder.</summary>
        /// <param name="DirName">Name of the directory to create, which is assumed to be in %A_WorkingDir% if an absolute path isn't specified.</param>
        public bool FileCreateDir(string DirName)
        {
            string dirName = DirName.Replace(",", "`,");
            string AHKLine = "FileCreateDir, " + dirName;  // ahk line to execute
            ErrorLog_Setup(true, "Error Creating Dir: " + dirName); // ErrorLevel Detection Enabled for this function in AHK 
            Execute(AHKLine);   // execute AHK code and return variable value 

            if (!ahkGlobal.ErrorLevel) { return true; } // no error level - return true for success
            return false;  // error level detected - success = false

            // v1 ToDo
            /*
                        if (!Directory.Exists(DirName))  // create DirName if it doesn't exist
                        {
                            Directory.CreateDirectory(DirName);
                        }
             */

        }

        /// <summary>Moves a folder along with all its sub-folders and files. It can also rename a folder.</summary>
        /// <param name="Source">Name of the source directory (with no trailing backslash), which is assumed to be in %A_WorkingDir% if an absolute path isn't specified.</param>
        /// <param name="Dest">The new path and name of the directory (with no trailing baskslash), which is assumed to be in %A_WorkingDir% if an absolute path isn't specified. For example: D:\My Folder. Note: Dest is the actual path and name that the directory will have after it is moved; it is not the directory into which Source is moved (except for the known limitation mentioned below).</param>
        /// <param name="Flag">0 (default): Do not overwrite existing files. | 1: Overwrite existing files. However, any files or subfolders inside Dest that do not have a counterpart in Source will not be deleted. | 2: The same as mode 1 above except that the limitation is absent. | R: Rename the directory rather than moving it. </param>
        public bool FileMoveDir(string Source, string Dest, string Flag = "0")
        {
            string source = Source.Replace(",", "`,");
            string dest = Dest.Replace(",", "`,");

            string AHKLine = "FileMoveDir, " + source + ", " + dest + ", " + Flag;  // ahk line to execute
            ErrorLog_Setup(true, "Error Moving Dir:" + source + " To: " + dest); // ErrorLevel Detection Enabled for this function in AHK 
            Execute(AHKLine);   // execute AHK code and return variable value

            if (!ahkGlobal.ErrorLevel) { return true; } // no error level - return true for success
            return false;  // error level detected - success = false
        }

        /// <summary>Deletes a folder.</summary>
        /// <param name="DirPath">Name of the directory to delete, which is assumed to be in %A_WorkingDir% if an absolute path isn't specified.</param>
        /// <param name="Recurse">Recurse = False - Do not remove files and sub-directories contained in DirName. In this case, if DirName is not empty, no action will be taken | True = Remove all files and subdirectories.</param>
        public bool FileRemoveDir(string DirPath, bool Recurse = false)
        {
            if (!Directory.Exists(DirPath)) { return true; }

            string dirPath = DirPath.Replace(",", "`,");

            string AHKLine = "FileRemoveDir, " + dirPath + ", " + Recurse;  // ahk line to execute
            ErrorLog_Setup(true, "Error Removing Dir :" + dirPath); // ErrorLevel Detection Enabled for this function in AHK 
            Execute(AHKLine);   // execute AHK code and return variable value

            if (!ahkGlobal.ErrorLevel) { return true; } // no error level - return true for success

            else// error level detected - success = false. retry with C# command if so
            {
                try
                {
                    Directory.Delete(DirPath, true); // true => recursive delete
                }
                catch
                {
                    return false;
                }

                return false;  
            }
            

            // v1 ToDo
            /*
                    bool _FileRemoveDir(string DirName)  // remove / delete directory path
                    {
                        if (Directory.Exists(DirName))
                        {
                            try
                            {
                                Directory.Delete(DirName, true);
                                return true;
                            }
                            catch
                            {
                                return false;
                            }

                        }

                        return true;
                    }
            */

        }

        /// <summary>Opens Directory in Windows Explorer Window (If Found), Returns False if there is an Error / Directory Not Found</summary>
        /// <param name="DirPath">Path to directory to open in explorer. Can also pass in FilePath to Open File's Directory</param>
        /// <param name="CreateIfMissing">Option to Create Missing Directory instead of Returning False, Opens New Dir After Creating</param>
        public bool OpenDir(string DirPath, bool CreateIfMissing = false)
        {
            if (DirPath.Trim() == "" || DirPath == null) { return false; }

            if (isFile(DirPath)) { DirPath = FileDir(DirPath); }  // if file passed in, open file's directory

            if (Directory.Exists(DirPath))
            {
                try
                {
                    Process.Start(DirPath);
                    return true;
                }
                catch
                {
                    return false;
                }
            }
            else  // directory not found on pc
            {
                if (CreateIfMissing)
                {
                    FileCreateDir(DirPath);
                    return OpenDir(DirPath);
                }
            }

            return false;
        }

        /// <summary>Opens Directory in Windows Explorer containing FilePath</summary>
        /// <param name="FilePath">Path to file, extracting the folder path to open</param>
        public bool OpenFileDir(string FilePath)
        {
            string dirPath = FileDir(FilePath, true);
            return OpenDir(dirPath);
        }

        // File Compare

        #region === File Compare / Hash ===

        /// <summary>
        /// Returns Hash value for File
        /// </summary>
        /// <param name="FilePath"></param>
        /// <returns></returns>
        public string FileHash(string FilePath)
        {
            try
            {
                using (var fs = new FileStream(FilePath, FileMode.Open))
                using (var reader = new BinaryReader(fs))
                {
                    var hash = new SHA512CryptoServiceProvider();
                    hash.ComputeHash(reader.ReadBytes((int)FilePath.Length));
                    return Convert.ToBase64String(hash.Hash);
                }
            }
            catch
            { return ""; }
        }

        /// <summary>
        /// Compare 2 Files To See if they are the same (either compares Hash or Bytes)
        /// </summary>
        /// <param name="fileOne">First File To Compare</param>
        /// <param name="fileTwo">Second File to Compare</param>
        /// <param name="Hash">Option to Compare File Hashes (Default = True). If False, Compares by Bytes</param>
        /// <returns>Returns True if Files are the Same</returns>
        public bool FileCompare(string fileOne, string fileTwo, bool Hash = true)
        {
            // source: http://stackoverflow.com/questions/1358510/how-to-compare-2-files-fast-using-net

            var sw = new Stopwatch();

            // false if file not located
            if (!File.Exists(fileOne)) { return false; }
            if (!File.Exists(fileTwo)) { return false; }

            if (Hash)  // file hash compare 
            {
                sw.Start();
                bool compare1 = FileCompare_Hash(fileOne, fileTwo);
                sw.Stop();
                Debug.WriteLine(string.Format("Compare using Hash {0}", sw.ElapsedTicks));
                return compare1;
            }
            if (!Hash) // file byte compare
            {
                sw.Start();
                bool compare2 = FileCompare_Bytes(fileOne, fileTwo);
                sw.Stop();
                Debug.WriteLine(string.Format("Compare byte-byte {0}", sw.ElapsedTicks));
                return compare2;
            }

            return false;
        }

        /// <summary>
        /// Compare two files by bytes - returns true if match found 
        /// </summary>
        /// <param name="file1"></param>
        /// <param name="file2"></param>
        /// <returns></returns>
        public bool FileCompare_Bytes(string file1, string file2)
        {
            using (var fs1 = new FileStream(file1, FileMode.Open))
            using (var fs2 = new FileStream(file2, FileMode.Open))
            {
                if (fs1.Length != fs2.Length) return false;
                int b1, b2;
                do
                {
                    b1 = fs1.ReadByte();
                    b2 = fs2.ReadByte();
                    if (b1 != b2 || b1 < 0) return false;
                }
                while (b1 >= 0);
            }
            return true;
        }

        /// <summary>
        /// Compares two image paths using hash - returns true if match found
        /// </summary>
        /// <param name="file1"></param>
        /// <param name="file2"></param>
        /// <returns></returns>
        public bool FileCompare_Hash(string file1, string file2)
        {
            var str1 = FileHash(file1);
            var str2 = FileHash(file2);
            return str1 == str2;
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
        public string DirPrint(string SearchDir, string OutFile = "", bool Recurse = true, string SearchPattern = "*.*", bool OverWritePrevious = true, bool OpenAfterWrite = true)
        {
            FileDelete(OutFile);

            string[] filelist = null;

            if (!Recurse) { filelist = Directory.GetFiles(SearchDir, SearchPattern, System.IO.SearchOption.TopDirectoryOnly); } // no recurse
            else { filelist = Directory.GetFiles(SearchDir, SearchPattern, System.IO.SearchOption.AllDirectories); }            // recurse

            if (filelist == null) { return ""; }

            string outText = "";
            foreach (string filename in filelist)
            {
                // Get Attributes for file.
                FileInfo info = new FileInfo(filename);

                string FullPath = info.FullName;
                string DirName = info.DirectoryName;
                string DateModified = info.LastWriteTime.ToString();
                string Ext = info.Extension;
                string FileName = info.Name;
                string FileNameNoExt = FileName.Replace(Ext, ""); // remove file extention from file name

                //string WriteLine = FullPath + " (" + DateModified + ")";
                string WriteLine = FullPath;

                if (outText != "") { outText = outText + "\n" + WriteLine; }
                if (outText == "") { outText = WriteLine; }

                FileAppend(WriteLine, OutFile);
            }

            // option to open output file after writing
            if (OpenAfterWrite) { Run(OutFile); }

            return outText;
        }

        /// <summary>Converts search directory contents to Datatable to display in DataGridView etc</summary>
        /// <param name="SearchDir"> </param>
        /// <param name="Recurse"> </param>
        /// <param name="SearchPattern"> </param>
        public DataTable GetDirectoryTable(string SearchDir, bool Recurse = true, string SearchPattern = "*.*")
        {
            if (!Directory.Exists(SearchDir)) { return null; }

            //// Display dir files in DataGrid
            //// Ex: 
            //DataTable FileList = ahk.GetDirectoryTable(SearchDir, Recurse, SearchPattern);

            //dataGridView2.DataSource = FileList; // populate datagrid 

            //// format the datagrid results

            //this.dataGridView2.Columns[0].Visible = false;  //FullPath
            ////this.dataGridView2.Columns[1].Visible = false;  //FileName
            //this.dataGridView2.Columns[2].Visible = false;  //DirName
            //this.dataGridView2.Columns[3].Visible = false;  //FileNameNoExt
            //this.dataGridView2.Columns[4].Visible = false;  //Ext
            //this.dataGridView2.Columns[5].Visible = false;  //FileSize
            //this.dataGridView2.Columns[6].Visible = false;  //DateModified

            //this.dataGridView2.Columns[1].AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells;



            // returns DataTable with info on files in a folder

            DataTable FileTable = new DataTable();
            FileTable.Columns.Add("FullPath", typeof(string));
            FileTable.Columns.Add("FileName", typeof(string));
            FileTable.Columns.Add("DirName", typeof(string));
            FileTable.Columns.Add("FileNameNoExt", typeof(string));
            FileTable.Columns.Add("Ext", typeof(string));
            FileTable.Columns.Add("FileSize", typeof(long));
            FileTable.Columns.Add("DateModified", typeof(DateTime));


            string[] filelist = null;

            if (Recurse == false)
            {
                filelist = Directory.GetFiles(SearchDir, SearchPattern, System.IO.SearchOption.TopDirectoryOnly);  // no recurse
            }

            if (Recurse == true)
            {
                filelist = Directory.GetFiles(SearchDir, SearchPattern, System.IO.SearchOption.AllDirectories);  // recurse
            }

            foreach (string filename in filelist)
            {
                // Get Attributes for file.
                FileInfo info = new FileInfo(filename);

                string FullPath = info.FullName;
                string DirName = info.DirectoryName;
                string DateModified = info.LastWriteTime.ToString();
                string Ext = info.Extension;
                string FileName = info.Name;
                string FileNameNoExt = FileName.Replace(Ext, ""); // remove file extention from file name
                long length = new System.IO.FileInfo(FullPath).Length; // file size in bytes

                FileTable.Rows.Add(FullPath, FileName, DirName, FileNameNoExt, Ext, length, DateModified);
            }

            return FileTable;

        }

        /// <summary>Returns Directory size in bytes</summary>
        /// <param name="DirPath">Path of Directory to Return Size</param>
        public static long DirSizeBytes(string DirPath)
        {
            // 1.
            // Get array of all file names.
            string[] a = Directory.GetFiles(DirPath, "*.*");

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
            // 4.
            // Return total size
            return b;
        }

        /// <summary>Returns true if path is a valid Directory Path</summary>
        /// <param name="FolderPath"> </param>
        public bool IsDir(string FolderPath)  // returns true if path is a directory
        {
            if (Directory.Exists(FolderPath))
            {
                // get the file attributes for file or directory
                FileAttributes attr = File.GetAttributes(FolderPath);

                //detect whether its a directory or file
                if ((attr & FileAttributes.Directory) == FileAttributes.Directory)
                    return true;
                //MessageBox.Show("Its a directory");
                else
                    return false;
            }
            else
                return false;
            //MessageBox.Show("Its a file");
        }

        /// <summary>Returns the next available file name in a folder, incrementing with "File (FileNumber).ext" Format</summary>
        /// <param name="FilePath">Original File Name</param>
        /// <param name="LeadingZeroCount">Option to Add Leading Zeros to New File Name Format</param>
        public string NextFileName(string FilePath, int LeadingZeroCount = 0)
        {
            string extension = Path.GetExtension(FilePath);

            int i = 0;
            while (File.Exists(FilePath))
            {
                if (i == 0)
                    FilePath = FilePath.Replace(extension, "(" + ++i + ")" + extension);
                else
                    FilePath = FilePath.Replace("(" + i + ")" + extension, "(" + ++i + ")" + extension);
            }

            if (LeadingZeroCount > 0)
            {
                FilePath = AddLeadingZeros(FilePath, LeadingZeroCount);
            }

            return FilePath;
        }

        // TODO: Add Timeout to wait function

        Stopwatch WaitForFileExistStopWatch = new Stopwatch();

        /// <summary>Waits for file to exist</summary>
        /// <param name="FileToWaitFor">File to search for, waiting until found</param>
        /// <param name="TimeOutSeconds">Number of Seconds to Wait Before Timing Out</param>
        /// <returns>Returns True If File Found, False if TimeOut Reached</returns>
        public bool WaitForFileExist(string FileToWaitFor, int TimeOutSeconds = -1)
        {
            WaitForFileExistStopWatch = new Stopwatch();
            WaitForFileExistStopWatch.Start();
            bool FoundFile = false; bool TimedOut = false;
            while (FoundFile == false)
            {
                if (File.Exists(FileToWaitFor)) { FoundFile = true; }
                Sleep(500);

                if (TimeOutSeconds != -1)  // if value provided, check to see if timeout reached
                {
                    if (WaitForFileExistStopWatch.Elapsed.TotalSeconds > TimeOutSeconds) { TimedOut = true; FoundFile = true; }
                }
            }
            WaitForFileExistStopWatch.Stop();
            if (FoundFile && !TimedOut) { return true; }
            else { return false; } // timeout reached, file not found
        }





        /// <summary>waits for file to exist, then returns it's contents as a string</summary>
        /// <param name="filePath"> </param>
        public string WaitForFileRead(string filePath)
        {
            WaitForFileExist(filePath);

            string ReturnText = FileRead(filePath);

            return ReturnText.ToString();  // return text from file 
        }


        //========== File Lists =====================

        /// <summary>
        /// Returns List<string> of files in directory path
        /// </summary>
        /// <param name="DirPath"> </param>
        /// <param name="SearchPattern">Normal Windows Search Param for Specific Search, or Provide Multiple Extensions ex: "mp3|wave"</param>
        /// <param name="Recurse"> </param>
        /// <param name="FileNameOnly"> </param>
        /// <param name="IncludeExt"> </param>
        public List<string> FileList(string DirPath, string SearchPattern = "*.*", bool Recurse = true, bool FileNameOnly = false, bool IncludeExt = true)
        {
            List<string> FileList = new List<string>();

            if (!Directory.Exists(DirPath)) { return FileList; }

            string[] files = null;
            if (Recurse) { files = Directory.GetFiles(DirPath, SearchPattern, System.IO.SearchOption.AllDirectories); }
            if (!Recurse) { files = Directory.GetFiles(DirPath, SearchPattern, System.IO.SearchOption.TopDirectoryOnly); }

            if (SearchPattern.Contains("|"))
            {
                files = Directory.GetFiles("path_to_files").Where(file => Regex.IsMatch(file, @"^.+\.(" + SearchPattern + ")$")).ToArray();
            }


            foreach (string file in files)  // loop through list of files and write file details to sqlite db
            {
                string addFile = file;
                if (FileNameOnly) { addFile = FileName(file); }
                if ((FileNameOnly) && (!IncludeExt)) { addFile = FileNameNoExt(file); }


                // add file to list to return
                FileList.Add(addFile);
            }

            return FileList;
        }

        /// <summary>
        /// Returns List of Folders in Directory Path
        /// </summary>
        /// <param name="DirPath"> </param>
        /// <param name="SearchPattern"> </param>
        /// <param name="Recurse"> </param>
        /// <param name="FullPathReturn">Option to return either the Full Directory Paths (true) or the Directory name list (false)</param>
        public List<string> DirList(string DirPath, string SearchPattern = "*.*", bool Recurse = true, bool FullPathReturn = false)
        {
            List<string> FileList = new List<string>();

            if (!Directory.Exists(DirPath)) { return null; }

            string[] files = null;
            if (Recurse) { files = Directory.GetDirectories(DirPath, SearchPattern, System.IO.SearchOption.AllDirectories); }
            if (!Recurse) { files = Directory.GetDirectories(DirPath, SearchPattern, System.IO.SearchOption.TopDirectoryOnly); }

            foreach (string file in files)  // loop through list of files and write file details to sqlite db
            {
                string dirName = "";
                // parse by dashes to get dir naem from full path
                List<string> split = StringSplit_List(file, @"\");
                foreach (string d in split) { dirName = d; }


                if (FullPathReturn) { FileList.Add(file); } // return full path in the list (option)
                if (!FullPathReturn) { FileList.Add(dirName); } // return full path in the list (option)
            }

            return FileList;
        }

        /// <summary>
        /// Search Returns list of (ex: .cs) Files Modified Today 
        /// </summary>
        /// <param name="SearchRoot"></param>
        /// <param name="format"></param>
        /// <returns></returns>
        public List<FileInfo> ModifiedToday(string SearchRoot, string format = "cs")
        {
            string[] Files = Directory.GetFiles(SearchRoot, "*." + format, System.IO.SearchOption.AllDirectories);

            List<FileInfo> files = new List<FileInfo>();

            foreach (string file in Files)
            {
                System.IO.FileInfo fileinfo = new System.IO.FileInfo(file); //retrieve info about each file

                if (fileinfo.LastWriteTime.Date == DateTime.Today)
                {
                    files.Add(fileinfo);
                }
            }

            return files;
        }


        /// <summary>
        /// Format String Removing Illegal Characters - Allowed to Save As File In Windows
        /// </summary>
        /// <param name="FileName"></param>
        /// <param name="ReplaceChar">Character to replace illegal characters with, Default = Space</param>
        /// <returns></returns>
        public string SafeSaveName(string FileName, string ReplaceChar = " ")
        {
            string showNameSave = FileName.Replace("|", " ");
            showNameSave = showNameSave.Replace(@"\", " ");
            showNameSave = showNameSave.Replace(@"/", " ");
            showNameSave = showNameSave.Replace(@":", " ");
            showNameSave = showNameSave.Replace(@"*", " ");
            showNameSave = showNameSave.Replace(@"?", " ");
            showNameSave = showNameSave.Replace(@"<", " ");
            showNameSave = showNameSave.Replace(@">", " ");

            return showNameSave;
        }



        /// <summary>
        /// Returns Dictionary with Full FilePath / FileSize (Bytes)
        /// </summary>
        /// <param name="DirPath"></param>
        /// <param name="SearchPattern"></param>
        /// <param name="Recurse"></param>
        /// <returns></returns>
        public Dictionary<string, string> FileSizeDict(string DirPath, string SearchPattern = "*.*", bool Recurse = true)
        {
            Dictionary<string, string> fileDict = new Dictionary<string, string>();

            if (!Directory.Exists(DirPath)) { return null; }

            string[] files = null;
            if (Recurse) { files = Directory.GetFiles(DirPath, SearchPattern, System.IO.SearchOption.AllDirectories); }
            if (!Recurse) { files = Directory.GetFiles(DirPath, SearchPattern, System.IO.SearchOption.TopDirectoryOnly); }

            foreach (string file in files)  // loop through list of files and write file details to sqlite db
            {
                string size = FileSize(file);
                fileDict.Add(file, size);
            }

            return fileDict;
        }


        /// <summary>
        /// Loop Through List of Directories, Returns Dictionary with FilePath / FileSize (bytes)
        /// </summary>
        /// <param name="DirPaths"></param>
        /// <param name="SearchPattern"></param>
        /// <param name="Recurse"></param>
        /// <returns></returns>
        public Dictionary<string, string> FileSizeDict_FromDirList(List<string> DirPaths, string SearchPattern = "*.*", bool Recurse = true)
        {
            Dictionary<string, string> fileDict = new Dictionary<string, string>();

            foreach(string DirPath in DirPaths)
            {
                if (!Directory.Exists(DirPath)) { continue; }

                string[] files = null;
                if (Recurse) { files = Directory.GetFiles(DirPath, SearchPattern, System.IO.SearchOption.AllDirectories); }
                if (!Recurse) { files = Directory.GetFiles(DirPath, SearchPattern, System.IO.SearchOption.TopDirectoryOnly); }

                foreach (string file in files)  // loop through list of files and write file details to sqlite db
                {
                    string size = FileSize(file);
                    fileDict.Add(file, size);
                }
            }

            return fileDict;
        }



        #endregion
    }
}
