using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace WhatDownload
{

    //request = (FtpWebRequest)WebRequest.Create(String.Format(baseurl, directory));
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

    public partial class FTP
    {
        public ICredentials creds { get; set; }
        public Uri connectUrl { get; private set; }
        FTPStatus status;

        public FTPStatus Status
        {
            get
            {
                return status;
            }

            private set
            {
                if (status != value)
                {
                    status = value;
                    this.OnStatusChanged(EventArgs.Empty);
                }
            }
        }

        public event EventHandler<ErrorEventHandler> ErrorOccurred;
        public event EventHandler StatusChanged;
        public event EventHandler<FileDownloadCompletedEventArgs> FileDownloadCompleted;
        public event EventHandler<NewMessageEventArg> NewMessageArrived;




        private string          baseUrl             = null;
        private string          userName            = null;
        private string          password            = null;
        private FtpWebRequest   ftpRequest          = null;
        private FtpWebResponse  ftpResponse         = null;
        private Stream          ftpStream           = null;
        private int             bufferSize          = 2048;

        /// <summary>
        /// Creates a new FTP object using the given base url, user, and password
        /// </summary>
        /// <param name="url">the base url for this ftp server. A token is appended to the end
        /// So that it may be formatted for other directories on the site.
        /// i.e., given "ftp://yoursite.home.gov/", it will create
        /// "ftp://yoursite.home.gov/{0}", allowing allowing it to be
        /// further expanded later to more directories.</param>
        /// <param name="user">The username to connect with</param>
        /// <param name="pass">The password to connect with</param>
        public FTP(string url, string user, string pass)
        {
            baseUrl = url.EndsWith("}") ? url : url.EndsWith("/") ? url + "{0}" : url + "/{0}"; //I love ternary operators!
            this.connectUrl = new Uri(baseUrl);
            userName = user;
            password = pass;
        }

        /// <summary>
        /// Download all of the files from a specified directory and
        /// all of its subdirectories
        /// </summary>
        /// <param name="remoteDir">The remote directory to download from</param>
        public void downloadAll(string remoteDir, string localDir)
        {
            List<FTPFile> filesAndDirs = getSubDirsAndFiles(remoteDir) as List<FTPFile>;
            if (!Directory.Exists(localDir))
                Directory.CreateDirectory(localDir);

            foreach (FTPFile file in filesAndDirs)
            {
                string remoteSubDir = remoteDir + "/" + file.name;
                string localSubDir = localDir + "\\" + file.name;
                if (!file.isDir)
                {
                    //string dest = localDir + "\\" + file.name;
                    //string src = remoteDir + "/" + file.name;
                    download(remoteSubDir, localSubDir);
                }
                else
                {
                    //string dirPath = localDir + "\\" + file.name;
                    if (!Directory.Exists(localSubDir))
                    {
                        Directory.CreateDirectory(localSubDir);
                    }
                    
                    List<FTPFile> subs = getSubDirsAndFiles(remoteSubDir) as List<FTPFile>;
                    foreach (FTPFile sub in subs)
                    {
                        downloadAll(remoteSubDir, localSubDir);
                    }
                }
            }
        }

        public void download(string remoteFile, string localFile)
        {
            FileStream localFileStream = null;
            try
            {
                ftpRequest = (FtpWebRequest)WebRequest.Create(String.Format(baseUrl, remoteFile));
                ftpRequest.Credentials = new NetworkCredential(userName, password);

                ftpRequest.UseBinary = false; //TODO: figure out why this downloads correctly when in most cases with windows it shouldn't
                ftpRequest.UsePassive = true;
                ftpRequest.KeepAlive = true;
                ftpRequest.EnableSsl = true;

                ftpRequest.Method = WebRequestMethods.Ftp.DownloadFile;

                ftpResponse = (FtpWebResponse)ftpRequest.GetResponse();

                ftpStream = ftpResponse.GetResponseStream();

                localFileStream = new FileStream(localFile, FileMode.Create);

                byte[] buffer = new byte[bufferSize];

                int bytesRead = ftpStream.Read(buffer, 0, bufferSize);
                try
                {
                    while (bytesRead > 0)
                    {
                        localFileStream.Write(buffer, 0, bufferSize);
                        bytesRead = ftpStream.Read(buffer, 0, bufferSize);
                    }
                }
                catch (Exception ex)
                {
                    if (localFileStream != null)
                        localFileStream.Close();
                    Console.WriteLine("Error reading remote file.\n" + ex.ToString());
                }

                localFileStream.Close();
                ftpStream.Close();
                ftpResponse.Close();
                ftpRequest = null;
            }
            catch(Exception ex)
            {
                if (ftpRequest != null)
                    ftpRequest = null;
                if (localFileStream != null)
                    localFileStream.Close();
                if (ftpResponse != null)
                    ftpResponse.Close();
                if (ftpStream != null)
                    ftpStream.Close();

                Console.WriteLine("Error downloading file.\n" + ex.ToString());
            }
        }

        public void upload(string localFile, string remoteFile)
        {
            FileStream localFileStream = null;
            try
            {
                /* Create an FTP Request */
                ftpRequest = (FtpWebRequest)FtpWebRequest.Create(String.Format(baseUrl,remoteFile));
                /* Log in to the FTP Server with the User Name and Password Provided */
                ftpRequest.Credentials = new NetworkCredential(userName, password);
                /* When in doubt, use these options */
                ftpRequest.UseBinary = true;
                ftpRequest.UsePassive = true;
                ftpRequest.KeepAlive = true;
                ftpRequest.EnableSsl = true;
                /* Specify the Type of FTP Request */
                ftpRequest.Method = WebRequestMethods.Ftp.UploadFile;
                /* Establish Return Communication with the FTP Server */
                ftpStream = ftpRequest.GetRequestStream();
                /* Open a File Stream to Read the File for Upload */
                localFileStream = new FileStream(localFile, FileMode.Create);
                /* Buffer for the Downloaded Data */
                byte[] byteBuffer = new byte[bufferSize];
                int bytesSent = localFileStream.Read(byteBuffer, 0, bufferSize);
                /* Upload the File by Sending the Buffered Data Until the Transfer is Complete */
                try
                {
                    while (bytesSent != 0)
                    {
                        ftpStream.Write(byteBuffer, 0, bytesSent);
                        bytesSent = localFileStream.Read(byteBuffer, 0, bufferSize);
                    }
                }
                catch (Exception ex) { Console.WriteLine(ex.ToString()); }
                /* Resource Cleanup */
                localFileStream.Close();
                ftpStream.Close();
                ftpRequest = null;
            }
            catch (Exception ex) 
            {
                if (localFileStream != null)
                    localFileStream.Close();
                if (ftpStream != null)
                    ftpStream.Close();
                if (ftpRequest != null)
                    ftpRequest = null;

                Console.WriteLine(ex.ToString()); //obnoxious 
            }
            return;
        }

        public void delete(string fileName)
        {
            try
            {
                /* Create an FTP Request */
                ftpRequest = (FtpWebRequest)FtpWebRequest.Create(String.Format(baseUrl, fileName));
                /* Log in to the FTP Server with the User Name and Password Provided */
                ftpRequest.Credentials = new NetworkCredential(userName, password);

                ftpRequest.UseBinary = true; //because Windows
                ftpRequest.UsePassive = true; //when in doubt...
                ftpRequest.KeepAlive = true; //when in doubt...
                ftpRequest.EnableSsl = true;
                //specify that we're deleting.
                ftpRequest.Method = WebRequestMethods.Ftp.DeleteFile;

                ftpResponse = (FtpWebResponse)ftpRequest.GetResponse();
                //Cleanup
                ftpResponse.Close();
                ftpRequest = null;
            }
            catch (Exception ex) 
            {
                //Clean up even though we failed
                if (ftpRequest != null)
                    ftpRequest = null;
                if (ftpResponse != null)
                    ftpResponse.Close();

                Console.WriteLine(ex.ToString()); //This is obnoxious.
            }
            return;
        }

        public void rename(string oldNameAndPath, string newFileName)
        {
            try
            {
                ftpRequest = (FtpWebRequest)WebRequest.Create(String.Format(baseUrl, oldNameAndPath));

                ftpRequest.Credentials = new NetworkCredential(userName, password);
                ftpRequest.UseBinary = true;
                ftpRequest.UsePassive = true;
                ftpRequest.KeepAlive = true;
                ftpRequest.EnableSsl = true;

                ftpRequest.Method = WebRequestMethods.Ftp.Rename;

                ftpRequest.RenameTo = newFileName;

                ftpResponse = (FtpWebResponse)ftpRequest.GetResponse();

                ftpResponse.Close();
                ftpRequest = null;
            }
            catch (Exception ex)
            {
                if (ftpResponse != null)
                    ftpResponse.Close();
                if (ftpRequest != null)
                    ftpRequest = null;

                Console.WriteLine("An error occurred in Rename\n" + ex.ToString());
            }
            return;
        }

        public void makeDir(string newDir)
        {
            try
            {
                ftpRequest = (FtpWebRequest)WebRequest.Create(String.Format(baseUrl, newDir));
                ftpRequest.Credentials = new NetworkCredential(userName, password);
                ftpRequest.UseBinary = true;
                ftpRequest.UsePassive = true;
                ftpRequest.KeepAlive = true;
                ftpRequest.EnableSsl = true;
                ftpRequest.Method = WebRequestMethods.Ftp.MakeDirectory;
                ftpResponse = (FtpWebResponse)ftpRequest.GetResponse();
                ftpResponse.Close();
                ftpRequest = null;
            }
            catch (WebException)
            {
                Console.WriteLine("Error in making directory. Directory may already exist.");
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error in makeDir.\n" + ex.ToString());
                if (ftpResponse != null)
                    ftpResponse.Close();
                if (ftpRequest != null)
                    ftpRequest = null;
            }
        }

        public DateTime getCreationDateTime(string fileName)
        {
            try
            {
                /* Create an FTP Request */
                ftpRequest = (FtpWebRequest)FtpWebRequest.Create(String.Format(baseUrl,fileName));
                /* Log in to the FTP Server with the User Name and Password Provided */
                ftpRequest.Credentials = new NetworkCredential(userName, password);
                /* When in doubt, use these options */
                ftpRequest.UseBinary = true;
                ftpRequest.UsePassive = true;
                ftpRequest.KeepAlive = true;
                ftpRequest.EnableSsl = true;
                /* Specify the Type of FTP Request */
                ftpRequest.Method = WebRequestMethods.Ftp.GetDateTimestamp;
                /* Establish Return Communication with the FTP Server */
                ftpResponse = (FtpWebResponse)ftpRequest.GetResponse();

                ftpResponse.Close();
                ftpRequest = null;
                /* Return File Created Date Time */
                return ftpResponse.LastModified;
            }
            catch (Exception ex)
            { 
                Console.WriteLine(ex.ToString()); 
            }
            /* Return an Empty string Array if an Exception Occurs */
            return DateTime.MinValue;
        }

        public long getFileSize(string fileName)
        {
            try
            {
                ftpRequest = (FtpWebRequest)WebRequest.Create(String.Format(baseUrl, fileName));
                ftpRequest.Credentials = new NetworkCredential(userName, password);
                ftpRequest.UseBinary = true;
                ftpRequest.UsePassive = true;
                ftpRequest.KeepAlive = true;
                ftpRequest.EnableSsl = true;

                ftpRequest.Method = WebRequestMethods.Ftp.GetFileSize;

                ftpResponse = (FtpWebResponse)ftpRequest.GetResponse();


                
                ftpResponse.Close();
                ftpRequest = null;
                
                return ftpResponse.ContentLength;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
            }
            return -1;
        }

        private string[] lsSimple(string dir)
        {
            try
            {
                ftpRequest = (FtpWebRequest)WebRequest.Create(String.Format(baseUrl, dir));
                ftpRequest.Credentials = new NetworkCredential(userName, password);
                ftpRequest.UseBinary = true;
                ftpRequest.UsePassive = true;
                ftpRequest.KeepAlive = true;
                ftpRequest.EnableSsl = true;

                ftpRequest.Method = WebRequestMethods.Ftp.ListDirectory;

                ftpResponse = (FtpWebResponse)ftpRequest.GetResponse();

                ftpStream = ftpResponse.GetResponseStream();

                StreamReader ftpReader = new StreamReader(ftpStream);

                StringBuilder result = new StringBuilder();
                try
                {
                    string line = ftpReader.ReadLine();
                    while (line != null)
                    {
                        result.Append(line);
                        result.Append("\n");
                        line = ftpReader.ReadLine();
                    }
                    //remove extra \n
                    result.Remove(result.ToString().LastIndexOf('\n'), 1);
                    ftpReader.Close();
                    ftpStream.Close();
                    ftpResponse.Close();
                    ftpRequest = null;
                    return result.ToString().Split('\n');
                }
                catch (Exception ex)
                {
                    Console.WriteLine("Error reading file list.\n" + ex.ToString());
                    if (result != null)
                        result = null;
                    if (ftpReader != null)
                        ftpReader.Close();
                    if (ftpResponse != null)
                        ftpResponse.Close();
                    if (ftpRequest != null)
                        ftpRequest = null;
                    return new string[] { "" };
                }
            }
            

            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
            }

            return new string[] { "" };
        }

        private string[] lsDetailed(string dir)
        {
            try
            {
                ftpRequest = (FtpWebRequest)WebRequest.Create(String.Format(baseUrl, dir));
                ftpRequest.Credentials = new NetworkCredential(userName, password);
                ftpRequest.UseBinary = true;
                ftpRequest.UsePassive = true;
                ftpRequest.KeepAlive = true;
                ftpRequest.EnableSsl = true;

                ftpRequest.Method = WebRequestMethods.Ftp.ListDirectoryDetails;

                ftpResponse = (FtpWebResponse)ftpRequest.GetResponse();

                ftpStream = ftpResponse.GetResponseStream();

                StreamReader ftpReader = new StreamReader(ftpStream);

                StringBuilder result = new StringBuilder();
                try
                {
                    string line = ftpReader.ReadLine();
                    while (line != null)
                    {
                        result.Append(line);
                        result.Append("\n");
                        line = ftpReader.ReadLine();
                    }
                    //remove extra \n
                    result.Remove(result.ToString().LastIndexOf('\n'), 1);
                    ftpReader.Close();
                    ftpStream.Close();
                    ftpResponse.Close();
                    ftpRequest = null;
                    return result.ToString().Split('\n');
                }
                catch (Exception ex)
                {
                    Console.WriteLine("Error reading file list.\n" + ex.ToString());
                    if (result != null)
                        result = null;
                    if (ftpReader != null)
                        ftpReader.Close();
                    if (ftpResponse != null)
                        ftpResponse.Close();
                    if (ftpRequest != null)
                        ftpRequest = null;
                    return new string[] { "" };
                }
            }


            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
            }

            return new string[] { "" };
        }

        public string pwdSimple(string dir)
        {
            string result = "";
            List<string> sorted = lsSimple(dir).ToList<string>();
            sorted.Sort();
            foreach(string s in sorted)
            {
                result += s;
                result += "\n";
            }
            return result;
        }

        public string pwdDetailed(string dir)
        {
            string result = "";
            foreach (string s in lsDetailed(dir))
            {
                result += s;
                result += "\n";
            }
            return result;
        }

        public IEnumerable<FTPFile> getSubDirsAndFiles(string dir)
        {
            string[] filesAndDirs = lsDetailed(dir);
            List<FTPFile> files = new List<FTPFile>();

            foreach(string fileOrDir in filesAndDirs)
            {
                files.Add(FTPFile.parseRecordString(new Uri(baseUrl), fileOrDir));
            }
            return files;                
        }

    }
}
