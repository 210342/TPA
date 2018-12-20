using Library.Logic.ViewModel;
using System;
using System.IO;

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
            return !File.Exists(this.path) || (File.Exists(path) && File.Open(path, FileMode.Open) != null);
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