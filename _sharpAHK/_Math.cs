using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace sharpAHK
{
    public partial class _AHK
    {
        #region === Math ===

        ///// <summary>Sets a variable to the sum of itself plus the given value (can also add or subtract time from a date-time value). Synonymous with: var += value</summary>
        ///// <param name="Var">The variable upon which to operate.</param>
        ///// <param name="Value">Any integer, floating point number, or expression</param>
        ///// <param name="TimeUnits">If present, this parameter directs the command to add Value to Var, treating Var as a date-time stamp in the YYYYMMDDHH24MISS format and treating Value as the integer or floating point number of units to add (specify a negative number to perform subtraction). TimeUnits can be either Seconds, Minutes, Hours, or Days (or just the first letter of each of these). If Var is an empty variable, the current time will be used in its place. If Var contains an invalid timestamp or a year prior to 1601, or if Value is non-numeric, Var will be made blank to indicate the problem. The built-in variable A_Now contains the current local time in YYYYMMDDHH24MISS format. To calculate the amount of time between two timestamps, use EnvSub.</param>
        //public string EnvAdd(string Var, string Value, string TimeUnits = "")  // Not finished
        //{
        //    //string AHKLine = "EnvAdd, OutputVar, " + Value + "," + Count;  // ahk line to execute
        //    //ErrorLog_Setup(false); // ErrorLevel Detection Enabled for this function in AHK 
        //    //string OutVar = Execute(AHKLine, "OutputVar");   // execute AHK code and return variable value 
        //    //return OutVar;
        //    return ""; 
        //}

        ///// <summary>Sets a variable to itself divided by the given value. Synonymous with: var /= value</summary>
        ///// <param name="Var">The variable upon which to operate.</param>
        ///// <param name="Value">Any integer, floating point number, or expression.</param>
        //public string EnvDiv(string Var, string Value)  // Not finished
        //{
        //    //string AHKLine = "OutputVar, " + Value + "," + Count;  // ahk line to execute
        //    //ErrorLog_Setup(false); // ErrorLevel Detection Enabled for this function in AHK 
        //    //string OutVar = Execute(AHKLine, "OutputVar");   // execute AHK code and return variable value 
        //    //return OutVar;
        //    return "";
        //}

        ///// <summary>Sets a variable to itself minus the given value (can also compare date-time values). Synonymous with: var -= value</summary>
        ///// <param name="Var">The variable upon which to operate.</param>
        ///// <param name="Value">Any integer, floating point number, or expression.</param>
        ///// <param name="TimeUnits">If present, this parameter directs the command to subtract Value from Var as though both of them are date-time stamps in the YYYYMMDDHH24MISS format. TimeUnits can be either Seconds, Minutes, Hours, or Days (or just the first letter of each of these). If Value is blank, the current time will be used in its place. Similarly, if Var is an empty variable, the current time will be used in its place. The result is always rounded down to the nearest integer. For example, if the actual difference between two timestamps is 1.999 days, it will be reported as 1 day. If higher precision is needed, specify Seconds for TimeUnits and divide the result by 60.0, 3600.0, or 86400.0. If either Var or Value is an invalid timestamp or contains a year prior to 1601, Var will be made blank to indicate the problem. The built-in variable A_Now contains the current local time in YYYYMMDDHH24MISS format. To precisely determine the elapsed time between two events, use the A_TickCount method because it provides millisecond precision. To add or subtract a certain number of seconds, minutes, hours, or days from a timestamp, use EnvAdd (subtraction is achieved by adding a negative number).</param>
        //public string EnvSub(string Var, string Value, string TimeUnits = "")  // Not finished
        //{
        //    //string AHKLine = "OutputVar, " + Value + "," + Count;  // ahk line to execute
        //    //ErrorLog_Setup(false); // ErrorLevel Detection Enabled for this function in AHK 
        //    //string OutVar = Execute(AHKLine, "OutputVar");   // execute AHK code and return variable value 
        //    //return OutVar;
        //    return "";
        //}

        ///// <summary>Sets a variable to itself times the given value. Synonymous with: var *= value</summary>
        ///// <param name="Var">The variable upon which to operate.</param>
        ///// <param name="Value">Any integer, floating point number, or expression.</param>
        //public string EnvMult(string Var, string Value)  // Not finished
        //{
        //    //string AHKLine = "OutputVar, " + Value + "," + Count;  // ahk line to execute
        //    //ErrorLog_Setup(false); // ErrorLevel Detection Enabled for this function in AHK 
        //    //string OutVar = Execute(AHKLine, "OutputVar");   // execute AHK code and return variable value 
        //    //return OutVar;
        //    return "";
        //}

        /// <summary>Generates a pseudo-random number.</summary>
        /// <param name="Min">The smallest number that can be generated, which can be negative, floating point, or an expression. If omitted, the smallest number will be 0. The lowest allowed value is -2147483648 for integers, but floating point numbers have no restrictions.</param>
        /// <param name="Max">The largest number that can be generated, which can be negative, floating point, or an expression. If omitted, the largest number will be 2147483647 (which is also the largest allowed integer value -- but floating point numbers have no restrictions).</param>
        public string Random(string Min = "", string Max = "")
        {
            string AHKLine = "Random, OutputVar, " + Min + "," + Max;  // ahk line to execute
            ErrorLog_Setup(false); // ErrorLevel Detection Enabled for this function in AHK 
            string OutVar = Execute(AHKLine, "OutputVar");   // execute AHK code and return variable value 
            return OutVar;
        }

        /// <summary>Reseeding can improve the quality/security of generated random numbers - affects all subsequently generated random numbers.</summary>
        /// <param name="NewSeed">This mode reseeds the random number generator with NewSeed (which can be an expression). This affects all subsequently generated random numbers. NewSeed should be an integer between 0 and 4294967295 (0xFFFFFFFF). Reseeding can improve the quality/security of generated random numbers, especially when NewSeed is a genuine random number rather than one of lesser quality such as a pseudo-random number. Generally, reseeding does not need to be done more than once. If reseeding is never done by the script, the seed starts off as the low-order 32-bits of the 64-bit value that is the number of 100-nanosecond intervals since January 1, 1601. This value travels from 0 to 4294967295 every ~7.2 minutes.</param>
        public void Random_NewSeed(string NewSeed = "")
        {
            string AHKLine = "Random, , " + NewSeed;  // ahk line to execute
            ErrorLog_Setup(false); // ErrorLevel Detection Enabled for this function in AHK 
            Execute(AHKLine);   // execute AHK code and return variable value 
        }

        /// <summary>Sets the format of integers and floating point numbers generated by math operations.</summary>
        /// <param name="NumberType">Must be either IntegerFast, FloatFast, Integer, or Float</param>
        /// <param name="Format">For NumberType Integer or IntegerFast, specify H or HEX for hexadecimal, or D for decimal. Hexadecimal numbers all start with the prefix 0x (e.g. 0xFF).  For NumberType Float or FloatFast, specify TotalWidth.DecimalPlaces (e.g. 0.6). In v1.0.46.11+, the letter "e" may be appended to produce scientific notation; e.g. 0.6e or 0.6E (using uppercase produces an uppercase E in each number instead of lowercase). Note: In AutoHotkey 1.x, scientific notation must include a decimal point; e.g. 1.0e1 is valid but not 1e1. TotalWidth is typically 0 to indicate that number should not have any blank or zero padding. If a higher value is used, numbers will be padded with spaces or zeroes (see remarks) to make them that wide. DecimalPlaces is the number of decimal places to display (rounding will occur). If blank or zero, neither a decimal portion nor a decimal point will be displayed, that is, floating point results are displayed as integers rather than a floating point number. The starting default is 6. Padding: If TotalWidth is high enough to cause padding, spaces will be added on the left side; that is, each number will be right-justified. To use left-justification instead, precede TotalWidth with a minus sign. To pad with zeroes instead of spaces, precede TotalWidth with a zero (e.g. 06.2).</param>
        public void SetFormat(string NumberType, string Format)
        {
            string AHKLine = "SetFormat," + NumberType + "," + Format;  // ahk line to execute
            ErrorLog_Setup(false); // ErrorLevel Detection Enabled for this function in AHK 
            Execute(AHKLine);   // execute AHK code and return variable value
        }

        /// <summary>Performs miscellaneous math functions, bitwise operations, and tasks such as ASCII/Unicode conversion.</summary>
        /// <param name="Cmd">See AHK Documentation For Options</param>
        /// <param name="Value1">See AHK Documentation For Options</param>
        /// <param name="Value2">See AHK Documentation For Options</param>
        public string Transform(string Cmd, string Value1, string Value2 = "")
        {
            string AHKLine = "Transform, OutputVar, " + Cmd + "," + Value1 + "," + Value2;  // ahk line to execute
            ErrorLog_Setup(false); // ErrorLevel Detection Enabled for this function in AHK 
            string OutVar = Execute(AHKLine, "OutputVar");   // execute AHK code and return variable value 
            return OutVar;
        }

        /// <summary>
        /// Returns the Remainder Value when Dividing Value, ex: 10/3 = remainder of 1
        /// </summary>
        /// <param name="ValueToDivide">Whole Number to Divide</param>
        /// <param name="DivideBy">Value to Divide ValueToDivide by</param>
        /// <returns>Returns Remaining Values that won't define evenly</returns>
        public int Remainder(int ValueToDivide, int DivideBy)
        {
            return ValueToDivide % DivideBy;
        }


        #endregion
    }
}
