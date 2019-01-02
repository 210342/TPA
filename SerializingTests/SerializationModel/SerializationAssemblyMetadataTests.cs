using Microsoft.VisualStudio.TestTools.UnitTesting;
using ModelContract;
using SerializationModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace SerializationModel.Tests
{
    [TestClass()]
    public class SerializationAssemblyMetadataTests
    {
        private class AssemblyTest : IAssemblyMetadata
        {
            public IEnumerable<INamespaceMetadata> Namespaces { get; }

            public string Name { get; }

            public IEnumerable<IMetadata> Children { get { return Namespaces; } }

            public int SavedHash { get; }

            internal AssemblyTest()
            {
                Name = "name";
                SavedHash = 1;
                Namespaces = Enumerable.Empty<INamespaceMetadata>();
            }
        }

        [TestMethod]
        public void CopyCtorTest()
        {
            var tmp = new AssemblyTest();
            SerializationAssemblyMetadata sut = new SerializationAssemblyMetadata(tmp);
            Assert.IsTrue(tmp.Name.Equals(sut.Name));
            Assert.AreEqual(tmp.SavedHash, sut.SavedHash);
            Assert.AreEqual(tmp.Namespaces.Count(), sut.Namespaces.Count());
        }
    }
}