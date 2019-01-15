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
            try
            {
                FileStream stream = File.Open(path, FileMode.Open);
                stream?.Close();
                stream?.Dispose();
                return true;
            }
            catch (IOException)
            {
                return false;
            }
            catch (ArgumentNullException)
            {
                // if path is null, return true and ask for a new path
                return true;
            }
        }

        public string GetPath()
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