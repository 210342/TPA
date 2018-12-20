using Microsoft.VisualStudio.TestTools.UnitTesting;
using FileSemanticTracing;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Tracing;
using System.IO;
using System.Threading;

namespace FileSemanticTracing.Tests
{
    [TestClass()]
    public class FileSemanticTracingTests
    {
        private FileSemanticTracing _systemUnderTest;
        private FileInfo fileInfo;

        [TestInitialize]
        public void Startup()
        {
            _systemUnderTest = new FileSemanticTracing();
            fileInfo = new FileInfo(_systemUnderTest.FilePath);
        }

        [TestCleanup]
        public void TearDown()
        {
            _systemUnderTest.Dispose();
        }

        [TestMethod()]
        public void FileSemanticTracingTest()
        {
            Assert.IsNotNull(_systemUnderTest);
            Assert.IsFalse(string.IsNullOrEmpty(_systemUnderTest.FilePath));
            Assert.IsTrue(File.Exists(_systemUnderTest.FilePath));
        }

        [TestMethod()]
        public void LogDatabaseConnectionClosedTest()
        {
            fileInfo.Refresh();
            long oldLength = fileInfo.Length;
            _systemUnderTest.LogDatabaseConnectionClosed("dbCloseTest");
            _systemUnderTest.Flush();
            fileInfo.Refresh();
            Assert.IsTrue(oldLength < fileInfo.Length);
        }

        [TestMethod()]
        public void LogDatabaseConnectionEstablishedTest()
        {
            fileInfo.Refresh();
            long oldLength = fileInfo.Length;
            _systemUnderTest.LogDatabaseConnectionEstablished("dbOpenTest");
            _systemUnderTest.Flush();
            fileInfo.Refresh();
            Assert.IsTrue(oldLength < fileInfo.Length);
        }

        [TestMethod()]
        public void LogFailureTest()
        {
            fileInfo.Refresh();
            long oldLength = fileInfo.Length;
            _systemUnderTest.LogFailure("FAIL");
            _systemUnderTest.Flush();
            fileInfo.Refresh();
            Assert.IsTrue(oldLength < fileInfo.Length);
        }

        [TestMethod()]
        public void LogFileClosedTest()
        {
            fileInfo.Refresh();
            long oldLength = fileInfo.Length;
            _systemUnderTest.LogFileClosed("file closed");
            _systemUnderTest.Flush();
            fileInfo.Refresh();
            Assert.IsTrue(oldLength < fileInfo.Length);
        }

        [TestMethod()]
        public void LogFileOpenedTest()
        {
            fileInfo.Refresh();
            long oldLength = fileInfo.Length;
            _systemUnderTest.LogFileOpened("file open");
            _systemUnderTest.Flush();
            fileInfo.Refresh();
            Assert.IsTrue(oldLength < fileInfo.Length);
        }

        [TestMethod()]
        public void LogLoadingModelTest()
        {
            fileInfo.Refresh();
            long oldLength = fileInfo.Length;
            _systemUnderTest.LogLoadingModel("loading model");
            _systemUnderTest.Flush();
            fileInfo.Refresh();
            Assert.IsTrue(oldLength < fileInfo.Length);
        }

        [TestMethod()]
        public void LogModelLoadedTest()
        {
            fileInfo.Refresh();
            long oldLength = fileInfo.Length;
            _systemUnderTest.LogModelLoaded("model loaded");
            _systemUnderTest.Flush();
            fileInfo.Refresh();
            Assert.IsTrue(oldLength < fileInfo.Length);
        }

        [TestMethod()]
        public void LogModelSavedTest()
        {
            fileInfo.Refresh();
            long oldLength = fileInfo.Length;
            _systemUnderTest.LogModelSaved("model saved");
            _systemUnderTest.Flush();
            fileInfo.Refresh();
            Assert.IsTrue(oldLength < fileInfo.Length);
        }

        [TestMethod()]
        public void LogSavingModelTest()
        {
            fileInfo.Refresh();
            long oldLength = fileInfo.Length;
            _systemUnderTest.LogSavingModel("saving model");
            _systemUnderTest.Flush();
            fileInfo.Refresh();
            Assert.IsTrue(oldLength < fileInfo.Length);
        }

        [TestMethod()]
        public void LogStartupTest()
        {
            FileInfo file = new FileInfo(_systemUnderTest.FilePath);
            long oldLength = file.Length;
            _systemUnderTest.LogStartup();
            _systemUnderTest.Flush();
            file.Refresh();
            Assert.AreNotEqual(oldLength, file.Length);
        }
    }
}