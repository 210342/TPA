using System.Collections.Generic;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using ModelContract;

namespace SerializationModel.Tests
{
    [TestClass]
    public class SerializationAttributeMetadataTests
    {
        [TestMethod]
        public void CopyCtorTest()
        {
            AttributeTest tmp = new AttributeTest();
            SerializationAttributeMetadata sut = new SerializationAttributeMetadata(tmp);
            Assert.IsTrue(tmp.Name.Equals(sut.Name));
            Assert.AreEqual(tmp.SavedHash, sut.SavedHash);
        }

        private class AttributeTest : IAttributeMetadata
        {
            internal AttributeTest()
            {
                Name = "name";
                SavedHash = 1;
            }

            public string Name { get; }

            public IEnumerable<IMetadata> Children => Enumerable.Empty<IMetadata>();

            public int SavedHash { get; }
        }
    }
}