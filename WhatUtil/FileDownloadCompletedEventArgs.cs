using System;
using System.IO;

namespace WhatDownload
{
    public class FileDownloadCompletedEventArgs : EventArgs
    {
        public Uri ServerPath { get; set; }
        public FileInfo LocalFile { get; set; }
    }
}
