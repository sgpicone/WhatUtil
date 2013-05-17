using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WhatDownload
{
    class Command
    {
        private string commandWord;

        private string[] args;

        public Command(string command, string[] args)
        {
            commandWord = command;
            this.args = args;
        }

        public string getCommand()
        {
            return commandWord;
        }

        public string[] getArgs()
        {
            return args;
        }

        public bool isUnknown()
        {
            return (commandWord == null);
        }

        public bool hasArgs()
        {
            return (args != null);
        }
    }
}
