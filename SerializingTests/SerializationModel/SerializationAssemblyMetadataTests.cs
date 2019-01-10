using System.Collections.Generic;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using ModelContract;

namespace SerializationModel.Tests
{
    [TestClass]
    public class SerializationAssemblyMetadataTests
    {
        [TestMethod]
        public void CopyCtorTest()
        {
            AssemblyTest tmp = new AssemblyTest();
            SerializationAssemblyMetadata sut = new SerializationAssemblyMetadata(tmp);
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
                Parameters = new[] { new ParameterTest { Name = "param1", TypeMetadata = type1, SavedHash = 7 } }
            };
            type1.Methods = new[] { method1 };
            namespaceMeta1.Types = new[] { type1 };
            assemblyMetadata.Namespaces = new[] { namespaceMeta1, namespaceMeta2 };
            SerializationAssemblyMetadata sut = new SerializationAssemblyMetadata(assemblyMetadata);
            Assert.AreEqual(2, sut.Namespaces.Count());
            Assert.AreEqual(1, sut.Namespaces.First().Types.Count());
            Assert.AreEqual(1, sut.Namespaces.First().Types.First().Methods.Count());
            Assert.AreEqual(1, sut.Namespaces.First().Types.First().Properties.Count());
            Assert.AreEqual(1, sut.Namespaces.First().Types.First().Attributes.Count());
            Assert.AreEqual(1, sut.Namespaces.First().Types.First().Methods.First().Parameters.Count());
        }
    }

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