using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace sharpAHK
{
    public partial class _AHK
    {
        // === Monitors ===

        /// <summary>Retrieves screen resolution, multi-monitor info, dimensions of system objects, and other system properties.</summary>
        /// <param name="SubCommand">MonitorCount | MonitorPrimary | Monitor, N | MonitorWorkArea, N | (See Documentation For Numeric Options)</param>
        /// <param name="Param3">Additional Param for SysGet</param>
        public string SysGet(string SubCommand, string Param3 = "")
        {
            return ahkReturn("SysGet, OutputVar, " + SubCommand + "," + Param3);
        }

        /// <summary>Retrieves the total number of monitors. Unlike SM_CMONITORS mentioned in the table below, MonitorCount includes all monitors, even those not being used as part of the desktop. On Windows 95/NT the count is always 1.</summary>
        public int MonitorCount()
        {
            return ahkReturnInt("SysGet, OutputVar, MonitorCount");
        }

        /// <summary>Retrieves the number of the primary monitor, which will be 1 in a single-monitor system. On Windows 95/NT the primary monitor is always 1.</summary>
        public int MonitorPrimary()
        {
            return ahkReturnInt("SysGet, OutputVar, MonitorPrimary");
        }

        /// <summary>Retrieves the bounding coordinates of monitor number N (if MonitorNumber is omitted, the primary monitor is used). The information is stored in four variables whose names all start with OutputVar. If N is too high or there is a problem retrieving the info, the variables are all made blank. .</summary>
        /// <param name="MonitorNumber">If Omitted, the primary monitor is used</param>
        public string SysGet_Monitor(string MonitorNumber = "1")
        {
            string AHKLine = "SysGet, OutputVar, Monitor, " + MonitorNumber;  // ahk line to execute
            ErrorLog_Setup(false); // ErrorLevel Detection Disabled for this function in AHK 
            string OutVar = Execute(AHKLine, "OutputVar");   // execute AHK code and return variable value 

            string MonLeft = GetVar("OutputVarLeft");
            string MonRight = GetVar("OutputVarRight");
            string MonTop = GetVar("OutputVarTop");
            string MonBottom = GetVar("OutputVarBottom");

            return OutVar;
        }

        /// <summary>Retrieves the bounding coordinates of monitor number N (if N is omitted, the primary monitor is used). The information is stored in four variables whose names all start with OutputVar. If N is too high or there is a problem retrieving the info, the variables are all made blank. .</summary>
        /// <param name="MonitorNumber">If Omitted, the primary monitor is used</param>
        public string SysGet_MonitorName(string MonitorNumber = "1")
        {
            string AHKLine = "SysGet, OutputVar, MonitorName, " + MonitorNumber;  // ahk line to execute
            ErrorLog_Setup(false); // ErrorLevel Detection Disabled for this function in AHK 
            string OutVar = Execute(AHKLine, "OutputVar");   // execute AHK code and return variable value 
            return OutVar;
        }


    }
}
