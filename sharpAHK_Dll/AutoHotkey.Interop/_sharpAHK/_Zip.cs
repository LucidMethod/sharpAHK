using System;
using System.Collections.Generic;
using System.IO;
using System.IO.Compression;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace sharpAHK
{
    public partial class _AHK
    {
        #region === Zip ===

        // using System.IO.Compression;

        /// <summary>Add File to New or Existing .Zip File</summary>
        /// <param name="FileToZip">FilePath to add to New/Existing Zip File</param>
        /// <param name="zipFile">New/Existing Zip File to Add File To</param>
        /// <param name="Fastest">Option to add Zip File Fastest Method (Slower = Smaller File)</param>
        /// <param name="FullPath">False = File in Root of Zip File, True = Full folder structure as found in original file path</param>
        /// <example>
        /// bool Zipped = ahk.Zip(FilePath, newZip, false, false, false);  // places file in root of zip file
        /// bool Zipped = ahk.Zip(FilePath, newZip, false, false, true);   // places file in same folder structure as found on location drive (buried in sub folders under drive letter inside zip)
        /// </example>
        public bool Zip(string FileToZip, string zipFile = "", bool Fastest = true, bool FullPath = false)
        {
            bool RelativeToRoot = false;  // doesn't seem to make a diff

            //bool DeleteExistingZip = true;
            //if (DeleteExistingZip) { FileDelete(zipFile); }

            // if user passes in a directory to zip - use ZipDir function 
            if (IsDir(FileToZip)) { ZipDir(FileToZip, zipFile); return true; }

            // update / add to existing zip file
            if (File.Exists(zipFile))
            {
                try
                {
                    string EntryPath = FileName(FileToZip);

                    //bool FullPath = false;
                    if (FullPath) { EntryPath = FileToZip; }

                    //bool RelativeToRoot = true;
                    //if (OutDirPath == "SameDirAsFile") { RelativeToRoot = true; }

                    //string RootDir = @"C:\Users\Jason\Google Drive\IMDB\";
                    //if (RelativeToRoot) { EntryPath = StringReplace(FileToZip, RootDir); }

                    CompressionLevel compLevel = CompressionLevel.Fastest;
                    if (!Fastest) { compLevel = CompressionLevel.Optimal; }

                    using (var fileStream = new FileStream(zipFile, FileMode.Open))
                    {
                        using (var archive = new ZipArchive(fileStream, ZipArchiveMode.Update, true))
                        {
                            byte[] bytes = System.IO.File.ReadAllBytes(FileToZip);
                            var zipArchiveEntry = archive.CreateEntry(EntryPath, compLevel);
                            using (var zipStream = zipArchiveEntry.Open())
                                zipStream.Write(bytes, 0, bytes.Length);
                        }
                    }
                }
                catch
                {
                    return false;
                }
            }

            // create new zip file if it doesn't exist yet
            if (!File.Exists(zipFile))
            {

                string EntryPath = FileName(FileToZip);

                //bool FullPath = false;
                if (FullPath) { EntryPath = FileToZip; }

                //bool RelativeToRoot = true;
                string RootDir = FileDir(FileToZip);
                if (zipFile != "") { RootDir = FileDir(zipFile); }

                if (RelativeToRoot) { EntryPath = StringReplace(FileToZip, RootDir + "\\"); }


                try
                {
                    using (var fileStream = new FileStream(zipFile, FileMode.CreateNew))
                    {
                        using (var archive = new ZipArchive(fileStream, ZipArchiveMode.Create, true))
                        {
                            byte[] bytes = System.IO.File.ReadAllBytes(FileToZip);
                            var zipArchiveEntry = archive.CreateEntry(EntryPath, CompressionLevel.Fastest);
                            using (var zipStream = zipArchiveEntry.Open())
                                zipStream.Write(bytes, 0, bytes.Length);
                        }
                    }

                }
                catch
                {
                    return false;
                }
            }

            // for moving zip files to new dirs as zipping

            //Sleep(1000);
            //string newZipLocation = FileDir(zipFile) + "\\Zip_Files";
            //string newFileLocation = FileDir(zipFile) + "\\Zipped";
            //FileCreateDir(newZipLocation);
            //FileCreateDir(newFileLocation);
            //FileMove(zipFile, FileDir(newZipLocation + "\\" + FileName(zipFile), false));
            //FileMove(FileToZip, FileDir(newFileLocation + "\\" + FileName(FileToZip), false));

            return true;
        }

        /// <summary>Add Folder to Zip File</summary>
        /// <param name="DirToZip">Path of Directory to Zip</param>
        /// <param name="zipFile">New/Existing Zip File to Add Folder To</param>
        /// <param name="Fastest">Either Fastest speed (True) or (False) for Optimal Compression level</param>
        /// <param name="IncludeBaseDir">Default option to include base directory in zip file structure</param>
        public bool ZipDir(string DirToZip, string zipFile, bool Fastest = true, bool IncludeBaseDir = true)
        {
            FileDelete(zipFile);

            if (Fastest) { ZipFile.CreateFromDirectory(DirToZip, zipFile, CompressionLevel.Fastest, IncludeBaseDir); }
            else { ZipFile.CreateFromDirectory(DirToZip, zipFile, CompressionLevel.Optimal, IncludeBaseDir); }

            return true;
        }

        /// <summary>Unzip Zip File to Directory</summary>
        /// <param name="zipFile">Zip File Path</param>
        /// <param name="UnZipDir">Location to Unzip Files To (If Blank Unzips To ZipFileDir\ZipFileName</param>
        public bool UnZip(string zipFile, string UnZipDir = "")
        {
            if (UnZipDir == "") { UnZipDir = FileDir(zipFile) + "\\" + FileNameNoExt(zipFile); }

            try
            {
                ZipFile.ExtractToDirectory(zipFile, UnZipDir);
                return true;
            }
            catch
            {
                return false;
            }
        }

        /// <summary>Loop through DirPath and create new ZIP file for each file in directory</summary>
        /// <param name="DirPath"> </param>
        /// <param name="Fastest"> </param>
        /// <param name="SearchPattern"> </param>
        public void ZipEachFile(string DirPath, bool Fastest = true, string ZipSaveDir = "", string SearchPattern = "*.*", bool Recurse = false)
        {
            List<string> fileList = new List<string>();

            string[] files = null;
            if (Recurse) { files = Directory.GetFiles(DirPath, SearchPattern, System.IO.SearchOption.AllDirectories); }
            if (!Recurse) { files = Directory.GetFiles(DirPath, SearchPattern, System.IO.SearchOption.TopDirectoryOnly); }

            foreach (string file in files)  // loop through directory and return list of directories under DirPath
            {
                fileList.Add(file);
            }

            int zippedCount = 0;
            string zipSaveDir = DirPath;
            foreach (string f in fileList)
            {
                zippedCount++;

                // update displayControl Text with progress (if user defined this control name)
                DisplayText("Zipping File " + f + "...");

                string fileNameNoExt = FileNameNoExt(f);

                if (ZipSaveDir.Trim() != "") { zipSaveDir = ZipSaveDir; }
                if (ZipSaveDir.Trim() == "") { zipSaveDir = DirPath; }

                FileCreateDir(zipSaveDir); // ensure save directory exists


                string newZip = zipSaveDir + "\\" + fileNameNoExt + ".zip";

                Zip(f, newZip, Fastest);

                //MsgBox("Zipped Dir " + zippedCount.ToString() + " / " + dirList.Count.ToString()); 
            }

            // update displayControl Text with progress (if user defined this control name)
            DisplayText("Finished Zipping " + zippedCount.ToString() + " + Files");

            //MsgBox("Finished Zipping " + zippedCount.ToString() + " Directories");

            //string DirName = FileName(file); // name of the folder

        }

        /// <summary>Loop through DirPath and create new ZIP file for each folder in directory [CREATES NEW THREAD]</summary>
        /// <param name="DirPath">Root directory to search for folders to zip</param>
        /// <param name="Fastest">Compression Method, Fastest = true, Optimal = false</param>
        /// <param name="SearchPattern">Search pattern for folder names to zip</param>
        public void ZipEachFile_NewThread(string DirPath, bool Fastest = true, string ZipSaveDir = "", string SearchPattern = "*.*", bool Recurse = false)
        {
            Thread zipthread = new Thread(() => ZipEachFile(DirPath, Fastest, ZipSaveDir, SearchPattern, Recurse));
            zipthread.Start();
        }


        /// <summary>Loop through DirPath and create new ZIP file for each folder in directory</summary>
        /// <param name="DirPath">Root directory to search for folders to zip</param>
        /// <param name="Fastest">Compression Method, Fastest = true, Optimal = false</param>
        /// <param name="SearchPattern">Search pattern for folder names to zip</param>
        public void ZipEachDir(string DirPath, bool Fastest = true, string SearchPattern = "*.*")
        {
            List<string> dirList = new List<string>();

            bool Recurse = false;
            string[] dirPaths = null;
            if (Recurse) { dirPaths = Directory.GetDirectories(DirPath, SearchPattern, System.IO.SearchOption.AllDirectories); }
            if (!Recurse) { dirPaths = Directory.GetDirectories(DirPath, SearchPattern, System.IO.SearchOption.TopDirectoryOnly); }

            foreach (string dir in dirPaths)  // loop through directory and return list of directories under DirPath
            {
                dirList.Add(dir);
            }

            int zippedCount = 0;
            foreach (string Directory in dirList)
            {
                zippedCount++;

                // update displayControl Text with progress (if user defined this control name)
                DisplayText("Zipping Directory " + zippedCount + " / " + dirList.Count.ToString() + "...");

                string dirName = DirName(Directory);

                string newZip = DirPath + "\\" + dirName + ".zip";

                ZipDir(Directory, newZip, Fastest);

                //MsgBox("Zipped Dir " + zippedCount.ToString() + " / " + dirList.Count.ToString()); 
            }

            // update displayControl Text with progress (if user defined this control name)
            DisplayText("Finished Zipping " + zippedCount + " Directories");

            MsgBox("Finished Zipping " + zippedCount.ToString() + " Directories");

            //string DirName = FileName(file); // name of the folder

        }

        /// <summary>Loop through DirPath and create new ZIP file for each folder in directory [CREATES NEW THREAD]</summary>
        /// <param name="DirPath">Root directory to search for folders to zip</param>
        /// <param name="Fastest">Compression Method, Fastest = true, Optimal = false</param>
        /// <param name="SearchPattern">Search pattern for folder names to zip</param>
        public void ZipEachDir_NewThread(string DirPath, bool Fastest = true, string SearchPattern = "*.*")
        {
            Thread zipthread = new Thread(() => ZipEachDir(DirPath, Fastest, SearchPattern));
            zipthread.Start();
        }



        #endregion
    }
}
