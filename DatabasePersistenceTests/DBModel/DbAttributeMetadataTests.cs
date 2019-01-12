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
    public class DbAttributeMetadataTests
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
            DbAttributeMetadata sut = new DbAttributeMetadata(tmp);
            Assert.IsTrue(tmp.Name.Equals(sut.Name));
            Assert.AreEqual(tmp.SavedHash, sut.SavedHash);
        }

        [TestMethod]
        public void DbAttributeMetadataEmptyCtorTest()
        {
            Assert.IsNotNull(new DbAttributeMetadata());
        }
    }

    [ExcludeFromCodeCoverage]
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