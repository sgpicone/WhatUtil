using System;

namespace WhatDownload
{
    public class ErrorEventArgs : EventArgs
    {
        public Exception ErrorException { get; set; }
    }
}
