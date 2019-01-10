using System.Collections.Generic;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using ModelContract;

namespace SerializationModel.Tests
{
    [TestClass]
    public class SerializationParameterMetadataTests
    {
        [TestMethod]
        public void CopyCtorTest()
        {
            ParameterTest tmp = new ParameterTest();
            SerializationParameterMetadata sut = new SerializationParameterMetadata(tmp);
            Assert.IsTrue(tmp.Name.Equals(sut.Name));
            Assert.AreEqual(tmp.SavedHash, sut.SavedHash);
            Assert.IsTrue(tmp.TypeMetadata.Name.Equals(sut.TypeMetadata.Name));
        }

        private class ParameterTest : IParameterMetadata
        {
            internal ParameterTest()
            {
                Name = "name";
                SavedHash = 1;
                TypeMetadata = new TypeTest("type");
            }

            public ITypeMetadata TypeMetadata { get; }
            public string Name { get; }
            public IEnumerable<IMetadata> Children { get; }
            public int SavedHash { get; }
        }
    }
}