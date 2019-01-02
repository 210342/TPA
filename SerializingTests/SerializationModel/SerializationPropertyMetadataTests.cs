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
    public class SerializationPropertyMetadataTests
    {
        private class PropertyTest : IPropertyMetadata
        {
            public ITypeMetadata MyType { get; }
            public string Name { get; }
            public IEnumerable<IMetadata> Children { get; }
            public int SavedHash { get; }

            internal PropertyTest()
            {
                Name = "name";
                SavedHash = 1;
                MyType = new TypeTest("type");
            }
        }

        [TestMethod]
        public void CopyCtorTest()
        {
            PropertyTest tmp = new PropertyTest();
            SerializationPropertyMetadata sut = new SerializationPropertyMetadata(tmp);
            Assert.IsTrue(tmp.Name.Equals(sut.Name));
            Assert.AreEqual(tmp.SavedHash, sut.SavedHash);
            Assert.IsTrue(tmp.MyType.Name.Equals(sut.MyType.Name));
        }
    }
}