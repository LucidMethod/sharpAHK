using AHKExpressions;
using System;
using System.Collections.Generic;
using System.Drawing;
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
        public string Download_HTML(string URL, string SaveFile = "", string login = "", string pass = "", bool HideErrorMsg = true)
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
            catch(Exception ex)
            {
                if (!HideErrorMsg)
                {
                    _AHK ahk = new _AHK();
                    ahk.MsgBox(ex.ToString());
                }
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

        /// <summary>
        /// URL to Image
        /// </summary>
        /// <param name="URL"></param>
        /// <returns></returns>
        public Image Download_Image(string URL)
        {
            using (System.Net.WebClient webClient = new System.Net.WebClient())
            {
                using (Stream stream = webClient.OpenRead(URL))
                {
                    return Image.FromStream(stream);
                }
            }
        }

        /// <summary>
        /// Returns Public IP Address
        /// </summary>
        /// <returns></returns>
        public static string PublicIP()
        {
            string url = "http://checkip.dyndns.org";
            System.Net.WebRequest req = System.Net.WebRequest.Create(url);
            System.Net.WebResponse resp = req.GetResponse();
            System.IO.StreamReader sr = new System.IO.StreamReader(resp.GetResponseStream());
            string response = sr.ReadToEnd().Trim();
            string[] a = response.Split(':');
            string a2 = a[1].Substring(1);
            string[] a3 = a2.Split('<');
            string a4 = a3[0];
            return a4;
        }


        /// <summary>
        /// Takes HTML Listing from IIS Directory Listing and Parses Out Links (works for 1 directory deep)
        /// </summary>
        /// <param name="URL"></param>
        /// <returns></returns>
        public List<string> LinkFromIISDir(string URL = "http://liveinthemedia.com/Sonarr")
        {
            List<string> Links = new List<string>();

            string baseURL = URL;
            WebClient client = new WebClient();
            string content = "";

            try
            {
                content = client.DownloadString(baseURL);
            }
            catch (Exception ex)
            {
                MsgBox(ex.ToString());
                return Links;
            }

            if (content != "")
            {
                string url = "";
                List<string> Lines = Text_ToList(content, true, true, false);
                foreach (string line in Lines)
                {
                    if (line.Contains("<frame src="))
                    {
                        url = StringSplit(line, "\"", 1).Replace("\"", "");
                    }
                }

                string Folder = StringSplit(baseURL, "/", 0, true);
                string bURL = baseURL.Replace(Folder, "").TrimLast(1);

                string html = Download_HTML(url);
                html = html.Replace("<A HREF=", "|");
                List<string> lines = StringSplit_List(html, "|", true);
                int i = 0; string header = "";
                foreach (string line in lines)
                {
                    i++; if (i < 3) { header = header + line; continue; } // skip first two header lines
                    string tLine = StringSplit(line, ">", 0).Replace("\"", "");

                    string urr = bURL + tLine;
                    Links.Add(urr);
                }
            }

            return Links;
        }

    }
}
