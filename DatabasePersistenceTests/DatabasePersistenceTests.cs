using Microsoft.VisualStudio.TestTools.UnitTesting;
using ModelContract;
using System;
using System.Data.SqlClient;
using System.Linq;

namespace DatabasePersistence.DBModel
{
    [TestClass]
    public class DatabasePersistenceTests
    {

        private DatabasePersister persister;
        private string connectionString;

        [TestInitialize]
        public void Init()
        {
            // string assemblyPath = System.IO.Path.GetDirectoryName(
            //    System.Reflection.Assembly.GetCallingAssembly().Location);
            // string fileName = "LocalDB.mdf";
            connectionString = $"Server=(localdb)\\mssqllocaldb;Integrated Security=true;";
            persister = new DatabasePersister() { Target = connectionString };
        }

        [TestMethod]
        public void DbSaveTest()
        {
            DbAssemblyMetadata assemblyMetadata = new DbAssemblyMetadata() { Name = "test0" };
            DbNamespaceMetadata namespaceMeta1 = new DbNamespaceMetadata() { Name = "test1" };
            DbNamespaceMetadata namespaceMeta2 = new DbNamespaceMetadata() { Name = "test2" };
            DbNamespaceMetadata namespaceMeta3 = new DbNamespaceMetadata() { Name = "test3" };
            assemblyMetadata.Namespaces = new DbNamespaceMetadata[] { namespaceMeta1, namespaceMeta2, namespaceMeta3 };

            persister.Save(assemblyMetadata);
            using (var connection = new SqlConnection(persister.Target))
            {
                connection.Open();
                SqlTransaction transaction = connection.BeginTransaction();
                using (var command = new SqlCommand("SELECT COUNT(*) FROM Assemblies", connection, transaction))
                {
                    Assert.AreEqual(1, (int)command.ExecuteScalar());
                }
                connection.Close();
            }
        }

        [TestMethod]
        public void DbSecondDepthTest()
        {
            DbAssemblyMetadata assemblyMetadata = new DbAssemblyMetadata() { Name = "test0" };
            DbNamespaceMetadata namespaceMeta1 = new DbNamespaceMetadata() { Name = "test1" };
            DbNamespaceMetadata namespaceMeta2 = new DbNamespaceMetadata() { Name = "test2" };
            DbTypeMetadata type1 = new DbTypeMetadata { Name = "Type1" };
            namespaceMeta1.Types = new[] { type1 };
            assemblyMetadata.Namespaces = new DbNamespaceMetadata[] { namespaceMeta1, namespaceMeta2 };

            persister.Save(assemblyMetadata);
            using (var connection = new SqlConnection(persister.Target))
            {
                connection.Open();
                SqlTransaction transaction = connection.BeginTransaction();
                using (var command = new SqlCommand("SELECT COUNT(*) FROM Types", connection, transaction))
                {
                    Assert.AreEqual(1, (int)command.ExecuteScalar());
                }
                connection.Close();
            }
        }

        [TestCleanup]
        public void CleanUp()
        {
            using (var connection = new SqlConnection(persister.Target))
            {
                connection.Open();
                SqlTransaction transaction = connection.BeginTransaction();
                using (var command = new SqlCommand("DELETE FROM Types", connection, transaction))
                {
                    command.ExecuteScalar();
                }
                using (var command = new SqlCommand("DELETE FROM Namespaces", connection, transaction))
                {
                    command.ExecuteScalar();
                }
                using (var command = new SqlCommand("DELETE FROM Assemblies", connection, transaction))
                {
                    command.ExecuteScalar();
                }
                using (var command = new SqlCommand("DELETE FROM Attributes", connection, transaction))
                {
                    command.ExecuteScalar();
                }
                using (var command = new SqlCommand("DELETE FROM Methods", connection, transaction))
                {
                    command.ExecuteScalar();
                }
                using (var command = new SqlCommand("DELETE FROM Parameters", connection, transaction))
                {
                    command.ExecuteScalar();
                }
                using (var command = new SqlCommand("DELETE FROM Properties", connection, transaction))
                {
                    command.ExecuteScalar();
                }
                transaction.Commit();
                connection.Close();
            }
        }
    }
}
