using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace sharpAHK
{
    public partial class _AHK
    {
        #region === AHK_DLL ===


        // AHK Session

        /// <summary>Resets Global AHK Session - Causes next AHK call to create a new AHK session. Resets all Hotkeys/Functions/Variables in Memory</summary>
        public void New_AHKSession(bool NewInstance = false)
        {
            if (ahkGlobal.ahkdll == null || NewInstance == true) { ahkGlobal.ahkdll = new AutoHotkey.Interop.AutoHotkeyEngine(); }

            else { ahkGlobal.ahkdll = null; }  // option to start new AHK session (resets variables and previously loaded functions)

            ahkGlobal.LoadedAHK = new List<string>(); // reset loaded ahk list


            Log("ahk.New_AHKSession();");
        }

        /// <summary>Executes AHK Command, Returns Variable Value if ReturnVar is Populated</summary>
        /// <param name="ahkSTRING">AHK Line / Collection of AHK Commands</param>
        /// <param name="ReturnVar">Variable name to return value of from AHK Session after executing ahkSTRING</param>
        /// <param name="NewSession">Option To Initiate New AHK Instance - Resets Previously Used Variables and Loaded AHK</param>
        /// <returns>Returns String with Value of ReturnVar if Provided</returns>
        public string Execute(string ahkSTRING, string ReturnVar = "OutputVar", bool NewSession = false)
        {
            string ahkCmd = StringReplace(ahkSTRING, "%", "`%", "ALL"); // fix illegal ahk chars

            //string ahkCmd = ahkSTRING.Trim(); 

            //create an autohotkey engine/session
            if (ahkGlobal.ahkdll == null) { New_AHKSession(); }

            if (NewSession) { New_AHKSession(); } // option to force ahk session to start new instance

            var ahkdll = ahkGlobal.ahkdll;

            //execute any raw ahk code
            ahkdll.ExecRaw(ahkCmd);

            ErrorLog(ahkCmd);

            // If user provided a ReturnVar Name, Return Value from AHK session to user as string
            if (ReturnVar != "")
            {
                string returnValue = GetVar(ReturnVar);

                if (returnValue.Length < 15) { Log("string ReturnVar (" + returnValue + ") = ahk.Execute(" + ahkSTRING + ", " + ReturnVar + ", " + NewSession + ");"); }
                else { Log("string ReturnVar = ahk.Execute(" + ahkSTRING + ", " + ReturnVar + ", " + NewSession + ");"); }

                return returnValue;
            }

            Log("ahk.Execute(" + ahkSTRING + ", " + ReturnVar + ", " + NewSession + ");");
            return "";   // no variable to return
        }

        /// <summary>Return the Contents of an AHK Variable Name</summary>
        /// <param name="VarName">Name of the Variable To Return from AHK</param>
        public string GetVar(string VarName = "OutputVar")
        {
            //create an autohotkey engine.
            if (ahkGlobal.ahkdll == null) { New_AHKSession(); }
            var ahkdll = ahkGlobal.ahkdll;

            //return value from ahk session
            string OutVar = ahkdll.GetVar(VarName);


            ahkGlobal.LastOutputVarName = VarName;
            ahkGlobal.LastOutputVarValue = OutVar;
            if (OutVar.Length < 15) { Log("string ReturnVar (" + OutVar + ") = ahk.GetVar(\"" + VarName + "\");"); }
            else { Log("string ReturnVar = ahk.GetVar(\"" + VarName + "\");"); }


            return OutVar;
        }

        /// <summary>Programmatically set variable value in AHK Session</summary>
        /// <param name="VarName">Variable Name in AHK Session</param>
        /// <param name="VarValue">Value to Assign Variable Name</param>
        public void SetVar(string VarName, string VarValue = "")
        {
            //create an autohotkey engine.
            if (ahkGlobal.ahkdll == null) { New_AHKSession(); }
            var ahkdll = ahkGlobal.ahkdll;

            ahkdll.SetVar(VarName, VarValue);
            Log("ahk.SetVar(" + VarName + ", " + VarValue + ");");
        }

        /// <summary>Load AHK Script from a .AHK File Into Current or New AHK Session</summary>
        /// <param name="AHK_FilePath">AHK Code or FilePath as a String to Load in Current or New AHK Instance</param>
        /// <param name="NewAHKSession">Option to Reset AHK Instance, Clearing Previously Saved Hotkeys and Variable Values in Memory</param>
        /// <param name="AddReturn">Option to add Return in front of code added. Keeps from executing your script while loading.</param>
        /// <returns>False if File Not Found - True if File Found. (Does not indicate success in loading ahk functionality)</returns>
        public bool Load_ahkFile(string AHK_FilePath, bool NewAHKSession = false, bool AddReturn = false)
        {
            //if (NewAHKSession) { New_AHKSession(); }  // option to start new AHK session (resets variables and previously loaded functions)

            // Create new AHK session if one hasn't been started
            if (ahkGlobal.ahkdll == null || NewAHKSession) { New_AHKSession(); }
            var ahkdll = ahkGlobal.ahkdll;

            // load the ahk function file to access in user scripts
            if (File.Exists(AHK_FilePath))
            {
                try
                {
                    ahkdll.LoadFile(AHK_FilePath, AddReturn);
                    ahkGlobal.LoadedAHK.Add(AHK_FilePath + " | AddReturn = " + AddReturn);
                    Log("ahk.Load_ahkFile(" + AHK_FilePath + ", " + NewAHKSession + ", " + AddReturn + ");");
                }
                catch  // error occured in AHK DLL while attempting to load
                {
                    return false;
                }

                return true;
            }
            else
                return false;
        }

        /// <summary>Load AHK Script from a String containing AHK Functions Into Current or New AHK Session</summary>
        /// <param name="functionsAHK">AHK Script String containing AHK Commands</param>
        /// <param name="NewAHKSession">option to restart AHK session, clearing previously saved hotkeys and variable values in memory</param>
        /// <param name="AddReturn">Option to add Return in front of code added. Keeps from executing your script while loading.</param>
        public void Load_ahkString(string functionsAHK, bool NewAHKSession = false, bool AddReturn = false)
        {
            if (NewAHKSession) { ahkGlobal.ahkdll = null; }  // option to start new AHK session (resets variables and previously loaded functions)

            // Create new AHK session if one hasn't been started
            if (ahkGlobal.ahkdll == null) { New_AHKSession(); }
            var ahkdll = ahkGlobal.ahkdll;

            string script = functionsAHK;
            if (AddReturn) { script = "return\n\n" + functionsAHK; }  // option to add return in front of code added to keep from executing on startup
            ahkdll.LoadScript(script);
            Log("ahk.Load_ahkString(" + functionsAHK + ", " + NewAHKSession + ", " + AddReturn + ");");
        }

        /// <summary>
        /// Displays/Returns String with Whether AHK Session Has Been Initiated + # of AHK Files Loaded into Memory + AHK FilePaths Loaded
        /// </summary>
        public string ahkSessionInfo(bool MsgBoxDisplay = false)
        {
            bool sessionStarted = false;
            if (ahkGlobal.ahkdll != null) { sessionStarted = true; }
            string info = "AHK Session Started = " + sessionStarted.ToString();

            //List<string> loaded = _loadedAhkFiles;

            int loadedAHKCount = 0;
            if (ahkGlobal.LoadedAHK != null) { loadedAHKCount = ahkGlobal.LoadedAHK.Count; }
            info = info + "\nLoaded AHK FileCount= " + loadedAHKCount.ToString();

            if (loadedAHKCount > 0)
            {
                foreach (string file in ahkGlobal.LoadedAHK)
                {
                    info = info + "\nLoaded AHK File= " + file;
                }
            }

            // skip display 
            //if (ahkGlobal.LastAction == "IfMsgBox") { return info; }

            if (MsgBoxDisplay)
                MessageBox.Show(info);


            return info;
        }

        /// <summary>Load All AHK Scripts in Directory Path into New or Current AHK Instance</summary>
        /// <param name="DirPath">Directory path of AHK files to load</param>
        /// <param name="Recurse">Option to search subdirectories underneath DirPath for AHK Files (default = false)</param>
        /// <param name="NewAHKSession">Option to Reset AHK Instance, Clearing Previously Saved Hotkeys and Variable Values in Memory</param>
        /// <param name="AddReturn">Option to add Return in front of code added. Keeps from executing your script while loading.</param>
        /// <returns>Returns list of .AHK file paths loaded into current AHK Session</returns>
        public List<string> Load_ahkDir(string DirPath, bool Recurse = false, bool NewAHKSession = false, bool AddReturn = false)
        {
            List<string> LoadedScripts = new List<string>();

            // nothing to load if directory not found - return null list
            if (!Directory.Exists(DirPath)) { return LoadedScripts; }

            // search through dir path for all .ahk files, user option to recurse into subdirectories
            string SearchPattern = "*.ahk";
            string[] files = null;
            if (Recurse) { files = Directory.GetFiles(DirPath, SearchPattern, System.IO.SearchOption.AllDirectories); }
            if (!Recurse) { files = Directory.GetFiles(DirPath, SearchPattern, System.IO.SearchOption.TopDirectoryOnly); }

            // option to start new AHK session (resets variables and previously loaded functions in memory)
            if (NewAHKSession) { New_AHKSession(); }

            foreach (string file in files)  // loop through list of files and write file details to sqlite db
            {
                if (DirName(file) == "Disabled") { continue; } // skip importing files in "Disabled" folder

                Load_ahkFile(file, false, AddReturn);  // load ahk file into session
            }

            Log("Added " + files.Count() + " Files | ahk.Load_ahkDir(" + DirPath + ", " + Recurse + ", " + NewAHKSession + "," + AddReturn + ");");

            //MsgBox("Loaded " + LoadedScripts.Count.ToString() + " AHK Scripts From\n\r" + DirPath);

            //if (LoadedScripts != null)  // store loaded ahk list in global var
            //{
            //    foreach(string filep in LoadedScripts)
            //        ahkGlobal.LoadedAHK.Add(filep); 
            //}


            return LoadedScripts;
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
        public string Function(string FunctionName, string Param1 = null, string Param2 = null, string Param3 = null, string Param4 = null, string Param5 = null, string Param6 = null, string Param7 = null, string Param8 = null, string Param9 = null, string Param10 = null)  // launch AHK function and return value if there is one
        {
            //create an autohotkey engine (AHK DLL) or use existing instance if it hasn't been initiated
            if (ahkGlobal.ahkdll == null) { New_AHKSession(); }
            var ahkdll = ahkGlobal.ahkdll;

            string ReturnValue = ahkdll.ExecFunction(FunctionName, Param1, Param2, Param3, Param4, Param5, Param6, Param7, Param8, Param9, Param10);  // execute loaded function

            Log("ahk.Function(" + FunctionName + ", " + Param1 + ", " + Param2 + "," + Param3 + "," + Param4 + "," + Param5 + "," + Param6 + "," + Param7 + "," + Param8 + "," + Param9 + "," + Param10 + ");");

            return ReturnValue;
        }

        /// <summary>
        /// Checks to see if Function has been defined in AHK Session
        /// </summary>
        /// <param name="FunctionName">Name of Function To Check For</param>
        /// <returns></returns>
        public bool FunctionExists(string FunctionName)
        {
            //create an autohotkey engine (AHK DLL) or use existing instance if it hasn't been initiated
            if (ahkGlobal.ahkdll == null) { New_AHKSession(); }
            var ahkdll = ahkGlobal.ahkdll;

            bool exists = ahkdll.FunctionExists(FunctionName);

            Log("bool Exists (" + exists + ") = ahk.FunctionExists(" + FunctionName + ");");

            return exists;
        }

        /// <summary>Execute Label/GoSub Command Loaded in Current AHK Session</summary>
        /// <param name="GoSubName">AHK Script Label to Execute</param>
        /// <param name="CheckIfExistsFirst">Option to Confirm Label Exists in Memory Before Attempting To Execute</param>
        public void GoSub(string GoSubName, bool CheckIfExistsFirst = false)
        {
            // Create new AHK session if one hasn't been started
            if (ahkGlobal.ahkdll == null) { New_AHKSession(); }
            var ahkdll = ahkGlobal.ahkdll;

            if (CheckIfExistsFirst)
            {
                bool Found = LabelExists(GoSubName);
                if (!Found) { return; }
            }

            Log("ahk.GoSub(" + GoSubName + ");");
            ahkdll.ExecLabel(GoSubName);  //execute a label
        }

        /// <summary>
        /// Checks to see if GoSub Label Exists in AHK Session
        /// </summary>
        /// <param name="GoSubLabel"></param>
        /// <returns></returns>
        public bool LabelExists(string GoSubLabel)
        {
            // Create new AHK session if one hasn't been started
            if (ahkGlobal.ahkdll == null) { New_AHKSession(); }
            var ahkdll = ahkGlobal.ahkdll;

            bool exists = ahkdll.LabelExists(GoSubLabel);  //execute a label

            if (exists) { Log("FOUND | bool labelExists = ahk.GoSubExists(" + GoSubLabel + ");"); }
            else { Log("NOT FOUND | bool labelExists = ahk.GoSubExists(" + GoSubLabel + ");"); }

            return exists;
        }


        /// <summary>Function Used to Flag Whether AHK Function Uses ErrorLog Value</summary>
        /// <param name="ErrorLogEnabled">Logs ErrorLevel Variables to Log if True, Otherwise Resets to Blank</param>
        /// <param name="ErrorLogText">ErrorMessage From Function To Log if Problem Detected</param>
        public void ErrorLog_Setup(bool ErrorLogEnabled = true, string ErrorLogText = "")
        {
            ahkGlobal.ErrorLevelEnabled = ErrorLogEnabled; // store user value in global var to access from ErrorLog()
            ahkGlobal.ErrorLevelCustom = ErrorLogText;
        }


        ErrorLogEntry errorLog = new ErrorLogEntry();

        /// <summary>Used by AHK Functions to Log Last Script Command/Line Executed and Sets ErrorLevel Value If Detected</summary>
        /// <param name="ScriptLine">AHK/C# Command with ErrorLevelMessage</param>
        /// <param name="ErrorLevelMsg"></param>
        public void ErrorLog(string ScriptLine, string ErrorLevelMsg = "")
        {
            errorLog = new ErrorLogEntry();

            //create an autohotkey engine.
            if (ahkGlobal.ahkdll == null) { New_AHKSession(); }
            var ahkdll = ahkGlobal.ahkdll;

            string Command = "";

            // extract the first word / command from the Script Line
            if (ScriptLine.Contains(","))
            {
                string[] words = ScriptLine.Split(',');
                foreach (string word in words) { Command = word; break; }  // return the first word before the ","
            }
            if (!ScriptLine.Contains(",")) // if there isn't a "," in the line submitted - split by spaces
            {
                if (ScriptLine.Contains(" "))
                {
                    string[] words = ScriptLine.Split(' ');
                    foreach (string word in words) { Command = word; break; }  // return the first word
                }
                if (!ScriptLine.Contains(" "))
                {
                    Command = ScriptLine.Trim();
                }
            }

            ahkGlobal.LastAction = Command;  // store the last command as a global variable to return after command completes
            ahkGlobal.LastLine = ScriptLine;  // store the AHK script line currently being run

            errorLog.LastAction = ahkGlobal.LastAction;
            errorLog.LastLine = ahkGlobal.LastLine;

            // reset global vars if error level not enabled for current command
            if (!ahkGlobal.ErrorLevelEnabled)
            {
                ahkGlobal.ErrorLevelMsg = "";
                ahkGlobal.ErrorLevelValue = "0";
                ahkGlobal.ErrorLevel = false;
                ahkGlobal.ErrorLevelEnabled = false;
                ahkGlobal.LastOutputVarName = "";
                ahkGlobal.LastOutputVarValue = "";


                errorLog.ErrorLevelMsg = ahkGlobal.ErrorLevelMsg;
                errorLog.ErrorLevelValue = ahkGlobal.ErrorLevelValue;
                errorLog.ErrorLevel = ahkGlobal.ErrorLevel;
                errorLog.ErrorLevelEnabled = ahkGlobal.ErrorLevelEnabled;
                errorLog.LastOutputVarName = ahkGlobal.LastOutputVarName;
                errorLog.LastOutputVarValue = ahkGlobal.LastOutputVarValue;
                return;
            }

            if (ahkGlobal.ErrorLevelEnabled)
            {
                ahkGlobal.ErrorLevelValue = GetVar("ErrorLevel");

                // set global bool if error level value detected
                if (ahkGlobal.ErrorLevelValue == "0") { ahkGlobal.ErrorLevel = false; } // no error detected - set bool to false
                if (ahkGlobal.ErrorLevelValue != "0") { ahkGlobal.ErrorLevel = true; }

                if (ErrorLevelMsg != "")
                {
                    if (ErrorLevelMsg.Contains("[ELV]"))  // replace [ELV] Macro with Actual Error Level Value 
                    {
                        ErrorLevelMsg = StringReplace(ErrorLevelMsg, "[ELV]", ahkGlobal.ErrorLevelValue);
                    }

                    ErrorLevelMsg = "[" + Command + "] | Error Level Detected: | " + ahkGlobal.ErrorLevelValue + " | " + ErrorLevelMsg;
                }
                if (ErrorLevelMsg == "")
                {
                    ErrorLevelMsg = "[" + Command + "] | Error Level Detected: | " + ahkGlobal.ErrorLevelValue;
                }

                if (ahkGlobal.ErrorLevelValue == "0")
                {
                    ErrorLevelMsg = "[" + Command + "] | Completed Successfully";
                }

                ahkGlobal.ErrorLevelMsg = ErrorLevelMsg;  // store the message for this error to pass on to user

                errorLog.ErrorLevelMsg = ahkGlobal.ErrorLevelMsg;
                errorLog.ErrorLevelValue = ahkGlobal.ErrorLevelValue;
                errorLog.ErrorLevel = ahkGlobal.ErrorLevel;
                errorLog.ErrorLevelEnabled = ahkGlobal.ErrorLevelEnabled;
                errorLog.LastOutputVarName = ahkGlobal.LastOutputVarName;
                errorLog.LastOutputVarValue = ahkGlobal.LastOutputVarValue;

            }

        }

        /// <summary>
        /// Examples using AHK InterOp Dll to Call AutoHotkey Commands
        /// </summary>
        private void Dev_Notes()
        {
            //if (ahkGlobal.ahkdll == null) { ahkGlobal.ahkdll = new AutoHotkey.Interop.AutoHotkeyEngine(); } var ahkdll = ahkGlobal.ahkdll;
            //ahkdll.ExecRaw("clipval := clipboard");
            //string clipboard = ahkdll.GetVar("clipval");

            ////execute any raw ahk code
            //ahkdll.ExecRaw("MsgBox, Hello World!");

            ////create new hotkeys
            ////ahk.ExecRaw("^a::Send, Hello World");
            //ahkdll.ExecRaw("F12::Msgbox Hotkey Pressed - %A_ThisHotkey%");

            ////programmatically set variables
            //ahkdll.SetVar("x", "1");
            //ahkdll.SetVar("y", "4");

            ////execute statements
            //ahkdll.ExecRaw("z:=x+y");

            ////return variables back from ahk
            //string zValue = ahkdll.GetVar("z");
            //Console.WriteLine("Value of z is {0}", zValue); // "Value of z is 5"

            ////Load a library or exec scripts in a file
            //ahkdll.Load("functions.ahk");

            ////execute a specific function (found in functions.ahk), with 2 parameters
            //ahkdll.ExecFunction("MyFunction", "Hello", "World");

            ////execute a label 
            //ahkdll.ExecLabel("DOSTUFF");

            ////create a new function
            //string sayHelloFunction = "SayHello(name) \r\n { \r\n MsgBox, Hello %name% \r\n return \r\n }";
            //ahkdll.ExecRaw(sayHelloFunction);

            ////execute's newly made function\
            //ahkdll.ExecRaw(@"SayHello(""Mario"") ");


            ////execute a function (in functions.ahk) that adds 5 and return results
            //var add5Results = ahkdll.Eval("Add5( 5 )");
            //Console.WriteLine("Eval: Result of 5 with Add5 func is {0}", add5Results);

            ////you can also return results with the ExecFunction 
            //add5Results = ahkdll.ExecFunction("Add5", "5");
            //Console.WriteLine("ExecFunction: Result of 5 with Add5 func is {0}", add5Results);
        }


        #endregion

        #region === Log / Debug ===

        /// <summary>
        /// Error Level Message / Last Executed Commands / Debug Functions
        /// </summary>
        public void debug(bool DebugDisplay = false)
        {
            bool ErrorLevel = ahkGlobal.ErrorLevel;  // true if error level detected in ahk command
            bool ErrorLevelEnabled = ahkGlobal.ErrorLevelEnabled;  // true if error level information is available for this command
            string ErrorLevelValue = ahkGlobal.ErrorLevelValue; // ErrorLevel variable value returned from AHK session
            string ErrorLevelMsg = ahkGlobal.ErrorLevelMsg;   // assembled error level message to display
            string ErrorLevelCustom = ahkGlobal.ErrorLevelCustom;  // custom error level text for a command


            string LastOutputVarName = ahkGlobal.LastOutputVarName;
            string LastOutputVarValue = ahkGlobal.LastOutputVarValue;

            string lastCShpar = ahkGlobal.cSharpCmd;
            string LastLine = ahkGlobal.LastLine;
            string LastAction = ahkGlobal.LastAction;

            string sessionInfo = ahkSessionInfo(false);

            string errorLogH = errorLogHist(true);
            string cmdH = cmdHist(true);

            if (DebugDisplay)
                MessageBox.Show("LastFunction = " + lastCShpar + "\nLastAction= " + LastAction + "\nErrorLevel= " + ErrorLevel.ToString() + "\nErrorMsg= " + ErrorLevelMsg + "\nValue= " + ErrorLevelValue + "\n\nLastLine= " + LastLine);
        }

        public void Log(object entry)
        {
            ahkGlobal.cSharpCmd = entry.ToString();

            if (ahkGlobal.sharpAHKcmdHist == null) { ahkGlobal.sharpAHKcmdHist = new List<string>(); }

            if (ahkGlobal.cSharpCmd != "") { ahkGlobal.sharpAHKcmdHist.Add(ahkGlobal.cSharpCmd); }

            if (ahkGlobal.ErrorLogHist == null) { ahkGlobal.ErrorLogHist = new List<ErrorLogEntry>(); }
            ahkGlobal.ErrorLogHist.Add(errorLog);
        }

        // Log / Hist Display|Return

        public string cmdHist(bool Display = false) // sharpAHK command history
        {
            string outView = "sharpAHKcmdHist\n\n";

            foreach (string act in ahkGlobal.sharpAHKcmdHist)
            {
                outView = outView + "\n" + act;
            }

            if (Display)
                MessageBox.Show(outView);

            return outView;
        }

        public string errorLogHist(bool Display = false)
        {
            if (ahkGlobal.ErrorLogHist == null) { ahkGlobal.ErrorLogHist = new List<ErrorLogEntry>(); }

            string outPut = "ErrorLogHist\n";
            if (ahkGlobal.ErrorLogHist.Count > 0)
            {
                int i = 0;
                foreach (ErrorLogEntry entry in ahkGlobal.ErrorLogHist)
                {
                    i++;
                    string msg = "ErrorLog Entry " + i + "\n";

                    msg = msg + entry.LastAction + "\n";


                    outPut = outPut + "\n" + msg;

                    if (Display)
                        MessageBox.Show(msg);
                }
            }

            return outPut;
        }

        public string loadedAHKList(bool Display = false)  // displays list of loaded ahk file in current session
        {
            if (ahkGlobal.LoadedAHK == null) { ahkGlobal.LoadedAHK = new List<string>(); }

            string outList = "";
            foreach (string file in ahkGlobal.LoadedAHK)
            {
                outList = outList + "\n" + file;
            }

            if (Display) { MessageBox.Show(outList); }

            return outList;
        }

        #endregion

        #region === Flow of Control ===

        /// <summary>Pauses the script's current thread.</summary>
        /// <param name="OnOffToggle">If blank or omitted, it defaults to Toggle. Otherwise, specify one of the following words: On: Pauses the current thread. | Off: If the thread beneath the current thread is paused, it will be in an unpaused state when resumed. Otherwise, the command has no effect.</param>
        /// <param name="OperateOnUnderlyingThread">This parameter is ignored for "Pause Off" because that always operates on the underlying thread. For the others, it is ignored unless Pause is being turned on (including via Toggle). False (Default): The command pauses the current thread; that is, the one now running the Pause command. | True: The command marks the thread beneath the current thread as paused so that when it resumes, it will finish the command it was running (if any) and then enter a paused state. If there is no thread beneath the current thread, the script itself is paused, which prevents timers from running (this effect is the same as having used the menu item "Pause Script" while the script has no threads).</param>
        public void Pause(string OnOffToggle = "Toggle", bool OperateOnUnderlyingThread = false)
        {
            // change overwrite bool to 1/0 
            int operate = 0; if (OperateOnUnderlyingThread) { operate = 1; }

            string AHKLine = "Pause, " + OnOffToggle + "," + operate.ToString();  // ahk line to execute
            ErrorLog_Setup(false); // ErrorLevel Detection Enabled for this function in AHK 
            Execute(AHKLine);   // execute AHK code and return variable value 
        }

        /// <summary>Determines how fast a script will run (affects CPU utilization).</summary>
        /// <param name="LineCount">The number of script lines to execute prior to sleeping for 10ms. The value can be as high as 9223372036854775807. Also, this mode is mutually exclusive of the 20ms mode in the previous paragraph; that is, only one of them can be in effect at a time.</param>        
        public void SetBatchLines(string LineCount)
        {
            string AHKLine = "SetBatchLines, " + LineCount;  // ahk line to execute
            ErrorLog_Setup(false); // ErrorLevel Detection Enabled for this function in AHK 
            Execute(AHKLine);   // execute AHK code and return variable value 
        }

        /// <summary>Causes a subroutine to be launched automatically and repeatedly at a specified time interval.</summary>
        /// <param name="Label">The name of the label or hotkey label to which to jump, which causes the commands beneath Label to be executed until a Return or Exit is encountered. As with the parameters of almost all other commands, Label can be a variable reference such as %MyLabel%, in which case the name stored in the variable is used as the target.</param>
        /// <param name="PeriodOnOff">On: Re-enables a previously disabled timer at its former period. If the timer doesn't exist, it is created (with a default period of 250). If the timer exists but was previously set to run-only-once mode, it will again run only once. | Off: Disables an existing timer. | Period: Creates or updates a timer using this parameter as the approximate number of milliseconds that must pass since the last time the Label subroutine was started. | Run only once: Specify a negative Period to indicate that the timer should run only once. </param>
        /// <param name="Priority">This optional parameter is an integer between -2147483648 and 2147483647 (or an expression) to indicate this timer's thread priority. If omitted, 0 will be used.</param>
        public void SetTimer(string Label, string PeriodOnOff = "On", string Priority = "0")
        {
            string AHKLine = "SetTimer, " + Label + "," + PeriodOnOff + "," + Priority;  // ahk line to execute
            ErrorLog_Setup(false); // ErrorLevel Detection Enabled for this function in AHK 
            Execute(AHKLine);   // execute AHK code and return variable value 
        }

        /// <summary>Waits the specified amount of time before continuing.</summary>
        /// <param name="DelayInMilliseconds">The amount of time to pause (in milliseconds) between 0 and 2147483647 (24 days), which can be an expression.</param>        
        public void Sleep(object DelayInMilliseconds)
        {
            string AHKLine = "Sleep, " + DelayInMilliseconds.ToString();  // ahk line to execute
            ErrorLog_Setup(false); // ErrorLevel Detection Enabled for this function in AHK 
            Execute(AHKLine);   // execute AHK code and return variable value 

            // v1 ToDo
            /*
                    _Sleep(int SleepTime)  // sleeps / idles for x seconds before continuing
                    {
                        Thread.Sleep(SleepTime);
                    }
            */

        }

        /// <summary>Disables or enables all or selected hotkeys and hotstrings.</summary>
        /// <param name="OnOffTogglePermit">On: Suspends all hotkeys and hotstrings except those explained the Remarks section. | Off: Re-enables the hotkeys and hotstrings that were disable above. | Toggle (default): Changes to the opposite of its previous state (On or Off). | Permit: Does nothing except mark the current subroutine as being exempt from suspension. </param>
        public void Suspend(string OnOffTogglePermit = "Toggle")
        {
            string AHKLine = "Suspend, " + OnOffTogglePermit;  // ahk line to execute
            ErrorLog_Setup(false); // ErrorLevel Detection Enabled for this function in AHK 
            Execute(AHKLine);   // execute AHK code and return variable value 
        }

        #endregion

    }
}
