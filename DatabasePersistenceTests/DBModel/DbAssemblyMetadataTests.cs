using Microsoft.VisualStudio.TestTools.UnitTesting;
using ModelContract;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Reflection;

namespace DatabasePersistence.DBModel.Tests
{
    [TestClass]
    [ExcludeFromCodeCoverage]
    public class DbAssemblyMetadataTests
    {
        [TestInitialize]
        public void NullifyDictionary()
        {
            FieldInfo field = typeof(AbstractMapper).GetField("<AlreadyMapped>k__BackingField",
                BindingFlags.Static | BindingFlags.NonPublic);
            field.SetValue(null, new Dictionary<int, IMetadata>());
        }

        [TestMethod]
        public void DbAssemblyMetadataTest()
        {
            Assert.IsNotNull(new DbAssemblyMetadata());
        }

        [TestMethod]
        public void DbAssemblyMetadataNamespacesPropertyTest()
        {
            DbAssemblyMetadata sut = new DbAssemblyMetadata
            {
                Namespaces = new[] {new DbNamespaceMetadata {Name = "test1"}}
            };
            Assert.IsNotNull(sut.EFNamespaces);
            Assert.AreEqual(1, sut.EFNamespaces.Count);
        }

        [TestMethod]
        public void CopyCtorTest()
        {
            AssemblyTest tmp = new AssemblyTest();
            DbAssemblyMetadata sut = new DbAssemblyMetadata(tmp);
            Assert.IsTrue(tmp.Name.Equals(sut.Name));
            Assert.AreEqual(tmp.SavedHash, sut.SavedHash);
            Assert.AreEqual(tmp.Namespaces.Count(), sut.Namespaces.Count());
        }

        [TestMethod]
        public void DeepMappingTest()
        {
            AssemblyTest assemblyMetadata = new AssemblyTest { Name = "test0", SavedHash = 0 };
            NamespaceTest namespaceMeta1 = new NamespaceTest { Name = "test1", SavedHash = 1 };
            NamespaceTest namespaceMeta2 = new NamespaceTest { Name = "test2", SavedHash = 2 };
            TypeTest type1 = new TypeTest { Name = "Type1", SavedHash = 3 };
            type1.Properties = new[] { new PropertyTest { Name = "prop", MyType = type1, SavedHash = 4 } };
            type1.Attributes = new[] { new AttributeTest { Name = "attr", SavedHash = 5 } };
            MethodTest method1 = new MethodTest
            {
                Name = "method1",
                SavedHash = 6,
                Parameters = new[] { new ParameterTest { Name = "param1", MyType = type1, SavedHash = 7 } }
            };
            type1.Methods = new[] { method1 };
            namespaceMeta1.Types = new[] { type1 };
            assemblyMetadata.Namespaces = new[] { namespaceMeta1, namespaceMeta2 };
            DbAssemblyMetadata sut = new DbAssemblyMetadata(assemblyMetadata);
            Assert.AreEqual(2, sut.Namespaces.Count());
            Assert.AreEqual(1, sut.Namespaces.First().Types.Count());
            Assert.AreEqual(1, sut.Namespaces.First().Types.First().Methods.Count());
            Assert.AreEqual(1, sut.Namespaces.First().Types.First().Properties.Count());
            Assert.AreEqual(1, sut.Namespaces.First().Types.First().Attributes.Count());
            Assert.AreEqual(1, sut.Namespaces.First().Types.First().Methods.First().Parameters.Count());
        }
    }

    [ExcludeFromCodeCoverage]
    internal class AssemblyTest : IAssemblyMetadata
    {
        internal AssemblyTest()
        {
            Name = "name";
            SavedHash = 1;
            Namespaces = Enumerable.Empty<INamespaceMetadata>();
        }

        public IEnumerable<INamespaceMetadata> Namespaces { get; internal set; }

        public string Name { get; internal set; }

        public IEnumerable<IMetadata> Children => Namespaces;

        public int SavedHash { get; internal set; }
    }
}