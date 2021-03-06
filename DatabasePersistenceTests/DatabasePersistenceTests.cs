﻿using System;
using System.Data.SqlClient;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using ModelContract;

namespace DatabasePersistence.DBModel
{
    [TestClass]
    [ExcludeFromCodeCoverage]
    public class DatabasePersistenceTests
    {
        private DatabasePersister persister;
        private string _target;

        [TestInitialize]
        public void Init()
        {
            persister = new DatabasePersister();
            try
            {
                persister.Load();
            }
            catch (Exception)
            {
                //BUG IN EF
            }
            _target = (string)typeof(DatabasePersister).GetField(
                "_originalTarget", BindingFlags.Instance | BindingFlags.NonPublic)?.GetValue(persister);
        }

        [TestCleanup]
        public void CleanUp()
        {
            persister.Dispose();
        }

        [TestMethod]
        public void DbSaveTest()
        {
            DbAssemblyMetadata assemblyMetadata = new DbAssemblyMetadata {Name = "test0"};
            DbNamespaceMetadata namespaceMeta1 = new DbNamespaceMetadata {Name = "test1"};
            DbNamespaceMetadata namespaceMeta2 = new DbNamespaceMetadata {Name = "test2"};
            DbNamespaceMetadata namespaceMeta3 = new DbNamespaceMetadata {Name = "test3"};
            assemblyMetadata.Namespaces = new[] {namespaceMeta1, namespaceMeta2, namespaceMeta3};

            int assembliesQuantityBefore = CountInTable("Assemblies");
            persister.Access(_target);
            persister.Save(assemblyMetadata);
            Assert.AreEqual(assembliesQuantityBefore + 1, CountInTable("Assemblies"));
        }

        [TestMethod]
        public void DbSecondDepthTest()
        {
            DbAssemblyMetadata assemblyMetadata = new DbAssemblyMetadata {Name = "test0"};
            DbNamespaceMetadata namespaceMeta1 = new DbNamespaceMetadata {Name = "test1"};
            DbNamespaceMetadata namespaceMeta2 = new DbNamespaceMetadata {Name = "test2"};
            DbTypeMetadata type1 = new DbTypeMetadata {Name = "Type1"};
            namespaceMeta1.Types = new[] {type1};
            assemblyMetadata.Namespaces = new[] {namespaceMeta1, namespaceMeta2};

            int typesQuantityBefore = CountInTable("Types");
            persister.Access(_target);
            persister.Save(assemblyMetadata);
            Assert.AreEqual(typesQuantityBefore + 1, CountInTable("Types"));
        }

        [TestMethod]
        public void RecurseTest()
        {
            DbAssemblyMetadata assemblyMetadata = new DbAssemblyMetadata {Name = "test0"};
            DbNamespaceMetadata namespaceMeta1 = new DbNamespaceMetadata {Name = "test1"};
            DbNamespaceMetadata namespaceMeta2 = new DbNamespaceMetadata {Name = "test2"};
            DbTypeMetadata type1 = new DbTypeMetadata {Name = "Type1"};
            type1.Properties = new[] {new DbPropertyMetadata {Name = "prop", EFMyType = type1}};
            namespaceMeta1.Types = new[] {type1};
            assemblyMetadata.Namespaces = new[] {namespaceMeta1, namespaceMeta2};

            int typesQuantityBefore = CountInTable("Types");
            int propertiesQuantityBefore = CountInTable("Properties");
            persister.Access(_target);
            persister.Save(assemblyMetadata);
            Assert.AreEqual(typesQuantityBefore + 1, CountInTable("Types"));
            Assert.AreEqual(propertiesQuantityBefore + 1, CountInTable("Properties"));
        }

        [TestMethod]
        public void DeepTest()
        {
            DbAssemblyMetadata assemblyMetadata = new DbAssemblyMetadata { Name = "test0" };
            DbNamespaceMetadata namespaceMeta1 = new DbNamespaceMetadata { Name = "test1" };
            DbNamespaceMetadata namespaceMeta2 = new DbNamespaceMetadata { Name = "test2" };
            DbTypeMetadata type1 = new DbTypeMetadata { Name = "Type1" };
            DbTypeMetadata type2 = new DbTypeMetadata { Name = "interface" };
            DbTypeMetadata type3 = new DbTypeMetadata { Name = "type2" };
            type1.ImplementedInterfaces = new[] { type2 };
            type3.ImplementedInterfaces = new[] { type2 };
            type1.Properties = new[] {new DbPropertyMetadata {Name = "prop", EFMyType = type1}};
            type1.Attributes = new[] {new DbAttributeMetadata {Name = "attr"}};
            DbParameterMetadata param1 = new DbParameterMetadata { Name = "param1", EFMyType = type1 };
            DbMethodMetadata method1 = new DbMethodMetadata
            {
                Name = "method1",
                Parameters = new[] {param1 , new DbParameterMetadata {Name = "param2", EFMyType = type3} }
            };
            DbMethodMetadata method2 = new DbMethodMetadata
            {
                Name = "method2",
                Parameters = new[] { param1 }
            };
            type1.Methods = new[] {method1, method2};
            namespaceMeta1.Types = new[] {type1, type2, type3};
            assemblyMetadata.Namespaces = new[] {namespaceMeta1, namespaceMeta2};

            int typesQuantityBefore = CountInTable("Types");
            int propertiesQuantityBefore = CountInTable("Properties");
            int attributesQuantityBefore = CountInTable("Attributes");
            int methodsQuantityBefore = CountInTable("Methods");
            int parametersQuantityBefore = CountInTable("Parameters");
            persister.Access(_target);
            persister.Save(assemblyMetadata);
            Assert.AreEqual(typesQuantityBefore + 3, CountInTable("Types"));
            Assert.AreEqual(propertiesQuantityBefore + 1, CountInTable("Properties"));
            Assert.AreEqual(attributesQuantityBefore + 1, CountInTable("Attributes"));
            Assert.AreEqual(methodsQuantityBefore + 2, CountInTable("Methods"));
            Assert.AreEqual(parametersQuantityBefore + 2, CountInTable("Parameters"));
        }

        [TestMethod]
        public async Task LoadTest()
        {
            DbAssemblyMetadata assemblyMetadata = new DbAssemblyMetadata { Name = "test0" };
            DbNamespaceMetadata namespaceMeta1 = new DbNamespaceMetadata { Name = "test1" };
            DbNamespaceMetadata namespaceMeta2 = new DbNamespaceMetadata { Name = "test2" };
            assemblyMetadata.Namespaces = new[] { namespaceMeta1, namespaceMeta2 };
            DbTypeMetadata type1 = new DbTypeMetadata { Name = "Type1" };
            DbTypeMetadata type2 = new DbTypeMetadata { Name = "Type2" };
            namespaceMeta1.Types = new[] { type1 };
            namespaceMeta2.Types = new[] { type2 };

            persister.Access(_target);
            await persister.Save(assemblyMetadata);
            IAssemblyMetadata loaded = await Task.Run(async () => await persister.Load());
            DbAssemblyMetadata loadedAssembly = loaded as DbAssemblyMetadata;
            Assert.IsNotNull(loadedAssembly);
            Assert.AreEqual("test0", loadedAssembly.Name);
            Assert.AreEqual(2, loadedAssembly.Namespaces.Count());
            Assert.AreEqual("test1", loadedAssembly.Namespaces.First().Name);
            Assert.AreEqual(1, loadedAssembly.Namespaces.First().Types.Count());
            Assert.AreEqual("test2", loadedAssembly.Namespaces.Last().Name);
            Assert.AreEqual(1, loadedAssembly.Namespaces.Last().Types.Count());
        }

        private int CountInTable(string tableName)
        {
            int result = 0;
            using (SqlConnection connection = new SqlConnection(_target))
            {
                connection.Open();
                SqlTransaction transaction = connection.BeginTransaction();
                using (SqlCommand command =
                    new SqlCommand($"SELECT COUNT(Id) FROM {tableName}", connection, transaction))
                {
                    try
                    {
                        result = (int) command.ExecuteScalar();
                    }
                    catch (SqlException)
                    {
                        result = 0;
                    }
                }
                connection.Close();
            }

            return result;
        }
    }
}