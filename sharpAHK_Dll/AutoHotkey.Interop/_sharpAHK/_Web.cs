using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace sharpAHK
{
    public partial class _AHK
    {
        //#region === Web ===

        /// <summary>
        /// Downloads HTML string from URL path
        /// </summary>
        /// <param name="URL">URL to Download</param>
        /// <param name="SaveFile">Optional Local File Path to Save HTML To (Only Returns HTML as String if Blank)</param>
        /// <param name="login">Optional Login Parameter for URL</param>
        /// <param name="pass">Optional Password Parameter for URL</param>
        /// <returns>Returns HTML from Website as String</returns>
        /// <example>
        /// string HTML = ahk.Download_HTML("http://www.imdb.com/title/tt1985949/", "c:\\HTML.txt");
        /// </example>
        public string Download_HTML(string URL, string SaveFile = "", string login = "", string pass = "")
        {
            //### download a web page to a string
            WebClient client = new WebClient();

            if (login != "")  // option to set login parameters
            {
                client.Credentials = new System.Net.NetworkCredential(login, pass);
            }

            client.Headers.Add("user-agent", "Mozilla/4.0 (compatible; MSIE 6.0; Windows NT 5.2; .NET CLR 1.0.3705;)");

            string s = "";
            try
            {
                Stream data = client.OpenRead(URL);
                StreamReader reader = new StreamReader(data);
                s = reader.ReadToEnd();

                if (SaveFile != "")
                {
                    FileDelete(SaveFile);
                    FileAppend(s, SaveFile);
                    //Run(SaveFile);
                }
            }
            catch
            {
                return "";
            }




            return s;
        }


        /// <summary>
        /// Download File From Website
        /// </summary>
        /// <param name="remoteFileUrl">URL to File</param>
        /// <param name="localFileName">Local Save Path</param>
        /// <param name="SkipExisting">Option to Skip Downloading if Local File Already Exists</param>
        /// <returns></returns>
        public bool Download_File(string remoteFileUrl, string localFileName, bool SkipExisting = true)
        {
            // if enabled, will skip downloading the same file again 

            if (SkipExisting) { if (File.Exists(localFileName)) { return true; } }


            WebClient webClient = new WebClient();
            try
            {
                webClient.DownloadFile(remoteFileUrl, localFileName);
            }
            catch (Exception ex)
            {
                //ahk.MsgBox(ex.ToString());
                return false;
            }

            // confirm file is found in local location after download, return true if found

            if (File.Exists(localFileName)) { return true; }
            else { MsgBox("Error Saving " + localFileName + " ??\n\nLocal File Not Found\n\nURL: " + remoteFileUrl); return false; }
        }



        
    }
}
