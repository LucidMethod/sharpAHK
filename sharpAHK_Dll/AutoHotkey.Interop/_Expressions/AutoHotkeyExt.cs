using sharpAHK;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AHKExpressions
{
    public static partial class StringExtension
    {
        // #region === AHK_DLL ===

        #region === Log / Debug ===

        ///// <summary>
        ///// Error Level Message / Last Executed Commands / Debug Functions
        ///// </summary>
        //public static void debug(bool DebugDisplay = false)
        //{
        //    bool ErrorLevel = ahkGlobal.ErrorLevel;  // true if error level detected in ahk command
        //    bool ErrorLevelEnabled = ahkGlobal.ErrorLevelEnabled;  // true if error level information is available for this command
        //    string ErrorLevelValue = ahkGlobal.ErrorLevelValue; // ErrorLevel variable value returned from AHK session
        //    string ErrorLevelMsg = ahkGlobal.ErrorLevelMsg;   // assembled error level message to display
        //    string ErrorLevelCustom = ahkGlobal.ErrorLevelCustom;  // custom error level text for a command


        //    string LastOutputVarName = ahkGlobal.LastOutputVarName;
        //    string LastOutputVarValue = ahkGlobal.LastOutputVarValue;

        //    string lastCShpar = ahkGlobal.cSharpCmd;
        //    string LastLine = ahkGlobal.LastLine;
        //    string LastAction = ahkGlobal.LastAction;

        //    string sessionInfo = ahkSessionInfo(false);

        //    string errorLogH = errorLogHist(true);
        //    string cmdH = cmdHist(true);

        //    if (DebugDisplay)
        //        MessageBox.Show("LastFunction = " + lastCShpar + "\nLastAction= " + LastAction + "\nErrorLevel= " + ErrorLevel.ToString() + "\nErrorMsg= " + ErrorLevelMsg + "\nValue= " + ErrorLevelValue + "\n\nLastLine= " + LastLine);
        //}

        //public static void Log(object entry) // not setup
        //{
        //    _AHK ahk = new _AHK();
        //    ahk.Log(entry);
        //}

        // Log / Hist Display|Return

        public static string cmdHist(bool Display = false) // sharpAHK command history
        {
            _AHK ahk = new _AHK();
            return ahk.cmdHist(Display);
        }

        public static string errorLogHist(bool Display = false)
        {
            _AHK ahk = new _AHK();
            return ahk.errorLogHist(Display);
        }

        public static string loadedAHKList(bool Display = false)  // displays list of loaded ahk file in current session
        {
            _AHK ahk = new _AHK();
            return ahk.loadedAHKList(Display);
        }

        #endregion


        // AHK Session

        /// <summary>Resets Global AHK Session - Causes next AHK call to create a new AHK session. Resets all Hotkeys/Functions/Variables in Memory</summary>
        public static void New_AHKSession(bool NewInstance = false)
        {
            _AHK ahk = new _AHK();
            ahk.New_AHKSession(NewInstance);
        }

        /// <summary>Executes AHK Command, Returns Variable Value if ReturnVar is Populated</summary>
        /// <param name="ahkSTRING">AHK Line / Collection of AHK Commands</param>
        /// <param name="ReturnVar">Variable name to return value of from AHK Session after executing ahkSTRING</param>
        /// <param name="NewSession">Option To Initiate New AHK Instance - Resets Previously Used Variables and Loaded AHK</param>
        /// <returns>Returns String with Value of ReturnVar if Provided</returns>
        public static string Execute(string ahkSTRING, string ReturnVar = "OutputVar", bool NewSession = false)
        {
            _AHK ahk = new _AHK();
            return ahk.Execute(ahkSTRING, ReturnVar, NewSession);
        }

        /// <summary>Return the Contents of an AHK Variable Name</summary>
        /// <param name="VarName">Name of the Variable To Return from AHK</param>
        public static string GetVar(string VarName = "OutputVar")
        {
            _AHK ahk = new _AHK();
            return ahk.GetVar(VarName);
        }

        /// <summary>Programmatically set variable value in AHK Session</summary>
        /// <param name="VarName">Variable Name in AHK Session</param>
        /// <param name="VarValue">Value to Assign Variable Name</param>
        public static void SetVar(string VarName, string VarValue = "")
        {
            _AHK ahk = new _AHK();
            ahk.SetVar(VarName, VarValue);
        }

        /// <summary>Load AHK Script from a .AHK File Into Current or New AHK Session</summary>
        /// <param name="AHK_FilePath">AHK Code or FilePath as a String to Load in Current or New AHK Instance</param>
        /// <param name="NewAHKSession">Option to Reset AHK Instance, Clearing Previously Saved Hotkeys and Variable Values in Memory</param>
        /// <param name="AddReturn">Option to add Return in front of code added. Keeps from executing your script while loading.</param>
        /// <returns>False if File Not Found - True if File Found. (Does not indicate success in loading ahk functionality)</returns>
        public static bool Load_ahkFile(string AHK_FilePath, bool NewAHKSession = false, bool AddReturn = false)
        {
            _AHK ahk = new _AHK();
            return ahk.Load_ahkFile(AHK_FilePath, NewAHKSession, AddReturn);
        }

        /// <summary>Load AHK Script from a String containing AHK Functions Into Current or New AHK Session</summary>
        /// <param name="functionsAHK">AHK Script String containing AHK Commands</param>
        /// <param name="NewAHKSession">option to restart AHK session, clearing previously saved hotkeys and variable values in memory</param>
        /// <param name="AddReturn">Option to add Return in front of code added. Keeps from executing your script while loading.</param>
        public static void Load_ahkString(string functionsAHK, bool NewAHKSession = false, bool AddReturn = false)
        {
            _AHK ahk = new _AHK();
            ahk.Load_ahkString(functionsAHK, NewAHKSession, AddReturn);
        }

        /// <summary>
        /// Displays/Returns String with Whether AHK Session Has Been Initiated + # of AHK Files Loaded into Memory + AHK FilePaths Loaded
        /// </summary>
        public static string ahkSessionInfo(bool MsgBoxDisplay = false)
        {
            _AHK ahk = new _AHK();
            return ahk.ahkSessionInfo(MsgBoxDisplay);
        }

        /// <summary>Load All AHK Scripts in Directory Path into New or Current AHK Instance</summary>
        /// <param name="DirPath">Directory path of AHK files to load</param>
        /// <param name="Recurse">Option to search subdirectories underneath DirPath for AHK Files (default = false)</param>
        /// <param name="NewAHKSession">Option to Reset AHK Instance, Clearing Previously Saved Hotkeys and Variable Values in Memory</param>
        /// <param name="AddReturn">Option to add Return in front of code added. Keeps from executing your script while loading.</param>
        /// <returns>Returns list of .AHK file paths loaded into current AHK Session</returns>
        public static List<string> Load_ahkDir(string DirPath, bool Recurse = false, bool NewAHKSession = false, bool AddReturn = false)
        {
            _AHK ahk = new _AHK();
            return ahk.Load_ahkDir(DirPath, Recurse, NewAHKSession, AddReturn);
        }

        /// <summary>Execute AHK Function Previously Loaded in Current AHK Session</summary>
        /// <param name="FunctionName">Name of Function to Call</param>
        /// <param name="Param1">Parameter to pass into AHK function</param>
        /// <param name="Param2">Parameter to pass into AHK function</param>
        /// <param name="Param3">Parameter to pass into AHK function</param>
        /// <param name="Param4">Parameter to pass into AHK function</param>
        /// <param name="Param5">Parameter to pass into AHK function</param>
        /// <param name="Param6">Parameter to pass into AHK function</param>
        /// <param name="Param7">Parameter to pass into AHK function</param>
        /// <param name="Param8">Parameter to pass into AHK function</param>
        /// <param name="Param9">Parameter to pass into AHK function</param>
        /// <param name="Param10">Parameter to pass into AHK function</param>
        /// <returns>Returns value of function? Untested</returns>
        public static string Function(string FunctionName, string Param1 = null, string Param2 = null, string Param3 = null, string Param4 = null, string Param5 = null, string Param6 = null, string Param7 = null, string Param8 = null, string Param9 = null, string Param10 = null)  // launch AHK function and return value if there is one
        {
            _AHK ahk = new _AHK();
            return ahk.Function(FunctionName, Param1, Param2, Param3, Param4, Param5, Param6, Param7, Param8, Param9, Param10);
        }

        /// <summary>
        /// Checks to see if Function has been defined in AHK Session
        /// </summary>
        /// <param name="FunctionName">Name of Function To Check For</param>
        /// <returns></returns>
        public static bool FunctionExists(string FunctionName)
        {
            _AHK ahk = new _AHK();
            return ahk.FunctionExists(FunctionName);
        }

        /// <summary>Execute Label/GoSub Command Loaded in Current AHK Session</summary>
        /// <param name="GoSubName">AHK Script Label to Execute</param>
        /// <param name="CheckIfExistsFirst">Option to Confirm Label Exists in Memory Before Attempting To Execute</param>
        public static void GoSub(string GoSubName, bool CheckIfExistsFirst = false)
        {
            _AHK ahk = new _AHK();
            ahk.GoSub(GoSubName, CheckIfExistsFirst);
        }

        /// <summary>
        /// Checks to see if GoSub Label Exists in AHK Session
        /// </summary>
        /// <param name="GoSubLabel"></param>
        /// <returns></returns>
        public static bool LabelExists(string GoSubLabel)
        {
            _AHK ahk = new _AHK();
            return ahk.LabelExists(GoSubLabel);
        }


        /// <summary>Function Used to Flag Whether AHK Function Uses ErrorLog Value</summary>
        /// <param name="ErrorLogEnabled">Logs ErrorLevel Variables to Log if True, Otherwise Resets to Blank</param>
        /// <param name="ErrorLogText">ErrorMessage From Function To Log if Problem Detected</param>
        public static void ErrorLog_Setup(bool ErrorLogEnabled = true, string ErrorLogText = "")
        {
            _AHK ahk = new _AHK();
            ahk.ErrorLog_Setup(ErrorLogEnabled, ErrorLogText);
        }


        //public static ErrorLogEntry errorLog = new ErrorLogEntry();

        /// <summary>Used by AHK Functions to Log Last Script Command/Line Executed and Sets ErrorLevel Value If Detected</summary>
        /// <param name="ScriptLine">AHK/C# Command with ErrorLevelMessage</param>
        /// <param name="ErrorLevelMsg"></param>
        public static void ErrorLog(string ScriptLine, string ErrorLevelMsg = "")
        {
            _AHK ahk = new _AHK();
            ahk.ErrorLog(ScriptLine, ErrorLevelMsg);
        }



    }
}
