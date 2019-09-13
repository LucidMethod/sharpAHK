using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace sharpAHK
{
    public partial class _AHK
    {
        #region === Images ===

        /// <summary>Convert Image Path (png, ico, exe) / Icon / ImageList (By Key) Item / or Returns Image if Provided</summary>
        /// <param name="Image"> </param>
        /// <param name="KeyName"> </param>
        public Image ToImage(object Image, int width = 0, int height = 0, string KeyName = "")
        {
            string VarType = Image.GetType().ToString();  //determine what kind of variable was passed into function

            if (VarType == "System.String")  // Image File Path
            {
                if (!File.Exists((string)Image)) { return null; }

                // check the file extension of the file passed in 
                string fileExt = FileExt((string)Image, true);

                if (fileExt.ToUpper() == ".EXE") // extract default image from exe 
                {
                    Icon a = Icon.ExtractAssociatedIcon((string)Image);
                    Image returnImage = ToImage(a);  // convert icon to image
                    return returnImage;  // return image
                }

                // otherwise read image file path and convert to image and return
                Image fileImage = GetCopyImage((string)Image);

                // resize image if user provided w/h
                if (width != 0 && height != 0)
                {
                    fileImage = ResizeImage((string)Image, width, height);
                }

                return fileImage;
            }
            if (VarType == "System.Drawing.Icon")  // Icon
            {
                Icon ico = (Icon)Image;
                return ico.ToBitmap();
            }
            if (VarType == "System.Drawing.Bitmap")  // Image (returns same image unchanged)
            {
                return (Image)Image;
            }
            if (VarType == "System.Windows.Forms.ImageList")  // ImageList
            {
                return From_ImageList((ImageList)Image, KeyName);  // return image from image list by key
            }

            return null;
        }

        /// <summary>copy image before loading, frees file to delete</summary>
        /// <param name="path"> </param>
        public Image GetCopyImage(string path)
        {
            if (File.Exists(path))
            {
                if (isImage(path))
                {
                    try
                    {
                        using (Image im = Image.FromFile(path))
                        {
                            Bitmap bm = new Bitmap(im);
                            return bm;
                        }
                    }
                    catch { }

                }
            }

            //ahk.MsgBox(path + "\r\nNot Found - Unable to Load Image");

            return null;
        }

        /// <summary>search imagelist for key (file name) - returns image</summary>
        /// <param name="ImageList IL"> </param>
        /// <param name="KeyName"> </param>
        public Image From_ImageList(ImageList IL, string KeyName = "png_arrow-down-icon.png")
        {
            List<string> imageList = ImageList_FileNames(IL);

            foreach (string image in imageList)
            {
                string img = FileName(image);
                if (KeyName == img)
                {
                    return GetCopyImage(image);
                }

                img = FileNameNoExt(image);
                if (KeyName == img)
                {
                    return GetCopyImage(image);
                }

                if (image.Contains(KeyName))
                {
                    return GetCopyImage(image);
                }
            }

            return null;
        }


        /// <summary>resize image (untested)</summary>
        /// <param name="newWidth"> </param>
        /// <param name=" newHeight"> </param>
        /// <param name=" stPhotoPath"> </param>
        public Image ResizeImage(string ImagePath, int newWidth, int newHeight)
        {
            Image imgPhoto = Image.FromFile(ImagePath);

            int sourceWidth = imgPhoto.Width;
            int sourceHeight = imgPhoto.Height;

            //Consider vertical pics
            if (sourceWidth < sourceHeight)
            {
                int buff = newWidth;

                newWidth = newHeight;
                newHeight = buff;
            }

            int sourceX = 0, sourceY = 0, destX = 0, destY = 0;
            float nPercent = 0, nPercentW = 0, nPercentH = 0;

            nPercentW = ((float)newWidth / (float)sourceWidth);
            nPercentH = ((float)newHeight / (float)sourceHeight);
            if (nPercentH < nPercentW)
            {
                nPercent = nPercentH;
                destX = System.Convert.ToInt16((newWidth -
                          (sourceWidth * nPercent)) / 2);
            }
            else
            {
                nPercent = nPercentW;
                destY = System.Convert.ToInt16((newHeight -
                          (sourceHeight * nPercent)) / 2);
            }

            int destWidth = (int)(sourceWidth * nPercent);
            int destHeight = (int)(sourceHeight * nPercent);


            Bitmap bmPhoto = new Bitmap(newWidth, newHeight,
                          PixelFormat.Format24bppRgb);

            bmPhoto.SetResolution(imgPhoto.HorizontalResolution,
                         imgPhoto.VerticalResolution);

            Graphics grPhoto = Graphics.FromImage(bmPhoto);
            grPhoto.Clear(Color.Black);
            grPhoto.InterpolationMode =
                System.Drawing.Drawing2D.InterpolationMode.HighQualityBicubic;

            grPhoto.DrawImage(imgPhoto,
                new Rectangle(destX, destY, destWidth, destHeight),
                new Rectangle(sourceX, sourceY, sourceWidth, sourceHeight),
                GraphicsUnit.Pixel);

            grPhoto.Dispose();
            imgPhoto.Dispose();
            return bmPhoto;
        }

        /// <summary>return list of key names (also the filename without ext) in ImageList</summary>
        /// <param name="ImageList IL"> </param>
        /// <param name="FileNameOnly"> </param>
        /// <param name="FileExt"> </param>
        public List<string> ImageList_FileNames(ImageList IL, bool FileNameOnly = false, bool FileExt = false)
        {
            List<string> ImageListKeys = new List<string>();

            foreach (string img in IL.Images.Keys)
            {
                if (FileNameOnly)
                {
                    if (!FileExt)
                    {
                        string fileName = FileNameNoExt(img);  // file name with extension
                        ImageListKeys.Add(fileName);
                    }
                    if (FileExt)
                    {
                        string fileName = FileName(img);  // file name with extension
                        ImageListKeys.Add(fileName);
                    }
                }
                else
                {
                    ImageListKeys.Add(img);  // full file path 
                }
            }

            return ImageListKeys;
        }

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
