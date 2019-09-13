//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Text;
//using System.Threading.Tasks;
//using System.Windows.Forms;
//using System.Data;
//using System.IO;
//using System.Diagnostics;
//using System.Text.RegularExpressions;
////using System.Data.SQLite;
//using System.Data.SqlClient;
//using sharpAHK;
//using System.Drawing;
////using ScintillaNET;
//using System.ComponentModel;
//using System.Collections;
//using System.Runtime.InteropServices;
//using System.Security.Cryptography;
//using System.Drawing.Imaging;
//using System.Xml.Serialization;
//using System.Threading;
//using System.Reflection;
//using System.Net;
////using System.Windows.Automation;
//using System.Timers;
//using System.Configuration;
//using System.IO.Compression;

//namespace sharpAHK
//{
//    public class _AHKc
//    {
//        private _AHK ahk = new _AHK(); 

//        #region === v1 User Input ===

//        public void MsgBox(string Text, string TitleText = "")  // popup messagebox with "OK" button
//        {
//            // examples: http://www.dotnetperls.com/messagebox-show

//            MessageBox.Show(Text, TitleText, MessageBoxButtons.OK, MessageBoxIcon.None, MessageBoxDefaultButton.Button1, (MessageBoxOptions)0x40000);
//        }
//        public DialogResult _InputBox(string title, string promptText, ref string value, string OKButton = "OK", string CancelButton = "Cancel")  // user input box that promps user for input
//        {
//            //EX: 
//            //string value = "New List Name";
//            //if (ahk.InputBox("Enter New List Name: ", "", ref value) == DialogResult.OK)
//            //{
//            //    string UserEntry = value;
//            //    bool Inserted = sqlite.InsertListItem(ahkGlobal.UserDb, "UserLists", UserEntry, "", "", "0");
//            //    Load_UserList_InGrid(ahkGlobal.UserDb, UserEntry);
//            //    PopulateListDDL();
//            //}


//            Form DialogForm = new Form();
//            Label label = new Label();
//            TextBox textBox = new TextBox();
//            Button buttonOk = new Button();
//            Button buttonCancel = new Button();


//            DialogForm.Text = title;
//            label.Text = promptText;
//            textBox.Text = value;

//            buttonOk.Text = OKButton;
//            buttonCancel.Text = CancelButton;
//            buttonOk.DialogResult = DialogResult.OK;
//            buttonCancel.DialogResult = DialogResult.Cancel;

//            label.SetBounds(9, 20, 372, 13);
//            textBox.SetBounds(12, 36, 372, 20);
//            buttonOk.SetBounds(228, 72, 75, 23);
//            buttonCancel.SetBounds(309, 72, 75, 23);

//            label.AutoSize = true;
//            textBox.Anchor = textBox.Anchor | AnchorStyles.Right;
//            buttonOk.Anchor = AnchorStyles.Bottom | AnchorStyles.Right;
//            buttonCancel.Anchor = AnchorStyles.Bottom | AnchorStyles.Right;

//            DialogForm.ClientSize = new Size(396, 107);
//            DialogForm.Controls.AddRange(new Control[] { label, textBox, buttonOk, buttonCancel });
//            DialogForm.ClientSize = new Size(Math.Max(300, label.Right + 10), DialogForm.ClientSize.Height);
//            DialogForm.FormBorderStyle = FormBorderStyle.FixedDialog;
//            DialogForm.StartPosition = FormStartPosition.CenterScreen;
//            DialogForm.TopMost = true;
//            DialogForm.MinimizeBox = false;
//            DialogForm.MaximizeBox = false;
//            DialogForm.AcceptButton = buttonOk;
//            DialogForm.CancelButton = buttonCancel;


//            DialogResult dialogResult = DialogForm.ShowDialog();
//            value = textBox.Text;
//            return dialogResult;
//        }
//        public DialogResult _YesNoBox(string Question, string Title)  // yes/no user prompt
//        {
//            //// EX: 
//            //var ResultValue = ahk.YesNoBox("Delete " + FileName + "?", "Delete File?");
//            //if (ResultValue.ToString() == "Yes") { ahk.FileDelete(FilePath); }


//            DialogResult result = MessageBox.Show(Question, Title, MessageBoxButtons.YesNo);
//            return result;
//        }
//        public DialogResult _YesNoCancelBox(string Question, string Title)  // yes/no/cancel prompt for user input
//        {
//            //// EX: 
//            //var ResultValue = ahk.YesNoCancelBox("Delete ?", "Delete File?");
//            //if (ResultValue.ToString() == "Cancel") { ahk.ahk.MsgBox("Canceled"); }


//            DialogResult result = MessageBox.Show(Question, Title, MessageBoxButtons.YesNoCancel);
//            return result;
//        }

//        public string _Select_Folder_Dialog()  // popup dialog to select folder path
//        {
//            FolderBrowserDialog folderBrowserDialog1 = new FolderBrowserDialog();

//            DialogResult result = folderBrowserDialog1.ShowDialog();
//            if (result == DialogResult.OK)
//            {
//                //string[] files = Directory.GetFiles(folderBrowserDialog1.SelectedPath);
//                //MessageBox.Show("Files found: " + files.Length.ToString(), "Message");
//                return folderBrowserDialog1.SelectedPath;
//            }

//            return "";
//        }

//        public string _Open_File_Dialog(string InitialDir = @"C:\", string DefaultExt = "txt", string Filter = "Text Files (*.txt)|*.txt|All Files (*.*)|*.*", string Title = "Browse Files")  // Open File Dialog
//        {
//            string SelectedFile = "";

//            OpenFileDialog openFileDialog1 = new OpenFileDialog();

//            openFileDialog1.InitialDirectory = InitialDir;
//            openFileDialog1.Title = Title;

//            openFileDialog1.CheckFileExists = true;
//            openFileDialog1.CheckPathExists = true;

//            openFileDialog1.DefaultExt = DefaultExt;
//            openFileDialog1.Filter = Filter;
//            openFileDialog1.FilterIndex = 2;
//            openFileDialog1.RestoreDirectory = true;

//            openFileDialog1.ReadOnlyChecked = true;
//            openFileDialog1.ShowReadOnly = true;

//            if (openFileDialog1.ShowDialog() == DialogResult.OK)
//            {
//                SelectedFile = openFileDialog1.FileName;
//                //ahk.MsgBox(SelectedFile); 
//                return SelectedFile;
//            }

//            return SelectedFile;
//        }

//        private List<string> _Open_File_Dialog_Multiple()  // Select Multiple Files From Open File Dialog
//        {
//            OpenFileDialog openFileDialog1 = new OpenFileDialog();
//            List<string> SelectedFiles = new List<string>();

//            openFileDialog1.Filter =
//                "Images (*.BMP;*.JPG;*.GIF,*.PNG,*.TIFF)|*.BMP;*.JPG;*.GIF;*.PNG;*.TIFF|" +
//                "All files (*.*)|*.*";

//            openFileDialog1.Multiselect = true;
//            openFileDialog1.Title = "Select Photos";

//            DialogResult dr = openFileDialog1.ShowDialog();
//            if (dr == System.Windows.Forms.DialogResult.OK)
//            {
//                foreach (String file in openFileDialog1.FileNames)
//                {
//                    SelectedFiles.Add(file);
//                }
//            }
//            return SelectedFiles;
//        }

//        public string _Save_File_Dialog(string InitialDirectory = @"C:\", string DefaultExt = "txt", string Filter = "Text Files (*.txt)|*.txt|All Files (*.*)|*.*", string Title = "Save File", bool FileMustExist = false)  // Save File Prompt
//        {
//            string SaveFileName = "";
//            SaveFileDialog saveFileDialog1 = new SaveFileDialog();
//            saveFileDialog1.InitialDirectory = InitialDirectory;
//            if (InitialDirectory != "") { saveFileDialog1.InitialDirectory = InitialDirectory; }
//            saveFileDialog1.Title = "Save text Files";
//            saveFileDialog1.CheckFileExists = FileMustExist;
//            saveFileDialog1.CheckPathExists = FileMustExist;
//            saveFileDialog1.DefaultExt = DefaultExt;
//            saveFileDialog1.Filter = Filter;
//            saveFileDialog1.FilterIndex = 2;
//            saveFileDialog1.RestoreDirectory = true;

//            if (saveFileDialog1.ShowDialog() == DialogResult.OK)
//            {
//                SaveFileName = saveFileDialog1.FileName;
//                ahk.MsgBox("Save File Path: " + SaveFileName);
//                return SaveFileName;
//            }

//            return SaveFileName;
//        }

//        public string _Save_File_As_Dialog(string InitialDirectory = @"C:\", string DefaultExt = "txt", string Filter = "Text Files (*.txt)|*.txt|All Files (*.*)|*.*", string Title = "Save File As") // Save File As Prompt
//        {
//            string SaveFileName = "";
//            SaveFileDialog saveFileDialog1 = new SaveFileDialog();
//            saveFileDialog1.InitialDirectory = InitialDirectory;
//            if (InitialDirectory != "") { saveFileDialog1.InitialDirectory = InitialDirectory; }
//            saveFileDialog1.Title = Title;
//            saveFileDialog1.CheckFileExists = false;
//            saveFileDialog1.CheckPathExists = false;
//            saveFileDialog1.DefaultExt = DefaultExt;
//            saveFileDialog1.Filter = Filter;
//            saveFileDialog1.FilterIndex = 2;
//            saveFileDialog1.RestoreDirectory = true;

//            if (saveFileDialog1.ShowDialog() == DialogResult.OK)
//            {
//                SaveFileName = saveFileDialog1.FileName;
//                //ahk.MsgBox("Save File Path: " + SaveFileName);
//                return SaveFileName;
//            }

//            return SaveFileName;
//        }


//        public Color _Color_Dialog()  // Select Color Dialog
//        {
//            //// select color dialog
//            //Color SelectedColor = ahk.Color_Dialog();

//            //// update control if value returned
//            //if (SelectedColor != Color.Empty)
//            //{
//            //    dataGridView1.BackgroundColor = SelectedColor;
//            //}


//            ColorDialog colorDialog1 = new ColorDialog();

//            if (colorDialog1.ShowDialog() == System.Windows.Forms.DialogResult.OK) { return colorDialog1.Color; }

//            return Color.Empty; // no value selected
//        }

//        //==== Send Key Strokes ======================
//        public void _SendInput(string Keys, int Mode = 1)  // sends input from application to another application (2 different modes to sending - 1 or 2)
//        {
//            if (Mode == 1)  // AHK Send Input
//            {
//                if (sharpAHK._AHK.ahkGlobal.ahkdll == null) { sharpAHK._AHK.ahkGlobal.ahkdll = new AutoHotkey.Interop.AutoHotkeyEngine(); }
//                var ahkdll = sharpAHK._AHK.ahkGlobal.ahkdll;

//                string Command = "SendInput " + Keys;
//                ahkdll.ExecRaw(Command);
//            }

//            if (Mode == 2)  // C# Send Input
//            {
//                SendKeys.SendWait(Keys);
//            }
//        }

//        public string _GetKeyState(string Key)  // checks to see if a key is pressed (returns the current state of that key)
//        {
//            if (sharpAHK._AHK.ahkGlobal.ahkdll == null) { sharpAHK._AHK.ahkGlobal.ahkdll = new AutoHotkey.Interop.AutoHotkeyEngine(); }
//            var ahkdll = sharpAHK._AHK.ahkGlobal.ahkdll;

//            string Command = "GetKeyState , state, " + Key + ", P";
//            ahkdll.ExecRaw(Command);

//            string ReturnVal = ahkdll.GetVar("state");

//            return ReturnVal;
//        }
//        public bool _KeyDown(string Key) // returns true if key is currently pressed down
//        {
//            if (sharpAHK._AHK.ahkGlobal.ahkdll == null) { sharpAHK._AHK.ahkGlobal.ahkdll = new AutoHotkey.Interop.AutoHotkeyEngine(); }
//            var ahkdll = sharpAHK._AHK.ahkGlobal.ahkdll;

//            string Command = "GetKeyState , state, " + Key + ", P";
//            ahkdll.ExecRaw(Command);

//            string ReturnVal = ahkdll.GetVar("state");

//            if (ReturnVal == "U") { return false; }
//            if (ReturnVal == "D") { return true; }

//            return false;
//        }

//        #endregion

//        #region === v1 Static Test ===

//        public static void Msg(string Message)
//        {
//            MessageBox.Show(Message);
//        }


//        #endregion

//        #region === v1 File Functions ===

//        // ##### File Functions ###########

//        public bool _FileAppend(string WriteText, string FileName, bool NewLineReplace = true, bool Debug = true)  // write text to file (starts on new line in file)
//        {
//            // Write single or multiple lines of text to a new or existing text file
//            // Debug Option to enable message box upon Failure
//            // Returns True/False Bool upon Success/Failure
//            // NewLineReplace add option to create new lines with `n in the submitted text

//            if (NewLineReplace) { WriteText = WriteText.Replace("`n", Environment.NewLine); } // replace `n with New Line in Text File


//            try
//            {
//                using (StreamWriter writer =
//                    new StreamWriter(FileName, true))
//                {
//                    writer.WriteLine(WriteText);
//                }
//                return true;
//            }
//            catch
//            {
//                if (Debug == true) { MessageBox.Show("Unable To Write to " + FileName); }
//                return false;
//            }

//        }
//        public bool _FileCopy(string FileToCopy, string FileDestination, bool OverWrite = true)  // copy file from one location to another
//        {
//            try
//            {
//                File.Copy(FileToCopy, FileDestination, OverWrite);
//            }
//            catch (Exception ex)
//            {
//                //Console.WriteLine(ex);
//                // System.IO.FileNotFoundException: Could not find file 'file-missing.txt'.
//                return false;
//            }

//            return true;
//        }
//        public static void _CopyAll(DirectoryInfo source, DirectoryInfo target, bool OverWrite = true)  // function used by FileCopy function to copy list of files to new location
//        {
//            Directory.CreateDirectory(target.FullName);

//            // Copy each file into the new directory.
//            foreach (FileInfo fi in source.GetFiles())
//            {
//                Console.WriteLine(@"Copying {0}\{1}", target.FullName, fi.Name);
//                fi.CopyTo(Path.Combine(target.FullName, fi.Name), OverWrite);
//            }

//            // Copy each subdirectory using recursion.
//            foreach (DirectoryInfo diSourceSubDir in source.GetDirectories())
//            {
//                DirectoryInfo nextTargetSubDir =
//                    target.CreateSubdirectory(diSourceSubDir.Name);
//                _CopyAll(diSourceSubDir, nextTargetSubDir);
//            }
//        }
//        public bool _FileDelete(string FileToDelete, bool PromptUser = false, bool Debug = true)  // deletes file from file path
//        {
//            //ToDo: Finish Delete Prompt Option

//            //if (!SkipPrompt) { ahk.YesNoBox("Delete " + FilePath + "?", "Delete File?"); }  // prompt user to confirm delete
//            //if (SkipPrompt) { ResultValue = "Yes"; } // confirm delete without user interaction

//            //if (ResultValue.ToString() == "Yes")
//            //{
//            //    //LoadImage.Dispose(); 
//            //    //SelectNextNode();  // load next image to free image on pc to delete
//            //    ahk.FileDelete(FilePath);
//            //    Populate_TV();   // reload tree display
//            //    tv.SelectNode(treeView1, NextNode);  // select the next node in tree

//            //    if (scintilla1.Visible) { NewFile(); }  // load new blank file in its place
//            //}

//            if (File.Exists(FileToDelete))
//            {
//                try
//                {
//                    File.Delete(FileToDelete); // Try to delete the file.
//                    return true;
//                }
//                catch (IOException ex)
//                {
//                    if (Debug) { ahk.MsgBox("Unable to Delete " + FileToDelete + Environment.NewLine + ex.ToString()); }
//                    return false;  // We could not delete the file.
//                }
//            }

//            return true;
//        }
//        public bool _FileMove(string FileToMove, string FileDestination, bool OverWrite = true)  // moves file from one location to another
//        {
//            if (File.Exists(FileToMove))
//            {
//                if (OverWrite == true)  // delete the existing destination file in order to overwrite with new file
//                {
//                    if (File.Exists(FileDestination))
//                    {
//                        File.Delete(FileDestination); // Try to delete the file.
//                    }
//                }

//                try
//                {
//                    File.Move(FileToMove, FileDestination); // Try to move
//                    return true;
//                }
//                catch (IOException ex)
//                {
//                    //Console.WriteLine(ex); // Write error
//                    return false;
//                }
//            }

//            return false;
//        }
//        public bool _FileRename(string FileToRename, string NewFileName, bool OverWrite = true)  // renames file on pc
//        {
//            // same as FileMove -- another copy

//            if (File.Exists(FileToRename))
//            {
//                if (OverWrite == true)  // delete the existing destination file in order to overwrite with new file
//                {
//                    if (File.Exists(NewFileName))
//                    {
//                        File.Delete(NewFileName); // Try to delete the file.
//                    }
//                }

//                try
//                {
//                    File.Move(FileToRename, NewFileName); // Try to move
//                    return true;
//                }
//                catch (IOException ex)
//                {
//                    //Console.WriteLine(ex); // Write error
//                    return false;
//                }
//            }

//            return false;
//        }

//        public string _FileReadLine(string FileToRead, int LineNumber)  // reads specific line number in text file to string
//        {
//            if (File.Exists(FileToRead))
//            {
//                string ReturnText = "";

//                // Read every line in the file.
//                using (StreamReader reader = new StreamReader(FileToRead))
//                {
//                    string line;
//                    int IndexCounter = 0;

//                    while ((line = reader.ReadLine()) != null)
//                    {
//                        IndexCounter++;

//                        if (IndexCounter == LineNumber)
//                        {
//                            ReturnText = line;
//                            return ReturnText;
//                        }

//                    }
//                }

//                return ReturnText;
//            }

//            return "UNABLE TO LOCATE " + FileToRead;
//        }
//        public string _FileRead(string FileToRead)  // reads contents of text file to string
//        {
//            if (File.Exists(FileToRead))
//            {
//                string ReturnText = File.ReadAllText(FileToRead);
//                return ReturnText;
//            }

//            return "UNABLE TO LOCATE " + FileToRead;
//        }

//        public bool _IfExist(string path)  // check to see if file/folder exists
//        {
//            if (File.Exists(path))
//            {
//                // This path is a file
//                return true;
//            }
//            else if (Directory.Exists(path))
//            {
//                // This path is a directory
//                return true;
//            }
//            else
//            {
//                return false;
//            }
//        }
//        public bool _IfNotExist(string path)  // check to see if file/folder exists
//        {
//            if (File.Exists(path))
//            {
//                // This path is a file 
//                return false;
//            }
//            else if (Directory.Exists(path))
//            {
//                // This path is a directory
//                return false;
//            }
//            else
//            {
//                return true;
//            }
//        }


//        public string _NextFileName(string fileName)  // returns the next available file name in a folder 
//        {
//            string extension = Path.GetExtension(fileName);

//            int i = 0;
//            while (File.Exists(fileName))
//            {
//                if (i == 0)
//                    fileName = fileName.Replace(extension, "(" + ++i + ")" + extension);
//                else
//                    fileName = fileName.Replace("(" + i + ")" + extension, "(" + ++i + ")" + extension);
//            }

//            return fileName;
//        }

//        public void _WaitForFileToExist(string FileToWaitFor) // waits for file to exist
//        {
//            bool FoundFile = false;
//            while (FoundFile == false)
//            {
//                if (File.Exists(FileToWaitFor))
//                {
//                    FoundFile = true;
//                }

//                ahk.Sleep(500);
//            }
//        }
//        public string _WaitForResponse(string ResponseFile) // waits for file to exist, then returns it's contents as a string
//        {

//            _WaitForFileToExist(ResponseFile);

//            string ReturnText = ahk.FileRead(ResponseFile);

//            return ReturnText.ToString();  // return text from file 
//        }

//        public string _TempText(string Text, bool OpenFile = true)  // write text to temp file and open in notepad (returns temp file path)
//        {
//            string TempFile = ahk.AppDir() + "\\Temp.txt";
//            ahk.FileDelete(TempFile);
//            ahk.FileAppend(Text, TempFile);

//            if (OpenFile) { _Run(TempFile); }

//            return TempFile;
//        }
//        public string _TempCS(string Text, bool OpenFile = true)  // write text to temp .CS file and open in default editor (returns temp file path)
//        {
//            string TempFile = ahk.AppDir() + "\\Temp.cs";
//            ahk.FileDelete(TempFile);
//            ahk.FileAppend(Text, TempFile);

//            if (OpenFile) { _Run(TempFile); }

//            return TempFile;
//        }


//        // File Name Info

//        public string _FileName(string FilePath, bool CheckIfExists = false)  // returns name of file from full file path
//        {
//            if (CheckIfExists)
//            {
//                if (File.Exists(FilePath))
//                {
//                    System.IO.FileInfo fileinfo = new System.IO.FileInfo(FilePath); //retrieve info about each file
//                    return fileinfo.Name.ToString();
//                }
//                return "";
//            }
//            else
//            {
//                System.IO.FileInfo fileinfo = new System.IO.FileInfo(FilePath); //retrieve info about each file
//                return fileinfo.Name.ToString();
//            }
//        }
//        public string _FileNameNoExt(string FilePath, bool CheckIfExists = false)  // returns name of file (no extension) from full file path
//        {
//            if (CheckIfExists)
//            {
//                if (File.Exists(FilePath))
//                {
//                    System.IO.FileInfo fileinfo = new System.IO.FileInfo(FilePath); //retrieve info about each file
//                    return _StringReplace(fileinfo.Name, fileinfo.Extension);
//                }
//                return "";
//            }
//            else
//            {
//                System.IO.FileInfo fileinfo = new System.IO.FileInfo(FilePath); //retrieve info about each file
//                return _StringReplace(fileinfo.Name, fileinfo.Extension);
//            }
//        }
//        public string _FileExt(string FilePath, bool CheckIfExists = false)  // (includex '.' prefix) returns the file extenstion from full file path
//        {
//            if (CheckIfExists)
//            {
//                if (File.Exists(FilePath))
//                {
//                    System.IO.FileInfo fileinfo = new System.IO.FileInfo(FilePath); //retrieve info about each file
//                    return fileinfo.Extension;
//                }
//                return "";
//            }
//            else
//            {
//                System.IO.FileInfo fileinfo = new System.IO.FileInfo(FilePath); //retrieve info about each file
//                return fileinfo.Extension;
//            }

//        }
//        public string _FileDir(string FilePath, bool CheckIfExists = false)  // returns name of file's directory from full file path
//        {
//            if (CheckIfExists)
//            {
//                if (File.Exists(FilePath))
//                {
//                    System.IO.FileInfo fileinfo = new System.IO.FileInfo(FilePath); //retrieve info about each file
//                    return fileinfo.Directory.ToString();
//                }
//                return "";
//            }
//            else
//            {
//                System.IO.FileInfo fileinfo = new System.IO.FileInfo(FilePath); //retrieve info about each file
//                return fileinfo.Directory.ToString();
//            }
//        }
//        public string _DirName(string FilePath, bool CheckIfExists = false)  // returns name of file's directory name from full file path
//        {
//            if (CheckIfExists)
//            {
//                if (File.Exists(FilePath))
//                {
//                    System.IO.FileInfo fileinfo = new System.IO.FileInfo(FilePath); //retrieve info about each file
//                    string s = fileinfo.DirectoryName.ToString();
//                    string DirName = "";
//                    string[] words = s.Split('\\');
//                    foreach (string word in words) { DirName = word; }
//                    return DirName;
//                }
//                return "";
//            }
//            else
//            {
//                System.IO.FileInfo fileinfo = new System.IO.FileInfo(FilePath); //retrieve info about each file

//                string DirName = "";

//                if (fileinfo.DirectoryName != null)
//                {
//                    string s = fileinfo.DirectoryName.ToString();
//                    string[] words = s.Split('\\');
//                    foreach (string word in words) { DirName = word; }
//                }

//                return DirName;
//            }
//        }
//        public string _FileSize(string FilePath, bool CheckIfExists = false)  // returns name of file size in bytes from file path
//        {
//            if (CheckIfExists)
//            {
//                if (File.Exists(FilePath))
//                {
//                    System.IO.FileInfo fileinfo = new System.IO.FileInfo(FilePath); //retrieve info about each file
//                    return fileinfo.Length.ToString();
//                }
//                return "";
//            }
//            else
//            {
//                System.IO.FileInfo fileinfo = new System.IO.FileInfo(FilePath); //retrieve info about each file
//                return fileinfo.Length.ToString();
//            }

//        }

//        public bool _isCompressed(string filePath)  // check whether a file is Compressed
//        {
//            bool isCompressed = ((File.GetAttributes(filePath) & FileAttributes.Compressed) == FileAttributes.Compressed);
//            return isCompressed;
//        }
//        public bool _isEncrypted(string filePath)  // check whether a file is encrypted
//        {
//            bool isEncrypted = ((File.GetAttributes(filePath) & FileAttributes.Encrypted) == FileAttributes.Encrypted);
//            return isEncrypted;
//        }
//        public bool _isReadOnly(string filePath)  // check whether a file is read only
//        {
//            bool isReadOnly = ((File.GetAttributes(filePath) & FileAttributes.ReadOnly) == FileAttributes.ReadOnly);
//            return isReadOnly;
//        }
//        public bool _isHidden(string filePath)  // check whether a file is hidden
//        {
//            bool isHidden = ((File.GetAttributes(filePath) & FileAttributes.Hidden) == FileAttributes.Hidden);
//            return isHidden;
//        }
//        public bool _isArchive(string filePath) // check whether a file has archive attribute
//        {
//            bool isArchive = ((File.GetAttributes(filePath) & FileAttributes.Archive) == FileAttributes.Archive);
//            return isArchive;
//        }
//        public bool _isSystem(string filePath)  // check whether a file is system file
//        {
//            bool isSystem = ((File.GetAttributes(filePath) & FileAttributes.System) == FileAttributes.System);
//            return isSystem;
//        }

//        public DateTime _CreationTime(string FilePath)  // returns the timestamp when the file was created
//        {
//            DateTime creationTime = File.GetCreationTime(FilePath);
//            return creationTime;
//        }
//        public DateTime _LastWriteTime(string FilePath)  // returns the timestamp when the file was written to
//        {
//            DateTime lastWriteTime = File.GetLastWriteTime(FilePath);
//            return lastWriteTime;
//        }
//        public DateTime _LastAccessTime(string FilePath)  // returns the timestamp when the file was last accessed
//        {
//            DateTime lastAccessTime = File.GetLastAccessTime(FilePath);
//            return lastAccessTime;
//        }


//        #endregion

//        #region === v1 Folder Functions ===


//        public void _FileCreateDir(string DirName)  // creates new directory (if it doesn't already exist)
//        {
//            if (!Directory.Exists(DirName))  // create DirName if it doesn't exist
//            {
//                Directory.CreateDirectory(DirName);
//            }
//        }
//        public void _FileCopyDir(string SourceDir, string DestinationDir, bool OverWrite = true)  // copy directory from one path to destination directory
//        {
//            DirectoryInfo diSource = new DirectoryInfo(SourceDir);
//            DirectoryInfo diTarget = new DirectoryInfo(DestinationDir);

//            _CopyAll(diSource, diTarget, OverWrite);
//        }
//        public bool _FileRemoveDir(string DirName)  // remove / delete directory path
//        {
//            if (Directory.Exists(DirName))
//            {
//                try
//                {
//                    Directory.Delete(DirName, true);
//                    return true;
//                }
//                catch
//                {
//                    return false;
//                }

//            }

//            return true;
//        }
//        public void _OpenDir(string DirName)  // opens directory in windows explorer window
//        {
//            if (Directory.Exists(DirName))
//            {
//                try
//                {
//                    Process.Start(DirName);
//                }
//                catch
//                {
//                    //return false;
//                }

//            }
//        }



//        public DataTable _GetDirectoryTable(string SearchDir, bool Recurse = true, string SearchPattern = "*.*")  // converts search directory contents to datatable to display in datagridview
//        {
//            if (!Directory.Exists(SearchDir)) { return null; }

//            //// Display dir files in DataGrid
//            //// Ex: 

//            //bool Recurse = false;
//            //string SearchDir = @"C:\Users\jason\Google Drive\AHK\IMDB\SQLiter\Db\SSRs\507322-9999999";
//            //string SearchPattern = "*.*";

//            //DataTable FileList = ahk.GetDirectoryTable(SearchDir, Recurse, SearchPattern);

//            //dataGridView2.DataSource = FileList; // populate datagrid 

//            //// format the datagrid results

//            //this.dataGridView2.Columns[0].Visible = false;  //FullPath
//            ////this.dataGridView2.Columns[1].Visible = false;  //FileName
//            //this.dataGridView2.Columns[2].Visible = false;  //DirName
//            //this.dataGridView2.Columns[3].Visible = false;  //FileNameNoExt
//            //this.dataGridView2.Columns[4].Visible = false;  //Ext
//            //this.dataGridView2.Columns[5].Visible = false;  //FileSize
//            //this.dataGridView2.Columns[6].Visible = false;  //DateModified

//            //this.dataGridView2.Columns[1].AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells;



//            // returns DataTable with info on files in a folder

//            DataTable FileTable = new DataTable();
//            FileTable.Columns.Add("FullPath", typeof(string));
//            FileTable.Columns.Add("FileName", typeof(string));
//            FileTable.Columns.Add("DirName", typeof(string));
//            FileTable.Columns.Add("FileNameNoExt", typeof(string));
//            FileTable.Columns.Add("Ext", typeof(string));
//            FileTable.Columns.Add("FileSize", typeof(long));
//            FileTable.Columns.Add("DateModified", typeof(DateTime));


//            string[] filelist = null;

//            if (Recurse == false)
//            {
//                filelist = Directory.GetFiles(SearchDir, SearchPattern, SearchOption.TopDirectoryOnly);  // no recurse
//            }

//            if (Recurse == true)
//            {
//                filelist = Directory.GetFiles(SearchDir, SearchPattern, SearchOption.AllDirectories);  // recurse
//            }

//            foreach (string filename in filelist)
//            {
//                // Get Attributes for file.
//                FileInfo info = new FileInfo(filename);

//                string FullPath = info.FullName;
//                string DirName = info.DirectoryName;
//                string DateModified = info.LastWriteTime.ToString();
//                string Ext = info.Extension;
//                string FileName = info.Name;
//                string FileNameNoExt = FileName.Replace(Ext, ""); // remove file extention from file name
//                long length = new System.IO.FileInfo(FullPath).Length; // file size in bytes

//                FileTable.Rows.Add(FullPath, FileName, DirName, FileNameNoExt, Ext, length, DateModified);
//            }

//            return FileTable;

//        }

//        static long _GetDirectorySize(string DirPath)  //returns directory size in bytes
//        {
//            // 1.
//            // Get array of all file names.
//            string[] a = Directory.GetFiles(DirPath, "*.*");

//            // 2.
//            // Calculate total bytes of all files in a loop.
//            long b = 0;
//            foreach (string name in a)
//            {
//                // 3.
//                // Use FileInfo to get length of each file.
//                FileInfo info = new FileInfo(name);
//                b += info.Length;
//            }
//            // 4.
//            // Return total size
//            return b;
//        }

//        public bool _IsDirectory(string FolderPath)  // returns true if path is a directory
//        {
//            // get the file attributes for file or directory
//            FileAttributes attr = File.GetAttributes(FolderPath);

//            //detect whether its a directory or file
//            if ((attr & FileAttributes.Directory) == FileAttributes.Directory)
//                return true;
//            //MessageBox.Show("Its a directory");
//            else
//                return false;
//            //MessageBox.Show("Its a file");
//        }

//        #endregion

//        #region === v1 Application Functions / MISC ===


//        public void _ClipboardWrite(string WriteText)  // writes text to user's clipboard
//        {
//            System.Windows.Forms.Clipboard.SetDataObject(WriteText);
//        }
//        public string _ClipboardGetText()  // returns the contents of the clipboard as string
//        {
//            string clipboard = "";

//            try
//            {
//                IDataObject ClipData = System.Windows.Forms.Clipboard.GetDataObject();

//                if (ClipData.GetDataPresent(DataFormats.Text))
//                {
//                    clipboard = System.Windows.Forms.Clipboard.GetData(DataFormats.Text).ToString();
//                }
//            }
//            catch
//            {
//            }

//            return clipboard;
//        }

//        public void _Test()  // ahk test function
//        {
//            ahk.MsgBox("Test Worked");
//        }


//        public IntPtr _Run(string Target, string Arguments = "", bool Hidden = false)  // run application path. includes win tools (Notepad, cmd, calc, word, paint, regedit, notepad++ as TARGET to launch)
//        {
//            IntPtr ApphWnd = new IntPtr(0);

//            if (Target.ToUpper() == "NOTEPAD") { Target = Environment.SystemDirectory + "\\notepad.exe"; }
//            if (Target.ToUpper() == "CMD" || Target.ToUpper() == "COMMAND") { Target = Environment.SystemDirectory + "\\cmd.exe"; }
//            if (Target.ToUpper() == "CALC" || Target.ToUpper() == "CALCULATOR") { Target = Environment.SystemDirectory + "\\calc.exe"; }
//            if (Target.ToUpper() == "WORD") { Target = Environment.SystemDirectory + "\\word.exe"; }
//            if (Target.ToUpper() == "PAINT") { Target = Environment.SystemDirectory + "\\paint.exe"; }
//            if (Target.ToUpper() == "REGEDIT") { Target = Environment.SystemDirectory + "\\regedit.exe"; }

//            if (Target.ToUpper() == "NOTEPAD++")
//            {
//                Target = @"C:\Google Drive\AHK\Lib\Tool_Library\Office\Notepad++\App\Notepad++\Notepad++.exe";
//                if (!File.Exists(Target)) { Target = @"C:\Users\jason\Google Drive\AHK\Lib\Tool_Library\Office\Notepad++\App\Notepad++\Notepad++.exe"; }
//            }

//            if (Target.ToUpper().Contains("HTTP://"))  //user trying to open url
//            {
//                Process proc;
//                proc = System.Diagnostics.Process.Start(Target);
//                proc.WaitForInputIdle();
//                try { ApphWnd = proc.MainWindowHandle; }
//                catch { }
//                return ApphWnd;
//            }

//            if (File.Exists(Target))
//            {
//                FileInfo info = new FileInfo(Target);

//                // check to see if target is a directory, if so 
//                FileAttributes attr = File.GetAttributes(Target);
//                if ((attr & FileAttributes.Directory) == FileAttributes.Directory)
//                {
//                    //it's a directory
//                    ahk.OpenDir(Target);
//                    return ApphWnd;
//                }


//                ProcessStartInfo startInfo = new ProcessStartInfo();
//                startInfo.WorkingDirectory = info.Directory.ToString();
//                startInfo.FileName = Target;
//                startInfo.Arguments = Arguments;

//                if (Hidden == true)
//                {
//                    startInfo.WindowStyle = ProcessWindowStyle.Hidden;
//                }


//                //Process.Start(startInfo);


//                Process proc;
//                proc = Process.Start(startInfo);
//                try { proc.WaitForInputIdle(); }
//                catch { }

//                try { ApphWnd = proc.MainWindowHandle; }
//                catch { }


//                return ApphWnd;
//            }

//            return ApphWnd;
//        }
//        public bool _RunWait(string ProcessPath, string Arguments = "", bool Hidden = false)  // runs application and waits for the application to end before continuing
//        {
//            if (File.Exists(ProcessPath))
//            {
//                Process process = new Process();
//                process.StartInfo.FileName = ProcessPath;
//                process.StartInfo.Arguments = Arguments;
//                process.StartInfo.ErrorDialog = true;
//                //process.StartInfo.WindowStyle = ProcessWindowStyle.Minimized;
//                if (Hidden == true)
//                {
//                    process.StartInfo.WindowStyle = ProcessWindowStyle.Hidden;
//                }
//                process.Start();
//                process.WaitForExit();
//                return true;
//            }

//            return false;
//        }

//        public void _Sleep(int SleepTime)  // sleeps / idles for x seconds before continuing
//        {
//            Thread.Sleep(SleepTime);
//        }

//        public void _ExitApp()  // exit application
//        {
//            Application.Exit();
//        }


//        #endregion

//        #region === v1 Sound ===

//        public void _SoundBeep(int Frequency = 523, int Duration = 150)  // plays beep sound
//        {
//            if (sharpAHK._AHK.ahkGlobal.ahkdll == null) { sharpAHK._AHK.ahkGlobal.ahkdll = new AutoHotkey.Interop.AutoHotkeyEngine(); }
//            var ahkdll = sharpAHK._AHK.ahkGlobal.ahkdll;

//            string Command = "SoundBeep," + Frequency + "," + Duration;
//            ahkdll.ExecRaw(Command);
//        }



//        #endregion

//        #region=== v1 Strings ===


//        //==== Strings ========================

//        public bool _IfInString(string Needle, string Haystack)  // Returns True if Needle Text is Found in Haystack Text
//        {
//            bool contains = Haystack.Contains(Needle);
//            return contains;
//        }
//        public bool _IfNotInString(string Needle, string Haystack)  // Returns True if Needle NOT Found In Haystack Text
//        {
//            bool contains = Haystack.Contains(Needle);

//            if (contains == true)
//            {
//                return false;
//            }

//            return true;
//        }

//        public string _StringReplace(string InText, string SearchText, string ReplaceText = "")  // replaces text in string, can replace with new text
//        {
//            if (InText == null) { return ""; }  // no source text passed in
//            if (InText == "") { return InText; }  // no source text passed in
//            if (SearchText == "") { return InText; }  // no value passed in to search for

//            string output = InText.Replace(SearchText, ReplaceText);
//            return output;
//        }
//        public string _TrimLastCharacter(string str)  // returns string with the input string's last character trimmed off
//        {
//            if (str == "") { return ""; } // do nothing if blank string

//            return str.TrimEnd(str[str.Length - 1]);
//        }

//        public string _AddLeadingZeros(int InNumber, int TotalReturnLength = 5)  // adds leading zeros to an int, returns string ex: 00012
//        {

//            string fmt = "00000";

//            if (TotalReturnLength == 2) { fmt = "00"; }
//            if (TotalReturnLength == 3) { fmt = "000"; }
//            if (TotalReturnLength == 4) { fmt = "0000"; }
//            if (TotalReturnLength == 5) { fmt = "00000"; }
//            if (TotalReturnLength == 6) { fmt = "000000"; }
//            if (TotalReturnLength == 7) { fmt = "0000000"; }
//            if (TotalReturnLength == 8) { fmt = "00000000"; }
//            if (TotalReturnLength == 9) { fmt = "000000000"; }
//            if (TotalReturnLength == 10) { fmt = "0000000000"; }


//            string ReturnValue = InNumber.ToString(fmt);
//            return ReturnValue;
//        }

//        public string _Remove_Numbers(string InString)  // remove digits from string
//        {
//            var output = Regex.Replace(InString, @"[\d-]", string.Empty);
//            return output.ToString();
//        }

//        public static string _UnHtml(string value) // remove HTML characters from string
//        {
//            var step1 = Regex.Replace(value, @"<[^>]+>|&nbsp;", "").Trim();
//            var step2 = Regex.Replace(step1, @"\s{2,}", " ");
//            return step2;
//        }
//        public string _FixSpecialChars(string str)  // format string to be compatible with SQL Inserts
//        {
//            //str = str.Trim();  // trim leading/end spaces 
//            //str = str.Replace("\"", @"\""");
//            str = str.Replace("'", "''");
//            return str;
//        }

//        public int _CharCount(string Line, string Char)  // returns the number of times a character is found in a string
//        {
//            char Char1 = Char[0];
//            int CharCount = Line.Count(f => f == Char1);  // count number of brackets in line
//            return CharCount;
//        }


//        public string _TrimFirst(string str, int RemoveCharacterCount = 1)  // Remove X Characters from beginning of string
//        {
//            if (str == "") { return ""; } // do nothing if blank string

//            string returnstring = str.Remove(0, RemoveCharacterCount);
//            return returnstring;
//        }
//        public string _TrimLast(string str, int RemoveCharacterCount = 1)  // Remove X Characters from end of string
//        {
//            if (str == "") { return ""; } // do nothing if blank string

//            return str.TrimEnd(str[str.Length - RemoveCharacterCount]);
//        }


//        public string _FirstCharacters(string Text, int NumberOfCharacters = 1) // returns first X characters in string
//        {
//            Text = Text.Trim();
//            if (Text == "") { return ""; }

//            if (Text.Length < NumberOfCharacters) { return ""; }

//            string str = Text.Substring(0, NumberOfCharacters);
//            return str;
//        }

//        public string _LastCharacters(string Text, int NumberOfCharacters = 1) // returns last X characters in string
//        {
//            Text = Text.Trim();
//            if (Text == "") { return ""; }
//            string str = Text.Substring(Text.Length - NumberOfCharacters, NumberOfCharacters);
//            return str;
//        }


//        public string _FirstWord(string InputString)  // returns First word in string 
//        {
//            string ReturnText = _WordNum(InputString, 1);
//            return ReturnText;
//        }
//        public string _LastWord(string InputString)  // returns last word in string 
//        {
//            List<string> WordsList = _WordList(InputString);

//            string ReturnWord = "";
//            foreach (string word in WordsList)
//            {
//                ReturnWord = word;
//            }

//            return ReturnWord;
//        }

//        public string _WordNum(string InputString, int WordNumber = 1)  // return specific word # from string
//        {
//            InputString = InputString.Trim();

//            List<string> List = _WordList(InputString);
//            int counter = 0;
//            foreach (string word in List)
//            {
//                counter++;
//                if (counter == WordNumber)
//                {
//                    return word;
//                }
//            }

//            return "";
//        }

//        public List<string> _WordList(string InputString)  // parse line by space, returns list of words
//        {
//            //List<string> WordList = WordList(InText); 
//            //foreach(string Word in WordList)


//            List<string> ReturnList = new List<string>();
//            string[] words = InputString.Split(' ');
//            foreach (string word in words)
//            {
//                string worda = word.Trim();
//                ReturnList.Add(worda);
//            }
//            return ReturnList;
//        }

//        public List<string> _LineList(string InputString, bool Trim = true, bool RemoveBlanks = false)  // parse line by new line, returns list of lines
//        {
//            List<string> ReturnList = new List<string>();
//            string[] lines = InputString.Split(Environment.NewLine.ToCharArray());
//            foreach (string line in lines)
//            {
//                string wline = line;

//                if (Trim) { wline = line.Trim(); }  // option to trim text line when creating list

//                if (RemoveBlanks) { if (line.Trim() == "") { continue; } }

//                ReturnList.Add(wline);
//            }
//            return ReturnList;
//        }

//        public bool _WordInString(string Line) // check to see if word is found in text line
//        {
//            List<string> WordLists = ahk.WordList(Line);
//            foreach (string Word in WordLists)
//            {
//                if (Word.ToUpper() == Line.ToUpper()) { return true; }
//            }

//            return false;
//        }


//        public string _RemoveComments(string line, string CommentCharacters = "//") // Returns code line without comments
//        {
//            //=== parse line of C# code, returns Code as output and returns comment in the out field ===

//            //string line = "public class SQLite  // JDL SQLite Collection";

//            string lineCode = line.Trim();  //trim extra spaces from line
//            string First2Chars = "";
//            string Code = "";

//            //================================
//            // Parse String by Space
//            //================================
//            string[] words = lineCode.Split(' ');
//            foreach (string word in words)
//            {
//                if (word == "") { continue; } //skip blank lines
//                //ahk.ahk.MsgBox(word); 

//                if (word.Length >= 2) { First2Chars = word.Substring(0, 2); } //returns the first two characters of the word (to see if a comment is starting)

//                if (First2Chars == CommentCharacters)
//                {
//                    return Code;
//                }

//                if (Code == "") { Code = word; continue; }
//                if (Code != "") { Code = Code + " " + word; continue; }
//            }
//            return Code;
//        }

//        public string _ReturnComments(string line, string CommentCharacters = "//") // Returns comments on line after code
//        {
//            //=== parse line of C# code, returns Code as output and returns comment in the out field ===

//            //string line = "public class SQLite  // JDL SQLite Collection";

//            string lineCode = line.Trim();  //trim extra spaces from line
//            string First2Chars = "";
//            string Code = "";

//            //================================
//            // Parse String by Space
//            //================================
//            string[] words = lineCode.Split(' ');
//            bool Capture = false;
//            foreach (string word in words)
//            {
//                if (word == "") { continue; } //skip blank lines
//                //ahk.ahk.MsgBox(word); 

//                if (word.Length >= 2) { First2Chars = word.Substring(0, 2); } //returns the first two characters of the word (to see if a comment is starting)

//                if (First2Chars == CommentCharacters)
//                {
//                    Capture = true;
//                }

//                if (Capture)
//                {
//                    if (Code == "") { Code = word; continue; }
//                    if (Code != "") { Code = Code + " " + word; continue; }
//                }

//            }
//            return Code;
//        }


//        public string _Extract_Between(string Code, string start = "{", string end = "}")  // extracts text between brackets (single character search)
//        {
//            //ahk.ahk.MsgBox(Code); 

//            string OutCode = "";
//            bool StartCapture = false;
//            int BracketCount = 0;

//            string[] lines = Code.Split(new string[] { Environment.NewLine }, StringSplitOptions.None);
//            foreach (string line in lines)
//            {
//                string LineCode = line.Trim();

//                if (LineCode == "") { continue; }

//                if (!StartCapture)
//                {
//                    if (line.Contains(start)) { StartCapture = true; BracketCount++; continue; }
//                }

//                if (StartCapture)
//                {
//                    if (line.Contains(start)) { StartCapture = true; BracketCount++; }
//                }


//                if (BracketCount == 1)
//                {
//                    if (line.Contains(end)) { BracketCount--; continue; }
//                }

//                if (BracketCount > 1)
//                {
//                    if (line.Contains(end)) { BracketCount--; }
//                }


//                if (StartCapture)
//                {
//                    OutCode = OutCode + Environment.NewLine + line;
//                }

//                if (StartCapture) { if (BracketCount == 0) { return OutCode; } }

//            }

//            //ahk.ahk.MsgBox(OutCode);
//            return OutCode;
//        }


//        public string _Insert_Text(string InText, string InsertText, int Position)  // insert text into specific position in string
//        {
//            string inserted = InText.Substring(0, Position) + InsertText + InText.Substring(Position);
//            return inserted;
//        }



//        public string _StringSplit(string InText, string SplitChar = "(", int ReturnPos = 0)  // split string by character (return pos starts at word 0)
//        {
//            char[] delimiterChars = { Char.Parse(SplitChar) };

//            string[] words = InText.Split(delimiterChars);

//            string ReturnText = "";

//            if (ReturnPos == 0) { ReturnText = words.First(); }  // return text before the split string char

//            if (ReturnPos != 0)
//            {
//                int partnum = 0;
//                foreach (string part in words)
//                {
//                    if (partnum == ReturnPos && part != "") { ReturnText = part; }

//                    partnum++;
//                }

//            } // return second part of split string line


//            return ReturnText;
//        }


//        public string Add_Leading_Spaces(string InText, int SpaceCount)  // add leading spaces before a string
//        {
//            string spaces = "";
//            int i = 0;
//            do
//            {
//                spaces = spaces + " ";
//                i++;
//            } while (i < SpaceCount);

//            //spaces = "[" + spaces + "]";

//            InText = spaces + InText;  // add leading spaces to intext

//            return InText;
//        }


//        public int _Leading_Space_Count(string InText)  // returns number of leading spaces before text begins
//        {
//            return InText.TakeWhile(Char.IsWhiteSpace).Count();
//        }

//        public static string _ToProperCase(string text)  // convert string to Proper casing -- Output: This Is A String Test
//        {
//            System.Globalization.CultureInfo cultureInfo = System.Threading.Thread.CurrentThread.CurrentCulture;
//            System.Globalization.TextInfo textInfo = cultureInfo.TextInfo;
//            return textInfo.ToTitleCase(text);
//        }


//        public string _Closest_FileName(string SearchTerm, List<string> SearchList, bool Debug = false)  // search list for Contains match - otherwise take close word match
//        {
//            string close = "";
//            foreach (string imaGe in SearchList)
//            {
//                if (imaGe.ToUpper().Contains(SearchTerm.ToUpper()))
//                {
//                    close = imaGe;
//                    if (Debug) { ahk.MsgBox("In String: " + close); }
//                }
//            }

//            if (close == "")  // no exact matches found - try closest match
//            {
//                close = _Closest_Word(SearchTerm, SearchList);
//                if (Debug) { ahk.MsgBox("Closest Word: " + close); }
//            }

//            return close;
//        }

//        public static string _Closest_Word(string SearchWord, List<string> WordList)  // find the closest match in a list to search word
//        {
//            string[] terms = WordList.ToArray();

//            string term = SearchWord.ToLower();
//            List<string> list = terms.ToList();
//            if (list.Contains(term))
//                return list.Find(t => t.ToLower() == term);
//            else
//            {
//                int[] counter = new int[terms.Length];
//                for (int i = 0; i < terms.Length; i++)
//                {
//                    for (int x = 0; x < Math.Min(term.Length, terms[i].Length); x++)
//                    {
//                        int difference = Math.Abs(term[x] - terms[i][x]);
//                        counter[i] += difference;
//                    }
//                }

//                int min = counter.Min();
//                int index = counter.ToList().FindIndex(t => t == min);
//                return terms[index];
//            }
//        }

//        /// <summary>
//        /// Contains approximate string matching
//        /// </summary>
//        static class LevenshteinDistance
//        {

//            // ex: 
//            //int last = -1; int val = -1; 
//            //foreach(string imaGe in imgNames)
//            //{
//            //    val = LevenshteinDistance.Compute(ItemName, imaGe);

//            //    if (last == -1) { last = val; close = imaGe; }
//            //    if (val < last) { last = val; close = imaGe; }
//            //}

//            //ahk.ahk.MsgBox(ItemName + " : " + close + " (" + last.ToString() + ")");


//            /// <summary>
//            /// Compute the distance between two strings.
//            /// </summary>
//            public static int _Compute(string s, string t)
//            {
//                int n = s.Length;
//                int m = t.Length;
//                int[,] d = new int[n + 1, m + 1];

//                // Step 1
//                if (n == 0)
//                {
//                    return m;
//                }

//                if (m == 0)
//                {
//                    return n;
//                }

//                // Step 2
//                for (int i = 0; i <= n; d[i, 0] = i++)
//                {
//                }

//                for (int j = 0; j <= m; d[0, j] = j++)
//                {
//                }

//                // Step 3
//                for (int i = 1; i <= n; i++)
//                {
//                    //Step 4
//                    for (int j = 1; j <= m; j++)
//                    {
//                        // Step 5
//                        int cost = (t[j - 1] == s[i - 1]) ? 0 : 1;

//                        // Step 6
//                        d[i, j] = Math.Min(
//                            Math.Min(d[i - 1, j] + 1, d[i, j - 1] + 1),
//                            d[i - 1, j - 1] + cost);
//                    }
//                }
//                // Step 7
//                return d[n, m];
//            }
//        }



//        #endregion

//        #region === v1 RegEx ===


//        //#################################
//        //           RegEx
//        //#################################


//        public Regex _regexProductionNumber = new Regex(@"\d{1,3}\b\.\s", RegexOptions.CultureInvariant | RegexOptions.Compiled);

//        public Regex _regexEpisodeNumber = new Regex(@"\d{1,2}-\d{1,2}", RegexOptions.IgnoreCase | RegexOptions.CultureInvariant | RegexOptions.IgnorePatternWhitespace | RegexOptions.Compiled);

//        public Regex _regexAirDate = new Regex("\\d\\d\\s\\b\\w{3,4}\\b\\s\\d\\d", RegexOptions.IgnoreCase | RegexOptions.CultureInvariant | RegexOptions.IgnorePatternWhitespace | RegexOptions.Compiled);

//        public Regex _regexEpisodeTitle = new Regex(@"(?<=>)(.*)(?=</a>)", RegexOptions.IgnoreCase | RegexOptions.CultureInvariant | RegexOptions.IgnorePatternWhitespace | RegexOptions.Compiled);


//        // from 1-3 format, pull the 1 or the 3 for season/episode
//        public Regex _regexSeasonNum = new Regex(@"\d{1,2}(?=-)", RegexOptions.IgnoreCase | RegexOptions.CultureInvariant | RegexOptions.IgnorePatternWhitespace | RegexOptions.Compiled);
//        public Regex _regexEpNum = new Regex(@"(?<=-)\d{1,2}", RegexOptions.IgnoreCase | RegexOptions.CultureInvariant | RegexOptions.IgnorePatternWhitespace | RegexOptions.Compiled);


//        //\d{3}\b\.\s  = Episode Number (101)

//        //\b\d{1,2}\b-\d\d  episode number (5-13)

//        //\d\d\s\b\w{3,4}\b\s\d\d  air date

//        //>(.*)</a>  episode title


//        /// <summary>
//        ///  Regular expression built for C# on: Wed, Apr 13, 2016, 03:14:33 PM
//        ///  Using Expresso Version: 3.0.5854, http://www.ultrapico.com
//        ///  
//        ///  A description of the regular expression:
//        ///  
//        ///  <h4>
//        ///      <h4>
//        ///  [1]: A numbered capture group. [.|\n], any number of repetitions, as few as possible
//        ///      Select from 2 alternatives
//        ///          Any character
//        ///          New line
//        ///  <\/h4>
//        ///      <
//        ///      Literal /
//        ///      h4>
//        ///  
//        ///
//        /// </summary>
//        public Regex _MyRegex = new Regex(
//              "<h4>(.|\\n)*?<\\/h4>",
//            RegexOptions.IgnoreCase
//            | RegexOptions.IgnorePatternWhitespace
//            );


//        // This is the replacement string
//        public string _MyRegexReplace =
//              "$& [${Day}-${Month}-${Year}]";



//        public void _MatchRegex()
//        {
//            string InputText = ahk.FileRead(@"C:\Users\jason\Google Drive\AHK\MDb\IMDB\RegExHTML\AHKPage.html").ToString();

//            //// Capture all Matches in the InputText
//            MatchCollection matches = _MyRegex.Matches(InputText.ToString());


//            string ResultLine = "";
//            foreach (Match match in matches)
//            {
//                try
//                {
//                    ResultLine = match.Value.ToString();
//                    ahk.FileAppend(ResultLine, @"C:\Users\jason\Google Drive\AHK\MDb\IMDB\RegExHTML\AHKPage_Parse1.txt");
//                    //MessageBox.Show(match.Value.ToString());
//                }
//                catch { }
//            }

//        }

//        //// Replace the matched text in the InputText using the replacement pattern
//        // string result = MyRegex.Replace(InputText,MyRegexReplace);

//        //// Split the InputText wherever the regex matches
//        // string[] results = MyRegex.Split(InputText);

//        //// Capture the first Match, if any, in the InputText
//        // Match m = MyRegex.Match(InputText);

//        //// Test to see if there is a match in the InputText
//        // bool IsMatch = MyRegex.IsMatch(InputText);

//        //// Get the names of all the named and numbered capture groups
//        // string[] GroupNames = MyRegex.GetGroupNames();

//        //// Get the numbers of all the named and numbered capture groups
//        // int[] GroupNumbers = MyRegex.GetGroupNumbers();


//        #endregion

//        #region === v1 Download ===

//        //====== Download Web Page =====================================
//        // string HTML = ahk.DownloadHTML("http://www.imdb.com/title/tt1985949/", "c:\\HTML.txt");  
//        public string _DownloadHTML(string theURL, string SaveFile = "", string login = "", string pass = "")  // downloads html string from URL path
//        {
//            ////### download a web page to a string
//            //WebClient client = new WebClient();

//            //if (login != "")  // option to set login parameters
//            //{
//            //    client.Credentials = new System.Net.NetworkCredential(login, pass);
//            //}


//            //client.Headers.Add("user-agent", "Mozilla/4.0 (compatible; MSIE 6.0; Windows NT 5.2; .NET CLR 1.0.3705;)");

//            //Stream data = client.OpenRead(theURL);
//            //StreamReader reader = new StreamReader(data);
//            //string s = reader.ReadToEnd();


//            //if (SaveFile != "")
//            //{
//            //    ahk.FileDelete(SaveFile);
//            //    FileAppend(s, SaveFile);
//            //    Run(SaveFile);
//            //}


//            //return s;

//            return "";
//        }


//        #endregion

//        #region === v1 Objects ===

//        public struct Rect
//        {
//            public int Left { get; set; }
//            public int Top { get; set; }
//            public int Right { get; set; }
//            public int Bottom { get; set; }

//            public int Width { get; set; }
//            public int Height { get; set; }
//        }   // used to return window position

//        public struct Coordinates
//        {
//            public int XPos { get; set; }
//            public int YPos { get; set; }

//        }   // used to return window position

//        public struct conInfo  // control info parameters
//        {
//            public string ControlName { get; set; }
//            public string ControlText { get; set; }
//            public IntPtr ControlHwnd { get; set; }
//            public string ControlClass { get; set; }
//            public string ControlValue { get; set; }

//            public int ControlX { get; set; }
//            public int ControlY { get; set; }
//            public int ControlW { get; set; }
//            public int ControlH { get; set; }

//            public bool ControlChecked { get; set; }
//            public bool ControlEnabled { get; set; }
//            public bool ControlVisible { get; set; }
//        }

//        public struct wInfo  // window / mouse / control info parameters
//        {
//            public int MouseXPos { get; set; }
//            public int MouseYPos { get; set; }

//            public string WinTitle { get; set; }
//            public IntPtr WinHwnd { get; set; }
//            public string WinClass { get; set; }
//            public string WinPID { get; set; }
//            public string WinText { get; set; }

//            public int WinX { get; set; }
//            public int WinY { get; set; }
//            public int WinW { get; set; }
//            public int WinH { get; set; }

//            public IntPtr ControlHwnd { get; set; }

//        }   // used to return window position

//        public void wInfo_Display(wInfo info)  // display wInfo (win info) object contents in messagebox
//        {
//            string MouseMsg = "";
//            MouseMsg = "MouseXPos: " + info.MouseXPos.ToString() + Environment.NewLine;
//            MouseMsg = MouseMsg + "MouseYPos: " + info.MouseYPos.ToString() + Environment.NewLine;
//            //ahk.ahk.MsgBox(MouseMsg, "Mouse.Info");

//            if (info.WinTitle == null) { info.WinTitle = ""; }
//            if (info.WinClass == null) { info.WinClass = ""; }
//            if (info.WinText == null) { info.WinText = ""; }

//            string WinMsg = "";
//            WinMsg = WinMsg + "WinTitle: " + info.WinTitle.ToString() + Environment.NewLine;
//            WinMsg = WinMsg + "WinHwnd: " + info.WinHwnd.ToString() + Environment.NewLine;
//            WinMsg = WinMsg + "WinClass: " + info.WinClass.ToString() + Environment.NewLine;
//            WinMsg = WinMsg + "WinText: " + info.WinText.ToString() + Environment.NewLine;
//            WinMsg = WinMsg + "WinX: " + info.WinX.ToString() + Environment.NewLine;
//            WinMsg = WinMsg + "WinY: " + info.WinY.ToString() + Environment.NewLine;
//            //WinMsg = WinMsg + "WinRight: " + info.WinRight.ToString() + Environment.NewLine;
//            //WinMsg = WinMsg + "WinBottom: " + info.WinBottom.ToString() + Environment.NewLine;
//            WinMsg = WinMsg + "WinWidth: " + info.WinW.ToString() + Environment.NewLine;
//            WinMsg = WinMsg + "WinHeight: " + info.WinH.ToString() + Environment.NewLine;
//            //ahk.ahk.MsgBox(WinMsg, "Win.Info");

//            //if (info.ControlName == null) { info.ControlName = ""; }
//            //if (info.ControlText == null) { info.ControlText = ""; }
//            //if (info.ControlClass == null) { info.ControlClass = ""; }
//            //if (info.ControlValue == null) { info.ControlValue = ""; }

//            //string ControlMsg = "";
//            //ControlMsg = ControlMsg + "ControlName: " + info.ControlName.ToString() + Environment.NewLine;
//            //ControlMsg = ControlMsg + "ControlText: " + info.ControlText.ToString() + Environment.NewLine;
//            WinMsg = WinMsg + "ControlHwnd: " + info.ControlHwnd.ToString() + Environment.NewLine;
//            //ControlMsg = ControlMsg + "ControlClass: " + info.ControlClass.ToString() + Environment.NewLine;
//            //ControlMsg = ControlMsg + "ControlValue: " + info.ControlValue.ToString() + Environment.NewLine;
//            //ControlMsg = ControlMsg + "ControlX: " + info.ControlX.ToString() + Environment.NewLine;
//            //ControlMsg = ControlMsg + "ControlY: " + info.ControlY.ToString() + Environment.NewLine;
//            //ControlMsg = ControlMsg + "ControlW: " + info.ControlW.ToString() + Environment.NewLine;
//            //ControlMsg = ControlMsg + "ControlH: " + info.ControlH.ToString() + Environment.NewLine;
//            //ControlMsg = ControlMsg + "ControlChecked: " + info.ControlChecked.ToString() + Environment.NewLine;
//            //ControlMsg = ControlMsg + "ControlEnabled: " + info.ControlEnabled.ToString() + Environment.NewLine;
//            //ControlMsg = ControlMsg + "ControlVisible: " + info.ControlVisible.ToString() + Environment.NewLine;
//            //ahk.ahk.MsgBox(ControlMsg, "Control.Info");

//            //string ALL = MouseMsg + Environment.NewLine + Environment.NewLine + WinMsg + Environment.NewLine + Environment.NewLine + ControlMsg;
//            string ALL = MouseMsg + Environment.NewLine + WinMsg + Environment.NewLine; // +ControlMsg;
//            ahk.MsgBox(ALL, "Mouse.Win.Control Info");
//        }


//        public string _VarType(object Object)  // returns type of variable passed into object parameter
//        {
//            string VarType = Object.GetType().ToString();  //determine what kind of variable was passed into function

//            //### CONTROLS #########################
//            //System.Windows.Forms.Button
//            //System.Windows.Forms.CheckBox
//            //System.Windows.Forms.DataGridView
//            //System.Windows.Forms.ListBox
//            //System.Windows.Forms.PictureBox
//            //System.Windows.Forms.TabControl
//            //System.Windows.Forms.TabPage
//            //System.Windows.Forms.TableLayoutPanel
//            //System.Windows.Forms.TextBox
//            //System.Windows.Forms.ToolStripMenuItem
//            //System.Windows.Forms.TreeView

//            //ScintillaNET.Scintilla
//            //TreeViewFast.Controls.TreeViewFast


//            //### VARIABLES #########################
//            // System.String (string)
//            // System.Int32 (int)
//            // System.Int64 (long)
//            // System.Collections.Generic.List`1(System.String)  (List<string>)
//            // System.Collections.Generic.List`1(System.Int32)   (List<int>)

//            return VarType;
//        }

//        #endregion

//        #region === v1 Process Functions ===

//        //################################################
//        //      Process Functions
//        //################################################

//        public string _ProcessPath(object WinTitle)  //returns exe path of window by wintitle or handle
//        {
//            IntPtr hWnd = _WinHwnd(WinTitle);  //returns Window Handle (from either handle or window title)

//            uint pid = 0;
//            _GetWindowThreadProcessId(hWnd, out pid);
//            if (hWnd != IntPtr.Zero)
//            {
//                if (pid != 0)
//                {
//                    var process = Process.GetProcessById((int)pid);
//                    if (process != null)
//                    {
//                        return process.MainModule.FileName.ToString();
//                    }
//                }
//            }
//            return "";
//        }

//        public List<string> _ProcessList()  // returns list of running processes
//        {
//            List<string> pList = new List<string>();

//            Process[] processlist = Process.GetProcesses();

//            foreach (Process process in processlist)
//            {
//                if (!String.IsNullOrEmpty(process.MainWindowTitle))
//                {
//                    //Console.WriteLine("Process: {0} ID: {1} Window title: {2}", process.ProcessName, process.Id, process.MainWindowTitle);
//                    //ahk.MsgBox("ProcessName: " + process.ProcessName + Environment.NewLine + "ProcessID: " + process.Id + Environment.NewLine + "WinTitle: " + process.MainWindowTitle);
//                    pList.Add(process.MainWindowTitle); // wintitle
//                }
//            }

//            return pList;
//        }

//        public List<string> _All_WinTitles_By_ProcessName(string processName = "mpc-hc64") // loop through all processes for process name, return all matching window titles
//        {
//            List<string> MPC_WinTitles = new List<string>();

//            Process[] processlist = Process.GetProcesses();

//            foreach (Process process in processlist)
//            {
//                if (!String.IsNullOrEmpty(process.MainWindowTitle))
//                {
//                    if (process.ProcessName == processName) { MPC_WinTitles.Add(process.MainWindowTitle); }
//                    //Console.WriteLine("Process: {0} ID: {1} Window title: {2}", process.ProcessName, process.Id, process.MainWindowTitle);
//                }
//            }

//            return MPC_WinTitles;
//        }

//        #endregion

//        #region === v1 Embed Application Example ===

//        //=== Embed Application in GUI =============

//        //=== Embed Notepad++

//        public void _Embed_Application_Example(Panel panelName, string ApplicationPath = "NotePad")  // example for embedding another application inside a winform panel (docking to c# application)
//        {
//            //// must create panel1 in C# gui

//            ////string NpadPlus = @"C:\Google Drive\AHK\Lib\Tool_Library\Office\Notepad++\App\Notepad++\Notepad++.exe";
//            ////if (!File.Exists(NpadPlus))
//            ////{
//            ////    NpadPlus = @"C:\Users\jason\Google Drive\AHK\Lib\Tool_Library\Office\Notepad++\App\Notepad++\Notepad++.exe";
//            ////}

//            ////NpadPlus = @"C:\Program Files (x86)\Google\Chrome\Application\chrome.exe";    // Chrome doesn't work, launches in own window
//            ////NpadPlus = @"C:\Users\langaj\Documents\SQLiteBrowser\App\SQLiteDatabaseBrowser64\SQLitebrowser.exe";  //doesn't work either

//            //Process proc;
//            //proc = Process.Start(ApplicationPath);
//            //proc.WaitForInputIdle();
//            //SetParent(proc.MainWindowHandle, panelName.Handle);
//            //NpadhWnd = proc.MainWindowHandle;
//            //MaximizeWin(NpadhWnd);
//            //Thread.Sleep(500);
//        }

//        public void _SetWinParent(IntPtr hWndChild, IntPtr hWndNewParent)  // set the window parent (used to dock applications inside current tool)
//        {
//            _SetParent(hWndChild, hWndNewParent);
//        }


//        #endregion

//        #region === v1 Mouse ===

//        //======= Mouse Click ======================


//        public void _MouseClick(int xpos, int ypos, string Button = "Left")  // simulates left/right mouse click on screen
//        {
//            _SetCursorPos(xpos, ypos);

//            Button = Button.ToLower();  // convert options to lower case

//            if (Button == "left")
//            {
//                mouse_event(MOUSEEVENTF_LEFTDOWN, xpos, ypos, 0, 0);
//                mouse_event(MOUSEEVENTF_LEFTUP, xpos, ypos, 0, 0);
//            }

//            if (Button == "right")
//            {
//                mouse_event(MOUSEEVENTF_RIGHTDOWN, xpos, ypos, 0, 0);
//                mouse_event(MOUSEEVENTF_RIGHTUP, xpos, ypos, 0, 0);
//            }

//        }

//        public void _MouseMove(int xpos, int ypos)  //moves mouse cursor to position on screen
//        {
//            _SetCursorPos(xpos, ypos);
//        }

//        //==== Get Mouse Pos =============
//        //EX: 
//        //  var MousePos = ahk.GetMousePos();
//        //  string MouseX = MousePos.XPos.ToString();
//        //  string MouseY = MousePos.YPos.ToString();

//        public wInfo _GetMousePos()  // gets the current mouse position, returns wInfo object populated
//        {
//            wInfo MousePos = new wInfo();
//            MousePos.MouseXPos = Cursor.Position.X;
//            MousePos.MouseYPos = Cursor.Position.Y;

//            return MousePos;
//        }

//        public wInfo _MouseGetPos()  // gets the current mouse position, returns wInfo object populated (includes control info/handles)
//        {
//            if (sharpAHK._AHK.ahkGlobal.ahkdll == null) { sharpAHK._AHK.ahkGlobal.ahkdll = new AutoHotkey.Interop.AutoHotkeyEngine(); }
//            var ahkdll = sharpAHK._AHK.ahkGlobal.ahkdll;

//            wInfo mouseInfo = new wInfo();

//            //ahkdll.SetVar("OutputVar", "");
//            string Command = "MouseGetPos, MouseX, MouseY, WinID, ControlID, 3";
//            ahkdll.ExecRaw(Command);

//            mouseInfo.MouseXPos = Int32.Parse(ahkdll.GetVar("MouseX"));
//            mouseInfo.MouseYPos = Int32.Parse(ahkdll.GetVar("MouseY"));

//            // convert string to IntPtr
//            string WinHwnd = ahkdll.GetVar("WinID");
//            if (WinHwnd != "")
//            {
//                int WinID = Int32.Parse(WinHwnd);  // string to int
//                mouseInfo.WinHwnd = _ToIntPtr(WinID);  // int to IntPtr
//            }

//            // convert string to IntPtr
//            string ControlHwnd = ahkdll.GetVar("ControlID");
//            if (ControlHwnd != "")
//            {
//                int cID = Int32.Parse(ControlHwnd);  // string to int
//                mouseInfo.ControlHwnd = _ToIntPtr(cID);  // int to IntPtr
//            }

//            //ahk.MsgBox("X=" + mouseInfo.MouseXPos.ToString());
//            //ahk.MsgBox("Y=" + mouseInfo.MouseYPos.ToString());
//            //ahk.MsgBox("WinHwnd=" + mouseInfo.WinHwnd.ToString());
//            //ahk.MsgBox("ControlHwnd=" + mouseInfo.ControlHwnd.ToString());

//            _ControlGetPos(null, mouseInfo.ControlHwnd);


//            return mouseInfo;
//        }


//        #endregion

//        #region === v1 Convert ===

//        public int _ToInt(object Input)  // convert string to int
//        {
//            if (Input == "") { return 0; }
//            //ahk.MsgBox("Input: " + Input.ToString());

//            string VarType = Input.GetType().ToString();  //determine what kind of variable was passed into function
//            int Out = 0;

//            if (VarType == "System.Boolean")
//            {
//                string InputBoolString = Input.ToString();
//                if (InputBoolString == "True")
//                {
//                    return 1;
//                }
//                else
//                {
//                    return 0;
//                }
//            }

//            if (VarType == "System.String")
//            {
//                Out = Int32.Parse(Input.ToString());  // string to int
//                return Out;
//            }

//            if (VarType == "System.IntPtr")
//            {
//                Out = (int)Input;
//                return Out;
//            }

//            return Out;
//        }

//        public IntPtr _ToIntPtr(object Input)  //convert string / int to IntPtr
//        {
//            string VarType = Input.GetType().ToString();  //determine what kind of variable was passed into function

//            IntPtr nIntPtr = new IntPtr(0);

//            if (VarType == "System.Int32" || VarType == "System.String") //String/Int32 to IntPtr
//            {
//                int ConvertInt = Int32.Parse(Input.ToString());  // convert object to int
//                IntPtr myPtr = new IntPtr(ConvertInt); //convert Int to IntPtr
//                return myPtr;
//            }

//            //if (VarType == "System.IntPtr")

//            return nIntPtr;
//        }

//        public static string _IntPtr_ToString(Encoding encoding, IntPtr ptr, int length)  // untested
//        {
//            //	null pointer = null string
//            if (ptr == IntPtr.Zero)
//                return null;

//            //	0 length string = string.Empty
//            if (length == 0)
//                return string.Empty;

//            byte[] buff = new byte[length];
//            Marshal.Copy(ptr, buff, 0, length);

//            //	We don't want to carry over the Trailing null
//            if (buff[buff.Length - 1] == 0)
//                length--;

//            return encoding.GetString(buff, 0, length);
//        }

//        public DateTime _ToDateTime(object TimeString)  //Convert To DateTime var
//        {
//            string VarType = TimeString.GetType().ToString();  //determine what kind of variable was passed into function

//            if (VarType == "System.String")
//            {
//                DateTime enteredDate = DateTime.Parse(TimeString.ToString());
//                return enteredDate;
//            }

//            ahk.MsgBox("Unable To Format This VarType Yet: " + VarType);
//            return DateTime.Now;
//        }

//        public bool _ToBool(object TrueFalseVar)  // converts string/int to bool
//        {
//            string VarType = TrueFalseVar.GetType().ToString();  //determine what kind of variable was passed into function

//            if (VarType == "System.Int32")
//            {
//                if (TrueFalseVar.ToString() == "1") { return true; }
//                if (TrueFalseVar.ToString() == "0") { return false; }
//            }

//            if (VarType == "System.String")
//            {
//                if (TrueFalseVar.ToString().ToUpper() == "TRUE") { return true; }
//                else { return false; }
//            }

//            ahk.MsgBox(VarType + " Not Setup For ToBool() Conversion");
//            return false;
//        }


//        //public static byte[] ToByteArray(this System.IO.Stream stream)
//        //{
//        //    // ex: 
//        //    //FileStream f = File.OpenRead(@"c:\testfile.txt");
//        //    //byte[] b = f.ConvertToByteArray();
//        //    //Console.WriteLine(b.Length);            


//        //    var streamLength = Convert.ToInt32(stream.Length);
//        //    byte[] data = new byte[streamLength + 1];

//        //    //convert to to a byte array
//        //    stream.Read(data, 0, streamLength);
//        //    stream.Close();

//        //    return data;
//        //}



//        // FILE Size Conversion

//        public string _FormatBytes(Int64 bytes)  // convert bytes to Text representation (adds kb/mb/tb to return)
//        {
//            /// <summary>
//            /// Returns a human-readable size discriptor for up 64-bit length fields
//            /// </summary>
//            /// <param name="bytes"></param>
//            /// <returns></returns>
//            /// 
//            if (bytes >= 0x1000000000000000) { return ((double)(bytes >> 50) / 1024).ToString("0.## EB"); }
//            if (bytes >= 0x4000000000000) { return ((double)(bytes >> 40) / 1024).ToString("0.## PB"); }
//            if (bytes >= 0x10000000000) { return ((double)(bytes >> 30) / 1024).ToString("0.## TB"); }
//            if (bytes >= 0x40000000) { return ((double)(bytes >> 20) / 1024).ToString("0.## GB"); }
//            if (bytes >= 0x100000) { return ((double)(bytes >> 10) / 1024).ToString("0.## MB"); }
//            if (bytes >= 0x400) { return ((double)(bytes) / 1024).ToString("0.##") + " KB"; }
//            return bytes.ToString("0 Bytes");
//        }

//        public double _Bytes_To_MB(long bytes)  // convert Bytes to Megabytes
//        {
//            return (bytes / 1024f) / 1024f;
//        }

//        public double _KB_To_MB(long kilobytes)  //Convert Kilobytes To Megabytes
//        {
//            return kilobytes / 1024f;
//        }

//        public double _MB_To_GB(double megabytes) // Convert Megabytes To Gigabytes
//        {
//            // 1024 megabyte in a gigabyte
//            return megabytes / 1024.0;
//        }

//        public double _MB_To_TB(double megabytes) // Convert Megabytes To Terabytes
//        {
//            // 1024 * 1024 megabytes in a terabyte
//            return megabytes / (1024.0 * 1024.0);
//        }

//        public double _GB_To_MB(double gigabytes) // Convert Gigabytes To Megabytes
//        {
//            // 1024 gigabytes in a terabyte
//            return gigabytes * 1024.0;
//        }

//        public double _GB_To_TB(double gigabytes) // Convert Gigabytes To Terabytes
//        {
//            // 1024 gigabytes in a terabyte
//            return gigabytes / 1024.0;
//        }

//        public double _TB_To_MB(double terabytes) // Convert Terabytes To Megabytes
//        {
//            // 1024 * 1024 megabytes in a terabyte
//            return terabytes * (1024.0 * 1024.0);
//        }

//        public double _TB_To_GB(double terabytes) // Convert Terabytes To Gigabytes
//        {
//            // 1024 gigabytes in a terabyte
//            return terabytes * 1024.0;
//        }






//        public int _MinFromMs(int value)  // converts ms to minutes
//        {
//            try
//            {
//                TimeSpan t = TimeSpan.FromMilliseconds(value);
//                return (int)t.TotalMinutes;
//            }
//            catch (Exception ex)
//            {
//                //WriteNote("Unexpected error - " + ex.Message.ToString());
//                return 0;
//            }
//        }
//        public int _SecsFromMs(int value)  // converts ms to seconds
//        {
//            try
//            {
//                TimeSpan t = TimeSpan.FromMilliseconds(value);
//                return (int)t.TotalSeconds;
//            }
//            catch (Exception ex)
//            {
//                //WriteNote("Unexpected error - " + ex.Message.ToString());
//                return 0;
//            }
//        }
//        public int _HoursFromMs(int value)  // converts ms to hours
//        {
//            try
//            {
//                TimeSpan t = TimeSpan.FromMilliseconds(value);
//                return (int)t.TotalHours;
//            }
//            catch (Exception ex)
//            {
//                //WriteNote("Unexpected error - " + ex.Message.ToString());
//                return 0;
//            }
//        }
//        public int _DaysFromMs(int value)  // converts ms to days
//        {
//            try
//            {
//                TimeSpan t = TimeSpan.FromMilliseconds(value);
//                return (int)t.TotalDays;
//            }
//            catch (Exception ex)
//            {
//                //WriteNote("Unexpected error - " + ex.Message.ToString());
//                return 0;
//            }
//        }


//        #endregion

//        #region === v1 ClipBoard ===

//        public string _ClipWait(int SecondsToWait = 5, bool AnyData = false)  // Waits for Clipboard To Contain value, returns Clipboard contents
//        {
//            if (sharpAHK._AHK.ahkGlobal.ahkdll == null) { sharpAHK._AHK.ahkGlobal.ahkdll = new AutoHotkey.Interop.AutoHotkeyEngine(); }
//            var ahkdll = sharpAHK._AHK.ahkGlobal.ahkdll;

//            ahkdll.SetVar("clipboard", "");  // clear out clipboard

//            // If this parameter is omitted, the command is more selective, waiting specifically for text or files to appear 
//            // ("text" includes anything that would produce text when you paste into Notepad). If this parameter is 1 (can be an expression), 
//            // the command waits for data of any kind to appear on the clipboard.

//            int AnyDataOnClipboard = 0;
//            if (AnyData) { AnyDataOnClipboard = 1; }

//            string Command = @"ClipWait, " + SecondsToWait + "," + AnyDataOnClipboard;

//            ahkdll.ExecRaw(Command);


//            //   Error Handling
//            ahkGlobal.ErrorLevelMsg = "The Attempt To Copy Text Onto The ClipBoard Failed.";
//            ahkGlobal.ErrorLevel = false;
//            string ErrorLevelValue = ahkdll.GetVar("ErrorLevel");
//            if (ErrorLevelValue == "0") { ahkGlobal.ErrorLevelMsg = ""; }
//            if (ErrorLevelValue == "1") { ahkGlobal.ErrorLevel = true; }
//            if (ahkGlobal.Debug) { if (ahkGlobal.ErrorLevel) { ahk.MsgBox(ahkGlobal.ErrorLevelMsg, "ErrorLevel"); } } //Display Error Messages in Debug Mode


//            // returns clipboard value
//            string clipboard = _Clipboard();

//            return clipboard;
//        }

//        public string _Clipboard()  // Returns contents of clipboard
//        {
//            if (sharpAHK._AHK.ahkGlobal.ahkdll == null) { sharpAHK._AHK.ahkGlobal.ahkdll = new AutoHotkey.Interop.AutoHotkeyEngine(); }
//            var ahkdll = sharpAHK._AHK.ahkGlobal.ahkdll;

//            // return clipboard contents
//            ahkdll.ExecRaw("clipval := clipboard");
//            string clipboard = ahkdll.GetVar("clipval");

//            return clipboard;
//        }

//        public void _ClipWrite(string ClipboardText = "")  // Write Text to Clipboard (Blank to Clear)
//        {
//            if (sharpAHK._AHK.ahkGlobal.ahkdll == null) { sharpAHK._AHK.ahkGlobal.ahkdll = new AutoHotkey.Interop.AutoHotkeyEngine(); }
//            var ahkdll = sharpAHK._AHK.ahkGlobal.ahkdll;

//            ahkdll.SetVar("clipboard", ClipboardText);
//        }


//        #endregion

//        #region === v1 Global HotKeys ===

//        //public GoDaddy()
//        //{
//        //    InitializeComponent();
//        //    Closing += Form1Closing;  // dispose hotkeys on exit
//        //}

//        //void Form1Closing(object sender, CancelEventArgs e) // dispose hotkeys on exit
//        //{
//        //    UnRegisterHotkeys(this.Handle);
//        //}

//        //public void RegisterHotkeys()
//        //{
//        //    //Hotkey("control num1");
//        //    //Hotkey("F1");
//        //    //Hotkey("Control Shift C"); 
//        //}

//        public void _Hotkey(string Hotkey, IntPtr hWnd) // register global hotkey
//        {
//            //Hotkey("control num1");
//            //Hotkey("F1");
//            //Hotkey("Control Shift C"); 


//            var key = Keys.F1; // set temp value
//            var mod = KeyModifier.None;

//            Hotkey = Hotkey.ToUpper();

//            //=== set the modifier value ==========================
//            if (Hotkey.Contains("ALT")) { mod = KeyModifier.Alt; }
//            if (Hotkey.Contains("SHIFT")) { mod = KeyModifier.Shift; }
//            if (Hotkey.Contains("CONTROL")) { mod = KeyModifier.Control; }
//            if (Hotkey.Contains("WIN")) { mod = KeyModifier.WinKey; }

//            //=== account for combinations of modifiers ====================
//            if (Hotkey.Contains(" "))
//            {
//                if (Hotkey.Contains("ALT") && Hotkey.Contains("SHIFT")) { mod = KeyModifier.Alt | KeyModifier.Shift; }
//                if (Hotkey.Contains("ALT") && Hotkey.Contains("CONTROL")) { mod = KeyModifier.Alt | KeyModifier.Control; }
//                if (Hotkey.Contains("ALT") && Hotkey.Contains("WIN")) { mod = KeyModifier.Alt | KeyModifier.WinKey; }

//                if (Hotkey.Contains("SHIFT") && Hotkey.Contains("CONTROL")) { mod = KeyModifier.Control | KeyModifier.Shift; }
//                if (Hotkey.Contains("SHIFT") && Hotkey.Contains("WIN")) { mod = KeyModifier.WinKey | KeyModifier.Shift; }

//                if (Hotkey.Contains("WIN") && Hotkey.Contains("CONTROL")) { mod = KeyModifier.WinKey | KeyModifier.Control; }
//            }


//            //=== extract just hotkey value ==========================
//            Hotkey = Hotkey.Replace("ALT", "");
//            Hotkey = Hotkey.Replace("SHIFT", "");
//            Hotkey = Hotkey.Replace("CONTROL", "");
//            Hotkey = Hotkey.Replace("WIN", "");

//            Hotkey = Hotkey.Trim();

//            //=== set the key value ===============================

//            bool KeySet = false;
//            if (Hotkey == "PRINTSCREEN") { key = Keys.PrintScreen; KeySet = true; }
//            if (Hotkey == "PS") { key = Keys.PrintScreen; KeySet = true; }


//            int HotKeyID = 0;     // The id of the hotkey. 

//            if (!KeySet)
//            {

//                if (Hotkey.Contains("NUMPAD"))
//                {
//                    Hotkey = Hotkey.ToUpper().Replace("NUMPAD", "NumPad");
//                }

//                if (Hotkey.Contains("NUM"))
//                {
//                    Hotkey = Hotkey.ToUpper().Replace("NUM", "NumPad");
//                }

//                key = (Keys)Enum.Parse(typeof(Keys), Hotkey);

//            }  //assign the hotkey value if not already set


//            //IntPtr handle = Process.GetCurrentProcess().MainWindowHandle;
//            //IntPtr hwnd = Handle; 

//            bool Registered = _RegisterHotKey(hWnd, HotKeyID, (int)mod, key.GetHashCode());
//            if (!Registered) { ahk.MsgBox("Failed to Register " + Hotkey); }
//        }

//        public void _UnRegisterHotkeys(IntPtr hWnd)  // unregister global hotkey
//        {
//            //Ex: 
//            //UnRegisterHotkeys(this.Handle);

//            //=== Unregister all hotkeys used in application
//            int IDRemove = 0;
//            do
//            {
//                _UnregisterHotKey(hWnd, IDRemove);
//                IDRemove++;
//            } while (IDRemove < 100);
//        }

//        //protected override void WndProc(ref Message m)  // Global Hotkey Actions (Place in WinForm Application)
//        //{
//        //    base.WndProc(ref m);

//        //    if (m.Msg == 0x0312)  // if registered hotkey is pressed
//        //    {
//        //        //===== Extract Info From Registered Hotkey That Was Pressed =====================================================
//        //        Keys key = (Keys)(((int)m.LParam >> 16) & 0xFFFF);                  // The key of the hotkey that was pressed.
//        //        AHK.AHK.KeyModifier modifier = (AHK.AHK.KeyModifier)((int)m.LParam & 0xFFFF);       // The modifier of the hotkey that was pressed.
//        //        int id = m.WParam.ToInt32();                                        // The id of the hotkey that was pressed.

//        //        ahk.ahk.MsgBox("Hotkey: " + key.ToString() + Environment.NewLine + "Modifier: " + modifier.ToString());
//        //    }
//        //}


//        //======Setup Hotkeys=============================================================
//        [System.Runtime.InteropServices.DllImport("user32.dll")]
//        private static extern bool _RegisterHotKey(IntPtr hWnd, int id, int fsModifiers, int vk);  // used to register hotkeys in windows
//        [System.Runtime.InteropServices.DllImport("user32.dll")]
//        private static extern bool _UnregisterHotKey(IntPtr hWnd, int id);  // used to unregister hotkeys in windows
//        public enum KeyModifier
//        {
//            None = 0,
//            Alt = 1,
//            Control = 2,
//            Shift = 4,
//            WinKey = 8
//        }



//        #endregion

//        #region === v1 Global Hotkeys (AHK) ===

//        //protected override void WndProc(ref Message m)  // Global Hotkey Actions (Place in WinForm Application)
//        //{

//        //    // Receive Message with Parameters from SendMessage Command in This Application or Another 
//        //    if (m.Msg == _HKMSG)
//        //    {
//        //        string Msg1 = Marshal.PtrToStringUni(m.WParam);
//        //        string Msg2 = Marshal.PtrToStringUni(m.LParam);

//        //        //ahk.ahk.MsgBox(Msg1 + " - " + Msg2);
//        //        HotKey_Actions(Msg1, Msg2);
//        //    }

//        //    base.WndProc(ref m);
//        //}


//        //public void HotKey_Actions(string Msg1, string Msg2)
//        //{
//        //    ahk.ahk.MsgBox(Msg1 + " - " + Msg2);

//        //}  // Actions to Perform Based on Incoming SendMessages


//        //public void SetupHotkeys()
//        //{
//        ////  Create Set of Hotkeys for Application (in batch)
//        //    string HKLine;
//        //    HKLine = ahk.AddHotKeyBatch("F10", "Do This Mkay?", this.Handle);
//        //    HKLine = ahk.AddHotKeyBatch("F9", "Another Act", this.Handle, HKLine);
//        //    HKLine = ahk.AddHotKeyBatch("!LButton", "Mouse Hotkey", this.Handle, HKLine);

//        //    ahk.SaveHotkeys(HKLine);  // Execute Hotkey Create Line
//        //}


//        #region Assign Hotkey PostMessage Values
//        public const int _HKMSG = 0xA124;
//        public const int _APPMSG = 0xA123;

//        public const int _HKMSG_F1 = 0xF001;
//        public const int _HKMSG_F2 = 0xF002;
//        public const int _HKMSG_F3 = 0xF003;
//        public const int _HKMSG_F4 = 0xF004;
//        public const int _HKMSG_F5 = 0xF005;
//        public const int _HKMSG_F6 = 0xF006;
//        public const int _HKMSG_F7 = 0xF007;
//        public const int _HKMSG_F8 = 0xF008;
//        public const int _HKMSG_F9 = 0xF009;
//        public const int _HKMSG_F10 = 0xF010;
//        public const int _HKMSG_F11 = 0xF011;
//        public const int _HKMSG_F12 = 0xF012;

//        public const int _HKMSG_CtlNumPad0 = 0xF013;
//        public const int _HKMSG_NumPad0 = 0xF014;
//        public const int _HKMSG_CtlNumPad1 = 0xF015;
//        public const int _HKMSG_NumPad1 = 0xF016;
//        public const int _HKMSG_CtlNumPad2 = 0xF017;
//        public const int _HKMSG_NumPad2 = 0xF018;
//        public const int _HKMSG_CtlNumPad3 = 0xF019;
//        public const int _HKMSG_NumPad3 = 0xF020;
//        public const int _HKMSG_CtlNumPad4 = 0xF021;
//        public const int _HKMSG_NumPad4 = 0xF022;
//        public const int _HKMSG_CtlNumPad5 = 0xF023;
//        public const int _HKMSG_NumPad5 = 0xF024;
//        public const int _HKMSG_CtlNumPad6 = 0xF025;
//        public const int _HKMSG_NumPad6 = 0xF026;
//        public const int _HKMSG_CtlNumPad7 = 0xF027;
//        public const int _HKMSG_NumPad7 = 0xF028;
//        public const int _HKMSG_CtlNumPad8 = 0xF029;
//        public const int _HKMSG_NumPad8 = 0xF029;
//        public const int _HKMSG_CtlNumPad9 = 0xF030;
//        public const int _HKMSG_NumPad9 = 0xF031;


//        public const int _HKMSG_PrintScreen = 0xF032;
//        public const int _HKMSG_CtlPrintScreen = 0xF033;
//        public const int _HKMSG_CtlAltPrintScreen = 0xF034;
//        public const int _HKMSG_Esc = 0xF035;

//        public const int _HKMSG_Win = 0xF036;
//        public const int _HKMSG_CtlWin = 0xF037;


//        public const int _HKMSG_CtlRButton = 0xA128; // ^RButton Hotkey
//        public const int _HKMSG_CtlLButton = 0xA228; // Control LButton
//        public const int _HKMSG_AltLButton = 0xA229; // Alt LButton

//        #endregion

//        private int _HKPostMsgValue(string Hotkey)
//        {
//            Hotkey = Hotkey.ToUpper();

//            if (Hotkey == "^RBUTTON") { return _HKMSG_CtlRButton; }
//            if (Hotkey == "^LBUTTON") { return _HKMSG_CtlLButton; }
//            if (Hotkey == "!LBUTTON") { return _HKMSG_AltLButton; }
//            if (Hotkey == "#") { return _HKMSG_Win; }
//            if (Hotkey == "LWIN") { return _HKMSG_Win; }
//            if (Hotkey == "^LWIN") { return _HKMSG_CtlWin; }

//            if (Hotkey == "^NUMPAD0") { return _HKMSG_CtlNumPad0; }
//            if (Hotkey == "NUMPAD0") { return _HKMSG_NumPad0; }
//            if (Hotkey == "^NUMPAD1") { return _HKMSG_CtlNumPad1; }
//            if (Hotkey == "NUMPAD1") { return _HKMSG_NumPad1; }
//            if (Hotkey == "^NUMPAD2") { return _HKMSG_CtlNumPad2; }
//            if (Hotkey == "NUMPAD2") { return _HKMSG_NumPad2; }
//            if (Hotkey == "^NUMPAD3") { return _HKMSG_CtlNumPad3; }
//            if (Hotkey == "NUMPAD3") { return _HKMSG_NumPad3; }
//            if (Hotkey == "^NUMPAD4") { return _HKMSG_CtlNumPad4; }
//            if (Hotkey == "NUMPAD4") { return _HKMSG_NumPad4; }
//            if (Hotkey == "^NUMPAD5") { return _HKMSG_CtlNumPad5; }
//            if (Hotkey == "NUMPAD5") { return _HKMSG_NumPad5; }
//            if (Hotkey == "^NUMPAD6") { return _HKMSG_CtlNumPad6; }
//            if (Hotkey == "NUMPAD6") { return _HKMSG_NumPad6; }
//            if (Hotkey == "^NUMPAD7") { return _HKMSG_CtlNumPad7; }
//            if (Hotkey == "NUMPAD7") { return _HKMSG_NumPad7; }
//            if (Hotkey == "^NUMPAD8") { return _HKMSG_CtlNumPad8; }
//            if (Hotkey == "NUMPAD8") { return _HKMSG_NumPad8; }
//            if (Hotkey == "^NUMPAD9") { return _HKMSG_CtlNumPad9; }
//            if (Hotkey == "NUMPAD9") { return _HKMSG_NumPad9; }

//            if (Hotkey == "F1") { return _HKMSG_F1; }
//            if (Hotkey == "F2") { return _HKMSG_F2; }
//            if (Hotkey == "F3") { return _HKMSG_F3; }
//            if (Hotkey == "F4") { return _HKMSG_F4; }
//            if (Hotkey == "F5") { return _HKMSG_F5; }
//            if (Hotkey == "F6") { return _HKMSG_F6; }
//            if (Hotkey == "F7") { return _HKMSG_F7; }
//            if (Hotkey == "F8") { return _HKMSG_F8; }
//            if (Hotkey == "F9") { return _HKMSG_F9; }
//            if (Hotkey == "F10") { return _HKMSG_F10; }
//            if (Hotkey == "F11") { return _HKMSG_F11; }
//            if (Hotkey == "F12") { return _HKMSG_F12; }

//            if (Hotkey == "PRINTSCREEN") { return _HKMSG_PrintScreen; }
//            if (Hotkey == "^PRINTSCREEN") { return _HKMSG_CtlPrintScreen; }
//            if (Hotkey == "^!PRINTSCREEN") { return _HKMSG_CtlAltPrintScreen; }
//            if (Hotkey == "ESC") { return _HKMSG_Esc; }


//            ahk.MsgBox("HK PostMsgValue Not Assigned Yet");
//            return 0;
//        }


//        public void _AddHotKey(string HotKey, string Action, IntPtr AppHandle)  // Create Global Hotkey using AHK.DLL and SendMessage to WndProc
//        {
//            int HKMSG = _HKPostMsgValue(HotKey);  // return the postmessage value to send to application

//            if (HKMSG == 0) { return; } // no value assigned, do nothing


//            //create an autohotkey engine (AHK DLL) or use existing instance if it hasn't be initiated
//            if (sharpAHK._AHK.ahkGlobal.ahkdll == null) { sharpAHK._AHK.ahkGlobal.ahkdll = new AutoHotkey.Interop.AutoHotkeyEngine(); }
//            var ahkdll = sharpAHK._AHK.ahkGlobal.ahkdll;

//            //string S = "Hello From Hotkey";
//            GCHandle GCH = GCHandle.Alloc(Action, GCHandleType.Pinned);
//            IntPtr pS = GCH.AddrOfPinnedObject();

//            //string HK = "F12";
//            GCHandle GCH2 = GCHandle.Alloc(HotKey, GCHandleType.Pinned);
//            IntPtr pS2 = GCH2.AddrOfPinnedObject();

//            string SendMessage = "SendMessage, " + HKMSG + ", " + pS2 + ", " + pS + ",, ahk_id " + AppHandle;

//            string AHKLine = HotKey + "::" + SendMessage;

//            ahkdll.ExecRaw(AHKLine); // assign hotkey value + message to application

//            GCH.Free();
//            GCH2.Free();
//            return;
//        }

//        public string _AddHotKeyBatch(string HotKey, string Action, IntPtr AppHandle, string HotKeyString = "")  // Create Global Hotkey using AHK.DLL and SendMessage to WndProc
//        {
//            ////create an autohotkey engine (AHK DLL) or use existing instance if it hasn't be initiated
//            //if (sharpAHK._AHK.ahkGlobal.ahkdll == null) { sharpAHK._AHK.ahkGlobal.ahkdll = new AutoHotkey.Interop.AutoHotkeyEngine(); }
//            //var ahkdll = sharpAHK._AHK.ahkGlobal.ahkdll;


//            int HKMSG = _HKPostMsgValue(HotKey);  // return the postmessage value to send to application

//            //string S = "Hello From Hotkey";
//            GCHandle GCH = GCHandle.Alloc(Action, GCHandleType.Pinned);
//            IntPtr pS = GCH.AddrOfPinnedObject();

//            //string HK = "F12";
//            GCHandle GCH2 = GCHandle.Alloc(HotKey, GCHandleType.Pinned);
//            IntPtr pS2 = GCH2.AddrOfPinnedObject();

//            string SendMessage = "SendMessage, " + HKMSG + ", " + pS2 + ", " + pS + ",, ahk_id " + AppHandle;

//            string AHKLine = HotKey + "::" + SendMessage;

//            //ahkdll.ExecRaw(AHKLine); // assign hotkey value + message to application

//            GCH.Free();
//            GCH2.Free();

//            HotKeyString = HotKeyString + " \r\n " + AHKLine;

//            return HotKeyString;
//        }

//        public void _SaveHotkeys(string AHKLine)  // executes ahk line in ahk dll file (used to save hotkeys - can be used for any ahk command)
//        {
//            //create an autohotkey engine (AHK DLL) or use existing instance if it hasn't be initiated
//            if (sharpAHK._AHK.ahkGlobal.ahkdll == null) { sharpAHK._AHK.ahkGlobal.ahkdll = new AutoHotkey.Interop.AutoHotkeyEngine(); }
//            var ahkdll = sharpAHK._AHK.ahkGlobal.ahkdll;

//            ahkdll.ExecRaw(AHKLine); // assign hotkey value + message to application
//        }



//        #region === HotKey TESTING / PostMessage Version ===


//        // Win Form Hotkeys (Application Window Activate)
//        //protected override bool ProcessCmdKey(ref Message msg, Keys keyData)  // set hotkeys for winform
//        //{
//        //    //ahk.ahk.MsgBox(keyData.ToString()); // displays the name of the key pressed


//        //    if (keyData == (Keys.Decimal))  //(keyData == (Keys.Control | Keys.F))
//        //    {
//        //        MessageBox.Show("-1");
//        //        return true;
//        //    }
//        //    if (keyData == (Keys.NumPad0))  //(keyData == (Keys.Control | Keys.F))
//        //    {
//        //        ahk.WinMinimize(this.Handle);
//        //        return true;
//        //    }
//        //    if (keyData == (Keys.NumPad1))  //(keyData == (Keys.Control | Keys.F))
//        //    {
//        //        Button1();
//        //        return true;
//        //    }
//        //    if (keyData == (Keys.NumPad2))  //(keyData == (Keys.Control | Keys.F))
//        //    {
//        //        Button2();
//        //        return true;
//        //    }
//        //    if (keyData == (Keys.NumPad3))  //(keyData == (Keys.Control | Keys.F))
//        //    {
//        //        Button3();
//        //        return true;
//        //    }
//        //    if (keyData == (Keys.NumPad4))  //(keyData == (Keys.Control | Keys.F))
//        //    {
//        //        Button4();
//        //        return true;
//        //    }
//        //    if (keyData == (Keys.NumPad5))  //(keyData == (Keys.Control | Keys.F))
//        //    {
//        //        Button5();
//        //        return true;
//        //    }
//        //    if (keyData == (Keys.NumPad6))  //(keyData == (Keys.Control | Keys.F))
//        //    {
//        //        Button6();
//        //        return true;
//        //    }
//        //    if (keyData == (Keys.NumPad7))  //(keyData == (Keys.Control | Keys.F))
//        //    {
//        //        Button7();
//        //        return true;
//        //    }
//        //    if (keyData == (Keys.NumPad8))  //(keyData == (Keys.Control | Keys.F))
//        //    {
//        //        Button8();
//        //        return true;
//        //    }
//        //    if (keyData == (Keys.NumPad9))  //(keyData == (Keys.Control | Keys.F))
//        //    {
//        //        Button9();
//        //        return true;
//        //    }


//        //    //return base.ProcessCmdKey(ref msg, keyData);
//        //}

//        ////protected override void WndProc(ref Message m)  // Global Hotkey Actions (Place in WinForm Application)
//        ////{
//        ////    //// Receive Message with Parameters from SendMessage Command in This Application or Another 
//        ////    //if (m.Msg == AHK.AHK._APPMSG)
//        ////    //{
//        ////    //    string Msg1 = Marshal.PtrToStringUni(m.WParam);
//        ////    //    string Msg2 = Marshal.PtrToStringUni(m.LParam);


//        ////    //    ahk.ahk.MsgBox(Msg1 + " - " + Msg2);
//        ////    //    //AppMsgAction(Msg1, Msg2);
//        ////    //}


//        ////    //// Receive Message with Parameters from AHK SendMessage Command in This Application (global hotkeys)
//        ////    //if (m.Msg == AHK.AHK._HKMSG)
//        ////    //{
//        ////    //    //RClickAction();
//        ////    //    //return; 

//        ////    //    string Msg1 = Marshal.PtrToStringUni(m.WParam);
//        ////    //    string Msg2 = Marshal.PtrToStringUni(m.LParam);


//        ////    //    //ahk.ahk.MsgBox(Msg1 + " - " + Msg2);

//        ////    //    if (Msg1 == "^RButton")
//        ////    //    {
//        ////    //        ahk.ahk.MsgBox("R Click Action");
//        ////    //        //RClickAction();
//        ////    //        return;
//        ////    //    }

//        ////    //    //HotKey_Actions(Msg1, Msg2);
//        ////    //}


//        ////    //if (m.Msg == AHK.AHK._HKMSG_CtlRButton)  // Control RightClick Menu
//        ////    //{
//        ////    //    //ahk.ahk.MsgBox("R Click Action HERE");
//        ////    //    RClickAction();
//        ////    //    return;
//        ////    //}
//        ////    //if (m.Msg == AHK.AHK._HKMSG_F12)
//        ////    //{
//        ////    //    ahk.ahk.MsgBox("F12 Action HERE");
//        ////    //    return;
//        ////    //}
//        ////    //if (m.Msg == AHK.AHK._HKMSG_Esc)
//        ////    //{
//        ////    //    ahk.ahk.MsgBox("Escape");
//        ////    //    return;
//        ////    //}

//        ////    //if (m.Msg == AHK.AHK._HKMSG_CtlNumPad0)  // Control Numpad 0 = Activate/Show Gui
//        ////    //{
//        ////    //    //ahk.ahk.MsgBox("Control Numpad 0");

//        ////    //    ahk.WinRestore(this.Handle);
//        ////    //    ahk.WinActivate(this.Handle);
//        ////    //    return;
//        ////    //}

//        ////    //if (m.Msg == AHK.AHK._HKMSG_Win)  // WinKey = Minimize All Windows
//        ////    //{
//        ////    //    //ahk.ahk.MsgBox("Win Key");
//        ////    //    ahk.WinMinimizeAll();
//        ////    //    return;
//        ////    //}
//        ////    //if (m.Msg == AHK.AHK._HKMSG_CtlWin)  // Control WinKey = UnMinimize All
//        ////    //{
//        ////    //    //ahk.ahk.MsgBox("Control + Win Key");
//        ////    //    ahk.WinMinimizeAllUndo();
//        ////    //    return;
//        ////    //}


//        ////    //if (m.Msg == AHK.AHK._HKMSG_PrintScreen)
//        ////    //{
//        ////    //    ahk.ahk.MsgBox("PrintScreen");
//        ////    //    return;
//        ////    //}
//        ////    //if (m.Msg == AHK.AHK._HKMSG_CtlPrintScreen)
//        ////    //{
//        ////    //    ahk.ahk.MsgBox("Control PrintScreen");
//        ////    //    return;
//        ////    //}
//        ////    //if (m.Msg == AHK.AHK._HKMSG_CtlAltPrintScreen)
//        ////    //{
//        ////    //    ahk.ahk.MsgBox("Control Alt PrintScreen");
//        ////    //    return;
//        ////    //}

//        ////    //if (m.Msg == AHK.AHK._HKMSG_AltLButton) // alt + lbutton down
//        ////    //{
//        ////    //    Button2();
//        ////    //    return;
//        ////    //}


//        ////    //base.WndProc(ref m);

//        ////    if (m.Msg == 0x0312)  // if registered Global Hotkey is Pressed
//        ////    {
//        ////        //===== Extract Info From Registered Hotkey That Was Pressed =====================================================
//        ////        Keys key = (Keys)(((int)m.LParam >> 16) & 0xFFFF);                  // The key of the hotkey that was pressed.
//        ////        KeyModifier modifier = (KeyModifier)((int)m.LParam & 0xFFFF);       // The modifier of the hotkey that was pressed.
//        ////        int id = m.WParam.ToInt32();                                        // The id of the hotkey that was pressed.

//        ////        ahk.MsgBox("Hotkey: " + key.ToString() + Environment.NewLine + "Modifier: " + modifier.ToString());
//        ////    }
//        ////}

//        public void _Setup_Hotkeys()
//        {
//            //var stopwatch = new Stopwatch(); stopwatch.Start(); // setup timer and start stopwatch

//            //=============================================================================================
//            // If Just Creating 1 Hotkey, Use AddHotKey. 
//            //=============================================================================================

//            //ahk.AddHotKey("^RButton", "RClickAction", this.Handle);
//            //ahk.AddHotKey("F12", "Action", this.Handle);
//            //ahk.AddHotKey("^Numpad0", "Action", this.Handle);
//            //ahk.AddHotKey("Esc", "Action", this.Handle);
//            //ahk.AddHotKey("PrintScreen", "Action", this.Handle); //  PrintScreen
//            //ahk.AddHotKey("^PrintScreen", "Action", this.Handle); //  Control PrintScreen
//            //ahk.AddHotKey("^!PrintScreen", "Action", this.Handle); // Control Alt PrintScreen

//            ////=============================================================================================
//            //// Otherwise use AddHotKeyBatch to start multiple hotkeys at once. (much faster startup)
//            ////=============================================================================================
//            //IntPtr Handle = null; 
//            //string HKCreateLine = AddHotKeyBatch("^RButton", "RClickAction", Handle);
//            //HKCreateLine = AddHotKeyBatch("!LButton", "Action", this.Handle, HKCreateLine);
//            //HKCreateLine = AddHotKeyBatch("F12", "Action", this.Handle, HKCreateLine);
//            //HKCreateLine = AddHotKeyBatch("^Numpad0", "Action", this.Handle, HKCreateLine);
//            //HKCreateLine = AddHotKeyBatch("LWin", "Action", this.Handle, HKCreateLine);
//            //HKCreateLine = AddHotKeyBatch("^LWin", "Action", this.Handle, HKCreateLine);
//            //SaveHotkeys(HKCreateLine);


//            //string HKSetupTime = stopwatch.ElapsedMilliseconds.ToString();  // returns the # of ms the stopwatch was running
//            //ahk.ahk.MsgBox(HKSetupTime); 
//        }

//        public void _RClickAction()
//        {

//            //"Add To List Item", "Open File", "Open Dir", "Rename File", "Delete File"
//            ContextMenuStrip mnu = new ContextMenuStrip();

//            ToolStripMenuItem Opt1 = new ToolStripMenuItem("Cancel");
//            ToolStripMenuItem Opt2 = new ToolStripMenuItem("WinInfo");
//            ToolStripMenuItem Opt3 = new ToolStripMenuItem("Open Dir");
//            ToolStripMenuItem Opt4 = new ToolStripMenuItem("Rename File");
//            ToolStripMenuItem Opt5 = new ToolStripMenuItem("Delete File");

//            //Assign event handlers
//            Opt1.Click += new EventHandler(m_Click);
//            Opt2.Click += new EventHandler(m_Click);
//            Opt3.Click += new EventHandler(m_Click);
//            Opt4.Click += new EventHandler(m_Click);
//            Opt5.Click += new EventHandler(m_Click);

//            //Add to main context menu
//            mnu.Items.AddRange(new ToolStripItem[] { Opt1, Opt2, Opt3, Opt4, Opt5 });

//            //Assign to datagridview
//            //dataGridView2.ContextMenuStrip = mnu;

//            mnu.Show(Cursor.Position.X, Cursor.Position.Y);

//        }

//        private void m_Click(object sender, System.EventArgs e)
//        {
//            string Options = sender.ToString();

//            if (Options == "Cancel") { return; }

//            if (Options == "WinInfo")
//            {
//                IntPtr ActiveHwnd = _WinGetActiveID();
//                string ActiveWin = _WinGetTitle(ActiveHwnd);
//                ahk.MsgBox(ActiveWin + " (" + ActiveHwnd.ToString() + ")");
//                return;
//            }

//            ahk.MsgBox(Options);

//        }


//        #endregion



//        #endregion

//        #region === v1 ShortCuts ===

//        public void _urlShortcutToDesktop(string linkName, string linkUrl)  // creates url shortcut to user's desktop
//        {
//            string deskDir = Environment.GetFolderPath(Environment.SpecialFolder.DesktopDirectory);

//            using (StreamWriter writer = new StreamWriter(deskDir + "\\" + linkName + ".url"))
//            {
//                writer.WriteLine("[InternetShortcut]");
//                writer.WriteLine("URL=" + linkUrl);
//                writer.Flush();
//            }
//        }

//        public void _appShortcutToDesktop(string linkName)  // creates a link from the current application to the user's desktop as an app link
//        {
//            string deskDir = Environment.GetFolderPath(Environment.SpecialFolder.DesktopDirectory);

//            string LinkFile = deskDir + "\\" + linkName + ".url";
//            ahk.FileDelete(LinkFile);

//            string app = System.IO.Path.GetDirectoryName(Environment.GetCommandLineArgs()[0]) + "\\" + Assembly.GetCallingAssembly().GetName().Name.ToString() + ".exe";

//            using (StreamWriter writer = new StreamWriter(deskDir + "\\" + linkName + ".url"))
//            {
//                writer.WriteLine("[InternetShortcut]");
//                writer.WriteLine("URL=file:///" + app);
//                writer.WriteLine("IconIndex=0");
//                string icon = app.Replace('\\', '/');
//                writer.WriteLine("IconFile=" + icon);
//                writer.Flush();
//            }

//            ahk.MsgBox(app);
//        }

//        public void _addShortcutToStartupGroup()  // adds shortcut to application for current executable in user's startup directory 
//        {
//            string AppName = Assembly.GetCallingAssembly().GetName().Name.ToString();
//            string ShortCutPath = Environment.GetFolderPath(Environment.SpecialFolder.DesktopDirectory) + "\\" + AppName + ".lnk";
//            string AppPath = System.IO.Path.GetDirectoryName(Environment.GetCommandLineArgs()[0]) + "\\" + Assembly.GetCallingAssembly().GetName().Name.ToString() + ".exe";
//            string IconPath = "shell32.dll";
//            string IconNum = "1";

//            Type t = Type.GetTypeFromCLSID(new Guid("72C24DD5-D70A-438B-8A42-98424B88AFB8")); //Windows Script Host Shell Object
//            dynamic shell = Activator.CreateInstance(t);
//            try
//            {
//                var lnk = shell.CreateShortcut(ShortCutPath);
//                try
//                {
//                    lnk.TargetPath = AppPath;
//                    lnk.IconLocation = IconPath + "," + IconNum;
//                    lnk.Save();
//                }
//                finally
//                {
//                    Marshal.FinalReleaseComObject(lnk);
//                }
//            }
//            finally
//            {
//                Marshal.FinalReleaseComObject(shell);
//            }
//        }

//        #endregion

//        #region === v1 Send Application Messages ===


//        //protected override void WndProc(ref Message m)  // Global Message/Hotkey Actions (Place in WinForm Application)
//        //{
//        //    // Receive Message with Parameters from SendMessage Command in This Application or Another 
//        //    if (m.Msg == _APPMSG)
//        //    {
//        //        string Msg1 = Marshal.PtrToStringUni(m.LParam);
//        //        string Msg2 = Marshal.PtrToStringUni(m.WParam);

//        //        //ahk.ahk.MsgBox(Msg1 + " - " + Msg2);
//        //        AppMsgAction(Msg1, Msg2);
//        //    }
//        //}

//        string _AppMsgReturn;



//        public void _AppMsgAction(string Msg1, string Msg2)
//        {
//            ahk.MsgBox(Msg1 + " - " + Msg2);
//            _AppMsgReturn = Msg2; // save global variable to use in return function

//        }  // Actions to Perform Based on Incoming SendMessages

//        public void _SendAppMsg(object WinTitle, string Message1, string Message2 = "")
//        {
//            string VarType = WinTitle.GetType().ToString();  //determine what kind of variable was passed into function

//            IntPtr hWnd = new IntPtr(0); // declare IntPtr (assign tmp value)

//            // user passed in the Window Title as string
//            if (VarType == "System.String")
//            {
//                hWnd = _WinGetHwnd(WinTitle.ToString());  //grab window title handle
//            }

//            // user passed in the handle, activate window
//            if (VarType == "System.IntPtr")
//            {
//                if (!WinTitle.Equals(IntPtr.Zero))
//                {
//                    hWnd = (IntPtr)WinTitle; //declare the object as an IntPtr variable
//                }
//            }


//            GCHandle GCH = GCHandle.Alloc(Message1, GCHandleType.Pinned);
//            IntPtr pS = GCH.AddrOfPinnedObject();

//            GCHandle GCH2 = GCHandle.Alloc(Message2, GCHandleType.Pinned);
//            IntPtr pS2 = GCH2.AddrOfPinnedObject();


//            _SendMessage(hWnd, _APPMSG, pS2, pS);
//            GCH.Free();
//        }  // send message to another application via sendmessage

//        public string _ReturnAppMsg(object WinTitle, string Message1, string Message2 = "")
//        {
//            _AppMsgReturn = "";

//            _SendAppMsg(WinTitle, Message1, Message2);

//            do
//            {
//                ahk.Sleep(20);
//            }
//            while (_AppMsgReturn == "");


//            return _AppMsgReturn;
//        } // send message to another application via sendmessage and wait on response sendmessage


//        #endregion

//        #region === v1 AHK DLL ===

//        public void _AHKDLL()  // ahk dll testing
//        {
//            //create an autohotkey engine (AHK DLL)
//            if (sharpAHK._AHK.ahkGlobal.ahkdll == null) { sharpAHK._AHK.ahkGlobal.ahkdll = new AutoHotkey.Interop.AutoHotkeyEngine(); }
//            var ahkdll = sharpAHK._AHK.ahkGlobal.ahkdll;

//            ahkdll.ExecRaw("F12::Msgbox Hotkey Pressed");
//            //ahkdll.ExecRaw("::.btw::by the way");
//        }

//        public void _AHKDLL2()   // ahk dll testing
//        {
//            //create an autohotkey engine (AHK DLL) or use existing instance if it hasn't be initiated
//            if (sharpAHK._AHK.ahkGlobal.ahkdll == null) { sharpAHK._AHK.ahkGlobal.ahkdll = new AutoHotkey.Interop.AutoHotkeyEngine(); }
//            var ahkdll = sharpAHK._AHK.ahkGlobal.ahkdll;

//            ahkdll.ExecRaw("F11::Msgbox Hotkey Pressed");
//            ahkdll.ExecRaw("::.btw::by the way");
//        }


//        public void _AHKDLL_Examples()  // ahk dll examples
//        {
//            //create an autohotkey engine (AHK DLL) or use existing instance if it hasn't be initiated
//            if (sharpAHK._AHK.ahkGlobal.ahkdll == null) { sharpAHK._AHK.ahkGlobal.ahkdll = new AutoHotkey.Interop.AutoHotkeyEngine(); }
//            var ahkdll = sharpAHK._AHK.ahkGlobal.ahkdll;

//            //execute any raw ahk code
//            ahkdll.ExecRaw("MsgBox, Hello World!");

//            //create new hotkeys
//            //ahk.ExecRaw("^a::Send, Hello World");
//            ahkdll.ExecRaw("F12::Msgbox Hotkey Pressed - %A_ThisHotkey%");

//            //programmatically set variables
//            ahkdll.SetVar("x", "1");
//            ahkdll.SetVar("y", "4");

//            //execute statements
//            ahkdll.ExecRaw("z:=x+y");

//            //return variables back from ahk
//            string zValue = ahkdll.GetVar("z");
//            Console.WriteLine("Value of z is {0}", zValue); // "Value of z is 5"

//            //Load a library or exec scripts in a file
//            //ahkdll.Load("functions.ahk");
//            ahkdll.LoadFile("functions.ahk");

//            //execute a specific function (found in functions.ahk), with 2 parameters
//            ahkdll.ExecFunction("MyFunction", "Hello", "World");

//            //execute a label 
//            ahkdll.ExecLabel("DOSTUFF");

//            //create a new function
//            string sayHelloFunction = "SayHello(name) \r\n { \r\n MsgBox, Hello %name% \r\n return \r\n }";
//            ahkdll.ExecRaw(sayHelloFunction);

//            //execute's newly made function\
//            ahkdll.ExecRaw(@"SayHello(""Mario"") ");


//            //execute a function (in functions.ahk) that adds 5 and return results
//            var add5Results = ahkdll.Eval("Add5( 5 )");
//            Console.WriteLine("Eval: Result of 5 with Add5 func is {0}", add5Results);

//            //you can also return results with the ExecFunction 
//            add5Results = ahkdll.ExecFunction("Add5", "5");
//            Console.WriteLine("ExecFunction: Result of 5 with Add5 func is {0}", add5Results);


//            Console.WriteLine("Press enter to exit...");
//            Console.ReadLine();
//        }


//        public void _RunCode(string AHKLine, bool NewAHK = false)  // execute ahk line in ahk.dll 
//        {
//            //create an autohotkey engine (AHK DLL) or use existing instance if it hasn't be initiated
//            if (sharpAHK._AHK.ahkGlobal.ahkdll == null) { sharpAHK._AHK.ahkGlobal.ahkdll = new AutoHotkey.Interop.AutoHotkeyEngine(); }
//            var ahkdll = sharpAHK._AHK.ahkGlobal.ahkdll;

//            if (NewAHK) // new ahk session, clears out previous variables
//            {
//                ahkdll = new AutoHotkey.Interop.AutoHotkeyEngine();
//            }

//            //execute any raw ahk code
//            ahkdll.ExecRaw(AHKLine);
//        }


//        public string _CodeReturn(string AHKLine = "")  // test function - execute ahk.dll function and return value (not finished/configured)
//        {
//            AHKLine = "WinGet, z, ID, A"; // return active ID / HWND

//            //create an autohotkey engine (AHK DLL) or use existing instance if it hasn't be initiated
//            if (sharpAHK._AHK.ahkGlobal.ahkdll == null) { sharpAHK._AHK.ahkGlobal.ahkdll = new AutoHotkey.Interop.AutoHotkeyEngine(); }
//            var ahkdll = sharpAHK._AHK.ahkGlobal.ahkdll;

//            ////execute any raw ahk code
//            ahkdll.ExecRaw(AHKLine);


//            ////return variables back from ahk
//            string zValue = ahkdll.GetVar("z");

//            //ahk.MsgBox(zValue);
//            return zValue;
//        }


//        #endregion

//        #region === v1 Controls ===

//        public string _ControlGetText(object Control = null, object WinTitle = null, string WinText = "", string ExcludeTitle = "", string ExcludeText = "")  // gets text for control 
//        {
//            string ControlID = "";
//            string SearchWin = "";

//            if (Control != null && WinTitle != null)
//            {
//                IntPtr hWnd = _WinHwnd(WinTitle);  //returns Window Handle (from either handle or window title)
//                SearchWin = "ahk_id " + hWnd.ToString();
//            }

//            if (Control != null)
//            {
//                string ControlVarType = Control.GetType().ToString();  //determine what kind of variable was passed into function

//                // user passed in the control name/class as string
//                if (ControlVarType == "System.String") { ControlID = Control.ToString(); }

//                // user passed in a control handle, set to the SearchWin parameter to search by the control id
//                if (ControlVarType == "System.IntPtr") { ControlID = ""; SearchWin = "ahk_id " + Control.ToString(); }
//            }


//            if (sharpAHK._AHK.ahkGlobal.ahkdll == null) { sharpAHK._AHK.ahkGlobal.ahkdll = new AutoHotkey.Interop.AutoHotkeyEngine(); }
//            var ahkdll = sharpAHK._AHK.ahkGlobal.ahkdll;


//            ahkdll.SetVar("OutputVar", "");
//            string Command = "ControlGetText, OutputVar, " + ControlID + "," + SearchWin + "," + WinText + "," + ExcludeTitle + "," + ExcludeText;
//            ahkdll.ExecRaw(Command);
//            string WinTextReturn = ahkdll.GetVar("OutputVar");



//            //   Error Handling
//            ahkGlobal.ErrorLevelMsg = "ControlGetText Unable To Read Control Text";
//            ahkGlobal.ErrorLevel = false;
//            string ErrorLevelValue = ahkdll.GetVar("ErrorLevel");
//            if (ErrorLevelValue == "0") { ahkGlobal.ErrorLevelMsg = ""; }
//            if (ErrorLevelValue == "1") { ahkGlobal.ErrorLevel = true; }
//            if (ahkGlobal.Debug) { if (ahkGlobal.ErrorLevel) { ahk.MsgBox(ahkGlobal.ErrorLevelMsg, "ErrorLevel"); } } //Display Error Messages in Debug Mode


//            return WinTextReturn;
//        }

//        public bool _ControlSetText(object Control = null, string NewText = "", object WinTitle = null, string WinText = "", string ExcludeTitle = "", string ExcludeText = "")  // sets new text on control
//        {
//            string ControlID = "";
//            string SearchWin = "";

//            if (Control != null && WinTitle != null)
//            {
//                IntPtr hWnd = _WinHwnd(WinTitle);  //returns Window Handle (from either handle or window title)
//                SearchWin = "ahk_id " + hWnd.ToString();
//            }

//            if (Control != null)
//            {
//                string ControlVarType = Control.GetType().ToString();  //determine what kind of variable was passed into function

//                // user passed in the control name/class as string
//                if (ControlVarType == "System.String") { ControlID = Control.ToString(); }

//                // user passed in a control handle, set to the SearchWin parameter to search by the control id
//                if (ControlVarType == "System.IntPtr") { ControlID = ""; SearchWin = "ahk_id " + Control.ToString(); }
//            }


//            if (sharpAHK._AHK.ahkGlobal.ahkdll == null) { sharpAHK._AHK.ahkGlobal.ahkdll = new AutoHotkey.Interop.AutoHotkeyEngine(); }
//            var ahkdll = sharpAHK._AHK.ahkGlobal.ahkdll;


//            //ahkdll.SetVar("OutputVar", "");
//            string Command = "ControlSetText, " + ControlID + "," + NewText + "," + SearchWin + "," + WinText + "," + ExcludeTitle + "," + ExcludeText;
//            ahkdll.ExecRaw(Command);



//            //   Error Handling
//            ahkGlobal.ErrorLevelMsg = "ControlSetText Failed";
//            ahkGlobal.ErrorLevel = false;
//            string ErrorLevelValue = ahkdll.GetVar("ErrorLevel");
//            if (ErrorLevelValue == "0") { ahkGlobal.ErrorLevelMsg = ""; }
//            if (ErrorLevelValue == "1") { ahkGlobal.ErrorLevel = true; }
//            if (ahkGlobal.Debug) { if (ahkGlobal.ErrorLevel) { ahk.MsgBox(ahkGlobal.ErrorLevelMsg, "ErrorLevel"); } } //Display Error Messages in Debug Mode


//            return ahkGlobal.ErrorLevel;
//        }

//        public conInfo _ControlGetPos(object Control = null, object WinTitle = null, string WinText = "", string ExcludeTitle = "", string ExcludeText = "")  //returns wInfo object with control's position 
//        {
//            conInfo con = new conInfo();
//            string ControlID = "";
//            string SearchWin = "";

//            if (Control != null && WinTitle != null)
//            {
//                IntPtr hWnd = _WinHwnd(WinTitle);  //returns Window Handle (from either handle or window title)
//                SearchWin = "ahk_id " + hWnd.ToString();
//            }

//            if (Control != null)
//            {
//                string ControlVarType = Control.GetType().ToString();  //determine what kind of variable was passed into function

//                // user passed in the control name/class as string
//                if (ControlVarType == "System.String") { ControlID = Control.ToString(); }

//                // user passed in a control handle, set to the SearchWin parameter to search by the control id
//                if (ControlVarType == "System.IntPtr") { ControlID = ""; SearchWin = "ahk_id " + Control.ToString(); }
//            }


//            if (sharpAHK._AHK.ahkGlobal.ahkdll == null) { sharpAHK._AHK.ahkGlobal.ahkdll = new AutoHotkey.Interop.AutoHotkeyEngine(); }
//            var ahkdll = sharpAHK._AHK.ahkGlobal.ahkdll;

//            string Command = "ControlGetPos, ControlX, ControlY, ControlW, ControlH," + ControlID + "," + SearchWin + "," + WinText + "," + ExcludeTitle + "," + ExcludeText;
//            ahkdll.ExecRaw(Command);

//            string contX = ahkdll.GetVar("ControlX");
//            string contY = ahkdll.GetVar("ControlY");
//            string contW = ahkdll.GetVar("ControlW");
//            string contH = ahkdll.GetVar("ControlH");

//            if (contX != "")
//            {
//                con.ControlX = Int32.Parse(contX);
//                con.ControlY = Int32.Parse(contY);
//                con.ControlW = Int32.Parse(contW);
//                con.ControlH = Int32.Parse(contH);
//            }

//            return con;
//        }

//        public string _ControlGet(string Cmd, string Value = "", object Control = null, object WinTitle = null, string WinText = "", string ExcludeTitle = "", string ExcludeText = "")  // function to get info about a control (controlget function wrapper for ahk dll)
//        {
//            string ControlID = "";
//            string SearchWin = "";

//            if (Control != null && WinTitle != null)
//            {
//                IntPtr hWnd = _WinHwnd(WinTitle);  //returns Window Handle (from either handle or window title)
//                SearchWin = "ahk_id " + hWnd.ToString();
//            }

//            if (Control != null)
//            {
//                string ControlVarType = Control.GetType().ToString();  //determine what kind of variable was passed into function

//                // user passed in the control name/class as string
//                if (ControlVarType == "System.String") { ControlID = Control.ToString(); }

//                // user passed in a control handle, set to the SearchWin parameter to search by the control id
//                if (ControlVarType == "System.IntPtr") { ControlID = ""; SearchWin = "ahk_id " + Control.ToString(); }
//            }


//            if (sharpAHK._AHK.ahkGlobal.ahkdll == null) { sharpAHK._AHK.ahkGlobal.ahkdll = new AutoHotkey.Interop.AutoHotkeyEngine(); }
//            var ahkdll = sharpAHK._AHK.ahkGlobal.ahkdll;

//            string Command = "ControlGet,OutputVar," + Cmd + "," + Value + "," + ControlID + "," + SearchWin + "," + WinText + "," + ExcludeTitle + "," + ExcludeText;
//            ahkdll.ExecRaw(Command);
//            string OutputVar = ahkdll.GetVar("OutputVar");

//            //ahk.MsgBox(OutputVar);


//            //   Error Handling
//            ahkGlobal.ErrorLevelMsg = "ControlGet Unable Return Value. Window/Control May Not Exist";
//            ahkGlobal.ErrorLevel = false;
//            string ErrorLevelValue = ahkdll.GetVar("ErrorLevel");
//            if (ErrorLevelValue == "0") { ahkGlobal.ErrorLevelMsg = ""; }
//            if (ErrorLevelValue == "1") { ahkGlobal.ErrorLevel = true; }
//            if (ahkGlobal.Debug) { if (ahkGlobal.ErrorLevel) { ahk.MsgBox(ahkGlobal.ErrorLevelMsg, "ErrorLevel"); } } //Display Error Messages in Debug Mode


//            return OutputVar;
//        }

//        public IntPtr _ControlGetHwnd(object Control = null, object WinTitle = null, string WinText = "", string ExcludeTitle = "", string ExcludeText = "")  // returns a control's handle (control get wrapper for ahk.dll)
//        {
//            string ControlID = "";
//            string SearchWin = "";

//            if (Control != null && WinTitle != null)
//            {
//                IntPtr hWnd = _WinHwnd(WinTitle);  //returns Window Handle (from either handle or window title)
//                SearchWin = "ahk_id " + hWnd.ToString();
//            }

//            if (Control != null)
//            {
//                string ControlVarType = Control.GetType().ToString();  //determine what kind of variable was passed into function

//                // user passed in the control name/class as string
//                if (ControlVarType == "System.String") { ControlID = Control.ToString(); }

//                // user passed in a control handle, set to the SearchWin parameter to search by the control id
//                if (ControlVarType == "System.IntPtr") { ControlID = ""; SearchWin = "ahk_id " + Control.ToString(); }
//            }


//            if (sharpAHK._AHK.ahkGlobal.ahkdll == null) { sharpAHK._AHK.ahkGlobal.ahkdll = new AutoHotkey.Interop.AutoHotkeyEngine(); }
//            var ahkdll = sharpAHK._AHK.ahkGlobal.ahkdll;

//            string Command = "ControlGet,OutputVar,Hwnd,," + ControlID + "," + SearchWin + "," + WinText + "," + ExcludeTitle + "," + ExcludeText;
//            ahkdll.ExecRaw(Command);
//            string OutputVar = ahkdll.GetVar("OutputVar");

//            int ID = Int32.Parse(OutputVar);  // string to int
//            IntPtr ControlHwnd = ahk.ToIntPtr(ID);  // int to IntPtr



//            //ahk.MsgBox(OutputVar);

//            //   Error Handling
//            ahkGlobal.ErrorLevelMsg = "ControlGet Unable Return Value. Window/Control May Not Exist";
//            ahkGlobal.ErrorLevel = false;
//            string ErrorLevelValue = ahkdll.GetVar("ErrorLevel");
//            if (ErrorLevelValue == "0") { ahkGlobal.ErrorLevelMsg = ""; }
//            if (ErrorLevelValue == "1") { ahkGlobal.ErrorLevel = true; }
//            if (ahkGlobal.Debug) { if (ahkGlobal.ErrorLevel) { ahk.MsgBox(ahkGlobal.ErrorLevelMsg, "ErrorLevel"); } } //Display Error Messages in Debug Mode


//            return ControlHwnd;
//        }





//        #endregion

//        #region === v1 Web ===

//        public string _ReturnURL(string URLFile) // read URL from file, returns web address
//        {
//            string URL = ahk.IniRead(URLFile, "InternetShortcut", "URL");
//            return URL;
//        }
//        public void _WriteURL(string linkName, string SaveDir, string linkURL) // write URL link file to local folder
//        {
//            ahk.IniWrite(linkURL, SaveDir + "\\" + linkName + ".url", "InternetShortcut", "URL");
//        }



//        #endregion

//        #region === v1 Date / Time Lib (UnTested) ===


//        public void _Time_Difference(DateTime date1, DateTime date2)
//        {
//            //DateTime date1 = dateTimePicker1.Value;
//            //DateTime date2 = dateTimePicker2.Value;

//            TimeSpan difference = date2 - date1;
//            string days = "Days: " + difference.TotalDays.ToString();
//            string hours = "Hours: " + difference.TotalHours.ToString();
//            string minutes = "Minutes: " + difference.TotalMinutes.ToString();
//            string seconds = "Seconds: " + difference.TotalSeconds.ToString();
//            string milliseconds = "Milliseconds: " + difference.TotalMilliseconds.ToString();
//        }



//        #endregion

//        #region === v1 Zip ===

//        // using System.IO.Compression;

//        public bool _Zip_File(string FileToZip, string OutZipFile = "")
//        {
//            //=====================================================================================================
//            // Creates Temp Folder - Moves Single File to Folder - Zips Folder Contents --- Single ZIP File
//            //=====================================================================================================


//            System.IO.FileInfo fileinfo = new System.IO.FileInfo(FileToZip); //retrieve info about each file
//            string FileName = fileinfo.Name.ToString();
//            string TempDir = "C:\\__ZipTemp";
//            ahk.FileCreateDir(TempDir);
//            ahk.Sleep(1000);
//            ahk.FileCopy(FileToZip, TempDir + "\\" + FileName);
//            if (OutZipFile == "")
//            {
//                OutZipFile = "c:\\" + FileName + ".zip";  //set out file name if not provided 
//            }
//            ahk.FileDelete(OutZipFile); // delete existing zip file if it exists
//            ZipFile.CreateFromDirectory(TempDir, OutZipFile, CompressionLevel.Fastest, true);
//            ahk.FileRemoveDir(TempDir);  // clean up temp space
//            return true;
//        }

//        public bool _Zip_Folder(string ZipDir, string OutZipFile, string Options = "")
//        {
//            ahk.FileDelete(OutZipFile);
//            ZipFile.CreateFromDirectory(ZipDir, OutZipFile, CompressionLevel.Fastest, true);
//            return true;
//        }

//        public bool _Zip_UnZipDir(string ZipPath, string UnZipDir)
//        {
//            ZipFile.ExtractToDirectory(ZipPath, UnZipDir);
//            return true;
//        }


//        #endregion

//        #region === v1 Email ===

//        //###################################
//        //#### E-Mail #######################
//        //###################################

//        public bool _SendEmail(string EmailList = "", string EmailBodyText = "", string Subject = "")
//        {

//            //SmtpClient ss = new SmtpClient();

//            //string FromEmail = "outgoing@macrosolutions.me";
//            //string FromPass = "OutGoingMail88";
//            //string EmailSubject = "SQLiter";
//            //string EmailBody = "Cause it SENT...";

//            ////string ToEmail = "jason.langan@gmail.com";
//            //string ToEmail = "2516220514@txt.att.net";

//            //try
//            //{
//            //    ss.Host = "mail.macrosolutions.me";
//            //    ss.Port = 25;
//            //    ss.Timeout = 10000;
//            //    ss.DeliveryMethod = SmtpDeliveryMethod.Network;
//            //    ss.UseDefaultCredentials = false;
//            //    ss.Credentials = new NetworkCredential(FromEmail, FromPass);

//            //    MailMessage mailMsg = new MailMessage(FromEmail, ToEmail, EmailSubject, EmailBody);
//            //    mailMsg.DeliveryNotificationOptions = DeliveryNotificationOptions.OnFailure;
//            //    ss.Send(mailMsg);

//            //    //MessageBox.Show("Sent Email");
//            //    return true;
//            //}
//            //catch (Exception ex)
//            //{
//            //    MessageBox.Show(ex.Message);
//            //    return false;
//            //}

//            return false;
//        }

//        public bool _TextJason(string EmailBody = "", string EmailSubject = "")
//        {
//            //SmtpClient ss = new SmtpClient();

//            //string FromName = "SQLiter@MacroSolutions.me";
//            //string FromEmail = "outgoing@macrosolutions.me";
//            //string FromPass = "OutGoingMail88";
//            ////EmailBody = "Cause it SENT...";

//            //if (EmailSubject == "")
//            //{
//            //    EmailSubject = DateTime.Now.ToShortTimeString();
//            //}


//            ////string ToEmail = "jason.langan@gmail.com";
//            //string ToEmail = "2516220514@txt.att.net";

//            //try
//            //{
//            //    ss.Host = "mail.macrosolutions.me";
//            //    ss.Port = 25;
//            //    ss.Timeout = 10000;
//            //    ss.DeliveryMethod = SmtpDeliveryMethod.Network;
//            //    ss.UseDefaultCredentials = false;
//            //    ss.Credentials = new NetworkCredential(FromEmail, FromPass);

//            //    MailMessage mailMsg = new MailMessage(FromName, ToEmail, EmailSubject, EmailBody);
//            //    mailMsg.DeliveryNotificationOptions = DeliveryNotificationOptions.OnFailure;
//            //    ss.Send(mailMsg);

//            //    MessageBox.Show("Sent Email");
//            //    return true;
//            //}
//            //catch (Exception ex)
//            //{
//            //    MessageBox.Show(ex.Message);
//            //    return false;
//            //}

//            return false;
//        }



//        #endregion

//        #region === v1 Windows Management ===

//        //################################################
//        //       Win Functions
//        //################################################

//        //===== Get Window Handle =======
//        public IntPtr _WinHwnd(object WinTitle)  // returns Window Handle (from either handle or window title)
//        {
//            string VarType = WinTitle.GetType().ToString();  //determine what kind of variable was passed into function

//            IntPtr hWnd = new IntPtr(0); // declare IntPtr (assign tmp value)

//            // user passed in the Window Title as string
//            if (VarType == "System.String")
//            {
//                hWnd = _WinGetHwnd(WinTitle.ToString());  //grab window title handle
//            }

//            // user passed in the handle, activate window
//            if (VarType == "System.IntPtr")
//            {
//                if (!WinTitle.Equals(IntPtr.Zero))
//                {
//                    hWnd = (IntPtr)WinTitle; //declare the object as an IntPtr variable
//                }
//            }

//            return hWnd;
//        }
//        public IntPtr _WinGetHwnd(string WindowTitle = "A")  // returns window handle, default "A" returns active window handle
//        {
//            IntPtr hWnd = IntPtr.Zero;

//            // c# version - works
//            foreach (Process pList in Process.GetProcesses())
//            {
//                if (pList.MainWindowTitle.Contains(WindowTitle))
//                {
//                    hWnd = pList.MainWindowHandle;
//                }
//            }
//            return hWnd; //Should contain the handle but may be zero if the title doesn't match


//            //// grabs ahk id
//            //if (sharpAHK._AHK.ahkGlobal.ahkdll == null) { sharpAHK._AHK.ahkGlobal.ahkdll = new AutoHotkey.Interop.AutoHotkeyEngine(); }
//            //var ahkdll = sharpAHK._AHK.ahkGlobal.ahkdll;

//            //string WinTitle = "Untitled - Notepad";
//            //string Command = "WinGet, active_id, ID, " + WinTitle;
//            ////string Command = "WinID := WinGetID(\"" + WindowTitle + "\")"; 
//            //ahkdll.ExecRaw(Command);
//            //string OutPutVar = ahkdll.GetVar("active_id");
//            //ahk.MsgBox(OutPutVar);

//            //return hWnd;
//            ////hWnd = ToIntPtr(OutPutVar); 

//            //return hWnd; 
//        }
//        public IntPtr _hWndFromProcessName(string ProcessName)  // returns the handle associated with a process name
//        {
//            IntPtr hWnd = IntPtr.Zero;
//            Process[] processes = Process.GetProcessesByName(ProcessName);
//            try
//            {
//                Process lol = processes[0];
//                hWnd = lol.MainWindowHandle;
//            }
//            catch { }
//            //int handle = hWnd.ToInt32();
//            return hWnd; //Should contain the handle but may be zero if the title doesn't match
//        }


//        public sharpAHK.winInfo _Return_wInfo(object WinTitle)  // populate wInfo object from the window title
//        {
//            //public wInfo MouseGetPos()  // gets the current mouse position, returns wInfo object populated (includes control info/handles)

//            if (sharpAHK._AHK.ahkGlobal.ahkdll == null) { sharpAHK._AHK.ahkGlobal.ahkdll = new AutoHotkey.Interop.AutoHotkeyEngine(); }
//            var ahkdll = sharpAHK._AHK.ahkGlobal.ahkdll;

//            sharpAHK.winInfo info = new sharpAHK.winInfo(); // declare the object to return populated



//            //ahkdll.SetVar("OutputVar", "");
//            string Command = "MouseGetPos, MouseX, MouseY, WinID, ControlID, 3";
//            ahkdll.ExecRaw(Command);

//            //info.MouseXPos = Int32.Parse(ahkdll.GetVar("MouseX"));
//            //info.MouseYPos = Int32.Parse(ahkdll.GetVar("MouseY"));

//            ////// convert string to IntPtr
//            ////string winHwnd = ahkdll.GetVar("WinID");
//            ////if (winHwnd != "")
//            ////{
//            ////    int WinID = Int32.Parse(winHwnd);  // string to int
//            ////    info.WinHwnd = ToIntPtr(WinID);  // int to IntPtr
//            ////}

//            ////// convert string to IntPtr
//            ////string ControlHwnd = ahkdll.GetVar("ControlID");
//            ////if (ControlHwnd != "")
//            ////{
//            ////    int cID = Int32.Parse(ControlHwnd);  // string to int
//            ////    info.ControlHwnd = ToIntPtr(cID);  // int to IntPtr
//            ////}

//            //info.WinHwnd = WinHwnd(WinTitle);

//            //info.WinClass = WinGetClass(WinTitle);   // win class


//            //// based on the window handle/window title, return a rectangle object populated with top/left/w/h
//            //AHK.AHK.wInfo WinCoords = ahk.WinGetPos(Window);
//            //ahk.ahk.MsgBox("X = " + WinCoords.WinX + Environment.NewLine + "Y = " + WinCoords.WinY + Environment.NewLine + "W = " + WinCoords.WinWidth + Environment.NewLine + "H = " + WinCoords.WinHeight);

//            Rect ReturnRect = new Rect(); // declare the object to return populated
//            IntPtr hWnd = _WinGetHwnd(WinTitle.ToString());  //returns Window Handle (from either handle or window title)

//            _GetWindowRect(hWnd, ref ReturnRect);   //return the window dimensions

//            // set the values / calculate width/height
//            int x = ReturnRect.Left;
//            int y = ReturnRect.Top;

//            int Width = ReturnRect.Right - ReturnRect.Left;
//            int Height = ReturnRect.Bottom - ReturnRect.Top;


//            info.WinX = ReturnRect.Left;
//            info.WinY = ReturnRect.Top;
//            info.WinW = Width;
//            info.WinH = Height;


//            return info;
//        }


//        //=== WinGet Pos======================================
//        // Returns Rect Object with Window Coordinates
//        public sharpAHK.winInfo _WinGetPos(object WinTitle)  // returns a wInfo object with window position populated
//        {
//            //// based on the window handle/window title, return a rectangle object populated with top/left/w/h
//            //AHK.AHK.wInfo WinCoords = ahk.WinGetPos(Window);
//            //ahk.ahk.MsgBox("X = " + WinCoords.WinX + Environment.NewLine + "Y = " + WinCoords.WinY + Environment.NewLine + "W = " + WinCoords.WinWidth + Environment.NewLine + "H = " + WinCoords.WinHeight);

//            sharpAHK.winInfo info = new sharpAHK.winInfo(); // declare the object to return populated
//            Rect ReturnRect = new Rect(); // declare the object to return populated
//            IntPtr hWnd = _WinHwnd(WinTitle);  //returns Window Handle (from either handle or window title)

//            _GetWindowRect(hWnd, ref ReturnRect);   //return the window dimensions

//            // set the values / calculate width/height
//            int x = ReturnRect.Left;
//            int y = ReturnRect.Top;

//            int Width = ReturnRect.Right - ReturnRect.Left;
//            int Height = ReturnRect.Bottom - ReturnRect.Top;


//            info.WinX = ReturnRect.Left;
//            info.WinY = ReturnRect.Top;
//            info.WinW = Width;
//            info.WinH = Height;


//            //MessageBox.Show("Width = " + Width.ToString() + Environment.NewLine + "Height = " + Height.ToString() + Environment.NewLine + "X = " + x.ToString() + "   Y = " + y.ToString());
//            return info;
//        }

//        public string _WinGetClass(object WinTitle)  // returns Class Name from Handle
//        {
//            IntPtr hWnd = _WinHwnd(WinTitle);  //returns Window Handle (from either handle or window title)

//            StringBuilder ClassName = new StringBuilder(256);
//            var nRet = _GetClassName(hWnd, ClassName, ClassName.Capacity);
//            return ClassName.ToString();
//        }


//        //Returns Active Window Title as String
//        public string _WinGetActiveTitle() // Returns Active Window Title as String
//        {
//            const int nChars = 256;
//            StringBuilder Buff = new StringBuilder(nChars);
//            IntPtr handle = _GetForegroundWindow();

//            if (_GetWindowText(handle, Buff, nChars) > 0)
//            {
//                return Buff.ToString();
//            }
//            return null;
//        }

//        public IntPtr _WinGetActiveID()  // returns the active window handle
//        {
//            const int nChars = 256;
//            StringBuilder Buff = new StringBuilder(nChars);
//            IntPtr handle = _GetForegroundWindow();
//            return handle;
//        }

//        public string _WinGetTitle(object WinTitle = null)   // returns Window Title from handle, if blank it grabs the active window title
//        {
//            IntPtr hWnd = new IntPtr(0); // declare IntPtr (assign tmp value)

//            if (WinTitle == null)
//            {
//                hWnd = _WinGetActiveID(); // grabs active window handle if no name is provided
//            }
//            if (WinTitle != null)
//            {
//                hWnd = _WinHwnd(WinTitle);  //returns Window Handle (from either handle or window title)
//            }


//            int handle = hWnd.ToInt32(); // convert to int
//            // get window title from handle
//            StringBuilder title = new StringBuilder(256);
//            _GetWindowText(handle, title, 256);
//            return title.ToString();
//        }

//        // renames existing window title
//        public void _WinSetTitle(object WinTitle = null, string NewTitle = "", string WinText = "", string ExcludeTitle = "", string ExcludeText = "")  // renames existing window title
//        {
//            if (sharpAHK._AHK.ahkGlobal.ahkdll == null) { sharpAHK._AHK.ahkGlobal.ahkdll = new AutoHotkey.Interop.AutoHotkeyEngine(); }
//            var ahkdll = sharpAHK._AHK.ahkGlobal.ahkdll;
//            ahkdll.ExecRaw("WinSetTitle, " + WinTitle.ToString() + "," + WinText + "," + NewTitle + "," + ExcludeTitle + "," + ExcludeText);
//        }



//        //=== Win Activate ===========================================
//        // Pass in the Window Title or Window Handle to Activate
//        public void _WinActivate(object WinTitle)  // Pass in the Window Title or Window Handle to Activate that window
//        {
//            //IntPtr hWnd = WinHwnd(WinTitle);  //returns Window Handle (from either handle or window title)
//            //// Activate by Process Name
//            //if (!hWnd.Equals(IntPtr.Zero))
//            //{
//            //    SetForegroundWindow(hWnd);
//            //}

//            IntPtr hWnd = _WinHwnd(WinTitle);  //returns Window Handle (from either handle or window title)
//            if (sharpAHK._AHK.ahkGlobal.ahkdll == null) { sharpAHK._AHK.ahkGlobal.ahkdll = new AutoHotkey.Interop.AutoHotkeyEngine(); }
//            var ahkdll = sharpAHK._AHK.ahkGlobal.ahkdll;

//            //string Command = "WinActivate, ahk_id " + hWnd.ToString() + "," + WinText + "," + Seconds + "," + ExcludeTitle + "," + ExcludeText;
//            string Command = "WinActivate, ahk_id " + hWnd.ToString();
//            ahkdll.ExecRaw(Command);

//        }

//        public bool _WinWaitActive(string WinTitle = "", string WinText = "", int Seconds = 5, string ExcludeTitle = "", string ExcludeText = "") // waits until window title is active before continuing
//        {
//            if (sharpAHK._AHK.ahkGlobal.ahkdll == null) { sharpAHK._AHK.ahkGlobal.ahkdll = new AutoHotkey.Interop.AutoHotkeyEngine(); }
//            var ahkdll = sharpAHK._AHK.ahkGlobal.ahkdll;

//            string Command = "WinWaitActive," + WinTitle + "," + WinText + "," + Seconds + "," + ExcludeTitle + "," + ExcludeText;
//            ahkdll.ExecRaw(Command);


//            //   Error Handling
//            ahkGlobal.ErrorLevelMsg = "WinWaitActive Timed Out (" + Seconds + " Seconds)";
//            ahkGlobal.ErrorLevel = false;
//            string ErrorLevelValue = ahkdll.GetVar("ErrorLevel");
//            if (ErrorLevelValue == "0") { ahkGlobal.ErrorLevelMsg = ""; }
//            if (ErrorLevelValue == "1") { ahkGlobal.ErrorLevel = true; }
//            if (ahkGlobal.Debug) { if (ahkGlobal.ErrorLevel) { ahk.MsgBox(ahkGlobal.ErrorLevelMsg, "ErrorLevel"); } } //Display Error Messages in Debug Mode


//            if (ahkGlobal.ErrorLevel == false) { return true; }

//            return false;
//        }
//        public bool _WinWaitNotActive(string WinTitle = "", string WinText = "", int Seconds = 5, string ExcludeTitle = "", string ExcludeText = "")  // waits until window isn't active before continuing
//        {
//            if (sharpAHK._AHK.ahkGlobal.ahkdll == null) { sharpAHK._AHK.ahkGlobal.ahkdll = new AutoHotkey.Interop.AutoHotkeyEngine(); }
//            var ahkdll = sharpAHK._AHK.ahkGlobal.ahkdll;

//            string Command = "WinWaitNotActive," + WinTitle + "," + WinText + "," + Seconds + "," + ExcludeTitle + "," + ExcludeText;
//            ahkdll.ExecRaw(Command);


//            //   Error Handling
//            ahkGlobal.ErrorLevelMsg = "WinWaitNotActive Timed Out (" + Seconds + " Seconds)";
//            ahkGlobal.ErrorLevel = false;
//            string ErrorLevelValue = ahkdll.GetVar("ErrorLevel");
//            if (ErrorLevelValue == "0") { ahkGlobal.ErrorLevelMsg = ""; }
//            if (ErrorLevelValue == "1") { ahkGlobal.ErrorLevel = true; }
//            if (ahkGlobal.Debug) { if (ahkGlobal.ErrorLevel) { ahk.MsgBox(ahkGlobal.ErrorLevelMsg, "ErrorLevel"); } } //Display Error Messages in Debug Mode


//            if (ahkGlobal.ErrorLevel == false) { return true; }

//            return false;
//        }


//        //==== WinRestore (From Minimized)==============================

//        public void _WinRestore(object WinTitle)  // Unminimizes or unmaximizes the specified window if it is minimized or maximized.
//        {
//            IntPtr hWnd = _WinHwnd(WinTitle);  //returns Window Handle (from either handle or window title)
//            _ShowWindowAsync(hWnd, 1);
//        }
//        public void WinMaximize(object WinTitle)  // maximize window
//        {
//            IntPtr hWnd = _WinHwnd(WinTitle);  //returns Window Handle (from either handle or window title)
//            _ShowWindowAsync(hWnd, 3); //maximize window
//        }
//        public void WinMinimize(object WinTitle)   // minimize window
//        {
//            IntPtr hWnd = _WinHwnd(WinTitle);  //returns Window Handle (from either handle or window title)
//            _ShowWindowAsync(hWnd, 2);  //minimize window
//        }


//        public bool IsMinimized(object WinTitle)  // returns true if window is minimized
//        {
//            IntPtr hWnd = _WinHwnd(WinTitle);  //returns Window Handle (from either handle or window title)

//            WINDOWPLACEMENT placement = new WINDOWPLACEMENT();
//            _GetWindowPlacement(hWnd, ref placement);
//            switch (placement.showCmd)
//            {
//                case 1:
//                    //Console.WriteLine("Normal");
//                    break;
//                case 2:
//                    return true;
//                //Console.WriteLine("Minimized");
//                case 3:
//                    //Console.WriteLine("Maximized");
//                    break;
//            }

//            return false;
//        }
//        public bool IsMaximized(object WinTitle)  // returns true if window is maximized
//        {
//            IntPtr hWnd = _WinHwnd(WinTitle);  //returns Window Handle (from either handle or window title)

//            WINDOWPLACEMENT placement = new WINDOWPLACEMENT();
//            _GetWindowPlacement(hWnd, ref placement);
//            switch (placement.showCmd)
//            {
//                case 1:
//                    return false;
//                //Console.WriteLine("Normal");
//                case 2:
//                    return false;
//                //Console.WriteLine("Minimized");
//                case 3:
//                    return true;
//                    //Console.WriteLine("Maximized");
//            }

//            return false;
//        }


//        public void _WinMinimizeAll(bool UnDo = false)  // Minimizes All Windows
//        {
//            if (sharpAHK._AHK.ahkGlobal.ahkdll == null) { sharpAHK._AHK.ahkGlobal.ahkdll = new AutoHotkey.Interop.AutoHotkeyEngine(); }
//            var ahkdll = sharpAHK._AHK.ahkGlobal.ahkdll;

//            if (!UnDo) { ahkdll.ExecRaw("WinMinimizeAll"); }
//            if (UnDo) { ahkdll.ExecRaw("WinMinimizeAllUndo"); }


//            return;
//        }
//        public void _WinMinimizeAllUndo()  // UnMinimizes All Windows
//        {
//            if (sharpAHK._AHK.ahkGlobal.ahkdll == null) { sharpAHK._AHK.ahkGlobal.ahkdll = new AutoHotkey.Interop.AutoHotkeyEngine(); }
//            var ahkdll = sharpAHK._AHK.ahkGlobal.ahkdll;

//            ahkdll.ExecRaw("WinMinimizeAllUndo");

//            return;
//        }


//        //=== WinMove --- Set Window Position =============================
//        public void _WinMove(object WinTitle, int x, int y, int w = 0, int h = 0) // moves window position (uses original height/width if 0)
//        {
//            IntPtr hWnd = _WinHwnd(WinTitle);  //returns Window Handle (from either handle or window title)

//            const short SWP_NOMOVE = 0X2;
//            const short SWP_NOSIZE = 1;
//            const short SWP_NOZORDER = 0X4;
//            const int SWP_SHOWWINDOW = 0x0040;

//            //hWnd Insert After (2nd parameter in SetWindowPos)
//            IntPtr HWND_TOPMOST = new IntPtr(-1);
//            IntPtr HWND_NOTOPMOST = new IntPtr(-2);
//            IntPtr HWND_TOP = new IntPtr(0);
//            IntPtr HWND_BOTTOM = new IntPtr(1);

//            uint NOSIZE = 0x0001;
//            uint NOMOVE = 0x0002;
//            uint NOZORDER = 0x0004;
//            uint NOREDRAW = 0x0008;
//            uint NOACTIVATE = 0x0010;
//            uint DRAWFRAME = 0x0020;
//            uint FRAMECHANGED = 0x0020;
//            uint SHOWWINDOW = 0x0040;
//            uint HIDEWINDOW = 0x0080;
//            uint NOCOPYBITS = 0x0100;
//            uint NOOWNERZORDER = 0x0200;
//            uint NOREPOSITION = 0x0200;
//            uint NOSENDCHANGING = 0x0400;
//            uint DEFERERASE = 0x2000;
//            uint ASYNCWINDOWPOS = 0x4000;

//            sharpAHK.winInfo WinSize = _WinGetPos(WinTitle); // current w/h used if not provided by user

//            if (w == 0)  // use original width if not provided
//            {
//                w = WinSize.WinW;
//            }
//            if (h == 0)  // use original height if not provided
//            {
//                h = WinSize.WinH;
//            }

//            _SetWindowPos(hWnd, HWND_TOPMOST, x, y, w, h, SHOWWINDOW);
//        }



//        //=== WinClose --- Close Window by WinTitle / Handle ==============
//        public void _WinClose(object WinTitle)  //Close Window by WinTitle / Handle
//        {
//            IntPtr hWnd = _WinHwnd(WinTitle);  //returns Window Handle (from either handle or window title)
//            _SendMessage(hWnd, WM_CLOSE, IntPtr.Zero, IntPtr.Zero);
//        }
//        public bool WinWaitClose(object WinTitle = null, string WinText = "", int Seconds = 5, string ExcludeTitle = "", string ExcludeText = "")  // waits for windows to close (pass wintitle or handle)
//        {
//            IntPtr hWnd = _WinHwnd(WinTitle);  //returns Window Handle (from either handle or window title)
//            if (sharpAHK._AHK.ahkGlobal.ahkdll == null) { sharpAHK._AHK.ahkGlobal.ahkdll = new AutoHotkey.Interop.AutoHotkeyEngine(); }
//            var ahkdll = sharpAHK._AHK.ahkGlobal.ahkdll;

//            string Command = "WinWaitClose, ahk_id " + hWnd + "," + WinText + "," + Seconds + "," + ExcludeTitle + "," + ExcludeText;
//            ahkdll.ExecRaw(Command);



//            //   Error Handling
//            ahkGlobal.ErrorLevelMsg = "WinWaitClose Timed Out (" + Seconds + " Seconds - " + WinTitle + " Still Running)";
//            ahkGlobal.ErrorLevel = false;
//            string ErrorLevelValue = ahkdll.GetVar("ErrorLevel");
//            if (ErrorLevelValue == "0") { ahkGlobal.ErrorLevelMsg = ""; }
//            if (ErrorLevelValue == "1") { ahkGlobal.ErrorLevel = true; }
//            if (ahkGlobal.Debug) { if (ahkGlobal.ErrorLevel) { ahk.MsgBox(ahkGlobal.ErrorLevelMsg, "ErrorLevel"); } } //Display Error Messages in Debug Mode


//            if (ahkGlobal.ErrorLevel == false) { return true; }

//            return false;
//        }
//        public void _WinKill(string WinTitle, string WinText = "", string SecondsToWait = "", string ExcludeTitle = "", string ExcludeText = "")  // Forces the specified window to close.
//        {
//            //IntPtr hWnd = WinHwnd(WinTitle);  //returns Window Handle (from either handle or window title)
//            if (sharpAHK._AHK.ahkGlobal.ahkdll == null) { sharpAHK._AHK.ahkGlobal.ahkdll = new AutoHotkey.Interop.AutoHotkeyEngine(); }
//            var ahkdll = sharpAHK._AHK.ahkGlobal.ahkdll;

//            ahkdll.ExecRaw("WinKill, " + WinTitle + "," + WinText + "," + SecondsToWait + "," + ExcludeTitle + "," + ExcludeText);
//        }


//        public void WinAlwaysOnTop(object WinTitle, bool SetAlwaysOnTop = true)  // Set Window to be alway on top of other windows on screen (use SetAlwaysOnTop = false to toggle back to normal view mode)
//        {
//            IntPtr hWnd = _WinHwnd(WinTitle);  //returns Window Handle (from either handle or window title)

//            if (SetAlwaysOnTop == true)
//            {
//                _SetWindowPos(hWnd, HWND_TOPMOST, 0, 0, 0, 0, TOPMOST_FLAGS);
//            }

//            if (SetAlwaysOnTop == false)
//            {
//                _SetWindowPos(hWnd, HWND_NOTOPMOST, 0, 0, 0, 0, TOPMOST_FLAGS);
//            }

//        }

//        public void _AlwaysOnTop(IntPtr hWnd, bool SetAlwaysOnTop = true)
//        {
//            if (SetAlwaysOnTop == true)
//            {
//                _SetWindowPos(hWnd, HWND_TOPMOST, 0, 0, 0, 0, TOPMOST_FLAGS);
//            }

//            if (SetAlwaysOnTop == false)
//            {
//                _SetWindowPos(hWnd, HWND_NOTOPMOST, 0, 0, 0, 0, TOPMOST_FLAGS);
//            }
//        }


//        public bool _WinExist(string WinTitle)  // returns true/false if the window title currently exists
//        {
//            //=== Create List of All Window Titles ===============================

//            Dictionary<string, string> WinList = new Dictionary<string, string>();

//            WinList.Clear();

//            Process[] processlist = Process.GetProcesses();

//            foreach (Process process in processlist)
//            {
//                if (!String.IsNullOrEmpty(process.MainWindowTitle))
//                {
//                    //Console.WriteLine("Process: {0} ID: {1} Window title: {2}", process.ProcessName, process.Id, process.MainWindowTitle);
//                    //ahk.MsgBox("ProcessName: " + process.ProcessName + Environment.NewLine + "ProcessID: " + process.Id + Environment.NewLine + "WinTitle: " + process.MainWindowTitle);

//                    // add to list if not already in the list
//                    if (!WinList.ContainsKey(process.MainWindowTitle))
//                    {
//                        WinList.Add(process.MainWindowTitle, process.MainWindowTitle);
//                    }


//                }
//            }


//            //=== Check to see if User's Window Title is in List of Window Titles

//            if (WinList.ContainsKey(WinTitle))
//            {
//                return true;
//            }

//            return false;
//        }

//        public bool _WinWait(object WinTitle = null, string WinText = "", int Seconds = 5, string ExcludeTitle = "", string ExcludeText = "")  // waits for window title to exist
//        {
//            IntPtr hWnd = _WinHwnd(WinTitle);  //returns Window Handle (from either handle or window title)
//            if (sharpAHK._AHK.ahkGlobal.ahkdll == null) { sharpAHK._AHK.ahkGlobal.ahkdll = new AutoHotkey.Interop.AutoHotkeyEngine(); }
//            var ahkdll = sharpAHK._AHK.ahkGlobal.ahkdll;

//            string Command = "WinWait, ahk_id " + hWnd.ToString() + "," + WinText + "," + Seconds + "," + ExcludeTitle + "," + ExcludeText;
//            ahkdll.ExecRaw(Command);

//            //   Error Handling
//            ahkGlobal.ErrorLevelMsg = "WinWait Timed Out.";
//            ahkGlobal.ErrorLevel = false;
//            string ErrorLevelValue = ahkdll.GetVar("ErrorLevel");
//            if (ErrorLevelValue == "0") { ahkGlobal.ErrorLevelMsg = ""; }
//            if (ErrorLevelValue == "1") { ahkGlobal.ErrorLevel = true; }
//            if (ahkGlobal.Debug) { if (ahkGlobal.ErrorLevel) { ahk.MsgBox(ahkGlobal.ErrorLevelMsg, "ErrorLevel"); } } //Display Error Messages in Debug Mode


//            bool WinFound = false;
//            if (ahkGlobal.ErrorLevel == false) { WinFound = true; }

//            return WinFound;
//        }

//        public List<string> _WinList(string WinTitle)  // returns list of window titles detected
//        {
//            List<string> WinList = new List<string>();

//            Process[] processlist = Process.GetProcesses();

//            foreach (Process process in processlist)
//            {
//                if (!String.IsNullOrEmpty(process.MainWindowTitle))
//                {
//                    //Console.WriteLine("Process: {0} ID: {1} Window title: {2}", process.ProcessName, process.Id, process.MainWindowTitle);
//                    //ahk.MsgBox("ProcessName: " + process.ProcessName + Environment.NewLine + "ProcessID: " + process.Id + Environment.NewLine + "WinTitle: " + process.MainWindowTitle);

//                    // add to list if not already in the list
//                    WinList.Add(process.MainWindowTitle);
//                }
//            }

//            return WinList;
//        }

//        public string _WinGetText(object WinTitle = null, string WinText = "", string ExcludeTitle = "", string ExcludeText = "")  // returns visible text from window
//        {
//            if (sharpAHK._AHK.ahkGlobal.ahkdll == null) { sharpAHK._AHK.ahkGlobal.ahkdll = new AutoHotkey.Interop.AutoHotkeyEngine(); }
//            var ahkdll = sharpAHK._AHK.ahkGlobal.ahkdll;
//            IntPtr hWnd = _WinHwnd(WinTitle);  //returns Window Handle (from either handle or window title)

//            ahkdll.SetVar("WinText", "");
//            string Command = "WinGetText, WinText, ahk_id " + hWnd.ToString() + "," + WinText + "," + ExcludeTitle + "," + ExcludeText;
//            ahkdll.ExecRaw(Command);
//            string WinTextReturn = ahkdll.GetVar("WinText");

//            return WinTextReturn;
//        }

//        public string _WinText(IntPtr hWnd)  // returns text from window (by win handle)
//        {
//            //StringBuilder builder = new StringBuilder(500);
//            //int handle = hWnd.ToInt32(); 
//            ////Get the text from the active window into the stringbuilder
//            //SendMessage(handle, WM_GETTEXT, builder.Capacity, builder);
//            //return builder.ToString();

//            int handle = hWnd.ToInt32();
//            const int WM_GETTEXT = 0x0D;
//            StringBuilder sb = new StringBuilder();
//            int retVal = _SendMessage(handle, WM_GETTEXT, 100, sb);
//            //MessageBox.Show(sb.ToString());

//            return sb.ToString();
//        }


//        public void _WinShow(object WinTitle = null, string WinText = "", string ExcludeTitle = "", string ExcludeText = "")  // shows a window (if already hidden)
//        {
//            IntPtr hWnd = _WinHwnd(WinTitle);  //returns Window Handle (from either handle or window title)
//            if (sharpAHK._AHK.ahkGlobal.ahkdll == null) { sharpAHK._AHK.ahkGlobal.ahkdll = new AutoHotkey.Interop.AutoHotkeyEngine(); }
//            var ahkdll = sharpAHK._AHK.ahkGlobal.ahkdll;

//            string Command = "WinShow, ahk_id " + hWnd.ToString() + "," + WinText + "," + ExcludeTitle + "," + ExcludeText;
//            ahkdll.ExecRaw(Command);
//        }

//        public void WinHide(object WinTitle = null, string WinText = "", string ExcludeTitle = "", string ExcludeText = "")  // hides window (if currently visible)
//        {
//            IntPtr hWnd = _WinHwnd(WinTitle);  //returns Window Handle (from either handle or window title)
//            if (sharpAHK._AHK.ahkGlobal.ahkdll == null) { sharpAHK._AHK.ahkGlobal.ahkdll = new AutoHotkey.Interop.AutoHotkeyEngine(); }
//            var ahkdll = sharpAHK._AHK.ahkGlobal.ahkdll;

//            string Command = "WinHide, ahk_id " + hWnd.ToString() + "," + WinText + "," + ExcludeTitle + "," + ExcludeText;
//            ahkdll.ExecRaw(Command);
//        }


//        public string StatusBarGetText(int Part = 1, object WinTitle = null, string WinText = "", string ExcludeTitle = "", string ExcludeText = "")  // returns text from statusbar control on another application
//        {
//            IntPtr hWnd = _WinHwnd(WinTitle);  //returns Window Handle (from either handle or window title)

//            if (sharpAHK._AHK.ahkGlobal.ahkdll == null) { sharpAHK._AHK.ahkGlobal.ahkdll = new AutoHotkey.Interop.AutoHotkeyEngine(); }
//            var ahkdll = sharpAHK._AHK.ahkGlobal.ahkdll;

//            ahkdll.SetVar("OutputText", "");
//            string Command = "StatusBarGetText, OutputText," + Part + ", ahk_id " + hWnd.ToString() + "," + WinText + "," + ExcludeTitle + "," + ExcludeText;
//            ahkdll.ExecRaw(Command);
//            string OutputText = ahkdll.GetVar("OutputText");


//            //   Error Handling
//            ahkGlobal.ErrorLevelMsg = "There Was A Problem Reading StatusBar";
//            ahkGlobal.ErrorLevel = false;
//            string ErrorLevelValue = ahkdll.GetVar("ErrorLevel");
//            if (ErrorLevelValue == "0") { ahkGlobal.ErrorLevelMsg = ""; }
//            if (ErrorLevelValue == "1") { ahkGlobal.ErrorLevel = true; }
//            if (ahkGlobal.Debug) { if (ahkGlobal.ErrorLevel) { ahk.MsgBox(ahkGlobal.ErrorLevelMsg, "ErrorLevel"); } } //Display Error Messages in Debug Mode


//            return OutputText;
//        }

//        public bool _StatusBarWait(string BarText = "", int Seconds = 5, int Part = 1, object WinTitle = null, string WinText = "", int Interval = 50, string ExcludeTitle = "", string ExcludeText = "")  // waits until statusbar text contains string before continuing
//        {
//            IntPtr hWnd = _WinHwnd(WinTitle);  //returns Window Handle (from either handle or window title)
//            if (sharpAHK._AHK.ahkGlobal.ahkdll == null) { sharpAHK._AHK.ahkGlobal.ahkdll = new AutoHotkey.Interop.AutoHotkeyEngine(); }
//            var ahkdll = sharpAHK._AHK.ahkGlobal.ahkdll;

//            string Command = "StatusBarWait," + BarText + "," + Seconds + "," + Part + ", ahk_id " + hWnd.ToString() + "," + WinText + "," + Interval + "," + ExcludeTitle + "," + ExcludeText;
//            ahkdll.ExecRaw(Command);

//            //   Error Handling
//            ahkGlobal.ErrorLevel = false;
//            string ErrorLevelValue = ahkdll.GetVar("ErrorLevel");
//            if (ErrorLevelValue == "0")
//            { ahkGlobal.ErrorLevelMsg = ""; ahkGlobal.ErrorLevel = false; }
//            if (ErrorLevelValue == "1")
//            { ahkGlobal.ErrorLevelMsg = "StatusBarWait Timed Out"; ahkGlobal.ErrorLevel = true; }
//            if (ErrorLevelValue == "2")
//            { ahkGlobal.ErrorLevelMsg = "StatusBarWait Could Not Access StatusBar"; ahkGlobal.ErrorLevel = true; }

//            if (ahkGlobal.Debug) { if (ahkGlobal.ErrorLevel) { ahk.MsgBox(ahkGlobal.ErrorLevelMsg, "ErrorLevel"); } } //Display Error Messages in Debug Mode


//            bool Success = false;
//            if (!ahkGlobal.ErrorLevel) { Success = true; }

//            return Success;
//        }


//        #endregion

//        #region === User32 DLL Calls ===


//        //       User32.dll (used in AHK Functions)

//        [DllImport("User32")]
//        private static extern int _ShowWindow(int hwnd, int nCmdShow);  // user32.dll call

//        private const int SW_HIDE = 0;  // user32.dll var
//        private const int SW_SHOW = 5;   // user32.dll var

//        [DllImport("user32.dll")]
//        private static extern bool _SetForegroundWindow(IntPtr hWnd);  // user32.dll call -  win activate

//        [DllImport("user32.dll")]
//        private static extern bool _ShowWindowAsync(IntPtr hWnd, int nCmdShow);  // user32.dll call - win restore

//        [DllImport("user32.dll")]
//        static extern IntPtr _GetForegroundWindow();  // user32.dll call - Active Window Title

//        [DllImport("user32.dll")]
//        static extern int _GetWindowText(IntPtr hWnd, StringBuilder text, int count);  // user32.dll call - Active Window Title

//        [DllImport("user32.dll")]
//        [return: MarshalAs(UnmanagedType.Bool)]
//        static extern bool _GetWindowPlacement(IntPtr hWnd, ref WINDOWPLACEMENT lpwndpl);  // user32.dll call - is WinMaximized / WinMinimized

//        private struct WINDOWPLACEMENT
//        {
//            public int length;
//            public int flags;
//            public int showCmd;
//            public System.Drawing.Point ptMinPosition;
//            public System.Drawing.Point ptMaxPosition;
//            public System.Drawing.Rectangle rcNormalPosition;
//        }

//        //=== Set Window Always On Top ============
//        private static readonly IntPtr HWND_TOPMOST = new IntPtr(-1);  // user32.dll call
//        private static readonly IntPtr HWND_NOTOPMOST = new IntPtr(-2);  // user32.dll call
//        private const UInt32 SWP_NOSIZE = 0x0001;  // user32.dll call
//        private const UInt32 SWP_NOMOVE = 0x0002;  // user32.dll call
//        private const UInt32 TOPMOST_FLAGS = SWP_NOMOVE | SWP_NOSIZE;  // user32.dll call

//        static readonly IntPtr HWND_TOP = new IntPtr(0);  // user32.dll call
//        static readonly IntPtr HWND_BOTTOM = new IntPtr(1);  // user32.dll call

//        [DllImport("user32.dll")]
//        [return: MarshalAs(UnmanagedType.Bool)]
//        public static extern bool _SetWindowPos(IntPtr hWnd, IntPtr hWndInsertAfter, int X, int Y, int cx, int cy, uint uFlags);  // user32.dll call

//        [DllImport("user32.dll", EntryPoint = "SetWindowPos")]
//        public static extern IntPtr _SetWindowPos(IntPtr hWnd, int hWndInsertAfter, int x, int Y, int cx, int cy, int wFlags);  // user32.dll call - Win Move


//        [DllImport("user32.dll", CharSet = CharSet.Auto)]
//        static extern IntPtr _SendMessage(IntPtr hWnd, UInt32 Msg, IntPtr wParam, IntPtr lParam);  // user32.dll call

//        const UInt32 WM_CLOSE = 0x0010;  //win close

//        //===== Get Window Position =======
//        [DllImport("user32.dll", CharSet = CharSet.Auto)]
//        public static extern IntPtr _FindWindow(string strClassName, string strWindowName);  // user32.dll call

//        [DllImport("user32.dll")]
//        public static extern bool _GetWindowRect(IntPtr hwnd, ref Rect rectangle);  // user32.dll call

//        [DllImport("user32.dll", CharSet = CharSet.Auto)]
//        private static extern int _GetWindowText(int hWnd, StringBuilder title, int size);  // user32.dll call - Get Window Title

//        private delegate bool EnumWindowsProc(IntPtr hWnd, int lParam);  // user32.dll call

//        [DllImport("USER32.DLL")]
//        private static extern bool _EnumWindows(EnumWindowsProc enumFunc, int lParam);  // user32.dll call

//        [DllImport("USER32.DLL")]
//        private static extern int _GetWindowTextLength(IntPtr hWnd);  // user32.dll call

//        [DllImport("USER32.DLL")]
//        private static extern bool _IsWindowVisible(IntPtr hWnd);  // user32.dll call

//        [DllImport("USER32.DLL")]
//        private static extern IntPtr _GetShellWindow();  // user32.dll call

//        //WARN: Only for "Any CPU":
//        [DllImport("user32.dll", CharSet = CharSet.Auto, SetLastError = true)]
//        private static extern int _GetWindowThreadProcessId(IntPtr handle, out uint processId);  // user32.dll call


//        //=== post message ==== 
//        [DllImport("user32.dll")]
//        [return: MarshalAs(UnmanagedType.Bool)]
//        static extern bool _PostMessage(IntPtr hWnd, uint msg, IntPtr wParam, IntPtr lParam);  // user32.dll call


//        const uint WM_KEYDOWN = 0x0100;
//        const uint WM_KEYUP = 0x0101;
//        const uint WM_CHAR = 0x0102;
//        const int VK_TAB = 0x09;
//        const int VK_ENTER = 0x0D;
//        const int VK_UP = 0x26;
//        const int VK_DOWN = 0x28;
//        const int VK_RIGHT = 0x27;

//        const uint NpadMsg = 0x111;
//        //const IntPtr wParam = 44010;


//        [DllImport("user32.dll")]
//        static extern int _GetFocus();  // user32.dll call

//        [DllImport("user32.dll")]
//        static extern bool _AttachThreadInput(uint idAttach, uint idAttachTo, bool fAttach);  // user32.dll call

//        [DllImport("kernel32.dll")]
//        static extern uint _GetCurrentThreadId();   // user32.dll call

//        [DllImport("user32.dll")]
//        static extern uint _GetWindowThreadProcessId(int hWnd, int ProcessId);  // user32.dll call

//        [DllImport("user32.dll", CharSet = CharSet.Auto, SetLastError = false)]
//        static extern int _SendMessage(int hWnd, int Msg, int wParam, StringBuilder lParam);  // user32.dll call

//        const int WM_SETTEXT = 12;
//        //const int WM_GETTEXT = 13;

//        [DllImport("user32.dll", SetLastError = true, CharSet = CharSet.Auto)]
//        static extern int _GetClassName(IntPtr hWnd, StringBuilder lpClassName, int nMaxCount);  // user32.dll call


//        private const int WM_SYSCOMMAND = 274;
//        private const int SC_MAXIMIZE = 61488;

//        [DllImport("user32.dll", CharSet = CharSet.Auto, SetLastError = true, ExactSpelling = true)]
//        public static extern int _SetParent(IntPtr hWndChild, IntPtr hWndNewParent);  // user32.dll call

//        [DllImport("user32.dll", CharSet = CharSet.Auto, SetLastError = true, ExactSpelling = true)]
//        public static extern int _SendMessage(IntPtr hWnd, int Msg, int wParam, int lParam);  // user32.dll call


//        //======= Mouse Click ======================
//        [System.Runtime.InteropServices.DllImport("user32.dll")]
//        static extern bool _SetCursorPos(int x, int y);  // user32.dll call

//        [System.Runtime.InteropServices.DllImport("user32.dll")]
//        public static extern void mouse_event(int dwFlags, int dx, int dy, int cButtons, int dwExtraInfo);  // user32.dll call

//        private const int MOUSEEVENTF_MOVE = 0x0001; /* mouse move */
//        private const int MOUSEEVENTF_RIGHTDOWN = 0x0008; /* right button down */
//        private const int MOUSEEVENTF_RIGHTUP = 0x0010; /* right button up */
//        public const int MOUSEEVENTF_LEFTDOWN = 0x02;
//        public const int MOUSEEVENTF_LEFTUP = 0x04;


//        const int WM_GETTEXT = 0x000D;
//        const int WM_GETTEXTLENGTH = 0x000E;

//        [System.Runtime.InteropServices.DllImport("user32.dll", SetLastError = true, CharSet = System.Runtime.InteropServices.CharSet.Auto)]
//        public static extern int _RegisterWindowMessage(string lpString);  // user32.dll call

//        [System.Runtime.InteropServices.DllImport("user32.dll", EntryPoint = "SendMessage", CharSet = System.Runtime.InteropServices.CharSet.Auto)] //
//        public static extern bool _SendMessage(IntPtr hWnd, uint Msg, int wParam, StringBuilder lParam);  // user32.dll call

//        [System.Runtime.InteropServices.DllImport("user32.dll", SetLastError = true)]
//        public static extern IntPtr _SendMessage(int hWnd, int Msg, int wparam, int lparam);  // user32.dll call

//        [DllImport("User32.dll")]
//        public static extern Boolean _EnumChildWindows(int hWndParent, Delegate lpEnumFunc, int lParam);  // user32.dll call

//        public void _PostMsg(IntPtr hWnd, uint msg, IntPtr wParam, IntPtr lParam)  // send postmessage to another application/form
//        {
//            _PostMessage(hWnd, msg, wParam, lParam);
//        }


//        #endregion





//    }


//    public class AHKOLD
//    {
//        public sharpAHK.winInfo WinGetPos(object WinTitle)  // returns a wInfo object with window position populated
//        {
//            //// based on the window handle/window title, return a rectangle object populated with top/left/w/h
//            //AHK.AHK.wInfo WinCoords = ahk.WinGetPos(Window);
//            //ahk.MsgBox("X = " + WinCoords.WinX + Environment.NewLine + "Y = " + WinCoords.WinY + Environment.NewLine + "W = " + WinCoords.WinWidth + Environment.NewLine + "H = " + WinCoords.WinHeight);

//            winInfo info = new winInfo(); // declare the object to return populated
//            Rect ReturnRect = new Rect(); // declare the object to return populated
//            IntPtr hWnd = WinHwnd(WinTitle);  //returns Window Handle (from either handle or window title)

//            GetWindowRect(hWnd, ref ReturnRect);   //return the window dimensions

//            // set the values / calculate width/height
//            int x = ReturnRect.Left;
//            int y = ReturnRect.Top;

//            int Width = ReturnRect.Right - ReturnRect.Left;
//            int Height = ReturnRect.Bottom - ReturnRect.Top;


//            info.WinX = ReturnRect.Left;
//            info.WinY = ReturnRect.Top;
//            info.WinW = Width;
//            info.WinH = Height;


//            //MessageBox.Show("Width = " + Width.ToString() + Environment.NewLine + "Height = " + Height.ToString() + Environment.NewLine + "X = " + x.ToString() + "   Y = " + y.ToString());
//            return info;
//        }

//        [DllImport("user32.dll")]
//        public static extern bool GetWindowRect(IntPtr hwnd, ref sharpAHK._AHK.Rect rectangle);  // user32.dll call

//        [DllImport("user32.dll", SetLastError = true, CharSet = CharSet.Auto)]
//        static extern int GetClassName(IntPtr hWnd, StringBuilder lpClassName, int nMaxCount);  // user32.dll call

//        //===== Get Window Handle =======

//        /// <summary> returns Window Handle (from either handle or window title)</summary>
//        /// <param name="object WinTitle"> </param>
//        public IntPtr WinHwnd(object WinTitle)
//        {
//            string VarType = WinTitle.GetType().ToString();  //determine what kind of variable was passed into function

//            IntPtr hWnd = new IntPtr(0); // declare IntPtr (assign tmp value)

//            // user passed in the Window Title as string
//            if (VarType == "System.String")
//            {
//                hWnd = WinGetHwnd(WinTitle.ToString());  //grab window title handle
//            }

//            // user passed in the handle, activate window
//            if (VarType == "System.IntPtr")
//            {
//                if (!WinTitle.Equals(IntPtr.Zero))
//                {
//                    hWnd = (IntPtr)WinTitle; //declare the object as an IntPtr variable
//                }
//            }

//            return hWnd;
//        }

//        /// <summary> returns window handle, default "A" returns active window handle</summary>
//        /// <param name="WindowTitle"> </param>
//        public IntPtr WinGetHwnd(string WindowTitle = "A")
//        {
//            IntPtr hWnd = IntPtr.Zero;

//            // c# version - works
//            foreach (Process pList in Process.GetProcesses())
//            {
//                if (pList.MainWindowTitle.Contains(WindowTitle))
//                {
//                    hWnd = pList.MainWindowHandle;
//                }
//            }
//            return hWnd; //Should contain the handle but may be zero if the title doesn't match


//            //// grabs ahk id
//            //if (sharpAHK._AHK.ahkGlobal.ahkdll == null) { sharpAHK._AHK.ahkGlobal.ahkdll = new AutoHotkey.Interop.AutoHotkeyEngineA(); }
//            //var ahkdll = sharpAHK._AHK.ahkGlobal.ahkdll;

//            //string WinTitle = "Untitled - Notepad";
//            //string Command = "WinGet, active_id, ID, " + WinTitle;
//            ////string Command = "WinID := WinGetID(\"" + WindowTitle + "\")"; 
//            //ahkdll.ExecRaw(Command);
//            //string OutPutVar = ahkdll.GetVar("active_id");
//            //MsgBox(OutPutVar);

//            //return hWnd;
//            ////hWnd = ToIntPtr(OutPutVar); 

//            //return hWnd; 
//        }

//        /// <summary> returns the handle associated with a process name</summary>
//        /// <param name="ProcessName"> </param>
//        public IntPtr hWndFromProcessName(string ProcessName)
//        {
//            IntPtr hWnd = IntPtr.Zero;
//            Process[] processes = Process.GetProcessesByName(ProcessName);
//            try
//            {
//                Process lol = processes[0];
//                hWnd = lol.MainWindowHandle;
//            }
//            catch { }
//            //int handle = hWnd.ToInt32();
//            return hWnd; //Should contain the handle but may be zero if the title doesn't match
//        }

//        /// <summary> returns Class Name from Handle</summary>
//        /// <param name="object WinTitle"> </param>
//        public string WinGetClass(object WinTitle)
//        {
//            IntPtr hWnd = WinHwnd(WinTitle);  //returns Window Handle (from either handle or window title)

//            StringBuilder ClassName = new StringBuilder(256);
//            var nRet = GetClassName(hWnd, ClassName, ClassName.Capacity);
//            return ClassName.ToString();
//        }

//        /// <summary> populate wInfo object from the window title</summary>
//        /// <param name="object WinTitle"> </param>
//        public winInfo Return_wInfo(object WinTitle)  // populate wInfo object from the window title
//        {
//            //public wInfo MouseGetPos()  // gets the current mouse position, returns wInfo object populated (includes control info/handles)

//            if (sharpAHK._AHK.ahkGlobal.ahkdll == null) { sharpAHK._AHK.ahkGlobal.ahkdll = new AutoHotkey.Interop.AutoHotkeyEngine(); }
//            var ahkdll = sharpAHK._AHK.ahkGlobal.ahkdll;

//            winInfo info = new winInfo(); // declare the object to return populated

//            //ahkdll.SetVar("OutputVar", "");
//            string Command = "MouseGetPos, MouseX, MouseY, WinID, ControlID, 3";
//            ahkdll.ExecRaw(Command);

//            mousePos mouse = new mousePos();
//            mouse.X_Screen = Int32.Parse(ahkdll.GetVar("MouseX"));
//            mouse.Y_Screen = Int32.Parse(ahkdll.GetVar("MouseY"));

//            //// convert string to IntPtr
//            //string winHwnd = ahkdll.GetVar("WinID");
//            //if (winHwnd != "")
//            //{
//            //    int WinID = Int32.Parse(winHwnd);  // string to int
//            //    info.WinHwnd = ToIntPtr(WinID);  // int to IntPtr
//            //}

//            //// convert string to IntPtr
//            //string ControlHwnd = ahkdll.GetVar("ControlID");
//            //if (ControlHwnd != "")
//            //{
//            //    int cID = Int32.Parse(ControlHwnd);  // string to int
//            //    info.ControlHwnd = ToIntPtr(cID);  // int to IntPtr
//            //}

//            info.Hwnd = WinHwnd(WinTitle).ToString();

//            info.Class = WinGetClass(WinTitle);   // win class


//            //// based on the window handle/window title, return a rectangle object populated with top/left/w/h
//            //AHK.AHK.wInfo WinCoords = ahk.WinGetPos(Window);
//            //ahk.MsgBox("X = " + WinCoords.WinX + Environment.NewLine + "Y = " + WinCoords.WinY + Environment.NewLine + "W = " + WinCoords.WinWidth + Environment.NewLine + "H = " + WinCoords.WinHeight);

//            Rect ReturnRect = new Rect(); // declare the object to return populated
//            IntPtr hWnd = WinGetHwnd(WinTitle.ToString());  //returns Window Handle (from either handle or window title)

//            GetWindowRect(hWnd, ref ReturnRect);   //return the window dimensions

//            // set the values / calculate width/height
//            int x = ReturnRect.Left;
//            int y = ReturnRect.Top;

//            int Width = ReturnRect.Right - ReturnRect.Left;
//            int Height = ReturnRect.Bottom - ReturnRect.Top;


//            info.WinX = ReturnRect.Left;
//            info.WinY = ReturnRect.Top;
//            info.WinW = Width;
//            info.WinH = Height;


//            return info;
//        }

//    }


//}