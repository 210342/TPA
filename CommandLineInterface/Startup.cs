using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CommandLineInterface
{
    class Startup
    {
        private readonly string _filename;
        static void Main(string[] args)
        {
            if(args.Count() > 0)
            {
                
            }
            else
            {
                Console.WriteLine("Please give a correct path to a .dll");
            }
        }
    }
}
