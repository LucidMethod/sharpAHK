using sharpAHK;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace AHKExpressions
{
    public static partial class AHKExpressions
    {
        // === Images ===

        /// <summary>Convert Image Path To Icon</summary>
        /// <param name="ImagePath"> </param>
        /// <param name="ICOSize">Size of Icon to Create (default = 64) </param>
        public static Icon ToIcon(this string ImagePath, int ICOSize = 64)
        {
            //string VarType = Image.GetType().ToString();  //determine what kind of variable was passed into function

            //if (VarType == "System.String")  // Image File Path
            //{
            // check the file extension of the file passed in 

            /*
                            string fileExt = (string)ImagePath. , true);

                            if (fileExt == ".exe") // extract default icon from exe 
                            {
                                return Icon.ExtractAssociatedIcon((string)Image);
                            }
            */
            // set icon size - use optional parameter
            //int ICOSize = 64;
            //if (Option != null) { ICOSize = (int)Option; }



            var bmp = Bitmap.FromFile(ImagePath);
            var thumb = (Bitmap)bmp.GetThumbnailImage(ICOSize, ICOSize, null, IntPtr.Zero);
            thumb.MakeTransparent();
            Icon ico = Icon.FromHandle(thumb.GetHicon());

            return ico;
        }

        /// <summary>
        /// Save Icon to File 
        /// </summary>
        /// <param name="IconToSave"></param>
        /// <param name="SavePath"></param>
        /// <returns></returns>
        public static bool SaveICO(this Icon IconToSave, string SavePath)
        {
            using (FileStream fs = new FileStream(SavePath, FileMode.Create))
                IconToSave.Save(fs);

            if (File.Exists(SavePath)) { return true; }
            else { return false; }
        }


        ///// <summary>Convert Image Path (png, ico, exe) / Icon / ImageList (By Key) Item / or Returns Image if Provided</summary>
        ///// <param name="Image"> </param>
        ///// <param name="KeyName"> </param>
        //public static Image To_Image(this Icon Image, string KeyName = "")
        //{
        //    string VarType = Image.GetType().ToString();  //determine what kind of variable was passed into function

        //    if (VarType == "System.Drawing.Icon")  // Icon
        //    {
        //        Icon ico = (Icon)Image;
        //        return ico.ToBitmap();
        //    }

        //    return null;
        //}

        ///// <summary>Convert Image Path (png, ico, exe) / Icon / ImageList (By Key) Item / or Returns Image if Provided</summary>
        ///// <param name="Image"> </param>
        ///// <param name="KeyName"> </param>
        //public static Image ToImage(this object Image, int width = 0, int height = 0, string KeyName = "")
        //{
        //    if (Image == null) { return null; }

        //    string VarType = Image.GetType().ToString();  //determine what kind of variable was passed into function

        //    if (VarType == "System.String")  // Image File Path
        //    {
        //        if (!File.Exists((string)Image)) { return null; }

        //        // check the file extension of the file passed in 
        //        string fileExt = FileExt((string)Image, true);

        //        if (fileExt.ToUpper() == ".EXE") // extract default image from exe 
        //        {
        //            Icon a = Icon.ExtractAssociatedIcon((string)Image);
        //            Image returnImage = ToImage(a);  // convert icon to image
        //            return returnImage;  // return image
        //        }

        //        // otherwise read image file path and convert to image and return
        //        Image fileImage = GetCopyImage((string)Image);

        //        // resize image if user provided w/h
        //        if (width != 0 && height != 0)
        //        {
        //            fileImage = Resize((string)Image, width, height);
        //        }

        //        return fileImage;
        //    }
        //    if (VarType == "System.Drawing.Icon")  // Icon
        //    {
        //        Icon ico = (Icon)Image;
        //        return ico.ToBitmap();
        //    }
        //    if (VarType == "System.Drawing.Bitmap")  // Image (returns same image unchanged)
        //    {
        //        return (Image)Image;
        //    }
        //    if (VarType == "System.Windows.Forms.ImageList")  // ImageList
        //    {
        //        return ToImage((ImageList)Image, KeyName);  // return image from image list by key
        //    }

        //    return null;
        //}

        /// <summary>
        /// Convert Bitmap to System.Drawing.Image 
        /// </summary>
        /// <param name="Image"></param>
        /// <returns></returns>
        public static Image ToImage(this Bitmap Image)
        {
            if (Image == null) { return null; }
            return (Image)Image;
        }

        /// <summary>
        /// Convert Icon Obj to System.Drawing.Image 
        /// </summary>
        /// <param name="icon"></param>
        /// <returns></returns>
        public static Image ToImage(this Icon icon)
        {
            if (icon == null) { return null; }
            //if (icon.GetType().ToString() == "System.Drawing.Icon")  // Icon
            Icon ico = (Icon)icon;
            return ico.ToBitmap();
        }


        /// <summary>
        /// Convert Image File Path or EXE FilePath to System.Drawing.Image 
        /// </summary>
        /// <param name="FilePath">Image File or EXE Containing Icon</param>
        /// <param name="Width">New Image Width</param>
        /// <param name="Height">New Image Height</param>
        /// <returns></returns>
        public static Image ToImage(this string FilePath, int Width=0, int Height=0)
        {
            if (!File.Exists(FilePath)) { return null; }

            // check the file extension of the file passed in 
            if (FileExt((string)FilePath, true).ToUpper() == ".EXE") // extract default image from exe 
            {
                Icon a = Icon.ExtractAssociatedIcon((string)FilePath);
                Image returnImage = ToImage(a);  // convert icon to image

                if (Width != 0 || Height != 0) { return returnImage.Resize(Width, Height); }

                return returnImage;  // return image
            }
            else
            {
                if (FilePath.IsImage()) { return System.Drawing.Image.FromFile(FilePath);  }
            }
            
            return null;
        }


        /// <summary>Search imagelist for key (file name) - returns image</summary>
        /// <param name="IL">ImageList to Return Value From</param>
        /// <param name="KeyName">ImageList Key Name</param>
        /// <param name="Width">New Image Width</param>
        /// <param name="Height">New Image Height</param>
        public static Image ToImage(this ImageList IL, string KeyName = "png_arrow-down-icon.png", int Width = 0, int Height = 0)
        {
            List<string> imageList = FileNames(IL);

            foreach (string image in imageList)
            {
                string img = FileName(image);
                if (KeyName == img)
                {
                    Image ime = GetCopyImage(image);
                    if (Width != 0 || Height != 0) { return ime.Resize(Width, Height); }
                    return ime;
                }
                img = FileNameNoExt(image);
                if (KeyName == img)
                {
                    Image ime = GetCopyImage(image);
                    if (Width != 0 || Height != 0) { return ime.Resize(Width, Height); }
                    return ime;
                }
                if (image.Contains(KeyName))
                {
                    Image ime = GetCopyImage(image);
                    if (Width != 0 || Height != 0) { return ime.Resize(Width, Height); }
                    return ime;
                }
            }

            return null;
        }

        
        
        /// <summary>return list of key names (also the filename without ext) in ImageList</summary>
        /// <param name="ImageList IL"> </param>
        /// <param name="FileNameOnly"> </param>
        /// <param name="FileExt"> </param>
        public static List<string> FileNames(this ImageList IL, bool FileNameOnly = false, bool FileExt = false)
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


        /// <summary>Copy Image Before Loading, Frees File to Delete</summary>
        /// <param name="path">FilePath to Image to Load Copy Of</param>
        public static Image GetCopyImage(this string path)
        {
            if (File.Exists(path))
            {
                if (IsImage(path))
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

        /// <summary>resize image (untested)</summary>
        /// <param name="ImagePath"> </param>
        /// <param name="Width">New Image Width</param>
        /// <param name="Height">New Image Height</param>
        public static Image Resize(this string ImagePath, int Width, int Height)
        {
            Image imgPhoto = Image.FromFile(ImagePath);

            int sourceWidth = imgPhoto.Width;
            int sourceHeight = imgPhoto.Height;

            //Consider vertical pics
            if (sourceWidth < sourceHeight)
            {
                int buff = Width;

                Width = Height;
                Height = buff;
            }

            int sourceX = 0, sourceY = 0, destX = 0, destY = 0;
            float nPercent = 0, nPercentW = 0, nPercentH = 0;

            nPercentW = ((float)Width / (float)sourceWidth);
            nPercentH = ((float)Height / (float)sourceHeight);
            if (nPercentH < nPercentW)
            {
                nPercent = nPercentH;
                destX = System.Convert.ToInt16((Width -
                          (sourceWidth * nPercent)) / 2);
            }
            else
            {
                nPercent = nPercentW;
                destY = System.Convert.ToInt16((Height -
                          (sourceHeight * nPercent)) / 2);
            }

            int destWidth = (int)(sourceWidth * nPercent);
            int destHeight = (int)(sourceHeight * nPercent);


            Bitmap bmPhoto = new Bitmap(Width, Height,
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

        /// <summary>
        /// Resize Image Object (untested)
        /// </summary>
        /// <param name="image"></param>
        /// <param name="newWidth"></param>
        /// <param name="newHeight"></param>
        /// <returns></returns>
        public static Image Resize(this Image image, int newWidth, int newHeight)
        {
            //Image imgPhoto = Image.FromFile(ImagePath);

            int sourceWidth = image.Width;
            int sourceHeight = image.Height;

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

            bmPhoto.SetResolution(image.HorizontalResolution,
                         image.VerticalResolution);

            Graphics grPhoto = Graphics.FromImage(bmPhoto);
            grPhoto.Clear(Color.Black);
            grPhoto.InterpolationMode =
                System.Drawing.Drawing2D.InterpolationMode.HighQualityBicubic;

            grPhoto.DrawImage(image,
                new Rectangle(destX, destY, destWidth, destHeight),
                new Rectangle(sourceX, sourceY, sourceWidth, sourceHeight),
                GraphicsUnit.Pixel);

            grPhoto.Dispose();
            image.Dispose();
            return bmPhoto;
        }


        /// <summary>Returns True if FilePath is PNG,GIF,ICO,JPG,JPEG</summary>
        /// <param name="FilePath">Path of File To Check if Image File</param>
        public static bool IsImage(this string FilePath)
        {
            string extension = Path.GetExtension(FilePath);
            extension = extension.ToUpper();

            // make sure file format is valid image, otherwise go to next item in list
            bool FormatAccepted = false;
            if (extension == ".PNG") { FormatAccepted = true; }
            if (extension == ".GIF") { FormatAccepted = true; }
            if (extension == ".ICO") { FormatAccepted = true; }
            if (extension == ".JPG") { FormatAccepted = true; }
            if (extension == ".JPEG") { FormatAccepted = true; }
            if (extension == ".ICO") { FormatAccepted = true; }
            if (extension == ".ICON") { FormatAccepted = true; }

            return FormatAccepted;
        }

        /// <summary>
        /// Returns Image's Width
        /// </summary>
        /// <param name="ImageFilePath">Path to Image</param>
        /// <returns></returns>
        public static int ImageWidth(this string ImageFilePath)
        {
            _AHK ahk = new _AHK();
            Image Bmap = ToImage(ImageFilePath);

            //Bitmap Bmap = Image.FromFile(FilePath);
            //int BmH = Bmap.Height;
            int BmW = Bmap.Width;
            Bmap.Dispose(); // Avoid Out of Memory errors

            return BmW;
        }

        /// <summary>
        /// Returns Image's Height
        /// </summary>
        /// <param name="ImageFilePath">Path to Image</param>
        /// <returns></returns>
        public static int ImageHeight(this string ImageFilePath)
        {
            Image Bmap = ToImage(ImageFilePath);
            //Bitmap Bmap = Image.FromFile(FilePath);
            int BmH = Bmap.Height;
            //int BmW = Bmap.Width;
            Bmap.Dispose(); // Avoid Out of Memory errors
            return BmH;
        }



    }
}
