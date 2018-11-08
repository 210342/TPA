using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Tracing;
using Tracing.DatabaseHandling;

namespace TracingTests
{
    [ExcludeFromCodeCoverage]
    [TestClass]
    public class DbTraceListenerTests
    {
        internal class MockDbWriter : IDatabaseWriter
        {
            string dbName = "DB";
            string tableName = "TB";
            string[] columnsNames = new string[] { "col1, col2" };
            List<string> queriesBuffered = new List<string>();

            public bool ColumnExists(string dbName, string SQLTableName, string SQLColumn)
            {
                if (this.dbName != dbName)
                    return false;
                if (SQLTableName != tableName)
                    return false;
                foreach (string str in columnsNames)
                    if (str == SQLColumn)
                        return true;
                return false;
            }

            public bool TableExists(string SQLTableName)
            {
                return SQLTableName == tableName;
            }

            public void WriteQuery(string data)
            {
                queriesBuffered.Add(data);
            }
            internal int Size() => queriesBuffered.Count;

        }
        internal DbTraceListener FabricateDBTL(IDatabaseWriter DW)
        {
            DbTraceListener dbtl = new DbTraceListener(DW);
            dbtl.DatabaseName = "DB";
            dbtl.TableName = "TB";
            dbtl.LogField = "col1";
            dbtl.TimeField = "col2";
            return dbtl;
        }

        [TestMethod]
        public void ContentExistsAfterTrace()
        {
            MockDbWriter customDW = new MockDbWriter();
            var dbtl = FabricateDBTL(customDW);
            int initialSize = customDW.Size();
            dbtl.Write("data");
            dbtl.Flush();
            int nextSize = customDW.Size();
            Assert.AreNotEqual(initialSize, nextSize);
        }
        [TestMethod]
        public void ExactlyAsManyLinesAsFlushed()
        {
            MockDbWriter customDW = new MockDbWriter();
            var dbtl = FabricateDBTL(customDW);
            int initialSize = customDW.Size();
            dbtl.Write("data");
            dbtl.Write("data2");
            dbtl.Write("data3");
            dbtl.Flush();
            int nextSize = customDW.Size();
            Assert.AreEqual(initialSize + 3, nextSize);
        }
    }
}
