using System;
using System.Collections.Generic;
using System.IO;
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
                FileStream fileStream = null;
                try
                {
                    fileStream = File.OpenRead(args[0]);
                }
                catch(FileNotFoundException)
                {
                    Console.WriteLine("File with given path doesn't exist");
                }
                catch (DirectoryNotFoundException e)
                {
                    Console.WriteLine("Couldn't find a directory \n{0}", e.Message);
                }
                catch (UnauthorizedAccessException)
                {
                    Console.WriteLine("Insufficient access rights to read the file");
                }
                catch(Exception e)
                {
                    Console.WriteLine("Unknown problem occured. \n{0}", e.Message);
                }
                finally
                {
                    if(fileStream != null)
                        fileStream.Close();
                }
            }
            else
            {
                Console.WriteLine("Please give a correct path to a .dll");
            }
            Console.ReadKey();
        }
    }
}
