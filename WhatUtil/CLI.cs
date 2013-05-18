using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WhatDownload
{
    /// <summary>
    /// CLI for the WhatUtil
    /// </summary>
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
            Console.WriteLine("\t\tWhatUtil - The Whatbox Downloader Utility\n");
            Console.WriteLine("\t\t\tNot affiliated with Whatbox.\n");
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
                case "cls": cls(); break;
                default:
                    Console.WriteLine("Do what now?");
                    break;
            }
            return end;
        }

        private void ls(Command c)
        {
            if (c.getArgs() == null || c.getArgs().Length != 1)
            {
                Console.WriteLine("Usage: ls <directory path>");
            }
            else
            {
                Console.WriteLine(c.getArgs()[0]);
                Console.WriteLine(
                    ftp.pwdSimple(c.getArgs()[0])
                    );
            }
        }

        private void lsd(Command c)
        {
            if (c.getArgs() == null || c.getArgs().Length != 1)
            {
                Console.WriteLine("Usage: lsd <directory path>");
            }
            else
            {
                Console.WriteLine(c.getArgs()[0]);
                Console.WriteLine(
                    ftp.pwdDetailed(c.getArgs()[0])
                    );
            }
        }

        private void help()
        {
            Console.WriteLine("Your commands are:\n");
            parser.showCommands();
            parser.showHelp();
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
            if (c.getArgs() == null || c.getArgs().Length != 2)
            {
                Console.WriteLine("Usage: dl <remote file> <local file>");
            }
            else
            {
                Console.WriteLine("I'd download that file if I was implemented yet.");
            }

        }

        private void ul(Command c)
        {
            if (c.getArgs() == null || c.getArgs().Length != 2)
            {
                Console.WriteLine("Usage: ul <local file> <remote file>");
            }
            else
            {
                Console.WriteLine("I'd upload that file if I was implemented yet.");
            }
        }

        private void rm(Command c)
        {
            if(c.getArgs() == null || c.getArgs().Length != 1)
            {
                Console.WriteLine("Usage: rm <remote file>");
            }
            else
            {
                Console.WriteLine("I'd remove that file if I was implemented yet.");
            }
        }

        private void mkdir(Command c)
        {
            if(c.getArgs() == null || c.getArgs().Length != 1)
            {
                Console.WriteLine("Usage: mkdir <remote directory>");
            }
            else
            {
                Console.WriteLine("I'd make that directory if I was implemented yet.");
            }
        }

        private void mv(Command c)
        {
            if (c.getArgs() == null || c.getArgs().Length != 2)
            {
                Console.WriteLine("Usage: mv <old filename> <new filename>");
            }
            else
            {
                Console.WriteLine("I'd rename that file if I was implemented yet.");
            }
        }

        private void autodl(Command c)
        {
            Console.WriteLine("Oh God, that's not even close to done yet.");
        }

        private void cls()
        {
            Console.Clear();
        }
    }
}
