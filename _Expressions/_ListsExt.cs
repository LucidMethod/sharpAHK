using sharpAHK;
using StringExtension;
using System;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace AHKExpressions
{
    public static partial class AHKExpressions
    {
        // === Lists ===

        #region === Lists: Management ===

        /// <summary>Checks if value in is List<string>. Returns true if found.</summary>
        /// <param name="LIST">List of strings to search for existing value</param>
        /// <param name="ListItem">List value to check for in LIST</param>
        /// <param name="CaseSensitive">Determines whether value must match list item's case exactly before returning 'true'</param>
        public static bool InLIST(this List<string> LIST, string ListItem, bool CaseSensitive = false)
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
        public static List<string> Add(this List<string> LIST, string AddItem, bool CaseSensitive = false, bool OnlyDistinct = false)
        {
            if (AddItem == null) { return LIST; }

            if (LIST.Count > 0)
            {
                bool FoundInList = InLIST(LIST, AddItem, CaseSensitive);

                if (OnlyDistinct)
                {
                    if (!FoundInList) { LIST.Add(AddItem); }
                }
                else
                {
                    LIST.Add(AddItem);
                }
            }
            else
            {
                LIST.Add(AddItem);  // list is empty, just add new item
            }

            return LIST;
        }

        /// <summary>
        /// Takes List of Items and Returns List of Distinct Items Only (option to sort alpha)
        /// </summary>
        /// <param name="OriginalLIST"></param>
        /// <param name="SortAlpha"></param>
        /// <returns></returns>
        public static List<string> Distinct(this List<string> OriginalLIST, bool SortAlpha = false)
        {
            List<string> rList = new List<string>();

            _AHK ahk = new _AHK();
            foreach(string item in OriginalLIST)
            {
                if (!ahk.InLIST(OriginalLIST, item)) { rList.Add(item); }
            }

            if (SortAlpha) { rList = rList.ListSORT(); }

            return rList;
        }


        /// <summary>
        /// Merges two Lists, Returning Only Distinct List Values
        /// </summary>
        /// <param name="OriginalLIST"></param>
        /// <param name="AddToList"></param>
        /// <param name="SortAlpha"></param>
        /// <returns></returns>
        public static List<string> Distinct(this List<string> OriginalLIST, List<string> AddToList,  bool SortAlpha = false)
        {
            List<string> rList = new List<string>();
            _AHK ahk = new _AHK();
            foreach (string item in OriginalLIST)
            {
                if (!ahk.InLIST(rList, item)) { rList.Add(item); }
            }
            foreach (string item in AddToList)
            {
                if (!ahk.InLIST(rList, item)) { rList.Add(item); }
            }
            if (SortAlpha) { rList = rList.ListSORT(); }
            return rList;
        }


        /// <summary>
        /// Returns Only Distinct Values From List, Adds New Item to List if New Distinct Item
        /// </summary>
        /// <param name="OriginalLIST"></param>
        /// <param name="AddToList"></param>
        /// <param name="SortAlpha"></param>
        /// <returns></returns>
        public static List<string> Distinct(this List<string> OriginalLIST, string AddToList, bool SortAlpha = false)
        {
            List<string> rList = new List<string>();
            _AHK ahk = new _AHK();
            foreach (string item in OriginalLIST)
            {
                if (!ahk.InLIST(rList, item)) { rList.Add(item); }
            }
            if (!ahk.InLIST(rList, AddToList)) { rList.Add(AddToList); }
            
            if (SortAlpha) { rList = rList.ListSORT(); }
            return rList;
        }



        /// <summary>Returns List with items containing SearchText</summary>
        /// <param name="LIST">List of items to search for items containing SearchText</param>
        /// <param name="SearchText">Return list of items containg SearchText in list item</param>
        /// <param name="CaseSensitive">Determines list item must contain same case as SearchText before adding to return list</param>
        /// <returns>Returns original list minus excluded items list values</returns>
        public static List<string> Search(this List<string> LIST, string SearchText, bool CaseSensitive = false)
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
        public static List<string> Remove(this List<string> OriginalList, List<string> RemoveItems, bool CaseSensitive = false, bool RemoveBlanks = true)
        {
            List<string> returnList = new List<string>();

            foreach (string Item in OriginalList)
            {
                if (RemoveBlanks) { if (Item == "") { continue; } } // skip adding blank values to return list

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
        public static List<string> Remove(this List<string> OriginalList, string RemoveItem, bool CaseSensitive = false, bool RemoveBlanks = true)
        {
            List<string> returnList = new List<string>();

            foreach (string Item in OriginalList)
            {
                if (RemoveBlanks) { if (Item == "") { continue; } } // skip adding blank values to return list

                bool Add = true;

                if (CaseSensitive) { if (Item.Trim() == RemoveItem.Trim()) { Add = false; } }
                if (!CaseSensitive) { if (Item.Trim().ToUpper() == RemoveItem.Trim().ToUpper()) { Add = false; } }

                if (Add) { returnList.Add(Item); }
            }

            return returnList;
        }

        /// <summary>Return list that does not contain any items with 'ExcludeText' in itemstring</summary>
        /// <param name="OriginalList">List of items to search and remove items from</param>
        /// <param name="ExcludeText">Return list excluding items containing this text</param>
        /// <param name="CaseSensitive">Determines whether ExcludeText must be case sensitive match to OriginalList item before removing</param>
        public static List<string> RemoveContaining(this List<string> OriginalList, string ExcludeText, bool CaseSensitive = false)
        {
            List<string> NewList = new List<string>();

            foreach (string project in OriginalList)
            {
                if (!CaseSensitive) { if (!project.ToUpper().Contains(ExcludeText.ToUpper())) { NewList.Add(project); } }
                if (CaseSensitive) { if (!project.Contains(ExcludeText.ToUpper())) { NewList.Add(project); } }
            }

            return NewList;
        }


        /// <summary>Return list that does not contain any items in containing text in ExcludeTextList</summary>
        /// <param name="OriginalList">List of items to search and remove items from</param>
        /// <param name="ExcludeTextList">List of text to exclude from original list</param>
        /// <param name="CaseSensitive">Determines whether ExcludeText must be case sensitive match to OriginalList item before removing</param>
        public static List<string> RemoveContaining(this List<string> OriginalList, List<string> ExcludeTextList, bool CaseSensitive = false)
        {
            List<string> NewList = new List<string>();

            foreach (string project in OriginalList)
            { 
                foreach(string ExcludeText in ExcludeTextList)
                {
                    if (!CaseSensitive) { if (!project.ToUpper().Contains(ExcludeText.ToUpper())) { NewList.Add(project); } }
                    if (CaseSensitive) { if (!project.Contains(ExcludeText.ToUpper())) { NewList.Add(project); } }
                }
            }

            return NewList;
        }


        /// <summary>Returns new list merging two lists together, with the option to exclude duplicate items</summary>
        /// <param name="MainList">Existing list to add to</param>
        /// <param name="AddList">List to add to MainList, returning combined list</param>
        /// <param name="ExcludeDuplicates">Option to not add items from AddList already found in MainList</param>
        /// <param name="CaseSensitive">When ExcludeDuplicates is True, determines whether AddList item must match MainList item's case before excluding from merged list</param>
        /// <returns>Returns new list merging two lists together, with the option to exclude duplicate items</returns>
        public static List<string> Merge(this List<string> MainList, List<string> AddList, bool ExcludeDuplicates = false, bool CaseSensitive = false)
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


        /// <summary>Returns list sorted alphabetically</summary>
        /// <param name="list">List to sort alphabetically</param>
        public static List<string> ListSORT(this List<string> list)
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

        /// <summary>Returns List Sorted Alphabetically</summary>
        /// <param name="list">List to Sort Alphabetically (A-Z)</param>
        /// <param name="ReverseOrder">Option to Reverse Order of Alpha List (Z-A)</param>
        public static List<string> SortAlpha(this List<string> list, bool ReverseOrder = false)
        {
            if (list == null) { return new List<string>(); } // nothing to sort, return blank list

            string[] s = list.ToArray();
            Array.Sort(s);
            List<string> OutList = new List<string>();
            foreach (string t in s)
            {
                OutList.Add(t);
            }

            if (ReverseOrder) { return Reverse(OutList); }

            return OutList;
        }


        // { Needs Testing }  - need to loop again to see if there is anything in B not found in A to add to diffList.. TODO

        /// <summary>Compare two lists, return values found in List A NOT Found in List B</summary>
        public static List<string> ListDIFF(this List<string> ListA, List<string> ListB)
        {
            List<string> diffList = new List<string>();

            foreach (string item in ListA)  // list of ssrs from test not found in prod
            {
                bool Found = InLIST(ListB, item);
                if (!Found) { diffList.Add(item); }
            }

            return diffList;
        }

        /// <summary>
        /// Returns Random List (aka Shuffled)
        /// </summary>
        /// <param name="OriginalList">List to Shuffle</param>
        /// <returns></returns>
        public static List<string> Shuffle(this List<string> OriginalList)
        {
            System.Random _random = new System.Random();
            List<string> randomList = new List<string>();

            int n = OriginalList.Count;
            for (int i = 0; i < n; i++)
            {
                int r = i + (int)(_random.NextDouble() * (n - i));
                randomList.Add(OriginalList[r]);
                OriginalList[r] = OriginalList[i];
                OriginalList[i] = OriginalList[r];
            }

            return OriginalList;
        }


        /// <summary>
        /// Returns the Same List in Reverse Order
        /// </summary>
        /// <param name="aList"></param>
        /// <returns></returns>
        public static List<string> Reverse(this List<string> aList)
        {
            List<string> rList = new List<string>();
            if (aList == null) { return rList; }
            if (aList.Count < 2) { rList = aList; }
            else
            {
                int i = aList.Count - 1;
                do
                {
                    rList.Add(aList[i]); 
                    i--;
                } while (i != -1);
            }

            return rList;
        }




        #endregion

        #region === Lists: Return From ===

        /// <summary>Return Position # of SearchTerm in LIST<string></summary>
        /// <param name="LIST">List to search for SearchTerm Item</param>
        /// <param name="SearchTerm">Term to Match in LIST, returns position of item in LIST</param>
        /// <param name="CaseSensitive">Determines whether SearchTerm must match LIST item's case exactly before considering terms a match</param>
        /// <returns>Returns int indicating where SearchTerm was located in LIST, -1 returned if SearchItem not found</returns>
        public static int List_ItemPosition(this List<string> LIST, string SearchTerm, bool CaseSensitive = false)
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
        public static int List_Int_ItemPosition(this List<int> LIST, int SearchInt)
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
        public static string List_ItemValue(this List<string> LIST, int ListPosition)
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
        public static int Return_List_ValueInt(this List<int> list, int ListPosition)
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
        public static string List_FirstItem(this List<string> LIST)
        {
            if (LIST == null || LIST.Count == 0) { return null; } // null return if no values found in LIST

            foreach (string item in LIST) { return item; } // return first item found in list

            return null; // otherwise null response
        }

        /// <summary>returns last item added to list</summary>
        /// <param name="List<string> list"> </param>
        public static string Last_List_Item(this List<string> list)
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
        public static string List_LastItem(this List<string> LIST)
        {
            if (LIST == null || LIST.Count == 0) { return null; } // null return if no values found in LIST

            string lastItem = "";
            foreach (string item in LIST) { lastItem = item; } // loop to last item, store that value to return
            return lastItem;
        }


        /// <summary>Return list split by SplitChar (ex: ",") as new string</summary>
        /// <param name="LIST">List to convert to string</param>
        /// <param name="SplitChar">Character to place between list items in string return</param>
        public static string List_ToString(this List<string> LIST, string SplitChar = ",")
        {
            if (LIST == null || LIST.Count == 0) { return ""; } // return empty string if no values found in LIST

            // Join strings into one CSV line.
            string line = string.Join(SplitChar, LIST.ToArray());

            return line;
        }

        /// <summary>return list to string, each item on new line</summary>
        /// <param name="List<string> list"> </param>
        public static string List_ToStringLines(this List<string> LIST)
        {
            if (LIST == null || LIST.Count == 0) { return ""; } // return empty string if no values found in LIST

            // Join list items into string, items separated by new lines
            string LineCode = List_ToString(LIST, Environment.NewLine);

            return LineCode;
        }

        #endregion

        #region === Lists: Populate ===

        /// <summary>Converts text from a text file to list <string></summary>
        /// <param name="TextString"> </param>
        /// <param name="SkipBlankLines"> </param>
        /// <param name="Trim"> </param>
        /// <param name="SkipCommentLines"> </param>
        public static List<string> toList(this string TextString, bool SkipBlankLines = true, bool Trim = true, bool SkipCommentLines = true)
        {
            List<string> list = new List<string>();
            _AHK ahk = new _AHK();
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
                            string First2 = ahk.FirstCharacters(line, 2);
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

        /// <summary>Converts numbers from a text file to list <int></summary>
        /// <param name="TextString"> </param>
        /// <param name="SkipBlankLines"> </param>
        /// <param name="SkipCommentLines"> </param>
        public static List<int> toList(this int INT, bool SkipBlankLines = true, bool SkipCommentLines = true)
        {
            List<int> list = new List<int>();
            bool Trim = true;
            _AHK ahk = new _AHK();

            // parse by new line
            {
                // Creates new StringReader instance from System.IO
                using (StringReader reader = new StringReader(INT.ToString()))
                {
                    // Loop over the lines in the string.
                    string line;
                    while ((line = reader.ReadLine()) != null)
                    {
                        if (SkipCommentLines)
                        {
                            string First2 = ahk.FirstCharacters(line, 2); // skip over lines if they are comments
                            if (First2 == @"//") { continue; }
                        }

                        string writeline = line;

                        if (Trim) { writeline = line.Trim(); } // trim leading spaces

                        if (SkipBlankLines) { if (writeline == "") { continue; } }

                        int WriteInt = ahk.ToInt(writeline);   // convert string from text to int

                        list.Add(WriteInt);
                    }
                }

            }
            return list;
        }

        /// <summary>Read text file, return list <string></summary>
        /// <param name="FilePath"> </param>
        /// <param name="SkipBlankLines"> </param>
        /// <param name="Trim"> </param>
        /// <param name="SkipCommentLines"> </param>
        public static List<string> TextFile_ToList(this string FilePath, bool SkipBlankLines = true, bool Trim = true, bool SkipCommentLines = true)
        {
            _AHK ahk = new _AHK();
            if (File.Exists(FilePath))
            {
                string ParseCode = ahk.FileRead(FilePath);
                List<string> list = ParseCode.toList(SkipBlankLines, Trim, SkipCommentLines);
                return list;
            }
            else
            {
                return null;
            }
        }

        /// <summary>Read text file, return list <int></summary>
        /// <param name="FilePath"> </param>
        public static List<int> TextFile_ToListInt(this string FilePath)
        {
            _AHK ahk = new _AHK();
            string ListTxt = ahk.FileRead(FilePath);
            List<int> list = new List<int>();

            // parse by new line
            {
                string[] lines = ListTxt.Split(Environment.NewLine.ToCharArray());
                foreach (string line in lines)
                {
                    string First2 = ahk.FirstCharacters(line, 2); // skip over lines if they are comments
                    if (First2 == @"//") { continue; }
                    string writeline = line.Trim();  // trim leading spaces
                    if (writeline == "") { continue; }
                    int lineInt = ahk.ToInt(writeline);   // convert line to integer
                    list.Add(lineInt);
                }
            }
            return list;
        }

        /// <summary>Convert Array[] to List</summary>
        /// <param name="arr">Array to convert to List</param>
        public static List<string> Array_ToList(this string[] arr)
        {
            List<string> list = new List<string>(arr); // Copy array values to List.
            return list;
        }

        /// <summary>Convert List to Array[]</summary>
        /// <param name="List<string> list"> </param>
        public static string[] List_ToArray(this List<string> list)
        {
            string[] s = list.ToArray();
            return s;
        }

        /// <summary>Returns list of Dictionary Keys <string></summary>
        /// <param name="Dictionary<string"> </param>
        /// <param name=" string> dictionary"> </param>
        public static List<string> Dict_KeyList(this Dictionary<string, string> dictionary)
        {
            // Get a List of all the Keys.
            List<string> keys = new List<string>(dictionary.Keys);
            return keys;
        }

        /// <summary>Returns list of Dictionary Keys <int></summary>
        /// <param name="Dictionary<int"> </param>
        /// <param name=" string> dictionary"> </param>
        public static List<int> Dict_KeyList(this Dictionary<int, string> dictionary)
        {
            // Get a List of all the Keys.
            List<int> keys = new List<int>(dictionary.Keys);
            return keys;
        }

        /// <summary>Returns list of Dictionary Values <string></summary>
        /// <param name="Dictionary<string"> </param>
        /// <param name=" string> dictionary"> </param>
        public static List<string> Dict_ValueList(this Dictionary<string, string> dictionary)
        {
            // Get a List of all the values
            List<string> returnlist = new List<string>(dictionary.Values);
            return returnlist;
        }

        /// <summary>Returns list of Dictionary Values <string></summary>
        /// <param name="Dictionary<string"> </param>
        /// <param name=" int> dictionary"> </param>
        public static List<int> Dict_ValueList(this Dictionary<string, int> dictionary)
        {
            // Get a List of all the values
            List<int> returnlist = new List<int>(dictionary.Values);
            return returnlist;
        }


        //=== File Path Lists ===

        // ex: List<string> FileList = lst.DirList(file, "*.sqlite", true); 


        /// <summary>Returns List<string> of Image File Paths in Directory Path (JPG, JPEG, GIF, ICO, PNG)</summary>
        /// <param name="DirPath"> </param>
        public static List<string> DirList_Images(this string DirPath, bool Recurse = true)
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
        public static List<string> FileList_SortedAlpha_ByFileName(this string DirPath, string ExtTypes = "*.*", bool Recurse = true)
        {
            _AHK ahk = new _AHK();
            string[] files = Directory.GetFiles(DirPath, ExtTypes, System.IO.SearchOption.AllDirectories);

            if (!Recurse) { files = Directory.GetFiles(DirPath, ExtTypes, System.IO.SearchOption.TopDirectoryOnly); }

            // sort list alpha first
            List<string> filelist = new List<string>(files);
            filelist.Sort();

            List<string> filelistSort = new List<string>();
            foreach (string fil in filelist)  // loop through list items
            {
                filelistSort.Add(ahk.FileName(fil) + "|" + fil);
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

        // TODO: Finish 

        /// <summary>returns List<string> of files in directory path modified today</summary>
        /// <param name="DirPath"> </param>
        /// <param name="SearchPattern"> </param>
        /// <param name="Recurse"> </param>
        /// <param name="FileNameOnly"> </param>
        /// <param name="IncludeExt"> </param>
        public static List<string> FileList_Modified_Today(this string DirPath, string SearchPattern = "*.*", bool Recurse = true, bool FileNameOnly = false, bool IncludeExt = true)
        {
            List<string> FileList = new List<string>();
            _AHK ahk = new _AHK();

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
                    if (FileNameOnly) { addFile = ahk.FileName(file); }
                    if ((FileNameOnly) && (!IncludeExt)) { addFile = ahk.FileNameNoExt(file); }
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
        public static List<string> FileList_Modified_Since(this string DirPath, DateTime Since, string SearchPattern = "*.*", bool Recurse = true, bool FileNameOnly = false, bool IncludeExt = true)
        {
            List<string> FileList = new List<string>();
            _AHK ahk = new _AHK();
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
                    if (FileNameOnly) { addFile = ahk.FileName(file); }
                    if ((FileNameOnly) && (!IncludeExt)) { addFile = ahk.FileNameNoExt(file); }
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
        public static List<string> List_Drives()
        {
            List<string> Drives = new List<string>();

            foreach (System.IO.DriveInfo di in System.IO.DriveInfo.GetDrives())
                Drives.Add(di.Name);

            return Drives;
        }


        ///  CSV
        /// <summary>convert list to csv formatted string</summary>
        /// <param name="List<string> list"> </param>
        public static string List_ToCSV(this List<string> list)
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
        public static List<string> CSV_ToList(this string CSV, string ParseBy = ",")
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


        /// <summary>Returns List of Controls on a Form as a Control List</summary>
        /// <param name="FormName">WinForm to return list of Controls From</param>
        public static List<Control> ControlList(this Control FormName)
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


        /// <summary>
        /// Regex Image Links from HTML to List
        /// </summary>
        /// <param name="HTML"></param>
        /// <returns></returns>
        public static List<string> ImageLinks(this string HTML)
        {
            List<string> matches = new List<string>();

            if (HTML == null || HTML.Trim() == "") { return matches; }

            string cmd = @"(http|ftp|https)://([\w_-]+(?:(?:\.[\w_-]+)+))([\w.,@?^=%&:/~+#-]*[\w@?^=%&/~+#-])?";

            Regex ItemRegex = new Regex(cmd, RegexOptions.IgnoreCase);
            foreach (Match ItemMatch in ItemRegex.Matches(HTML))
            {
                //Console.WriteLine(ItemMatch);
                string val = ItemMatch.Value;

                if (val.ToUpper().Contains(".JPG") || val.ToUpper().Contains(".JPEG") || val.ToUpper().Contains(".PNG"))
                {
                    //ahk.MsgBox(val); 
                    matches.Add(val);
                }
            }
            return matches;
        }

        /// <summary>
        /// Parse Standard URL For FileName at End of Path
        /// </summary>
        /// <param name="urlLink">URL to Parse</param>
        /// <returns>Returns Last Section of URL - Usually File Name</returns>
        public static string LinkFileName(this string urlLink)
        {
            _AHK ahk = new _AHK();
            return ahk.StringSplit(urlLink, "/", 0, true);
        }


        /// <summary>
        /// Parse HTML for List of RapidGator Links
        /// </summary>
        /// <param name="HTML">Text/HTML to Parse for Links</param>
        /// <returns>Returns List of RapidGator Links</returns>
        public static List<string> RgLinks(this string HTML)
        {
            List<string> matches = new List<string>();
            string cmd = @"(http|ftp|https)://([\w_-]+(?:(?:\.[\w_-]+)+))([\w.,@?^=%&:/~+#-]*[\w@?^=%&/~+#-])?";
            Regex ItemRegex = new Regex(cmd, RegexOptions.IgnoreCase);
            foreach (Match ItemMatch in ItemRegex.Matches(HTML))
            {
                //Console.WriteLine(ItemMatch);
                string val = ItemMatch.Value;

                if (val.ToUpper().Contains("RAPIDGATOR.NET")) { matches.Add(val); }
                if (val.ToUpper().Contains("RG.NET")) { matches.Add(val); }
                if (val.ToUpper().Contains("RG.TO")) { matches.Add(val); }
                if (val.ToUpper().Contains("SAFELINKING.NET")) { matches.Add(val); }
            }
            return matches;
        }


        /// <summary>
        /// Parse HTML for List of .Zip Links
        /// </summary>
        /// <param name="HTML">Text/HTML to Parse for Links</param>
        /// <returns>Returns List of RapidGator Links</returns>
        public static List<string> ZipLinks(this string HTML)
        {
            List<string> matches = new List<string>();
            string cmd = @"(http|ftp|https)://([\w_-]+(?:(?:\.[\w_-]+)+))([\w.,@?^=%&:/~+#-]*[\w@?^=%&/~+#-])?";
            Regex ItemRegex = new Regex(cmd, RegexOptions.IgnoreCase);
            foreach (Match ItemMatch in ItemRegex.Matches(HTML))
            {
                //Console.WriteLine(ItemMatch);
                string val = ItemMatch.Value;

                if (val.ToUpper().Contains(".ZIP")) { matches.Add(val); }
            }
            return matches;
        }

        /// <summary>
        /// Parse HTML for List of IMDb.com Links
        /// </summary>
        /// <param name="HTML"></param>
        /// <returns></returns>
        public static List<string> IMDbLinks(this string HTML)
        {
            List<string> matches = new List<string>();
            string cmd = @"(http|ftp|https)://([\w_-]+(?:(?:\.[\w_-]+)+))([\w.,@?^=%&:/~+#-]*[\w@?^=%&/~+#-])?";
            Regex ItemRegex = new Regex(cmd, RegexOptions.IgnoreCase);
            foreach (Match ItemMatch in ItemRegex.Matches(HTML))
            {
                string val = ItemMatch.Value;
                if (val.ToUpper().Contains("IMDB.COM")) { matches.Add(val); }
            }
            return matches;
        }

        /// <summary>
        /// Parses HTML, Returns list of Links Containing SiteURL in the Link
        /// </summary>
        /// <param name="HTML"></param>
        /// <param name="SiteURL">Part of the Site URL Required in Text to Be Considered a Match</param>
        /// <returns></returns>
        public static List<string> SiteLinks(this string HTML, string SiteURL, string SiteURL2 = "", string SiteURL3 = "")
        {
            List<string> matches = new List<string>();
            string cmd = @"(http|ftp|https)://([\w_-]+(?:(?:\.[\w_-]+)+))([\w.,@?^=%&:/~+#-]*[\w@?^=%&/~+#-])?";
            Regex ItemRegex = new Regex(cmd, RegexOptions.IgnoreCase);
            foreach (Match ItemMatch in ItemRegex.Matches(HTML))
            {
                string val = ItemMatch.Value;
                if (val.ToUpper().Contains(SiteURL.ToUpper().Trim())) { matches.Add(val); }
                if (SiteURL2 != "") { if (val.ToUpper().Contains(SiteURL2.ToUpper().Trim())) { matches.Add(val); } }
                if (SiteURL3 != "") { if (val.ToUpper().Contains(SiteURL3.ToUpper().Trim())) { matches.Add(val); } }
            }
            return matches;
        }

        /// <summary>
        /// Returns Text Between Two <XML>Tags</XML>
        /// </summary>
        /// <param name="Text">String Containg XML Content with Tag</param>
        /// <param name="Tag">XML Tag to Return Text From</param>
        /// <returns></returns>
        public static string XML_Text(this string XMLText, string Tag = "<h5>")
        {
            if (!XMLText.Contains(Tag)) { return ""; }

            if (!Tag.Contains("<") && !Tag.Contains(">")) { Tag = "<" + Tag + ">"; }
            string end = Tag.Replace("<", "</");

            int anstart = (XMLText.ToString().IndexOf(Tag) + Tag.Length);
            int anend = (XMLText.ToString().IndexOf(end));
            int anlen = 0;
            if (anstart > 0 && anend > 0)
            {
                anlen = anend - anstart;
            }

            string Asset = XMLText.ToString().Substring(anstart, anlen);
            return Asset;
        }


        #region === Lists: Display  ===

        //  Display Lists

        // List of Tables in SQLite File to GridView Display
        // Ex: lst.List_To_Grid(dataGridView1, sqlite.TableList(DbFile), "TABLES"); 

        /// <summary>populate DataGridView from List <string></summary>
        /// <param name="dv"> </param>
        /// <param name=" List<string> list"> </param>
        /// <param name="ListName"> </param>
        /// <param name="AddCheckBox"> </param>
        public static void List_To_Grid(this DataGridView dv, List<string> list, string ListName = "List_View", bool AddCheckBox = false)
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
        public static void List_To_Grid(this DataGridView dv, List<int> list, string ListName = "List_View")
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
        public static void List_To_TreeView(this TreeView TV, List<string> LoadList, string ParentName = "List", bool cLearTV = true)
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
        public static void List_To_TreeView(this TreeView TV, List<int> LoadList, string ParentName = "List", bool NoParent = false, bool cLearTV = true)
        {
            if (cLearTV) { TV.Nodes.Clear(); }

            if (!NoParent) { TreeViewList(TV, LoadList, ParentName); }

            if (NoParent) { TreeViewList(TV, LoadList); }
        }

        /// <summary>populate ComboBox from List <string></summary>
        /// <param name="ComboBox cb"> </param>
        /// <param name=" List<string> LoadList"> </param>
        public static void List_To_ComboBox(this ComboBox cb, List<string> LoadList)
        {
            //Setup data binding
            cb.DataSource = LoadList;
            cb.DisplayMember = "Name";
            //cb.ValueMember = "Value";
        }

        /// <summary>populate ComboBox from List <int></summary>
        /// <param name="ComboBox cb"> </param>
        /// <param name=" List<string> LoadList"> </param>
        public static void List_To_ComboBox(this ComboBox cb, List<int> LoadList)
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
        public static void List_To_ListBox(this ListBox listbox, List<string> LoadList, bool Clear = true)
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
        public static void List_To_ListBox(this ListBox listbox, List<int> LoadList)
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
        public static void TreeViewList(this TreeView TV, List<string> LoadList, string NodeParentName = "")
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
        public static void TreeViewList(this TreeView TV, List<int> LoadList, string NodeParentName = "")
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


        #endregion

        #region === Randomize ===
        //source: https://stackoverflow.com/questions/273313/randomize-a-listt

        private static Random rng = new Random();

        /// <summary>
        /// Shuffle Contents of List 
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="list"></param>
        public static void Shuffle<T>(this IList<T> list)
        {
            int n = list.Count;
            while (n > 1)
            {
                n--;
                int k = rng.Next(n + 1);
                T value = list[k];
                list[k] = list[n];
                list[n] = value;
            }
        }


        #endregion

        #region === List: Convert ===

        /// <summary>
        /// Convert List to DataTable
        /// </summary>
        /// <param name="dv"></param>
        /// <param name="list"></param>
        /// <param name="ListName"></param>
        /// <param name="AddCheckBox"></param>
        public static DataTable ToDT(this List<string> list, string ListHeader = "ListView", bool AddCheckBox = false, string CheckBoxHeader = "Selected")
        {
            //======= Create DataTable and Assign to DataGrid  =======
            DataTable dt = new DataTable();

            if (!AddCheckBox)
            {
                dt.Columns.Add(ListHeader, typeof(String));                     // Create Columns

                foreach (string item in list)
                {
                    dt.Rows.Add(new object[] { item });
                }
            }

            if (AddCheckBox)  // option to add checkboxes to first column of datagridview
            {
                dt.Columns.Add(CheckBoxHeader, typeof(bool));
                dt.Columns.Add(ListHeader, typeof(String));

                foreach (string item in list)
                {
                    dt.Rows.Add(new object[] { false, item });
                }
            }

            return dt;
        }


        #endregion


        // === Dictionary ===

        //#region === Dictionary Actions ===

        ///// <summary>Sort Dictionary by Value</summary>
        ///// <param name="dict">Dictionary to Sort</param>
        ///// <param name="Reverse">Reverses the Alpha Sort from A-Z to Z-A</param>
        //public static Dictionary<string, string> SortByValue(this Dictionary<string, string> dict, bool Reverse = false)
        //{
        //    Dictionary<string, string> returndict = new Dictionary<string, string>();

        //    // Order by values.
        //    // ... Use LINQ to specify sorting by value.
        //    var items = from pair in dict
        //                orderby pair.Value ascending
        //                select pair;

        //    if (Reverse)
        //    {
        //        // Reverse sort.
        //        // ... Can be looped over in the same way as above.
        //        items = from pair in dict
        //                orderby pair.Value descending
        //                select pair;
        //    }

        //    // Display results.
        //    foreach (KeyValuePair<string, string> pair in items)
        //    {
        //        //Console.WriteLine("{0}: {1}", pair.Key, pair.Value);

        //        returndict.Add(pair.Key, pair.Value);
        //    }

        //    return returndict;
        //}


        ///// <summary>Sort Dictionary by Key</summary>
        ///// <param name="dict">Dictionary to Sort</param>
        ///// <param name="Reverse">Reverses the Alpha Sort from A-Z to Z-A</param>
        //public static Dictionary<string, string> SortByKey(this Dictionary<string, string> dict, bool Reverse = false)
        //{
        //    Dictionary<string, string> returndict = new Dictionary<string, string>();

        //    // Order by values.
        //    // ... Use LINQ to specify sorting by value.
        //    var items = from pair in dict
        //                orderby pair.Key ascending
        //                select pair;

        //    if (Reverse)
        //    {
        //        // Reverse sort.
        //        // ... Can be looped over in the same way as above.
        //        items = from pair in dict
        //                orderby pair.Key descending
        //                select pair;
        //    }

        //    // Display results.
        //    foreach (KeyValuePair<string, string> pair in items)
        //    {
        //        //Console.WriteLine("{0}: {1}", pair.Key, pair.Value);

        //        returndict.Add(pair.Key, pair.Value);
        //    }

        //    return returndict;
        //}



        ///// <summary>Converts Dictionary to DataTable</summary>
        ///// <param name="dictionary">Dictionary to Convert</param>
        ///// <param name="KeyField">DataTable Column Header for Key Column</param>
        ///// <param name="ValueField">DataTable Column Header for Value Column</param>
        //public static DataTable ToDataTable(this Dictionary<string, string> dictionary, string KeyField = "Key", string ValueField = "Value")
        //{
        //    // Here we create a DataTable with four columns.
        //    DataTable table = new DataTable();
        //    table.Columns.Add(KeyField, typeof(string));
        //    table.Columns.Add(ValueField, typeof(string));

        //    // Use var keyword to enumerate dictionary.
        //    foreach (var pair in dictionary)
        //    {
        //        table.Rows.Add(pair.Key, pair.Value);
        //    }

        //    return table;
        //}


        ///// <summary>
        ///// Returns Dictionary Value from Key
        ///// </summary>
        ///// <param name="Dict"></param>
        ///// <param name="KeyName"></param>
        ///// <returns></returns>
        //public static string Value(this Dictionary<string, string> Dict, string KeyName)
        //{
        //    string val = "";

        //    // See whether Dictionary contains this string.
        //    if (Dict.ContainsKey(KeyName)) { val = Dict[KeyName]; }

        //    return val;
        //}

        ///// <summary>Returns list<string> of Keys from Dictionary
        ///// <param name="dictionary"></param>
        ///// /// <param name="SortAlpha">Option to Sort Return List Alphabetically</param>
        //public static List<string> KeyList(this Dictionary<string, string> dictionary, bool SortAlpha = false)
        //{
        //    _AHK ahk = new _AHK();

        //    // Get a List of all the Keys.
        //    List<string> keys = new List<string>(dictionary.Keys);

        //    if (SortAlpha) { keys = ahk.ListSORT(keys); }

        //    return keys;
        //}


        ///// <summary>Returns List of Values from Dictionary </summary>
        ///// <param name="dictionary"></param>
        ///// <param name="SortAlpha">Option to Sort Return List Alphabetically</param>
        //public static List<string> ValueList(this Dictionary<string, string> dictionary, bool SortAlpha = false)
        //{
        //    _AHK ahk = new _AHK();
        //    // Get a List of all the values
        //    List<string> values = new List<string>(dictionary.Values);
        //    //if (SortAlpha) { values =  values.ListSORT(); }
        //    return values;
        //}


        //#endregion


        #region === Return From Dictionary ===

        
        public static bool ContainsKey(this Dictionary<string, string> Dict, string KeyName)
        {
            // See whether Dictionary contains this string.
            if (Dict.ContainsKey(KeyName)) { return true; }
            return false;
        }


        /// <summary>
        /// Returns Dictionary Value from Key
        /// </summary>
        /// <param name="Dict"></param>
        /// <param name="KeyName"></param>
        /// <returns></returns>
        public static string Value(this Dictionary<string, string> Dict, string KeyName)
        {
            string val = "";

            // See whether Dictionary contains this string.
            if (Dict.ContainsKey(KeyName)) { val = Dict[KeyName]; }

            return val;
        }

        /// <summary>
        /// Returns List of Keys from Dictionary
        /// </summary>
        /// <param name="dictionary"> </param>
        public static List<string> Keys(this Dictionary<string, string> dictionary)
        {
            // Get a List of all the Keys.
            List<string> keys = new List<string>(dictionary.Keys);
            return keys;
        }
        /// <summary>returns list<int> of Keys from Dictionary </summary>
        /// <param name="Dictionary<int"> </param>
        /// <param name=" string> dictionary"> </param>
        public static List<int> Keys(this Dictionary<int, string> dictionary)
        {
            // Get a List of all the Keys.
            List<int> keys = new List<int>(dictionary.Keys);
            return keys;
        }
        /// <summary>returns list<int> of Keys from Dictionary </summary>
        /// <param name="Dictionary<int"> </param>
        /// <param name=" string> dictionary"> </param>
        public static List<string> Keys(this Dictionary<string, int> dictionary)
        {
            // Get a List of all the Keys.
            List<string> keys = new List<string>(dictionary.Keys);
            return keys;
        }



        /// <summary> Returns list<string> of Values from Dictionary </summary>
        /// <param name="Dictionary<string"> </param>
        /// <param name=" string> dictionary"> </param>
        public static List<string> Values(this Dictionary<string, string> dictionary)
        {
            // Get a List of all the values
            List<string> values = new List<string>(dictionary.Values);
            return values;
        }

        /// <summary>returns list<int> of Values from Dictionary </summary>
        /// <param name="Dictionary<string"> </param>
        /// <param name=" int> dictionary"> </param>
        public static List<int> Values(this Dictionary<string, int> dictionary)
        {
            // Get a List of all the values
            List<int> values = new List<int>(dictionary.Values);
            return values;
        }

        /// <summary>Converts Dictionary<string,string> to DataTable</summary>
        /// <param name="Dictionary<string"> </param>
        /// <param name=" string> dictionary"> </param>
        /// <param name="KeyField"> </param>
        /// <param name="ValueField"> </param>
        public static DataTable ToDataTable(this Dictionary<string, string> dictionary, string KeyField = "Key", string ValueField = "Value")
        {
            // Here we create a DataTable with four columns.
            DataTable table = new DataTable();
            table.Columns.Add(KeyField, typeof(string));
            table.Columns.Add(ValueField, typeof(string));

            // Use var keyword to enumerate dictionary.
            foreach (var pair in dictionary)
            {
                table.Rows.Add(pair.Key, pair.Value);
            }

            return table;
        }

        /// <summary>Converts Dictionary<int,string> to DataTable</summary>
        /// <param name="Dictionary<string"> </param>
        /// <param name=" string> dictionary"> </param>
        /// <param name="KeyField"> </param>
        /// <param name="ValueField"> </param>            
        public static DataTable ToDataTable(this Dictionary<int, string> dictionary, string KeyField = "Key", string ValueField = "Value")
        {
            // Here we create a DataTable with four columns.
            DataTable table = new DataTable();
            table.Columns.Add(KeyField, typeof(string));
            table.Columns.Add(ValueField, typeof(string));

            // Use var keyword to enumerate dictionary.
            foreach (var pair in dictionary)
            {
                table.Rows.Add(pair.Key, pair.Value);
            }

            return table;
        }


        #endregion


        #region === Dictionary Actions ===

        /// <summary>Sort Dictionary<string, string> by Value</summary>
        /// <param name="Dictionary<string"> </param>
        /// <param name=" string> dict"> </param>
        /// <param name="Reverse"> </param>
        public static Dictionary<string, string> SortByValue(this Dictionary<string, string> dict, bool Reverse = false)
        {
            Dictionary<string, string> returndict = new Dictionary<string, string>();

            // Order by values.
            // ... Use LINQ to specify sorting by value.
            var items = from pair in dict
                        orderby pair.Value ascending
                        select pair;

            if (Reverse)
            {
                // Reverse sort.
                // ... Can be looped over in the same way as above.
                items = from pair in dict
                        orderby pair.Value descending
                        select pair;
            }

            // Display results.
            foreach (KeyValuePair<string, string> pair in items)
            {
                //Console.WriteLine("{0}: {1}", pair.Key, pair.Value);

                returndict.Add(pair.Key, pair.Value);
            }

            return returndict;
        }


        /// <summary>Sort Dictionary<string, string> by Key</summary>
        /// <param name="Dictionary<string"> </param>
        /// <param name=" string> dict"> </param>
        /// <param name="Reverse"> </param>
        public static Dictionary<string, string> SortByKey(this Dictionary<string, string> dict, bool Reverse = false)
        {
            Dictionary<string, string> returndict = new Dictionary<string, string>();

            // Order by values.
            // ... Use LINQ to specify sorting by value.
            var items = from pair in dict
                        orderby pair.Key ascending
                        select pair;

            if (Reverse)
            {
                // Reverse sort.
                // ... Can be looped over in the same way as above.
                items = from pair in dict
                        orderby pair.Key descending
                        select pair;
            }

            // Display results.
            foreach (KeyValuePair<string, string> pair in items)
            {
                //Console.WriteLine("{0}: {1}", pair.Key, pair.Value);

                returndict.Add(pair.Key, pair.Value);
            }

            return returndict;
        }


        /// <summary>
        /// Splits One Dictionary into 2 Equal Parts (as equal as possible)
        /// </summary>
        /// <param name="urls">Dictionary to Split into 2 Equal Parts (remainder go on last list)</param>
        /// <param name="dict1"></param>
        /// <param name="dict2"></param>
        public static void SplitDict(this Dictionary<string, string> urls, out Dictionary<string, string> dict1, out Dictionary<string, string> dict2)
        {
            // divide with no remainder, then add remainder value to last list 
            int remainder = urls.Count % 2;
            int totalWithOutRemainder = urls.Count - remainder;
            int half = totalWithOutRemainder / 2;

            int part1 = half + remainder;
            int part2 = half;

            Dictionary<string, string> urls1 = new Dictionary<string, string>();
            Dictionary<string, string> urls2 = new Dictionary<string, string>();

            int counter = 0;
            foreach (string url in urls.Keys)
            {
                if (counter < part1) { urls1.Add(url, urls[url]); counter++; continue; }
                else
                {
                    urls2.Add(url, urls[url]); counter++;
                }
            }

            dict1 = urls1;
            dict2 = urls2;
        }


        #endregion


        #region === Populate Dictionary ===

        /// <summary>
        /// Returns Dictionary with Full FilePath / FileSize (Bytes)
        /// </summary>
        /// <param name="DirPath"></param>
        /// <param name="SearchPattern"></param>
        /// <param name="Recurse"></param>
        /// <returns></returns>
        public static Dictionary<string, string> FileSizeDict(this string DirPath, string SearchPattern = "*.*", bool Recurse = true)
        {
            _AHK ahk = new _AHK();
            Dictionary<string, string> fileDict = new Dictionary<string, string>();

            if (!Directory.Exists(DirPath)) { return null; }

            string[] files = null;
            if (Recurse) { files = Directory.GetFiles(DirPath, SearchPattern, System.IO.SearchOption.AllDirectories); }
            if (!Recurse) { files = Directory.GetFiles(DirPath, SearchPattern, System.IO.SearchOption.TopDirectoryOnly); }

            foreach (string file in files)  // loop through list of files and write file details to sqlite db
            {
                string size = ahk.FileSize(file);
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
        public static Dictionary<string, string> FileSizeDict(this List<string> DirPaths, string SearchPattern = "*.*", bool Recurse = true)
        {
            _AHK ahk = new _AHK();
            Dictionary<string, string> fileDict = new Dictionary<string, string>();

            foreach (string DirPath in DirPaths)
            {
                if (!Directory.Exists(DirPath)) { continue; }

                string[] files = null;
                if (Recurse) { files = Directory.GetFiles(DirPath, SearchPattern, System.IO.SearchOption.AllDirectories); }
                if (!Recurse) { files = Directory.GetFiles(DirPath, SearchPattern, System.IO.SearchOption.TopDirectoryOnly); }

                foreach (string file in files)  // loop through list of files and write file details to sqlite db
                {
                    string size = ahk.FileSize(file);
                    fileDict.Add(file, size);
                }
            }

            return fileDict;
        }


        /// <summary>Returns Dictionary List of ALL Processes Running on PC - If ProcessName provided then return all processes with that name</summary>
        /// <param name="ProcessName">Optional parameter to return all processes with this process name</param>
        /// <param name="SortAlpha">Option to Sort Return List Alphabetically</param>
        public static Dictionary<string, Process> ProcessDict(bool SortAlpha = true, string ProcessName = "")
        {
            Dictionary<string, Process> ProcessList = new Dictionary<string, Process>();

            Process[] processlist = Process.GetProcesses();

            foreach (Process theprocess in processlist)
            {
                if (!String.IsNullOrEmpty(theprocess.MainWindowTitle))
                {
                    try
                    {
                        if (ProcessName == "") { ProcessList.Add(theprocess.MainWindowTitle + "|" + theprocess.Id, theprocess); }  // return all processes if Process Name not provided
                        if (ProcessName != "") { if (theprocess.ProcessName == ProcessName) { ProcessList.Add(theprocess.MainWindowTitle + "|" + theprocess.Id, theprocess); } } // return all processes if Process Name not provided
                    }
                    catch
                    {
                    }
                }
            }

            if (SortAlpha)
            {
                bool Reverse = false;
                Dictionary<string, Process> returndict = new Dictionary<string, Process>();

                // Order by values.
                // ... Use LINQ to specify sorting by value.
                var items = from pair in ProcessList
                            orderby pair.Key ascending
                            select pair;

                if (Reverse)
                {
                    // Reverse sort.
                    // ... Can be looped over in the same way as above.
                    items = from pair in ProcessList
                            orderby pair.Key descending
                            select pair;
                }

                // Display results.
                foreach (KeyValuePair<string, Process> pair in items)
                {
                    //Console.WriteLine("{0}: {1}", pair.Key, pair.Value);

                    returndict.Add(pair.Key, pair.Value);
                }

                ProcessList = returndict;
            }

            return ProcessList;
        }



        #endregion




    }
}
