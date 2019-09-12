using AHKExpressions;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace sharpAHK
{
    public partial class _AHK
    {
        // === Strings ===

        /// <summary>Transforms a YYYYMMDDHH24MISS timestamp into the specified date/time format.</summary>
        /// <param name="YYYYMMDDHH24MISS">Leave this parameter blank to use the current local date and time. Otherwise, specify all or the leading part of a timestamp in the YYYYMMDDHH24MISS format. If the date and/or time portion of the timestamp is invalid -- such as February 29th of a non-leap year -- the date and/or time will be omitted from OutputVar. Although only years between 1601 and 9999 are supported, a formatted time can still be produced for earlier years as long as the time portion is valid.</param>
        /// <param name="Format">If omitted, it defaults to the time followed by the long date, both of which will be formatted according to the current user's locale. For example: 4:55 PM Saturday, November 27, 2004 Otherwise, specify one or more of the date-time formats below, along with any literal spaces and punctuation in between (commas do not need to be escaped; they can be used normally). In the following example, note that M must be capitalized: M/d/yyyy h:mm tt</param>
        public string FormatTime(string YYYYMMDDHH24MISS = "", string Format = "")
        {
            string AHKLine = "FormatTime, OutputVar, " + YYYYMMDDHH24MISS + ", " + Format;  // ahk line to execute
            ErrorLog_Setup(false); // ErrorLevel Detection Enabled for this function in AHK 
            string OutVar = Execute(AHKLine, "OutputVar");   // execute AHK code and return variable value 
            return OutVar;
        }

        /// <summary>Determines whether string comparisons are case sensitive (default is "not case sensitive").</summary>
        /// <param name="OnOffLocale">On: String comparisons are case sensitive. This setting also makes the expression equal sign operator (=) and the case-insensitive mode of InStr() use the locale method described below. Off (starting default): The letters A-Z are considered identical to their lowercase counterparts. This is the starting default for all scripts due to backward compatibility and performance (Locale is 1 to 8 times slower than Off depending on the nature of the strings being compared). Locale [v1.0.43.03+]: String comparisons are case insensitive according to the rules of the current user's locale. For example, most English and Western European locales treat not only the letters A-Z as identical to their lowercase counterparts, but also ANSI letters like Ä and Ü as identical to theirs. </param>
        public void StringCaseSense(string OnOffLocale)
        {
            ErrorLog_Setup(false);
            Execute("StringCaseSense, " + OnOffLocale);
        }

        /// <summary>Retrieves the position of the specified substring within a string.</summary>
        /// <param name="InputVar">The name of the input variable, whose contents will be searched. Do not enclose the name in percent signs unless you want the contents of the variable to be used as the name.</param>
        /// <param name="SearchText">The string to search for. Matching is not case sensitive unless StringCaseSense has been turned on. </param>
        /// <param name="LR">This affects which occurrence will be found if SearchText occurs more than once within InputVar. If this parameter is omitted, InputVar will be searched starting from the left for the first match. If this parameter is 1 or the letter R, the search will start looking at the right side of InputVar and will continue leftward until the first match is found. To find a match other than the first, specify the letter L or R followed by the number of the occurrence. For example, to find the fourth occurrence from the right, specify r4. Note: If the number is less than or equal to zero, no match will be found.</param>
        /// <param name="Offset">The number of characters on the leftmost or rightmost side (depending on the parameter above) to skip over. If omitted, the default is 0. For example, the following would start searching at the tenth character from the left: StringGetPos, OutputVar, InputVar, abc, , 9. This parameter can be an expression.</param>
        public string StringGetPos(string InputVar, string SearchText, string LR = "", string Offset = "")
        {
            string AHKLine = "Needle = " + SearchText + Environment.NewLine + "HayStack = " + InputVar + Environment.NewLine + "StringGetPos, OutputVar, HayStack, %Needle%, " + LR + "," + Offset;  // ahk line to execute
            ErrorLog_Setup(true, "[StringGetPos] SearchText could not be found within InputVar:"); // ErrorLevel Detection Enabled for this function in AHK 
            string OutVar = Execute(AHKLine, "OutputVar");   // execute AHK code and return variable value 
            return OutVar;
        }

        /// <summary>Retrieves a number of characters from the left-hand side of a string.</summary>
        /// <param name="InputVar">The name of the variable whose contents will be extracted from. Do not enclose the name in percent signs unless you want the contents of the variable to be used as the name.</param>
        /// <param name="Count">The number of characters to extract, which can be an expression. If Count is less than or equal to zero, OutputVar will be made empty (blank). If Count exceeds the length of InputVar, OutputVar will be set equal to the entirety of InputVar.</param>
        public string StringLeft(string InputVar, object Count)
        {
            string Out = "";

            try
            {
                Out = InputVar.Substring(0, ToInt(Count));
            }
            catch { }

            return Out;

            //string AHKLine = "InputVar = " + InputVar + Environment.NewLine + "StringLeft, OutputVar, InputVar," + Count;  // ahk line to execute
            //ErrorLog_Setup(false); // ErrorLevel Detection Enabled for this function in AHK 
            //string OutVar = Execute(AHKLine, "OutputVar");   // execute AHK code and return variable value 
            //return OutVar;
        }

        /// <summary>Retrieves a number of characters from the right-hand side of a string.</summary>
        /// <param name="InputVar">The name of the variable whose contents will be extracted from. Do not enclose the name in percent signs unless you want the contents of the variable to be used as the name.</param>
        /// <param name="Count">The number of characters to extract, which can be an expression. If Count is less than or equal to zero, OutputVar will be made empty (blank). If Count exceeds the length of InputVar, OutputVar will be set equal to the entirety of InputVar.</param>
        public string StringRight(string InputVar, object Count)
        {
            return InputVar.Substring(0, InputVar.Length - ToInt(Count));

            //string AHKLine = "InputVar = " + InputVar + Environment.NewLine + "StringRight, OutputVar, InputVar," + Count;  // ahk line to execute
            //ErrorLog_Setup(false); // ErrorLevel Detection Enabled for this function in AHK 
            //string OutVar = Execute(AHKLine, "OutputVar");   // execute AHK code and return variable value 
            //return OutVar;
        }

        /// <summary>Retrieves the count of how many characters are in a string.</summary>
        /// <param name="InputVar">The name of the variable whose contents will be measured. Do not enclose the name in percent signs unless you want the contents of the variable to be used as the name.</param>
        public string StringLen(string InputVar)
        {
            string AHKLine = "InputVar = " + InputVar + Environment.NewLine + "StringLen, OutputVar, InputVar";  // ahk line to execute
            ErrorLog_Setup(false); // ErrorLevel Detection Enabled for this function in AHK 
            string OutVar = Execute(AHKLine, "OutputVar");   // execute AHK code and return variable value 
            return OutVar;
        }

        /// <summary>Converts a string to lowercase.</summary>
        /// <param name="InputVar">The name of the variable whose contents will be read from. Do not enclose the name in percent signs unless you want the contents of the variable to be used as the name.</param>
        /// <param name="ProperCase">If this parameter is True, the string will be converted to title case. For example, "GONE with the WIND" would become "Gone With The Wind". </param>
        public string StringLower(string InputVar, bool ProperCase = false)
        {
            string T = ""; if (ProperCase) { T = "T"; }  // option to return string in Proper Case

            string AHKLine = "InputVar = " + InputVar + Environment.NewLine + "StringLower, OutputVar, InputVar," + T;  // ahk line to execute
            ErrorLog_Setup(false); // ErrorLevel Detection Enabled for this function in AHK 
            string OutVar = Execute(AHKLine, "OutputVar");   // execute AHK code and return variable value 
            return OutVar;
        }

        /// <summary>Converts a string to uppercase.</summary>
        /// <param name="InputVar">The name of the variable whose contents will be read from. Do not enclose the name in percent signs unless you want the contents of the variable to be used as the name.</param>
        /// <param name="ProperCase">If this parameter is True, the string will be converted to title case. For example, "GONE with the WIND" would become "Gone With The Wind". </param>
        public string StringUpper(string InputVar, bool ProperCase = false)
        {
            string T = ""; if (ProperCase) { T = "T"; }  // option to return string in Proper Case

            string AHKLine = "InputVar = " + InputVar + Environment.NewLine + "StringUpper, OutputVar, InputVar," + T;  // ahk line to execute
            ErrorLog_Setup(false); // ErrorLevel Detection Enabled for this function in AHK 
            string OutVar = Execute(AHKLine, "OutputVar");   // execute AHK code and return variable value 
            return OutVar;
        }

        /// <summary>Retrieves one or more characters from the specified position in a string.</summary>
        /// <param name="InputVar">The name of the variable from whose contents the substring will be extracted. Do not enclose the name in percent signs unless you want the contents of the variable to be used as the name.</param>
        /// <param name="StartChar">The position of the first character to be extracted, which can be an expression. Unlike StringGetPos, 1 is the first character. If StartChar is less than 1, it will be assumed to be 1. If StartChar is beyond the end of the string, OutputVar is made empty (blank).</param>
        /// <param name="Count"> this parameter may be omitted or left blank, which is the same as specifying an integer large enough to retrieve all characters from the string. Otherwise, specify the number of characters to extract, which can be an expression. If Count is less than or equal to zero, OutputVar will be made empty (blank). If Count exceeds the length of InputVar measured from StartChar, OutputVar will be set equal to the entirety of InputVar starting at StartChar.</param>
        /// <param name="L">The letter L can be used to extract characters that lie to the left of StartChar rather than to the right. In the following example, OutputVar will be set to Red: InputVar = The Red Fox StringMid, OutputVar, InputVar, 7, 3, L If the L option is present and StartChar is less than 1, OutputVar will be made blank. If StartChar is beyond the length of InputVar, only those characters within reach of Count will be extracted. For example, the below will set OutputVar to Fox: InputVar = The Red Fox StringMid, OutputVar, InputVar, 14, 6, L</param>
        public string StringMid(string InputVar, string StartChar, string Count = "", string L = "")
        {
            string AHKLine = "InputVar = " + InputVar + Environment.NewLine + "StringMid, OutputVar, InputVar," + StartChar + "," + Count + "," + L;  // ahk line to execute
            ErrorLog_Setup(false); // ErrorLevel Detection Enabled for this function in AHK 
            string OutVar = Execute(AHKLine, "OutputVar");   // execute AHK code and return variable value 
            return OutVar;
        }

        /// <summary>Replaces the specified substring with a new string.</summary>
        /// <param name="InputVar">The name of the variable whose contents will be read from. Do not enclose the name in percent signs unless you want the contents of the variable to be used as the name.</param>
        /// <param name="SearchText">The string to search for. Matching is not case sensitive unless StringCaseSense has been turned on.</param>
        /// <param name="ReplaceText">SearchText will be replaced with this text. If omitted or blank, SearchText will be replaced with blank (empty). In other words, it will be omitted from OutputVar.</param>
        /// <param name="ReplaceAll">If omitted, only the first occurrence of SearchText will be replaced. But if this parameter is 1, A, or All, all occurrences will be replaced. Specify the word UseErrorLevel to store in ErrorLevel the number of occurrences replaced (0 if none). UseErrorLevel implies "All".</param>
        public string StringReplace(string InputVar, string SearchText, string ReplaceText = "", string ReplaceAll = "", int Mode = 1)
        {
            if (Mode == 1)
            {
                if (SearchText.Trim() == "") { return ""; }
                string val = InputVar.Replace(SearchText, ReplaceText);
                return val;
            }

            string AHKLine = "#EscapeChar |" + Environment.NewLine + "SearchText = " + Environment.NewLine + "(" + Environment.NewLine + SearchText + Environment.NewLine + ")" + Environment.NewLine + "InputVar = " + Environment.NewLine + "(" + Environment.NewLine + InputVar + Environment.NewLine + ")" + Environment.NewLine + "StringReplace, OutputVar, InputVar, %SearchText%," + ReplaceText + "," + ReplaceAll;  // ahk line to execute
            ErrorLog_Setup(false); // ErrorLevel Detection Enabled for this function in AHK 
            string OutVar = Execute(AHKLine, "OutputVar");   // execute AHK code and return variable value 
            return OutVar;


            // v1 ToDo
            /*
                    public string _StringReplace(string InText, string SearchText, string ReplaceText = "")  // replaces text in string, can replace with new text
                    {
                        if (InText == null) { return ""; }  // no source text passed in
                        if (InText == "") { return InText; }  // no source text passed in
                        if (SearchText == "") { return InText; }  // no value passed in to search for

                        string output = InText.Replace(SearchText, ReplaceText);
                        return output;
                    }
            */

        }

        /// <summary>Removes a number of characters from the left-hand side of a string.</summary>
        /// <param name="InputVar">The name of the variable whose contents will be read from. Do not enclose the name in percent signs unless you want the contents of the variable to be used as the name.</param>
        /// <param name="Count">The number of characters to remove, which can be an expression. If Count is less than or equal to zero, OutputVar will be set equal to the entirety of InputVar. If Count exceeds the length of InputVar, OutputVar will be made empty (blank).</param>
        public string StringTrimLeft(string InputVar, string Count)
        {
            string AHKLine = "InputVar = " + InputVar + Environment.NewLine + "StringTrimLeft, OutputVar, InputVar, " + Count;  // ahk line to execute
            ErrorLog_Setup(false); // ErrorLevel Detection Enabled for this function in AHK 
            string OutVar = Execute(AHKLine, "OutputVar");   // execute AHK code and return variable value 
            return OutVar;
        }

        /// <summary>Removes a number of characters from the right-hand side of a string.</summary>
        /// <param name="InputVar">The name of the variable whose contents will be read from. Do not enclose the name in percent signs unless you want the contents of the variable to be used as the name.</param>
        /// <param name="Count">The number of characters to remove, which can be an expression. If Count is less than or equal to zero, OutputVar will be set equal to the entirety of InputVar. If Count exceeds the length of InputVar, OutputVar will be made empty (blank).</param>
        public string StringTrimRight(string InputVar, string Count)
        {
            string AHKLine = "InputVar = " + InputVar + Environment.NewLine + "StringTrimRight, OutputVar, InputVar, " + Count;  // ahk line to execute
            ErrorLog_Setup(false); // ErrorLevel Detection Enabled for this function in AHK 
            string OutVar = Execute(AHKLine, "OutputVar");   // execute AHK code and return variable value 
            return OutVar;
        }

        /// <summary>Checks if a string contains the specified string.</summary>
        /// <param name="Text">Text to search for SearchString</param>
        /// <param name="SearchString">The string to search for. Matching is not case sensitive unless StringCaseSense has been turned on.</param>
        public bool IfInString(string Text, string SearchString)
        {
            if (Text.Contains(SearchString)) { return true; }
            return false;

            //string ahkString = "Text = " + Text;
            //ahkString = ahkString + Environment.NewLine + "SearchString = " + SearchString;
            //ahkString = ahkString + Environment.NewLine + @"
            //    OutputVar = false
            //    IfInString, Text, %SearchString%
            //        OutputVar = true
            //    ";

            //ErrorLog_Setup(false); // ErrorLevel Detection Enabled for this function in AHK 
            //string OutputVar = Execute(ahkString, "OutputVar");   // execute AHK code and return variable value

            //// return bool value indicating whether text was detected in string
            //if (OutputVar == "true") { return true; }
            //return false;

            // v1 ToDo
            /*
                    public bool _IfInString(string Needle, string Haystack)  // Returns True if Needle Text is Found in Haystack Text
                    {
                        return Haystack.Contains(Needle);
                    }
            */


        }

        /// <summary>Checks if a string contains the specified string.</summary>
        /// <param name="Text">Text to search for SearchString</param>
        /// <param name="SearchString">The string to search for. Matching is not case sensitive unless StringCaseSense has been turned on.</param>
        public bool IfNotInString(string Text, string SearchString)
        {
            if (!Text.Contains(SearchString)) { return true; }
            return false;


            //string ahkString = "Text = " + Text;
            //ahkString = ahkString + Environment.NewLine + "SearchString = " + SearchString;
            //ahkString = ahkString + Environment.NewLine + @"
            //    OutputVar = false
            //    IfNotInString, Text, %SearchString%
            //        OutputVar = true
            //    ";

            //ErrorLog_Setup(false); // ErrorLevel Detection Enabled for this function in AHK 
            //string OutputVar = Execute(ahkString, "OutputVar");   // execute AHK code and return variable value

            //// return bool value indicating whether text was detected in string
            //if (OutputVar == "true") { return true; }
            //return false;

            // v1 ToDo
            /*
                    public bool _IfNotInString(string Needle, string Haystack)  // Returns True if Needle NOT Found In Haystack Text
                    {
                        bool contains = Haystack.Contains(Needle);
                        if (contains) { return false; }
                        return true;
                    }
            */
        }

        /// <summary>Arranges a variable's contents in alphabetical, numerical, or random order (optionally removing duplicates).</summary>
        /// <param name="Text">String whose contents will be sorted.</param>
        /// <param name="Options">C = Case Sensitive Sort | CL = Case Insensitive sort based on User's Locale | Dx = Specifies x as the delimiter character, which determins where each item in the Text begins and ends (default = newline) | F MyFunction = uses custom sorting according to the criteria in MyFunction | N = Numeric Sort | Pn = Sorts items based on character position n | R = Sorts in reverse order | Random = Sorts in random order | U = Removes Duplicates | Z = Last linefeed is considered to be part of the last item</param>
        public string Sort(string Text, string Options = "")
        {
            string AHKLine = "Text = " + Environment.NewLine + "(" + Environment.NewLine + Text + Environment.NewLine + ")" + Environment.NewLine + "Sort, Text, " + Options;  // ahk line to execute
            ErrorLog_Setup(false); // ErrorLevel Detection Enabled for this function in AHK 
            string OutVar = Execute(AHKLine, "Text");   // execute AHK code and return variable value 
            return OutVar;
        }


        // ### New Additions ###

        /// <summary>Removes Numbers from String</summary>
        /// <param name="InString"> </param>
        public string Remove_Numbers(string InString)
        {
            var output = Regex.Replace(InString, @"[\d-]", string.Empty);
            return output.ToString();
        }

        /// <summary>
        /// Remove Letters from String
        /// </summary>
        /// <param name="InString"></param>
        /// <returns></returns>
        public string Remove_Letters(string InString)
        {
            string value = Regex.Replace(InString, "[A-Za-z ]", "");
            return value;
        }

        /// <summary>Remove HTML characters from string</summary>
        /// <param name="HTML">HTML to strip</param>
        public string UnHtml(string HTML)
        {
            var step1 = Regex.Replace(HTML, @"<[^>]+>|&nbsp;", "").Trim();
            var step2 = Regex.Replace(step1, @"\s{2,}", " ");
            return step2;
        }

        /// <summary>Format string to be compatible with SQL/SQLite Inserts</summary>
        /// <param name="str"> </param>
        public string FixSpecialChars(string str)
        {
            //str = str.Trim();  // trim leading/end spaces 
            //str = str.Replace("\"", @"\""");
            str = str.Replace("'", "''");
            return str;
        }

        /// <summary>Returns the number of times a character is found in a string</summary>
        /// <param name="Line"> </param>
        /// <param name=" Char"> </param>
        public int CharCount(string Line, string Char)
        {
            char Char1 = Char[0];
            int CharCount = Line.Count(f => f == Char1);  // count number of brackets in line
            return CharCount;
        }

        /// <summary>Remove X Characters from beginning of string</summary>
        /// <param name="str"> </param>
        /// <param name="RemoveCharacterCount"> </param>
        public string TrimFirst(string str, int RemoveCharacterCount = 1)
        {
            if (str == "") { return ""; } // do nothing if blank string

            string returnstring = str.Remove(0, RemoveCharacterCount);
            return returnstring;
        }

        /// <summary>Remove X Characters from end of string</summary>
        /// <param name="str"> </param>
        /// <param name="RemoveCharacterCount"> </param>
        public string TrimLast(string str, int RemoveCharacterCount = 1)
        {
            if (str == "") { return ""; } // do nothing if blank string

            return str.TrimEnd(str[str.Length - RemoveCharacterCount]);
        }

        /// <summary>
        /// Trims 0's from beginning of string 
        /// </summary>
        /// <param name="Text">String to trim leading zeros from</param>
        /// <returns>Returns string minus leading zeroes</returns>
        public string TrimLeadingZeros(string Text)
        {
            string ReturnVal = "";

            int i = 0;

            bool FirstNonZeroFound = false;

            Text = Text.Trim();

            if (Text == "") { return ""; }

            //const string s = Text; 
            foreach (char c in Text)
            {
                string Chara = c.ToString();
                bool isLetterOrDigit = char.IsLetterOrDigit(c);

                if (!isLetterOrDigit)  // dash mark - just add to string instead of checking for value
                {
                    ReturnVal = ReturnVal + Chara;
                    continue;
                }

                int check = Int32.Parse(Chara);  // string to int

                if (check != 0) { FirstNonZeroFound = true; ReturnVal = ReturnVal + Chara; }

                if (check == 0)
                {
                    if (!FirstNonZeroFound) { continue; }
                    if (FirstNonZeroFound) { ReturnVal = ReturnVal + Chara; }
                }
            }

            ReturnVal = ReturnVal.Trim();
            return ReturnVal;
        }

        /// <summary>
        /// Trim ending characters from string if they exist, returns string without ending chars
        /// </summary>
        /// <param name="Text"></param>
        /// <param name="ToTrim"></param>
        /// <returns></returns>
        public string TrimEndIf(string Text, string ToTrim)
        {
            string returnText = Text;
            int len = ToTrim.Length; // # of characters to trim
            string endChars = LastCharacters(Text, len);  // get those last characters
            if (endChars == ToTrim) { returnText = TrimLast(Text, len); } // trim if found
            return returnText;
        }

        /// <summary>
        /// Trims all of a specific leading character from the from beginning of string { UNTESTED }
        /// </summary>
        /// <param name="Text"></param>
        /// <param name="Char"></param>
        /// <returns></returns>
        public string TrimLeadingChars(string Text, string Char = "0")
        {
            string ReturnVal = "";

            int i = 0;

            bool FirstNonZeroFound = false;

            Text = Text.Trim();

            if (Text == "") { return ""; }

            //const string s = Text; 
            foreach (char c in Text)
            {
                string Chara = c.ToString();
                bool isLetterOrDigit = char.IsLetterOrDigit(c);

                if (!isLetterOrDigit)  // dash mark - just add to string instead of checking for value
                {
                    ReturnVal = ReturnVal + Chara;
                    continue;
                }

                //int check = Int32.Parse(Chara);  // string to int

                if (Chara != Char) { FirstNonZeroFound = true; ReturnVal = ReturnVal + Chara; }

                if (Chara == Char)
                {
                    if (!FirstNonZeroFound) { continue; }
                    if (FirstNonZeroFound) { ReturnVal = ReturnVal + Chara; }
                }
            }

            ReturnVal = ReturnVal.Trim();
            return ReturnVal;
        }

        /// <summary>Returns first X characters in string</summary>
        /// <param name="Text"> </param>
        /// <param name="NumberOfCharacters"> </param>
        public string FirstCharacters(string Text, int NumberOfCharacters = 1)
        {
            Text = Text.Trim();
            if (Text == "") { return ""; }

            if (Text.Length < NumberOfCharacters) { return ""; }

            string str = Text.Substring(0, NumberOfCharacters);
            return str;
        }

        /// <summary>Returns last X characters in string</summary>
        /// <param name="Text"> </param>
        /// <param name="NumberOfCharacters"> </param>
        public string LastCharacters(string Text, int NumberOfCharacters = 1)
        {
            Text = Text.Trim();
            if (Text == "") { return ""; }
            string str = Text.Substring(Text.Length - NumberOfCharacters, NumberOfCharacters);
            return str;
        }

        /// <summary>Returns First word in string</summary>
        /// <param name="InputString"> </param>
        public string FirstWord(string InputString)  // returns First word in string 
        {
            string[] words = InputString.Split(' ');
            foreach (string word in words)
            {
                if (word.Trim() != "") { return word.Trim(); }
            }

            return "";
        }

        /// <summary>Returns last word in string</summary>
        /// <param name="InputString"> </param>
        public string LastWord(string InputString)
        {
            List<string> WordsList = WordList(InputString);

            string ReturnWord = "";
            foreach (string word in WordsList)
            {
                ReturnWord = word;
            }

            return ReturnWord;
        }

        /// <summary>Return specific word # from string</summary>
        /// <param name="InputString"> </param>
        /// <param name="WordNumber"> </param>
        public string WordNum(string InputString, int WordNumber = 1)
        {
            InputString = InputString.Trim();

            List<string> List = WordList(InputString);
            int counter = 0;
            foreach (string word in List)
            {
                counter++;
                if (counter == WordNumber)
                {
                    return word;
                }
            }

            return "";
        }

        /// <summary>Parse line by space, returns list of words</summary>
        /// <param name="InputString"> </param>
        public List<string> WordList(string InputString)
        {
            //List<string> WordList = WordList(InText); 
            //foreach(string Word in WordList)


            List<string> ReturnList = new List<string>();
            string[] words = InputString.Split(' ');
            foreach (string word in words)
            {
                string worda = word.Trim();
                ReturnList.Add(worda);
            }
            return ReturnList;
        }

        /// <summary>
        /// Returns # of Words in String
        /// </summary>
        /// <param name="Text">String to check word count</param>
        /// <returns></returns>
        public int WordCount(string Text)
        {
            return Text.Split(new char[] { ' ', '.', '?' }, StringSplitOptions.RemoveEmptyEntries).Length;
        }

        /// <summary>Parse line by new line, returns list of lines</summary>
        /// <param name="InputString"> </param>
        /// <param name="Trim"> </param>
        /// <param name="RemoveBlanks"> </param>
        public List<string> LineList(string InputString, bool Trim = true, bool RemoveBlanks = false)
        {
            List<string> ReturnList = new List<string>();
            string[] lines = InputString.Split(Environment.NewLine.ToCharArray());
            foreach (string line in lines)
            {
                string wline = line;

                if (Trim) { wline = line.Trim(); }  // option to trim text line when creating list

                if (RemoveBlanks) { if (line.Trim() == "") { continue; } }

                ReturnList.Add(wline);
            }
            return ReturnList;
        }

        /// <summary>Returns code line without comments</summary>
        /// <param name="line"> </param>
        /// <param name="CommentCharacters"> </param>
        public string RemoveComments(string line, string CommentCharacters = "//")
        {
            //=== parse line of C# code, returns Code as output and returns comment in the out field ===

            //string line = "public class SQLite  // JDL SQLite Collection";

            string lineCode = line.Trim();  //trim extra spaces from line
            string First2Chars = "";
            string Code = "";

            //================================
            // Parse String by Space
            //================================
            string[] words = lineCode.Split(' ');
            foreach (string word in words)
            {
                if (word == "") { continue; } //skip blank lines
                //ahk.MsgBox(word); 

                if (word.Length >= 2) { First2Chars = word.Substring(0, 2); } //returns the first two characters of the word (to see if a comment is starting)

                if (First2Chars == CommentCharacters)
                {
                    return Code;
                }

                if (Code == "") { Code = word; continue; }
                if (Code != "") { Code = Code + " " + word; continue; }
            }
            return Code;
        }

        /// <summary>Returns comments on line after code</summary>
        /// <param name="line"> </param>
        /// <param name="CommentCharacters"> </param>
        public string ReturnComments(string line, string CommentCharacters = "//")
        {
            //=== parse line of C# code, returns Code as output and returns comment in the out field ===

            //string line = "public class SQLite  // JDL SQLite Collection";

            string lineCode = line.Trim();  //trim extra spaces from line
            string First2Chars = "";
            string Code = "";

            //================================
            // Parse String by Space
            //================================
            string[] words = lineCode.Split(' ');
            bool Capture = false;
            foreach (string word in words)
            {
                if (word == "") { continue; } //skip blank lines
                //ahk.MsgBox(word); 

                if (word.Length >= 2) { First2Chars = word.Substring(0, 2); } //returns the first two characters of the word (to see if a comment is starting)

                if (First2Chars == CommentCharacters)
                {
                    Capture = true;
                }

                if (Capture)
                {
                    if (Code == "") { Code = word; continue; }
                    if (Code != "") { Code = Code + " " + word; continue; }
                }

            }
            return Code;
        }

        /// <summary>Extracts text between brackets</summary>
        /// <param name="Code"> </param>
        /// <param name="start"> </param>
        /// <param name="end"> </param>
        public string Extract_Between(string Code, string start = "{", string end = "}")
        {
            //ahk.MsgBox(Code); 

            string OutCode = "";
            bool StartCapture = false;
            int BracketCount = 0;

            string[] lines = Code.Split(new string[] { Environment.NewLine }, StringSplitOptions.None);
            foreach (string line in lines)
            {
                string LineCode = line.Trim();

                if (LineCode == "") { continue; }

                if (!StartCapture)
                {
                    if (line.Contains(start)) { StartCapture = true; BracketCount++; continue; }
                }

                if (StartCapture)
                {
                    if (line.Contains(start)) { StartCapture = true; BracketCount++; }
                }


                if (BracketCount == 1)
                {
                    if (line.Contains(end)) { BracketCount--; continue; }
                }

                if (BracketCount > 1)
                {
                    if (line.Contains(end)) { BracketCount--; }
                }


                if (StartCapture)
                {
                    OutCode = OutCode + Environment.NewLine + line;
                }

                if (StartCapture) { if (BracketCount == 0) { return OutCode; } }

            }

            //ahk.MsgBox(OutCode);
            return OutCode;
        }

        /// <summary>Extract text between <Tag> XML style tags </Tag></summary>
        /// <param name="XMLString">String to extract tag text from</param>
        /// <param name="Tag">Name of tag to return text between. Ex: <UserTag>About this Project</UserTag> returns "About this Project"</param>
        public string XML_TagText(string XMLString, string Tag)
        {
            var startTag = "<" + Tag + ">";
            int startIndex = XMLString.IndexOf(startTag) + startTag.Length;
            int endIndex = XMLString.IndexOf("</" + Tag + ">", startIndex);
            return XMLString.Substring(startIndex, endIndex - startIndex);
        }


        /// <summary>Insert text into specific position in string</summary>
        /// <param name="InText"> </param>
        /// <param name="InsertText"> </param>
        /// <param name="Position"> </param>
        public string Insert_Text(string InText, string InsertText, int Position)
        {
            string inserted = InText.Substring(0, Position) + InsertText + InText.Substring(Position);
            return inserted;
        }

        /// <summary>Split string by character (return pos starts at word 0, final option overrides position to return last item)</summary>
        /// <param name="InText">Text to split</param>
        /// <param name="SplitChar">Character(s) to split string by</param>
        /// <param name="ReturnPos">Position # of the word(s) to return. Ex: ReturnPos 0 returns the text before the SplitChar is found, 1 returns the text after the first splitchar and before the 2nd splitchar</param>
        /// <param name="ReturnLast">Override for ReturnPos value - will return last value in split string</param>
        /// <param name="NoBlanks">Option to return next available value if ReturnPos value is blank</param>
        public string StringSplit(string InText, string SplitChar = "(", int ReturnPos = 0, bool ReturnLast = false, bool NoBlanks = true)
        {
            if (InText == null) { return ""; }
            if (SplitChar.Length == 0) { return InText; } // nothing to split if no splitchar is passed in - return original text

            // confirm only 1 character was passed in to this method. display error message if longer than 1 char
            if (SplitChar.Length > 1) { MsgBox("SplitChar Can Only Be 1 Character\nSplitChar " + SplitChar.Length.ToString() + " = Characters : " + SplitChar); return ""; }

            char[] delimiterChars = { Char.Parse(SplitChar) };

            string[] words = InText.Split(delimiterChars);

            string ReturnText = "";
            string lastSplit = "";

            if (!ReturnLast)  // if returning last, skip condition to return first value
            {
                if (ReturnPos == 0)
                {
                    ReturnText = words.First();

                    if (NoBlanks) // option to ensure results are found even if moving to next available position
                    {
                        if (ReturnText == "") { ReturnPos = 1; }
                    }

                }  // return text before the split string char - if blank, return next available value
            }

            if (ReturnPos != 0) // return text after first position, depending on return position # provided by user
            {
                int partnum = 0;
                foreach (string part in words)
                {
                    if (NoBlanks) // option to ensure results are found even if moving to next available position
                    {
                        if (partnum == ReturnPos && part != "") { ReturnText = part; }
                    }
                    else
                    {
                        if (partnum == ReturnPos) { ReturnText = part; }
                    }

                    if (part != "") { lastSplit = part; }   // stores value of last value in split
                    partnum++;
                }
            }

            if (ReturnLast)
            {
                foreach (string part in words)
                {
                    if (part != "") { lastSplit = part; }   // stores value of last value in split
                }
                return lastSplit;
            }

            return ReturnText;
        }

        /// <summary>Split string by character, Returns List of values separated by the SplitChar</summary>
        /// <param name="InText">Text to split</param>
        /// <param name="SplitChar">Character(s) to split string by</param>
        /// <param name="SkipBlanks">Default Option To Not Return Blank Values in Return List</param>
        public List<string> StringSplit_List(string InText, string SplitChar = "(", bool SkipBlanks = true)
        {
            if (InText == null) { return new List<string>(); }

            List<string> returnList = new List<string>();

            if (InText.Trim() == "") { return returnList; }

            if (SplitChar.Length == 0) { return returnList; } // nothing to split if no splitchar is passed in - return original text

            // confirm only 1 character was passed in to this method. display error message if longer than 1 char
            if (SplitChar.Length > 1) { MsgBox("SplitChar Can Only Be 1 Character\nSplitChar " + SplitChar.Length.ToString() + " = Characters : " + SplitChar); return returnList; }

            char[] delimiterChars = { Char.Parse(SplitChar) };

            string[] words = InText.Split(delimiterChars);

            foreach (string part in words)
            {
                string addPart = part.Trim();
                if (SkipBlanks) { if (addPart != "") { returnList.Add(part); } }  // don't add blank values
                else { returnList.Add(part); }
            }

            return returnList;
        }

        /// <summary>Add leading zeros to an int/string, ex: InNumber 12 with TotalReturnLength 5 returns string "00012"</summary>
        /// <param name="InNumber">Original number (int or string) to add leading zeros to.</param>
        /// <param name="TotalReturnLength">Total # of desired digits, adding zeros in front of the InNumber to ensure return value is TotalReturnLength characters long.</param>
        /// <tested>True</tested>
        public string AddLeadingZeros(object InNumber, int TotalReturnLength = 5)
        {
            int inNumber = ToInt(InNumber); // convert input (int32/string/etc to an int)

            string fmt = "";

            if (TotalReturnLength == 1) { fmt = "0"; }
            if (TotalReturnLength == 2) { fmt = "00"; }
            if (TotalReturnLength == 3) { fmt = "000"; }
            if (TotalReturnLength == 4) { fmt = "0000"; }
            if (TotalReturnLength == 5) { fmt = "00000"; }
            if (TotalReturnLength == 6) { fmt = "000000"; }
            if (TotalReturnLength == 7) { fmt = "0000000"; }
            if (TotalReturnLength == 8) { fmt = "00000000"; }
            if (TotalReturnLength == 9) { fmt = "000000000"; }
            if (TotalReturnLength == 10) { fmt = "0000000000"; }

            string ReturnValue = inNumber.ToString(fmt);  // format string with leading zeros
            return ReturnValue;
        }

        /// <summary>Add leading spaces before a string</summary>
        /// <param name="InText">Original string to add spaces</param>
        /// <param name="SpaceCount">Number of Spaces To Add to String</param>
        public string AddLeadingSpaces(string InText, int SpaceCount)
        {
            string spaces = "";
            int i = 0;
            do
            {
                spaces = spaces + " ";
                i++;
            } while (i < SpaceCount);

            //spaces = "[" + spaces + "]";

            InText = spaces + InText;  // add leading spaces to intext

            return InText;
        }

        /// <summary>Returns number of leading spaces before text begins</summary>
        /// <param name="InText"> </param>
        public int LeadingSpaceCount(string InText)
        {
            return InText.TakeWhile(Char.IsWhiteSpace).Count();
        }

        /// <summary>convert string to Proper casing -- Output: This Is A String Test</summary>
        /// <param name="InText"> </param>
        public string ToTitleCase(string InText)
        {
            System.Globalization.CultureInfo cultureInfo = System.Threading.Thread.CurrentThread.CurrentCulture;
            System.Globalization.TextInfo textInfo = cultureInfo.TextInfo;
            return textInfo.ToTitleCase(InText);
        }

        /// <summary>Search list for Contains match - otherwise take close word match</summary>
        /// <param name="SearchTerm"> </param>
        /// <param name="SearchList"> </param>
        /// <param name="Debug"> </param>
        public string Closest_FileName(string SearchTerm, List<string> SearchList, bool Debug = false)
        {
            string close = "";
            foreach (string imaGe in SearchList)
            {
                if (imaGe.ToUpper().Contains(SearchTerm.ToUpper()))
                {
                    close = imaGe;
                    if (Debug) { MsgBox("In String: " + close); }
                }
            }

            if (close == "")  // no exact matches found - try closest match
            {
                close = Closest_Word(SearchTerm, SearchList);
                if (Debug) { MsgBox("Closest Word: " + close); }
            }

            return close;
        }

        /// <summary>Find the closest match in a list to search word</summary>
        /// <param name="SearchWord"> </param>
        /// <param name="WordList"> </param>
        public string Closest_Word(string SearchWord, List<string> WordList)
        {
            string[] terms = WordList.ToArray();

            string term = SearchWord.ToLower();
            List<string> list = terms.ToList();
            if (list.Contains(term))
                return list.Find(t => t.ToLower() == term);
            else
            {
                int[] counter = new int[terms.Length];
                for (int i = 0; i < terms.Length; i++)
                {
                    for (int x = 0; x < Math.Min(term.Length, terms[i].Length); x++)
                    {
                        int difference = Math.Abs(term[x] - terms[i][x]);
                        counter[i] += difference;
                    }
                }

                int min = counter.Min();
                int index = counter.ToList().FindIndex(t => t == min);
                return terms[index];
            }
        }

        /// <summary>Reverse Order of Characters in String</summary>
        /// <param name="StringToReverse">String to reverse</param>
        public string Reverse(string StringToReverse)
        {
            char[] charArray = StringToReverse.ToCharArray();
            Array.Reverse(charArray);
            return new string(charArray);
        }

        /// <summary>
        /// Encodes Title Text into Database Compatible Storage
        /// </summary>
        /// <param name="TitleText"></param>
        /// <returns></returns>
        public string Encode(string TitleText)
        {
            return TitleText.Encode();
        }

        /// <summary>
        /// Decodes Encoded Text into Display Text
        /// </summary>
        /// <param name="TitleText"></param>
        /// <returns></returns>
        public string Decode(string TitleText)
        {
            return TitleText.Decode();
        }


        #region === Object Strings (ObjStrings) ===


        /// <summary>
        /// Used for Object Strings Separated by '|' - this temporarily replaces with temp char to void parsing errors
        /// </summary>
        /// <param name="Text">Text Potentially Containing '|' to Replace</param>
        /// <returns></returns>
        public string ObjStringFix(string Text)
        {
            string text = Text.Replace("|", "Î");
            return text;
        }

        /// <summary>
        /// Restores Object String Value, Adding Back Replaced Temp Character
        /// </summary>
        /// <param name="Text"></param>
        /// <returns></returns>
        public string ObjStringRestore(string Text)
        {
            string text = Text.Replace("Î", "|");
            return text;
        }

        /// <summary>
        /// Convert Object String to Dictionary (object name, Col1, Val1, Col2...)
        /// </summary>
        /// <param name="ObjString"></param>
        /// <returns></returns>
        public Dictionary<string, string> ObjStringToDict(string ObjString)
        {
            Dictionary<string, string> items = new Dictionary<string, string>();

            List<string> lines = StringSplit_List(ObjString, "|", false);
            int i = 1; bool first = true; string First = "";
            foreach (string item in lines)
            {
                if (i == 1) { items.Add(item, item); i++; continue; }
                if (first) { First = item; first = false; continue; }
                if (!first) { items.Add(First.ObjStringRestore(), item.ObjStringRestore()); first = true; }
                i++;
            }

            return items;
        }


        /// <summary>
        /// Convert List of ObjectStrings to DataTable
        /// </summary>
        /// <param name="ObjStrings"></param>
        /// <param name="Checked"></param>
        /// <returns></returns>
        public DataTable ObjectStringDTs(List<string> ObjStrings, bool Checked = false)
        {
            DataTable table = new DataTable();
            table.Columns.Add("Selected", typeof(bool));
            table.Columns.Add("FieldName", typeof(string));
            table.Columns.Add("FieldValue", typeof(string));
            table.Columns.Add("FieldType", typeof(string));

            string cked = "False"; if (Checked) { cked = "True"; }

            foreach (string ObjString in ObjStrings)
            {
                string VarType = "";
                List<string> lines = StringSplit_List(ObjString, "|", false);
                int i = 1; bool first = true; string First = ""; string Next = "";
                foreach (string item in lines)
                {
                    if (VarType == "") { VarType = item; continue; }
                    if (first) { First = item; first = false; continue; }
                    if (!first)
                    {
                        try { table.Rows.Add(cked, First.ObjStringRestore(), item.ObjStringRestore(), VarType); } catch { table.Rows.Add(cked, First.ObjStringRestore(), "", VarType); }
                        first = true;
                    }
                    i++;
                }
            }

            return table;
        }

        /// <summary>
        /// Convert ObjectString to DataTable
        /// </summary>
        /// <param name="ObjString">ObjectString Format</param>
        public DataTable ObjStringDT(string ObjString)
        {
            DataTable table = new DataTable();
            table.Columns.Add("Selected", typeof(bool));
            table.Columns.Add("FieldName", typeof(string));
            table.Columns.Add("FieldValue", typeof(string));
            table.Columns.Add("FieldType", typeof(string));

            string VarType = "";
            List<string> lines = StringSplit_List(ObjString, "|", false);
            int i = 1; bool first = true; string First = "";
            foreach (string item in lines)
            {
                if (VarType == "") { VarType = item; continue; }
                if (first) { First = item; first = false; continue; }
                if (!first)
                {
                    try { table.Rows.Add("False", First.ObjStringRestore(), item.ObjStringRestore(), VarType); } catch { table.Rows.Add("False", First.ObjStringRestore(), "", VarType); }
                    first = true;
                }
                i++;
            }

            return table;
        }

        /// <summary>
        /// Extract the ObjectType from ObjectString
        /// </summary>
        /// <param name="ObjString">ObjectString Format</param>
        /// <returns></returns>
        public string ObjStringVarType(string ObjString)
        {
            List<string> lines = StringSplit_List(ObjString, "|", false);
            foreach (string item in lines) { return item; }
            return "";
        }


        public class dtLoop
        {
            public bool Checked { get; set; }
            public string FieldName { get; set; }
            public string FieldValue { get; set; }
            public string FieldType { get; set; }
            public string FieldCompare { get; set; }
        }


        /// <summary>
        /// Convert DataTable to dtLoop List
        /// </summary>
        /// <param name="Table">DTLoop Table</param>
        /// <returns></returns>
        public List<dtLoop> DTLoop_DtToDtLoop(DataTable Table)
        {
            List<dtLoop> Items = new List<dtLoop>();

            // loop through datatable
            foreach (DataRow row in Table.Rows)
            {
                dtLoop Item = new dtLoop();
                int i = 0;
                foreach (var item in row.ItemArray)
                {
                    if (i == 0) { Item.Checked = item.ToBool(); }
                    if (i == 1) { Item.FieldName = item.ToString(); }
                    if (i == 2) { Item.FieldValue = item.ToString(); }
                    if (i == 3) { Item.FieldType = item.ToString(); }
                    i++; if (i > 3) { i = 0; }
                }
                Items.Add(Item);
            }

            return Items;
        }


        /// <summary>
        /// Convert dtLoop List to DataTable
        /// </summary>
        /// <param name="LoopList">dtLoop List to Convert</param>
        /// <returns></returns>
        public DataTable DTLoop_ToDT(List<dtLoop> LoopList)
        {
            DataTable table = new DataTable();
            table.Columns.Add("Selected", typeof(bool));
            table.Columns.Add("FieldName", typeof(string));
            table.Columns.Add("FieldValue", typeof(string));
            table.Columns.Add("FieldType", typeof(string));

            foreach (dtLoop row in LoopList)
            {
                try { table.Rows.Add(row.Checked, row.FieldName, ObjStringFix(row.FieldValue), row.FieldType); } catch { table.Rows.Add("False", row.FieldName, "NULL", row.FieldType); }
            }

            return table;
        }


        /// <summary>
        /// Convert ObjString to DataTable
        /// </summary>
        /// <param name="RadGrid"></param>
        /// <returns></returns>
        public DataTable DTLoop_FromObjString(string ObjString)
        {
            DataTable table = new DataTable();
            table.Columns.Add("Selected", typeof(bool));
            table.Columns.Add("FieldName", typeof(string));
            table.Columns.Add("FieldValue", typeof(string));
            table.Columns.Add("FieldType", typeof(string));

            string VarType = "";
            List<string> lines = StringSplit_List(ObjString, "|", false);
            int i = 1; string selected = ""; string fieldname = ""; string fieldvalue = ""; string fieldtype = "";
            foreach (string item in lines)
            {
                if (VarType == "") { VarType = item; continue; }

                if (i == 1) { selected = item; i++; continue; }
                if (i == 2) { fieldname = item; i++; continue; }
                if (i == 3) { fieldvalue = item; i++; continue; }
                if (i == 4)
                {
                    fieldtype = item;
                    try { table.Rows.Add(selected, fieldname, ObjStringRestore(fieldvalue), fieldtype); } catch { table.Rows.Add("False", fieldname, "", fieldtype); }
                    i = 1;
                }
            }

            return table;
        }


        /// <summary>
        /// Convert ObjString to dtLoop List
        /// </summary>
        /// <param name="RadGrid"></param>
        /// <returns></returns>
        public List<dtLoop> DTLoop_DtLoopFromObjString(string ObjString)
        {
            List<dtLoop> Items = new List<dtLoop>();

            string VarType = "";
            List<string> lines = StringSplit_List(ObjString, "|", false);
            int i = 1; string selected = ""; string fieldname = ""; string fieldvalue = ""; string fieldtype = "";
            foreach (string item in lines)
            {
                if (VarType == "") { VarType = item; continue; }

                if (i == 1) { selected = item; i++; continue; }
                if (i == 2) { fieldname = item; i++; continue; }
                if (i == 3) { fieldvalue = item; i++; continue; }
                if (i == 4)
                {
                    fieldtype = item;

                    dtLoop Item = new dtLoop();
                    Item.Checked = selected.ToBool();
                    Item.FieldName = fieldname;
                    Item.FieldValue = ObjStringRestore(fieldvalue);
                    Item.FieldType = fieldtype;
                    Items.Add(Item);
                }
            }

            return Items;
        }


        /// <summary>
        /// Merge Two ObjString Lines from DTLoop Table
        /// </summary>
        /// <param name="MainObjString"></param>
        /// <param name="AddObjString"></param>
        /// <returns></returns>
        public string DTLoop_CombineObjStrings(string MainObjString, string AddObjString)
        {
            // remove header from new objString
            string VarType = "";
            List<string> lines = StringSplit_List(AddObjString, "|", false);
            foreach (string item in lines) { if (VarType == "") { VarType = item; break; } }

            string add = AddObjString.Replace(VarType + "|", ""); // remove header from objString
            string all = MainObjString + add;

            return all;
        }

        /// <summary>
        /// Convert List of ObjStrings to DataTable
        /// </summary>
        /// <param name="RadGrid"></param>
        /// <returns></returns>
        public DataTable DTLoop_FromObjStrings(List<string> ObjStrings)
        {
            DataTable table = new DataTable();
            table.Columns.Add("Selected", typeof(bool));
            table.Columns.Add("FieldName", typeof(string));
            table.Columns.Add("FieldValue", typeof(string));
            table.Columns.Add("FieldType", typeof(string));

            foreach (string ObjString in ObjStrings)
            {
                string VarType = "";
                List<string> lines = StringSplit_List(ObjString, "|", false);
                int i = 1; string selected = ""; string fieldname = ""; string fieldvalue = ""; string fieldtype = "";
                foreach (string item in lines)
                {
                    if (VarType == "") { VarType = item; continue; }
                    if (i == 1) { selected = item; i++; continue; }
                    if (i == 2) { fieldname = item; i++; continue; }
                    if (i == 3) { fieldvalue = item; i++; continue; }
                    if (i == 4)
                    {
                        fieldtype = item;
                        try { table.Rows.Add(selected, fieldname, ObjStringRestore(fieldvalue), fieldtype); } catch { table.Rows.Add("False", fieldname, "", fieldtype); }
                        i = 1;
                    }
                }
            }

            return table;
        }


        /// <summary>
        /// Loop Through List of dtLoop Values, Returns True if Conditions List Matches All Values in ValuesList
        /// </summary>
        /// <param name="Values">Dataset of Values</param>
        /// <param name="Conditions">Specific Values to Find and Match</param>
        /// <param name="RequireAllConditionsFound">All Conditions Must Be Found In ValueList In Order To Be True</param>
        /// <returns></returns>
        public bool DTLoop_ConditionMatch(List<dtLoop> ValuesList, List<dtLoop> Conditions, out List<dtLoop> Results, bool RequireAllConditionsFound = true)
        {
            Results = new List<dtLoop>();
            bool Matched = true;
            int GoodMatches = 0;
            int BadMatches = 0;
            foreach (dtLoop value in ValuesList)
            {
                dtLoop result = new dtLoop();
                foreach (dtLoop con in Conditions)
                {
                    if (value.FieldName == con.FieldName && value.FieldType == con.FieldType) // condition found list, compare value
                    {
                        result.FieldName = value.FieldName; result.FieldType = value.FieldType;

                        if (value.FieldValue != con.FieldValue)
                        {
                            result.FieldValue = value.FieldValue + " != " + con.FieldValue + " (expected)";
                            result.Checked = true;
                            Matched = false; BadMatches++;
                        }
                        else
                        {
                            result.FieldValue = value.FieldValue + " = " + con.FieldValue;
                            result.Checked = false;
                            GoodMatches++;
                        }
                    }
                }
                Results.Add(result);
            }
            if (RequireAllConditionsFound) { if (GoodMatches != Conditions.Count) { Matched = false; } }
            return Matched;
        }

        /// <summary>
        /// Compare Two dtLoop Lists, Returns List of Value Differences
        /// </summary>
        /// <param name="BeforeList">Dataset of Values</param>
        /// <param name="AfterList">Specific Values to Find and Match</param>
        /// <returns></returns>
        public List<dtLoop> DTLoop_DiffList(List<dtLoop> BeforeList, List<dtLoop> AfterList)
        {
            List<dtLoop> Diff = new List<dtLoop>();
            foreach (dtLoop value in BeforeList)
            {
                foreach (dtLoop con in AfterList)
                {
                    if (value.FieldName == con.FieldName && value.FieldType == con.FieldType) // condition found list, compare value
                    {
                        dtLoop diff = new dtLoop();

                        diff.FieldName = value.FieldName; diff.FieldType = value.FieldType;

                        if (value.FieldValue != con.FieldValue)
                        {
                            diff.FieldValue = "(was) " + value.FieldValue + " (now) " + con.FieldValue;
                            Diff.Add(diff);
                        }
                    }
                }
            }
            return Diff;
        }

        public DataTable DTLoop_DiffDT(List<dtLoop> BeforeList, List<dtLoop> AfterList)
        {
            DataTable dt = new DataTable();
            List<dtLoop> Diff = new List<dtLoop>();
            foreach (dtLoop value in BeforeList)
            {
                foreach (dtLoop con in AfterList)
                {
                    if (value.FieldName == con.FieldName && value.FieldType == con.FieldType) // condition found list, compare value
                    {
                        dtLoop diff = new dtLoop();

                        diff.FieldName = value.FieldName; diff.FieldType = value.FieldType;

                        if (value.FieldValue != con.FieldValue)
                        {
                            diff.FieldValue = "(was) " + value.FieldValue + " (now) " + con.FieldValue;
                            Diff.Add(diff);
                        }
                    }
                }
            }

            return DTLoop_ToDT(Diff);
        }

        public DataTable DtLoop_ObjStringDiffDT(string BeforeObjString, string AfterObjString)
        {
            List<dtLoop> Before = DTLoop_DtLoopFromObjString(BeforeObjString);
            List<dtLoop> After = DTLoop_DtLoopFromObjString(AfterObjString);
            return DTLoop_DiffDT(Before, After);
        }






        #endregion


    }
}
