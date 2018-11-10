using System.ComponentModel.Composition;
using System.Configuration;
using System.Diagnostics;
using System.IO;

namespace Tracing
{
    [Export(typeof(TraceListener))]
    public class FileTraceListener : TraceListener
    {
        private Stream file;
        private TextWriterTraceListener wrappedListener;
        public FileTraceListener() : this(ConfigurationManager.AppSettings["FileName"])
        {
        }
        public FileTraceListener(string fileName)
        {
            this.file = File.Create(fileName);
            wrappedListener = new TextWriterTraceListener(file);
        }
        public FileTraceListener(Stream stream)
        {
            wrappedListener = new TextWriterTraceListener(stream);
        }
        public override void Write(string message)
        {
            wrappedListener.Write(message);
        }

        public override void WriteLine(string message)
        {
            wrappedListener.WriteLine(message);
        }
        public override void Flush()
        {
            wrappedListener.Flush();
        }
    }
}
