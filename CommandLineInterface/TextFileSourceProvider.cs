using System;
using System.IO;
using Library.Logic.ViewModel;

namespace CommandLineInterface
{
    internal class TextFileSourceProvider : ISourceProvider
    {
        private readonly string path;

        internal TextFileSourceProvider()
        {
        }

        internal TextFileSourceProvider(string dllPath)
        {
            path = dllPath;
        }

        public bool GetAccess()
        {
            return !File.Exists(path) || File.Exists(path) && File.Open(path, FileMode.Open) != null;
        }

        public string GetFilePath()
        {
            if (string.IsNullOrEmpty(path))
            {
                Console.WriteLine("Please provide path to the file");
                return Console.ReadLine();
            }

            return path;
        }
    }
}