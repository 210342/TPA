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
    public class SerializationMethodMetadataTests
    {
        private class MethodTest : IMethodMetadata
        {
            public string Name { get; }

            public IEnumerable<IMetadata> Children { get { return Enumerable.Empty<IMetadata>(); } }

            public int SavedHash { get; }

            public IEnumerable<ITypeMetadata> GenericArguments { get; }

            public ITypeMetadata ReturnType { get; }

            public bool IsExtension { get; }

            public IEnumerable<IParameterMetadata> Parameters { get; }

            public Tuple<AccessLevelEnum, AbstractEnum, StaticEnum, VirtualEnum> Modifiers { get; }

            internal MethodTest()
            {
                Name = "name";
                SavedHash = 1;
                IsExtension = false;
                Parameters = Enumerable.Empty<IParameterMetadata>();
                ReturnType = new TypeTest("type");
            }
        }

        [TestMethod]
        public void CopyCtorTest()
        {
            var tmp = new MethodTest();
            SerializationMethodMetadata sut = new SerializationMethodMetadata(tmp);
            Assert.IsTrue(tmp.Name.Equals(sut.Name));
            Assert.AreEqual(tmp.SavedHash, sut.SavedHash);
            Assert.AreEqual(tmp.Parameters.Count(), sut.Parameters.Count());
            Assert.IsNull(sut.GenericArguments);
            Assert.IsNull(sut.Modifiers);
            Assert.IsFalse(sut.IsExtension);
            Assert.IsTrue(tmp.ReturnType.Name.Equals(sut.ReturnType.Name));
        }
    }
}