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

        // === DATAGRIDVIEW ===



        #region == New Grid Functions 8/24/17 ===

        /// <summary>
        /// Adds New Text Column To DataGridView (from any thread)
        /// </summary>
        /// <param name="dv">DataGridView to Add Column To</param>
        /// <param name="ColName">New Column Name</param>
        /// <param name="HeaderText">New Column Header Text</param>
        public static void AddColumn(this DataGridView dv, string ColName, string HeaderText = "")
        {
            if (dv == null) { return; }  // Avoid Attempting to Add to DataGridView Control That Hasn't Been Assigned

            if (HeaderText == "") { HeaderText = ColName; }
            // Add row to DataGridView (from any thread)
            if (dv.InvokeRequired) { dv.BeginInvoke((MethodInvoker)delegate () { dv.Columns.Add(ColName, HeaderText); }); }
            else { dv.Columns.Add(ColName, HeaderText); }
        }

        public static void RefreshGrid(this DataGridView dv)
        {
            if (dv == null) { return; }  // Avoid Attempting to Add to DataGridView Control That Hasn't Been Assigned

            // Add row to DataGridView (from any thread)
            if (dv.InvokeRequired) { dv.BeginInvoke((MethodInvoker)delegate () { dv.Refresh(); }); }
            else { dv.Refresh(); }
        }


        /// <summary>
        /// Add New Columns to DataGridView - Checkbox, ComboDDL, Textbox, Link, Image
        /// </summary>
        /// <param name="dv">DataGridView to Populate</param>
        /// <param name="cbColName">Checkbox Column Name</param>
        /// <param name="comboDDL">ComboDDL Column Name</param>
        /// <param name="textColumn">Text Column Name</param>
        /// <param name="linkColumn">Link Column Name</param>
        /// <param name="imageColumn">Image Column Name</param>
        public static void MixedColumnGrid(this DataGridView dv, string cbColName = "Flag", string comboDDL = "DropDownDisplay", string textColumn = "TextColumn", string linkColumn = "LinkColumn", string imageColumn = "ImageColumn")
        {
            if (dv == null) { return; }  // Avoid Attempting to Add to DataGridView Control That Hasn't Been Assigned


            // Add New Column to GridView - CheckBox Column
            // string cbColName = "";
            DataGridViewCheckBoxColumn checkCol = new DataGridViewCheckBoxColumn();
            checkCol.Name = cbColName;  // set column name
            dv.Columns.Add(checkCol);  // assign to grid

            // Add New Column to GridView - ComboBox Column
            // string comboDDL = "";
            DataGridViewComboBoxColumn comboCol = new DataGridViewComboBoxColumn();
            comboCol.Name = comboDDL;  // set column name
            dv.Columns.Add(comboCol);  // assign to grid

            // Add New Column to GridView - TextBox Column
            // string textColumn = "";
            DataGridViewTextBoxColumn textCol = new DataGridViewTextBoxColumn();
            textCol.Name = textColumn; // set column name
            dv.Columns.Add(textCol);  // assign to grid

            // Add New Column to GridView - Link Column
            // string linkColumn = "";
            DataGridViewLinkColumn linkCol = new DataGridViewLinkColumn();
            linkCol.Name = linkColumn;  // set column name
            dv.Columns.Add(linkCol);  // assign to grid

            // Add New Column to GridView - Image Column
            // string imageColumn = "";
            DataGridViewImageColumn imgcol = new DataGridViewImageColumn();//Create image column
            imgcol.Name = imageColumn; //set  column name
            dv.Columns.Add(imgcol); // add column to Datagridview

        }



        #endregion


        #region === DataGridView: Populate ===
        /*
                /// <summary>Populate DataGrid with SQL Search Results (returns Row Count) (option to add column with checkboxes to search result grid)</summary>
                /// <param name="dv"> </param>
                /// <param name="conn"> </param>
                /// <param name="SQLQuery"> </param>
                /// <param name="AddCheckBoxColumn"> </param>
                public static int SQL(this DataGridView dv, SqlConnection conn, string SQLQuery, bool AddCheckBoxColumn = false)
                {
                    if (dv == null) { return; }

                    sql.Populate_DataGrid(dv, conn, SQLQuery, true, AddCheckBoxColumn);
                    int GridRowCount = dv.RowCount;
                    return GridRowCount;
                }

                /// <summary>Populate DataGridView with SQLite Search Results (returns Row Count) (option to add column with checkboxes to search result grid)</summary>
                /// <param name="dv"> </param>
                /// <param name="DbFile"> </param>
                /// <param name="SQLiteCommand"> </param>
                /// <param name="AddCheckBoxColumn"> </param>
                /// <param name="CheckBoxColText"> </param>
                public int SQLite(DataGridView dv, string DbFile, string SQLiteCommand, bool AddCheckBoxColumn = false, string CheckBoxColText = "Selected")
                {
                    if (dv == null) { dv = gridSetup.currentGrid; }
                    if (dv != null) { gridSetup.currentGrid = dv; }

                    SQLiteConnection db = sqlite.Connect(DbFile); // connect to SQLite DB file path - returns connection data

                    try
                    {
                        DataTable dt = new DataTable();
                        var da = new SQLiteDataAdapter(SQLiteCommand, db);  // search SQLite DB
                        da.Fill(dt);

                        if (AddCheckBoxColumn)  // option to add additional column with check boxes to sqlite search results being displayed
                        {
                            dt.Columns.Add(new DataColumn(CheckBoxColText, typeof(bool))); //this will show new checkbox column in grid
                        }


                        // assign the DataGridView Name to Populate
                        //dv.DataSource = dt; 

                        PopulateGrid(dv, dt); // populate grid from any thread
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show("SQLite Exception Catch Here:\n\r" + ex.ToString());
                    }


                    if (AddCheckBoxColumn)
                    {
                        Change_Column_Position(dv, CheckBoxColText, 0);  // move this column to the first position in the column
                    }

                    //=== update gui with grid row count ====================================

                    int GridRowCount = 0;

                    if (dv.InvokeRequired)  // if currently on a different thread, invoke datagrid first
                    {
                        dv.BeginInvoke((MethodInvoker)delegate () { GridRowCount = dv.RowCount; });
                        dv.BeginInvoke((MethodInvoker)delegate () { dv.AutoResizeColumns(); });
                        dv.BeginInvoke((MethodInvoker)delegate () { dv.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.AllCells; });

                    }
                    else
                    {
                        GridRowCount = dv.RowCount;
                        dv.AutoResizeColumns();
                        dv.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.AllCells;
                    }


                    //GridRowCount = GridRowCount - 1;  // adjust for additional row counted
                    //lblGridCount.Text = GridRowCount.ToString() + " Shows"; //update gui with Row Count in Grid


                    //ColumnView(1);  // Set the GridView Column Layout (all fields fields)
                    //ColumnView(2);  // Set the GridView Column Layout (specific fields)

                    // Resize the master DataGridView columns to fit the newly loaded data.



                    // Configure the details DataGridView so that its columns automatically 
                    // adjust their widths when the data changes.


                    //_Database.SQLite lite = new _Database.SQLite();
                    sqlite.Disconnect(db);  // free up db for other use

                    return GridRowCount;
                }

                /// <summary>Search Directory, Populate DataGrid with Files</summary>
                /// <param name="dv"> </param>
                /// <param name="SearchDir"> </param>
                /// <param name="Recurse"> </param>
                /// <param name="SearchPattern"> </param>
                public void FileList(DataGridView dv, string SearchDir, bool Recurse = false, string SearchPattern = "*.*")
                {
                    if (dv == null) { dv = gridSetup.currentGrid; }
                    if (dv != null) { gridSetup.currentGrid = dv; }

                    // clear out existing datagridview values
                    dv.DataSource = null;
                    dv.Rows.Clear();
                    dv.Columns.Clear();


                    //============================================
                    // Add ComboBox to DataGridView (WORKS)
                    //============================================

                    dv.AutoGenerateColumns = false;

                    DataTable dt = new DataTable();
                    dt.Columns.Add("FullPath", typeof(String));
                    dt.Columns.Add("FileName", typeof(String));
                    dt.Columns.Add("DirName", typeof(String));
                    dt.Columns.Add("FileNameNoExt", typeof(String));
                    dt.Columns.Add("Ext", typeof(String));
                    dt.Columns.Add("FileSize", typeof(String));
                    dt.Columns.Add("DateModified", typeof(String));
                    dt.Columns.Add("Options", typeof(String));

                    string[] filelist = null;

                    if (!Recurse) { filelist = Directory.GetFiles(SearchDir, SearchPattern, SearchOption.TopDirectoryOnly); }  // no recurse
                    if (Recurse) { filelist = Directory.GetFiles(SearchDir, SearchPattern, SearchOption.AllDirectories); } // recurse



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

                        dt.Rows.Add(new object[] { FullPath, FileName, DirName, FileNameNoExt, Ext, length, DateModified, "OPTIONS" });

                        //FileTable.Rows.Add(FullPath, FileName, DirName, FileNameNoExt, Ext, length, DateModified);
                    }


                    DataGridViewComboBoxColumn OptionsCol = new DataGridViewComboBoxColumn();
                    var list11 = new List<string>() { "OPTIONS", "Add To List Item", "Open File", "Open Dir", "Rename File", "Delete File" };
                    OptionsCol.DataSource = list11;
                    OptionsCol.HeaderText = "Options";
                    OptionsCol.DataPropertyName = "Options";

                    DataGridViewTextBoxColumn FullPathCol = new DataGridViewTextBoxColumn();
                    FullPathCol.HeaderText = "FullPath";
                    FullPathCol.DataPropertyName = "FullPath";

                    DataGridViewTextBoxColumn FileNameCol = new DataGridViewTextBoxColumn();
                    FileNameCol.HeaderText = "FileName";
                    FileNameCol.DataPropertyName = "FileName";

                    DataGridViewTextBoxColumn DirNameCol = new DataGridViewTextBoxColumn();
                    DirNameCol.HeaderText = "DirName";
                    DirNameCol.DataPropertyName = "DirName";

                    DataGridViewTextBoxColumn FileNameNoExtCol = new DataGridViewTextBoxColumn();
                    FileNameNoExtCol.HeaderText = "FileNameNoExt";
                    FileNameNoExtCol.DataPropertyName = "FileNameNoExt";

                    DataGridViewTextBoxColumn ExtCol = new DataGridViewTextBoxColumn();
                    ExtCol.HeaderText = "Ext";
                    ExtCol.DataPropertyName = "Ext";

                    DataGridViewTextBoxColumn FileSizeCol = new DataGridViewTextBoxColumn();
                    FileSizeCol.HeaderText = "FileSize";
                    FileSizeCol.DataPropertyName = "FileSize";

                    DataGridViewTextBoxColumn DateModifiedCol = new DataGridViewTextBoxColumn();
                    DateModifiedCol.HeaderText = "DateModified";
                    DateModifiedCol.DataPropertyName = "DateModified";

                    dv.DataSource = dt;
                    dv.Columns.AddRange(FullPathCol, FileNameCol, OptionsCol, DirNameCol, FileNameNoExtCol, ExtCol, FileSizeCol, DateModifiedCol);


                    //// format the datagrid results

                    //this.dv.Columns[0].Visible = false;  //FullPath
                    //this.dv.Columns[1].Visible = true;  //FileName
                    //this.dv.Columns[2].Visible = false;  //Options
                    //this.dv.Columns[3].Visible = false;  //DirName
                    //this.dv.Columns[4].Visible = false;  //FileNameNoExt
                    //this.dv.Columns[5].Visible = false;  //Ext
                    //this.dv.Columns[6].Visible = false;  //FileSize
                    //this.dv.Columns[7].Visible = false;  //DateModified

                    //this.dv.Columns[0].AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells;
                    //this.dv.Columns[1].AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells;
                    //this.dv.Columns[2].AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells;
                    //this.dv.Columns[3].AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells;
                    //this.dv.Columns[4].AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells;
                    //this.dv.Columns[5].AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells;
                    //this.dv.Columns[6].AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells;
                    //this.dv.Columns[7].AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells;

                    // Show DataGrid               
                    dv.Visible = true;
                }

                /// <summary>Convert text string to list, then display in DataGridView</summary>
                /// <param name="dv"> </param>
                /// <param name="Text"> </param>
                /// <param name="ColumnText"> </param>
                /// <param name="SkipBlankLines"> </param>
                /// <param name="Trim"> </param>
                /// <param name="SkipComments"> </param>
                public void Text_To_Grid(DataGridView dv, string Text, string ColumnText = "Text_Column", bool SkipBlankLines = true, bool Trim = true, bool SkipComments = false)
                {
                    if (dv == null) { dv = gridSetup.currentGrid; }
                    if (dv != null) { gridSetup.currentGrid = dv; }

                    List<string> TextList = lst.Text_To_List(Text, SkipBlankLines, Trim, SkipComments);
                    lst.List_To_Grid(dv, TextList, ColumnText);
                }

                /// <summary>Convert text file to list, then display in gridview</summary>
                /// <param name="dv"> </param>
                /// <param name="FilePath"> </param>
                /// <param name="ColumnText"> </param>
                /// <param name="SkipBlankLines"> </param>
                /// <param name="Trim"> </param>
                /// <param name="SkipComments"> </param>
                public void TextFile_To_Grid(DataGridView dv, string FilePath, string ColumnText = "Text_Column", bool SkipBlankLines = true, bool Trim = true, bool SkipComments = false)
                {
                    if (dv == null) { dv = gridSetup.currentGrid; }
                    if (dv != null) { gridSetup.currentGrid = dv; }

                    string Text = ahk.FileRead(FilePath);
                    List<string> TextList = lst.Text_To_List(Text, SkipBlankLines, Trim, SkipComments);
                    lst.List_To_Grid(dv, TextList, ColumnText);
                }

                /// <summary>Display List<string> in DataGridView</summary>
                /// <param name="dv"> </param>
                /// <param name="list"> </param>
                /// <param name="ColumnText"> </param>
                public void List_To_Grid(DataGridView dv, List<string> list, string ColumnText = "List_Values")
                {
                    if (dv == null) { dv = gridSetup.currentGrid; }
                    if (dv != null) { gridSetup.currentGrid = dv; }

                    lst.List_To_Grid(dv, list, ColumnText);
                }

                /// <summary>Display List<int> in DataGridView</summary>
                /// <param name="dv"> </param>
                /// <param name="list"> </param>
                /// <param name="ColumnText"> </param>
                public void ListInt_To_Grid(DataGridView dv, List<int> list, string ColumnText = "List_Values")
                {
                    if (dv == null) { dv = gridSetup.currentGrid; }
                    if (dv != null) { gridSetup.currentGrid = dv; }

                    lst.List_To_GridInt(dv, list, ColumnText);
                }

                /// <summary>Populate DataGridView from Dictionary <string, string></summary>
                /// <param name="dv"> </param>
                /// <param name="Dict"> </param>
                /// <param name="KeyField"> </param>
                /// <param name="ValueField"> </param>
                public void Dictionary_To_Grid(DataGridView dv, Dictionary<string, string> Dict, string KeyField = "Key", string ValueField = "Value")
                {
                    if (dv == null) { dv = gridSetup.currentGrid; }
                    if (dv != null) { gridSetup.currentGrid = dv; }

                    DataTable dt = dict.Dict_to_DataTable(Dict, KeyField, ValueField);
                    PopulateGrid(dv, dt);
                }

                /// <summary>Populate DataGridView from Dictionary <int, string></summary>
                /// <param name="dv"> </param>
                /// <param name="Dict"> </param>
                /// <param name="KeyField"> </param>
                /// <param name="ValueField"> </param>
                public void DictionaryInt_To_Grid(DataGridView dv, Dictionary<int, string> Dict, string KeyField = "Key", string ValueField = "Value")
                {
                    if (dv == null) { dv = gridSetup.currentGrid; }
                    if (dv != null) { gridSetup.currentGrid = dv; }

                    DataTable dt = dict.DictInt_to_DataTable(Dict, KeyField, ValueField);
                    PopulateGrid(dv, dt);
                }

                /// <summary>Update DataGridView from DataTable on Any/Current Thread</summary>
                /// <param name="dv"> </param>
                /// <param name="dt"> </param>
                public void PopulateGrid(DataGridView dv, DataTable dt)
                {
                    if (dv == null) { dv = gridSetup.currentGrid; }
                    if (dv != null) { gridSetup.currentGrid = dv; }

                    if (dv.InvokeRequired)  // if currently on a different thread, invoke 
                    {
                        dv.BeginInvoke((MethodInvoker)delegate () { dv.DataSource = dt; });
                    }
                    else  // otherwise populate the grid
                    {
                        dv.DataSource = dt;
                    }
                }

                /// <summary>Display List of Running Processes in DataGridView</summary>
                /// <param name="dg"> </param>
                public void Process_List(DataGridView dv)
                {
                    if (dv == null) { dv = gridSetup.currentGrid; }
                    if (dv != null) { gridSetup.currentGrid = dv; }

                    // displays list of user's current processes

                    //Clear DataGrid 
                    dv.DataSource = null;
                    dv.Rows.Clear();
                    dv.Columns.Clear();


                    // populate new data table
                    DataTable dt = new DataTable();
                    dt.Columns.Add("Select", typeof(Boolean));
                    dt.Columns.Add("Process Name", typeof(String));
                    dt.Columns.Add("WinTitle", typeof(String));
                    dt.Columns.Add("Handle", typeof(String));
                    dt.Columns.Add("MainWin Handle", typeof(String));
                    dt.Columns.Add("WinX", typeof(String));
                    dt.Columns.Add("WinY", typeof(String));
                    dt.Columns.Add("WinW", typeof(String));
                    dt.Columns.Add("WinH", typeof(String));


                    Process[] processlist = Process.GetProcesses();

                    foreach (Process theprocess in processlist)
                    {
                        if (!String.IsNullOrEmpty(theprocess.MainWindowTitle))
                        {
                            try
                            {
                                sharpAHK.winInfo WinPositions = new sharpAHK.winInfo();
                                //WinPositions = ahk.WinGetPos(theprocess.MainWindowHandle);
                                WinPositions = ahk.WinGetPos("ahk_PID " + theprocess.MainWindowHandle);

                                //if (MinimalView == true)
                                //    dt.Rows.Add(new object[] { false, theprocess.ProcessName, theprocess.MainWindowTitle, WinPositions.WinX, WinPositions.WinY, WinPositions.WinWidth, WinPositions.WinHeight });

                                dt.Rows.Add(new object[] { false, theprocess.ProcessName, theprocess.MainWindowTitle, theprocess.Handle, theprocess.MainWindowHandle, WinPositions.WinX, WinPositions.WinY, WinPositions.WinW, WinPositions.WinH });
                            }
                            catch
                            {
                            }
                        }
                    }

                    dv.DataSource = dt; // load datatable in grid
                    //StatusBar("Loaded Running Process List.");
                }


                public List<string> All_WinTitles_By_ProcessName(string processName = "mpc-hc64") // loop through all processes for process name, return all matching window titles
                {
                    List<string> MPC_WinTitles = new List<string>();

                    Process[] processlist = Process.GetProcesses();

                    foreach (Process process in processlist)
                    {
                        if (!String.IsNullOrEmpty(process.MainWindowTitle))
                        {
                            if (process.ProcessName == processName) { MPC_WinTitles.Add(process.MainWindowTitle); }
                            //Console.WriteLine("Process: {0} ID: {1} Window title: {2}", process.ProcessName, process.Id, process.MainWindowTitle);
                        }
                    }

                    return MPC_WinTitles;
                }

                /// <summary>Display List of System Variables in DataGridView</summary>
                /// <param name="dg"> </param>
                public void System_Folders(DataGridView dv)
                {
                    if (dv == null) { dv = gridSetup.currentGrid; }
                    if (dv != null) { gridSetup.currentGrid = dv; }

                    //Clear DataGrid 
                    dv.DataSource = null;
                    dv.Rows.Clear();
                    dv.Columns.Clear();

                    // populate new data table
                    DataTable dt = new DataTable();
                    dt.Columns.Add("Folder Name", typeof(String));
                    dt.Columns.Add("Folder Path", typeof(String));

                    //dt.Rows.Add(new object[] { "[ Project Folder Paths ]", "" }); // Blank Line
                    //dt.Rows.Add(new object[] { "CodeBin", CodeBin });
                    //dt.Rows.Add(new object[] { "DbDir", DbDir }); // Blank Line
                    //dt.Rows.Add(new object[] { "ICODir", ICODir });
                    //dt.Rows.Add(new object[] { "ImageDir", ImageDir });
                    //dt.Rows.Add(new object[] { "ProjectFileDir", ProjectFileDir });
                    //dt.Rows.Add(new object[] { "TagDir", TagDir });
                    //dt.Rows.Add(new object[] { "WebLinksDir", WebLinksDir });
                    //dt.Rows.Add(new object[] { "", "" }); // Blank Line

                    dt.Rows.Add(new object[] { "[ System Folder Paths ]", "" }); // Blank Line
                    dt.Rows.Add(new object[] { "AdminTools", Environment.GetFolderPath(Environment.SpecialFolder.AdminTools) });
                    dt.Rows.Add(new object[] { "ApplicationData", Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData) });
                    dt.Rows.Add(new object[] { "CDBurning", Environment.GetFolderPath(Environment.SpecialFolder.CDBurning) });
                    dt.Rows.Add(new object[] { "CommonAdminTools", Environment.GetFolderPath(Environment.SpecialFolder.CommonAdminTools) });
                    dt.Rows.Add(new object[] { "CommonDesktopDirectory", Environment.GetFolderPath(Environment.SpecialFolder.CommonDesktopDirectory) });
                    dt.Rows.Add(new object[] { "CommonDocuments", Environment.GetFolderPath(Environment.SpecialFolder.CommonDocuments) });
                    dt.Rows.Add(new object[] { "CommonMusic", Environment.GetFolderPath(Environment.SpecialFolder.CommonMusic) });
                    dt.Rows.Add(new object[] { "CommonPictures", Environment.GetFolderPath(Environment.SpecialFolder.CommonPictures) });
                    dt.Rows.Add(new object[] { "CommonProgramFiles", Environment.GetFolderPath(Environment.SpecialFolder.CommonProgramFiles) });
                    dt.Rows.Add(new object[] { "CommonProgramFilesX86", Environment.GetFolderPath(Environment.SpecialFolder.CommonProgramFilesX86) });
                    dt.Rows.Add(new object[] { "CommonStartMenu", Environment.GetFolderPath(Environment.SpecialFolder.CommonStartMenu) });
                    dt.Rows.Add(new object[] { "CommonStartup", Environment.GetFolderPath(Environment.SpecialFolder.CommonStartup) });
                    dt.Rows.Add(new object[] { "CommonVideos", Environment.GetFolderPath(Environment.SpecialFolder.CommonVideos) });
                    dt.Rows.Add(new object[] { "Cookies", Environment.GetFolderPath(Environment.SpecialFolder.Cookies) });
                    dt.Rows.Add(new object[] { "Desktop", Environment.GetFolderPath(Environment.SpecialFolder.Desktop) });
                    dt.Rows.Add(new object[] { "DesktopDirectory", Environment.GetFolderPath(Environment.SpecialFolder.DesktopDirectory) });
                    dt.Rows.Add(new object[] { "Favorites", Environment.GetFolderPath(Environment.SpecialFolder.Favorites) });
                    dt.Rows.Add(new object[] { "History", Environment.GetFolderPath(Environment.SpecialFolder.History) });
                    dt.Rows.Add(new object[] { "InternetCache", Environment.GetFolderPath(Environment.SpecialFolder.InternetCache) });
                    dt.Rows.Add(new object[] { "LocalApplicationData", Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData) });
                    //dt.Rows.Add(new object[] { "MyComputer", Environment.GetFolderPath(Environment.SpecialFolder.MyComputer) });
                    dt.Rows.Add(new object[] { "MyDocuments", Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments) });
                    dt.Rows.Add(new object[] { "MyMusic", Environment.GetFolderPath(Environment.SpecialFolder.MyMusic) });
                    dt.Rows.Add(new object[] { "MyPictures", Environment.GetFolderPath(Environment.SpecialFolder.MyPictures) });
                    dt.Rows.Add(new object[] { "MyVideos", Environment.GetFolderPath(Environment.SpecialFolder.MyVideos) });
                    dt.Rows.Add(new object[] { "NetworkShortcuts", Environment.GetFolderPath(Environment.SpecialFolder.NetworkShortcuts) });
                    dt.Rows.Add(new object[] { "PrinterShortcuts", Environment.GetFolderPath(Environment.SpecialFolder.PrinterShortcuts) });
                    dt.Rows.Add(new object[] { "ProgramFiles", Environment.GetFolderPath(Environment.SpecialFolder.ProgramFiles) });
                    dt.Rows.Add(new object[] { "ProgramFilesX86", Environment.GetFolderPath(Environment.SpecialFolder.ProgramFilesX86) });
                    dt.Rows.Add(new object[] { "Programs", Environment.GetFolderPath(Environment.SpecialFolder.Programs) });
                    dt.Rows.Add(new object[] { "Recent", Environment.GetFolderPath(Environment.SpecialFolder.Recent) });
                    dt.Rows.Add(new object[] { "SendTo", Environment.GetFolderPath(Environment.SpecialFolder.SendTo) });
                    dt.Rows.Add(new object[] { "StartMenu", Environment.GetFolderPath(Environment.SpecialFolder.StartMenu) });
                    dt.Rows.Add(new object[] { "Startup", Environment.GetFolderPath(Environment.SpecialFolder.Startup) });
                    dt.Rows.Add(new object[] { "System", Environment.GetFolderPath(Environment.SpecialFolder.System) });
                    dt.Rows.Add(new object[] { "SystemX86", Environment.GetFolderPath(Environment.SpecialFolder.SystemX86) });
                    dt.Rows.Add(new object[] { "Templates", Environment.GetFolderPath(Environment.SpecialFolder.Templates) });
                    dt.Rows.Add(new object[] { "UserProfile", Environment.GetFolderPath(Environment.SpecialFolder.UserProfile) });
                    dt.Rows.Add(new object[] { "Windows", Environment.GetFolderPath(Environment.SpecialFolder.Windows) });

                    dv.DataSource = dt; // load datatable in grid
                }

        */
        #endregion


        #region === DataGridView: Columns and Rows ===

        /// <summary>Hide (by Column Number) in DataGridView</summary>
        /// <param name="dv"> </param>
        /// <param name="ColNumber"> </param>
        public static void HideColumn(this DataGridView dv, int ColNumber)
        {
            if (dv == null) { return; }

            // update control text (from any thread) -- [ works in dll ]
            if (dv.InvokeRequired) { dv.BeginInvoke((MethodInvoker)delegate () { try { dv.Columns[ColNumber].Visible = false; } catch { } }); }
            else { try { dv.Columns[ColNumber].Visible = false; } catch { } }
        }

        /// <summary>Show (by Column Number) in DataGridView</summary>
        /// <param name="dv"> </param>
        /// <param name="ColNumber"> </param>
        public static void ShowColumn(this DataGridView dv, int ColNumber)
        {
            if (dv == null) { return; }

            // update control text (from any thread) -- [ works in dll ]
            if (dv.InvokeRequired) { dv.BeginInvoke((MethodInvoker)delegate () { try { dv.Columns[ColNumber].Visible = true; } catch { } }); }
            else { try { dv.Columns[ColNumber].Visible = true; } catch { } }
        }

        /// <summary>Returns Column Count (visible and hidden cols)</summary>
        /// <param name="dv"> </param>
        public static int ColCount(this DataGridView dv)
        {
            if (dv == null) { return -1; }

            int columnCount = 0;

            // access control (from any thread) -- [ works in dll ]
            if (dv.InvokeRequired) { dv.BeginInvoke((MethodInvoker)delegate () { try { columnCount = dv.Rows[0].Cells.Count; } catch { } }); }
            else { try { columnCount = dv.Rows[0].Cells.Count; } catch { } }

            return columnCount;
        }
        /*
                /// <summary>Returns the row count from a datagridview</summary>
                /// <param name="dv"> </param>
                public int RowCount(DataGridView dv)
                {
                    if (dv == null) { return -1; }

                    int GridRowCount = -1;

                    // access control (from any thread) -- [ works in dll ]
                    if (dv.InvokeRequired) { dv.BeginInvoke((MethodInvoker)delegate () { try { GridRowCount = dv.RowCount; } catch { } }); }
                    else { try { GridRowCount = dv.RowCount; } catch { } }


                    return GridRowCount;
                }

                /// <summary>Returns # of Columns to OffSet when Looping (1 if user edit is enabled)</summary>
                /// <param name="dv"> </param>
                public int Row_Count_Adjust(DataGridView dv)
                {
                    if (dv == null) { return -1; }

                    int AdjustCount = 0; // subtract 1 row if the user is allowed to add blank new rows

                    // access control (from any thread) -- [ works in dll ]
                    if (dv.InvokeRequired) { dv.BeginInvoke((MethodInvoker)delegate () { if (dv.AllowUserToAddRows) { AdjustCount = 1; } }); }
                    else { if (dv.AllowUserToAddRows) { AdjustCount = 1; } }

                    return AdjustCount;
                }

                /// <summary>Clear contents of DataGridView</summary>
                /// <param name="dv"> </param>
                public void ClearGrid(DataGridView dv)
                {
                    if (dv == null) { return; }

                    dv.DataSource = null;
                    dv.Rows.Clear();
                    dv.Columns.Clear();
                }

                /// <summary>Rename column header text (using column # position)</summary>
                /// <param name="dv"> </param>
                /// <param name="ColNumber"> </param>
                /// <param name="HeaderText"> </param>
                public void ColName(DataGridView dv, int ColNumber, string HeaderText)
                {
                    if (dv == null) { return; }

                    try { dv.Columns[ColNumber].HeaderText = HeaderText; }
                    catch { }
                }

                /// <summary>Return the column position based on the column name</summary>
                /// <param name="dv"> </param>
                /// <param name="SearchColumn"> </param>
                public int Return_Column_Position(DataGridView dv, string SearchColumn)
                {
                    if (dv == null) { return -1; }

                    int AdjustCount = 0;
                    if (dv.AllowUserToAddRows) { AdjustCount = 1; }  //adjust for user ability to add new lines to grid


                    // check to see if there are any columns to populate list first
                    int colCount = 0;
                    try { colCount = dv.Rows[0].Cells.Count; }
                    catch
                    {
                        return 0;
                    }


                    string cColumnName = "";
                    int columnCount = dv.Rows[0].Cells.Count;
                    for (int i = 0; i < columnCount - AdjustCount; i++)
                    {
                        string ColumnName = dv.Columns[i].HeaderText;

                        if (ColumnName.ToUpper() == SearchColumn.ToUpper())
                        {
                            return i;
                        }
                    }

                    return 0;

                    //int ColCount = 0; 

                    //// checked items in grid
                    //for (int i = 0; i < dv.Rows.Count - AdjustCount; i++)
                    //{
                    //    string rValue = dv.Rows[i].Cells[ColumnName].Value.ToString();

                    //    if (rValue.ToUpper() == ColumnName.ToUpper())
                    //    {
                    //        return ColCount;
                    //    }

                    //    ColCount++; 

                    //}

                    //return ColCount;
                }

                /// <summary>Returns list of column header names from DataGridView</summary>
                /// <param name="dv"> </param>
                public List<string> Column_Names(DataGridView dv)
                {
                    if (dv == null) { dv = gridSetup.currentGrid; }
                    if (dv != null) { gridSetup.currentGrid = dv; }

                    List<string> Grid_Columns = new List<string>();

                    // check to see if there are any columns to populate list first
                    int colCount = 0;
                    try { colCount = dv.Rows[0].Cells.Count; }
                    catch
                    {
                        return Grid_Columns;
                    }


                    string ColumnName = "";
                    int columnCount = dv.Rows[0].Cells.Count;
                    for (int i = 0; i < columnCount; i++)
                    {
                        ColumnName = dv.Columns[i].HeaderText;
                        Grid_Columns.Add(ColumnName);
                        //ahk.MsgBox(ColumnName);
                    }

                    return Grid_Columns;
                }

                /// <summary>Returns true / false if column name found</summary>
                /// <param name="dv"> </param>
                /// <param name="SearchColumn"> </param>
                public bool Column_Names_Search(DataGridView dv, string SearchColumn)
                {
                    if (dv == null) { dv = gridSetup.currentGrid; }
                    if (dv != null) { gridSetup.currentGrid = dv; }

                    List<string> Grid_Columns = new List<string>();

                    // check to see if there are any columns to populate list first
                    int colCount = 0;
                    try { colCount = dv.Rows[0].Cells.Count; }
                    catch
                    {
                        return false;
                    }

                    string ColumnName = "";
                    int columnCount = dv.Rows[0].Cells.Count;
                    for (int i = 0; i < columnCount; i++)
                    {
                        ColumnName = dv.Columns[i].HeaderText;

                        if (ColumnName.ToUpper() == SearchColumn.ToUpper())
                        {
                            return true;
                        }
                    }

                    return false;
                }

                /// <summary>Returns true / false if column name is found & visible</summary>
                /// <param name="dv"> </param>
                /// <param name="SearchColumn"> </param>
                public bool Column_Visible(DataGridView dv, string SearchColumn)
                {
                    if (dv == null) { dv = gridSetup.currentGrid; }
                    if (dv != null) { gridSetup.currentGrid = dv; }

                    List<string> Grid_Columns = new List<string>();


                    // check to see if there are any columns to populate list first
                    int colCount = 0;
                    try { colCount = dv.Rows[0].Cells.Count; }
                    catch
                    {
                        return false;
                    }


                    string ColumnName = "";
                    int columnCount = dv.Rows[0].Cells.Count;
                    for (int i = 0; i < columnCount; i++)
                    {
                        ColumnName = dv.Columns[i].HeaderText;

                        if (ColumnName.ToUpper() == SearchColumn.ToUpper())
                        {
                            bool ColumnVisible = dv.Columns[i].Visible;
                            return ColumnVisible;
                        }
                    }

                    return false;
                }

                /// <summary>if column is found (by SearchColumn name), if visible sets to hidden or hidden from visible</summary>
                /// <param name="dv"> </param>
                /// <param name="SearchColumn"> </param>
                /// <param name="ForceShow"> </param>
                /// <param name="ForceHide"> </param>
                public bool Column_Visibility_Toggle(DataGridView dv, string SearchColumn, bool ForceShow = false, bool ForceHide = false)
                {
                    if (dv == null) { dv = gridSetup.currentGrid; }
                    if (dv != null) { gridSetup.currentGrid = dv; }

                    List<string> Grid_Columns = new List<string>();

                    // check to see if there are any columns to populate list first
                    int colCount = 0;
                    try { colCount = dv.Rows[0].Cells.Count; }
                    catch
                    {
                        return false;
                    }


                    string ColumnName = "";
                    int columnCount = dv.Rows[0].Cells.Count;
                    for (int i = 0; i < columnCount; i++)
                    {
                        ColumnName = dv.Columns[i].HeaderText;

                        if (ColumnName.ToUpper() == SearchColumn.ToUpper())
                        {
                            if (ForceShow) { dv.Columns[i].Visible = true; return true; }   // optional parameter to not toggle, just show if found
                            if (ForceHide) { dv.Columns[i].Visible = false; return true; }  // optional parameter to not toggle, just hide if found

                            bool ColumnVisible = dv.Columns[i].Visible; // check to see if the column is currently visible
                            if (ColumnVisible) { dv.Columns[i].Visible = false; return true; }
                            if (!ColumnVisible) { dv.Columns[i].Visible = true; return true; }

                            return ColumnVisible;
                        }
                    }

                    return false;
                }

                /// <summary>Pass column number or column header text, enable editing on a single column in grid, sets other cols to readonly</summary>
                /// <param name="dv"> </param>
                /// <param name="EditColName"> </param>
                /// <param name="EditableColNum"> </param>
                public void Set_One_Column_Editable(DataGridView dv, string EditColName = "", int EditableColNum = 0)
                {
                    if (dv == null) { dv = gridSetup.currentGrid; }
                    if (dv != null) { gridSetup.currentGrid = dv; }

                    dv.EditMode = System.Windows.Forms.DataGridViewEditMode.EditOnEnter;  // enable editing on entire grid, then set 1 to editable

                    if (EditColName != "") { EditableColNum = Return_Column_Position(dv, EditColName); }  // find the column position # if user passed in the header name

                    foreach (DataGridViewColumn dc in dv.Columns)
                    {
                        if (dc.Index.Equals(EditableColNum))
                        {
                            dc.ReadOnly = false;
                        }
                        else
                        {
                            dc.ReadOnly = true;
                        }
                    }
                }

                /// <summary>Returns list of column types names from DataGridView</summary>
                /// <param name="dv">DataGridView to Search</param>
                /// <param name="TrimReturnValue">Default Option to Trim System.Windows.Forms From Column Type</param>
                public List<string> Column_Types(DataGridView dv, bool TrimReturnValue = true)
                {
                    if (dv == null) { dv = gridSetup.currentGrid; }
                    if (dv != null) { gridSetup.currentGrid = dv; }

                    //System.Windows.Forms.DataGridViewCheckBoxCell
                    //System.Windows.Forms.DataGridViewTextBoxCell

                    List<string> Grid_Column_Types = new List<string>();

                    // check to see if there are any columns to populate list first
                    int colCount = 0;
                    try { colCount = dv.Rows[0].Cells.Count; }
                    catch
                    {
                        return Grid_Column_Types;
                    }


                    string colType = "";
                    int columnCount = dv.Rows[0].Cells.Count;
                    for (int i = 0; i < columnCount; i++)
                    {
                        colType = dv.Columns[i].CellType.ToString();

                        string type = colType;

                        if (TrimReturnValue) { type = colType.Replace("System.Windows.Forms.", ""); }

                        Grid_Column_Types.Add(colType);
                        //ahk.MsgBox(colType);
                    }

                    return Grid_Column_Types;
                }

                /// <summary>
                /// Return DataGridView Column Type by Column Number
                /// </summary>
                /// <param name="dv">DataGridView to Search</param>
                /// <param name="ColNum">Column Number (starting with 0) To Return</param>
                /// <param name="TrimReturnValue">Default Option to Trim System.Windows.Forms From Column Type</param>
                /// <returns>Returns Column Type as String</returns>
                public string Column_Type(DataGridView dv, int ColNum = 0, bool TrimReturnValue = true)
                {
                    if (dv == null) { dv = gridSetup.currentGrid; }
                    if (dv != null) { gridSetup.currentGrid = dv; }

                    // check to see if there are any columns to populate list first
                    int colCount = 0;
                    try { colCount = dv.Rows[0].Cells.Count; }
                    catch
                    {
                        return "";
                    }


                    string colType = "";
                    int columnCount = dv.Rows[0].Cells.Count;
                    for (int i = 0; i < columnCount; i++)
                    {
                        if (i == ColNum)
                        {
                            string type = dv.Columns[i].CellType.ToString();

                            if (TrimReturnValue) { type = type.Replace("System.Windows.Forms.", ""); }

                            return type;
                        }
                    }

                    return "";
                }


                // column checkboxes - detect / return name / return position

                //        ahk.MsgBox("Has CheckBox Column = " + grid.Has_CheckBox_Column(dataGridView1));
                //        ahk.MsgBox("CheckBox Column Name = " + grid.Column_Name_CheckBox(dataGridView1));
                //        ahk.MsgBox("CheckBox Column Position = " + grid.CheckBox_Column_Position(dataGridView1).ToString()); 

                /// <summary>Returns name of Column that is a CheckBox column</summary>
                /// <param name="dv"> </param>
                public string Column_Name_CheckBox(DataGridView dv)
                {
                    if (dv == null) { dv = gridSetup.currentGrid; }
                    if (dv != null) { gridSetup.currentGrid = dv; }

                    List<string> Grid_Column_Types = new List<string>();

                    // check to see if there are any columns to populate list first
                    int colCount = 0;
                    try { colCount = dv.Rows[0].Cells.Count; }
                    catch { return ""; }


                    int columnCount = dv.Rows[0].Cells.Count;
                    for (int i = 0; i < columnCount; i++)
                    {
                        string colType = dv.Columns[i].CellType.ToString();
                        string ColName = dv.Columns[i].HeaderText;
                        bool ColumnVisible = dv.Columns[i].Visible;

                        if (colType == "System.Windows.Forms.DataGridViewCheckBoxCell")
                        {
                            if (ColumnVisible)
                            {
                                //ahk.MsgBox(ColName);
                                return ColName;
                            }
                        }
                    }

                    return "";
                }

                /// <summary>Returns name postion # of the checkbox column in grid</summary>
                /// <param name="dv"> </param>
                public int CheckBox_Column_Position(DataGridView dv)
                {
                    if (dv == null) { dv = gridSetup.currentGrid; }
                    if (dv != null) { gridSetup.currentGrid = dv; }

                    List<string> Grid_Column_Types = new List<string>();

                    // check to see if there are any columns to populate list first
                    int colCount = 0;
                    try { colCount = dv.Rows[0].Cells.Count; }
                    catch { return -1; }


                    int columnCount = dv.Rows[0].Cells.Count;
                    for (int i = 0; i < columnCount; i++)
                    {
                        string colType = dv.Columns[i].CellType.ToString();
                        string ColName = dv.Columns[i].HeaderText;
                        bool ColumnVisible = dv.Columns[i].Visible;

                        if (colType == "System.Windows.Forms.DataGridViewCheckBoxCell")
                        {
                            if (ColumnVisible)
                            {
                                //ahk.MsgBox(ColName);
                                return i;
                            }
                        }
                    }

                    return -1;
                }

                /// <summary>Returns true / false if there is a VISIBLE checkbox column in grid</summary>
                /// <param name="dv"> </param>
                public bool Has_CheckBox_Column(DataGridView dv)
                {
                    if (dv == null) { dv = gridSetup.currentGrid; }
                    if (dv != null) { gridSetup.currentGrid = dv; }

                    List<string> Grid_Column_Types = new List<string>();

                    // check to see if there are any columns to populate list first
                    int colCount = 0;
                    try { colCount = dv.Rows[0].Cells.Count; }
                    catch { return false; }


                    int columnCount = dv.Rows[0].Cells.Count;
                    for (int i = 0; i < columnCount; i++)
                    {
                        string colType = dv.Columns[i].CellType.ToString();
                        string ColName = dv.Columns[i].HeaderText;
                        bool ColumnVisible = dv.Columns[i].Visible;

                        if (colType == "System.Windows.Forms.DataGridViewCheckBoxCell")
                        {
                            if (ColumnVisible)
                            {
                                return true;
                            }
                        }
                    }

                    return false;
                }


                /// <summary>Check all checkboxes in column #</summary>
                /// <param name="dv"> </param>
                /// <param name="ColNumber"> </param>
                public void CheckALL(DataGridView dv, int ColNumber = -1)
                {
                    if (dv == null) { dv = gridSetup.currentGrid; }
                    if (dv != null) { gridSetup.currentGrid = dv; }

                    string CheckColumn = Column_Name_CheckBox(dv);  // name of column with checkboxes

                    foreach (DataGridViewRow dgv in dv.Rows)
                    {
                        if (ColNumber != -1)
                        {
                            dgv.Cells[ColNumber].Value = true;
                        }
                        if (ColNumber == -1)
                        {
                            dgv.Cells[CheckColumn].Value = true;
                        }
                    }
                }

                /// <summary>Uncheck all checkboxes in column #</summary>
                /// <param name="dv"> </param>
                /// <param name="ColNumber"> </param>
                public void UnCheckALL(DataGridView dv, int ColNumber = -1)
                {
                    if (dv == null) { dv = gridSetup.currentGrid; }
                    if (dv != null) { gridSetup.currentGrid = dv; }

                    string CheckColumn = Column_Name_CheckBox(dv);  // name of column with checkboxes

                    foreach (DataGridViewRow dgv in dv.Rows)
                    {
                        if (ColNumber != -1)
                        {
                            dgv.Cells[ColNumber].Value = false;
                        }
                        if (ColNumber == -1)
                        {
                            dgv.Cells[CheckColumn].Value = false;
                        }
                    }
                }

                /// <summary>Autosize all columns in datagrid</summary>
                /// <param name="dv"> </param>
                public void AutoSize_AllColumns(DataGridView dv)
                {
                    if (dv == null) { dv = gridSetup.currentGrid; }
                    if (dv != null) { gridSetup.currentGrid = dv; }

                    int i = 0;
                    do
                    {
                        try
                        {
                            dv.Columns[i].AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells;
                        }
                        catch { }

                        i++;
                    } while (i < dv.ColumnCount);

                }

                /// <summary>Returns list of RowNumbers Checked in DataGridView</summary>
                /// <param name="dv"> </param>
                /// <param name="CheckboxField"> </param>
                public List<int> Return_Checked_RowNumbers(DataGridView dv, int CheckboxField = 0)
                {
                    if (dv == null) { dv = gridSetup.currentGrid; }
                    if (dv != null) { gridSetup.currentGrid = dv; }

                    List<int> CheckedList = new List<int>();

                    Move_Selected_To_SubmitValue(dv);  // move users current position in grid to submit value under selected item

                    int AdjustCount = 0; // subtract 1 row if the user is allowed to add blank new rows
                    if (dv.AllowUserToAddRows) { AdjustCount = 1; }

                    // checked items in grid
                    for (int i = 0; i < dv.Rows.Count - AdjustCount; i++)
                    {
                        string sFlagged = dv.Rows[i].Cells[CheckboxField].Value.ToString();
                        if (sFlagged.ToUpper() == "TRUE")
                        {
                            CheckedList.Add(i);
                        }
                    }

                    return CheckedList;
                }

                /// <summary>Return list of values from column in gridview</summary>
                /// <param name="dv"> </param>
                /// <param name="ColumnName"> </param>
                /// <param name="CheckboxField"> </param>
                public List<string> Return_Checked_Values(DataGridView dv, string ColumnName, int CheckboxField = 0)
                {
                    if (dv == null) { dv = gridSetup.currentGrid; }
                    if (dv != null) { gridSetup.currentGrid = dv; }

                    // EX: 

                    //// Return List of Checked Items in "SSRNumber" Column
                    //List<string> CheckedSSRs = grid.Return_Checked_Values(dataGridView1, "SSRNumber", 0);
                    //foreach (string SSR in CheckedSSRs)
                    //{
                    //    ahk.MsgBox(SSR);
                    //}


                    List<string> CheckedList = new List<string>();

                    //Move_Selected_To_SubmitValue(dv);  // move users current position in grid to submit value under selected item


                    int AdjustCount = 0; // subtract 1 row if the user is allowed to add blank new rows
                    if (dv.AllowUserToAddRows) { AdjustCount = 1; }

                    // checked items in grid
                    for (int i = 0; i < dv.Rows.Count - AdjustCount; i++)
                    {
                        string sFlagged = dv.Rows[i].Cells[CheckboxField].Value.ToString();
                        if (sFlagged.ToUpper() == "TRUE")
                        {
                            string rValue = dv.Rows[i].Cells[ColumnName].Value.ToString();
                            CheckedList.Add(rValue);
                            //ahk.MsgBox("Row " + i + " is Checked" + Environment.NewLine + "Value = " + rValue);
                        }
                    }

                    return CheckedList;
                }

                /// <summary>Return list of values from column in gridview</summary>
                /// <param name="dv"> </param>
                public List<string> Return_Selected_Row_Values(DataGridView dv)
                {
                    if (dv == null) { dv = gridSetup.currentGrid; }
                    if (dv != null) { gridSetup.currentGrid = dv; }

                    // EX: 

                    //// Return List of Checked Items in "SSRNumber" Column
                    //List<string> CheckedSSRs = grid.Return_Checked_Values(dataGridView1, "SSRNumber", 0);
                    //foreach (string SSR in CheckedSSRs)
                    //{
                    //    ahk.MsgBox(SSR);
                    //}


                    List<string> CheckedList = new List<string>();

                    // check to see if there are any columns to populate list first
                    int colCount = 0;
                    try { colCount = dv.Rows[0].Cells.Count; }
                    catch
                    {
                        return CheckedList;
                    }


                    //Move_Selected_To_SubmitValue(dv);  // move users current position in grid to submit value under selected item


                    int AdjustCount = 0; // subtract 1 row if the user is allowed to add blank new rows
                    if (dv.AllowUserToAddRows) { AdjustCount = 1; }

                    // checked items in grid
                    for (int i = 0; i < dv.Rows.Count - AdjustCount; i++)
                    {
                        string sFlagged = dv.Rows[i].Selected.ToString();
                        if (sFlagged.ToUpper() == "TRUE")
                        {
                            int columnCount = dv.Rows[0].Cells.Count;
                            for (int f = 0; f < columnCount; f++)
                            {
                                string rValue = dv.Rows[i].Cells[f].Value.ToString();
                                CheckedList.Add(rValue);
                                //ahk.MsgBox(colType);
                            }

                            //ahk.MsgBox("Row " + i + " is Checked" + Environment.NewLine + "Value = " + rValue);
                        }
                    }

                    return CheckedList;
                }

                /// <summary>Returns list of RowNumbers Selected in DataGridView</summary>
                /// <param name="dv"> </param>
                public List<int> Return_Selected_RowNumbers(DataGridView dv)
                {
                    if (dv == null) { dv = gridSetup.currentGrid; }
                    if (dv != null) { gridSetup.currentGrid = dv; }

                    List<int> SelectedList = new List<int>();

                    int AdjustCount = 0; // subtract 1 row if the user is allowed to add blank new rows
                    if (dv.AllowUserToAddRows) { AdjustCount = 1; }

                    // checked items in grid
                    for (int i = 0; i < dv.Rows.Count - AdjustCount; i++)
                    {
                        string sFlagged = dv.Rows[i].Selected.ToString();
                        if (sFlagged.ToUpper() == "TRUE")
                        {
                            SelectedList.Add(i);
                        }
                    }

                    return SelectedList;
                }

                /// <summary>Change position of column displayed in datagridview</summary>
                /// <param name="dv"> </param>
                /// <param name="ColumnName"> </param>
                /// <param name="ColPosition"> </param>
                public void Change_Column_Position(DataGridView dv, string ColumnName, int ColPosition = 0)
                {
                    if (dv == null) { dv = gridSetup.currentGrid; }
                    if (dv != null) { gridSetup.currentGrid = dv; }

                    dv.Columns[ColumnName].DisplayIndex = ColPosition;
                }


                /// <summary>Move users current position in grid to submit value under selected item</summary>
                /// <param name="dv"> </param>
                public void Move_Selected_To_SubmitValue(DataGridView dv)
                {
                    if (dv == null) { dv = gridSetup.currentGrid; }
                    if (dv != null) { gridSetup.currentGrid = dv; }

                    // action to move out of current cell to allow value change to be recognized
                    try
                    {
                        dv.CurrentCell = dv.Rows[dv.CurrentCell.RowIndex + 1].Cells[dv.CurrentCell.ColumnIndex];
                    }
                    catch
                    {
                        try
                        {
                            dv.CurrentCell = dv.Rows[dv.CurrentCell.RowIndex - 1].Cells[dv.CurrentCell.ColumnIndex];
                        }
                        catch
                        {
                            try
                            {
                                dv.CurrentCell = dv.Rows[dv.CurrentCell.RowIndex].Cells[dv.CurrentCell.ColumnIndex + 1];
                            }
                            catch
                            {
                                try
                                {
                                    dv.CurrentCell = dv.Rows[dv.CurrentCell.RowIndex].Cells[dv.CurrentCell.ColumnIndex - 1];
                                }
                                catch
                                {

                                }

                            }

                        }
                    }
                }



                private void SB(string Text, int section = 1, bool startTimer = false, bool stopTimer = false)
                {
                    sb.StatusBar(Text, section, startTimer, stopTimer);
                }

                //### Save and Restore Grid Column Visibility

                /// <summary>Save the current visibility settings of the datagrid (to SQLite Settings Db File)</summary>
                /// <param name="dv"> </param>
                /// <param name="GridDbFile"> </param>
                /// <param name="TableName"> </param>
                public void Save_Grid_Column_Settings(DataGridView dv, string GridDbFile = "Settings.sqlite", string TableName = "GridDisplay")
                {
                    if (dv == null) { dv = gridSetup.currentGrid; }
                    if (dv != null) { gridSetup.currentGrid = dv; }

                    if (GridDbFile == "Settings.sqlite") { GridDbFile = ahk.AppDir() + "\\Settings.sqlite"; }

                    // check to see if there are any columns to populate list first
                    int colCount = 0;
                    try { colCount = dv.Rows[0].Cells.Count; }
                    catch
                    {
                        ahk.MsgBox("Unable To Save Grid Settings - No Columns Visible.");
                        return;
                    }

                    SB("Saving Grid Values");


                    List<int> RowHeightList = RowHeights_Get(dv, -1);  // height of each row              
                    List<int> ColumnWidthPercentages = Column_Width_Percentages(dv);  // width % of each column
                    string ColumnName = "";
                    bool Visible = false;
                    int columnCount = dv.Rows[0].Cells.Count;
                    for (int i = 0; i < columnCount; i++)
                    {
                        ColumnName = dv.Columns[i].HeaderText;
                        Visible = dv.Columns[i].Visible;

                        sqlite.Setting_Save(ColumnName, Visible.ToString(), TableName, GridDbFile);
                    }
                }


                // scroll down until row is visible, then programatically select 
                public void Select_Row(DataGridView dv, int RowNum)
                {
                    dv.FirstDisplayedScrollingRowIndex = RowNum;
                    dv.Refresh();

                    //Then you must select the row so that binding sources update their Current item:
                    dv.CurrentCell = dv.Rows[RowNum].Cells[0];

                    //Finally, you can visually select the row with C#:
                    dv.Rows[RowNum].Selected = true;
                }






                //int i = 0;
                //foreach (int width in ColumnWidthPercentages)
                //{
                //    sb.StatusBar("Column " + i + " | Width = " + width + "%");
                //    ahk.Sleep(200); 
                //    i++;
                //}


                public bool Setting_Save(string SettingName, string Value, string Option = "", string DbFile = "Settings.sqlite", string TableName = "Settings")
                {
                    // if no database name is provided - default saved to Settings.sqlite in application directory
                    if (DbFile == "Settings.sqlite" || DbFile == "") { DbFile = ahk.AppDir() + "\\Settings.sqlite"; }

                    // Create New Table If It Does NOT Exist Yet
                    //bool TableExist = sqlite.Table_Exists(DbFile, TableName);  //See if selected Table Exists in SQLite DB file
                    //if (!TableExist) 
                    //{ 
                    bool TableExist = Settings_NewDb(DbFile, TableName);
                    //}  // Table DOES NOT exist in SQLite DB - Create Now

                    // UPDATE or INSERT Server Files
                    string UpdateLine = "UPDATE " + TableName + " set Value = '" + Value + "', Option = '" + Option + "',TimeStamp = '" + DateTime.Now.ToString() + "' WHERE Setting = '" + SettingName + "'";
                    if (Option != "") { UpdateLine = "UPDATE " + TableName + " set Value = '" + Value + "', Option = '" + Option + "',TimeStamp = '" + DateTime.Now.ToString() + "' WHERE Setting = '" + SettingName + "' AND Option = '" + Option + "'"; }

                    bool Updated = sqlite.Execute(DbFile, UpdateLine);  // Update Table

                    if (!Updated) { Updated = sqlite.Execute(DbFile, "INSERT into " + TableName + " (Setting, Value, Option, TimeStamp) values ('" + SettingName + "','" + Value + "','" + Option + "','" + DateTime.Now.ToString() + "')"); }  // insert into a Table
                    if (!Updated) { MessageBox.Show("Failed to Insert: " + SettingName + "' | '" + Value + " | " + Option + "' | '" + DateTime.Now.ToString()); }
                    return Updated;
                }

                public bool Table_Exists(string DbFile, string SearchTableName)
                {
                    if (!File.Exists(DbFile)) { return false; } // database file doesn't exist - table not found

                    List<string> TableNames = sqlite.Table_List(DbFile);

                    foreach (string table in TableNames)
                    {
                        if (table.ToUpper().Trim() == SearchTableName.ToUpper().Trim()) { return true; }
                    }

                    return false;
                }

                public bool Settings_NewDb(string DbFile = "Settings.sqlite", string TableName = "Settings", bool OverWriteExisting = false)
                {
                    // if no database name is provided - default saved to Settings.sqlite in application directory
                    if (DbFile == "Settings.sqlite" || DbFile == "") { DbFile = ahk.AppDir() + "\\Settings.sqlite"; }

                    if (OverWriteExisting)  // option to clear out previous Db setttings
                    {
                        sqlite.Table_Clear(DbFile, TableName, true);
                    }

                    // Create New SQLite DB (*Used First-Run*)
                    if (!File.Exists(DbFile)) // create database file if it doen't exist
                    {
                        SQLiteConnection.CreateFile(DbFile);
                    }

                    // Create New Table If It Does NOT Exist Yet
                    bool TableExist = Table_Exists(DbFile, TableName);  //See if selected Table Exists in SQLite DB file

                    if (!TableExist)  // Table DOES NOT exist in SQLite DB
                    {

                        string NewTableLine = "ID INTEGER PRIMARY KEY, Setting VARCHAR, Value VARCHAR, Option VARCHAR, TimeStamp VARCHAR, Enabled VARCHAR";

                        //ahk.MsgBox(NewTableLine); 

                        bool ReturnValue = sqlite.Execute(DbFile, "CREATE TABLE [" + TableName + "] (" + NewTableLine + ")");  // Create a Table [ONLY EXECUTE ONCE! WILL ERROR 2ND TIME]
                    }


                    if (File.Exists(DbFile)) { return true; }
                    return false;
                }

                public void SaveGridSettings(DataGridView dv)
                {
                    if (dv == null) { dv = gridSetup.currentGrid; }
                    if (dv != null) { gridSetup.currentGrid = dv; }

                    string DbSavePath = ahk.AppDir() + "\\Settings.sqlite";

                    if (dv.Width != null) { sqlite.Setting_Save("Width", dv.Width.ToString(), "", DbSavePath, dv.Name); }
                    if (dv.Visible != null) { sqlite.Setting_Save("Visible", dv.Visible.ToString(), "", DbSavePath, dv.Name); }
                    if (dv.VirtualMode != null) { sqlite.Setting_Save("VirtualMode", dv.VirtualMode.ToString(), "", DbSavePath, dv.Name); }
                    if (dv.VerticalScrollingOffset != null) { sqlite.Setting_Save("VerticalScrollingOffset", dv.VerticalScrollingOffset.ToString(), "", DbSavePath, dv.Name); }
                    if (dv.UseWaitCursor != null) { sqlite.Setting_Save("UseWaitCursor", dv.UseWaitCursor.ToString(), "", DbSavePath, dv.Name); }
                    if (dv.UserSetCursor != null) { sqlite.Setting_Save("UserSetCursor", dv.UserSetCursor.ToString(), "", DbSavePath, dv.Name); }
                    if (dv.TopLevelControl != null) { sqlite.Setting_Save("TopLevelControl", dv.TopLevelControl.ToString(), "", DbSavePath, dv.Name); }
                    if (dv.TopLeftHeaderCell != null) { sqlite.Setting_Save("TopLeftHeaderCell", dv.TopLeftHeaderCell.ToString(), "", DbSavePath, dv.Name); }
                    if (dv.Top != null) { sqlite.Setting_Save("Top", dv.Top.ToString(), "", DbSavePath, dv.Name); }
                    if (dv.Tag != null) { sqlite.Setting_Save("Tag", dv.Tag.ToString(), "", DbSavePath, dv.Name); }
                    if (dv.TabStop != null) { sqlite.Setting_Save("TabStop", dv.TabStop.ToString(), "", DbSavePath, dv.Name); }
                    if (dv.TabIndex != null) { sqlite.Setting_Save("TabIndex", dv.TabIndex.ToString(), "", DbSavePath, dv.Name); }
                    if (dv.StandardTab != null) { sqlite.Setting_Save("StandardTab", dv.StandardTab.ToString(), "", DbSavePath, dv.Name); }
                    if (dv.SortOrder != null) { sqlite.Setting_Save("SortOrder", dv.SortOrder.ToString(), "", DbSavePath, dv.Name); }
                    if (dv.SortedColumn != null) { sqlite.Setting_Save("SortedColumn", dv.SortedColumn.ToString(), "", DbSavePath, dv.Name); }
                    if (dv.Site != null) { sqlite.Setting_Save("Site", dv.Site.ToString(), "", DbSavePath, dv.Name); }
                    if (dv.Size != null) { sqlite.Setting_Save("Size", dv.Size.ToString(), "", DbSavePath, dv.Name); }
                    if (dv.ShowRowErrors != null) { sqlite.Setting_Save("ShowRowErrors", dv.ShowRowErrors.ToString(), "", DbSavePath, dv.Name); }
                    if (dv.ShowEditingIcon != null) { sqlite.Setting_Save("ShowEditingIcon", dv.ShowEditingIcon.ToString(), "", DbSavePath, dv.Name); }
                    if (dv.ShowCellToolTips != null) { sqlite.Setting_Save("ShowCellToolTips", dv.ShowCellToolTips.ToString(), "", DbSavePath, dv.Name); }
                    if (dv.ShowCellErrors != null) { sqlite.Setting_Save("ShowCellErrors", dv.ShowCellErrors.ToString(), "", DbSavePath, dv.Name); }
                    if (dv.SelectionMode != null) { sqlite.Setting_Save("SelectionMode", dv.SelectionMode.ToString(), "", DbSavePath, dv.Name); }
                    if (dv.SelectedRows != null) { sqlite.Setting_Save("SelectedRows", dv.SelectedRows.ToString(), "", DbSavePath, dv.Name); }
                    if (dv.SelectedColumns != null) { sqlite.Setting_Save("SelectedColumns", dv.SelectedColumns.ToString(), "", DbSavePath, dv.Name); }
                    if (dv.SelectedCells != null) { sqlite.Setting_Save("SelectedCells", dv.SelectedCells.ToString(), "", DbSavePath, dv.Name); }
                    if (dv.ScrollBars != null) { sqlite.Setting_Save("ScrollBars", dv.ScrollBars.ToString(), "", DbSavePath, dv.Name); }
                    if (dv.RowTemplate != null) { sqlite.Setting_Save("RowTemplate", dv.RowTemplate.ToString(), "", DbSavePath, dv.Name); }
                    if (dv.RowsDefaultCellStyle != null) { sqlite.Setting_Save("RowsDefaultCellStyle", dv.RowsDefaultCellStyle.ToString(), "", DbSavePath, dv.Name); }
                    if (dv.Rows != null) { sqlite.Setting_Save("Rows", dv.Rows.ToString(), "", DbSavePath, dv.Name); }
                    if (dv.RowHeadersWidthSizeMode != null) { sqlite.Setting_Save("RowHeadersWidthSizeMode", dv.RowHeadersWidthSizeMode.ToString(), "", DbSavePath, dv.Name); }
                    if (dv.RowHeadersWidth != null) { sqlite.Setting_Save("RowHeadersWidth", dv.RowHeadersWidth.ToString(), "", DbSavePath, dv.Name); }
                    if (dv.RowHeadersVisible != null) { sqlite.Setting_Save("RowHeadersVisible", dv.RowHeadersVisible.ToString(), "", DbSavePath, dv.Name); }
                    if (dv.RowHeadersDefaultCellStyle != null) { sqlite.Setting_Save("RowHeadersDefaultCellStyle", dv.RowHeadersDefaultCellStyle.ToString(), "", DbSavePath, dv.Name); }
                    if (dv.RowHeadersBorderStyle != null) { sqlite.Setting_Save("RowHeadersBorderStyle", dv.RowHeadersBorderStyle.ToString(), "", DbSavePath, dv.Name); }
                    if (dv.RowCount != null) { sqlite.Setting_Save("RowCount", dv.RowCount.ToString(), "", DbSavePath, dv.Name); }
                    if (dv.RightToLeft != null) { sqlite.Setting_Save("RightToLeft", dv.RightToLeft.ToString(), "", DbSavePath, dv.Name); }
                    if (dv.Right != null) { sqlite.Setting_Save("Right", dv.Right.ToString(), "", DbSavePath, dv.Name); }
                    if (dv.Region != null) { sqlite.Setting_Save("Region", dv.Region.ToString(), "", DbSavePath, dv.Name); }
                    if (dv.RecreatingHandle != null) { sqlite.Setting_Save("RecreatingHandle", dv.RecreatingHandle.ToString(), "", DbSavePath, dv.Name); }
                    if (dv.ReadOnly != null) { sqlite.Setting_Save("ReadOnly", dv.ReadOnly.ToString(), "", DbSavePath, dv.Name); }
                    if (dv.ProductVersion != null) { sqlite.Setting_Save("ProductVersion", dv.ProductVersion.ToString(), "", DbSavePath, dv.Name); }
                    if (dv.ProductName != null) { sqlite.Setting_Save("ProductName", dv.ProductName.ToString(), "", DbSavePath, dv.Name); }
                    if (dv.PreferredSize != null) { sqlite.Setting_Save("PreferredSize", dv.PreferredSize.ToString(), "", DbSavePath, dv.Name); }
                    if (dv.Parent != null) { sqlite.Setting_Save("Parent", dv.Parent.ToString(), "", DbSavePath, dv.Name); }
                    if (dv.NewRowIndex != null) { sqlite.Setting_Save("NewRowIndex", dv.NewRowIndex.ToString(), "", DbSavePath, dv.Name); }
                    if (dv.Name != null) { sqlite.Setting_Save("Name", dv.Name.ToString(), "", DbSavePath, dv.Name); }
                    if (dv.MultiSelect != null) { sqlite.Setting_Save("MultiSelect", dv.MultiSelect.ToString(), "", DbSavePath, dv.Name); }
                    if (dv.MinimumSize != null) { sqlite.Setting_Save("MinimumSize", dv.MinimumSize.ToString(), "", DbSavePath, dv.Name); }
                    if (dv.MaximumSize != null) { sqlite.Setting_Save("MaximumSize", dv.MaximumSize.ToString(), "", DbSavePath, dv.Name); }
                    if (dv.Margin != null) { sqlite.Setting_Save("Margin", dv.Margin.ToString(), "", DbSavePath, dv.Name); }
                    if (dv.Location != null) { sqlite.Setting_Save("Location", dv.Location.ToString(), "", DbSavePath, dv.Name); }
                    if (dv.Left != null) { sqlite.Setting_Save("Left", dv.Left.ToString(), "", DbSavePath, dv.Name); }
                    if (dv.LayoutEngine != null) { sqlite.Setting_Save("LayoutEngine", dv.LayoutEngine.ToString(), "", DbSavePath, dv.Name); }
                    if (dv.IsMirrored != null) { sqlite.Setting_Save("IsMirrored", dv.IsMirrored.ToString(), "", DbSavePath, dv.Name); }
                    if (dv.IsHandleCreated != null) { sqlite.Setting_Save("IsHandleCreated", dv.IsHandleCreated.ToString(), "", DbSavePath, dv.Name); }
                    if (dv.IsDisposed != null) { sqlite.Setting_Save("IsDisposed", dv.IsDisposed.ToString(), "", DbSavePath, dv.Name); }
                    if (dv.IsCurrentRowDirty != null) { sqlite.Setting_Save("IsCurrentRowDirty", dv.IsCurrentRowDirty.ToString(), "", DbSavePath, dv.Name); }
                    if (dv.IsCurrentCellInEditMode != null) { sqlite.Setting_Save("IsCurrentCellInEditMode", dv.IsCurrentCellInEditMode.ToString(), "", DbSavePath, dv.Name); }
                    if (dv.IsCurrentCellDirty != null) { sqlite.Setting_Save("IsCurrentCellDirty", dv.IsCurrentCellDirty.ToString(), "", DbSavePath, dv.Name); }
                    if (dv.IsAccessible != null) { sqlite.Setting_Save("IsAccessible", dv.IsAccessible.ToString(), "", DbSavePath, dv.Name); }
                    if (dv.InvokeRequired != null) { sqlite.Setting_Save("InvokeRequired", dv.InvokeRequired.ToString(), "", DbSavePath, dv.Name); }
                    if (dv.ImeMode != null) { sqlite.Setting_Save("ImeMode", dv.ImeMode.ToString(), "", DbSavePath, dv.Name); }
                    if (dv.HorizontalScrollingOffset != null) { sqlite.Setting_Save("HorizontalScrollingOffset", dv.HorizontalScrollingOffset.ToString(), "", DbSavePath, dv.Name); }
                    if (dv.Height != null) { sqlite.Setting_Save("Height", dv.Height.ToString(), "", DbSavePath, dv.Name); }
                    if (dv.HasChildren != null) { sqlite.Setting_Save("HasChildren", dv.HasChildren.ToString(), "", DbSavePath, dv.Name); }
                    if (dv.Handle != null) { sqlite.Setting_Save("Handle", dv.Handle.ToString(), "", DbSavePath, dv.Name); }
                    if (dv.GridColor != null) { sqlite.Setting_Save("GridColor", dv.GridColor.ToString(), "", DbSavePath, dv.Name); }
                    if (dv.ForeColor != null) { sqlite.Setting_Save("ForeColor", dv.ForeColor.ToString(), "", DbSavePath, dv.Name); }
                    if (dv.Font != null) { sqlite.Setting_Save("Font", dv.Font.ToString(), "", DbSavePath, dv.Name); }
                    if (dv.Focused != null) { sqlite.Setting_Save("Focused", dv.Focused.ToString(), "", DbSavePath, dv.Name); }
                    if (dv.FirstDisplayedScrollingRowIndex != null) { sqlite.Setting_Save("FirstDisplayedScrollingRowIndex", dv.FirstDisplayedScrollingRowIndex.ToString(), "", DbSavePath, dv.Name); }
                    if (dv.FirstDisplayedScrollingColumnIndex != null) { sqlite.Setting_Save("FirstDisplayedScrollingColumnIndex", dv.FirstDisplayedScrollingColumnIndex.ToString(), "", DbSavePath, dv.Name); }
                    if (dv.FirstDisplayedScrollingColumnHiddenWidth != null) { sqlite.Setting_Save("FirstDisplayedScrollingColumnHiddenWidth", dv.FirstDisplayedScrollingColumnHiddenWidth.ToString(), "", DbSavePath, dv.Name); }
                    if (dv.FirstDisplayedCell != null) { sqlite.Setting_Save("FirstDisplayedCell", dv.FirstDisplayedCell.ToString(), "", DbSavePath, dv.Name); }
                    if (dv.EnableHeadersVisualStyles != null) { sqlite.Setting_Save("EnableHeadersVisualStyles", dv.EnableHeadersVisualStyles.ToString(), "", DbSavePath, dv.Name); }
                    if (dv.Enabled != null) { sqlite.Setting_Save("Enabled", dv.Enabled.ToString(), "", DbSavePath, dv.Name); }
                    if (dv.EditMode != null) { sqlite.Setting_Save("EditMode", dv.EditMode.ToString(), "", DbSavePath, dv.Name); }
                    if (dv.EditingPanel != null) { sqlite.Setting_Save("EditingPanel", dv.EditingPanel.ToString(), "", DbSavePath, dv.Name); }
                    if (dv.EditingControl != null) { sqlite.Setting_Save("EditingControl", dv.EditingControl.ToString(), "", DbSavePath, dv.Name); }
                    if (dv.Dock != null) { sqlite.Setting_Save("Dock", dv.Dock.ToString(), "", DbSavePath, dv.Name); }
                    if (dv.Disposing != null) { sqlite.Setting_Save("Disposing", dv.Disposing.ToString(), "", DbSavePath, dv.Name); }
                    if (dv.DisplayRectangle != null) { sqlite.Setting_Save("DisplayRectangle", dv.DisplayRectangle.ToString(), "", DbSavePath, dv.Name); }
                    if (dv.DefaultCellStyle != null) { sqlite.Setting_Save("DefaultCellStyle", dv.DefaultCellStyle.ToString(), "", DbSavePath, dv.Name); }
                    if (dv.DataSource != null) { sqlite.Setting_Save("DataSource", dv.DataSource.ToString(), "", DbSavePath, dv.Name); }
                    if (dv.DataMember != null) { sqlite.Setting_Save("DataMember", dv.DataMember.ToString(), "", DbSavePath, dv.Name); }
                    if (dv.DataBindings != null) { sqlite.Setting_Save("DataBindings", dv.DataBindings.ToString(), "", DbSavePath, dv.Name); }
                    if (dv.Cursor != null) { sqlite.Setting_Save("Cursor", dv.Cursor.ToString(), "", DbSavePath, dv.Name); }
                    if (dv.CurrentRow != null) { sqlite.Setting_Save("CurrentRow", dv.CurrentRow.ToString(), "", DbSavePath, dv.Name); }
                    if (dv.CurrentCellAddress != null) { sqlite.Setting_Save("CurrentCellAddress", dv.CurrentCellAddress.ToString(), "", DbSavePath, dv.Name); }
                    if (dv.CurrentCell != null) { sqlite.Setting_Save("CurrentCell", dv.CurrentCell.ToString(), "", DbSavePath, dv.Name); }
                    if (dv.Created != null) { sqlite.Setting_Save("Created", dv.Created.ToString(), "", DbSavePath, dv.Name); }
                    if (dv.Controls != null) { sqlite.Setting_Save("Controls", dv.Controls.ToString(), "", DbSavePath, dv.Name); }
                    if (dv.ContextMenuStrip != null) { sqlite.Setting_Save("ContextMenuStrip", dv.ContextMenuStrip.ToString(), "", DbSavePath, dv.Name); }
                    if (dv.ContextMenu != null) { sqlite.Setting_Save("ContextMenu", dv.ContextMenu.ToString(), "", DbSavePath, dv.Name); }
                    if (dv.ContainsFocus != null) { sqlite.Setting_Save("ContainsFocus", dv.ContainsFocus.ToString(), "", DbSavePath, dv.Name); }
                    if (dv.Container != null) { sqlite.Setting_Save("Container", dv.Container.ToString(), "", DbSavePath, dv.Name); }
                    if (dv.CompanyName != null) { sqlite.Setting_Save("CompanyName", dv.CompanyName.ToString(), "", DbSavePath, dv.Name); }
                    if (dv.Columns != null) { sqlite.Setting_Save("Columns", dv.Columns.ToString(), "", DbSavePath, dv.Name); }
                    if (dv.ColumnHeadersVisible != null) { sqlite.Setting_Save("ColumnHeadersVisible", dv.ColumnHeadersVisible.ToString(), "", DbSavePath, dv.Name); }
                    if (dv.ColumnHeadersHeightSizeMode != null) { sqlite.Setting_Save("ColumnHeadersHeightSizeMode", dv.ColumnHeadersHeightSizeMode.ToString(), "", DbSavePath, dv.Name); }
                    if (dv.ColumnHeadersHeight != null) { sqlite.Setting_Save("ColumnHeadersHeight", dv.ColumnHeadersHeight.ToString(), "", DbSavePath, dv.Name); }
                    if (dv.ColumnHeadersDefaultCellStyle != null) { sqlite.Setting_Save("ColumnHeadersDefaultCellStyle", dv.ColumnHeadersDefaultCellStyle.ToString(), "", DbSavePath, dv.Name); }
                    if (dv.ColumnHeadersBorderStyle != null) { sqlite.Setting_Save("ColumnHeadersBorderStyle", dv.ColumnHeadersBorderStyle.ToString(), "", DbSavePath, dv.Name); }
                    if (dv.ColumnCount != null) { sqlite.Setting_Save("ColumnCount", dv.ColumnCount.ToString(), "", DbSavePath, dv.Name); }
                    if (dv.ClipboardCopyMode != null) { sqlite.Setting_Save("ClipboardCopyMode", dv.ClipboardCopyMode.ToString(), "", DbSavePath, dv.Name); }
                    if (dv.ClientSize != null) { sqlite.Setting_Save("ClientSize", dv.ClientSize.ToString(), "", DbSavePath, dv.Name); }
                    if (dv.ClientRectangle != null) { sqlite.Setting_Save("ClientRectangle", dv.ClientRectangle.ToString(), "", DbSavePath, dv.Name); }
                    if (dv.CellBorderStyle != null) { sqlite.Setting_Save("CellBorderStyle", dv.CellBorderStyle.ToString(), "", DbSavePath, dv.Name); }
                    if (dv.CausesValidation != null) { sqlite.Setting_Save("CausesValidation", dv.CausesValidation.ToString(), "", DbSavePath, dv.Name); }
                    if (dv.Capture != null) { sqlite.Setting_Save("Capture", dv.Capture.ToString(), "", DbSavePath, dv.Name); }
                    if (dv.CanSelect != null) { sqlite.Setting_Save("CanSelect", dv.CanSelect.ToString(), "", DbSavePath, dv.Name); }
                    if (dv.CanFocus != null) { sqlite.Setting_Save("CanFocus", dv.CanFocus.ToString(), "", DbSavePath, dv.Name); }
                    if (dv.Bounds != null) { sqlite.Setting_Save("Bounds", dv.Bounds.ToString(), "", DbSavePath, dv.Name); }
                    if (dv.Bottom != null) { sqlite.Setting_Save("Bottom", dv.Bottom.ToString(), "", DbSavePath, dv.Name); }
                    if (dv.BorderStyle != null) { sqlite.Setting_Save("BorderStyle", dv.BorderStyle.ToString(), "", DbSavePath, dv.Name); }
                    if (dv.BindingContext != null) { sqlite.Setting_Save("BindingContext", dv.BindingContext.ToString(), "", DbSavePath, dv.Name); }
                    if (dv.BackgroundColor != null) { sqlite.Setting_Save("BackgroundColor", dv.BackgroundColor.ToString(), "", DbSavePath, dv.Name); }
                    if (dv.AutoSizeRowsMode != null) { sqlite.Setting_Save("AutoSizeRowsMode", dv.AutoSizeRowsMode.ToString(), "", DbSavePath, dv.Name); }
                    if (dv.AutoSizeColumnsMode != null) { sqlite.Setting_Save("AutoSizeColumnsMode", dv.AutoSizeColumnsMode.ToString(), "", DbSavePath, dv.Name); }
                    if (dv.AutoSize != null) { sqlite.Setting_Save("AutoSize", dv.AutoSize.ToString(), "", DbSavePath, dv.Name); }
                    if (dv.AutoScrollOffset != null) { sqlite.Setting_Save("AutoScrollOffset", dv.AutoScrollOffset.ToString(), "", DbSavePath, dv.Name); }
                    if (dv.AutoGenerateColumns != null) { sqlite.Setting_Save("AutoGenerateColumns", dv.AutoGenerateColumns.ToString(), "", DbSavePath, dv.Name); }
                    if (dv.Anchor != null) { sqlite.Setting_Save("Anchor", dv.Anchor.ToString(), "", DbSavePath, dv.Name); }
                    if (dv.AlternatingRowsDefaultCellStyle != null) { sqlite.Setting_Save("AlternatingRowsDefaultCellStyle", dv.AlternatingRowsDefaultCellStyle.ToString(), "", DbSavePath, dv.Name); }
                    if (dv.AllowUserToResizeRows != null) { sqlite.Setting_Save("AllowUserToResizeRows", dv.AllowUserToResizeRows.ToString(), "", DbSavePath, dv.Name); }
                    if (dv.AllowUserToResizeColumns != null) { sqlite.Setting_Save("AllowUserToResizeColumns", dv.AllowUserToResizeColumns.ToString(), "", DbSavePath, dv.Name); }
                    if (dv.AllowUserToOrderColumns != null) { sqlite.Setting_Save("AllowUserToOrderColumns", dv.AllowUserToOrderColumns.ToString(), "", DbSavePath, dv.Name); }
                    if (dv.AllowUserToDeleteRows != null) { sqlite.Setting_Save("AllowUserToDeleteRows", dv.AllowUserToDeleteRows.ToString(), "", DbSavePath, dv.Name); }
                    if (dv.AllowUserToAddRows != null) { sqlite.Setting_Save("AllowUserToAddRows", dv.AllowUserToAddRows.ToString(), "", DbSavePath, dv.Name); }
                    if (dv.AllowDrop != null) { sqlite.Setting_Save("AllowDrop", dv.AllowDrop.ToString(), "", DbSavePath, dv.Name); }
                    if (dv.AdvancedRowHeadersBorderStyle != null) { sqlite.Setting_Save("AdvancedRowHeadersBorderStyle", dv.AdvancedRowHeadersBorderStyle.ToString(), "", DbSavePath, dv.Name); }
                    if (dv.AdvancedColumnHeadersBorderStyle != null) { sqlite.Setting_Save("AdvancedColumnHeadersBorderStyle", dv.AdvancedColumnHeadersBorderStyle.ToString(), "", DbSavePath, dv.Name); }
                    if (dv.AdvancedCellBorderStyle != null) { sqlite.Setting_Save("AdvancedCellBorderStyle", dv.AdvancedCellBorderStyle.ToString(), "", DbSavePath, dv.Name); }
                    if (dv.AdjustedTopLeftHeaderBorderStyle != null) { sqlite.Setting_Save("AdjustedTopLeftHeaderBorderStyle", dv.AdjustedTopLeftHeaderBorderStyle.ToString(), "", DbSavePath, dv.Name); }
                    if (dv.AccessibleRole != null) { sqlite.Setting_Save("AccessibleRole", dv.AccessibleRole.ToString(), "", DbSavePath, dv.Name); }
                    if (dv.AccessibleName != null) { sqlite.Setting_Save("AccessibleName", dv.AccessibleName.ToString(), "", DbSavePath, dv.Name); }
                    if (dv.AccessibleDescription != null) { sqlite.Setting_Save("AccessibleDescription", dv.AccessibleDescription.ToString(), "", DbSavePath, dv.Name); }
                    if (dv.AccessibleDefaultActionDescription != null) { sqlite.Setting_Save("AccessibleDefaultActionDescription", dv.AccessibleDefaultActionDescription.ToString(), "", DbSavePath, dv.Name); }
                    //if (dv.AccessibilityObject != null) { sqlite.Setting_Save("AccessibilityObject", dv.AccessibilityObject.ToString(), "", DbSavePath, dv.Name); }
                }





                /// <summary>Restore saved visibility settings of the datagrid (from SQLite Settings Db File)</summary>
                /// <param name="dv"> </param>
                /// <param name="GridDbFile"> </param>
                /// <param name="TableName"> </param>
                public void Load_Grid_Column_Settings(DataGridView dv, string GridDbFile, string TableName = "GridDisplay")
                {
                    if (dv == null) { dv = gridSetup.currentGrid; }
                    if (dv != null) { gridSetup.currentGrid = dv; }

                    // check to see if there are any columns to populate list first
                    int colCount = 0;
                    try { colCount = dv.Rows[0].Cells.Count; }
                    catch
                    {
                        ahk.MsgBox("Unable To Save Grid Settings - No Columns Visible.");
                        return;
                    }

                    string ColumnName = "";
                    int columnCount = dv.Rows[0].Cells.Count;
                    for (int i = 0; i < columnCount; i++)
                    {

                        ColumnName = dv.Columns[i].HeaderText;

                        string Visible = sqlite.Setting_Value(dv.Columns[i].HeaderText, GridDbFile, TableName);

                        if (Visible.ToUpper() == "TRUE") { dv.Columns[i].Visible = true; }
                        if (Visible.ToUpper() == "FALSE") { dv.Columns[i].Visible = false; }
                    }
                }


                /// <summary>Save DataGridView contents to sqlite db file</summary>
                /// <param name="dv"> </param>
                /// <param name="DbFile"> </param>
                /// <param name="TableName"> </param>
                /// <param name="ClearTableFirst"> </param>
                /// <param name="SkipColumnNum"> </param>
                public List<string> Grid_InsertSQLite_CodeGen(DataGridView dv, string DbFile, string TableName, bool ClearTableFirst = true, int SkipColumnNum = -1)
                {
                    if (dv == null) { dv = gridSetup.currentGrid; }
                    if (dv != null) { gridSetup.currentGrid = dv; }

                    List<string> CommandList = new List<string>();
                    // SkipColumnNum allows you to omit saving data from that column, -1 means don't skip any
                    bool SkipIDField = false;
                    if (SkipColumnNum == -1) { SkipIDField = true; }


                    List<string> Grid_Columns = Column_Names(dv);  // return list of column names from gridview


                    // populate sqlite insert statement

                    string Command = "Insert into [" + TableName + "] (";
                    string VarList = "";

                    int ColNum = 0;
                    foreach (string col in Grid_Columns)
                    {
                        if (SkipIDField) { if (ColNum == SkipColumnNum) { ColNum++; continue; } } // option to skip updating field in sqlite db

                        if (VarList != "") { VarList = VarList + ", [" + col + "]"; }
                        if (VarList == "") { VarList = "[" + col + "]"; }
                        ColNum++;
                    }

                    Command = Command + VarList + ") Values (";

                    int columnCount = dv.Rows[0].Cells.Count;
                    string RowValues = "";

                    // check to see if user has datagrid row adding enabled, used to offset counter in row loops
                    int AdjustCount = 0;
                    if (dv.AllowUserToAddRows) { AdjustCount = 1; }  // subtract 1 row if the user is allowed to add blank new rows


                    for (int i = 0; i < dv.Rows.Count - AdjustCount; i++) // loop through each row
                    {
                        RowValues = "";

                        for (int c = 0; c < columnCount; c++)  // loop through each column
                        {
                            if (SkipIDField) { if (c == SkipColumnNum) { continue; } } // option to skip updating first field in sqlite db

                            string value = dv.Rows[i].Cells[c].Value.ToString();

                            value = ahk.FixSpecialChars(value); //remove invalid characters before writing

                            if (RowValues != "") { RowValues = RowValues + ", '" + value + "'"; }
                            if (RowValues == "") { RowValues = "'" + value + "'"; continue; }
                        }

                        string SQLCommand = Command + RowValues + ")";


                        CommandList.Add(SQLCommand);
                        //bool Inserted = sqlite.Execute(DbFile, SQLCommand, true);

                    }


                    return CommandList;
                }

                /// <summary>Create New SQLite Table From DataGridView Column Names</summary>
                /// <param name="dv"> </param>
                /// <param name="CreateDbFile"> </param>
                /// <param name="NewTableName"> </param>
                public bool New_Table_From_GridColumns(DataGridView dv, string CreateDbFile, string NewTableName = "GridTable")
                {
                    if (dv == null) { dv = gridSetup.currentGrid; }
                    if (dv != null) { gridSetup.currentGrid = dv; }

                    //string NewTableName = "";  // grab new table name from first line 
                    //for (int i = 0; i < 1; i++) { if (i == 0) { NewTableName = dataGridView1.Rows[i].Cells[0].Value.ToString(); continue; } }

                    //string NewTableLine = "ID INTEGER PRIMARY KEY, SettingName VARCHAR, SettingValue VARCHAR, Flag BOOL";

                    string NewTableLine = "ID INTEGER PRIMARY KEY";  // default to create ID field

                    List<string> ColumnList = Column_Names(dv);

                    foreach (string Col in ColumnList)
                    {
                        NewTableLine = NewTableLine + ", [" + Col + "] VARCHAR";
                    }


                    // check to see if the table already exists 
                    bool TableExists = sqlite.Table_Exists(CreateDbFile, NewTableName);

                    if (TableExists)
                    {
                        if (GlobalDebug) { ahk.MsgBox("Unable To Create " + NewTableName + " - ALREADY EXISTS"); }
                        return false;
                    }

                    // create new sqlite table
                    bool Created = sqlite.Table_New(CreateDbFile, NewTableName, NewTableLine, false);


                    // return true / false after table attempts to create, with option to display debug error message
                    if (!Created)
                    {
                        if (GlobalDebug) { ahk.MsgBox("Unable To Create " + NewTableName + " - ERROR CREATING TABLE"); }
                        return false;
                    }

                    if (Created)
                    {
                        if (GlobalDebug) { ahk.MsgBox("Created " + NewTableName); }
                        return true;
                    }

                    return false;
                }


                /// <summary>Edit DataGrid Column Visibility - Display from TreeView</summary>
                /// <param name="dv"> </param>
                /// <param name="TV"> </param>
                /// <param name="ParentName"> </param>
                /// <param name="ClearTV"> </param>
                /// <param name="CheckBoxes"> </param>
                /// <param name="ExpandOnLoad"> </param>
                public void Load_DataGridColumns_inTreeView(DataGridView dv, TreeView TV, string ParentName = "DataGridView_Columns", bool ClearTV = true, bool CheckBoxes = true, bool ExpandOnLoad = true)
                {
                    if (dv == null) { dv = gridSetup.currentGrid; }
                    if (dv != null) { gridSetup.currentGrid = dv; }

                    _TreeViewControl tv = new _TreeViewControl();

                    if (dv.Visible)
                    {
                        TV.CheckBoxes = true;
                        tv.Load_DataGridColumns(TV, dv, ParentName, ClearTV, CheckBoxes, ExpandOnLoad);
                        TV.ExpandAll(); // expand search results in tree
                    }
                }



                private void DataGrid_Notes()  // not functions yet - random dv notes
                {

                    //dv.Columns["Selected"].DisplayIndex = 0;  // change position of column to display in datagridview

                    //dv.Columns["SSR"].Name = "SSRNumber";  // rename a datatable column (didn't work well)


                    //dt.Columns.Add(new DataColumn("Selected", typeof(bool))); // add checkbox column to datatable



                    //// check to see if user has datagrid row adding enabled, used to offset counter in row loops

                    //int AdjustCount = 0; // subtract 1 row if the user is allowed to add blank new rows
                    //if (dv.AllowUserToAddRows) { AdjustCount = 1; }

                }

        */
        #endregion


        #region === Row / Column Width/Height Get/Set ====
        /*
                //===== Row Numbers =============================================

                public void RowNumbers_Add(DataGridView dv)
                {
                    if (dv == null) { dv = gridSetup.currentGrid; }
                    if (dv != null) { gridSetup.currentGrid = dv; }

                    for (int i = 0; i < dv.Rows.Count; i++)
                    {
                        dv.Rows[i].HeaderCell.Value = (i + 1).ToString(); // add numbers to row header
                        dv.Rows[i].HeaderCell.Style.BackColor = Color.Beige;
                    }
                }

                public void RowNumbers_Remove(DataGridView dv)
                {
                    if (dv == null) { dv = gridSetup.currentGrid; }
                    if (dv != null) { gridSetup.currentGrid = dv; }

                    for (int i = 0; i < dv.Rows.Count; i++)
                    {
                        dv.Rows[i].HeaderCell.Value = "";  // clear numbers from row header
                        dv.Rows[i].HeaderCell.Style.BackColor = Color.Beige;
                    }
                }


                //===== ROW HEIGHT ===============================================================================

                /// <summary>
                /// Get List of Row Heights from DataGridView
                /// </summary>
                /// <param name="dv">DataGridView to Return Heights From</param>
                /// <param name="RowNumber">Specific Row Number to Search. Default = -1 (All Rows)</param>
                public List<int> RowHeights_Get(DataGridView dv, int RowNumber = -1)
                {
                    if (dv == null) { dv = gridSetup.currentGrid; }
                    if (dv != null) { gridSetup.currentGrid = dv; }

                    List<int> RowHeights = new List<int>();

                    // loop through each row in grid
                    int AdjustCount = 0; // subtract 1 row if the user is allowed to add blank new rows
                    if (dv.AllowUserToAddRows) { AdjustCount = 1; }  // remove 1 from row loop to account for new/blank row if enabled
                    for (int i = 0; i < dv.Rows.Count - AdjustCount; i++)
                    {
                        if (RowNumber == -1)  // -1 = Apply to All Rows
                        {
                            DataGridViewRow dgv = dv.Rows[i];
                            int RowHeight = dgv.Height;
                            RowHeights.Add(RowHeight);
                            //ahk.MsgBox("Row " + i + " Height = " + RowHeight.ToString());
                        }

                        if (RowNumber != -1)  // Apply specific single row
                        {
                            if (i == RowNumber)
                            {
                                DataGridViewRow dgv = dv.Rows[i];
                                int RowHeight = dgv.Height;
                                RowHeights.Add(RowHeight);
                            }
                        }

                    }

                    return RowHeights;
                }

                /// <summary>
                /// Set the Row Height in a DataGridView
                /// </summary>
                /// <param name="dv">DataGridView to Modify</param>
                /// <param name="NewHeight">New Row Height</param>
                /// <param name="RowNumber">Specific Row Number to Adjust. Default = -1 (All Rows)</param>
                public void RowHeight_Set(DataGridView dv, int NewHeight, int RowNumber = -1)
                {
                    if (dv == null) { dv = gridSetup.currentGrid; }
                    if (dv != null) { gridSetup.currentGrid = dv; }

                    List<int> RowHeights = new List<int>();

                    // loop through each row in grid
                    int AdjustCount = 0; // subtract 1 row if the user is allowed to add blank new rows
                    if (dv.AllowUserToAddRows) { AdjustCount = 1; }  // remove 1 from row loop to account for new/blank row if enabled
                    for (int i = 0; i < dv.Rows.Count - AdjustCount; i++)
                    {
                        if (RowNumber == -1)  // -1 = Apply to All Rows
                        {
                            DataGridViewRow dgv = dv.Rows[i];
                            int RowHeight = dgv.Height;
                            RowHeights.Add(RowHeight);
                        }

                        if (RowNumber != -1)  // Apply specific single row
                        {
                            if (i == RowNumber)
                            {
                                DataGridViewRow dgv = dv.Rows[i];
                                int RowHeight = dgv.Height;
                                RowHeights.Add(RowHeight);
                            }
                        }

                        //ahk.MsgBox("Row " + i + " Height = " + RowHeight.ToString());
                    }
                }


                public void RowHeights_Set(DataGridView dv, List<int> HeightList)
                {
                    if (dv == null) { dv = gridSetup.currentGrid; }
                    if (dv != null) { gridSetup.currentGrid = dv; }

                    // loop through each row in grid
                    int AdjustCount = 0; // subtract 1 row if the user is allowed to add blank new rows
                    if (dv.AllowUserToAddRows) { AdjustCount = 1; }  // remove 1 from row loop to account for new/blank row if enabled
                    for (int i = 0; i < dv.Rows.Count - AdjustCount; i++)
                    {
                        int listHeight = lst.Return_List_ValueInt(HeightList, i);

                        DataGridViewRow dgv = dv.Rows[i];
                        dgv.Height = listHeight;
                    }
                }


                /// <summary>
                /// Autosizes Row Heights + Headers, Shrinks Down Results
                /// </summary>
                /// <param name="dv"></param>
                public void RowHeight_Shrink(DataGridView dv)
                {
                    if (dv == null) { dv = gridSetup.currentGrid; }
                    if (dv != null) { gridSetup.currentGrid = dv; }

                    //Resize the height of the column headers. 
                    dv.AutoResizeColumnHeadersHeight();

                    //Resize all the row heights to fit the contents of all non-header cells.
                    dv.AutoResizeRows(DataGridViewAutoSizeRowsMode.AllCellsExceptHeaders);
                }


                //===== ROW DIVIDER =================================================================================

                public int RowDividerHeight_Get(DataGridView dv)
                {
                    if (dv == null) { dv = gridSetup.currentGrid; }
                    if (dv != null) { gridSetup.currentGrid = dv; }

                    // loop through each row in grid
                    int AdjustCount = 0; // subtract 1 row if the user is allowed to add blank new rows
                    if (dv.AllowUserToAddRows) { AdjustCount = 1; }  // remove 1 from row loop to account for new/blank row if enabled
                    int dividerHeight = 0;
                    for (int i = 0; i < dv.Rows.Count - AdjustCount; i++)
                    {
                        DataGridViewRow dgv = dv.Rows[i];
                        dividerHeight = dgv.DividerHeight;
                        ahk.MsgBox("Row " + i + " | Divider Height = " + dividerHeight.ToString());
                    }

                    return dividerHeight;
                }

                public void RowDividerHeight_Set(DataGridView dv, int RowDividerHeight = 0)
                {
                    if (dv == null) { dv = gridSetup.currentGrid; }
                    if (dv != null) { gridSetup.currentGrid = dv; }

                    // loop through each row in grid
                    int AdjustCount = 0; // subtract 1 row if the user is allowed to add blank new rows
                    if (dv.AllowUserToAddRows) { AdjustCount = 1; }  // remove 1 from row loop to account for new/blank row if enabled
                    for (int i = 0; i < dv.Rows.Count - AdjustCount; i++)
                    {
                        DataGridViewRow dgv = dv.Rows[i];
                        dgv.DividerHeight = RowDividerHeight;
                        sb.StatusBar("Set RowDividerHeight = " + RowDividerHeight);
                    }
                }



                //===== COLUMN WIDTH ========================================================================================

                public int Columns_Total_Width(DataGridView dv)  // Total Display Width adding Visible Column Sizes
                {
                    if (dv == null) { dv = gridSetup.currentGrid; }
                    if (dv != null) { gridSetup.currentGrid = dv; }

                    Fill_Column_Width(dv);

                    int TotalWidth = 0;

                    int i = 0;
                    foreach (DataGridViewColumn col in dv.Columns)
                    {
                        //col = dv.Columns[i];
                        if (col.Visible)
                        {
                            TotalWidth += col.Width;
                        }
                        i++;
                    }

                    //int columnCount = dv.Rows[0].Cells.Count;
                    //for (int i = 0; i < columnCount; i++)
                    //{
                    //    DataGridViewColumn coll = new DataGridViewColumn();
                    //    coll = dv.Columns[i];
                    //    if (coll.Visible)
                    //    {
                    //        TotalWidth += coll.Width;
                    //    }
                    //}

                    return TotalWidth;
                }
                public int Column_Width_Calculated(DataGridView dv)  // figure out the column space available to fill on gridview
                {
                    if (dv == null) { dv = gridSetup.currentGrid; }
                    if (dv != null) { gridSetup.currentGrid = dv; }

                    int wid = dv.Width;  // width of grid
                    if (dv.RowHeadersVisible) { wid -= dv.RowHeadersWidth; } // subtract row header if visible
                    wid -= dv.Margin.Left; // margin space
                    //wid -= dv.Margin.Right;
                    return wid;
                }


                //=== Get Width (1 Column)
                public int Column_Width_Percentage(DataGridView dv, string ColumnText = "", int ColNum = -1) //
                {
                    if (dv == null) { dv = gridSetup.currentGrid; }
                    if (dv != null) { gridSetup.currentGrid = dv; }

                    //int CalcColWidth = Calculated_Column_Width(dataGridView1);
                    int TotalWidth = Columns_Total_Width(dv);  // total width of all columns

                    int columnCount = dv.Rows[0].Cells.Count;
                    for (int i = 0; i < columnCount; i++)
                    {
                        DataGridViewColumn coll = new DataGridViewColumn();
                        coll = dv.Columns[i];

                        if (ColumnText != "")
                        {
                            if (coll.HeaderText == ColumnText)
                            {
                                if (coll.Visible)
                                {
                                    // convert width to `% int
                                    double Percent = (double)coll.Width / (double)TotalWidth;
                                    Percent *= 100;
                                    double percent = Math.Round(Percent);
                                    int per = (int)percent;
                                    return per;
                                }
                            }
                        }

                        if (ColNum != -1)
                        {
                            if (i == ColNum)
                            {
                                if (coll.Visible)
                                {
                                    // convert width to `% int
                                    double Percent = (double)coll.Width / (double)TotalWidth;
                                    Percent *= 100;
                                    double percent = Math.Round(Percent);
                                    int per = (int)percent;
                                    return per;
                                }
                            }
                        }
                    }

                    return -1;  // no match found (visible)
                }

                //=== Get Width (All Columns)
                public List<int> Column_Width_Percentages(DataGridView dv, bool MsgBoxDisp = false)  // displays the width percentage of each column in grid
                {
                    if (dv == null) { dv = gridSetup.currentGrid; }
                    if (dv != null) { gridSetup.currentGrid = dv; }

                    List<int> ColWidths = new List<int>();

                    int i = 0;
                    foreach (DataGridViewColumn col in dv.Columns)
                    {
                        if (col.Visible)
                        {
                            int per = Column_Width_Percentage(dv, col.HeaderText);

                            if (MsgBoxDisp) { ahk.MsgBox("[visible] Col " + i + " = " + per.ToString()); }

                            ColWidths.Add(per);
                        }
                        if (!col.Visible)
                        {
                            int per = Column_Width_Percentage(dv, col.HeaderText);

                            if (MsgBoxDisp) { ahk.MsgBox("[hidden] Col " + i + " = " + per.ToString()); }
                        }

                        i++;
                    }

                    return ColWidths;
                }


                //=== Set Width Int (1 Column)
                public void Column_Width_Set(DataGridView dv, int ColNumber, int Width)  // set the width of a column
                {
                    if (dv == null) { dv = gridSetup.currentGrid; }
                    if (dv != null) { gridSetup.currentGrid = dv; }

                    try
                    {
                        DataGridViewColumn col = new DataGridViewColumn();
                        col = dv.Columns[ColNumber];
                        col.Width = Width;
                    }
                    catch
                    {

                    }
                }

                //=== Set Width Percentage (1 Column)
                public void Column_Width_SetPercent(DataGridView dv, int ColNumber, int WidthPercent)  // set the width of a column (percentage of total grid display)
                {
                    if (dv == null) { dv = gridSetup.currentGrid; }
                    if (dv != null) { gridSetup.currentGrid = dv; }

                    int CalcColWidth = Column_Width_Calculated(dv);  // total width available
                    double wPercent = (double)WidthPercent / 100;
                    double Percent = (double)CalcColWidth * wPercent;
                    Percent = Math.Round(Percent); // round to nearest whole number
                    int ColSpace = (int)Percent;

                    ColSpace -= 2;

                    Column_Width_Set(dv, ColNumber, ColSpace);
                }


                //=== Set Width Percentages (All Columns)
                public void Column_WidthPercents_Set(DataGridView dv, List<int> ColWidth)
                {
                    if (dv == null) { dv = gridSetup.currentGrid; }
                    if (dv != null) { gridSetup.currentGrid = dv; }

                    int i = 0;
                    foreach (DataGridViewColumn col in dv.Columns)
                    {
                        int colWidth = lst.Return_List_ValueInt(ColWidth, i);  // return value from position in list
                        Column_Width_SetPercent(dv, i, colWidth);
                        i++;
                    }


                    //int columnCount = dv.Rows[0].Cells.Count;
                    //for (int i = 0; i < columnCount; i++)
                    //{
                    //   int colWidth = lst.Return_List_ValueInt(ColWidth, i);  // return value from position in list
                    // Column_Width_SetPercent(dv, i, colWidth);
                    //}
                }

        */
        #endregion


        #region === DataGridView: Right Click ===
        /*
                // File List DataGrid Right Click Options

                // int DataGrid1_CurrentRow = 0;
                // populate datagrid function here

                // in the application, assign mouse click action  [example]

                private void dataGridView1_MouseClick(object sender, MouseEventArgs e)  // example datagridview mouseclick event
                {
                    Grid_RClickMenu(sender, e);
                }

                // create menu options [example]
                private void Grid_RClickMenu(object sender, MouseEventArgs e)  // example right click menu for datagridview
                {
                    //ContextMenuStrip dg1 = new ContextMenuStrip();
                    //dg1.Items.Clear();
                    //dataGridView1.ContextMenuStrip = null;

                    //// if user right clicks on the grid
                    //if (e.Button == MouseButtons.Right)
                    //{
                    //    DataGrid1_CurrentRow = dataGridView1.HitTest(e.X, e.Y).RowIndex; //current row number selected by user

                    //    dg1.Items.Clear();
                    //    dataGridView1.ContextMenuStrip = null;

                    //    string Option = "Default"; // option to have more than one menu later

                    //    if (Option == "Default")
                    //    {
                    //        ToolStripMenuItem Opt1 = new ToolStripMenuItem("[ File Options ]");
                    //        ToolStripMenuItem Opt2 = new ToolStripMenuItem("Open File");
                    //        ToolStripMenuItem Opt3 = new ToolStripMenuItem("Open Dir");
                    //        ToolStripMenuItem Opt4 = new ToolStripMenuItem("Rename File");
                    //        ToolStripMenuItem Opt5 = new ToolStripMenuItem("Delete File");

                    //        //Assign event handlers
                    //        Opt1.Click += new EventHandler(RClick_Options);
                    //        Opt2.Click += new EventHandler(RClick_Options);
                    //        Opt3.Click += new EventHandler(RClick_Options);
                    //        Opt4.Click += new EventHandler(RClick_Options);
                    //        Opt5.Click += new EventHandler(RClick_Options);

                    //        //Add to main context menu
                    //        dg1.Items.AddRange(new ToolStripItem[] { Opt1, Opt2, Opt3, Opt4, Opt5 });
                    //    }


                    //    //Assign to datagridview
                    //    dataGridView1.ContextMenuStrip = dg1;

                    //    dg1.Show(dataGridView1, new Point(e.X, e.Y));

                    //}
                }

                // actions for menu options [example]
                private void RClick_Options(object sender, System.EventArgs e)  // example right click menu options for datagridview
                {
                    //DataGridView dv = (DataGridView)sender;

                    //try
                    //{
                    //    //dv.Rows.

                    //    int DDLRowVal = DataGrid1_CurrentRow; //currently selected grid2 row number

                    //    string Options = sender.ToString();  // name of the option passed from the rclick menu
                    //    string FilePath = dataGridView.Rows[DDLRowVal].Cells[0].Value.ToString();
                    //    string FileName = dataGridView.Rows[DDLRowVal].Cells[1].Value.ToString();

                    //    //======================
                    //    //  File Options
                    //    //======================

                    //    Options = Options.ToUpper();

                    //    if (Options.ToString() == "OPEN FILE")
                    //    {
                    //        ahk.Run(FilePath);
                    //        //Load_FileGrid();
                    //    }
                    //    if (Options.ToString() == "OPEN DIR")
                    //    {
                    //        FileInfo info = new FileInfo(FilePath);
                    //        ahk.OpenDir(info.Directory.ToString());
                    //        Load_FileGrid();
                    //    }
                    //    if (Options.ToString() == "RENAME FILE")
                    //    {
                    //        //MessageBox.Show(Options.ToString() + " - " + FilePath);
                    //        FileInfo info = new FileInfo(FilePath);

                    //        // prompt to rename existing file
                    //        string value = info.FullName;
                    //        if (ahk.InputBox("Rename File: ", "", ref value) == DialogResult.OK)
                    //        {
                    //            string UserEntry = value;
                    //            ahk.FileRename(info.FullName, value, true);
                    //            Load_FileGrid();
                    //        }

                    //    }
                    //    if (Options.ToString() == "DELETE FILE")
                    //    {
                    //        var ResultValue = ahk.YesNoBox("Delete " + FileName + "?", "Delete File?");
                    //        if (ResultValue.ToString() == "Yes")
                    //        {
                    //            ahk.FileDelete(FilePath);
                    //            Load_FileGrid();
                    //        }
                    //    }

                    //}
                    //catch
                    //{
                    //}

                }
        */
        #endregion


        #region === DataGridView: Display / Customization ===
        /*
                // Grid Visibility

                /// <summary>Show DataGridView Control (if Hidden)</summary>
                /// <param name="dv"> </param>
                public void ShowGrid(DataGridView dv)
                {
                    if (dv == null) { dv = gridSetup.currentGrid; }
                    if (dv != null) { gridSetup.currentGrid = dv; }

                    dv.Visible = true;
                }

                /// <summary>Hide DataGridView Control (if Visible)</summary>
                /// <param name="dv"> </param>
                public void HideGrid(DataGridView dv)
                {
                    if (dv == null) { dv = gridSetup.currentGrid; }
                    if (dv != null) { gridSetup.currentGrid = dv; }

                    dv.Visible = false;
                }

                /// <summary>If Hidden then Show, If Visible then Hide (Toggle Visibility of DataGridView Control)</summary>
                /// <param name="dv"> </param>
                public void Toggle_Grid(DataGridView dv)
                {
                    if (dv == null) { dv = gridSetup.currentGrid; }
                    if (dv != null) { gridSetup.currentGrid = dv; }

                    if (dv.Visible) { dv.Visible = false; return; }
                    if (!dv.Visible) { dv.Visible = true; return; }
                }

                /// <summary>Sets the background color in a DataGridView</summary>
                /// <param name="dv"> </param>
                /// <param name="BackGroundColor"> </param>
                public void Grid_Background_Color(DataGridView dv, Color BackGroundColor)
                {
                    if (dv == null) { dv = gridSetup.currentGrid; }
                    if (dv != null) { gridSetup.currentGrid = dv; }

                    dv.BackgroundColor = BackGroundColor;
                }


                //#### Add New Column Type to DataGridView  #####

                /// <summary>Add TextBox Column to DataGridView</summary>
                /// <param name="dv"> </param>
                /// <param name="ColName"> </param>
                public void Column_Add_TextColumn(DataGridView dv, string ColName = "TextBox")
                {
                    if (dv == null) { dv = gridSetup.currentGrid; }
                    if (dv != null) { gridSetup.currentGrid = dv; }

                    // add new column to gridview - TextBox Column
                    DataGridViewTextBoxColumn textCol = new DataGridViewTextBoxColumn();
                    textCol.Name = ColName; // set column name
                    dv.Columns.Add(textCol);  // assign to grid
                }

                /// <summary>Add CheckBox Column to DataGridView</summary>
                /// <param name="dv"> </param>
                /// <param name="ColName"> </param>
                public void Column_Add_CheckBoxColumn(DataGridView dv, string ColName = "Check")
                {
                    if (dv == null) { dv = gridSetup.currentGrid; }
                    if (dv != null) { gridSetup.currentGrid = dv; }

                    // add new column to gridview - CheckBox Column
                    DataGridViewCheckBoxColumn checkCol = new DataGridViewCheckBoxColumn();
                    checkCol.Name = ColName;  // set column name
                    dv.Columns.Add(checkCol);  // assign to grid
                }

                /// <summary>Add ComboBox Column to DataGridView</summary>
                /// <param name="dv"> </param>
                /// <param name="ColName"> </param>
                public void Column_Add_ComboBoxColumn(DataGridView dv, string ColName = "Combo")
                {
                    if (dv == null) { dv = gridSetup.currentGrid; }
                    if (dv != null) { gridSetup.currentGrid = dv; }

                    // add new column to gridview - ComboBox Column
                    DataGridViewComboBoxColumn comboCol = new DataGridViewComboBoxColumn();
                    comboCol.Name = ColName;  // set column name
                    dv.Columns.Add(comboCol);  // assign to grid
                }

                /// <summary>Add Button Column to DataGridView</summary>
                /// <param name="dv"> </param>
                /// <param name="ColName"> </param>
                public void Column_Add_ButtonColumn(DataGridView dv, string ColName = "Button")
                {
                    if (dv == null) { dv = gridSetup.currentGrid; }
                    if (dv != null) { gridSetup.currentGrid = dv; }

                    // add new column to gridview - Button Column
                    DataGridViewButtonColumn buttonCol = new DataGridViewButtonColumn();
                    buttonCol.Name = ColName;  // set column name
                    dv.Columns.Add(buttonCol);  // assign to grid
                }

                /// <summary>Add Link Column to DataGridView</summary>
                /// <param name="dv"> </param>
                /// <param name="ColName"> </param>
                public void Column_Add_LinkColumn(DataGridView dv, string ColName = "Link")
                {
                    if (dv == null) { dv = gridSetup.currentGrid; }
                    if (dv != null) { gridSetup.currentGrid = dv; }

                    // add new column to gridview - Link Column
                    DataGridViewLinkColumn linkCol = new DataGridViewLinkColumn();
                    linkCol.Name = ColName;  // set column name
                    dv.Columns.Add(linkCol);  // assign to grid
                }

                /// <summary>Add Image Column to DataGridView</summary>
                /// <param name="dv"> </param>
                /// <param name="ColName"> </param>
                public void Column_Add_ImageColumn(DataGridView dv, string ColName = "Image")
                {
                    if (dv == null) { dv = gridSetup.currentGrid; }
                    if (dv != null) { gridSetup.currentGrid = dv; }

                    // add new column to gridview - Image Column
                    DataGridViewImageColumn imgcol = new DataGridViewImageColumn();//Create image column
                    imgcol.Name = ColName; //set  column name
                    dv.Columns.Add(imgcol); // add column to Datagridview
                }



                //### Add + Populate Columns ####

                /// <summary>Add new Image Column to DataGridView, populating new column with images</summary>
                /// <param name="dv"> </param>
                /// <param name="ImageDir"> </param>
                /// <param name="SearchParam"> </param>
                /// <param name="Recurse"> </param>
                /// <param name="ResizeRowSize"> </param>
                public void Add_Populate_ImageColumn(DataGridView dv, string ImageDir, string SearchParam = "*.png", bool Recurse = false, bool ResizeRowSize = true)
                {
                    if (dv == null) { dv = gridSetup.currentGrid; }
                    if (dv != null) { gridSetup.currentGrid = dv; }

                    _Images img = new _Images();

                    // add new column to gridview - Image Column
                    DataGridViewImageColumn imgcol = new DataGridViewImageColumn();//Create image column
                    imgcol.Name = "Image"; //set  column name
                    dv.Columns.Add(imgcol); // add column to Datagridview

                    // populate Image Column
                    List<System.Drawing.Image> gridImages = img.Grid_Image_List(ImageDir, SearchParam, Recurse);  // build image list 

                    int RowOffset = 0; if (dv.AllowUserToAddRows) { RowOffset = 1; }  // adjust for extra row that can be there when looping
                    for (int i = 0; i < dv.Rows.Count - RowOffset; i++)
                    {
                        Image resized = img.ResizeImage(gridImages[i], 50, 75);  // resize image to fix grid field

                        dv.Rows[i].Cells["Image"].Value = resized; //set value
                    }


                    if (ResizeRowSize)  // option to autosize the row height to fit the image contents
                    {
                        //Resize all the row heights to fit the contents of all non-header cells.
                        dv.AutoResizeRows(DataGridViewAutoSizeRowsMode.AllCellsExceptHeaders);
                    }

                }

                /// <summary>Add new ComboBox Column to DataGridView and Populate with Options From List</summary>
                /// <param name="dv"> </param>
                /// <param name="Options"> </param>
                /// <param name="ColName"> </param>
                public void Add_Populate_ComboBoxColumn(DataGridView dv, List<string> Options, string ColName = "Options")
                {
                    if (dv == null) { dv = gridSetup.currentGrid; }
                    if (dv != null) { gridSetup.currentGrid = dv; }

                    // add new column to gridview - ComboBox Column
                    DataGridViewComboBoxColumn comboCol = new DataGridViewComboBoxColumn();
                    comboCol.Name = ColName;  // set column name
                    comboCol.HeaderText = ColName;
                    comboCol.DataPropertyName = ColName;

                    //var list11 = new List<string>() { "OPTIONS", "Add To List Item", "Open File", "Open Dir", "Rename File", "Delete File" };
                    comboCol.DataSource = Options;

                    dv.Columns.Add(comboCol);  // assign to grid
                }

                /// <summary>Take selected grid image and create new bitmap in picturebox control</summary>
                /// <param name="dv"> </param>
                /// <param name="pic"> </param>
                public void Grid_Image_To_PictureBox(DataGridView dv, PictureBox pic)
                {
                    if (dv == null) { dv = gridSetup.currentGrid; }
                    if (dv != null) { gridSetup.currentGrid = dv; }

                    MemoryStream ms = new MemoryStream();
                    Bitmap img = (Bitmap)dv.CurrentRow.Cells[1].Value;
                    img.Save(ms, ImageFormat.Jpeg);
                    pic.Image = Image.FromStream(ms);
                }



                //####  Column Options  #####

                /// <summary>Expand column width to fill out entire grid</summary>
                /// <param name="dv"> </param>
                public void Fill_Column_Width(DataGridView dv)
                {
                    if (dv == null) { dv = gridSetup.currentGrid; }
                    if (dv != null) { gridSetup.currentGrid = dv; }

                    if (dv.InvokeRequired) { dv.BeginInvoke((MethodInvoker)delegate () { dv.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill; }); }
                    else { dv.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill; }

                    if (dv.InvokeRequired)
                    {
                        dv.BeginInvoke((MethodInvoker)delegate ()
                        {
                            dv.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
                        });
                    }
                    else
                    {
                        dv.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
                    }

                    try
                    {
                        DataGridViewTextBoxColumn subTitleColumn = new DataGridViewTextBoxColumn();
                        subTitleColumn.MinimumWidth = 50;
                        subTitleColumn.FillWeight = 100;

                        DataGridViewTextBoxColumn summaryColumn = new DataGridViewTextBoxColumn();
                        summaryColumn.MinimumWidth = 50;
                        summaryColumn.FillWeight = 200;

                        DataGridViewTextBoxColumn contentColumn = new DataGridViewTextBoxColumn();
                        contentColumn.MinimumWidth = 50;
                        contentColumn.FillWeight = 300;
                    }
                    catch
                    {
                        ahk.MsgBox("Grid cAtch");
                    }
                }

                /// <summary>Disable user ability to sort by clicking column header</summary>
                /// <param name="dv"> </param>
                public void Grid_Disable_Sort(DataGridView dv)
                {
                    if (dv == null) { dv = gridSetup.currentGrid; }
                    if (dv != null) { gridSetup.currentGrid = dv; }

                    foreach (DataGridViewColumn column in dv.Columns)
                    {
                        column.SortMode = DataGridViewColumnSortMode.NotSortable;
                    }
                }

                /// <summary>Set the column headers to bold text</summary>
                /// <param name="dv"> </param>
                public void Column_Header_Bold(DataGridView dv)
                {
                    if (dv == null) { dv = gridSetup.currentGrid; }
                    if (dv != null) { gridSetup.currentGrid = dv; }

                    // Set the column header style.
                    DataGridViewCellStyle columnHeaderStyle = new DataGridViewCellStyle();

                    columnHeaderStyle.BackColor = Color.Beige;
                    columnHeaderStyle.Font = new Font("Verdana", 10, FontStyle.Bold);
                    dv.ColumnHeadersDefaultCellStyle = columnHeaderStyle;
                }

                /// <summary>Set a column's values to italic text</summary>
                /// <param name="dv"> </param>
                /// <param name="ColNum"> </param>
                public void Column_Font_Italic(DataGridView dv, int ColNum)
                {
                    if (dv == null) { dv = gridSetup.currentGrid; }
                    if (dv != null) { gridSetup.currentGrid = dv; }

                    // Make the font italic for column #
                    dv.Columns[ColNum].DefaultCellStyle.Font = new Font(DataGridView.DefaultFont, FontStyle.Italic);
                }

                /// <summary>Set the text color + background color of a column in the grid</summary>
                /// <param name="dv"> </param>
                /// <param name="FontColor"> </param>
                /// <param name="BackColor"> </param>
                /// <param name="ColNum"> </param>
                public void Column_Color(DataGridView dv, Color FontColor, Color BackColor, int ColNum)
                {
                    if (dv == null) { dv = gridSetup.currentGrid; }
                    if (dv != null) { gridSetup.currentGrid = dv; }

                    dv.Columns[ColNum].DefaultCellStyle.ForeColor = FontColor;
                    dv.Columns[ColNum].DefaultCellStyle.BackColor = BackColor;
                }


                /// <summary>
                /// Reset All (or Specific) Column Colors Back to BlackText / WhiteBack
                /// </summary>
                /// <param name="dv">DataGridView to Alter</param>
                /// <param name="ColNum">Default ColumNum = -1 (All Columns) or Enter Specific Column To Alter</param>
                public void Column_ColorReset(DataGridView dv, int ColNum = -1)
                {
                    if (dv == null) { dv = gridSetup.currentGrid; }
                    if (dv != null) { gridSetup.currentGrid = dv; }

                    int columnCount = dv.Rows[0].Cells.Count;

                    if (ColNum == -1) // apply to all columns
                    {
                        for (int i = 0; i < columnCount; i++)
                        {
                            dv.Columns[i].DefaultCellStyle.ForeColor = Color.Black;
                            dv.Columns[i].DefaultCellStyle.BackColor = Color.White;
                        }
                    }

                    if (ColNum != -1) // apply to all columns
                    {
                        for (int i = 0; i < columnCount; i++)
                        {
                            if (i == ColNum)
                            {
                                dv.Columns[i].DefaultCellStyle.ForeColor = Color.Black;
                                dv.Columns[i].DefaultCellStyle.BackColor = Color.White;
                            }
                        }
                    }

                }




                /// <summary>
                /// Returns List of Column Widths (percentage of grid used) Per Column
                /// </summary>
                /// <param name="dv">DataGridView to Search</param>
                /// <returns>Returns list of Column Width Percentages</returns>
                public List<int> ColumnWidthPercentage_Get(DataGridView dv)
                {
                    if (dv == null) { dv = gridSetup.currentGrid; }
                    if (dv != null) { gridSetup.currentGrid = dv; }

                    List<int> colWidth = new List<int>();

                    int columnCount = dv.Rows[0].Cells.Count;
                    for (int i = 0; i < columnCount; i++)
                    {
                        DataGridViewColumn coll = dv.Columns[i];

                        if (coll.Visible)
                        {
                            int per = Column_Width_Percentage(dv, coll.HeaderText);
                            colWidth.Add(per);
                            //ahk.MsgBox("Col " + i + " = " + per.ToString());
                        }
                        if (!coll.Visible)
                        {
                            //ahk.MsgBox("Col " + i + " = " + 0);
                        }
                    }

                    return colWidth;
                }


                //#### Row Size #####

                /// <summary>Enable wordwrap for text in cells</summary>
                /// <param name="dv"> </param>
                public void Grid_WordWrap(DataGridView dv)
                {
                    if (dv == null) { dv = gridSetup.currentGrid; }
                    if (dv != null) { gridSetup.currentGrid = dv; }

                    dv.DefaultCellStyle.WrapMode = DataGridViewTriState.True;
                }

                /// <summary>Change the height of one / all rows in the grid</summary>
                /// <param name="dv"> </param>
                /// <param name="Height"> </param>
                /// <param name="Row"> </param>
                public void Row_Height(DataGridView dv, int Height = 30, int Row = -1)
                {
                    if (dv == null) { dv = gridSetup.currentGrid; }
                    if (dv != null) { gridSetup.currentGrid = dv; }

                    int r = 0;
                    foreach (DataGridViewRow dr in dv.Rows)  // loop through each row and set height
                    {
                        if (Row != -1) { if (r == Row) { dr.Height = Height; dr.MinimumHeight = 2; } } // only apply to single row num
                        if (Row == -1) { dr.Height = Height; } // apply to all rows

                        if (Row == 3) { if (r == Row) { dr.Height = Height; dr.MinimumHeight = 2; dr.DividerHeight = 1; } } // only apply to single row num
                        r++;
                    }
                }

                /// <summary>Indent text spacing in one / all rows</summary>
                /// <param name="dv"> </param>
                /// <param name="Tab"> </param>
                /// <param name="TopBottom"> </param>
                /// <param name="Row"> </param>
                public void Row_Padding_Indent(DataGridView dv, int Tab = 5, int TopBottom = 5, int Row = -1)
                {
                    if (dv == null) { dv = gridSetup.currentGrid; }
                    if (dv != null) { gridSetup.currentGrid = dv; }

                    int r = 0;
                    foreach (DataGridViewRow dr in dv.Rows)  // loop through each row and set height
                    {
                        if (Row != -1) { if (r == Row) { dv.Rows[r].DefaultCellStyle.Padding = new Padding(Tab, TopBottom, 0, TopBottom); } } // only apply to single row num
                        if (Row == -1) { dv.Rows[r].DefaultCellStyle.Padding = new Padding(Tab, TopBottom, 0, TopBottom); } // apply to all rows
                        r++;
                    }
                }

                /// <summary>Autosize row height to display all text in one row or all rows</summary>
                /// <param name="dv"> </param>
                /// <param name="Row"> </param>
                public void Row_AutoResize(DataGridView dv, int Row = -1)
                {
                    if (dv == null) { dv = gridSetup.currentGrid; }
                    if (dv != null) { gridSetup.currentGrid = dv; }

                    int r = 0;
                    foreach (DataGridViewRow dr in dv.Rows)  // loop through each row and set height
                    {
                        if (Row != -1) { if (r == Row) { dv.AutoResizeRow(r); } } // only apply to single row num
                        if (Row == -1) { dv.AutoResizeRow(r); } // apply to all rows
                        r++;
                    }
                }


                //###  Row Font  ###

                /// <summary>Set the text in one/all rows to Bold font</summary>
                /// <param name="dv"> </param>
                /// <param name="FontSize"> </param>
                /// <param name="Row"> </param>
                public void Row_Font_Bold(DataGridView dv, int FontSize = 10, int Row = -1)
                {
                    if (dv == null) { dv = gridSetup.currentGrid; }
                    if (dv != null) { gridSetup.currentGrid = dv; }

                    int r = 0;
                    foreach (DataGridViewRow dr in dv.Rows)  // loop through each row and set height
                    {
                        if (Row != -1) { if (r == Row) { dr.DefaultCellStyle.Font = new Font("Verdana", FontSize, FontStyle.Bold); } } // only apply to single row num
                        if (Row == -1) { dr.DefaultCellStyle.Font = new Font("Verdana", FontSize, FontStyle.Bold); } // apply to all rows
                        r++;
                    }
                }

                /// <summary>Change Row Font - UNFINISHED</summary>
                /// <param name="dv"> </param>
                /// <param name="TextColor"> </param>
                /// <param name="Row"> </param>
                public void Row_Font(DataGridView dv, Color TextColor, int Row = -1)
                {
                    if (dv == null) { dv = gridSetup.currentGrid; }
                    if (dv != null) { gridSetup.currentGrid = dv; }

                    int r = 0;
                    foreach (DataGridViewRow dr in dv.Rows)  // loop through each row and set height
                    {
                        //if (Row != -1) 
                        dr.DefaultCellStyle.Font = new Font("Verdana", 10, FontStyle.Bold);
                        //dv.CurrentRow.DefaultCellStyle.Font = new Font("Verdana", 10, FontStyle.Bold);
                    }
                }


                //### Row Color ###

                /// <summary>Change Text Color + Background Color of One/All rows in the DataGridView | Row -1 = ALL ROWS</summary>
                /// <param name="dv"> </param>
                /// <param name="FontColor"> </param>
                /// <param name="BackColor"> </param>
                /// <param name="Row"> </param>
                public void Row_Color(DataGridView dv, Color FontColor, Color BackColor, int Row = -1)
                {
                    if (dv == null) { dv = gridSetup.currentGrid; }
                    if (dv != null) { gridSetup.currentGrid = dv; }

                    int r = 0;
                    foreach (DataGridViewRow dr in dv.Rows)  // loop through each row and set height
                    {
                        if (Row != -1) { if (r == Row) { dv.Rows[r].DefaultCellStyle.ForeColor = FontColor; dv.Rows[r].DefaultCellStyle.BackColor = BackColor; } } // only apply to single row num
                        if (Row == -1) { dv.Rows[r].DefaultCellStyle.ForeColor = FontColor; dv.Rows[r].DefaultCellStyle.BackColor = BackColor; } // apply to all rows
                        r++;
                    }
                }

                /// <summary>Set the text color of one/all rows in the DataGridView</summary>
                /// <param name="dv"> </param>
                /// <param name="color"> </param>
                /// <param name="Row"> </param>
                public void Row_Text_Color(DataGridView dv, Color color, int Row = -1)
                {
                    if (dv == null) { dv = gridSetup.currentGrid; }
                    if (dv != null) { gridSetup.currentGrid = dv; }

                    int r = 0;
                    foreach (DataGridViewRow dr in dv.Rows)  // loop through each row and set height
                    {
                        if (Row != -1) { if (r == Row) { dv.Rows[r].DefaultCellStyle.ForeColor = color; } } // only apply to single row num
                        if (Row == -1) { dv.Rows[r].DefaultCellStyle.ForeColor = color; } // apply to all rows
                        r++;
                    }
                }

                /// <summary>Set the background color of one/all rows in the DataGridView</summary>
                /// <param name="dv"> </param>
                /// <param name="color"> </param>
                /// <param name="Row"> </param>
                public void Row_Background_Color(DataGridView dv, Color color, int Row = -1)
                {
                    if (dv == null) { dv = gridSetup.currentGrid; }
                    if (dv != null) { gridSetup.currentGrid = dv; }

                    int r = 0;
                    foreach (DataGridViewRow dr in dv.Rows)  // loop through each row and set height
                    {
                        if (Row != -1) { if (r == Row) { dv.Rows[r].DefaultCellStyle.BackColor = color; } } // only apply to single row num
                        if (Row == -1) { dv.Rows[r].DefaultCellStyle.BackColor = color; } // apply to all rows
                        r++;
                    }
                }

                /// <summary>Set the color of the selected DataGridView row</summary>
                /// <param name="dv"> </param>
                /// <param name="TextColor"> </param>
                /// <param name="BackColor"> </param>
                /// <param name="FullRowSelect"> </param>
                public void Row_Selected_Color(DataGridView dv, Color TextColor, Color BackColor, bool FullRowSelect = true)
                {
                    if (dv == null) { dv = gridSetup.currentGrid; }
                    if (dv != null) { gridSetup.currentGrid = dv; }

                    // Set the selection background color for all the cells.
                    dv.DefaultCellStyle.SelectionBackColor = BackColor;
                    dv.DefaultCellStyle.SelectionForeColor = TextColor;

                    if (FullRowSelect)
                    {
                        dv.SelectionMode = DataGridViewSelectionMode.FullRowSelect;  // set selection mode to full row select
                    }
                }

                /// <summary>Alternate between 2 different row colors</summary>
                /// <param name="dv"> </param>
                /// <param name="Color1"> </param>
                /// <param name="Color2"> </param>
                public void Row_Alternative_RowColor(DataGridView dv, Color Color1, Color Color2)
                {
                    if (dv == null) { dv = gridSetup.currentGrid; }
                    if (dv != null) { gridSetup.currentGrid = dv; }

                    // Set RowHeadersDefaultCellStyle.SelectionBackColor so that its default 
                    // value won't override DataGridView.DefaultCellStyle.SelectionBackColor.
                    dv.RowHeadersDefaultCellStyle.SelectionBackColor = Color.Empty;

                    // Set the background color for all rows and for alternating rows.  
                    // The value for alternating rows overrides the value for all rows. 
                    dv.RowsDefaultCellStyle.BackColor = Color1;
                    dv.AlternatingRowsDefaultCellStyle.BackColor = Color2;
                }

        */
        #endregion


        #region === DataGridView: OnClick Events ===
        /*
                /// <summary>
                /// Stores data about dataGridView when selected
                /// </summary>
                public struct gridInfo
                {
                    public int CurrentColNum { get; set; }
                    public int CurrentRowNum { get; set; }
                    public string CellValue { get; set; }
                    public List<string> GridControlList { get; set; }
                }

                public static gridInfo clickedGrid = new gridInfo();

                public void dataGridView_CellClick(object sender, DataGridViewCellEventArgs e)
                {
                    if (e.RowIndex == -1 || e.ColumnIndex == -1) { return; }  // do nothing if row isn't within range
                    DataGridView dv = (DataGridView)sender;  // DataGridView control clicked

                    if (dv == null) { dv = gridSetup.currentGrid; }
                    if (dv != null) { gridSetup.currentGrid = dv; }

                    clickedGrid = new gridInfo();

                    clickedGrid.CurrentRowNum = e.RowIndex;
                    clickedGrid.CurrentColNum = e.ColumnIndex;

                    try
                    {
                        // value selected in gridview
                        clickedGrid.CellValue = dv.Rows[clickedGrid.CurrentRowNum].Cells[clickedGrid.CurrentColNum].Value.ToString();
                        sb.StatusBar("Selected " + clickedGrid.CellValue, 1);
                        sb.StatusBar("Row " + clickedGrid.CurrentRowNum + " | Column " + clickedGrid.CurrentColNum, 2);
                    }
                    catch
                    {
                    }

                    //ahk.MsgBox(clickedGrid.CellValue); 
                }

                private void dataGridView_RowEnter(object sender, DataGridViewCellEventArgs e)
                {
                    //if (e.RowIndex == -1 || e.ColumnIndex == -1) { return; }  // do nothing if row isn't within range

                    //// Extract Selected Cell Contents in Row

                    //DataGridView dv = (DataGridView)sender;
                    //int RowNum = e.RowIndex;
                    //int ColNum = e.ColumnIndex;

                    //string RowValue = "";

                    //// loop through each Column, pull value for each item in row
                    //int columnCount = dv.Rows[0].Cells.Count;
                    //for (int i = 0; i < columnCount; i++)
                    //{
                    //    DataGridViewColumn col = dv.Columns[i];
                    //    string CellValue = dv.Rows[RowNum].Cells[i].Value.ToString();
                    //    RowValue = CellValue + "," + RowValue;
                    //}


                    ////string CellValue = dv.Rows[RowNum].Cells[ColNum].Value.ToString();

                    //ahk.MsgBox(RowValue); 
                }


        */
        #endregion


        #region #### GRID MENUS ########################################
        /*
                public void Attach_GridMenus(MenuStrip menuStrip)
                {
                    // assign global menustrip if not assigned yet
                    if (_GridControl.gridSetup.currentMenu == null && menuStrip != null) { _GridControl.gridSetup.currentMenu = menuStrip; }

                    // build and attach cell border style menu
                    GridMenuBasics_Attach(CurrentGrid(), menuStrip, GridMenuBasics_ClickEvent, "GridMenuBasics", false);
                    CellBorderStyleMenu_Attach(menuStrip);
                    AutoSizeRows_Attach(menuStrip);
                    ColumnHeaderBorder_Attach(menuStrip);
                    SelectionMode_Attach(menuStrip);
                    AddColumns_Attach(menuStrip);

                    // check cell border style menu based on existing grid settings
                    GridMenuCheckValues(CurrentGrid(), CurrentMenu());  // check options on menu based on gridview's current settings
                }


                #region === Cell Border Style Menu + Actions ====

                // build and attach CellBorderStyle Grid Menu
                public void CellBorderStyleMenu_Attach(MenuStrip menuStrip)
                {
                    // attach to parent menu item (leave name blank to not create subgroup)
                    if (menu == null) { menu = new _MenuControl(); }

                    ToolStripMenuItem gridMenu = menu.List_To_Menu(CellBorderStyle_MenuItems(), menuStrip, CellBorderStyle_MenuItem_Click, "CellBorderStyle") as ToolStripMenuItem;
                }

                // returns list of GridView Options
                public List<string> CellBorderStyle_MenuItems()
                {
                    List<string> menuItems = new List<string>();

                    menuItems.Add("Raised");
                    menuItems.Add("RaisedHorizontal");
                    menuItems.Add("RaisedVertical");
                    menuItems.Add("Single");
                    menuItems.Add("SingleHorizontal");
                    menuItems.Add("SingleVertical");
                    menuItems.Add("Sunken");
                    menuItems.Add("SunkenHorizontal");
                    menuItems.Add("SunkenVertical");
                    menuItems.Add("None");
                    return menuItems;
                }


                // loop through list of menu items and uncheck all items
                public void UnCheck_MenuItemsList(List<string> menuItems)
                {
                    //List<string> menuItems = CellBorderStyle_MenuItems();

                    foreach (string item in menuItems)
                    {
                        ToolStripMenuItem CurrentMenuItem = menu.Return_ToolStripItem(CurrentMenu(), item);
                        CurrentMenuItem.Checked = false;
                    }
                }

                // check options on menu based on gridview's current settings
                public void GridMenuCheckValues(DataGridView dv, MenuStrip menuStrip)
                {
                    if (dv == null) { dv = gridSetup.currentGrid; }
                    if (dv != null) { gridSetup.currentGrid = dv; }

                    List<string> TopMenuItems = menu.Menu_Item_List(menuStrip, true);

                    foreach (string topMenuItem in TopMenuItems)
                    {

                        if (topMenuItem == "CellBorderStyle")
                        {

                            UnCheckCellBorderStyleMenu(); // uncheck all items on CellBorderStyle Menu

                            // "CellBorderStyle"
                            List<string> MenuItems = menu.Menu_Item_List(menuStrip, false, topMenuItem);  // list of CellBorderStyle Menu Options

                            foreach (string item in MenuItems)
                            {
                                //ahk.MsgBox(item);
                                ToolStripMenuItem CurrentMenuItem = menu.Return_ToolStripItem(menuStrip, item);
                                string Style = CurrentMenuItem.Text;

                                //UnCheck_MenuItemsList(CellBorderStyle_MenuItems()); // uncheck all items on CellBorderStyle Menu

                                if (Style == "Raised") { if (dv.CellBorderStyle == DataGridViewCellBorderStyle.Raised) { CurrentMenuItem.Checked = true; } }
                                if (Style == "RaisedHorizontal") { if (dv.CellBorderStyle == DataGridViewCellBorderStyle.RaisedHorizontal) { CurrentMenuItem.Checked = true; } }
                                if (Style == "RaisedVertical") { if (dv.CellBorderStyle == DataGridViewCellBorderStyle.RaisedVertical) { CurrentMenuItem.Checked = true; } }
                                if (Style == "Single") { if (dv.CellBorderStyle == DataGridViewCellBorderStyle.Single) { CurrentMenuItem.Checked = true; } }
                                if (Style == "SingleHorizontal") { if (dv.CellBorderStyle == DataGridViewCellBorderStyle.SingleHorizontal) { CurrentMenuItem.Checked = true; } }
                                if (Style == "SingleVertical") { if (dv.CellBorderStyle == DataGridViewCellBorderStyle.SingleVertical) { CurrentMenuItem.Checked = true; } }
                                if (Style == "Sunken") { if (dv.CellBorderStyle == DataGridViewCellBorderStyle.Sunken) { CurrentMenuItem.Checked = true; } }
                                if (Style == "SunkenHorizontal") { if (dv.CellBorderStyle == DataGridViewCellBorderStyle.SunkenHorizontal) { CurrentMenuItem.Checked = true; } }
                                if (Style == "SunkenVertical") { if (dv.CellBorderStyle == DataGridViewCellBorderStyle.SunkenVertical) { CurrentMenuItem.Checked = true; } }
                                if (Style == "None") { if (dv.CellBorderStyle == DataGridViewCellBorderStyle.None) { CurrentMenuItem.Checked = true; } }
                            }
                        }
                    }
                }

                private void CellBorderStyle_MenuItem_Click(object sender, EventArgs e)
                {
                    ToolStripMenuItem selected = (ToolStripMenuItem)sender;

                    // uncheck all children under "CellBorderStyle" parent
                    ToolStripMenuItem selectedParent = menu.Return_ToolStripItem(CurrentMenu(), "CellBorderStyle");
                    List<ToolStripMenuItem> children = menu.Menu_ToolStrip_Items(selectedParent);
                    foreach (ToolStripMenuItem child in children) { child.Checked = false; }

                    //ahk.MsgBox(selected.Text); 
                    CellBorderStyle(CurrentGrid(), selected);

                    //GridMenuCheckValues(CurrentGrid(), CurrentMenu());  // check options on menu based on gridview's current settings
                }

                public void CellBorderStyle(DataGridView dv, ToolStripMenuItem clicked)
                {
                    if (dv == null) { dv = gridSetup.currentGrid; }
                    if (dv != null) { gridSetup.currentGrid = dv; }

                    string Style = clicked.Text;

                    UnCheckCellBorderStyleMenu(); // uncheck all items on CellBorderStyle Menu

                    if (Style == "Raised") { dv.CellBorderStyle = DataGridViewCellBorderStyle.Raised; CheckMenuItem(clicked, true); }
                    if (Style == "RaisedHorizontal") { dv.CellBorderStyle = DataGridViewCellBorderStyle.RaisedHorizontal; CheckMenuItem(clicked, true); }
                    if (Style == "RaisedVertical") { dv.CellBorderStyle = DataGridViewCellBorderStyle.RaisedVertical; CheckMenuItem(clicked, true); }
                    if (Style == "Single") { dv.CellBorderStyle = DataGridViewCellBorderStyle.Single; CheckMenuItem(clicked, true); }
                    if (Style == "SingleHorizontal") { dv.CellBorderStyle = DataGridViewCellBorderStyle.SingleHorizontal; CheckMenuItem(clicked, true); }
                    if (Style == "SingleVertical") { dv.CellBorderStyle = DataGridViewCellBorderStyle.SingleVertical; CheckMenuItem(clicked, true); }
                    if (Style == "Sunken") { dv.CellBorderStyle = DataGridViewCellBorderStyle.Sunken; CheckMenuItem(clicked, true); }
                    if (Style == "SunkenHorizontal") { dv.CellBorderStyle = DataGridViewCellBorderStyle.SunkenHorizontal; CheckMenuItem(clicked, true); }
                    if (Style == "SunkenVertical") { dv.CellBorderStyle = DataGridViewCellBorderStyle.SunkenVertical; CheckMenuItem(clicked, true); }
                    if (Style == "None") { dv.CellBorderStyle = DataGridViewCellBorderStyle.None; CheckMenuItem(clicked, true); }
                }


                public void UnCheckCellBorderStyleMenu()
                {
                    // uncheck all children under "CellBorderStyle" parent
                    ToolStripMenuItem selectedParent = menu.Return_ToolStripItem(CurrentMenu(), "CellBorderStyle");
                    List<ToolStripMenuItem> children = menu.Menu_ToolStrip_Items(selectedParent);
                    foreach (ToolStripMenuItem child in children) { child.Checked = false; }
                }

                public void CheckMenuItem(ToolStripMenuItem item, bool Checked = true)
                {
                    if (item != null)
                    {
                        item.Checked = Checked;
                    }
                }

        */
        #endregion


        #region === AutoSizeGrid ===
        /*

                // returns list of GridView Options
                public List<string> AutoSizeRows_MenuItems()
                {
                    List<string> menuItems = new List<string>();

                    menuItems.Add("AllCells");
                    menuItems.Add("AllHeaders");
                    menuItems.Add("AllCellsExceptHeaders");
                    menuItems.Add("");
                    menuItems.Add("DisplayedCells");
                    menuItems.Add("DisplayedHeaders");
                    menuItems.Add("DisplayedCellsExceptHeaders");
                    menuItems.Add("");
                    menuItems.Add("None");
                    return menuItems;
                }

                // build and attach CellBorderStyle Grid Menu
                public void AutoSizeRows_Attach(MenuStrip menuStrip)
                {
                    // attach to parent menu item (leave name blank to not create subgroup)
                    ToolStripMenuItem gridMenu = menu.List_To_Menu(AutoSizeRows_MenuItems(), menuStrip, AutoSizeRows_MenuItem_Click, "AutoSizeRows") as ToolStripMenuItem;
                }

                private void AutoSizeRows_MenuItem_Click(object sender, EventArgs e)
                {
                    ToolStripMenuItem selected = (ToolStripMenuItem)sender;

                    // uncheck all children under "AutoSizeRows" parent
                    ToolStripMenuItem selectedParent = menu.Return_ToolStripItem(CurrentMenu(), "AutoSizeRows");
                    List<ToolStripMenuItem> children = menu.Menu_ToolStrip_Items(selectedParent);
                    foreach (ToolStripMenuItem child in children) { child.Checked = false; }

                    //ahk.MsgBox(selected.Text); 
                    AutoSizeRows(CurrentGrid(), selected);

                    //GridMenuCheckValues(CurrentGrid(), CurrentMenu());  // check options on menu based on gridview's current settings
                }

                public void AutoSizeRows(DataGridView dv, ToolStripMenuItem clicked)
                {
                    if (dv == null) { dv = gridSetup.currentGrid; }
                    if (dv != null) { gridSetup.currentGrid = dv; }

                    string Style = clicked.Text;

                    // uncheck all children under "AutoSizeRows" parent
                    ToolStripMenuItem selectedParent = menu.Return_ToolStripItem(CurrentMenu(), "AutoSizeRows");
                    List<ToolStripMenuItem> children = menu.Menu_ToolStrip_Items(selectedParent);
                    foreach (ToolStripMenuItem child in children) { child.Checked = false; }

                    if (Style == "AllCells") { dv.AutoSizeRowsMode = DataGridViewAutoSizeRowsMode.AllCells; CheckMenuItem(clicked, true); }
                    if (Style == "AllHeaders") { dv.AutoSizeRowsMode = DataGridViewAutoSizeRowsMode.AllHeaders; CheckMenuItem(clicked, true); }
                    if (Style == "AllCellsExceptHeaders") { dv.AutoSizeRowsMode = DataGridViewAutoSizeRowsMode.AllCellsExceptHeaders; CheckMenuItem(clicked, true); }
                    if (Style == "DisplayedCells") { dv.AutoSizeRowsMode = DataGridViewAutoSizeRowsMode.DisplayedCells; CheckMenuItem(clicked, true); }
                    if (Style == "DisplayedHeaders") { dv.AutoSizeRowsMode = DataGridViewAutoSizeRowsMode.DisplayedHeaders; CheckMenuItem(clicked, true); }
                    if (Style == "DisplayedCellsExceptHeaders") { dv.AutoSizeRowsMode = DataGridViewAutoSizeRowsMode.DisplayedCellsExceptHeaders; CheckMenuItem(clicked, true); }
                    if (Style == "None") { dv.AutoSizeRowsMode = DataGridViewAutoSizeRowsMode.None; CheckMenuItem(clicked, true); }
                }


        */
        #endregion


        #region === ColumnHeaderBorder ===
        /*

                // returns list of GridView Options
                public List<string> ColumnHeaderBorder_MenuItems()
                {
                    List<string> menuItems = new List<string>();

                    //menuItems.Add("Custom");
                    menuItems.Add("Raised");
                    menuItems.Add("Single");
                    menuItems.Add("Sunken");
                    menuItems.Add("");
                    menuItems.Add("None");
                    return menuItems;
                }

                // build and attach CellBorderStyle Grid Menu
                public void ColumnHeaderBorder_Attach(MenuStrip menuStrip)
                {
                    // attach to parent menu item (leave name blank to not create subgroup)
                    ToolStripMenuItem gridMenu = menu.List_To_Menu(ColumnHeaderBorder_MenuItems(), menuStrip, ColumnHeaderBorder_MenuItem_Click, "ColumnHeaderBorder") as ToolStripMenuItem;
                }

                private void ColumnHeaderBorder_MenuItem_Click(object sender, EventArgs e)
                {
                    ToolStripMenuItem selected = (ToolStripMenuItem)sender;

                    // uncheck all children under "AutoSizeRows" parent
                    ToolStripMenuItem selectedParent = menu.Return_ToolStripItem(CurrentMenu(), "ColumnHeaderBorder");
                    List<ToolStripMenuItem> children = menu.Menu_ToolStrip_Items(selectedParent);
                    foreach (ToolStripMenuItem child in children) { child.Checked = false; }

                    //ahk.MsgBox(selected.Text); 
                    ColumnHeaderBorder(CurrentGrid(), selected);

                    //GridMenuCheckValues(CurrentGrid(), CurrentMenu());  // check options on menu based on gridview's current settings
                }

                public void ColumnHeaderBorder(DataGridView dv, ToolStripMenuItem clicked)
                {
                    if (dv == null) { dv = gridSetup.currentGrid; }
                    if (dv != null) { gridSetup.currentGrid = dv; }

                    string Style = clicked.Text;

                    // uncheck all children under "AutoSizeRows" parent
                    ToolStripMenuItem selectedParent = menu.Return_ToolStripItem(CurrentMenu(), "ColumnHeaderBorder");
                    List<ToolStripMenuItem> children = menu.Menu_ToolStrip_Items(selectedParent);
                    foreach (ToolStripMenuItem child in children) { child.Checked = false; }

                    //ColumnHeaderStyle.Clickable

                    if (Style == "Border Fixed3D") { dv.BorderStyle = BorderStyle.Fixed3D; CheckMenuItem(clicked, true); }
                    if (Style == "Border FixedSingle") { dv.BorderStyle = BorderStyle.FixedSingle; CheckMenuItem(clicked, true); }
                    if (Style == "Border None") { dv.BorderStyle = BorderStyle.None; CheckMenuItem(clicked, true); }




                    // Column Header Border

                    //if (Style == "Custom") { dv.ColumnHeadersBorderStyle = DataGridViewHeaderBorderStyle.Custom; CheckMenuItem(clicked, true); }
                    if (Style == "Raised") { dv.ColumnHeadersBorderStyle = DataGridViewHeaderBorderStyle.Raised; CheckMenuItem(clicked, true); }
                    if (Style == "Single") { dv.ColumnHeadersBorderStyle = DataGridViewHeaderBorderStyle.Single; CheckMenuItem(clicked, true); }
                    if (Style == "Sunken") { dv.ColumnHeadersBorderStyle = DataGridViewHeaderBorderStyle.Sunken; CheckMenuItem(clicked, true); }
                    if (Style == "None") { dv.ColumnHeadersBorderStyle = DataGridViewHeaderBorderStyle.None; CheckMenuItem(clicked, true); }
                }


        */
        #endregion


        #region === SelectionMode ===
        /*

                // returns list of GridView Options
                public List<string> SelectionMode_MenuItems()
                {
                    List<string> menuItems = new List<string>();

                    menuItems.Add("CellSelect");
                    menuItems.Add("ColumnHeaderSelect");
                    menuItems.Add("FullColumnSelect");
                    menuItems.Add("FullRowSelect");
                    menuItems.Add("RowHeaderSelect");
                    return menuItems;
                }

                // build and attach SelectionMode Grid Menu
                public void SelectionMode_Attach(MenuStrip menuStrip)
                {
                    // attach to parent menu item (leave name blank to not create subgroup)
                    ToolStripMenuItem gridMenu = menu.List_To_Menu(SelectionMode_MenuItems(), menuStrip, SelectionMode_MenuItem_Click, "SelectionMode") as ToolStripMenuItem;
                }

                private void SelectionMode_MenuItem_Click(object sender, EventArgs e)
                {
                    ToolStripMenuItem selected = (ToolStripMenuItem)sender;

                    // uncheck all children under "AutoSizeRows" parent
                    ToolStripMenuItem selectedParent = menu.Return_ToolStripItem(CurrentMenu(), "SelectionMode");
                    List<ToolStripMenuItem> children = menu.Menu_ToolStrip_Items(selectedParent);
                    foreach (ToolStripMenuItem child in children) { child.Checked = false; }

                    //ahk.MsgBox(selected.Text); 
                    SelectionMode(CurrentGrid(), selected);

                    //GridMenuCheckValues(CurrentGrid(), CurrentMenu());  // check options on menu based on gridview's current settings
                }

                public void SelectionMode(DataGridView dv, ToolStripMenuItem clicked)
                {
                    if (dv == null) { dv = gridSetup.currentGrid; }
                    if (dv != null) { gridSetup.currentGrid = dv; }

                    string Style = clicked.Text;

                    // uncheck all children under "AutoSizeRows" parent
                    ToolStripMenuItem selectedParent = menu.Return_ToolStripItem(CurrentMenu(), "SelectionMode");
                    List<ToolStripMenuItem> children = menu.Menu_ToolStrip_Items(selectedParent);
                    foreach (ToolStripMenuItem child in children) { child.Checked = false; }

                    ////ColumnHeaderStyle.Clickable

                    //if (Style == "Border Fixed3D") { dv.BorderStyle = BorderStyle.Fixed3D; CheckMenuItem(clicked, true); }
                    //if (Style == "Border FixedSingle") { dv.BorderStyle = BorderStyle.FixedSingle; CheckMenuItem(clicked, true); }
                    //if (Style == "Border None") { dv.BorderStyle = BorderStyle.None; CheckMenuItem(clicked, true); }

                    // Column Header Border


                    //addressBookGrid.Columns[0].SortMode = DataGridViewColumnSortMode.Automatic;
                    //addressBookGrid.Columns[1].SortMode = DataGridViewColumnSortMode.Automatic;



                    if (Style == "ColumnSortMode - Automatic") { foreach (DataGridViewColumn column in dv.Columns) { column.SortMode = DataGridViewColumnSortMode.Automatic; CheckMenuItem(clicked, true); } }
                    if (Style == "ColumnSortMode - NotSortable") { foreach (DataGridViewColumn column in dv.Columns) { column.SortMode = DataGridViewColumnSortMode.NotSortable; CheckMenuItem(clicked, true); } }
                    if (Style == "ColumnSortMode - Programmatic") { foreach (DataGridViewColumn column in dv.Columns) { column.SortMode = DataGridViewColumnSortMode.Programmatic; CheckMenuItem(clicked, true); } }



                    if (Style == "FullRowSelect")
                    {
                        ColumnSortMode("Automatic"); // set default column sort mode
                        dv.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect; CheckMenuItem(clicked, true);
                    }
                    if (Style == "CellSelect")
                    {
                        ColumnSortMode("Automatic"); // set default column sort mode
                        dv.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.CellSelect; CheckMenuItem(clicked, true);
                    }
                    if (Style == "ColumnHeaderSelect")
                    {
                        ColumnSortMode("NotSortable"); // must set column sort mode to not sortable in this mode
                        dv.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.ColumnHeaderSelect; CheckMenuItem(clicked, true);
                    }
                    if (Style == "FullColumnSelect")
                    {

                        ColumnSortMode("NotSortable"); // must set column sort mode to not sortable in this mode
                        dv.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullColumnSelect; CheckMenuItem(clicked, true);
                    }
                    if (Style == "RowHeaderSelect")
                    {
                        ColumnSortMode("Automatic"); // set default column sort mode
                        dv.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.RowHeaderSelect; CheckMenuItem(clicked, true);
                    }
                }



                public void ColumnSortMode(string Mode = "Automatic")
                {
                    DataGridView dv = CurrentGrid();

                    if (Mode.Contains("Automatic"))
                    {
                        foreach (DataGridViewColumn column in dv.Columns)
                        {
                            // can't switch column sort mode to automatic when fullcolumnselect = true
                            if (dv.SelectionMode == System.Windows.Forms.DataGridViewSelectionMode.FullColumnSelect || dv.SelectionMode == System.Windows.Forms.DataGridViewSelectionMode.ColumnHeaderSelect)
                            {
                                dv.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.CellSelect;
                            }
                            column.SortMode = DataGridViewColumnSortMode.Automatic;
                        }
                    }
                    if (Mode.Contains("NotSortable")) { foreach (DataGridViewColumn column in dv.Columns) { column.SortMode = DataGridViewColumnSortMode.NotSortable; } }
                    if (Mode.Contains("Programmatic")) { foreach (DataGridViewColumn column in dv.Columns) { column.SortMode = DataGridViewColumnSortMode.Programmatic; } }
                }
        */
        #endregion


        #region === AddColumns Menu ===
        /*

                // returns list of GridView Options
                public List<string> AddColumns_MenuItems()
                {
                    List<string> menuItems = new List<string>();

                    menuItems.Add("Add Text Column");
                    menuItems.Add("Add Checkbox Column");
                    menuItems.Add("Add ComboBox Column");
                    menuItems.Add("Add Button Column");
                    menuItems.Add("Add Link Column");
                    menuItems.Add("Add Image Column");
                    return menuItems;
                }

                // build and attach AddColumns Grid Menu
                public void AddColumns_Attach(MenuStrip menuStrip)
                {
                    // attach to parent menu item (leave name blank to not create subgroup)
                    ToolStripMenuItem gridMenu = menu.List_To_Menu(AddColumns_MenuItems(), menuStrip, AddColumns_MenuItem_Click, "AddColumns") as ToolStripMenuItem;
                }

                private void AddColumns_MenuItem_Click(object sender, EventArgs e)
                {
                    ToolStripMenuItem selected = (ToolStripMenuItem)sender;

                    // uncheck all children under "AutoSizeRows" parent
                    ToolStripMenuItem selectedParent = menu.Return_ToolStripItem(CurrentMenu(), "AddColumns");
                    List<ToolStripMenuItem> children = menu.Menu_ToolStrip_Items(selectedParent);
                    foreach (ToolStripMenuItem child in children) { child.Checked = false; }

                    //ahk.MsgBox(selected.Text); 
                    AddColumns(CurrentGrid(), selected);

                    //GridMenuCheckValues(CurrentGrid(), CurrentMenu());  // check options on menu based on gridview's current settings
                }

                public void AddColumns(DataGridView dv, ToolStripMenuItem clicked)
                {
                    if (dv == null) { dv = gridSetup.currentGrid; }
                    if (dv != null) { gridSetup.currentGrid = dv; }

                    string Style = clicked.Text;

                    //// uncheck all children under "AddColumns" parent
                    //ToolStripMenuItem selectedParent = menu.Return_ToolStripItem(CurrentMenu(), "AddColumns");
                    //List<ToolStripMenuItem> children = menu.Menu_ToolStrip_Items(selectedParent);
                    //foreach (ToolStripMenuItem child in children) { child.Checked = false; }


                    if (Style == "Add Text Column") { Column_Add_TextColumn(CurrentGrid(), "TextBoxes"); CheckMenuItem(clicked, true); }
                    if (Style == "Add Checkbox Column") { Column_Add_CheckBoxColumn(CurrentGrid(), "CheckBox"); CheckMenuItem(clicked, true); }
                    if (Style == "Add ComboBox Column") { Column_Add_ComboBoxColumn(CurrentGrid(), "Combo"); CheckMenuItem(clicked, true); }
                    if (Style == "Add Button Column") { Column_Add_ButtonColumn(CurrentGrid(), "Buttons"); CheckMenuItem(clicked, true); }
                    if (Style == "Add Link Column") { Column_Add_LinkColumn(CurrentGrid(), "Links"); CheckMenuItem(clicked, true); }
                    if (Style == "Add Image Column") { Column_Add_ImageColumn(CurrentGrid(), "Images"); CheckMenuItem(clicked, true); }
                }


        */
        #endregion


        #region === GridMenuBasics ===
        /*

                //#### build / attach basic gridview menu options (must assign datagrid name in GridMenu_ClickEvent) ####


                /// <summary>Build / attach basic gridview menu options (must assign datagrid name in GridMenu_ClickEvent)</summary>
                /// <param name="dv"> </param>
                /// <param name="MenuOrItem"> </param>
                /// <param name="ClickEventFunction"> </param>
                /// <param name="NewMenuName"> </param>
                /// <param name="Rebuild"> </param>
                public void GridMenuBasics_Attach(DataGridView dv, object MenuOrItem, EventHandler ClickEventFunction, string NewMenuName = "GridView", bool Rebuild = false)
                {
                    if (dv == null) { dv = gridSetup.currentGrid; }
                    if (dv != null) { gridSetup.currentGrid = dv; }

                    _MenuControl menu = new _MenuControl();

                    // populate list of datagrid options available from menu strip options
                    List<string> DataGridView_Menu_Options = new List<string>();
                    DataGridView_Menu_Options.Add("Check All");
                    DataGridView_Menu_Options.Add("UnCheck All");
                    DataGridView_Menu_Options.Add("Background Color");

                    List<string> DataGridView_Menu_Options_Misc = new List<string>();
                    DataGridView_Menu_Options_Misc.Add("Clear Grid");
                    DataGridView_Menu_Options_Misc.Add("Show Grid");
                    DataGridView_Menu_Options_Misc.Add("Hide Grid");
                    DataGridView_Menu_Options_Misc.Add("Toggle Grid");
                    DataGridView_Menu_Options_Misc.Add("Stretch Columns");
                    DataGridView_Menu_Options_Misc.Add("AutoSize Columns");
                    DataGridView_Menu_Options_Misc.Add("Check All");
                    DataGridView_Menu_Options_Misc.Add("UnCheck All");
                    DataGridView_Menu_Options_Misc.Add("Checked Row Numbers");
                    DataGridView_Menu_Options_Misc.Add("Checked Row Values");
                    DataGridView_Menu_Options_Misc.Add("Grid Info");
                    DataGridView_Menu_Options_Misc.Add("Save Column Settings");
                    DataGridView_Menu_Options_Misc.Add("Load Column Settings");

                    // create top level menu item to attach sub items to MenuStrip
                    ToolStripMenuItem parentMenuItem = new ToolStripMenuItem();

                    if (MenuOrItem is MenuStrip)
                    {
                        if (Rebuild) { menu.Menu_Remove(MenuOrItem as MenuStrip, NewMenuName); }

                        parentMenuItem = menu.Add_Menu_Item(MenuOrItem as MenuStrip, NewMenuName, true);
                    }

                    if (MenuOrItem is ToolStripMenuItem)
                    {
                        parentMenuItem = menu.Add_Sub_Menu_Item(MenuOrItem as ToolStripMenuItem, NewMenuName, ClickEventFunction);
                    }


                    // attach columns to menu item (in "Grid Columns" Sub group)
                    ToolStripMenuItem gridMenuCols = menu.Grid_ColumnNames_To_Menu(dv, parentMenuItem, ClickEventFunction, "Grid Columns") as ToolStripMenuItem;  // toggle displaying columns from menu

                    // attach columns to drop down list from main menu strip 
                    //ToolStripMenuItem gridMenuCols = menu.Grid_ColumnNames_To_Menu(dataGridView1, menuStrip1, ClickEventFunction, "Grid Columns") as ToolStripMenuItem;  // toggle displaying columns from menu


                    // full list of datagrid options 
                    ToolStripMenuItem gridMenuMisc = menu.List_To_Menu(DataGridView_Menu_Options_Misc, parentMenuItem, ClickEventFunction, "Misc") as ToolStripMenuItem;


                    // attach to parent menu item (leave name blank to not create subgroup)
                    ToolStripMenuItem gridMenu = menu.List_To_Menu(DataGridView_Menu_Options, parentMenuItem, ClickEventFunction, "") as ToolStripMenuItem;

                }

                /// <summary>Click Events for GridMenu Basic Options</summary>
                /// <param name="sender"> </param>
                /// <param name="e"> </param>
                public void GridMenuBasics_ClickEvent(object sender, EventArgs e)
                {
                    DataGridView dv = null;
                    if (dv == null) { dv = gridSetup.currentGrid; }
                    if (dv != null) { gridSetup.currentGrid = dv; }

                    //ToolStripMenuItem item = (ToolStripMenuItem)sender;
                    //Form sendForm = item.Owner.FindForm();

                    string ClickedItem = sender.ToString();  // text from the menu item clicked


                    bool ActionFound = GridActions(ClickedItem);
                    if (ActionFound) { return; }

                    // assign grid
                    //DataGridView dv = gridMenuDGV;

                    //cOntrols con = new cOntrols(); 
                    //DataGridView dv = con.Return_FirstGridView(sendForm); 

                    //Dev.cOntrols controls = new Dev.cOntrols();
                    //DataGridView dv = controls.Return_FirstGridView((Form)sender);  // Returns the First DataGridView Control Found on Form


                    if (ClickedItem == "Grid Info")
                    {
                        //grid.Set_One_Column_Editable(dv, "", 0);  // enable editing on the checkbox column while setting the rest to read-only
                        Display_GridInfo(CurrentGrid(), CurrentGrid());
                        return;
                        //int ColCount = grid.ColCount(dv);
                        //int RowCount = grid.RowCount(dv);
                        //bool UserAddRows = dv.AllowUserToAddRows;  // check to see if there is an extra blank line in grid
                        //string GridInfo = "DataGridView: " + dv.Name + "\nColumn Count: " + ColCount + "\nRow Count: " + RowCount + "\nUser Add Rows: " + UserAddRows.ToString();
                        //ahk.MsgBox(GridInfo); 
                        //return; 
                    }



                    //=== Toggle Viewing DataGridView Columns using Menu ================

                    var clickedMenuItem = sender as ToolStripMenuItem;  // capture the clicked toolstrip menu item

                    // toggle the checked state of the selected column name, as well as the visibility of that column on the datagrid

                    if (clickedMenuItem.Checked)
                    {
                        clickedMenuItem.Checked = false;
                        Column_Visibility_Toggle(dv, sender.ToString());  // toggle the visibility of the clicked column name from menu
                        return;
                    }
                    if (!clickedMenuItem.Checked)
                    {
                        clickedMenuItem.Checked = true;
                        Column_Visibility_Toggle(dv, sender.ToString());  // toggle the visibility of the clicked column name from menu
                        return;
                    }

                }


                public bool GridActions(string ClickedItem)
                {
                    DataGridView dv = gridSetup.currentGrid;


                    if (ClickedItem == "Clear Grid") { ClearGrid(dv); return true; }
                    if (ClickedItem == "Show Grid") { ShowGrid(dv); return true; }
                    if (ClickedItem == "Hide Grid") { HideGrid(dv); return true; }
                    if (ClickedItem == "Toggle Grid") { Toggle_Grid(dv); return true; }
                    if (ClickedItem == "Stretch Columns") { Fill_Column_Width(dv); return true; }
                    if (ClickedItem == "AutoSize Columns") { AutoSize_AllColumns(dv); return true; }



                    if (ClickedItem == "Save Column Settings") { Save_Grid_Column_Settings(dv, ahk.AppDir() + "\\Settings.sqlite", "GridDisplay"); return true; }
                    if (ClickedItem == "Load Column  Settings") { Load_Grid_Column_Settings(dv, ahk.AppDir() + "\\Settings.sqlite", "GridDisplay"); return true; }

                    if (ClickedItem == "Background Color")
                    {
                        Color SelectedColor = ahk.Color_Dialog(); // select color dialog
                        // update control if value returned
                        if (SelectedColor != Color.Empty) { Grid_Background_Color(dv, SelectedColor); dv.BackgroundColor = Color.Honeydew; }
                        return true;
                    }


                    // Grid CheckBox Actions

                    if (ClickedItem == "Check All")
                    {
                        bool hasCheckBoxColumn = Has_CheckBox_Column(dv);  // confirm grid has checkbox
                        if (hasCheckBoxColumn)  // if so, find checkbox column number and check all
                        {
                            int checkBoxColumnNum = CheckBox_Column_Position(dv);
                            CheckALL(dv, checkBoxColumnNum); return true;
                        }
                        else { sb.StatusBar("No CheckBoxes Found In " + dv.Name, 1); return true; }
                    }
                    if (ClickedItem == "UnCheck All")
                    {
                        bool hasCheckBoxColumn = Has_CheckBox_Column(dv);  // confirm grid has checkbox
                        if (hasCheckBoxColumn)  // if so, find checkbox column number and UnCheck all
                        {
                            int checkBoxColumnNum = CheckBox_Column_Position(dv);
                            UnCheckALL(dv, checkBoxColumnNum); return true;
                        }
                        else { sb.StatusBar("No CheckBoxes Found In " + dv.Name, 1); return true; }
                    }
                    if (ClickedItem == "Checked Row Numbers")
                    {
                        bool hasCheckBoxColumn = Has_CheckBox_Column(dv);  // confirm grid has checkbox
                        if (hasCheckBoxColumn)  // if so, find checkbox column number and UnCheck all
                        {
                            int checkBoxColumnNum = CheckBox_Column_Position(dv);
                            List<int> CheckedRowNums = Return_Checked_RowNumbers(dv, checkBoxColumnNum);
                            ahk.MsgBox("Checked Row Number Count: " + CheckedRowNums.Count.ToString());
                            return true;
                        }
                        else { sb.StatusBar("No CheckBoxes Found In " + dv.Name, 1); return true; }
                    }
                    if (ClickedItem == "Checked Row Values")
                    {
                        bool hasCheckBoxColumn = Has_CheckBox_Column(dv);  // confirm grid has checkbox
                        if (hasCheckBoxColumn)  // if so, find checkbox column number and UnCheck all
                        {
                            int checkBoxColumnNum = CheckBox_Column_Position(dv);
                            List<string> CheckedRowValues = Return_Checked_Values(dv, "Function", checkBoxColumnNum);
                            ahk.MsgBox("Checked Row Number Count: " + CheckedRowValues.Count.ToString());
                            return true;
                        }
                        else { sb.StatusBar("No CheckBoxes Found In " + dv.Name, 1); return true; }
                    }

                    return false;
                }



                /// <summary>Create Datatable with DataGridView settings - Display in same/another grid</summary>
                /// <param name="dv"> </param>
                /// <param name="populateDv"> </param>
                public void Display_GridInfo(DataGridView dv, DataGridView populateDv)
                {
                    if (dv == null) { dv = gridSetup.currentGrid; }
                    if (dv != null) { gridSetup.currentGrid = dv; }

                    DataTable table = new DataTable();
                    table.Columns.Add("Setting_Name", typeof(string));
                    table.Columns.Add("Setting_Bool", typeof(bool));
                    table.Columns.Add("Setting_Value", typeof(string));

                    // Here we add five DataRows.
                    table.Rows.Add("GridView Name", false, dv.Name);
                    table.Rows.Add("Allow Drop", dv.AllowDrop, "");
                    table.Rows.Add("Allow User Add Rows", dv.AllowUserToAddRows, "");
                    table.Rows.Add("Allow User Delete Rows", dv.AllowUserToDeleteRows, "");
                    table.Rows.Add("Allow User Order Columns", dv.AllowUserToOrderColumns, "");
                    table.Rows.Add("Allow User Resize Columns", dv.AllowUserToResizeColumns, "");
                    table.Rows.Add("Allow User Resize Rows", dv.AllowUserToResizeRows, "");
                    table.Rows.Add("Causes Validation", dv.CausesValidation, "");
                    table.Rows.Add("Column Headers Visible", dv.ColumnHeadersVisible, "");
                    table.Rows.Add("Enabled", dv.Enabled, "");
                    table.Rows.Add("Enable Header Visual Styles", dv.EnableHeadersVisualStyles, "");
                    table.Rows.Add("Multi Select", dv.MultiSelect, "");
                    table.Rows.Add("Read Only", dv.ReadOnly, "");
                    table.Rows.Add("Row Headers Visible", dv.RowHeadersVisible, "");
                    table.Rows.Add("Show Cell Errors", dv.ShowCellErrors, "");
                    table.Rows.Add("Show Cell ToolTips", dv.ShowCellToolTips, "");
                    table.Rows.Add("Show Editing Icon", dv.ShowEditingIcon, "");
                    table.Rows.Add("Show Row Errors", dv.ShowRowErrors, "");
                    table.Rows.Add("Tab Stop", dv.TabStop, "");
                    table.Rows.Add("Use Wait Cursor", dv.UseWaitCursor, "");
                    table.Rows.Add("Visible", dv.Visible, "");


                    ClearGrid(populateDv); // clear previous grid values
                    //populateDv.DataSource = null;

                    populateDv.DataSource = table;  // display grid info in the same / another grid

                    //GridInfo_Assign_EventHandlers(populateDv); // assign grid actions to the displaying grid

                    //Set_One_Column_Editable(populateDv, "Setting_Bool"); // set the bool field to editable, others read-only

                }


                public void GridMenuBasics_Reload(object sender = null, EventArgs e = null)
                {
                    // remove and rebuild grid menu options (updates columns etc)
                    menu.Menu_Remove(CurrentMenu(), "GridMenuBasics");
                    GridMenuBasics_Attach(CurrentGrid(), CurrentMenu(), GridMenuBasics_ClickEvent, "GridMenuBasics", false);
                }


                #region === GridInfo Config ===


                private void GridInfo_Assign_EventHandlers(DataGridView dv)  // assign event handlers + default behavior options for DataGridView control
                {
                    if (dv == null) { dv = gridSetup.currentGrid; }
                    if (dv != null) { gridSetup.currentGrid = dv; }

                    // remove existing event handlers
                    //DataGridView_Remove_Existing_EventHandlers(dv);


                    // grid config options
                    dv.AllowDrop = true;  // used to drag/drop rows
                    dv.AllowUserToAddRows = true;

                    dv.EditMode = System.Windows.Forms.DataGridViewEditMode.EditOnKeystrokeOrF2;
                    dv.MultiSelect = false;
                    dv.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
                    dv.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.AllCells;
                    //dv.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;


                    // add new click events to this grid
                    dv.CellClick += new System.Windows.Forms.DataGridViewCellEventHandler(GridInfo_CellClick);
                    dv.CellDoubleClick += new System.Windows.Forms.DataGridViewCellEventHandler(GridInfo_CellDoubleClick);
                    dv.CellValueChanged += new System.Windows.Forms.DataGridViewCellEventHandler(GridInfo_CellValueChanged);
                    dv.KeyUp += new System.Windows.Forms.KeyEventHandler(GridInfo_KeyUp);
                    dv.MouseClick += new System.Windows.Forms.MouseEventHandler(GridInfo_RightClick_Menu);

                    //// drag / drop rows
                    //dv.DragDrop += new System.Windows.Forms.DragEventHandler(GridInfo_DragDrop);
                    //dv.DragOver += new System.Windows.Forms.DragEventHandler(GridInfo_DragOver);
                    //dv.DragEnter += new System.Windows.Forms.DragEventHandler(GridInfo_DragEnter);
                    ////dv.MouseDown += new System.Windows.Forms.MouseEventHandler(GridInfo_MouseDown);
                    //dv.CellMouseDown += new System.Windows.Forms.DataGridViewCellMouseEventHandler(GridInfo_CellMouseDown);
                    //dv.MouseMove += new System.Windows.Forms.MouseEventHandler(GridInfo_MouseMove);

                    //StatusBar("[ GridInfo Mode ]", 2);

                }

                private void GridInfo_Menu_ClickEvent(object sender, EventArgs e)  // click event action for optional menustrip entry to edit this control dynamically
                {
                    DataGridView dv = null;
                    if (dv == null) { dv = gridSetup.currentGrid; }
                    if (dv != null) { gridSetup.currentGrid = dv; }

                    // assign grid
                    //DataGridView dv = gridMenuDGV;
                    //Dev.cOntrols controls = new Dev.cOntrols(); 
                    //DataGridView dv = controls.Return_FirstGridView((Form)sender);  // Returns the First DataGridView Control Found on Form


                    string DbFile = @"C:\Users\jason\Google Drive\IMDB\SQLiter\Dynamic_Coder\bin\Debug\Project_Files\Db\FunctionLib.sqlite";

                    _Database.Tags tags = new _Database.Tags();
                    tags.Display_Function_Tags(dv, DbFile, sender.ToString(), true);  // search [FunctionLib] for selected menu item tag
                }


                private void GridInfo_CellClick(object sender, DataGridViewCellEventArgs e)  // single click grid action
                {
                    if (e.RowIndex < 0) { return; }  // invalid row number passed into function

                    int index = e.RowIndex; ((DataGridView)sender).Rows[e.RowIndex].Selected = true;  // select the row 

                    if (e.RowIndex != -1)  // out of range value
                    {
                        int RowNum = (e.RowIndex); // returns selected row # in grid

                        string Setting_Name = ((DataGridView)sender).Rows[RowNum].Cells["Setting_Name"].Value.ToString();  // return value from datagridview
                        string Setting_Bool = ((DataGridView)sender).Rows[RowNum].Cells["Setting_Bool"].Value.ToString();  // return value from datagridview
                        string Setting_Value = ((DataGridView)sender).Rows[RowNum].Cells["Setting_Value"].Value.ToString();  // return value from datagridview

                        //string TableName = ((DataGridView)sender).Rows[RowNum].Cells[0].Value.ToString();          // return value from datagridview

                        //StatusBar("Selected " + Setting_Name, 1);
                    }
                }

                private void GridInfo_CellDoubleClick(object sender, DataGridViewCellEventArgs e)  // double click grid action
                {
                    if (e.RowIndex < 0) { return; }  // invalid row number passed into function

                    if (e.RowIndex != -1)  // Return Row # in Grid
                    {
                        int RowNum = (e.RowIndex); // returns selected row # in grid

                        string Setting_Name = ((DataGridView)sender).Rows[RowNum].Cells["Setting_Name"].Value.ToString();  // return value from datagridview
                        string Setting_Bool = ((DataGridView)sender).Rows[RowNum].Cells["Setting_Bool"].Value.ToString();  // return value from datagridview
                        string Setting_Value = ((DataGridView)sender).Rows[RowNum].Cells["Setting_Value"].Value.ToString();  // return value from datagridview

                        ahk.MsgBox("Setting Name: " + Setting_Name + "\nSetting Bool: " + Setting_Bool + "\nSetting Value: " + Setting_Value);
                    }
                }

                private void GridInfo_CellValueChanged(object sender, DataGridViewCellEventArgs e)
                {
                    if (e.RowIndex < 0) { return; }  // invalid row number passed into function

                    DataGridView dv = ((DataGridView)sender); // capture sending datagridview name

                    if (e.RowIndex != -1)  // Return Row # in Grid
                    {
                        int RowNum = (e.RowIndex); // returns selected row # in grid

                        string Setting_Name = dv.Rows[RowNum].Cells["Setting_Name"].Value.ToString();  // return value from datagridview
                        string Setting_Bool = dv.Rows[RowNum].Cells["Setting_Bool"].Value.ToString();  // return value from datagridview
                        string Setting_Value = dv.Rows[RowNum].Cells["Setting_Value"].Value.ToString();  // return value from datagridview

                        bool SettingBool = ahk.ToBool(Setting_Bool); // convert string to bool


                        // change the selected value for setting changed
                        if (Setting_Name == "Allow Drop") { dv.AllowDrop = SettingBool; }
                        if (Setting_Name == "Allow User Add Rows") { dv.AllowUserToAddRows = SettingBool; }
                        if (Setting_Name == "Allow User Delete Rows") { dv.AllowUserToDeleteRows = SettingBool; }
                        if (Setting_Name == "Allow User Order Columns") { dv.AllowUserToOrderColumns = SettingBool; }
                        if (Setting_Name == "Allow User Resize Columns") { dv.AllowUserToResizeColumns = SettingBool; }
                        if (Setting_Name == "Allow User Resize Rows") { dv.AllowUserToResizeRows = SettingBool; }
                        if (Setting_Name == "Causes Validation") { dv.CausesValidation = SettingBool; }
                        if (Setting_Name == "Column Headers Visible") { dv.ColumnHeadersVisible = SettingBool; }
                        if (Setting_Name == "Enabled") { dv.Enabled = SettingBool; }
                        if (Setting_Name == "Enable Header Visual Styles") { dv.EnableHeadersVisualStyles = SettingBool; }
                        if (Setting_Name == "Multi Select") { dv.MultiSelect = SettingBool; }
                        if (Setting_Name == "Read Only") { dv.ReadOnly = SettingBool; }
                        if (Setting_Name == "Row Headers Visible") { dv.RowHeadersVisible = SettingBool; }
                        if (Setting_Name == "Show Cell Errors") { dv.ShowCellErrors = SettingBool; }
                        if (Setting_Name == "Show Cell ToolTips") { dv.ShowCellToolTips = SettingBool; }
                        if (Setting_Name == "Show Editing Icon") { dv.ShowEditingIcon = SettingBool; }
                        if (Setting_Name == "Show Row Errors") { dv.ShowRowErrors = SettingBool; }
                        if (Setting_Name == "Tab Stop") { dv.TabStop = SettingBool; }
                        if (Setting_Name == "Use Wait Cursor") { dv.UseWaitCursor = SettingBool; }
                        if (Setting_Name == "Visible") { dv.Visible = SettingBool; }


                        //StatusBar("Changed Setting: " + Setting_Name + " To: " + SettingBool.ToString());
                        //ahk.MsgBox(Setting_Name + " Value Changed");
                    }
                }

                private void GridInfo_KeyUp(object sender, KeyEventArgs e)  // Delete selected rows on DELETE Key Press on DataGrid, Check Checkboxes with SPACE Key Press when Selected
                {
                    DataGridView dv = ((DataGridView)sender);

                    // [ Delete Key ]

                    if (e.KeyCode == Keys.Delete)  // delete key to remove selected row(s) when grid option is enabled
                    {
                        foreach (DataGridViewRow row in dv.SelectedRows)
                        {
                            dv.Rows.Remove(row);
                        }
                    }


                    // [ Space Bar ]

                    if (e.KeyCode == Keys.Space)  // space key to toggle check box fields with the keyboard
                    {
                        foreach (DataGridViewRow r in dv.SelectedRows)
                        {
                            // toggle between checked and unchecked when highlighted and space bar pressed

                            if (r.Cells["Setting_Bool"].Value.ToString().ToUpper() == "FALSE") { r.Cells["Setting_Bool"].Value = true; continue; }
                            if (r.Cells["Setting_Bool"].Value.ToString().ToUpper() == "TRUE") { r.Cells["Setting_Bool"].Value = false; continue; }
                        }

                        e.Handled = true; //this is necessary because otherwise when the checkbox cell is selected, it will apply this keyup and also apply the default behavior for the checkbox
                    }


                }


                // DataGrid Right Click Menu + Actions

                int GridInfo_CurrentRow = 0;

                public void GridInfo_RightClick_Menu(object sender, MouseEventArgs e)  // build / display right click menu options for this gridview
                {
                    // assign grid
                    DataGridView dv = null;
                    if (dv == null) { dv = gridSetup.currentGrid; }
                    if (dv != null) { gridSetup.currentGrid = dv; }
                    //Dev.cOntrols controls = new Dev.cOntrols();
                    //DataGridView dv = controls.Return_FirstGridView((Form)sender);  // Returns the First DataGridView Control Found on Form

                    ContextMenuStrip dg1 = new ContextMenuStrip();
                    dg1.Items.Clear();
                    dv.ContextMenuStrip = null;


                    // If  [ Right Click ]

                    if (e.Button == MouseButtons.Right)
                    {
                        GridInfo_CurrentRow = dv.HitTest(e.X, e.Y).RowIndex; //current row number selected by user

                        dg1.Items.Clear();
                        dv.ContextMenuStrip = null;

                        string Option = "Default"; // option to have more than one menu later

                        if (Option == "Default")
                        {
                            ToolStripMenuItem Opt1 = new ToolStripMenuItem("[ File Options ]"); Opt1.Click += new EventHandler(GridInfo_RightClick_Actions);
                            ToolStripMenuItem Opt2 = new ToolStripMenuItem("Open File"); Opt2.Click += new EventHandler(GridInfo_RightClick_Actions);
                            ToolStripMenuItem Opt3 = new ToolStripMenuItem("Open Dir"); Opt3.Click += new EventHandler(GridInfo_RightClick_Actions);
                            ToolStripMenuItem Opt4 = new ToolStripMenuItem("Rename File"); Opt4.Click += new EventHandler(GridInfo_RightClick_Actions);
                            ToolStripMenuItem Opt5 = new ToolStripMenuItem("Delete File"); Opt5.Click += new EventHandler(GridInfo_RightClick_Actions);
                            ToolStripMenuItem Opt6 = new ToolStripMenuItem("Grid Display Options"); Opt6.Click += new EventHandler(GridInfo_RightClick_Actions);

                            //Add to main context menu
                            dg1.Items.AddRange(new ToolStripItem[] { Opt1, Opt2, Opt3, Opt4, Opt5, Opt6 });
                        }


                        //Assign to datagridview
                        dv.ContextMenuStrip = dg1;

                        dg1.Show(dv, new Point(e.X, e.Y));

                    }
                }

                private void GridInfo_RightClick_Actions(object sender, System.EventArgs e)  // define right click actions for the RightClick_Menu above
                {
                    DataGridView dv = null;
                    if (dv == null) { dv = gridSetup.currentGrid; }
                    if (dv != null) { gridSetup.currentGrid = dv; }



                    //Dev.cOntrols controls = new Dev.cOntrols();
                    //DataGridView dv = controls.Return_FirstGridView((Form)sender);  // Returns the First DataGridView Control Found on Form

                    try
                    {
                        int DDLRowVal = GridInfo_CurrentRow; //currently selected grid2 row number

                        string Options = sender.ToString();  // name of the option passed from the rclick menu
                        //string FilePath = dv.Rows[DDLRowVal].Cells[0].Value.ToString();
                        //string FileName = dv.Rows[DDLRowVal].Cells[1].Value.ToString();

                        string Setting_Name = dv.Rows[DDLRowVal].Cells[0].Value.ToString();
                        string Setting_Bool = dv.Rows[DDLRowVal].Cells[1].Value.ToString();
                        string Setting_Value = dv.Rows[DDLRowVal].Cells[2].Value.ToString();


                        //======================
                        //  File Options
                        //======================

                        Options = Options.ToUpper();

                        if (Options.ToString() == "OPEN FILE")
                        {
                            ahk.MsgBox(Options + " - " + Setting_Name);
                            //ahk.Run(FilePath);
                        }
                        if (Options.ToString() == "OPEN DIR")
                        {
                            ahk.MsgBox(Options + " - " + Setting_Name);
                            //FileInfo info = new FileInfo(FilePath);
                            //ahk.OpenDir(info.Directory.ToString());
                            //Load_FileGrid();
                        }
                        if (Options.ToString() == "RENAME FILE")
                        {
                            ahk.MsgBox(Options + " - " + Setting_Name);
                        }
                        if (Options.ToString() == "DELETE FILE")
                        {
                            ahk.MsgBox(Options + " - " + Setting_Name);
                        }
                        if (Options.ToString() == "GRID DISPLAY OPTIONS")
                        {
                            ahk.MsgBox(Options + " - " + Setting_Name);
                            //Load_DataGridColumns_inTreeView(dataGridView1, treeView1, "DataGridView1"); //Load_FileGrid();
                        }

                    }
                    catch
                    {
                    }

                }
        */
        #endregion


        #region === GRID TREEVIEW ===
        /*

                // Grid Options Toggle TreeView

                public void Load_DataGrid_OptionTree(TreeView TV, DataGridView dv, string ParentName = "Grid Options", bool ClearTree = true, bool CheckBoxes = true, bool ExpandOnLoad = true, string TagText = "GridOptions") // Create Node in TreeView with List of DataGridView Columns
                {
                    if (ClearTree) { tv.ClearTV(TV); } // option to clear treeview before populating

                    if (dv == null) { dv = gridSetup.currentGrid; }
                    if (dv != null) { gridSetup.currentGrid = dv; }

                    // assign event handler for the treeView check/uncheck action
                    TV.AfterCheck += new System.Windows.Forms.TreeViewEventHandler(DataGrid_OptionTree_AfterCheck);


                    //### Grid Column Visibility ########

                    //==== Create Parent Node =============
                    TreeNode parent1 = new TreeNode();
                    parent1.Text = "Column Visibility";

                    try
                    {
                        int columnCount = dv.Rows[0].Cells.Count;
                        for (int i = 0; i < columnCount; i++)
                        {
                            TreeNode child3 = new TreeNode();
                            child3.Text = dv.Columns[i].HeaderText;
                            child3.Checked = dv.Columns[i].Visible;
                            child3.Tag = "ColumnVisibility";
                            parent1.Nodes.Add(child3);

                        }

                        TV.Nodes.Add(parent1);  // add to treeview
                    }
                    catch { }




                    //### Grid Options ########################

                    //==== Create Parent Node =============
                    TreeNode parent = new TreeNode();
                    parent.Text = ParentName;

                    try
                    {
                        TreeNode child2 = new TreeNode();
                        child2.Text = "Allow Drop";
                        child2.Checked = dv.AllowDrop;
                        child2.Tag = TagText;
                        parent.Nodes.Add(child2);

                        child2 = new TreeNode();
                        child2.Text = "Allow User Add Rows";
                        child2.Tag = TagText;
                        child2.Checked = dv.AllowUserToAddRows;
                        parent.Nodes.Add(child2);

                        child2 = new TreeNode();
                        child2.Text = "Allow User Delete Rows";
                        child2.Tag = TagText;
                        child2.Checked = dv.AllowUserToDeleteRows;
                        parent.Nodes.Add(child2);

                        child2 = new TreeNode();
                        child2.Text = "Allow User Order Columns";
                        child2.Tag = TagText;
                        child2.Checked = dv.AllowUserToOrderColumns;
                        parent.Nodes.Add(child2);

                        child2 = new TreeNode();
                        child2.Text = "Allow User Resize Columns";
                        child2.Tag = TagText;
                        child2.Checked = dv.AllowUserToResizeColumns;
                        parent.Nodes.Add(child2);

                        child2 = new TreeNode();
                        child2.Text = "Allow User Resize Rows";
                        child2.Tag = TagText;
                        child2.Checked = dv.AllowUserToResizeRows;
                        parent.Nodes.Add(child2);

                        child2 = new TreeNode();
                        child2.Text = "Causes Validation";
                        child2.Tag = TagText;
                        child2.Checked = dv.CausesValidation;
                        parent.Nodes.Add(child2);

                        child2 = new TreeNode();
                        child2.Text = "Column Headers Visible";
                        child2.Tag = TagText;
                        child2.Checked = dv.ColumnHeadersVisible;
                        parent.Nodes.Add(child2);

                        child2 = new TreeNode();
                        child2.Text = "Enabled";
                        child2.Tag = TagText;
                        child2.Checked = dv.Enabled;
                        parent.Nodes.Add(child2);

                        child2 = new TreeNode();
                        child2.Text = "Enable Header Visual Styles";
                        child2.Tag = TagText;
                        child2.Checked = dv.EnableHeadersVisualStyles;
                        parent.Nodes.Add(child2);

                        child2 = new TreeNode();
                        child2.Text = "Multi Select";
                        child2.Tag = TagText;
                        child2.Checked = dv.MultiSelect;
                        parent.Nodes.Add(child2);

                        child2 = new TreeNode();
                        child2.Text = "Read Only";
                        child2.Tag = TagText;
                        child2.Checked = dv.ReadOnly;
                        parent.Nodes.Add(child2);

                        child2 = new TreeNode();
                        child2.Text = "Row Headers Visible";
                        child2.Tag = TagText;
                        child2.Checked = dv.RowHeadersVisible;
                        parent.Nodes.Add(child2);

                        child2 = new TreeNode();
                        child2.Text = "Show Cell Errors";
                        child2.Tag = TagText;
                        child2.Checked = dv.ShowCellErrors;
                        parent.Nodes.Add(child2);

                        child2 = new TreeNode();
                        child2.Text = "Show Cell ToolTips";
                        child2.Tag = TagText;
                        child2.Checked = dv.ShowCellToolTips;
                        parent.Nodes.Add(child2);

                        child2 = new TreeNode();
                        child2.Text = "Show Editing Icon";
                        child2.Tag = TagText;
                        child2.Checked = dv.ShowEditingIcon;
                        parent.Nodes.Add(child2);

                        child2 = new TreeNode();
                        child2.Text = "Show Row Errors";
                        child2.Tag = TagText;
                        child2.Checked = dv.ShowRowErrors;
                        parent.Nodes.Add(child2);

                        child2 = new TreeNode();
                        child2.Text = "Tab Stop";
                        child2.Tag = TagText;
                        child2.Checked = dv.TabStop;
                        parent.Nodes.Add(child2);

                        child2 = new TreeNode();
                        child2.Text = "Use Arrow Cursor";
                        child2.Tag = TagText;
                        child2.Checked = dv.UseWaitCursor;
                        parent.Nodes.Add(child2);

                        child2 = new TreeNode();
                        child2.Text = "Use Cross Cursor";
                        child2.Tag = TagText;
                        child2.Checked = dv.UseWaitCursor;
                        parent.Nodes.Add(child2);

                        child2 = new TreeNode();
                        child2.Text = "Use Hand Cursor";
                        child2.Tag = TagText;
                        child2.Checked = dv.UseWaitCursor;
                        parent.Nodes.Add(child2);

                        child2 = new TreeNode();
                        child2.Text = "Use Wait Cursor";
                        child2.Tag = TagText;
                        child2.Checked = dv.UseWaitCursor;
                        parent.Nodes.Add(child2);

                        child2 = new TreeNode();
                        child2.Text = "Visible";
                        child2.Tag = TagText;
                        child2.Checked = dv.Visible;
                        parent.Nodes.Add(child2);



                        TV.Nodes.Add(parent);  // add to treeview
                    }
                    catch { }


                    if (ExpandOnLoad) { TV.ExpandAll(); } // option to expand results in treeview

                    if (CheckBoxes) { TV.CheckBoxes = true; }

                }

                public void DataGrid_OptionTree_AfterCheck(object sender, TreeViewEventArgs e)
                {
                    DataGridView DV = CurrentGrid();
                    TreeView TV = (TreeView)sender;

                    // === Node Checked / Unchecked
                    TreeNode node = e.Node;
                    if (node == null) { return; }  //nothing to do if null value passed while user is clicking

                    string CheckedText = e.Node.Text;
                    string RootNode = tv.Root_Node(node);
                    string NodeTag = "";
                    if (node.Tag != null) { NodeTag = node.Tag.ToString(); }


                    // check/uncheck all children nodes if parent is selected
                    if (node.Level == 0)
                    {
                        if (node.Checked) { tv.Check_Children(TV, e.Node.Text); return; }
                        if (!node.Checked) { tv.UnCheck_Children(TV, e.Node.Text); return; }
                    }


                    if (NodeTag == "ColumnVisibility")  // toggle visibility on check 
                    {
                        if (node.Checked)
                        {
                            //DV.Columns[node.Text].Visible = true;
                            Column_Visibility_Toggle(DV, node.Text, true, false);
                            Fill_Column_Width(DV);
                        }
                        if (!node.Checked)
                        {
                            //DV.Columns[node.Text].Visible = false; 
                            Column_Visibility_Toggle(DV, node.Text, false, true);
                            Fill_Column_Width(DV);
                        }

                    }

                    if (NodeTag == "GridOptions")
                    {
                        string Setting_Name = node.Text;

                        if (Setting_Name == "Allow Drop") { DV.AllowDrop = node.Checked; }
                        if (Setting_Name == "Allow User Add Rows") { DV.AllowUserToAddRows = node.Checked; }
                        if (Setting_Name == "Allow User Delete Rows") { DV.AllowUserToDeleteRows = node.Checked; }
                        if (Setting_Name == "Allow User Order Columns") { DV.AllowUserToOrderColumns = node.Checked; }
                        if (Setting_Name == "Allow User Resize Columns") { DV.AllowUserToResizeColumns = node.Checked; }
                        if (Setting_Name == "Allow User Resize Rows") { DV.AllowUserToResizeRows = node.Checked; }
                        if (Setting_Name == "Causes Validation") { DV.CausesValidation = node.Checked; }
                        if (Setting_Name == "Column Headers Visible") { DV.ColumnHeadersVisible = node.Checked; }
                        if (Setting_Name == "Enabled") { DV.Enabled = node.Checked; }
                        if (Setting_Name == "Enable Header Visual Styles") { DV.EnableHeadersVisualStyles = node.Checked; }
                        if (Setting_Name == "Multi Select") { DV.MultiSelect = node.Checked; }
                        if (Setting_Name == "Read Only") { DV.ReadOnly = node.Checked; }
                        if (Setting_Name == "Row Headers Visible") { DV.RowHeadersVisible = node.Checked; }
                        if (Setting_Name == "Show Cell Errors") { DV.ShowCellErrors = node.Checked; }
                        if (Setting_Name == "Show Cell ToolTips") { DV.ShowCellToolTips = node.Checked; }
                        if (Setting_Name == "Show Editing Icon") { DV.ShowEditingIcon = node.Checked; }
                        if (Setting_Name == "Show Row Errors") { DV.ShowRowErrors = node.Checked; }
                        if (Setting_Name == "Tab Stop") { DV.TabStop = node.Checked; }
                        if (Setting_Name == "Visible") { DV.Visible = node.Checked; }

                        //==== Grid Cursors ===================================

                        if (Setting_Name == "Use Arrow Cursor")
                        {
                            if (node.Checked)
                            {
                                DV.Cursor = Cursors.Arrow;
                                //TreeNode FindNode = tv.ReturnNode(TV, "Use Cross Cursor"); FindNode.Checked = false;
                                //FindNode = tv.ReturnNode(TV, "Use Hand Cursor"); FindNode.Checked = false;
                                //FindNode = tv.ReturnNode(TV, "Use Wait Cursor"); FindNode.Checked = false;
                            }
                            else
                            {
                                DV.Cursor = Cursors.Default;
                            }
                        }
                        if (Setting_Name == "Use Cross Cursor")
                        {
                            if (node.Checked)
                            {
                                DV.Cursor = Cursors.Cross;
                                //TreeNode FindNode = tv.ReturnNode(TV, "Use Arrow Cursor"); FindNode.Checked = false;
                                //FindNode = tv.ReturnNode(TV, "Use Hand Cursor"); FindNode.Checked = false;
                                //FindNode = tv.ReturnNode(TV, "Use Wait Cursor"); FindNode.Checked = false;
                            }
                            else { DV.Cursor = Cursors.Default; }
                        }
                        if (Setting_Name == "Use Hand Cursor")
                        {
                            if (node.Checked)
                            {
                                DV.Cursor = Cursors.Hand;
                                //TreeNode FindNode = tv.ReturnNode(TV, "Use Arrow Cursor"); FindNode.Checked = false;
                                //FindNode = tv.ReturnNode(TV, "Use Cross Cursor"); FindNode.Checked = false;
                                //FindNode = tv.ReturnNode(TV, "Use Wait Cursor"); FindNode.Checked = false;
                            }
                            else
                            {
                                DV.Cursor = Cursors.Default;
                            }
                        }
                        if (Setting_Name == "Use Wait Cursor")
                        {
                            if (node.Checked)
                            {
                                DV.Cursor = Cursors.WaitCursor;

                                //DV.UseWaitCursor = node.Checked;
                                //TreeNode FindNode = tv.ReturnNode(TV, "Use Arrow Cursor"); FindNode.Checked = false;
                                //FindNode = tv.ReturnNode(TV, "Use Cross Cursor"); FindNode.Checked = false;
                                //FindNode = tv.ReturnNode(TV, "Use Hand Cursor"); FindNode.Checked = false;
                            }
                            else
                            {
                                DV.Cursor = Cursors.Default;
                            }
                        }



                    }

                }



                public void ColumnWidthTreePopulate(DataGridView dv, TreeView TV)
                {
                    if (dv == null) { dv = gridSetup.currentGrid; }
                    if (dv != null) { gridSetup.currentGrid = dv; }

                    TreeNode parent = new TreeNode();  // level 1
                    parent.Text = "Column Width %";


                    // remove previously loaded column width node (if already added)
                    tv.Remove_Node(TV, parent.Text, "");


                    //DataGridView dv = DV; // grid to search
                    gridSetup.ColumnWidthPercentages = Column_Width_Percentages(dv);


                    if (gridSetup.ColumnWidthPercentages == null) { return; }


                    int i = 0;
                    foreach (int width in gridSetup.ColumnWidthPercentages)
                    {
                        TreeNode section = new TreeNode();  // level 2
                        section.Text = dv.Columns[i].HeaderText + " (" + width + "%)";
                        parent.Nodes.Add(section);

                        //ahk.MsgBox("Column " + i + " | Width = " + width + "%");
                        i++;
                    }

                    //TreeNode entry = new TreeNode();  // level 3
                    //entry.Text = FunctionName;
                    //section.Nodes.Add(entry);

                    TV.Nodes.Add(parent);  // populate tree

                    tv.Expand_Children(TV, parent.Text);  // expand children under new node after attaching
                }


        */
        #endregion


        #region === GRID TYPES ===========
        /*

                #region === FunctionGrid Config ===

                // Add to project startup to map datagrid to event clicks
                // dv = dataGridView1;

                string DbFile = @"C:\Users\jason\Google Drive\IMDB\SQLiter\Dynamic_Coder\bin\Debug\Project_Files\Db\FunctionLib.sqlite";
                //DataGridView dv = new DataGridView();


                // Populate menu on startup :: ex :: FuctionGrid_Populate_TagMenu(dataGridView1, tagsToolStripMenuItem);  // populate tag names in menu drop down
                /// <summary> </summary>
                /// <param name="dv"> </param>
                /// <param name="parentMenuItem"> </param>
                /// <param name="CodeTagDir"> </param>
                public void FuctionGrid_Populate_TagMenu(DataGridView dv, ToolStripMenuItem parentMenuItem, string CodeTagDir = "") // populate parent menu item with list of tag categories + tag names
                {
                    if (dv == null) { dv = gridSetup.currentGrid; }
                    if (dv != null) { gridSetup.currentGrid = dv; }

                    _MenuControl menu = new _MenuControl();

                    string tagDir = "";
                    if (CodeTagDir != "") { tagDir = CodeTagDir; }
                    if (CodeTagDir == "") { tagDir = @"C:\_Code\LucidProjects\ADBindex\ADBindex\bin\Debug\Docs\Tags\[CodeTags]"; }  // !!! hard coded path

                    FunctionGrid_Assign_EventHandlers(dv);

                    List<string> MenuItems = lst.DirList(tagDir, "*.*", false);

                    DirList_To_Menu(tagDir, parentMenuItem, FunctionGrid_Menu_ClickEvent, "Tag_Menu", true, true);

                    // populate menu drop down with single tag folder (control types)
                    //lst.Dir_To_Menu(@"C:\Users\jason\Google Drive\IMDB\SQLiter\Dynamic_Coder\bin\Debug\Project_Files\Tags\[CodeTags]\_Controls", controlTagsToolStripMenuItem, FunctionGrid_Menu_ClickEvent);
                }



                /// <summary>Create menu from list of directories, populate submenus with directory file names, attach to menu item, assign click event</summary>
                /// <param name="DirPath"> </param>
                /// <param name="MenuOrItem"> </param>
                /// <param name="ClickEventFunction"> </param>
                /// <param name="NewMenuName"> </param>
                /// <param name="RemoveFileExt"> </param>
                /// <param name="RemovePreviousMenu"> </param>
                public void DirList_To_Menu(string DirPath, object MenuOrItem, EventHandler ClickEventFunction, string NewMenuName = "Dir_Menu", bool RemoveFileExt = false, bool RemovePreviousMenu = false)
                {
                    if (!Directory.Exists(DirPath)) { ahk.MsgBox("Dir Not Found : " + DirPath); return; }

                    List<string> DirList = lst.DirList(DirPath, "*.*", false);

                    if (DirList == null) { ahk.MsgBox("DirList Null : " + DirPath); return; }

                    if (MenuOrItem is MenuStrip)
                    {
                        MenuStrip AttachMenu = MenuOrItem as MenuStrip;  // cast object as MenuStrip item

                        if (RemovePreviousMenu)
                        {
                            //Menu_Remove(AttachMenu, NewMenuName);
                        }

                        ToolStripMenuItem parentMenuItem = new ToolStripMenuItem(NewMenuName);  // new top level menu item

                        foreach (string dirName in DirList)  // create new submenu items and attach to parent Menu item
                        {
                            ToolStripMenuItem foo = new ToolStripMenuItem(dirName);
                            parentMenuItem.DropDownItems.Add(foo);

                            List<string> fileMenuItems = lst.FileList(DirPath + "\\" + dirName, "*.*", false);

                            foreach (string fileName in fileMenuItems)  // create new submenu items and attach to parent Menu item
                            {
                                string FileName = "";

                                if (RemoveFileExt) { FileName = ahk.FileNameNoExt(fileName); }
                                if (!RemoveFileExt) { FileName = ahk.FileName(fileName); }


                                ToolStripMenuItem fileM = new ToolStripMenuItem(FileName);
                                fileM.Click += new EventHandler(ClickEventFunction);
                                foo.DropDownItems.Add(fileM);
                            }
                        }

                        AttachMenu.Items.Add(parentMenuItem);
                    }


                    // user passed in an existing ToolStrip Item - build and Populate on that ToolStrip
                    if (MenuOrItem is ToolStripMenuItem)
                    {
                        ToolStripMenuItem parentMenuItem = MenuOrItem as ToolStripMenuItem;

                        foreach (string dirName in DirList)  // create new submenu items and attach to parent Menu item
                        {
                            ToolStripMenuItem foo = new ToolStripMenuItem(dirName);
                            parentMenuItem.DropDownItems.Add(foo);

                            List<string> fileMenuItems = lst.FileList(DirPath + "\\" + dirName, "*.*", false);

                            foreach (string fileName in fileMenuItems)  // create new submenu items and attach to parent Menu item
                            {
                                string FileName = "";

                                if (RemoveFileExt) { FileName = ahk.FileNameNoExt(fileName); }
                                if (!RemoveFileExt) { FileName = ahk.FileName(fileName); }


                                ToolStripMenuItem fileM = new ToolStripMenuItem(FileName);
                                fileM.Click += new EventHandler(ClickEventFunction);
                                foo.DropDownItems.Add(fileM);
                            }
                        }
                    }


                }



                private void FunctionGrid_Assign_EventHandlers(DataGridView dv)  // FunctionGrid Actions - Assign Event Handlers
                {
                    if (dv == null) { dv = gridSetup.currentGrid; }
                    if (dv != null) { gridSetup.currentGrid = dv; }

                    dv.CellClick += new System.Windows.Forms.DataGridViewCellEventHandler(FunctionGrid_CellClick);
                    dv.CellDoubleClick += new System.Windows.Forms.DataGridViewCellEventHandler(FunctionGrid_CellClick);
                    //this.dvTreeViewFunctions.CellValueChanged += new System.Windows.Forms.DataGridViewCellEventHandler(this.ControlFunctionGrid_CellValueChanged);

                    dv.EditMode = System.Windows.Forms.DataGridViewEditMode.EditProgrammatically;  // disable ability for user to edit fields in grid
                    dv.AllowUserToAddRows = false;
                    dv.MultiSelect = false;
                    dv.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
                    dv.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.DisplayedCells;
                    //dv.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
                }

                private void FunctionGrid_Menu_ClickEvent(object sender, EventArgs e)  // FunctionGrid Actions - Menu ClickEvent Action
                {
                    _Database.Tags tags = new _Database.Tags();

                    //ahk.MsgBox(sender.ToString());
                    tags.Display_Function_Tags(grID.dvName, DbFile, sender.ToString(), true);  // search [FunctionLib] for selected menu item tag
                }

                // single click grid action
                private void FunctionGrid_CellClick(object sender, DataGridViewCellEventArgs e)  // FunctionGrid Actions - SingleClick Grid Action
                {
                    if (e.RowIndex < 0) { return; }  // invalid row number passed into function

                    int index = e.RowIndex; ((DataGridView)sender).Rows[e.RowIndex].Selected = true;  // select the row 

                    if (e.RowIndex != -1)  // out of range value
                    {
                        int RowNum = (e.RowIndex); // returns selected row # in grid

                        string Function = ((DataGridView)sender).Rows[RowNum].Cells["Function"].Value.ToString();  // return value from datagridview
                        string FunctionLine = ((DataGridView)sender).Rows[RowNum].Cells["FunctionLine"].Value.ToString();  // return value from datagridview
                        //string TableName = ((DataGridView)sender).Rows[RowNum].Cells[0].Value.ToString();          // return value from datagridview


                        //StatusBar(Function, 1);
                    }
                }

                // double click grid action
                private void FunctionGrid_CellDoubleClick(object sender, DataGridViewCellEventArgs e)  // FunctionGrid Actions - Cell DoubleClick Action
                {
                    if (e.RowIndex < 0) { return; }  // invalid row number passed into function

                    if (e.RowIndex != -1)  // Return Row # in Grid
                    {
                        int RowNum = (e.RowIndex); // returns selected row # in grid

                        string FileName = ((DataGridView)sender).Rows[RowNum].Cells["FileName"].Value.ToString();  // return value from datagridview
                        string TableName = ((DataGridView)sender).Rows[RowNum].Cells[0].Value.ToString();          // return value from datagridview
                    }
                }

                // when user edits grid, set value to true to save with next save action
                private void FunctionGrid_CellValueChanged(object sender, DataGridViewCellEventArgs e)  // FunctionGrid Actions - Cell Value Changed
                {
                    if (e.RowIndex < 0) { return; }  // invalid row number passed into function

                    if (e.RowIndex != -1)  // Return Row # in Grid
                    {
                        int RowNum = (e.RowIndex); // returns selected row # in grid

                        string FileName = ((DataGridView)sender).Rows[RowNum].Cells["FileName"].Value.ToString();  // return value from datagridview
                        string TableName = ((DataGridView)sender).Rows[RowNum].Cells[0].Value.ToString();          // return value from datagridview
                    }
                }

                // Delete selected rows on DELETE Key Press on DataGrid
                private void FunctionGrid_KeyUp(object sender, KeyEventArgs e) // FunctionGrid Actions - Delete selected rows on DELETE Key Press on DataGrid
                {
                    if (e.KeyCode == Keys.Delete)
                    {
                        foreach (DataGridViewRow row in ((DataGridView)sender).SelectedRows)
                        {
                            ((DataGridView)sender).Rows.Remove(row);
                        }
                    }
                }


                #endregion

                #region === GridList Config ===


                public void GridList_Assign_EventHandlers(DataGridView dv)
                {
                    if (dv == null) { dv = gridSetup.currentGrid; }
                    if (dv != null) { gridSetup.currentGrid = dv; }

                    //DataGridView_Remove_Existing_EventHandlers(dv);  // remove handlers from all other event handlers for this control

                    // add new event handlers and presets

                    dv.CellClick += new System.Windows.Forms.DataGridViewCellEventHandler(GridList_CellClick);
                    dv.CellDoubleClick += new System.Windows.Forms.DataGridViewCellEventHandler(GridList_CellClick);
                    //this.dvTreeViewFunctions.CellValueChanged += new System.Windows.Forms.DataGridViewCellEventHandler(this.ControlFunctionGrid_CellValueChanged);
                    dv.MouseClick += new System.Windows.Forms.MouseEventHandler(GridList_RightClick_Menu);

                    dv.EditMode = System.Windows.Forms.DataGridViewEditMode.EditProgrammatically;  // disable ability for user to edit fields in grid
                    //dv.EditMode = System.Windows.Forms.DataGridViewEditMode.EditOnEnter; 
                    dv.AllowUserToAddRows = false;
                    dv.MultiSelect = false;
                    dv.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
                    dv.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.DisplayedCells;
                    //dv.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;

                    sb.StatusBar("[ GridList Mode ]", 2);
                }

                public void GridList_Menu_ClickEvent(object sender, EventArgs e)
                {
                    GridList_Assign_EventHandlers(grID.dvName);
                    tags.Display_Function_Tags(grID.dvName, DbFile, sender.ToString(), true);  // search [FunctionLib] for selected menu item tag
                }

                // single click grid action
                public void GridList_CellClick(object sender, DataGridViewCellEventArgs e)
                {
                    if (e.RowIndex < 0) { return; }  // invalid row number passed into function

                    int index = e.RowIndex; ((DataGridView)sender).Rows[e.RowIndex].Selected = true;  // select the row 

                    if (e.RowIndex != -1)  // out of range value
                    {
                        int RowNum = (e.RowIndex); // returns selected row # in grid

                        string ListItem = ((DataGridView)sender).Rows[RowNum].Cells[1].Value.ToString();  // return value from datagridview

                        //if (grid.Column_Names_Search(((DataGridView)sender), "Function"))  // check to see if column name exists in grid
                        //{
                        //    Function = ((DataGridView)sender).Rows[RowNum].Cells["Function"].Value.ToString();  // return value from datagridview
                        //}
                        //else
                        //{
                        //    Function = ((DataGridView)sender).Rows[RowNum].Cells[1].Value.ToString();  // return value from datagridview
                        //}

                        //string FunctionLine = "";

                        //if (grid.Column_Names_Search(((DataGridView)sender), "FunctionLine"))  // check to see if column name exists in grid
                        //{
                        //    FunctionLine = ((DataGridView)sender).Rows[RowNum].Cells["FunctionLine"].Value.ToString();  // return value from datagridview
                        //}
                        //else
                        //{
                        //    FunctionLine = ((DataGridView)sender).Rows[RowNum].Cells[0].Value.ToString();  // return value from datagridview
                        //}


                        //string TableName = ((DataGridView)sender).Rows[RowNum].Cells[0].Value.ToString();          // return value from datagridview


                        sb.StatusBar(ListItem, 1);
                    }
                }

                // double click grid action
                public void GridList_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
                {
                    if (e.RowIndex < 0) { return; }  // invalid row number passed into function

                    if (e.RowIndex != -1)  // Return Row # in Grid
                    {
                        int RowNum = (e.RowIndex); // returns selected row # in grid

                        string FileName = ((DataGridView)sender).Rows[RowNum].Cells["FileName"].Value.ToString();  // return value from datagridview
                        string TableName = ((DataGridView)sender).Rows[RowNum].Cells[0].Value.ToString();          // return value from datagridview

                        ahk.MsgBox(FileName);
                    }
                }

                // when user edits grid, set value to true to save with next save action
                public void GridList_CellValueChanged(object sender, DataGridViewCellEventArgs e)
                {
                    if (e.RowIndex < 0) { return; }  // invalid row number passed into function

                    if (e.RowIndex != -1)  // Return Row # in Grid
                    {
                        int RowNum = (e.RowIndex); // returns selected row # in grid

                        string FileName = ((DataGridView)sender).Rows[RowNum].Cells["FileName"].Value.ToString();  // return value from datagridview
                        string TableName = ((DataGridView)sender).Rows[RowNum].Cells[0].Value.ToString();          // return value from datagridview
                    }
                }

                // Delete selected rows on DELETE Key Press on DataGrid
                public void GridList_KeyUp(object sender, KeyEventArgs e)
                {
                    if (e.KeyCode == Keys.Delete)
                    {
                        foreach (DataGridViewRow row in ((DataGridView)sender).SelectedRows)
                        {
                            ((DataGridView)sender).Rows.Remove(row);
                        }

                    }
                }


                // DataGrid Right Click Menu + Actions

                public int GridList_CurrentRow = 0;

                public void GridList_RightClick_Menu(object sender, MouseEventArgs e)  // build / display right click menu options for this gridview
                {
                    DataGridView dv = grID.dvName;

                    ContextMenuStrip dg1 = new ContextMenuStrip();
                    dg1.Items.Clear();
                    dv.ContextMenuStrip = null;


                    // If  [ Right Click ]

                    if (e.Button == MouseButtons.Right)
                    {
                        GridList_CurrentRow = dv.HitTest(e.X, e.Y).RowIndex; //current row number selected by user

                        dg1.Items.Clear();
                        dv.ContextMenuStrip = null;

                        string Option = "Default"; // option to have more than one menu later

                        if (Option == "Default")
                        {
                            ToolStripMenuItem Opt1 = new ToolStripMenuItem("[ Grid List ]"); Opt1.Click += new EventHandler(GridList_RightClick_Actions);
                            ToolStripMenuItem Opt2 = new ToolStripMenuItem("Open File"); Opt2.Click += new EventHandler(GridList_RightClick_Actions);
                            ToolStripMenuItem Opt3 = new ToolStripMenuItem("Open Dir"); Opt3.Click += new EventHandler(GridList_RightClick_Actions);
                            ToolStripMenuItem Opt4 = new ToolStripMenuItem("Rename File"); Opt4.Click += new EventHandler(GridList_RightClick_Actions);
                            ToolStripMenuItem Opt5 = new ToolStripMenuItem("Delete File"); Opt5.Click += new EventHandler(GridList_RightClick_Actions);
                            ToolStripMenuItem Opt6 = new ToolStripMenuItem("Grid Display Options"); Opt6.Click += new EventHandler(GridList_RightClick_Actions);

                            //Add to main context menu
                            dg1.Items.AddRange(new ToolStripItem[] { Opt1, Opt2, Opt3, Opt4, Opt5, Opt6 });
                        }


                        //Assign to datagridview
                        dv.ContextMenuStrip = dg1;

                        dg1.Show(dv, new Point(e.X, e.Y));

                    }
                }

                public void GridList_RightClick_Actions(object sender, System.EventArgs e)  // define right click actions for the RightClick_Menu above
                {
                    DataGridView dv = grID.dvName;

                    try
                    {

                        string Options = sender.ToString();  // name of the option passed from the rclick menu

                        string List_Bool = dv.Rows[GridList_CurrentRow].Cells[0].Value.ToString();
                        string List_Value = dv.Rows[GridList_CurrentRow].Cells[1].Value.ToString();


                        //======================
                        //  File Options
                        //======================

                        Options = Options.ToUpper();

                        if (Options.ToString() == "OPEN FILE")
                        {
                            ahk.MsgBox(Options + " - " + List_Value);
                            //ahk.Run(FilePath);
                        }
                        if (Options.ToString() == "OPEN DIR")
                        {
                            ahk.MsgBox(Options + " - " + List_Value);
                            //FileInfo info = new FileInfo(FilePath);
                            //ahk.OpenDir(info.Directory.ToString());
                            //Load_FileGrid();
                        }
                        if (Options.ToString() == "RENAME FILE")
                        {
                            ahk.MsgBox(Options + " - " + List_Value);

                            //MessageBox.Show(Options.ToString() + " - " + FilePath);
                            //FileInfo info = new FileInfo(FilePath);

                            //// prompt to rename existing file
                            //string value = info.FullName;
                            //if (ahk.InputBox("Rename File: ", "", ref value) == DialogResult.OK)
                            //{
                            //    string UserEntry = value;
                            //    ahk.FileRename(info.FullName, value, true);
                            //    //Load_FileGrid();
                            //}

                        }
                        if (Options.ToString() == "DELETE FILE")
                        {
                            ahk.MsgBox(Options + " - " + List_Value);

                            //var ResultValue = ahk.YesNoBox("Delete " + FileName + "?", "Delete File?");
                            //if (ResultValue.ToString() == "Yes")
                            //{
                            //    ahk.FileDelete(FilePath);
                            //    //Load_FileGrid();
                            //}
                        }
                        if (Options.ToString() == "GRID DISPLAY OPTIONS")
                        {
                            ahk.MsgBox(Options + " - " + List_Value);

                            //Load_DataGridColumns_inTreeView(dataGridView1, treeView1, "DataGridView1"); //Load_FileGrid();
                        }

                    }
                    catch
                    {
                    }

                }



                #endregion


        */
        #endregion



        #region === Grid SAVE ===
        /*

                /// <summary>
                /// Save Grid Column Width to Settings.sqlite
                /// </summary>
                /// <param name="dv"></param>
                public void SaveGridColumnWidth(DataGridView dv)
                {
                    if (dv == null) { dv = gridSetup.currentGrid; }
                    if (dv != null) { gridSetup.currentGrid = dv; }

                    // save gridview column width % to sqlite 
                    //DataGridView dv = currentGrid; // grid to search
                    sb.StatusBar("Saving GridView column Width...", 1, true);
                    gridSetup.ColumnWidthPercentages = Column_Width_Percentages(dv);

                    int i = 0;
                    foreach (int width in gridSetup.ColumnWidthPercentages)
                    {
                        sb.StatusBar("Saving Column Width | Column " + i + " | Width = " + width + "%");
                        //string cmd = "Insert into [

                        string Option = "Width";
                        string DbFile = ahk.AppDir() + "\\Settings.sqlite";
                        string SettingName = "Column" + i;
                        string TableName = dv.Name + "_Columns";
                        string Value = width.ToString();

                        Setting_Save(SettingName, Value, Option, DbFile, TableName);
                        i++;
                    }

                    sb.StatusBar("Saved GridView column Width", 1, false, true);

                }


                /// <summary>
                ///  Save Grid Row Height to SQLite Settings
                /// </summary>
                /// <param name="dv"></param>

                public void SaveGridRowHeight(DataGridView dv)
                {
                    if (dv == null) { dv = gridSetup.currentGrid; }
                    if (dv != null) { gridSetup.currentGrid = dv; }

                    // save gridview column width % to sqlite 
                    //DataGridView dv = currentGrid; // grid to search
                    sb.StatusBar("Saving GridView Row Height...", 1, true);
                    gridSetup.RowHeightList = RowHeights_Get(dv);

                    int i = 0;
                    foreach (int rowHeight in gridSetup.RowHeightList)
                    {
                        sb.StatusBar("Saving RowHeight | Row " + i + " | RowHeight = " + rowHeight);

                        string Option = "RowHeight";
                        string DbFile = ahk.AppDir() + "\\Settings.sqlite";
                        string SettingName = "Column" + i;
                        string TableName = dv.Name + "_Columns";
                        string Value = rowHeight.ToString();

                        Setting_Save(SettingName, Value, Option, DbFile, TableName);
                        i++;
                    }

                    sb.StatusBar("Saved GridView Row Height", 1, false, true);
                }




                int insertCount = 0;

                // ### Save DataGrid vales to SQLite Table ####

                /// <summary>Save DataGridView contents to sqlite db file</summary>
                /// <param name="dv">DataGridView To Save</param>
                /// <param name="DbFile">FilePath to .SQLite File (Creates New If It Doesn't Exist Yet)</param>
                /// <param name="TableName">Name to Save Grid Contents As. Defaults to OverWrite Previous Contents with ClearTableFirst = True</param>
                /// <param name="ClearTableFirst">Defaults to Clear the Previous Values Saved in this TableName</param>
                /// <param name="SkipColumnNum">SkipColumnNum allows you to omit saving data from that column, -1 means don't skip any. 0 = First Column</param>
                public void Save_Grid_Values(DataGridView dv, string DbFile, string TableName, bool ClearTableFirst = true, int SkipColumnNum = -1)
                {
                    if (dv == null) { dv = gridSetup.currentGrid; }
                    if (dv != null) { gridSetup.currentGrid = dv; }

                    sb.StatusBar("Saving " + ahk.FileName(DbFile) + " | TableName " + TableName + " | ClearTableFirst = " + ClearTableFirst.ToString() + " | SkipColNum = " + SkipColumnNum.ToString());
                    bool SkipIDField = false;
                    if (SkipColumnNum != -1) { SkipIDField = true; }

                    if (DbFile == "") { DbFile = ahk.AppDir() + "\\Settings.sqlite"; }




                    //if (ClearTableFirst) { bool Cleared = sqlite.Table_Clear(DbFile, TableName); }  // option to clear out table values before writing


                    SB("Saving Grid Values...");
                    SaveGridSettings(dv); // save all control options
                    SB("FINISHED Saving Grid Values");


                    SaveGridColumnWidth(dv);  // save grid column width values

                    SaveGridRowHeight(dv);  // save grid column height values 

                    SaveColumnTypes(dv); // save grid column types


                    // create table from grid column names if table doesn't exist

                    List<string> Grid_Columns = Column_Names(dv);  // return list of column names from gridview


                    bool TableExists = Table_Exists(DbFile, TableName + "_Values"); //check to see if table name exists yet

                    if (!TableExists) { New_Table_From_GridColumns(dv, DbFile, TableName + "_Values"); }


                    // populate sqlite insert statement

                    string Command = "Insert into [" + TableName + "_Values" + "] (";
                    string VarList = "";

                    int ColNum = 0;
                    foreach (string col in Grid_Columns)
                    {
                        if (SkipIDField) { if (ColNum == SkipColumnNum) { ColNum++; continue; } } // option to skip updating field in sqlite db

                        if (VarList != "") { VarList = VarList + ", [" + col + "]"; }
                        if (VarList == "") { VarList = "[" + col + "]"; }
                        ColNum++;
                    }

                    Command = Command + VarList + ") Values (";

                    int columnCount = dv.Rows[0].Cells.Count;
                    string RowValues = "";

                    // check to see if user has datagrid row adding enabled, used to offset counter in row loops
                    int AdjustCount = 0;
                    if (dv.AllowUserToAddRows) { AdjustCount = 1; }  // subtract 1 row if the user is allowed to add blank new rows


                    int insertCount = 0;
                    for (int i = 0; i < dv.Rows.Count - AdjustCount; i++) // loop through each row
                    {
                        RowValues = "";

                        for (int c = 0; c < columnCount; c++)  // loop through each column
                        {
                            if (SkipIDField) { if (c == SkipColumnNum) { continue; } } // option to skip updating first field in sqlite db

                            string value = dv.Rows[i].Cells[c].Value.ToString();

                            value = ahk.FixSpecialChars(value); //remove invalid characters before writing

                            if (RowValues != "") { RowValues = RowValues + ", '" + value + "'"; }
                            if (RowValues == "") { RowValues = "'" + value + "'"; continue; }
                        }

                        string SQLCommand = Command + RowValues + ")";

                        bool Inserted = sqlite.Execute(DbFile, SQLCommand);
                        if (Inserted) { insertCount++; }

                    }

                    SB("Finished Saving " + ahk.FileName(DbFile) + " | Saved " + insertCount.ToString() + " Records | TableName " + TableName + " & " + TableName + "_Values");
                }



                public void SaveColumnTypes(DataGridView dv)
                {
                    if (dv == null) { dv = gridSetup.currentGrid; }
                    if (dv != null) { gridSetup.currentGrid = dv; }

                    List<string> colTypes = Column_Types(dv);
                    int i = 0;
                    foreach (string col in colTypes)
                    {

                        sb.StatusBar("Saving ColumnType | Column " + i + "/" + colTypes.Count);

                        string Option = "ColumnType";
                        string DbFile = ahk.AppDir() + "\\Settings.sqlite";
                        string SettingName = "Column" + i;
                        string TableName = dv.Name + "_Columns";

                        string type = col.Replace("System.Windows.Forms.", "");
                        string Value = type;

                        Setting_Save(SettingName, Value, Option, DbFile, TableName);
                        i++;

                        //ahk.MsgBox("Column " + i + "/" + (colTypes.Count - 1) + " | ColumnType = " + type);
                    }


                    sb.StatusBar("Saved GridView Column Types");
                }

        */
        #endregion


        // === NEW / TESTING ================================


        #region === TESTING -- Scintill In Grid ---
        /*
                public void Add_Scintilla_To_GridView()
                {
                    DataGridView dv = null;
                    if (dv == null) { dv = gridSetup.currentGrid; }
                    if (dv != null) { gridSetup.currentGrid = dv; }

                    // add 
                    //DataGridView dv = dataGridView1;
                    ScintillaColumn col = new ScintillaColumn();
                    dv.Columns.Add(col);
                    //dv.RowCount = 5;
                    foreach (DataGridViewRow row in dv.Rows)
                    {
                        row.Cells[0].Value = DateTime.Now;
                    }
                }


                //Microsoft example - adding datetime to gridview cells https://msdn.microsoft.com/en-us/library/7tas5c80(v=vs.110).aspx

                public class ScintillaColumn : DataGridViewColumn
                {

                    public override object Clone()
                    {
                        ScintillaColumn that = (ScintillaColumn)base.Clone();

                        return that;
                    }

                    private ScintillaCell _cell = null;
                    public ScintillaColumn()
                    {
                        _cell = new ScintillaCell();
                        base.CellTemplate = _cell;
                    }

                    public override DataGridViewCell CellTemplate
                    {
                        get
                        {
                            return base.CellTemplate;
                        }
                        set
                        {
                            // Ensure that the cell used for the template is a CalendarCell.
                            if (value != null &&
                                !value.GetType().IsAssignableFrom(typeof(ScintillaCell)))
                            {
                                throw new InvalidCastException("Must be a ScintillaCell");
                            }
                            base.CellTemplate = value;
                        }
                    }
                }

                public class ScintillaCell : DataGridViewTextBoxCell
                {

                    public ScintillaCell()
                        : base()
                    {
                        // Use the short date format.
                        this.Style.Format = "d";
                    }

                    public override void InitializeEditingControl(int rowIndex, object initialFormattedValue, DataGridViewCellStyle dataGridViewCellStyle)
                    {
                        // Set the value of the editing control to the current cell value.
                        base.InitializeEditingControl(rowIndex, initialFormattedValue, dataGridViewCellStyle);
                        ScintillaEditingControl ctl = DataGridView.EditingControl as ScintillaEditingControl;
                        // Use the default row value when Value property is null.
                        if (this.Value == null)
                        {
                            //ctl.Value = (DateTime)this.DefaultNewRowValue;
                            // (ScintillaNET.Scintilla)this.DefaultNewRowValue;

                            ctl.Text = "Loaded";
                            //ctl.Height = 300;

                            // set the height for every row in the grid 

                            DataGridView dv = DataGridView;
                            //foreach (DataGridViewRow row in dv.Rows)
                            //{
                            //    row.Height = 200; 
                            //}

                        }
                        else
                        {
                            //ctl.Value = (ScintillaNET.Scintilla)this.Value;
                            ctl.Text = "Loaded Again";
                            ctl.Height = 300;
                        }
                    }

                    public override Type EditType
                    {
                        get
                        {
                            // Return the type of the editing control that CalendarCell uses.
                            return typeof(ScintillaEditingControl);
                        }
                    }

                    public override Type ValueType
                    {
                        get
                        {
                            // Return the type of the value that CalendarCell contains.

                            return typeof(string);
                        }
                    }

                    public override object DefaultNewRowValue
                    {
                        get
                        {
                            // Use the current date and time as the default value.
                            return "TextHere";
                        }
                    }


                    protected override void OnClick(DataGridViewCellEventArgs e)
                    {
                        //ScintillaEditingControl ctl = DataGridView.EditingControl as ScintillaEditingControl;

                        // Take any action - I will just change the color for now.
                        //ctl.BackColor = Color.Red;
                        //ctl.Refresh();
                        DataGridView.InvalidateCell(e.ColumnIndex, e.RowIndex);
                    }

                    public override void DetachEditingControl()
                    {
                        DataGridView dataGridView = this.DataGridView;

                        if (dataGridView == null || dataGridView.EditingControl == null)
                        {
                            throw new InvalidOperationException("Cell is detached or its grid has no editing control.");
                        }

                        ScintillaEditingControl ctl = DataGridView.EditingControl as ScintillaEditingControl;
                        if (ctl != null)
                        {
                            ctl.Clear();
                            //ctl.CheckBoxItems.Clear();  //Forgot to do this.
                            ctl.EditingControlFormattedValue = String.Empty;
                        }

                        base.DetachEditingControl();
                    }

                }

                class ScintillaEditingControl : ScintillaNET.Scintilla, IDataGridViewEditingControl
                {
                    DataGridView dataGridView;
                    private bool valueChanged = false;
                    int rowIndex;

                    public ScintillaEditingControl()
                    {
                        this.Text = "Test";
                    }

                    // Implements the IDataGridViewEditingControl.EditingControlFormattedValue property.
                    public object EditingControlFormattedValue
                    {
                        get
                        {
                            return typeof(string);
                        }
                        set
                        {
                            if (value is string)
                            {
                                try
                                {
                                    // This will throw an exception of the string is 
                                    // null, empty, or not in the format of a date.
                                    //this.Value = DateTime.Parse((String)value);
                                    value = "wwsss";
                                }
                                catch
                                {
                                    // In the case of an exception, just use the 
                                    // default value so we're not left with a null
                                    // value.
                                    value = DateTime.Now.ToShortDateString();
                                }
                            }
                        }
                    }

                    // Implements the IDataGridViewEditingControl.GetEditingControlFormattedValue method.
                    public object GetEditingControlFormattedValue(
                        DataGridViewDataErrorContexts context)
                    {
                        return EditingControlFormattedValue;
                    }

                    // Implements the IDataGridViewEditingControl.ApplyCellStyleToEditingControl method.
                    public void ApplyCellStyleToEditingControl(
                        DataGridViewCellStyle dataGridViewCellStyle)
                    {
                        this.Font = dataGridViewCellStyle.Font;
                        //this.CalendarForeColor = dataGridViewCellStyle.ForeColor;
                        //this.CalendarMonthBackground = dataGridViewCellStyle.BackColor;
                    }

                    // Implements the IDataGridViewEditingControl.EditingControlRowIndex property.
                    public int EditingControlRowIndex
                    {
                        get
                        {
                            return rowIndex;
                        }
                        set
                        {
                            rowIndex = value;
                        }
                    }

                    // Implements the IDataGridViewEditingControl.EditingControlWantsInputKey method.
                    public bool EditingControlWantsInputKey(
                        Keys key, bool dataGridViewWantsInputKey)
                    {
                        // Let the DateTimePicker handle the keys listed.
                        switch (key & Keys.KeyCode)
                        {
                            case Keys.Left:
                            case Keys.Up:
                            case Keys.Down:
                            case Keys.Right:
                            case Keys.Home:
                            case Keys.End:
                            case Keys.PageDown:
                            case Keys.PageUp:
                                return true;
                            default:
                                return !dataGridViewWantsInputKey;
                        }
                    }

                    // Implements the IDataGridViewEditingControl PrepareEditingControlForEdit method.
                    public void PrepareEditingControlForEdit(bool selectAll)
                    {
                        // No preparation needs to be done.
                    }

                    // Implements the IDataGridViewEditingControl RepositionEditingControlOnValueChange property.
                    public bool RepositionEditingControlOnValueChange
                    {
                        get
                        {
                            return false;
                        }
                    }

                    // Implements the IDataGridViewEditingControl EditingControlDataGridView property.
                    public DataGridView EditingControlDataGridView
                    {
                        get
                        {
                            return dataGridView;
                        }
                        set
                        {
                            dataGridView = value;
                        }
                    }

                    // Implements the IDataGridViewEditingControl EditingControlValueChanged property.
                    public bool EditingControlValueChanged
                    {
                        get
                        {
                            return valueChanged;
                        }
                        set
                        {
                            valueChanged = value;
                        }
                    }

                    // Implements the IDataGridViewEditingControl EditingPanelCursor property.
                    public Cursor EditingPanelCursor
                    {
                        get
                        {
                            return base.Cursor;
                        }
                    }

                    //protected override void OnValueChanged(EventArgs eventargs)
                    //{
                    //    // Notify the DataGridView that the contents of the cell
                    //    // have changed.
                    //    valueChanged = true;
                    //    this.EditingControlDataGridView.NotifyCurrentCellDirty(true);
                    //    //base.OnValueChanged(eventargs);
                    //}


                }



                #endregion


                #region === GRID TESTING ===

                // not working yet
                public void RunTimeEdit_Columns(DataGridView dv, TreeView TV, string ParentName = "DataGridView1", bool ClearTree = true, bool CheckBoxes = true, bool ExpandOnLoad = true)  // edit visible columns while running
                {
                    if (dv == null) { dv = gridSetup.currentGrid; }
                    if (dv != null) { gridSetup.currentGrid = dv; }

                    tv.Load_DataGridColumns(TV, dv, ParentName, ClearTree, CheckBoxes, ExpandOnLoad); // Create Node in TreeView with List of DataGridView Columns
                }


                #endregion




                #endregion
        */

        #endregion




    }
}
