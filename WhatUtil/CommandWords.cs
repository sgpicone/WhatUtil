using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WhatDownload
{
    class CommandWords
    {
        private static string[] validWords = {
            "autodl", "ls", "lsd", "dl", "ul", "rm", "mkdir", "mv", "help", "quit"
                                            };

        private static string[] definitions = {
            "Downloads all files from a specified remote directory to a specified local directory",
            "Prints a simple file/directory listing of a specified directory",
            "Prints a detailed file/directory listing of a specified directory",
            "Downloads a specified remote file/directory to a specified local directory",
            "Uploads a specified local file to a specified remote file",
            "Removes a specified remote file",
            "Makes a specified remote directory",
            "Renames a specified remote file to another remote file",
            "Prints help.",
            "Quits WhatUtil"
                                              };

        private static Dictionary<string, string> commandDefinitions = new Dictionary<string, string>();
        public CommandWords()
        {
            for (int i = 0; i < validWords.Length; i++)
            {
                commandDefinitions.Add(validWords[i], definitions[i]);
            }
        }

        public bool isCommand(string command)
        {
            return (validWords.Contains(command));
        }

        public void showAll()
        {
            foreach (string word in validWords)
                Console.Write(word + " ");
            Console.WriteLine("\n");
        }

        public void showDefinitions()
        {
            foreach (KeyValuePair<string, string> kvp in commandDefinitions)
            {
                Console.WriteLine(kvp.Key + "\t\t" + kvp.Value);
            }
            Console.WriteLine("\n");
        }
    }
}
