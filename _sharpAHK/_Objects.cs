using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace sharpAHK
{
        //#region === OBJECTS ===


        public class StudentName
        {
            // The default constructor has no parameters. The default constructor 
            // is invoked in the processing of object initializers. 
            public StudentName() { }

            // The following constructor has parameters for two of the three 
            // properties. 
            public StudentName(string first, string last)
            {
                FirstName = first;
                LastName = last;
            }

            // Properties.
            public string FirstName { get; set; }
            public string LastName { get; set; }
            public int ID { get; set; }

            public override string ToString()
            {
                return FirstName + "  " + ID;
            }
        }

        /// <summary>Stores Global Variables / Session Info / Error Level / Logging Values</summary>
        public static class ahkGlobal
        {
            public static List<string> LoadedAHK { get; set; } // list of ahk files loaded from disk (file paths)

            public static AutoHotkey.Interop.AutoHotkeyEngine ahkdll { get; set; }  // stores current AHK session

            public static bool ErrorLevel { get; set; }         // true if error level detected in ahk command
            public static bool ErrorLevelEnabled { get; set; }  // true if error level information is available for this command
            public static string ErrorLevelValue { get; set; } // ErrorLevel variable value returned from AHK session
            public static string ErrorLevelMsg { get; set; }   // assembled error level message to display
            public static string ErrorLevelCustom { get; set; }  // custom error level text for a command

            public static string LastOutputVarName { get; set; }     // name of last AHK variable returned/set in AHK session
            public static string LastOutputVarValue { get; set; }     // value returned from OutputVar in AHK session
            public static string LastLine { get; set; }     // last line executed by AHK execute function
            public static string LastAction { get; set; }  // last function/ahk command used by ahk execute function

            public static string cSharpCmd { get; set; }  // sharpAHK command to recreate / log function

            public static bool Debug { get; set; } // 

            public static List<string> sharpAHKcmdHist { get; set; }  // command + variables in c# format - logging ability for playback
            public static List<ErrorLogEntry> ErrorLogHist { get; set; }  // command + variables in c# format - logging ability for playback


            public static bool GlobalDebugEnabled { get; set; }   // Display Debug/Diagnostic Values while Executing
            public static bool ErrorLevelOnGui { get; set; }  // option to display error level details on gui in real time
                                                              //public Control ErrorLevelDisp = new Control(); // control that displays .Text 



            public static bool MacroPlaying { get; set; }
        }

        public class ErrorLogEntry
        {
            public bool ErrorLevel { get; set; }         // true if error level detected in ahk command
            public bool ErrorLevelEnabled { get; set; }  // true if error level information is available for this command
            public string ErrorLevelValue { get; set; } // ErrorLevel variable value returned from AHK session
            public string ErrorLevelMsg { get; set; }   // assembled error level message to display
            public string ErrorLevelCustom { get; set; }  // custom error level text for a command

            public string LastOutputVarName { get; set; }     // name of last AHK variable returned/set in AHK session
            public string LastOutputVarValue { get; set; }     // value returned from OutputVar in AHK session
            public string LastLine { get; set; }     // last line executed by AHK execute function
            public string LastAction { get; set; }  // last function/ahk command used by ahk execute function

            public string cSharpCmd { get; set; }  // sharpAHK command to recreate / log function
        }

        /// <summary>Stores Mouse Coordinates (Relative to Screen or Window) and Info Gathered Under Mouse</summary>
        public class mousePos
        {
            // ex:
            // mousePos mp = ahk.MouseGetPos("3");  // populate object with values

            public int X_Window { get; set; } // stores x mouse coordinate relative to the active window  
            public int X_Screen { get; set; } // stores y mouse coordinate relative to the screen

            public int Y_Window { get; set; } // stores x mouse coordinate relative to the active window  
            public int Y_Screen { get; set; } // stores y mouse coordinate relative to the screen


            public string WinHwnd { get; set; }
            public string ControlClassNN { get; set; }
            public string ControlHwnd { get; set; }
        }

        /// <summary>Stores Window Coordinates and Additional Details Returned from AHK Functions</summary>
        public class winInfo
        {
            // ex:
            // winInfo win = new winInfo();   // define object reference
            /*
                    public string WinTitle { get; set; }
                    public string WinText { get; set; }
                    public string Hwnd { get; set; }
                    public string Class { get; set; }
                    public string PID { get; set; }
                    public string ProcessName { get; set; }

                    public string Count { get; set; }

                    public int WinX { get; set; }
                    public int WinY { get; set; }
                    public int WinW { get; set; }
                    public int WinH { get; set; }
            */

            public string WinTitle { get; set; }
            public string WinText { get; set; }
            public string WinID { get; set; }
            public string Class { get; set; }
            public string MinMax { get; set; }
            public bool IsMin { get; set; }
            public bool IsMax { get; set; }
            public string Count { get; set; }

            public int WinX { get; set; }
            public int WinY { get; set; }
            public int WinW { get; set; }
            public int WinH { get; set; }



            public string PID { get; set; }
            public string ProcessName { get; set; }


        }


        // #region === v1 Objects ===

        /// <summary>
        /// used to return window position
        /// </summary>
        public struct Rect
        {
            public int Left { get; set; }
            public int Top { get; set; }
            public int Right { get; set; }
            public int Bottom { get; set; }

            public int Width { get; set; }
            public int Height { get; set; }
        }

        /// <summary>
        /// used to return window position
        /// </summary>
        public struct Coordinates
        {
            public int XPos { get; set; }
            public int YPos { get; set; }

        }

        /// <summary>
        /// control info parameters
        /// </summary>
        public struct conInfo
        {
            public string ControlName { get; set; }
            public string ControlText { get; set; }
            public IntPtr ControlHwnd { get; set; }
            public string ControlClass { get; set; }
            public string ControlValue { get; set; }

            public int ControlX { get; set; }
            public int ControlY { get; set; }
            public int ControlW { get; set; }
            public int ControlH { get; set; }

            public bool ControlChecked { get; set; }
            public bool ControlEnabled { get; set; }
            public bool ControlVisible { get; set; }
        }

        /// <summary>
        /// window / mouse / control info parameters
        /// </summary>
        public struct wInfo
        {
            public int MouseXPos { get; set; }
            public int MouseYPos { get; set; }

            public string WinTitle { get; set; }
            public IntPtr WinHwnd { get; set; }
            public string WinClass { get; set; }
            public string WinPID { get; set; }
            public string WinText { get; set; }

            public int WinX { get; set; }
            public int WinY { get; set; }
            public int WinW { get; set; }
            public int WinH { get; set; }

            public IntPtr ControlHwnd { get; set; }

        }



}
