using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using ModelContract;

namespace SerializationModel.Tests
{
    [TestClass]
    public class SerializationAttributeMetadataTests
    {
        [TestInitialize]
        public void NullifyDictionary()
        {
            FieldInfo field = typeof(AbstractMapper).GetField("<AlreadyMapped>k__BackingField",
                BindingFlags.Static | BindingFlags.NonPublic);
            field.SetValue(null, new Dictionary<int, IMetadata>());
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

    internal class AttributeTest : IAttributeMetadata
    {
        internal AttributeTest()
        {
            Name = "name";
            SavedHash = 1;
        }

        public string Name { get; internal set; }

        public IEnumerable<IMetadata> Children => Enumerable.Empty<IMetadata>();

        public int SavedHash { get; internal set; }
    }
}