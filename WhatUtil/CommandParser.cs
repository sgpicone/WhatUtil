using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WhatDownload
{
    class CommandParser
    {
        private CommandWords commands;

        public CommandParser()
        {
            commands = new CommandWords();
        }

        public Command getCommand()
        {
            string input = "";
            string command;
            string[] args;

            Console.Write("WhatUtil >>> ");
            try
            {
                input = Console.ReadLine();
            }
            catch (IOException ex)
            {
                Console.WriteLine("Error while reading: " + ex.Message);
            }

            List<string> wordsLine = input.Split(' ').ToList<String>() as List<string>;
            command = wordsLine[0];
            if (wordsLine.Count > 1)
                args = wordsLine.Skip(1).ToArray<string>();
            else
                args = null;

            if (commands.isCommand(command))
                return new Command(command, args);
            else
                return new Command(null, args);
        }

        public void showCommands()
        {
            commands.showAll();
        }            
    }
}
