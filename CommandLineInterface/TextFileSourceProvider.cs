using Library.Logic.ViewModel;

namespace CommandLineInterface
{
    internal class TextFileSourceProvider : ISourceProvider
    {
        string path;
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
            return this.path;
        }
    }
}