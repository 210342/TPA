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
            bool value = !File.Exists(path);
            if(!value)
            {
                FileStream stream = File.Open(path, FileMode.Open);
                value = stream != null;
                stream?.Close();
                stream?.Dispose();
            }
            return value;
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