using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net;
using System.IO;
using System.Globalization;
using System.Threading;

namespace WhatDownload
{
    class WhatDownloadMain
    {
        static string baseurl = "ftp://gumdrop.whatbox.ca/{0}";
        static void Main(string[] args)
        {
            CLI cli = new CLI(baseurl, "foojewel", "jewfoo");
            cli.run();

            //FTP ftp = new FTP(baseurl, "foojewel", "jewfoo");
            //ftp.upload("steve/movie/test.txt", @"C:\Users\spicone\Desktop\poop.bat");
            //string[] list = ftp.lsDetailed("steve/movie");
            //foreach (string s in list)
            //{
            //    Console.WriteLine(s);
            //}
            //ftp.makeDir("steve/movie/testing/hork");
            //CommandWords derp = new CommandWords();
            //derp.showAll();
            //Console.WriteLine(derp.isCommand("ls"));

            //ftp.upload("steve/movie/testing/hork/butt.txt", @"C:\Users\spicone\Desktop\poop.bat");
            //Console.WriteLine(ftp.pwdDetailed("steve/movie/testing"));
            //Console.WriteLine(ftp.pwdDetailed("steve/movie/testing/hork"));

            //ftp.downloadAll("steve/movie/testing/", @"C:\Users\spicone\Desktop\poop\");



            //Console.WriteLine(ftp.pwdDetailed("steve/movie/40 Days and Nights (2012)"));

            //Console.WriteLine(ftp.getFileSize("steve/movie/40 Days and Nights (2012)/40.Days.and.Nights.2012.720p.BluRay.x264.YIFY.mp4"));
            //Console.WriteLine(ftp.getCreationDateTime("steve/movie/40 Days and Nights (2012)/40.Days.and.Nights.2012.720p.BluRay.x264.YIFY.mp4"));
            //ftp = null;
            //Console.WriteLine("Enter the directory to check: ");
            //string directory = Console.ReadLine();
            //FtpWebRequest request = (FtpWebRequest)WebRequest.Create(String.Format(baseurl,directory));
            //request.Method = WebRequestMethods.Ftp.ListDirectoryDetails; 

            //request.Credentials = new NetworkCredential("foojewel", "jewfoo");

            //FtpWebResponse response = (FtpWebResponse)request.GetResponse();

            //Stream responseStream = response.GetResponseStream();
            //StreamReader reader = new StreamReader(responseStream);
            //Console.WriteLine(reader.ReadToEnd());

            //Console.WriteLine("Listing complete, status {0}", response.StatusDescription);

            //reader.Close();
            //response.Close();
            //Console.ReadKey();
            Console.Write("Shutting down in 3. . .");
            Thread.Sleep(1000);
            Console.Write("\rShutting down in 2. . .");
            Thread.Sleep(1000);
            Console.Write("\rShutting down in 1. . .");
            Thread.Sleep(1000);
        }
    }
}
