using System.Collections.Generic;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using ModelContract;

namespace SerializationModel.Tests
{
    [TestClass]
    public class SerializationPropertyMetadataTests
    {
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

    internal class PropertyTest : IPropertyMetadata
    {
        internal PropertyTest()
        {
            Name = "name";
            SavedHash = 1;
            MyType = new TypeTest("type");
        }

        public ITypeMetadata MyType { get; internal set; }
        public string Name { get; internal set; }
        public IEnumerable<IMetadata> Children { get; }
        public int SavedHash { get; internal set; }

        public void MapTypes() { }
    }
}