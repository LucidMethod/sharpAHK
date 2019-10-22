using System;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace sharpAHK
{
    public partial class _AHK
    {
        // === Lists ===


        #region === Lists: Management ===

        /// <summary>Checks if value in is List<string>. Returns true if found.</summary>
        /// <param name="LIST">List of strings to search for existing value</param>
        /// <param name="ListItem">List value to check for in LIST</param>
        /// <param name="CaseSensitive">Determines whether value must match list item's case exactly before returning 'true'</param>
        public bool InLIST(List<string> LIST, string ListItem, bool CaseSensitive = false)
        {
            if (LIST == null) { return false; }

            for (int i = 0; i < LIST.Count; i++)
            {
                if (CaseSensitive) { if (LIST[i].Trim() == ListItem.Trim()) { return true; } }
                if (!CaseSensitive) { if (LIST[i].ToUpper().Trim() == ListItem.ToUpper().Trim()) { return true; } }
            }
            return false;
        }

        /// <summary>Checks if AddItem already exists in LIST, adds to existing list if NOT in LIST</summary>
        /// <param name="LIST">List of strings to add new distinct item to</param>
        /// <param name="AddItem">Item to add to LIST if not already in LIST</param>
        /// <param name="CaseSensitive">Determines whether AddItem value must match LIST item's case exactly before excluding as existing item. False would add "haVe" and "HAVe" as different item values.</param>
        public List<string> ListADD(List<string> LIST, string AddItem, bool CaseSensitive = false)
        {
            if (LIST.Count > 0)
            {
                bool FoundInList = InLIST(LIST, AddItem, CaseSensitive);
                if (!FoundInList) { LIST.Add(AddItem); }
            }

            if (LIST == null || LIST.Count == 0) { LIST.Add(AddItem); }  // list is empty, just add new item

            return LIST;
        }

        /// <summary>Returns List with items containing SearchText</summary>
        /// <param name="LIST">List of items to search for items containing SearchText</param>
        /// <param name="SearchText">Return list of items containg SearchText in list item</param>
        /// <param name="CaseSensitive">Determines list item must contain same case as SearchText before adding to return list</param>
        /// <returns>Returns original list minus excluded items list values</returns>
        public List<string> ListSEARCH(List<string> LIST, string SearchText, bool CaseSensitive = false)
        {
            List<string> returnList = new List<string>();

            for (int i = 0; i < LIST.Count; i++)
            {
                if (CaseSensitive) { if (LIST[i].Contains(SearchText.Trim())) { returnList.Add(LIST[i]); } }
                if (!CaseSensitive) { if (LIST[i].ToUpper().Contains(SearchText.Trim().ToUpper())) { returnList.Add(LIST[i]); } }
            }

            return returnList;
        }

        /// <summary>Removes list of items passed in to remove from Original list - subtracting unwanted items</summary>
        /// <param name="OriginalList">List of items to search and remove items from</param>
        /// <param name="RemoveItems">List of items to remove from Original List</param>
        /// <param name="CaseSensitive">Determines whether RemoveItem must match item in Original list's case before excluding from return list</param>
        /// <returns>Returns original list minus excluded items list values</returns>
        public List<string> ListREMOVE(List<string> OriginalList, List<string> RemoveItems, bool CaseSensitive = false)
        {
            List<string> returnList = new List<string>();

            foreach (string Item in OriginalList)
            {
                bool Add = true;
                foreach (string eItem in RemoveItems)
                {
                    if (CaseSensitive) { if (Item.Trim() == eItem.Trim()) { Add = false; } }
                    if (!CaseSensitive) { if (Item.Trim().ToUpper() == eItem.Trim().ToUpper()) { Add = false; } }
                }

                if (Add) { returnList.Add(Item); }
            }

            return returnList;
        }

        /// <summary>Removes list of exclude items from the original list - subtracting unwanted items</summary>
        /// <param name="OriginalList">List of items to search and remove items from</param>
        /// <param name="RemoveItem">List item to remove from existing list</param>
        /// <param name="CaseSensitive">Determines whether RemoveItem must match item in Original list's case before excluding from return list</param>
        /// <returns>Returns list with all items except RemoveItem</returns>
        public List<string> ListREMOVE_Item(List<string> OriginalList, string RemoveItem, bool CaseSensitive = false)
        {
            List<string> returnList = new List<string>();

            foreach (string Item in OriginalList)
            {
                bool Add = true;

                if (CaseSensitive) { if (Item.Trim() == RemoveItem.Trim()) { Add = false; } }
                if (!CaseSensitive) { if (Item.Trim().ToUpper() == RemoveItem.Trim().ToUpper()) { Add = false; } }

                if (Add) { returnList.Add(Item); }
            }

            return returnList;
        }

        /// <summary>Return list that does not contain any items with TextToExclude in itemstring</summary>
        /// <param name="OriginalList">List of items to search and remove items from</param>
        /// <param name="ExcludeText">Return list excluding items containing this text</param>
        /// <param name="CaseSensitive">Determines whether ExcludeText must be case sensitive match to OriginalList item before removing</param>
        public List<string> ListREMOVE_Containing(List<string> OriginalList, string ExcludeText, bool CaseSensitive = false)
        {
            List<string> NewList = new List<string>();

            foreach (string project in OriginalList)
            {
                if (!CaseSensitive) { if (!project.ToUpper().Contains(ExcludeText.ToUpper())) { NewList.Add(project); } }
                if (CaseSensitive) { if (!project.Contains(ExcludeText.ToUpper())) { NewList.Add(project); } }
            }

            return NewList;
        }

        /// <summary>Returns new list merging two lists together, with the option to exclude duplicate items</summary>
        /// <param name="MainList">Existing list to add to</param>
        /// <param name="AddList">List to add to MainList, returning combined list</param>
        /// <param name="ExcludeDuplicates">Option to not add items from AddList already found in MainList</param>
        /// <param name="CaseSensitive">When ExcludeDuplicates is True, determines whether AddList item must match MainList item's case before excluding from merged list</param>
        /// <returns>Returns new list merging two lists together, with the option to exclude duplicate items</returns>
        public List<string> ListMERGE(List<string> MainList, List<string> AddList, bool ExcludeDuplicates = false, bool CaseSensitive = false)
        {
            // Merge two entire lists if not excluding duplicates
            if (!ExcludeDuplicates) { MainList.AddRange(AddList); }

            // otherwise, check to see if items in AddList already exist in Main List, add to Main List if not found
            if (ExcludeDuplicates)
            {
                foreach (string item in AddList)
                {
                    if (!InLIST(MainList, item, CaseSensitive)) { MainList.Add(item); }
                }
            }

            return MainList;
        }


        // { Needs Testing }  - need to loop again to see if there is anything in B not found in A to add to diffList.. TODO

        /// <summary>Compare two lists, return values found in List A NOT Found in List B</summary>
        public List<string> ListDIFF(List<string> ListA, List<string> ListB)
        {
            List<string> diffList = new List<string>();

            foreach (string item in ListA)  // list of ssrs from test not found in prod
            {
                bool Found = InLIST(ListB, item);
                if (!Found) { diffList.Add(item); }
            }

            return diffList;
        }

        // untested --- aiming for random list shuffle
        public List<string> List_Shuffle(List<string> aList)
        {
            System.Random _random = new System.Random();
            List<string> randomList = new List<string>();

            int n = aList.Count;
            for (int i = 0; i < n; i++)
            {
                int r = i + (int)(_random.NextDouble() * (n - i));
                randomList.Add(aList[r]);
                aList[r] = aList[i];
                aList[i] = aList[r];
            }

            return aList;
        }

        #endregion


        #region === Lists: Return From ===

        /// <summary>Return Position # of SearchTerm in LIST<string></summary>
        /// <param name="LIST">List to search for SearchTerm Item</param>
        /// <param name="SearchTerm">Term to Match in LIST, returns position of item in LIST</param>
        /// <param name="CaseSensitive">Determines whether SearchTerm must match LIST item's case exactly before considering terms a match</param>
        /// <returns>Returns int indicating where SearchTerm was located in LIST, -1 returned if SearchItem not found</returns>
        public int List_ItemPosition(List<string> LIST, string SearchTerm, bool CaseSensitive = false)
        {
            if (LIST == null || LIST.Count == 0) { return -1; } // -1 return value means item not found in list

            int iIndex = 0;
            foreach (string line in LIST)
            {
                // compare line text to search term
                if (CaseSensitive) { if (line.Trim() == SearchTerm.Trim()) { return iIndex; } } // if found, return current index number
                if (!CaseSensitive) { if (line.ToUpper().Trim() == SearchTerm.ToUpper().Trim()) { return iIndex; } } // if found, return current index number
                iIndex++;
            }

            return -1; // -1 return value means item not found in list
        }

        /// <summary>Return Position # of SearchInt in LIST<string></summary>
        /// <param name="LIST">List to search for SearchInt value</param>
        /// <param name="SearchInt">Integer to look for in LIST of numbers, returns position of int in LIST</param>
        /// <returns>Returns int indicating where SearchInt was located in LIST, -1 returned if SearchInt not found</returns>
        public int List_Int_ItemPosition(List<int> LIST, int SearchInt)
        {
            if (LIST == null || LIST.Count == 0) { return -1; } // -1 return value means item not found in list

            int iIndex = 0;
            foreach (int line in LIST)
            {
                // compare line text to search term
                if (line == SearchInt) { return iIndex; } // if found, return current index number
                iIndex++;
            }

            return -1;
        }

        /// <summary>Return value of LIST item by Position # in List</summary>
        /// <param name="LIST">List to return item value from</param>
        /// <param name="ListPosition">Item # in LIST to Return Value From</param>
        public string List_ItemValue(List<string> LIST, int ListPosition)
        {
            if (LIST == null || LIST.Count == 0) { return null; } // null return if no values found in LIST
            if (LIST.Count < ListPosition) { return null; } // null return if ListPosition not found in LIST

            int iIndex = 0;
            foreach (string line in LIST)
            {
                if (iIndex == ListPosition) { return line; } // if found, return current index number
                iIndex++;
            }

            return null;  // null return if ListPosition not found in LIST
        }

        /// <summary>return value of list<int> item by position in list</summary>
        /// <param name="List<int> list"> </param>
        /// <param name=" ListPosition"> </param>
        public int Return_List_ValueInt(List<int> list, int ListPosition)
        {
            int iIndex = 0;
            int ReturnInt = 0;
            foreach (int line in list)
            {
                ReturnInt = line;

                // compare line text to search term
                if (iIndex == ListPosition)
                {
                    return ReturnInt; // if found, return current index number
                }

                iIndex++;
            }

            return ReturnInt;  // return last value in the list if position out of range
        }

        /// <summary>Returns First Item added to LIST</summary>
        /// <param name="LIST">List to return First Item Value From</param>
        /// <returns>Returns first item in LIST, Returns NULL if no items in LIST</returns>
        public string List_FirstItem(List<string> LIST)
        {
            if (LIST == null || LIST.Count == 0) { return null; } // null return if no values found in LIST

            foreach (string item in LIST) { return item; } // return first item found in list

            return null; // otherwise null response
        }

        /// <summary>returns last item added to list</summary>
        /// <param name="List<string> list"> </param>
        public string Last_List_Item(List<string> list)
        {
            string lastItem = "";
            foreach (string item in list)
            {
                lastItem = item;
            }
            return lastItem;
        }

        /// <summary>Returns Last Item added to LIST</summary>
        /// <param name="LIST">List to return Last Item Value From</param>
        /// <returns>Returns last item in LIST, Returns NULL if no items in LIST</returns>
        public string List_LastItem(List<string> LIST)
        {
            if (LIST == null || LIST.Count == 0) { return null; } // null return if no values found in LIST

            string lastItem = "";
            foreach (string item in LIST) { lastItem = item; } // loop to last item, store that value to return
            return lastItem;
        }

        /// <summary>Return list split by SplitChar (ex: ",") as new string</summary>
        /// <param name="LIST">List to convert to string</param>
        /// <param name="SplitChar">Character to place between list items in string return</param>
        public string List_ToString(List<string> LIST, string SplitChar = ",")
        {
            if (LIST == null || LIST.Count == 0) { return ""; } // return empty string if no values found in LIST

            // Join strings into one CSV line.
            string line = string.Join(SplitChar, LIST.ToArray());

            return line;
        }

        /// <summary>return list to string, each item on new line</summary>
        /// <param name="List<string> list"> </param>
        public string List_ToStringLines(List<string> LIST)
        {
            if (LIST == null || LIST.Count == 0) { return ""; } // return empty string if no values found in LIST

            // Join list items into string, items separated by new lines
            string LineCode = List_ToString(LIST, Environment.NewLine);

            return LineCode;
        }

        #endregion


        #region === Lists: Populate ===

        /// <summary>Converts text from a text file to string List</summary>
        /// <param name="TextString"> </param>
        /// <param name="SkipBlankLines"> </param>
        /// <param name="Trim"> </param>
        /// <param name="SkipCommentLines"> </param>
        public List<string> Text_ToList(string TextString, bool SkipBlankLines = true, bool Trim = true, bool SkipCommentLines = true)
        {
            List<string> list = new List<string>();

            if (TextString == null || TextString == "") { return list; }

            // parse by new line
            {
                // Creates new StringReader instance from System.IO
                using (StringReader reader = new StringReader(TextString))
                {
                    // Loop over the lines in the string.
                    string line;
                    while ((line = reader.ReadLine()) != null)
                    {
                        if (SkipCommentLines)
                        {
                            string First2 = FirstCharacters(line, 2); // skip over lines if they are comments
                            if (First2 == @"//") { continue; }
                        }

                        string writeline = line;

                        if (Trim) { writeline = line.Trim(); } // trim leading spaces

                        if (SkipBlankLines) { if (writeline == "") { continue; } }

                        list.Add(writeline);
                    }
                }

            }
            return list;
        }

        /// <summary>Converts numbers from a text file to int List</summary>
        /// <param name="TextString"> </param>
        /// <param name="SkipBlankLines"> </param>
        /// <param name="SkipCommentLines"> </param>
        public List<int> Text_ToListInt(string TextString, bool SkipBlankLines = true, bool SkipCommentLines = true)
        {
            List<int> list = new List<int>();
            bool Trim = true;

            // parse by new line
            {
                // Creates new StringReader instance from System.IO
                using (StringReader reader = new StringReader(TextString))
                {
                    // Loop over the lines in the string.
                    string line;
                    while ((line = reader.ReadLine()) != null)
                    {
                        if (SkipCommentLines)
                        {
                            string First2 = FirstCharacters(line, 2); // skip over lines if they are comments
                            if (First2 == @"//") { continue; }
                        }

                        string writeline = line;

                        if (Trim) { writeline = line.Trim(); } // trim leading spaces

                        if (SkipBlankLines) { if (writeline == "") { continue; } }

                        int WriteInt = ToInt(writeline);  // convert string from text to int

                        list.Add(WriteInt);
                    }
                }

            }
            return list;
        }

        /// <summary>Read text file, return string List</summary>
        /// <param name="FilePath"> </param>
        /// <param name="SkipBlankLines"> </param>
        /// <param name="Trim"> </param>
        /// <param name="SkipCommentLines"> </param>
        public List<string> TextFile_ToList(string FilePath, bool SkipBlankLines = true, bool Trim = true, bool SkipCommentLines = true)
        {
            if (File.Exists(FilePath))
            {
                string ParseCode = FileRead(FilePath);

                List<string> list = Text_ToList(ParseCode, SkipBlankLines, Trim, SkipCommentLines);
                return list;
            }
            else
            {
                return null;
            }
        }

        /// <summary>Read text file, return list <int></summary>
        /// <param name="FilePath"> </param>
        public List<int> TextFile_ToListInt(string FilePath)
        {
            string ListTxt = FileRead(FilePath);
            List<int> list = new List<int>();

            // parse by new line
            {
                string[] lines = ListTxt.Split(Environment.NewLine.ToCharArray());
                foreach (string line in lines)
                {
                    string First2 = FirstCharacters(line, 2); // skip over lines if they are comments
                    if (First2 == @"//") { continue; }
                    string writeline = line.Trim();  // trim leading spaces
                    if (writeline == "") { continue; }
                    int lineInt = ToInt(writeline);  // convert line to integer
                    list.Add(lineInt);
                }
            }
            return list;
        }

        /// <summary>Convert Array[] to List</summary>
        /// <param name="arr">Array to convert to List</param>
        public List<string> Array_ToList(string[] arr)
        {
            List<string> list = new List<string>(arr); // Copy array values to List.
            return list;
        }

        /// <summary>Convert List to Array[]</summary>
        /// <param name="List<string> list"> </param>
        public string[] List_ToArray(List<string> list)
        {
            string[] s = list.ToArray();
            return s;
        }

        /// <summary>Returns list of Dictionary Keys <string></summary>
        /// <param name="Dictionary<string"> </param>
        /// <param name=" string> dictionary"> </param>
        public List<string> Dict_KeyList(Dictionary<string, string> dictionary)
        {
            // Get a List of all the Keys.
            List<string> keys = new List<string>(dictionary.Keys);
            return keys;
        }

        /// <summary>Returns list of Dictionary Keys <int></summary>
        /// <param name="Dictionary<int"> </param>
        /// <param name=" string> dictionary"> </param>
        public List<int> Dict_KeyListInt(Dictionary<int, string> dictionary)
        {
            // Get a List of all the Keys.
            List<int> keys = new List<int>(dictionary.Keys);
            return keys;
        }

        /// <summary>Returns list of string Dictionary Values</summary>
        /// <param name="Dictionary<string"> </param>
        /// <param name=" string> dictionary"> </param>
        public List<string> Dict_ValueList(Dictionary<string, string> dictionary)
        {
            // Get a List of all the values
            List<string> returnlist = new List<string>(dictionary.Values);
            return returnlist;
        }

        /// <summary>Returns list of string Dictionary Values</summary>
        /// <param name="Dictionary"> </param>
        public List<int> Dict_ValueListInt(Dictionary<string, int> dictionary)
        {
            // Get a List of all the values
            List<int> returnlist = new List<int>(dictionary.Values);
            return returnlist;
        }


        //=== File Path Lists ===

        // ex: List<string> FileList = lst.DirList(file, "*.sqlite", true); 


        /// <summary>Returns List<string> of Image File Paths in Directory Path (JPG, JPEG, GIF, ICO, PNG)</summary>
        /// <param name="DirPath"> </param>
        public List<string> DirList_Images(string DirPath, bool Recurse = true)
        {
            List<string> myImageList = new List<string>();

            if (!Directory.Exists(DirPath)) { return myImageList; } // return blank list if dir not found

            System.IO.SearchOption recurse = System.IO.SearchOption.AllDirectories;
            if (!Recurse) { recurse = System.IO.SearchOption.TopDirectoryOnly; }

            string[] files = Directory.GetFiles(DirPath, "*.*", recurse);

            foreach (string file in files)  // loop through list of files and write file details to sqlite db
            {
                System.IO.FileInfo fileinfo = new System.IO.FileInfo(file); //retrieve info about each file

                string FileName = fileinfo.Name.ToString();
                string FileExt = fileinfo.Extension.ToString();
                //string FileNameNoExt = ahk.StringReplace(FileName, FileExt);

                bool AddToList = false;

                if (FileExt.ToUpper() == ".JPG") { AddToList = true; }
                if (FileExt.ToUpper() == ".JPEG") { AddToList = true; }
                if (FileExt.ToUpper() == ".GIF") { AddToList = true; }
                if (FileExt.ToUpper() == ".PNG") { AddToList = true; }
                if (FileExt.ToUpper() == ".ICO") { AddToList = true; }

                // add image to list if valid image format
                if (AddToList) { myImageList.Add(file); }
            }

            //if (Debug)
            //{
            //    if (myImageList.Images.Count == 0)
            //    { ahk.MsgBox("Zero Images Imported to ImageList:" + Environment.NewLine + ICODir); }

            //    //ahk.MsgBox(myImageList.Images.Count.ToString() + " Images Added To ImageList");
            //}

            return myImageList;
        }

        /// <summary>loop through multiple folders, return files meeting search criteria, sort files by FileName regardless of directory path, return sorted list of full file paths as list</summary>
        /// <param name="DirPath"> </param>
        /// <param name="ExtTypes"> </param>
        /// <param name="Recurse"> </param>
        public List<string> FileList_SortedAlpha_ByFileName(string DirPath, string ExtTypes = "*.*", bool Recurse = true)
        {
            string[] files = Directory.GetFiles(DirPath, ExtTypes, System.IO.SearchOption.AllDirectories);

            if (!Recurse) { files = Directory.GetFiles(DirPath, ExtTypes, System.IO.SearchOption.TopDirectoryOnly); }

            // sort list alpha first
            List<string> filelist = new List<string>(files);
            filelist.Sort();

            List<string> filelistSort = new List<string>();
            foreach (string fil in filelist)  // loop through list items
            {
                filelistSort.Add(FileName(fil) + "|" + fil);
            }
            filelistSort.Sort();

            List<string> filelistSorted = new List<string>();
            foreach (string fi in filelistSort)  // loop through listitems
            {
                string[] words = fi.Split('|');
                string fullPath = "";
                foreach (string word in words)
                {
                    fullPath = word;
                }
                filelistSorted.Add(fullPath);
            }

            return filelistSorted;
        }


        /// <summary>
        /// Returns List of Files (Default sorted largest to smallest) size
        /// </summary>
        /// <param name="DirPath">Folder Path</param>
        /// <param name="ExtTypes">File Extentions to Search For</param>
        /// <param name="Descending">Largest to Smallest FileSize</param>
        /// <param name="Recurse">Search into SubFolders</param>
        /// <returns></returns>
        public List<string> FileList_SortedSize(string DirPath, string ExtTypes = "*.*", bool Descending = true, bool Recurse = true)
        {
            List<string> files = new List<string>();

            // File names.
            string[] fns; // = Directory.GetFiles(DirPath, ExtTypes, SearchOption.AllDirectories);

            if (Recurse) { fns = Directory.GetFiles(DirPath, ExtTypes, SearchOption.AllDirectories); }
            else { fns = Directory.GetFiles(DirPath, ExtTypes, SearchOption.TopDirectoryOnly); }


            // Order by size.
            var sort = from fn in fns
                       orderby new FileInfo(fn).Length descending
                       select fn;

            if (!Recurse) // option for smallest to largest
            {
                sort = from fn in fns
                       orderby new FileInfo(fn).Length ascending
                       select fn;
            }

            // List files.
            foreach (string n in sort)
            {
                files.Add(n);
            }

            return files;
        }



        // TODO: Finish 

        /// <summary>returns List<string> of files in directory path modified today</summary>
        /// <param name="DirPath"> </param>
        /// <param name="SearchPattern"> </param>
        /// <param name="Recurse"> </param>
        /// <param name="FileNameOnly"> </param>
        /// <param name="IncludeExt"> </param>
        public List<string> FileList_Modified_Today(string DirPath, string SearchPattern = "*.*", bool Recurse = true, bool FileNameOnly = false, bool IncludeExt = true)
        {
            List<string> FileList = new List<string>();

            if (!Directory.Exists(DirPath)) { return null; }

            string[] files = null;
            if (Recurse) { files = Directory.GetFiles(DirPath, SearchPattern, System.IO.SearchOption.AllDirectories); }
            if (!Recurse) { files = Directory.GetFiles(DirPath, SearchPattern, System.IO.SearchOption.TopDirectoryOnly); }

            foreach (string file in files)  // loop through list of files and write file details to sqlite db
            {
                FileInfo info = new FileInfo(file);

                if (info.LastWriteTime.Date == DateTime.Today)  // if file modified today - add to list
                {
                    string addFile = file;
                    if (FileNameOnly) { addFile = FileName(file); }
                    if ((FileNameOnly) && (!IncludeExt)) { addFile = FileNameNoExt(file); }
                    FileList.Add(addFile);
                }

                //ahk.MsgBox(info.LastWriteTime.Date.ToString());

                //DateTime value = new DateTime(2010, 1, 18);
                //string dateCompare = DateTimeCompare(info.LastWriteTime.Date, DateTime.Today);
                //ahk.MsgBox(dateCompare); 

                //// file size
                //FileInfo info = new FileInfo("C:\\a");
                //long value = info.Length;
                //Console.WriteLine(value);

                // add file to list to return
                //FileList.Add(addFile);
            }

            return FileList;
        }

        // TODO: Finish 

        /// <summary>returns List<string> of files in directory path modified today</summary>
        /// <param name="DirPath"> </param>
        /// <param name=" DateTime Since"> </param>
        /// <param name="SearchPattern"> </param>
        /// <param name="Recurse"> </param>
        /// <param name="FileNameOnly"> </param>
        /// <param name="IncludeExt"> </param>
        public List<string> FileList_Modified_Since(string DirPath, DateTime Since, string SearchPattern = "*.*", bool Recurse = true, bool FileNameOnly = false, bool IncludeExt = true)
        {
            List<string> FileList = new List<string>();

            if (!Directory.Exists(DirPath)) { return null; }

            string[] files = null;
            if (Recurse) { files = Directory.GetFiles(DirPath, SearchPattern, System.IO.SearchOption.AllDirectories); }
            if (!Recurse) { files = Directory.GetFiles(DirPath, SearchPattern, System.IO.SearchOption.TopDirectoryOnly); }

            foreach (string file in files)  // loop through list of files and write file details to sqlite db
            {
                FileInfo info = new FileInfo(file);


                DateTime fileDate = info.LastWriteTime;

                if (fileDate > Since)
                {
                    string addFile = file;
                    if (FileNameOnly) { addFile = FileName(file); }
                    if ((FileNameOnly) && (!IncludeExt)) { addFile = FileNameNoExt(file); }
                    FileList.Add(addFile);
                }

                //string diff = DateTimeCompare(info.LastWriteTime.Date, Since.Date);

                ////if (diff == "Later" || diff == "Same")  // date is later or the same day
                //if (diff == "Later")  // date is later or the same day
                //{
                //    string addFile = file;
                //    if (FileNameOnly) { addFile = ahk.FileName(file); }
                //    if ((FileNameOnly) && (!IncludeExt)) { addFile = ahk.FileNameNoExt(file); }
                //    FileList.Add(addFile);
                //}

                //if (info.LastWriteTime.Date == Since.Date)  // if file modified today - add to list
                //{
                //    string addFile = file;
                //    if (FileNameOnly) { addFile = ahk.FileName(file); }
                //    if ((FileNameOnly) && (!IncludeExt)) { addFile = ahk.FileNameNoExt(file); }
                //    FileList.Add(addFile);
                //}

                //ahk.MsgBox(info.LastWriteTime.Date.ToString());

                //DateTime value = new DateTime(2010, 1, 18);
                //string dateCompare = DateTimeCompare(info.LastWriteTime.Date, DateTime.Today);
                //ahk.MsgBox(dateCompare); 

                //// file size
                //FileInfo info = new FileInfo("C:\\a");
                //long value = info.Length;
                //Console.WriteLine(value);

                // add file to list to return
                //FileList.Add(addFile);
            }

            return FileList;
        }

        ///  Drives
        /// <summary>Returns list of drive letters (C:\ etc) visible on this pc</summary>
        public List<string> List_Drives()
        {
            List<string> Drives = new List<string>();

            foreach (System.IO.DriveInfo di in System.IO.DriveInfo.GetDrives())
                Drives.Add(di.Name);

            return Drives;
        }


        ///  CSV
        /// <summary>convert list to csv formatted string</summary>
        /// <param name="List<string> list"> </param>
        public string List_ToCSV(List<string> list)
        {
            string returnText = "";
            foreach (string item in list)
            {
                if (returnText != "") { returnText = returnText + ", " + item; }
                if (returnText == "") { returnText = item; }
            }

            return returnText;
        }

        /// <summary>convert csv to list<string></summary>
        /// <param name="CSV"> </param>
        /// <param name="ParseBy"> </param>
        public List<string> CSV_ToList(string CSV, string ParseBy = ",")
        {
            char myChar = ParseBy[0];
            List<string> CSV_List = new List<string>();
            string[] words = CSV.Split(myChar);
            foreach (string word in words)
            {
                CSV_List.Add(word);
            }

            return CSV_List;
        }



        /// <summary>
        /// Returns List of Alphabet (to loop). If Start Letter Provided, Will Return All Letters Starting with That One
        /// </summary>
        /// <param name="StartLetter"></param>
        /// <returns></returns>
        public List<string> Letters(string StartLetter = "")
        {
            List<string> lets = new List<string>() { "A", "B", "C", "D", "E", "F", "G", "H", "I", "J", "K", "L", "M", "N", "O", "P", "Q", "R", "S", "T", "U", "V", "W", "X", "Y", "Z" };

            if (StartLetter != "")
            {
                List<string> Lets = new List<string>();
                bool start = false;
                foreach (string let in lets)
                {
                    if (let.ToUpper() == StartLetter.ToUpper()) { start = true; }
                    if (start) { Lets.Add(let); }
                }
                return Lets;
            }

            return lets;
        }


        #endregion

        #region === Processes ===

        /// <summary>Returns List of Controls on a Form as a Control List</summary>
        /// <param name="FormName">WinForm to return list of Controls From</param>
        public List<Control> ControlList(Control FormName)
        {
            var controlList = new List<Control>();

            foreach (Control childControl in FormName.Controls)
            {
                // Recurse child controls.
                controlList.AddRange(ControlList(childControl));
                controlList.Add(childControl);
            }
            return controlList;
        }


        #endregion


        #region === Lists: Display  ===

        //  Display Lists

        // List of Tables in SQLite File to GridView Display
        // Ex: lst.List_To_Grid(dataGridView1, sqlite.TableList(DbFile), "TABLES"); 

        /// <summary>populate DataGridView from List <string></summary>
        /// <param name="dv"> </param>
        /// <param name=" List<string> list"> </param>
        /// <param name="ListName"> </param>
        /// <param name="AddCheckBox"> </param>
        public void List_To_Grid(DataGridView dv, List<string> list, string ListName = "List_View", bool AddCheckBox = false)
        {
            //======= Create DataTable and Assign to DataGrid  =======
            DataTable dt = new DataTable();

            if (!AddCheckBox)
            {
                dt.Columns.Add(ListName, typeof(String));                     // Create Columns

                foreach (string item in list)
                {
                    dt.Rows.Add(new object[] { item });
                }
            }

            if (AddCheckBox)  // option to add checkboxes to first column of datagridview
            {
                dt.Columns.Add("Selected", typeof(bool));
                dt.Columns.Add(ListName, typeof(String));

                foreach (string item in list)
                {
                    dt.Rows.Add(new object[] { false, item });
                }
            }


            // Assign to Grid / Display
            dv.DataSource = dt;
        }

        /// <summary>populate DataGridView from List <int></summary>
        /// <param name="dv"> </param>
        /// <param name=" List<int> list"> </param>
        /// <param name="ListName"> </param>
        public void List_To_GridInt(DataGridView dv, List<int> list, string ListName = "List_View")
        {
            //======= Create DataTable and Assign to DataGrid  =======
            DataTable dt = new DataTable();

            // Create Columns
            dt.Columns.Add(ListName, typeof(String));

            foreach (int item in list)
            {
                dt.Rows.Add(new object[] { item });
            }

            // Assign to Grid / Display
            dv.DataSource = dt;
        }

        /// <summary>populate TreeView from List <string></summary>
        /// <param name="TV"> </param>
        /// <param name=" List<string> LoadList"> </param>
        /// <param name="ParentName">Name of Parent Node to List Items Under - Leave Blank to Add to TreeView Root</param>
        /// <param name="cLearTV"> </param>
        public void List_To_TreeView(TreeView TV, List<string> LoadList, string ParentName = "List", bool cLearTV = true)
        {
            if (cLearTV) { TV.Nodes.Clear(); }
            TreeViewList(TV, LoadList, ParentName);
        }

        /// <summary>populate TreeView from List <int></summary>
        /// <param name="TV"> </param>
        /// <param name=" List<string> LoadList"> </param>
        /// <param name="ParentName"> </param>
        /// <param name="NoParent"> </param>
        /// <param name="cLearTV"> </param>
        public void List_To_TreeViewInt(TreeView TV, List<int> LoadList, string ParentName = "List", bool NoParent = false, bool cLearTV = true)
        {
            if (cLearTV) { TV.Nodes.Clear(); }

            if (!NoParent) { TreeViewList_Int(TV, LoadList, ParentName); }

            if (NoParent) { TreeViewList_Int(TV, LoadList); }
        }

        /// <summary>populate ComboBox from List <string></summary>
        /// <param name="ComboBox cb"> </param>
        /// <param name=" List<string> LoadList"> </param>
        public void List_To_ComboBox(ComboBox cb, List<string> LoadList)
        {
            //Setup data binding
            cb.DataSource = LoadList;
            cb.DisplayMember = "Name";
            //cb.ValueMember = "Value";
        }

        /// <summary>populate ComboBox from List <int></summary>
        /// <param name="ComboBox cb"> </param>
        /// <param name=" List<string> LoadList"> </param>
        public void List_To_ComboBoxInt(ComboBox cb, List<int> LoadList)
        {
            //Setup data binding
            cb.DataSource = LoadList;
            cb.DisplayMember = "Name";
            //cb.ValueMember = "Value";
        }

        /// <summary>populate ListBox from List <string> (from any thread)</summary>
        /// <param name="ListBox listbox"> </param>
        /// <param name=" List<string> LoadList"> </param>
        /// <param name="Clear"> </param>
        public void List_To_ListBox(ListBox listbox, List<string> LoadList, bool Clear = true)
        {
            if (Clear)  // option to clear out previous listbox values before adding
            {
                if (listbox.InvokeRequired) { listbox.BeginInvoke((MethodInvoker)delegate () { listbox.Items.Clear(); }); }
                else { listbox.Items.Clear(); }
            }

            // update listbox values (from any thread) 
            if (listbox.InvokeRequired) { listbox.BeginInvoke((MethodInvoker)delegate () { listbox.Items.AddRange(LoadList.ToArray()); }); }
            else { listbox.Items.AddRange(LoadList.ToArray()); }
        }

        /// <summary>populate ListBox from List <int></summary>
        /// <param name="ListBox listbox"> </param>
        /// <param name=" List<int> LoadList"> </param>
        public void List_To_ListBoxInt(ListBox listbox, List<int> LoadList)
        {
            //Setup data binding
            listbox.DataSource = LoadList;
            listbox.DisplayMember = "Name";
            //cb.ValueMember = "Value";
        }


        #region === TreeView ===

        /// <summary>Load List<string> into TreeView (option to display under parent node) - If List Item contains "|" it splits by NodeText|NodeTag</summary>
        /// <param name="TV"> </param>
        /// <param name="LoadList"> </param>
        /// <param name="NodeParentName"> </param>
        public void TreeViewList(TreeView TV, List<string> LoadList, string NodeParentName = "")
        {
            // if node parent name provided in parameters, use that as Node header. otherwise no header
            bool NoParent = true; if (NodeParentName.Trim() != "") { NoParent = false; }

            // ### Load List in TreeView - UnderNeath "ParentName" ===
            if (!NoParent)
            {
                //==== Create Parent Node =============
                TreeNode parent = new TreeNode();
                parent.Text = NodeParentName;

                if (NodeParentName.Contains("|"))
                {
                    string[] words = NodeParentName.Split('|');
                    int i = 0;
                    foreach (string word in words)
                    {
                        if (i == 0) { parent.Text = word; }
                        if (i == 1) { parent.Tag = word; }
                        i++;
                    }

                    if (parent.Text.Trim() == "") { parent.Text = parent.Tag.ToString(); }
                }


                //==== Loop To Create Children Nodes =======
                foreach (string ListItem in LoadList)
                {
                    TreeNode child2 = new TreeNode();
                    child2.Text = ListItem;

                    if (ListItem.Contains("|"))
                    {
                        string[] words = ListItem.Split('|');
                        int i = 0;
                        foreach (string word in words)
                        {
                            if (i == 0) { child2.Text = word; }
                            if (i == 1) { child2.Tag = word; }
                            i++;
                        }

                        if (child2.Text.Trim() == "") { child2.Text = child2.Tag.ToString(); }
                    }

                    parent.Nodes.Add(child2);
                }

                TV.Nodes.Add(parent);  // add to treeview

            }


            // ### Load List In TreeView With No Parent ===
            if (NoParent)
            {
                //==== Loop To Create Parent Nodes =======
                foreach (string ListItem in LoadList)
                {
                    //==== Create Parent Node =============
                    TreeNode parent = new TreeNode();
                    parent.Text = ListItem;

                    if (ListItem.Contains("|"))
                    {
                        string[] words = ListItem.Split('|');
                        int i = 0;
                        foreach (string word in words)
                        {
                            if (i == 0) { parent.Text = word; }
                            if (i == 1) { parent.Tag = word; }
                            i++;
                        }
                    }


                    TV.Nodes.Add(parent);  // add to treeview    
                }
            }
        }

        /// <summary>Load List<int> into TreeView (option to display under parent node)</summary>
        /// <param name="TV"> </param>
        /// <param name="LoadList"> </param>
        /// <param name="NodeParentName"> </param>
        public void TreeViewList_Int(TreeView TV, List<int> LoadList, string NodeParentName = "")
        {
            // if node parent name provided in parameters, use that as Node header. otherwise no header
            bool NoParent = true; if (NodeParentName.Trim() != "") { NoParent = false; }

            // ### Load Lis in TreeView - UnderNeath "ParentName" ===
            if (!NoParent)
            {
                //==== Create Parent Node =============
                TreeNode parent = new TreeNode();
                parent.Text = NodeParentName;

                //==== Loop To Create Children Nodes =======
                foreach (int ListItem in LoadList)
                {
                    TreeNode child2 = new TreeNode();
                    child2.Text = ListItem.ToString();
                    parent.Nodes.Add(child2);
                }

                TV.Nodes.Add(parent);  // add to treeview

            }


            // ### Load List In TreeView With No Parent ===
            if (NoParent)
            {
                //==== Loop To Create Parent Nodes =======
                foreach (int ListItem in LoadList)
                {
                    //==== Create Parent Node =============
                    TreeNode parent = new TreeNode();
                    parent.Text = ListItem.ToString();
                    TV.Nodes.Add(parent);  // add to treeview    
                }
            }
        }


        #endregion



        #endregion

        public List<string> ListSORT(List<string> list)
        {
            if (list == null) { return new List<string>(); } // nothing to sort, return blank list

            string[] s = list.ToArray();
            Array.Sort(s);
            List<string> OutList = new List<string>();
            foreach (string t in s)
            {
                OutList.Add(t);
            }

            return OutList;
        }


        #region === Processes ===

        /// <summary>Returns List of all WinTitles with ProcessName</summary>
        /// <param name="processName">Name of process to seach for</param>
        public List<string> WinTitles_By_ProcessName(string processName = "mpc-hc64", bool ExactMatch = true)
        {
            List<string> MPC_WinTitles = new List<string>();

            Process[] processlist = Process.GetProcesses();

            foreach (Process process in processlist)
            {
                if (!String.IsNullOrEmpty(process.MainWindowTitle))
                {
                    if (process.ProcessName.ToUpper() == processName.ToUpper())
                    {
                        MPC_WinTitles.Add(process.MainWindowTitle);
                    }
                    else if (process.ProcessName.ToUpper().Contains(processName.ToUpper()))
                    {
                        MPC_WinTitles.Add(process.MainWindowTitle);
                    }

                    //Console.WriteLine("Process: {0} ID: {1} Window title: {2}", process.ProcessName, process.Id, process.MainWindowTitle);
                }
            }

            return MPC_WinTitles;
        }

        /// <summary>Returns List of ALL Processes Running on PC - If ProcessName Provided then Returns All Processes with that ProcessName</summary>
        /// <param name="ProcessName">Optional parameter to return all processes with this process name</param>
        public List<Process> ProcessList(string ProcessName = "")
        {
            List<Process> ProcessList = new List<Process>();

            // loop through system processes 
            foreach (Process theprocess in Process.GetProcesses())
            {
                if (!String.IsNullOrEmpty(theprocess.MainWindowTitle))
                {
                    try
                    {
                        //winInfo WinPositions = new winInfo();
                        //WinPositions = WinGetPos("ahk_PID " + theprocess.MainWindowHandle);

                        if (ProcessName == "") { ProcessList.Add(theprocess); }  // return all processes if Process Name not provided

                        if (ProcessName != "") { if (theprocess.ProcessName == ProcessName) { ProcessList.Add(theprocess); } } // return all processes if Process Name not provided
                    }
                    catch
                    {
                    }
                }
            }

            return ProcessList;
        }

        /// <summary>
        /// Returns EXE Path from Process
        /// </summary>
        /// <param name="process"></param>
        /// <returns></returns>
        public string ProcessPath(Process p)
        {
            return p.MainModule.FileName;
        }

        /// <summary>
        /// Returns Image from Process (Icon)
        /// </summary>
        /// <param name="process"></param>
        /// <returns></returns>
        public Image ProcessImage(Process p)
        {
            Icon ico = Icon.ExtractAssociatedIcon(p.MainModule.FileName);
            Image IMG = ico.ToBitmap();
            return IMG;
        }


        /// <summary>
        /// Searches for Process by Name
        /// </summary>
        /// <param name="ProcessName"></param>
        /// <returns></returns>
        public Process ReturnProcess(string ProcessName)
        {
            List<Process> ProcessList = new List<Process>();

            Process[] processlist = Process.GetProcesses();

            foreach (Process theprocess in processlist)
            {
                if (!String.IsNullOrEmpty(theprocess.MainWindowTitle))
                {
                    try
                    {
                        if (ProcessName != "") { if (theprocess.ProcessName.ToLower() == ProcessName.ToLower() || theprocess.ProcessName.ToLower() == ProcessName.ToLower() + ".exe") { return theprocess; } } // return all processes if Process Name not provided
                    }
                    catch
                    {
                    }
                }
            }

            return new Process();
        }


        #endregion


    }
}
