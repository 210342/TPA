using System.IO;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace SemanticTracing.Tests
{
    [TestClass]
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

        [TestMethod]
        public void FileSemanticTracingTest()
        {
            Assert.IsNotNull(_systemUnderTest);
            Assert.IsFalse(string.IsNullOrEmpty(_systemUnderTest.FilePath));
            Assert.IsTrue(File.Exists(_systemUnderTest.FilePath));
        }

        [TestMethod]
        public void LogFailureTest()
        {
            fileInfo.Refresh();
            long oldLength = fileInfo.Length;
            _systemUnderTest.LogFailure("FAIL");
            _systemUnderTest.Flush();
            fileInfo.Refresh();
            Assert.IsTrue(oldLength < fileInfo.Length);
        }

        [TestMethod]
        public void LogSuccessTest()
        {
            fileInfo.Refresh();
            long oldLength = fileInfo.Length;
            _systemUnderTest.LogFailure("SUCCESS");
            _systemUnderTest.Flush();
            fileInfo.Refresh();
            Assert.IsTrue(oldLength < fileInfo.Length);
        }

        [TestMethod]
        public void LogLoadingModelTest()
        {
            fileInfo.Refresh();
            long oldLength = fileInfo.Length;
            _systemUnderTest.LogLoadingModel("loading model");
            _systemUnderTest.Flush();
            fileInfo.Refresh();
            Assert.IsTrue(oldLength < fileInfo.Length);
        }

        [TestMethod]
        public void LogModelLoadedTest()
        {
            fileInfo.Refresh();
            long oldLength = fileInfo.Length;
            _systemUnderTest.LogModelLoaded("model loaded");
            _systemUnderTest.Flush();
            fileInfo.Refresh();
            Assert.IsTrue(oldLength < fileInfo.Length);
        }

        [TestMethod]
        public void LogModelSavedTest()
        {
            fileInfo.Refresh();
            long oldLength = fileInfo.Length;
            _systemUnderTest.LogModelSaved("model saved");
            _systemUnderTest.Flush();
            fileInfo.Refresh();
            Assert.IsTrue(oldLength < fileInfo.Length);
        }

        [TestMethod]
        public void LogSavingModelTest()
        {
            fileInfo.Refresh();
            long oldLength = fileInfo.Length;
            _systemUnderTest.LogSavingModel("saving model");
            _systemUnderTest.Flush();
            fileInfo.Refresh();
            Assert.IsTrue(oldLength < fileInfo.Length);
        }

        [TestMethod]
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