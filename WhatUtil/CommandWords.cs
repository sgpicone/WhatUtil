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

        public CommandWords()
        { }

        public bool isCommand(string command)
        {
            return (validWords.Contains(command));
        }

        public void showAll()
        {
            foreach (string word in validWords)
                Console.Write(word + " ");
            Console.WriteLine();
        }
    }
}
