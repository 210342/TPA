using System.Collections.Generic;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using ModelContract;

namespace SerializationModel.Tests
{
    [TestClass]
    public class SerializationNamespaceMetadataTests
    {
        [TestMethod]
        public void CopyCtorTest()
        {
            INamespaceMetadata tmp = new NamespaceTest();
            SerializationNamespaceMetadata sut = new SerializationNamespaceMetadata(tmp);
            Assert.IsTrue(tmp.Name.Equals(sut.Name));
            Assert.AreEqual(tmp.SavedHash, sut.SavedHash);
            Assert.AreEqual(tmp.Types.Count(), sut.Types.Count());
        }

        private class NamespaceTest : INamespaceMetadata
        {
            internal NamespaceTest()
            {
                Name = "name";
                SavedHash = 1;
                Types = Enumerable.Empty<ITypeMetadata>();
            }

            public IEnumerable<ITypeMetadata> Types { get; }
            public string Name { get; }
            public IEnumerable<IMetadata> Children { get; }
            public int SavedHash { get; }
        }
    }
}