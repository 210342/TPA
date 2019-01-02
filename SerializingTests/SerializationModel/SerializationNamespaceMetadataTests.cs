using Microsoft.VisualStudio.TestTools.UnitTesting;
using ModelContract;
using SerializationModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SerializationModel.Tests
{
    [TestClass()]
    public class SerializationNamespaceMetadataTests
    {
        private class NamespaceTest : INamespaceMetadata
        {
            public IEnumerable<ITypeMetadata> Types { get; }
            public string Name { get; }
            public IEnumerable<IMetadata> Children { get; }
            public int SavedHash { get; }

            internal NamespaceTest()
            {
                Name = "name";
                SavedHash = 1;
                Types = Enumerable.Empty<ITypeMetadata>();
            }
        }

        [TestMethod]
        public void CopyCtorTest()
        {
            INamespaceMetadata tmp = new NamespaceTest();
            SerializationNamespaceMetadata sut = new SerializationNamespaceMetadata(tmp);
            Assert.IsTrue(tmp.Name.Equals(sut.Name));
            Assert.AreEqual(tmp.SavedHash, sut.SavedHash);
            Assert.AreEqual(tmp.Types.Count(), sut.Types.Count());
        }
    }
}