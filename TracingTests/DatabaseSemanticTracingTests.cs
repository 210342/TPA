using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.IO;
using DB = DatabaseSemanticTracing;

namespace FileSemanticTracing.Tests
{
    [TestClass()]
    public class DatabaseSemanticTracingTests
    {
        private DB.DatabaseSemanticTracing _systemUnderTest;

        [TestInitialize]
        public void Startup()
        {
            _systemUnderTest = new DB.DatabaseSemanticTracing();
        }

        [TestCleanup]
        public void TearDown()
        {
            _systemUnderTest.Dispose();
        }

        //[TestMethod()]
        public void DatabaseSemanticTracingTest()
        {
            Assert.IsNotNull(_systemUnderTest);
        }
        //[TestMethod()]
        public void LogFailureTest()
        {
            
            _systemUnderTest.LogFailure("FAIL");
            _systemUnderTest.Flush();
            //Assert.IsTrue(oldLength < fileInfo.Length);
        }
        
        //[TestMethod()]
        public void LogSuccessTest()
        {
            _systemUnderTest.LogSuccess("SUCCESS");
            _systemUnderTest.Flush();
            //Assert.IsTrue(oldLength < fileInfo.Length);
        }
        /*
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
        */
    }
}