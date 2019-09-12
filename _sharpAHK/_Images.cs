using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace sharpAHK
{
    public partial class _AHK
    {
        #region === Images ===

        public void sb(string Text, int pos = 1)
        {

        }

        public void Toggle_PictureBoxes(bool show = false) // hide pictureboxes while using the imagesearch
        {

        }

        #region === Image Search ===

        //; -Single Click  				Find_Click(Image, searchLoopCount = 10)
        //;	-Double Click 				Find_DoubleClick(Image, searchLoopCount = 10)
        //;	-Right Click 				Find_RightClick(Image, searchLoopCount = 10)
        //;	-Return Found Coordinates 	Find_Coordinates(Image, searchLoopCount = 10)

        public bool ImgSearch_Find_Click(string SearchImagePath, int SearchTime = 10, bool Debug = false, bool RightClick = false, bool DoubleClick = false)
        {
            sb("Starting Search For Image To Click");

            //create an autohotkey engine (AHK DLL) or use existing instance if it hasn't been initiated
            if (ahkGlobal.ahkdll == null) { New_AHKSession(); }
            var ahkdll = ahkGlobal.ahkdll;

            Toggle_PictureBoxes(false); // hide pictureboxes while using the imagesearch

            string ReturnMessage = "";


            if (RightClick == false && DoubleClick == false)  // default = single left click
            {
                ReturnMessage = ahkdll.ExecFunction("Find_Click", SearchImagePath, SearchTime.ToString());  // execute loaded function
            }
            if (RightClick)
            {
                ReturnMessage = ahkdll.ExecFunction("Find_RightClick", SearchImagePath, SearchTime.ToString());  // execute loaded function
            }
            if (DoubleClick)
            {
                ReturnMessage = ahkdll.ExecFunction("Find_DoubleClick", SearchImagePath, SearchTime.ToString());  // execute loaded function
            }


            if (Debug) { MsgBox(ReturnMessage); }

            if (!ReturnMessage.ToUpper().Contains("FALSE"))  // if return is successful - image found and clicked = true
            {
                sb("Found Search Image And Clicked It");
                return true;
            }

            if (ReturnMessage.ToUpper().Contains("FALSE"))  // unsuccessful
            {
                sb("Failed to Locate Search Image And Click It");
                return false;
            }

            return false; // image not found on screen
        }

        public void ImgSearch_Find_Click_Thread(string SearchImagePath, int SearchTime = 10, bool Debug = false)
        {
            // new thread with parameters in thread function
            Thread ImgSearchThread = new Thread(() => ImgSearch_Find_Click_ThreadAction(SearchImagePath, SearchTime, Debug));
            ImgSearchThread.Start(); // Start the thread
        }

        private void ImgSearch_Find_Click_ThreadAction(string SearchImagePath, int SearchTime = 10, bool Debug = false)  // launch new thread when searching for images - keeps gui from hanging during search
        {
            //("Starting New Thread To Locate Search Image To Click");

            //create an autohotkey engine (AHK DLL) or use existing instance if it hasn't been initiated
            if (ahkGlobal.ahkdll == null) { New_AHKSession(); }
            var ahkdll = ahkGlobal.ahkdll;

            Toggle_PictureBoxes(false); // hide pictureboxes while using the imagesearch

            string ReturnMessage = ahkdll.ExecFunction("Find_Click", SearchImagePath, SearchTime.ToString());  // execute loaded function

            if (Debug) { MsgBox(ReturnMessage); }

            bool Found = false;
            if (!ReturnMessage.ToUpper().Contains("FALSE")) { Found = true; }

            if (Found) { sb("IMAGE FOUND : Finished Looking For Search Image. Found/Clicked = " + Found.ToString()); }
            if (!Found) { sb("!IMAGE NOT FOUND! : Finished Looking For Search Image. Found/Clicked = " + Found.ToString()); }


            //if (!ReturnMessage.ToUpper().Contains("FALSE")) { return true; } // if return is successful - image found and clicked = true
            //return false; // image not found on screen
        }


        public bool ImgSearch_Find_DoubleClick(string SearchImagePath, int SearchTime = 10, bool Debug = false)
        {
            //create an autohotkey engine (AHK DLL) or use existing instance if it hasn't been initiated
            if (ahkGlobal.ahkdll == null) { New_AHKSession(); }
            var ahkdll = ahkGlobal.ahkdll;

            string ReturnValue = ahkdll.ExecFunction("Find_DoubleClick", SearchImagePath, SearchTime.ToString());  // execute loaded function

            if (Debug) { MsgBox(ReturnValue); }

            if (!ReturnValue.ToUpper().Contains("FALSE")) { return true; } // if return is successful - image found and clicked = true

            return false; // image not found on screen
        }

        public bool ImgSearch_Find_RightClick(string SearchImagePath, int SearchTime = 10, bool Debug = false)
        {
            //create an autohotkey engine (AHK DLL) or use existing instance if it hasn't been initiated
            if (ahkGlobal.ahkdll == null) { New_AHKSession(); }
            var ahkdll = ahkGlobal.ahkdll;

            string ReturnValue = ahkdll.ExecFunction("Find_RightClick", SearchImagePath, SearchTime.ToString());  // execute loaded function

            if (Debug) { MsgBox(ReturnValue); }

            if (!ReturnValue.ToUpper().Contains("FALSE")) { return true; } // if return is successful - image found and clicked = true

            return false; // image not found on screen
        }

        public bool ImgSearch_Find_Coordinates(string SearchImagePath, out int FoundXPos, out int FoundYPos, int SearchTime = 10, bool Debug = false)  // returns false if image isn't found, otherwise coordinates where search image was located on screen
        {
            //create an autohotkey engine (AHK DLL) or use existing instance if it hasn't been initiated
            if (ahkGlobal.ahkdll == null) { New_AHKSession(); }
            var ahkdll = ahkGlobal.ahkdll;

            string ReturnValue = ahkdll.ExecFunction("Find_Coordinates", SearchImagePath, SearchTime.ToString());  // execute loaded function

            FoundXPos = -1;
            FoundYPos = -1;

            if (ReturnValue.ToUpper().Contains("FALSE")) { return false; }

            // coordinates were returned - parse the x and y values out

            string[] values = ReturnValue.Split('|');
            int i = 0;
            foreach (string value in values)
            {
                if (i == 0) { FoundXPos = ToInt(value); }
                if (i == 1) { FoundYPos = ToInt(value); }
                i++;
            }

            if (Debug) { MsgBox(ReturnValue); }

            return true; // image found - out coordinates populated 
        }


        #endregion



        #endregion
    }
}
