# sharpAHK

AutoHotkey for .Net 

Exeute Your Own AHK Functions (Using Functions Documented Below)

Or Use Library of Wrappers Included (Documentation Coming Soon - Example Here)
    _AHK ahk = new _AHK();
Ex: ahk.MsgBox("Hello AHK World"); 



// Execute AHK : 

       /// <summary>Executes AHK Command, Returns Variable Value if ReturnVar is Populated</summary>
        /// <param name="ahkSTRING">AHK Line / Collection of AHK Commands</param>
        /// <param name="ReturnVar">Variable name to return value of from AHK Session after executing ahkSTRING</param>
        /// <param name="NewSession">Option To Initiate New AHK Instance - Resets Previously Used Variables and Loaded AHK</param>
        /// <returns>Returns String with Value of ReturnVar if Provided</returns>
        public string Execute(string ahkSTRING, string ReturnVar = "OutputVar", bool NewSession = false) { return ""; }


        /// <summary>Return the Contents of an AHK Variable Name</summary>
        /// <param name="VarName">Name of the Variable To Return from AHK</param>
        public string GetVar(string VarName = "OutputVar") { return ""; }


        /// <summary>Programmatically set variable value in AHK Session</summary>
        /// <param name="VarName">Variable Name in AHK Session</param>
        /// <param name="VarValue">Value to Assign Variable Name</param>
        public void SetVar(string VarName, string VarValue = "") { }


        /// <summary>Load AHK Script from a .AHK File Into Current or New AHK Session</summary>
        /// <param name="AHK_FilePath">AHK Code or FilePath as a String to Load in Current or New AHK Instance</param>
        /// <param name="NewAHKSession">Option to Reset AHK Instance, Clearing Previously Saved Hotkeys and Variable Values in Memory</param>
        /// <param name="AddReturn">Option to add Return in front of code added. Keeps from executing your script while loading.</param>
        /// <returns>False if File Not Found - True if File Found. (Does not indicate success in loading ahk functionality)</returns>
        public bool Load_ahkFile(string AHK_FilePath, bool NewAHKSession = false, bool AddReturn = false) { return true; }


        /// <summary>Load AHK Script from a String containing AHK Functions Into Current or New AHK Session</summary>
        /// <param name="functionsAHK">AHK Script String containing AHK Commands</param>
        /// <param name="NewAHKSession">option to restart AHK session, clearing previously saved hotkeys and variable values in memory</param>
        /// <param name="AddReturn">Option to add Return in front of code added. Keeps from executing your script while loading.</param>
        public void Load_ahkString(string functionsAHK, bool NewAHKSession = false, bool AddReturn = false) { }


        /// <summary>
        /// List that stores .AHK Files Loaded Into AHK Instance via Load_ahkFile()
        /// </summary>
        public List<string> _loadedAhkFiles;


        /// <summary>Load All AHK Scripts in Directory Path into New or Current AHK Instance</summary>
        /// <param name="DirPath">Directory path of AHK files to load</param>
        /// <param name="Recurse">Option to recurse into subdirectories when searching for AHK files</param>
        /// <param name="NewAHKSession">Option to Reset AHK Instance, Clearing Previously Saved Hotkeys and Variable Values in Memory</param>
        /// <param name="AddReturn">Option to add Return in front of code added. Keeps from executing your script while loading.</param>
        /// <returns>Returns list of .AHK file paths loaded into current AHK Session</returns>
        public List<string> Load_ahkDir(string DirPath, bool Recurse = true, bool NewAHKSession = false, bool AddReturn = false) { return new List<string>(); }



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
        public string Function(string FunctionName, string Param1 = null, string Param2 = null, string Param3 = null, string Param4 = null, string Param5 = null, string Param6 = null, string Param7 = null, string Param8 = null, string Param9 = null, string Param10 = null) { return ""; } // launch AHK function and return value if there is one



        /// <summary>Execute Label/GoSub Command Loaded in Current AHK Session</summary>
        /// <param name="GoSubName">AHK Script Label to Execute</param>
        public void GoSub(string GoSubName) { }



        //// Global Variables setup to capture ErrorLevel Values from AHK
        //        ahkGlobal.ErrorLevelMsg = "";
        //        ahkGlobal.ErrorLevelValue = "0";
        //        ahkGlobal.ErrorLevel = false;
