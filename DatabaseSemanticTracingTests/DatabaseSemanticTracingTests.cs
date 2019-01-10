using System.Data.SqlClient;
using System.Linq;
using System.Transactions;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace DatabaseSemanticTracing.Tests
{
    [TestClass]
    public class DatabaseSemanticTracingTests
    {
        private DatabaseSemanticTracing _sut;

        [TestInitialize]
        public void SetUp()
        {
            _sut = new DatabaseSemanticTracing();
        }

        [TestCleanup]
        public void CleanUp()
        {
            _sut.Dispose();
        }


        [TestMethod]
        public void DatabaseSemanticTracingTest()
        {
            Assert.IsNotNull(_sut);
        }

        [TestMethod]
        public void LogStartupTest()
        {
            using (var scope = new TransactionScope())
            {
                var logsQuantity = CheckLogsQuantity();
                _sut.LogStartup();
                _sut.Flush();
                Assert.AreEqual(logsQuantity + 1, CheckLogsQuantity());
                scope.Dispose();
            }
        }

        [TestMethod]
        public void LogSuccessTest()
        {
            using (var scope = new TransactionScope())
            {
                CallGivenMethod("LogSuccess");
                scope.Dispose();
            }
        }

        [TestMethod]
        public void LogFailureTest()
        {
            using (var scope = new TransactionScope())
            {
                CallGivenMethod("LogFailure");
                scope.Dispose();
            }
        }

        [TestMethod]
        public void LogLoadingModelTest()
        {
            using (var scope = new TransactionScope())
            {
                CallGivenMethod("LogLoadingModel");
                scope.Dispose();
            }
        }

        [TestMethod]
        public void LogModelLoadedTest()
        {
            using (var scope = new TransactionScope())
            {
                CallGivenMethod("LogModelLoaded");
                scope.Dispose();
            }
        }

        [TestMethod]
        public void LogModelSavedTest()
        {
            using (var scope = new TransactionScope())
            {
                CallGivenMethod("LogModelSaved");
                scope.Dispose();
            }
        }

        [TestMethod]
        public void LogSavingModelTest()
        {
            using (var scope = new TransactionScope())
            {
                CallGivenMethod("LogSavingModel");
                scope.Dispose();
            }
        }

        private void CallGivenMethod(string methodName)
        {
            var logsQuantity = CheckLogsQuantity();
            var methodToCall = (from method in _sut.GetType().GetMethods()
                where method.Name.Equals(methodName)
                select method).First();
            methodToCall.Invoke(_sut, new object[] {"TEST METHOD"});
            _sut.Flush();
            Assert.AreEqual(logsQuantity + 1, CheckLogsQuantity());
        }

        private int CheckLogsQuantity()
        {
            int quantity;
            using (var connection = new SqlConnection(_sut.ConnectionString))
            {
                connection.Open();
                var transaction = connection.BeginTransaction();
                using (var command = new SqlCommand("SELECT COUNT(*) FROM dbo.Traces", connection, transaction))
                {
                    quantity = (int) command.ExecuteScalar();
                }

                connection.Close();
            }

            return quantity;
        }
    }
}