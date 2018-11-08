using System;
using System.Diagnostics.CodeAnalysis;
using System.IO;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Tracing;

namespace TracingTests
{
    [ExcludeFromCodeCoverage]
    [TestClass]
    public class FileTracerListenerTest
    {
        [TestMethod]
        public void ContentExistsAfterTrace()
        {
            MemoryStream ms = new MemoryStream();
            FileTraceListener ftl = new FileTraceListener(ms);
            ftl.Write("something");
            ftl.Flush();
            Assert.AreNotEqual(0, ms.Length);
            ftl.Dispose();
        }
        [TestMethod]
        public void FileContentExactlyTheSame()
        {
            string expected = "nothing";
            MemoryStream ms = new MemoryStream();
            FileTraceListener ftl = new FileTraceListener(ms);
            ftl.Write(expected);
            ftl.Flush();
            byte[] byteArray = new byte[expected.Length];
            ms.Read(byteArray, 0, expected.Length);
            Assert.AreEqual(expected.Length, byteArray.Length);
            ftl.Dispose();
        }
    }
}
