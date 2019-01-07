using System;
using DatabasePersistence.DBModel;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Linq;

namespace DatabasePersistence.Tests
{
    [TestClass]
    public class DatabasePersistenceTests
    {

        private DatabasePersister persister;

        [TestInitialize]
        public void Init()
        {
            persister = new DatabasePersister();
            persister.Target = @"Server=(localdb)\mssqllocaldb;Integrated Security=true;
            AttachDbFileName='C:\Users\Script\Desktop\TPA\DatabasePersistenceTests\bin\Debug\LocalDB.mdf';";
        }

        [TestMethod]
        public void DbSaveTest()
        {
            DbAssemblyMetadata assemblyMetadata = new DbAssemblyMetadata();
            assemblyMetadata.Name = "test";
            DbNamespaceMetadata namespaceMeta1 = new DbNamespaceMetadata();
            DbNamespaceMetadata namespaceMeta2 = new DbNamespaceMetadata();
            DbNamespaceMetadata namespaceMeta3 = new DbNamespaceMetadata();
            namespaceMeta1.Name = "test1";
            namespaceMeta2.Name = "test2";
            namespaceMeta3.Name = "test3";
            assemblyMetadata.Namespaces = new DbNamespaceMetadata[] { namespaceMeta1, namespaceMeta2, namespaceMeta3 };

            persister.Save(assemblyMetadata);
            DbAssemblyMetadata loadedBack = (DbAssemblyMetadata)persister.Load();
            Assert.IsNotNull(loadedBack);
            Assert.AreEqual(loadedBack.Name, assemblyMetadata.Name);
            Assert.AreEqual(loadedBack.Children.Count(), assemblyMetadata.Children.Count());
        }
    }
}
