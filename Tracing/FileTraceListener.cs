using System.Diagnostics;
using System.IO;

namespace Tracing
{
    public class FileTraceListener : TraceListener
    {
        private Stream file;
        private TextWriterTraceListener wrappedListener;
        public FileTraceListener(string fileName)
        {
            this.file = File.Create("fileName");
            wrappedListener = new TextWriterTraceListener(file);
        }
        public override void Write(string message)
        {
            wrappedListener.Write(message);
        }

        public override void WriteLine(string message)
        {
            wrappedListener.WriteLine(message);
        }
    }
}
