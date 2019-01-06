using Microsoft.VisualStudio.TestTools.UnitTesting;
using DatabaseSemanticTracing;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.SqlClient;
using System.Reflection;
using System.Threading;

namespace DatabaseSemanticTracing.Tests
{
    [TestClass()]
    public class DatabaseSemanticTracingTests
    {
        private DatabaseSemanticTracing _sut;

        [TestInitialize]
        public void SetUp()
        {
            _sut = new DatabaseSemanticTracing();
        }

        [TestMethod()]
        public void DatabaseSemanticTracingTest()
        {
            Assert.IsNotNull(_sut);
        }

        [TestMethod]
        public void LogStartupTest()
        {
            using (var connection = new SqlConnection(_sut.ConnectionString))
            {
                connection.Open();
                SqlTransaction transaction = connection.BeginTransaction();
                int logsQuantity = CheckLogsQuantity();
                _sut.LogStartup();
                _sut.Dispose();
                int logsQuantityAfter = CheckLogsQuantity();
                Assert.AreEqual(logsQuantity + 1, logsQuantityAfter);
                RemoveLastEntry();
                connection.Close();
            }
        }

        [TestMethod]
        public void LogSuccessTest()
        {
            CallGivenMethod("LogSuccess");
        }

        [TestMethod]
        public void LogFailureTest()
        {
            CallGivenMethod("LogFailure");
        }

        [TestMethod]
        public void LogLoadingModelTest()
        {
            CallGivenMethod("LogLoadingModel");
        }

        [TestMethod]
        public void LogModelLoadedTest()
        {
            CallGivenMethod("LogModelLoaded");
        }

        [TestMethod]
        public void LogModelSavedTest()
        {
            CallGivenMethod("LogModelSaved");
        }

        [TestMethod]
        public void LogSavingModelTest()
        {
            CallGivenMethod("LogSavingModel");
        }

        private void CallGivenMethod(string methodName)
        {
            int logsQuantity = CheckLogsQuantity();
            MethodInfo methodToCall = (from method in _sut.GetType().GetMethods()
                                       where method.Name.Equals(methodName)
                                       select method).First();
            methodToCall.Invoke(_sut, new object[] { "TEST METHOD" });
            _sut.Dispose();
            int logsQuantityAfter = CheckLogsQuantity();
            Assert.AreEqual(logsQuantity + 1, logsQuantityAfter);
            RemoveLastEntry();
        }

        private int CheckLogsQuantity()
        {
            int quantity;
            using (var connection = new SqlConnection(_sut.ConnectionString))
            {
                connection.Open();
                SqlTransaction transaction = connection.BeginTransaction();
                using (var command = new SqlCommand("SELECT COUNT(*) FROM dbo.Traces", connection, transaction))
                {
                    quantity = (int)command.ExecuteScalar();
                }
                connection.Close();
            }
            return quantity;
        }

        private void RemoveLastEntry()
        {
            using (var connection = new SqlConnection(_sut.ConnectionString))
            {
                connection.Open();
                SqlTransaction transaction = connection.BeginTransaction();
                using (var command = new SqlCommand("SELECT TOP 1 * FROM Traces ORDER BY id DESC", connection, transaction))
                {
                    command.ExecuteNonQuery();
                }
                connection.Close();
            }
        }
    }
}