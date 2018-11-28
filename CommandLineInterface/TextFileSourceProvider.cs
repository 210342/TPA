using Library.Logic.ViewModel;
using System;

namespace CommandLineInterface
{
    internal class TextFileSourceProvider : ISourceProvider
    {
        string path;

        internal TextFileSourceProvider() { }

        internal TextFileSourceProvider(string dllPath)
        {
            this.path = dllPath;
        }

        public bool GetAccess()
        {
            return System.IO.File.Exists(this.path);
        }

        public string GetFilePath()
        {
            if(string.IsNullOrEmpty(path))
            {
                Console.WriteLine("Please provide path to the file");
                return Console.ReadLine();
            }
            else
                return path;
        }
    }
}