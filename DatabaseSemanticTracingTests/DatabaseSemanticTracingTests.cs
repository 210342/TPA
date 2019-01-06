using Microsoft.VisualStudio.TestTools.UnitTesting;
using DatabaseSemanticTracing;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.SqlClient;
using System.Reflection;

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

        [TestCleanup]
        public void CleanUp()
        {
            _sut.Dispose();
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
                int logsQuantity, logsQuantityAfter;
                using (var command = new SqlCommand("SELECT COUNT(*) FROM dbo.Traces", connection, transaction))
                {
                    logsQuantity = (int)command.ExecuteScalar();
                }
                _sut.LogStartup();
                using (var command = new SqlCommand("SELECT COUNT(*) FROM dbo.Traces", connection, transaction))
                {
                    logsQuantityAfter = (int)command.ExecuteScalar();
                }
                transaction.Rollback();
                transaction.Dispose();
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
            MethodInfo methodToCall = (from method in _sut.GetType().GetMethods()
                                       where method.Name.Equals(methodName)
                                       select method).First();
            using (var connection = new SqlConnection(_sut.ConnectionString))
            {
                connection.Open();
                SqlTransaction transaction = connection.BeginTransaction();
                int logsQuantity, logsQuantityAfter;
                using (var command = new SqlCommand("SELECT COUNT(*) FROM dbo.Traces", connection, transaction))
                {
                    logsQuantity = (int)command.ExecuteScalar();
                }
                methodToCall.Invoke(_sut, new object[] { "TEST METHOD" });
                using (var command = new SqlCommand("SELECT COUNT(*) FROM dbo.Traces", connection, transaction))
                {
                    logsQuantityAfter = (int)command.ExecuteScalar();
                }
                transaction.Rollback();
                transaction.Dispose();
                connection.Close();
            }
        }
    }
}