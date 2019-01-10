using System.Data.SqlClient;
using System.Linq;
using System.Reflection;
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
            using (TransactionScope scope = new TransactionScope())
            {
                int logsQuantity = CheckLogsQuantity();
                _sut.LogStartup();
                _sut.Flush();
                Assert.AreEqual(logsQuantity + 1, CheckLogsQuantity());
                scope.Dispose();
            }
        }

        [TestMethod]
        public void LogSuccessTest()
        {
            using (TransactionScope scope = new TransactionScope())
            {
                CallGivenMethod("LogSuccess");
                scope.Dispose();
            }
        }

        [TestMethod]
        public void LogFailureTest()
        {
            using (TransactionScope scope = new TransactionScope())
            {
                CallGivenMethod("LogFailure");
                scope.Dispose();
            }
        }

        [TestMethod]
        public void LogLoadingModelTest()
        {
            using (TransactionScope scope = new TransactionScope())
            {
                CallGivenMethod("LogLoadingModel");
                scope.Dispose();
            }
        }

        [TestMethod]
        public void LogModelLoadedTest()
        {
            using (TransactionScope scope = new TransactionScope())
            {
                CallGivenMethod("LogModelLoaded");
                scope.Dispose();
            }
        }

        [TestMethod]
        public void LogModelSavedTest()
        {
            using (TransactionScope scope = new TransactionScope())
            {
                CallGivenMethod("LogModelSaved");
                scope.Dispose();
            }
        }

        [TestMethod]
        public void LogSavingModelTest()
        {
            using (TransactionScope scope = new TransactionScope())
            {
                CallGivenMethod("LogSavingModel");
                scope.Dispose();
            }
        }

        private void CallGivenMethod(string methodName)
        {
            int logsQuantity = CheckLogsQuantity();
            MethodInfo methodToCall = (from method in _sut.GetType().GetMethods()
                where method.Name.Equals(methodName)
                select method).First();
            methodToCall.Invoke(_sut, new object[] {"TEST METHOD"});
            _sut.Flush();
            Assert.AreEqual(logsQuantity + 1, CheckLogsQuantity());
        }

        private int CheckLogsQuantity()
        {
            int quantity;
            using (SqlConnection connection = new SqlConnection(_sut.ConnectionString))
            {
                connection.Open();
                SqlTransaction transaction = connection.BeginTransaction();
                using (SqlCommand command = new SqlCommand("SELECT COUNT(*) FROM dbo.Traces", connection, transaction))
                {
                    quantity = (int) command.ExecuteScalar();
                }

                connection.Close();
            }

            return quantity;
        }
    }
}