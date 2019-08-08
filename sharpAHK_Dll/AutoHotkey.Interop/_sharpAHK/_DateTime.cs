using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace sharpAHK
{
    public partial class _AHK
    {
        // === DateTime ===

        /// <summary>Compare Date to DateToCompare, Returns either Earlier, Same, or Later as String</summary>
        /// <param name="Date">Date To Compare</param>
        /// <param name="DateToCompare">Date to Compare Against</param>
        /// <returns>Returns either Earlier/Same/Later as String</returns>
        public string DateTimeCompare(DateTime Date, DateTime DateToCompare)
        {
            //DateTime date1 = new DateTime(2009, 8, 1, 0, 0, 0);
            //DateTime date2 = new DateTime(2009, 8, 2, 0, 0, 0);

            int result = DateTime.Compare(Date, DateToCompare);

            if (result < 0)
                return "Earlier"; // 1 is before 2 
            else if (result == 0)
                return "Same"; // 1 = 2
            else
                return "Later"; // 1 is later than 2
        }

        /// <summary>Verify/Format dates before inserting into sql db</summary>
        /// <param name="InTime">DateTime String To Verify/Convert to DateTime</param>
        public DateTime ValidTime(string InTime)
        {
            bool Valid = IsValidSqlDatetime(InTime);

            if (Valid)
            {
                DateTime Converted = Convert.ToDateTime(InTime);
                return Converted;
            }

            //if (!Valid)
            DateTime PlaceHolderTime = new DateTime(1900, 1, 1);
            return PlaceHolderTime;
        }

        /// <summary>Checks to see if Date is a Valid SQL Date</summary>
        /// <param name="DateString">Date as string to Check</param>
        /// <returns>Returns True if Date is Valid in SQL</returns>
        public bool IsValidSqlDatetime(string DateString)
        {
            bool valid = false;
            DateTime testDate = DateTime.MinValue;
            DateTime minDateTime = DateTime.MaxValue;
            DateTime maxDateTime = DateTime.MinValue;

            minDateTime = new DateTime(1753, 1, 1);
            maxDateTime = new DateTime(9999, 12, 31, 23, 59, 59, 997);

            if (DateTime.TryParse(DateString, out testDate))
            {
                if (testDate >= minDateTime && testDate <= maxDateTime)
                {
                    valid = true;
                }
            }

            return valid;
        }



        // === Compare Time ===

        //bool beforeTime = BeforeCurrentTime("11:00:00 AM");
        //bool afterTime = AfterCurrentTime("11:00:00 AM");
        //ahk.MsgBox("Before Time = " + beforeTime.ToString());
        //ahk.MsgBox("After Time = " + afterTime.ToString()); 

        /// <summary>
        /// compares timestamp string to current time, returns true if timestamp is before current time
        /// </summary>
        /// <param name="TimeToCompare"></param>
        /// <returns></returns>
        public bool BeforeCurrentTime(string TimeToCompare = "11:00:00 AM")
        {
            DateTime t1 = DateTime.Now;
            DateTime t2 = Convert.ToDateTime(TimeToCompare);
            int i = DateTime.Compare(t1, t2);

            //if t1 is less than t2 then result is Less than zero
            //if t1 equals t2 then result is Zero
            //if t1 is greater than t2 then result isGreater zero

            if (i > 0) { return false; }
            if (i < 0) { return true; }
            return false;
        }

        /// <summary>
        /// compares timestamp string to current time, returns true if timestamp is after current time
        /// </summary>
        /// <param name="TimeToCompare"></param>
        /// <returns></returns>
        public bool AfterCurrentTime(string TimeToCompare = "11:00:00 AM")
        {
            DateTime t1 = DateTime.Now;
            DateTime t2 = Convert.ToDateTime("11:00:00 AM");
            int i = DateTime.Compare(t1, t2);

            //if t1 is less than t2 then result is Less than zero
            //if t1 equals t2 then result is Zero
            //if t1 is greater than t2 then result isGreater zero

            if (i > 0) { return true; }
            if (i < 0) { return false; }
            return false;
        }


        /// <summary>
        /// Returns difference between two dates (in Minutes)
        /// </summary>
        /// <param name="DateTime1">First Date/Time - Can Pass in String or DateTime Var</param>
        /// <param name="DateTime2">Date/Time to compare to first - Can Pass in String or DateTime Var</param>
        /// <returns>Returns the Number of Min Between DateTime1 and DateTime2</returns>
        public int TimeDiff_Min(object DateTime1, object DateTime2)
        {

            DateTime dateTime11 = ToDateTime(DateTime1.ToString());
            DateTime dateTime22 = ToDateTime(DateTime2.ToString());

            TimeSpan diff = dateTime11 - dateTime22;

            double totalMinutes = diff.TotalMinutes;
            int min = ToInt(totalMinutes);

            //string timeDiff = diff.ToString();
            return min;
        }


        // !!! finish 

        /// <summary>
        /// 
        /// </summary>
        /// <param name="date1"></param>
        /// <param name="date2"></param>
        public void Time_Difference(DateTime date1, DateTime date2)
        {
            //DateTime date1 = dateTimePicker1.Value;
            //DateTime date2 = dateTimePicker2.Value;

            TimeSpan difference = date2 - date1;
            string days = "Days: " + difference.TotalDays.ToString();
            string hours = "Hours: " + difference.TotalHours.ToString();
            string minutes = "Minutes: " + difference.TotalMinutes.ToString();
            string seconds = "Seconds: " + difference.TotalSeconds.ToString();
            string milliseconds = "Milliseconds: " + difference.TotalMilliseconds.ToString();

            // v1 ToDo
            /*
                    // difference between two dates (currently returns in minutes)
                    string timeDiff(object dateTime1, object dateTime2)
                    {

                        DateTime dateTime11 = ToDateTime(dateTime1.ToString());
                        DateTime dateTime22 = ToDateTime(dateTime2.ToString());

                        TimeSpan diff = dateTime11 - dateTime22;
                        //if (diff < 0)
                        //    {
                        //    diff = diff + TimeSpan.FromDays(1);
                        //    }

                        double totalMinutes = diff.TotalMinutes;
                        int min = ToInt(totalMinutes);

                        string timeDiff = diff.ToString();
                        return timeDiff;
                    }
            */

        }


        
        // === Convert Time Formats ===


        /// <summary>
        /// Convert Milliseconds into Minutes
        /// </summary>
        /// <param name="TimeMS">Millisecond Value to Convert</param>
        /// <returns>Returns Minutes as Int</returns>
        public int MsToMin(int TimeMS)
        {
            try
            {
                TimeSpan t = TimeSpan.FromMilliseconds(TimeMS);
                return (int)t.TotalMinutes;
            }
            catch (Exception ex)
            {
                //WriteNote("Unexpected error - " + ex.Message.ToString());
                return -1;
            }
        }

        /// <summary>
        /// Convert Milliseconds into Seconds
        /// </summary>
        /// <param name="TimeMS">Millisecond Value to Convert</param>
        /// <returns>Returns Seconds as Int</returns>
        public int MsToSeconds(int TimeMS)
        {
            try
            {
                TimeSpan t = TimeSpan.FromMilliseconds(TimeMS);
                return (int)t.TotalSeconds;
            }
            catch (Exception ex)
            {
                //WriteNote("Unexpected error - " + ex.Message.ToString());
                return -1;
            }
        }

        /// <summary>
        /// Convert Milliseconds into Hours
        /// </summary>
        /// <param name="TimeMS">Millisecond Value to Convert</param>
        /// <returns>Returns Hours as Int</returns>        
        public int MsToHours(int TimeMS)
        {
            try
            {
                TimeSpan t = TimeSpan.FromMilliseconds(TimeMS);
                return (int)t.TotalHours;
            }
            catch (Exception ex)
            {
                //WriteNote("Unexpected error - " + ex.Message.ToString());
                return -1;
            }
        }

        /// <summary>
        /// Convert Milliseconds into Days
        /// </summary>
        /// <param name="TimeMS">Millisecond Value to Convert</param>
        /// <returns>Returns Days as Int</returns>          
        public int MsToDays(int TimeMS)
        {
            try
            {
                TimeSpan t = TimeSpan.FromMilliseconds(TimeMS);
                return (int)t.TotalDays;
            }
            catch (Exception ex)
            {
                //WriteNote("Unexpected error - " + ex.Message.ToString());
                return -1;
            }
        }


        public static Stopwatch stopwatch = new Stopwatch();

        /// <summary>
        /// Start/Stop StopWatch Timer
        /// </summary>
        /// <param name="Start"></param>
        /// <returns></returns>
        public string StopWatch(bool Start = true)
        {
            if (Start) { stopwatch = new Stopwatch(); stopwatch.Start(); }
            else { stopwatch.Stop(); }
            return StringSplit(stopwatch.Elapsed.ToString(), ".", 0);
        }

    }
}
