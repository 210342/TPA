﻿using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CommandLineInterface
{
    class Startup
    {
        static void Main(string[] args)
        {
            CommandLineInterface cli = new CommandLineInterface();
            cli.Start("");
            /*
            if(args.Count() > 0)
            {
                CommandLineInterface cli = new CommandLineInterface();
                cli.Start(args[0]); // args[0] is supposed to be a .dll path
            }
            else
            {
                Console.WriteLine("Please give a correct path to a .dll");
            }*/
        }
    }
}
