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
    public class SerializationAttributeMetadataTests
    {
        private class AttributeTest : IAttributeMetadata
        {
            public string Name { get; }

            public IEnumerable<IMetadata> Children { get { return Enumerable.Empty<IMetadata>(); } }

            public int SavedHash { get; }

            internal AttributeTest()
            {
                Name = "name";
                SavedHash = 1;
            }
        }

        [TestMethod]
        public void CopyCtorTest()
        {
            AttributeTest tmp = new AttributeTest();
            SerializationAttributeMetadata sut = new SerializationAttributeMetadata(tmp);
            Assert.IsTrue(tmp.Name.Equals(sut.Name));
            Assert.AreEqual(tmp.SavedHash, sut.SavedHash);
        }
    }
}