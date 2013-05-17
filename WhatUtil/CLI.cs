using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WhatDownload
{
    class CLI
    {
        private CommandParser parser;
        private FTP ftp;

        public CLI(string url, string user, string pass)
        {
            parser = new CommandParser();
            ftp = new FTP(url, user, pass);
        }

        public void run()
        {
            printWelcome();

            bool done = false;
            while (!done)
            {
                Command command = parser.getCommand();
                done = processCommand(command);
            }
            Console.WriteLine("Goodbye.");
        }

        public void printWelcome()
        {
            Console.WriteLine("WhatUtil - The Whatbox Downloader Utility");
            Console.WriteLine("Not affiliated with Whatbox.");
            Console.WriteLine("Your commands are:");
            parser.showCommands();
        }

        private bool processCommand(Command command)
        {
            bool end = false;

            if (command.isUnknown())
            {
                Console.WriteLine("Do what now?");
                return false;
            }

            string commandWord = command.getCommand();
            switch (commandWord)
            {
                case "ls": ls(command); break;
                case "lsd": lsd(command); break;
                case "help": help(); break;
                case "quit": end = quit(command); break;
                case "dl": dl(command); break;
                case "ul": ul(command); break;
                case "rm": rm(command); break;
                case "mkdir": mkdir(command); break;
                case "mv": mv(command); break;
                case "autodl": autodl(command); break;
                default:
                    Console.WriteLine("Do what now?");
                    break;
            }
            return end;
        }

        private void ls(Command c)
        {
            if (c.getArgs() == null)
            {
                Console.WriteLine("Need an argument.");
            }
            else
            {
                //Console.WriteLine(c.getArgs()[0]);
                ftp.pwdSimple(c.getArgs()[0]);
            }
        }

        private void lsd(Command c)
        {
        }

        private void help()
        {
        }

        private bool quit(Command c)
        {
            if (c.getArgs() != null)
            {
                Console.WriteLine("Quit what?");
                return false;
            }
            return true;
        }

        private void dl(Command c)
        {
        }

        private void ul(Command c)
        {
        }

        private void rm(Command c)
        {
        }

        private void mkdir(Command c)
        {
        }

        private void mv(Command c)
        {
        }

        private void autodl(Command c)
        {
        }
    }
}
