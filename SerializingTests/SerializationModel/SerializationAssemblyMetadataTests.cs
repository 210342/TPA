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
            var tmp = new AssemblyTest();
            var sut = new SerializationAssemblyMetadata(tmp);
            Assert.IsTrue(tmp.Name.Equals(sut.Name));
            Assert.AreEqual(tmp.SavedHash, sut.SavedHash);
            Assert.AreEqual(tmp.Namespaces.Count(), sut.Namespaces.Count());
        }

        private class AssemblyTest : IAssemblyMetadata
        {
            internal AssemblyTest()
            {
                Name = "name";
                SavedHash = 1;
                Namespaces = Enumerable.Empty<INamespaceMetadata>();
            }

            public IEnumerable<INamespaceMetadata> Namespaces { get; }

            public string Name { get; }

            public IEnumerable<IMetadata> Children => Namespaces;

            public int SavedHash { get; }
        }
    }
}