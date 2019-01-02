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
    public class SerializationParameterMetadataTests
    {
        private class ParameterTest : IParameterMetadata
        {
            public ITypeMetadata TypeMetadata { get; }
            public string Name { get; }
            public IEnumerable<IMetadata> Children { get; }
            public int SavedHash { get; }

            internal ParameterTest()
            {
                Name = "name";
                SavedHash = 1;
                TypeMetadata = new TypeTest("type");
            }
        }

        [TestMethod]
        public void CopyCtorTest()
        {
            ParameterTest tmp = new ParameterTest();
            SerializationParameterMetadata sut = new SerializationParameterMetadata(tmp);
            Assert.IsTrue(tmp.Name.Equals(sut.Name));
            Assert.AreEqual(tmp.SavedHash, sut.SavedHash);
            Assert.IsTrue(tmp.TypeMetadata.Name.Equals(sut.TypeMetadata.Name));
        }
    }
}