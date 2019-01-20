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
    public class DbPropertyMetadataTests
    {
        [TestInitialize]
        public void NullifyDictionary()
        {
            FieldInfo field = typeof(AbstractMapper).GetField("<AlreadyMappedProperties>k__BackingField",
                BindingFlags.Static | BindingFlags.NonPublic);
            field.SetValue(null, new Dictionary<int, DbPropertyMetadata>());
        }

        [TestMethod]
        public void CopyCtorTest()
        {
            PropertyTest tmp = new PropertyTest();
            DbPropertyMetadata sut = new DbPropertyMetadata(tmp);
            Assert.IsTrue(tmp.Name.Equals(sut.Name));
            Assert.AreEqual(tmp.SavedHash, sut.SavedHash);
            Assert.IsTrue(tmp.MyType.Name.Equals(sut.MyType.Name));
        }

        [TestMethod()]
        public void DbPropertyMetadataEmptyCtorTest()
        {
            Assert.IsNotNull(new DbPropertyMetadata());
        }
    }

    [ExcludeFromCodeCoverage]
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