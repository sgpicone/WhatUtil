using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Text.RegularExpressions;

namespace WhatDownload
{
    class FTPFile
    {
        public string originalRecordString { get; set; }

        public Uri url { get; set; }
        public string name { get; set; }
        public bool isDir { get; set; }
        public DateTime modTime { get; set; }
        public long size { get; set; }

        private FTPFile()
        {
            //nothing to do...
        }

        public override string ToString()
        {
            return String.Format("{0}\t{1}\t\t{2}",
                this.modTime.ToString("yyyy-MM-dd HH:mm"),
                this.isDir ? "<DIR>" : this.size.ToString(),
                this.name);
        }

        public static FTPFile parseRecordString(Uri baseUrl, string recordString)
        {
            FTPFile ftpFile = null;
            ftpFile = parseUNIXRecordString(recordString);//only need UNIX, since this is for the whatbox only, which runs UNIX
            ftpFile.url = new Uri(baseUrl, ftpFile.name + (ftpFile.isDir ? "/" : string.Empty));
            return ftpFile;
        }

        /// <summary>
        /// The recordString is like
        /// Directory: drwxrwxrwx   1 owner    group               0 Dec 13 11:25 Folder A
        /// File:      -rwxrwxrwx   1 owner    group               1024 Dec 13 11:25 File B
        /// NOTE: The date segment does not contains year.
        /// </summary>
        private static FTPFile parseUNIXRecordString(string recordString)
        {
            FTPFile ftpFile = new FTPFile();

            ftpFile.originalRecordString = recordString.Trim();

            // The segments is like "drwxrwxrwx", "",  "", "1", "owner", "", "", "", 
            // "group", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", 
            // "0", "Dec", "13", "11:25", "Folder", "A".
            string[] segments = ftpFile.originalRecordString.Split(' ');

            int index = 0;

            // The permission segment is like "drwxrwxrwx".
            string permissionsegment = segments[index];

            // If the property start with 'd', then it means a directory.
            ftpFile.isDir = permissionsegment[0] == 'd';

            // Skip the empty segments.
            while (segments[++index] == string.Empty) { }

            // Skip the directories segment.

            // Skip the empty segments.
            while (segments[++index] == string.Empty) { }

            // Skip the owner segment.

            // Skip the empty segments.
            while (segments[++index] == string.Empty) { }

            // Skip the group segment.

            // Skip the empty segments.
            while (segments[++index] == string.Empty) { }

            // If this fileSystem is a file, then the size is larger than 0. 
            ftpFile.size = int.Parse(segments[index]);

            // Skip the empty segments.
            while (segments[++index] == string.Empty) { }

            // The month segment.
            string monthsegment = segments[index];

            // Skip the empty segments.
            while (segments[++index] == string.Empty) { }

            // The day segment.
            string daysegment = segments[index];

            // Skip the empty segments.
            while (segments[++index] == string.Empty) { }

            // The time segment.
            string timesegment = segments[index];

            ftpFile.modTime = DateTime.Parse(string.Format("{0} {1} {2} ",
                timesegment, monthsegment, daysegment));

            // Skip the empty segments.
            while (segments[++index] == string.Empty) { }

            // Calculate the index of the file name part in the original string.
            int filenameIndex = 0;

            for (int i = 0; i < index; i++)
            {
                // "" represents ' ' in the original string.
                if (segments[i] == string.Empty)
                {
                    filenameIndex += 1;
                }
                else
                {
                    filenameIndex += segments[i].Length + 1;
                }
            }
            // The file name may include many segments because the name can contain ' '.          
            ftpFile.name = ftpFile.originalRecordString.Substring(filenameIndex).Trim();

            return ftpFile;
        }

    }
}
