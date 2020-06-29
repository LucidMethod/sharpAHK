using sharpAHK;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace AHKExpressions
{
    public static partial class AHKExpressions
    {

        // === Strings ===

        /// <summary>
        /// Returns # of Words in String
        /// </summary>
        /// <param name="Text">String to check word count</param>
        /// <returns></returns>
        public static int WordCount(this String Text)
        {
            _AHK ahk = new _AHK();
            return ahk.WordCount(Text);
        }

        /// <summary>
        /// Replaces Invalid SQL Characters in Insert/Update Statements
        /// </summary>
        /// <param name="Text"></param>
        /// <returns></returns>
        public static string SQL(this string Text)
        {
            return Text.Replace("'", "''");
        }

        /// <summary>Transforms a YYYYMMDDHH24MISS timestamp into the specified date/time format.</summary>
        /// <param name="YYYYMMDDHH24MISS">Leave this parameter blank to use the current local date and time. Otherwise, specify all or the leading part of a timestamp in the YYYYMMDDHH24MISS format. If the date and/or time portion of the timestamp is invalid -- such as February 29th of a non-leap year -- the date and/or time will be omitted from OutputVar. Although only years between 1601 and 9999 are supported, a formatted time can still be produced for earlier years as long as the time portion is valid.</param>
        /// <param name="Format">If omitted, it defaults to the time followed by the long date, both of which will be formatted according to the current user's locale. For example: 4:55 PM Saturday, November 27, 2004 Otherwise, specify one or more of the date-time formats below, along with any literal spaces and punctuation in between (commas do not need to be escaped; they can be used normally). In the following example, note that M must be capitalized: M/d/yyyy h:mm tt</param>
        public static string FormatTime(string YYYYMMDDHH24MISS = "", string Format = "")
        {
            _AHK ahk = new _AHK();
            return ahk.FormatTime(YYYYMMDDHH24MISS, Format);
        }

        /// <summary>Determines whether string comparisons are case sensitive (default is "not case sensitive").</summary>
        /// <param name="OnOffLocale">On: String comparisons are case sensitive. This setting also makes the expression equal sign operator (=) and the case-insensitive mode of InStr() use the locale method described below. Off (starting default): The letters A-Z are considered identical to their lowercase counterparts. This is the starting default for all scripts due to backward compatibility and performance (Locale is 1 to 8 times slower than Off depending on the nature of the strings being compared). Locale [v1.0.43.03+]: String comparisons are case insensitive according to the rules of the current user's locale. For example, most English and Western European locales treat not only the letters A-Z as identical to their lowercase counterparts, but also ANSI letters like Ä and Ü as identical to theirs. </param>
        public static void StringCaseSense(this string OnOffLocale)
        {
            _AHK ahk = new _AHK();
            ahk.StringCaseSense(OnOffLocale);
        }

        /// <summary>Retrieves the position of the specified substring within a string.</summary>
        /// <param name="InputVar">The name of the input variable, whose contents will be searched. Do not enclose the name in percent signs unless you want the contents of the variable to be used as the name.</param>
        /// <param name="SearchText">The string to search for. Matching is not case sensitive unless StringCaseSense has been turned on. </param>
        /// <param name="LR">This affects which occurrence will be found if SearchText occurs more than once within InputVar. If this parameter is omitted, InputVar will be searched starting from the left for the first match. If this parameter is 1 or the letter R, the search will start looking at the right side of InputVar and will continue leftward until the first match is found. To find a match other than the first, specify the letter L or R followed by the number of the occurrence. For example, to find the fourth occurrence from the right, specify r4. Note: If the number is less than or equal to zero, no match will be found.</param>
        /// <param name="Offset">The number of characters on the leftmost or rightmost side (depending on the parameter above) to skip over. If omitted, the default is 0. For example, the following would start searching at the tenth character from the left: StringGetPos, OutputVar, InputVar, abc, , 9. This parameter can be an expression.</param>
        public static string StringGetPos(this string InputVar, string SearchText, string LR = "", string Offset = "")
        {
            _AHK ahk = new _AHK();
            return ahk.StringGetPos(InputVar, SearchText, LR, Offset);
        }

        /// <summary>Retrieves a number of characters from the left-hand side of a string.</summary>
        /// <param name="InputVar">The name of the variable whose contents will be extracted from. Do not enclose the name in percent signs unless you want the contents of the variable to be used as the name.</param>
        /// <param name="Count">The number of characters to extract, which can be an expression. If Count is less than or equal to zero, OutputVar will be made empty (blank). If Count exceeds the length of InputVar, OutputVar will be set equal to the entirety of InputVar.</param>
        public static string StringLeft(this string InputVar, object Count)
        {
            _AHK ahk = new _AHK();
            return ahk.StringLeft(InputVar, Count);
        }

        /// <summary>Retrieves a number of characters from the right-hand side of a string.</summary>
        /// <param name="InputVar">The name of the variable whose contents will be extracted from. Do not enclose the name in percent signs unless you want the contents of the variable to be used as the name.</param>
        /// <param name="Count">The number of characters to extract, which can be an expression. If Count is less than or equal to zero, OutputVar will be made empty (blank). If Count exceeds the length of InputVar, OutputVar will be set equal to the entirety of InputVar.</param>
        public static string StringRight(this string InputVar, object Count)
        {
            _AHK ahk = new _AHK();
            return ahk.StringRight(InputVar, Count);
        }

        /// <summary>Retrieves the count of how many characters are in a string.</summary>
        /// <param name="InputVar">The name of the variable whose contents will be measured. Do not enclose the name in percent signs unless you want the contents of the variable to be used as the name.</param>
        public static string StringLen(this string InputVar)
        {
            _AHK ahk = new _AHK();
            return ahk.StringLen(InputVar);
        }

        /// <summary>Converts a string to lowercase.</summary>
        /// <param name="InputVar">The name of the variable whose contents will be read from. Do not enclose the name in percent signs unless you want the contents of the variable to be used as the name.</param>
        /// <param name="ProperCase">If this parameter is True, the string will be converted to title case. For example, "GONE with the WIND" would become "Gone With The Wind". </param>
        public static string StringLower(this string InputVar, bool ProperCase = false)
        {
            _AHK ahk = new _AHK();
            return ahk.StringLower(InputVar, ProperCase);
        }

        /// <summary>Converts a string to uppercase.</summary>
        /// <param name="InputVar">The name of the variable whose contents will be read from. Do not enclose the name in percent signs unless you want the contents of the variable to be used as the name.</param>
        /// <param name="ProperCase">If this parameter is True, the string will be converted to title case. For example, "GONE with the WIND" would become "Gone With The Wind". </param>
        public static string StringUpper(this string InputVar, bool ProperCase = false)
        {
            _AHK ahk = new _AHK();
            return ahk.StringUpper(InputVar, ProperCase);
        }

        /// <summary>Retrieves one or more characters from the specified position in a string.</summary>
        /// <param name="InputVar">The name of the variable from whose contents the substring will be extracted. Do not enclose the name in percent signs unless you want the contents of the variable to be used as the name.</param>
        /// <param name="StartChar">The position of the first character to be extracted, which can be an expression. Unlike StringGetPos, 1 is the first character. If StartChar is less than 1, it will be assumed to be 1. If StartChar is beyond the end of the string, OutputVar is made empty (blank).</param>
        /// <param name="Count"> this parameter may be omitted or left blank, which is the same as specifying an integer large enough to retrieve all characters from the string. Otherwise, specify the number of characters to extract, which can be an expression. If Count is less than or equal to zero, OutputVar will be made empty (blank). If Count exceeds the length of InputVar measured from StartChar, OutputVar will be set equal to the entirety of InputVar starting at StartChar.</param>
        /// <param name="L">The letter L can be used to extract characters that lie to the left of StartChar rather than to the right. In the following example, OutputVar will be set to Red: InputVar = The Red Fox StringMid, OutputVar, InputVar, 7, 3, L If the L option is present and StartChar is less than 1, OutputVar will be made blank. If StartChar is beyond the length of InputVar, only those characters within reach of Count will be extracted. For example, the below will set OutputVar to Fox: InputVar = The Red Fox StringMid, OutputVar, InputVar, 14, 6, L</param>
        public static string StringMid(this string InputVar, string StartChar, string Count = "", string L = "")
        {
            _AHK ahk = new _AHK();
            return ahk.StringMid(InputVar, StartChar, Count, L);
        }

        /// <summary>Replaces the specified substring with a new string.</summary>
        /// <param name="InputVar">The name of the variable whose contents will be read from. Do not enclose the name in percent signs unless you want the contents of the variable to be used as the name.</param>
        /// <param name="SearchText">The string to search for. Matching is not case sensitive unless StringCaseSense has been turned on.</param>
        /// <param name="ReplaceText">SearchText will be replaced with this text. If omitted or blank, SearchText will be replaced with blank (empty). In other words, it will be omitted from OutputVar.</param>
        /// <param name="ReplaceAll">If omitted, only the first occurrence of SearchText will be replaced. But if this parameter is 1, A, or All, all occurrences will be replaced. Specify the word UseErrorLevel to store in ErrorLevel the number of occurrences replaced (0 if none). UseErrorLevel implies "All".</param>
        public static string StringReplace(this string InputVar, string SearchText, string ReplaceText = "", string ReplaceAll = "", int Mode = 1)
        {
            _AHK ahk = new _AHK();
            return ahk.StringReplace(InputVar, SearchText, ReplaceText, ReplaceAll, Mode);
        }

        /// <summary>Removes a number of characters from the left-hand side of a string.</summary>
        /// <param name="InputVar">The name of the variable whose contents will be read from. Do not enclose the name in percent signs unless you want the contents of the variable to be used as the name.</param>
        /// <param name="Count">The number of characters to remove, which can be an expression. If Count is less than or equal to zero, OutputVar will be set equal to the entirety of InputVar. If Count exceeds the length of InputVar, OutputVar will be made empty (blank).</param>
        public static string StringTrimLeft(this string InputVar, string Count)
        {
            _AHK ahk = new _AHK();
            return ahk.StringTrimLeft(InputVar, Count);
        }

        /// <summary>Removes a number of characters from the right-hand side of a string.</summary>
        /// <param name="InputVar">The name of the variable whose contents will be read from. Do not enclose the name in percent signs unless you want the contents of the variable to be used as the name.</param>
        /// <param name="Count">The number of characters to remove, which can be an expression. If Count is less than or equal to zero, OutputVar will be set equal to the entirety of InputVar. If Count exceeds the length of InputVar, OutputVar will be made empty (blank).</param>
        public static string StringTrimRight(this string InputVar, string Count)
        {
            _AHK ahk = new _AHK();
            return ahk.StringTrimRight(InputVar, Count);
        }

        /// <summary>Checks if a string contains the specified string.</summary>
        /// <param name="Text">Text to search for SearchString</param>
        /// <param name="SearchString">The string to search for. Matching is not case sensitive unless StringCaseSense has been turned on.</param>
        public static bool IfInString(this string Text, string SearchString)
        {
            _AHK ahk = new _AHK();
            return ahk.IfInString(Text, SearchString);
        }

        /// <summary>Checks if a string contains the specified string.</summary>
        /// <param name="Text">Text to search for SearchString</param>
        /// <param name="SearchString">The string to search for. Matching is not case sensitive unless StringCaseSense has been turned on.</param>
        public static bool IfNotInString(this string Text, string SearchString)
        {
            _AHK ahk = new _AHK();
            return ahk.IfNotInString(Text, SearchString);
        }

        /// <summary>Arranges a variable's contents in alphabetical, numerical, or random order (optionally removing duplicates).</summary>
        /// <param name="Text">String whose contents will be sorted.</param>
        /// <param name="Options">C = Case Sensitive Sort | CL = Case Insensitive sort based on User's Locale | Dx = Specifies x as the delimiter character, which determins where each item in the Text begins and ends (default = newline) | F MyFunction = uses custom sorting according to the criteria in MyFunction | N = Numeric Sort | Pn = Sorts items based on character position n | R = Sorts in reverse order | Random = Sorts in random order | U = Removes Duplicates | Z = Last linefeed is considered to be part of the last item</param>
        public static string Sort(this string Text, string Options = "")
        {
            _AHK ahk = new _AHK();
            return ahk.Sort(Text, Options);
        }


        // ### New Additions ###

        /// <summary>Removes Numbers from String</summary>
        /// <param name="InString"> </param>
        public static string Remove_Numbers(this string InString)
        {
            _AHK ahk = new _AHK();
            return ahk.Remove_Numbers(InString);
        }

        /// <summary>
        /// Remove Letters from String
        /// </summary>
        /// <param name="InString"></param>
        /// <returns></returns>
        public static string Remove_Letters(this string InString)
        {
            _AHK ahk = new _AHK();
            return ahk.Remove_Letters(InString);
        }

        /// <summary>Remove HTML characters from string</summary>
        /// <param name="HTML">HTML to strip</param>
        public static string UnHtml(this string HTML)
        {
            _AHK ahk = new _AHK();
            return ahk.UnHtml(HTML);
        }

        /// <summary>Returns the number of times a character is found in a string</summary>
        /// <param name="Line"> </param>
        /// <param name=" Char"> </param>
        public static int CharCount(this string Line, string Char)
        {
            _AHK ahk = new _AHK();
            return ahk.CharCount(Line, Char);
        }

        /// <summary>Remove X Characters from beginning of string</summary>
        /// <param name="str"> </param>
        /// <param name="RemoveCharacterCount"> </param>
        public static string TrimFirst(this string str, int RemoveCharacterCount = 1)
        {
            _AHK ahk = new _AHK();
            return ahk.TrimFirst(str, RemoveCharacterCount);
        }

        /// <summary>Remove X Characters from end of string</summary>
        /// <param name="str"> </param>
        /// <param name="RemoveCharacterCount"> </param>
        public static string TrimLast(this string str, int RemoveCharacterCount = 1)
        {
            _AHK ahk = new _AHK();
            return ahk.TrimLast(str, RemoveCharacterCount);
        }

        /// <summary>
        /// Trims 0's from beginning of string 
        /// </summary>
        /// <param name="Text">String to trim leading zeros from</param>
        /// <returns>Returns string minus leading zeroes</returns>
        public static string TrimLeadingZeros(this string Text)
        {
            _AHK ahk = new _AHK();
            return ahk.TrimLeadingZeros(Text);
        }

        /// <summary>
        /// Trim ending characters from string if they exist, returns string without ending chars
        /// </summary>
        /// <param name="Text"></param>
        /// <param name="ToTrim"></param>
        /// <returns></returns>
        public static string TrimEndIf(this string Text, string ToTrim)
        {
            _AHK ahk = new _AHK();
            return ahk.TrimEndIf(Text, ToTrim);
        }

        /// <summary>
        /// Trims all of a specific leading character from the from beginning of string { UNTESTED }
        /// </summary>
        /// <param name="Text"></param>
        /// <param name="Char"></param>
        /// <returns></returns>
        public static string TrimLeadingChars(this string Text, string Char = "0")
        {
            _AHK ahk = new _AHK();
            return ahk.TrimLeadingChars(Text, Char);
        }

        /// <summary>Returns first X characters in string</summary>
        /// <param name="Text"> </param>
        /// <param name="NumberOfCharacters"> </param>
        public static string FirstCharacters(this string Text, int NumberOfCharacters = 1)
        {
            _AHK ahk = new _AHK();
            return ahk.FirstCharacters(Text, NumberOfCharacters);
        }

        /// <summary>Returns last X characters in string</summary>
        /// <param name="Text"> </param>
        /// <param name="NumberOfCharacters"> </param>
        public static string LastCharacters(this string Text, int NumberOfCharacters = 1)
        {
            _AHK ahk = new _AHK();
            return ahk.LastCharacters(Text, NumberOfCharacters);
        }

        /// <summary>Returns First word in string</summary>
        /// <param name="InputString"> </param>
        public static string FirstWord(this string InputString)  // returns First word in string 
        {
            _AHK ahk = new _AHK();
            return ahk.FirstWord(InputString);
        }

        /// <summary>Returns last word in string</summary>
        /// <param name="InputString"> </param>
        public static string LastWord(this string InputString)
        {
            _AHK ahk = new _AHK();
            return ahk.LastWord(InputString);
        }

        /// <summary>Return specific word # from string</summary>
        /// <param name="InputString"> </param>
        /// <param name="WordNumber"> </param>
        public static string WordNum(this string InputString, int WordNumber = 1)
        {
            _AHK ahk = new _AHK();
            return ahk.WordNum(InputString, WordNumber);
        }

        /// <summary>Parse line by space, returns list of words</summary>
        /// <param name="InputString"> </param>
        public static List<string> WordList(this string InputString)
        {
            _AHK ahk = new _AHK();
            return ahk.WordList(InputString);
        }

        /// <summary>Parse line by new line, returns list of lines</summary>
        /// <param name="InputString"> </param>
        /// <param name="Trim"> </param>
        /// <param name="RemoveBlanks"> </param>
        public static List<string> LineList(this string InputString, bool Trim = true, bool RemoveBlanks = false)
        {
            _AHK ahk = new _AHK();
            return ahk.LineList(InputString, Trim, RemoveBlanks);
        }

        /// <summary>Returns code line without comments</summary>
        /// <param name="line"> </param>
        /// <param name="CommentCharacters"> </param>
        public static string RemoveComments(this string line, string CommentCharacters = "//")
        {
            _AHK ahk = new _AHK();
            return ahk.RemoveComments(line, CommentCharacters);
        }

        /// <summary>Returns comments on line after code</summary>
        /// <param name="line"> </param>
        /// <param name="CommentCharacters"> </param>
        public static string ReturnComments(this string line, string CommentCharacters = "//")
        {
            _AHK ahk = new _AHK();
            return ahk.ReturnComments(line, CommentCharacters);
        }

        /// <summary>Extracts text between brackets</summary>
        /// <param name="Code"> </param>
        /// <param name="start"> </param>
        /// <param name="end"> </param>
        public static string Extract_Between(this string Code, string start = "{", string end = "}")
        {
            _AHK ahk = new _AHK();
            return ahk.Extract_Between(Code, start, end);
        }

        /// <summary>Extract text between <Tag> XML style tags </Tag></summary>
        /// <param name="XMLString">String to extract tag text from</param>
        /// <param name="Tag">Name of tag to return text between. Ex: <UserTag>About this Project</UserTag> returns "About this Project"</param>
        public static string XML_TagText(this string XMLString, string Tag)
        {
            _AHK ahk = new _AHK();
            return ahk.XML_TagText(XMLString, Tag);
        }


        /// <summary>Insert text into specific position in string</summary>
        /// <param name="InText"> </param>
        /// <param name="InsertText"> </param>
        /// <param name="Position"> </param>
        public static string Insert(this string InText, string InsertText, int Position)
        {
            _AHK ahk = new _AHK();
            return ahk.Insert_Text(InText, InsertText, Position);
        }

        /// <summary>Split string by character (return pos starts at word 0, final option overrides position to return last item)</summary>
        /// <param name="InText">Text to split</param>
        /// <param name="SplitChar">Character(s) to split string by</param>
        /// <param name="ReturnPos">Position # of the word(s) to return. Ex: ReturnPos 0 returns the text before the SplitChar is found, 1 returns the text after the first splitchar and before the 2nd splitchar</param>
        /// <param name="ReturnLast">Override for ReturnPos value - will return last value in split string</param>
        /// <param name="NoBlanks">Option to return next available value if ReturnPos value is blank</param>
        public static string StringSplit(this string InText, string SplitChar = "(", int ReturnPos = 0, bool ReturnLast = false, bool NoBlanks = true)
        {
            _AHK ahk = new _AHK();
            return ahk.StringSplit(InText, SplitChar, ReturnPos, ReturnLast, NoBlanks);
        }

        /// <summary>
        /// Split string by character, Returns List of values separated by the SplitChar
        /// </summary>
        /// <param name="InText"></param>
        /// <param name="SplitChar">Character(s) to split string by</param>
        /// <returns>Returns List of Items separated by SplitChar</returns>
        public static List<string> StringSplit(this string InText, string SplitChar = "(")
        {
            _AHK ahk = new _AHK();
            return ahk.StringSplit_List(InText, SplitChar);
        }

        /// <summary>Add leading zeros to an int/string, ex: InNumber 12 with TotalReturnLength 5 returns string "00012"</summary>
        /// <param name="InNumber">Original number (int or string) to add leading zeros to.</param>
        /// <param name="TotalReturnLength">Total # of desired digits, adding zeros in front of the InNumber to ensure return value is TotalReturnLength characters long.</param>
        /// <tested>True</tested>
        public static string AddLeadingZeros(this object InNumber, int TotalReturnLength = 5)
        {
            _AHK ahk = new _AHK();
            return ahk.AddLeadingZeros(InNumber, TotalReturnLength);
        }

        /// <summary>Add leading spaces before a string</summary>
        /// <param name="InText">Original string to add spaces</param>
        /// <param name="SpaceCount">Number of Spaces To Add to String</param>
        public static string AddLeadingSpaces(this string InText, int SpaceCount)
        {
            _AHK ahk = new _AHK();
            return ahk.AddLeadingSpaces(InText, SpaceCount);
        }

        /// <summary>Returns number of leading spaces before text begins</summary>
        /// <param name="InText"></param>
        public static int LeadingSpaceCount(this string InText)
        {
            _AHK ahk = new _AHK();
            return ahk.LeadingSpaceCount(InText);
        }

        /// <summary>Converts string to Proper Casing -- Output: This Is A String Test</summary>
        /// <param name="InText">Text to Convert</param>
        public static string ToTitleCase(this string InText)
        {
            _AHK ahk = new _AHK();
            return ahk.ToTitleCase(InText);
        }

        // was: Closest_FileName

        /// <summary>Search list for Contains match - otherwise take close word match</summary>
        /// <param name="SearchTerm"> </param>
        /// <param name="SearchList"> </param>
        /// <param name="Debug"> </param> 
        public static string CloseMatch(this string SearchTerm, List<string> SearchList, bool Debug = false)
        {
            _AHK ahk = new _AHK();
            return ahk.Closest_FileName(SearchTerm, SearchList, Debug);
        }

        /// <summary>
        /// Returns True if File Path is Valid 
        /// </summary>
        /// <param name="FilePath"></param>
        /// <returns></returns>
        public static bool FileExists(this string FilePath)
        {
            return File.Exists(FilePath); 
        }


        /// <summary>
        /// Gets whether the specified Uri is a universal naming convention (UNC) path.
        /// </summary>
        /// <param name="FilePath"></param>
        /// <returns></returns>
        public static bool IsUNC(this string FilePath)
        {
            Uri address = new Uri(FilePath);
            return address.IsUnc;
        }

        /// <summary>
        /// Gets a value indicating whether the specified Uri is a file URI.
        /// </summary>
        /// <param name="FilePath"></param>
        /// <returns></returns>
        public static bool IsFile(this string FilePath)
        {
            Uri address = new Uri(FilePath);
            return address.IsFile;
        }


        /// <summary>Find the closest match in a list to search word</summary>
        /// <param name="SearchWord"> </param>
        /// <param name="WordList"> </param>
        public static string Closest_Word(this string SearchWord, List<string> WordList)
        {
            _AHK ahk = new _AHK();
            return ahk.Closest_Word(SearchWord, WordList);
        }

        /// <summary>Reverse Order of Characters in String</summary>
        /// <param name="StringToReverse">String to reverse</param>
        public static string Reverse(this string StringToReverse)
        {
            _AHK ahk = new _AHK();
            return ahk.Reverse(StringToReverse);
        }

        /// <summary>
        /// Encodes Text into Database Compatible Storage
        /// </summary>
        /// <param name="Text"></param>
        /// <returns></returns>
        public static string Encode(this string Text)
        {
            if (Text == null) { return ""; }
            var encoded = System.Web.HttpUtility.HtmlEncode(Text);
            return encoded.ToString();
        }

        /// <summary>
        /// Decodes HTML Encoded Text into Readable Text
        /// </summary>
        /// <param name="Text"></param>
        /// <returns></returns>
        public static string Decode(this string Text)
        {
            if (Text == null) { return ""; }
            StringWriter myWriter = new StringWriter();
            System.Web.HttpUtility.HtmlDecode(Text, myWriter);  // Decode the encoded string.
            return myWriter.ToString();
        }


        /// <summary>Encrypt String Using passPhrase to Decrypt</summary>
        /// <param name="plainText">String to Encrypt</param>
        /// <param name="passPhrase">Password to Decrypt Later</param>
        public static string Encrypt(this string plainText, string passPhrase)
        {
            if (plainText == null) { return ""; }
            _AHK ahk = new _AHK();
            return ahk.Encrypt(plainText, passPhrase);
        }

        /// <summary>Decrypts an Encrypted String using passphrase</summary>
        /// <param name="cipherText">Encryptd String to Decrypt</param>
        /// <param name="passPhrase">Password to Decrypt</param>
        public static string Decrypt(this string cipherText, string passPhrase)
        {
            if (cipherText == null) { return ""; }
            _AHK ahk = new _AHK();
            return ahk.Decrypt(cipherText, passPhrase);
        }


        /// <summary>
        /// Parses IMDb URL for IMDb Title ID
        /// </summary>
        /// <param name="URL">IMDb.com URL to Parse</param>
        /// <returns>Return IMDb ID</returns>
        public static string IMDbID(this string URL)
        {
            _AHK ahk = new _AHK();
            string ID = URL;
            ID = ahk.StringSplit(ID, "?", 0);
            ID = ahk.StringReplace(ID, "http://www.imdb.com/title/");
            ID = ahk.StringReplace(ID, "http://imdb.com/title/");
            ID = ahk.StringReplace(ID, "/");
            return ID;
        }

        /// <summary>
        /// Replace Text in String (Not Case Sensitive)
        /// </summary>
        /// <param name="str"></param>
        /// <param name="from"></param>
        /// <param name="to"></param>
        /// <returns></returns>
        public static string ReplaceInsensitive(this string str, string from, string to)
        {
            str = Regex.Replace(str, from, to, RegexOptions.IgnoreCase);
            return str;
        }

        /// <summary>Converts text from a text file to list <string>
        /// <param name="Input">Either Text String of Valid Text File Path</param>
        /// <param name="SkipBlankLines">Option To Skip Blank Lines in List Return</param>
        /// <param name="Trim">Option To Trim Each Line</param>
        /// <param name="SkipCommentLines">Skip Lines Starting with '//' (For Excluding C# Comments)</param>
        public static List<string> ToList(this string Input, string SplitChar = "NewLine", bool SkipBlankLines = true, bool Trim = true, bool SkipCommentLines = false)
        {
            _AHK ahk = new _AHK();
            List<string> list = new List<string>();
            if (Input == null) { return list; }

            if (Input.CharCount() < 259) // valid number of chars for a file path
            {
                if (Input.IsFile()) { if (File.Exists(Input)) { Input = ahk.FileRead(Input); } }
                
                if (Input.IsDir())
                {
                    if (Directory.Exists(Input)) { return DirList(Input, "*.*", true, true); }
                }
            }

            if (SplitChar == "NewLine" || SplitChar == "\n" || SplitChar == "\r" || SplitChar == "\n\r")
            {
                // Creates new StringReader instance from System.IO
                using (StringReader reader = new StringReader(Input))
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

                        list.Add(writeline);
                    }
                }
            }
            else
            {
                return ahk.StringSplit_List(Input, SplitChar, SkipBlankLines); 
            }
            return list;
        }


        /// <summary>
        /// Returns the Number of Characters in a String
        /// </summary>
        /// <param name="Text"></param>
        /// <returns></returns>
        public static int CharCount(this string Text)
        {
            return Text.Length; 
        }

        #region === ObjectStrings ===

        /// <summary>
        /// Used for Object Strings Separated by '|' - this temporarily replaces with temp char to void parsing errors
        /// </summary>
        /// <param name="Text">Text Potentially Containing '|' to Replace</param>
        /// <returns></returns>
        public static string ObjStringFix(this string Text)
        {
            _AHK ahk = new _AHK();
            return ahk.ObjStringFix(Text);
        }

        /// <summary>
        /// Restores Object String Value, Adding back Replaced Temp Character
        /// </summary>
        /// <param name="Text"></param>
        /// <returns></returns>
        public static string ObjStringRestore(this string Text)
        {
            _AHK ahk = new _AHK();
            return ahk.ObjStringRestore(Text);
        }


        #endregion

    }
}
