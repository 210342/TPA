using Microsoft.VisualStudio.TestTools.UnitTesting;
using DatabasePersistence.DBModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Diagnostics.CodeAnalysis;
using ModelContract;
using System.Reflection;

namespace DatabasePersistence.DBModel.Tests
{
    [ExcludeFromCodeCoverage]
    [TestClass()]
    public class DbParameterMetadataTests
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
            ParameterTest tmp = new ParameterTest();
            DbParameterMetadata sut = new DbParameterMetadata(tmp);
            Assert.IsTrue(tmp.Name.Equals(sut.Name));
            Assert.AreEqual(tmp.SavedHash, sut.SavedHash);
            Assert.IsTrue(tmp.TypeMetadata.Name.Equals(sut.TypeMetadata.Name));
        }

        [TestMethod()]
        public void DbParameterMetadataEmptyCtorTest()
        {
            Assert.IsNotNull(new DbParameterMetadata());
        }
    }

    [ExcludeFromCodeCoverage]
    internal class ParameterTest : IParameterMetadata
    {
        internal ParameterTest()
        {
            Name = "name";
            SavedHash = 1;
            TypeMetadata = new TypeTest("type");
        }

        public ITypeMetadata TypeMetadata { get; internal set; }
        public string Name { get; internal set; }
        public IEnumerable<IMetadata> Children { get; }
        public int SavedHash { get; internal set; }

        public void MapTypes() { }
    }
}