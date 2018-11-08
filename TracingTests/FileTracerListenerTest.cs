using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Tracing;

namespace TracingTests
{
    [TestClass]
    public class FileTracerListenerTest
    {
        [TestMethod]
        public void FileExistsAfterTrace()
        {
            FileTraceListener ftl = new FileTraceListener("./test.txt");
            ftl.Write("test");
            ftl.Flush();
            Assert.IsTrue(System.IO.File.Exists("./text.txt"));
        }
    }
}
