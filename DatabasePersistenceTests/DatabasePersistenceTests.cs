using Microsoft.VisualStudio.TestTools.UnitTesting;
using ModelContract;
using System;
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
            string assemblyPath = System.IO.Path.GetDirectoryName(
                System.Reflection.Assembly.GetCallingAssembly().Location);
            string fileName = "LocalDB.mdf";
            connectionString = $"Server=(localdb)\\mssqllocaldb;Integrated Security=true;" +
            $"AttachDbFileName='{assemblyPath}\\{fileName}';";
            persister = new DatabasePersister();
            persister.SetTarget ( connectionString );
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
        [TestMethod]
        public void DbSecondDepthTest()
        {
            DbAssemblyMetadata assemblyMetadata = new DbAssemblyMetadata();
            assemblyMetadata.Name = "test";
            DbNamespaceMetadata namespaceMeta1 = new DbNamespaceMetadata();
            DbNamespaceMetadata namespaceMeta2 = new DbNamespaceMetadata();
            DbTypeMetadata type1 = new DbTypeMetadata();
            type1.Name = "Type1";
            namespaceMeta1.Name = "test1";
            namespaceMeta2.Name = "test2";
            //----
            namespaceMeta1.Types = new[] { type1 };
            assemblyMetadata.Namespaces = new DbNamespaceMetadata[] { namespaceMeta1, namespaceMeta2 };

            persister.Save(assemblyMetadata);
            DbAssemblyMetadata loadedBack = (DbAssemblyMetadata)persister.Load();
            PrintAssembly(loadedBack);
            Assert.IsNotNull(loadedBack);
            Assert.IsNotNull(loadedBack.Children);
            Assert.IsNotNull(loadedBack.Children.First());
            Assert.AreEqual(loadedBack.Namespaces.First(), assemblyMetadata.Namespaces.First());
            Assert.AreEqual(loadedBack.Namespaces.First().Children, assemblyMetadata.Namespaces.First().Children);
        }

        [TestCleanup]
        public void CleanUp()
        {
            persister.Dispose();
        }
        void PrintAssembly(IAssemblyMetadata assembly)
        {
            Console.WriteLine(assembly?.Name);
            if (assembly.Children != null)
            {
                foreach (var child in assembly.Children)
                    Console.WriteLine(child?.Name);
            }
        }
        void PrintNodes(IMetadata head)
        {
            if (head != null)
            {
                Console.WriteLine($"Node: {head.GetType()}::: {head.Name}");
                if(head.Children != null)
                {
                    foreach (var child in head.Children)
                        PrintNodes((IMetadata)child);
                }
            }
        }
    }
}
