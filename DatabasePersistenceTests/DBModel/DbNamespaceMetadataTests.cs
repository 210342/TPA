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
    public class DbNamespaceMetadataTests
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
            INamespaceMetadata tmp = new NamespaceTest();
            DbNamespaceMetadata sut = new DbNamespaceMetadata(tmp);
            Assert.IsTrue(tmp.Name.Equals(sut.Name));
            Assert.AreEqual(tmp.SavedHash, sut.SavedHash);
            Assert.AreEqual(tmp.Types.Count(), sut.Types.Count());
        }

        [TestMethod()]
        public void DbNamespaceMetadataEmptyCtorTest()
        {
            Assert.IsNotNull(new DbNamespaceMetadata());
        }
    }

    [ExcludeFromCodeCoverage]
    internal class NamespaceTest : INamespaceMetadata
    {
        internal NamespaceTest()
        {
            Name = "name";
            SavedHash = 1;
            Types = Enumerable.Empty<ITypeMetadata>();
        }

        public IEnumerable<ITypeMetadata> Types { get; internal set; }
        public string Name { get; internal set; }
        public IEnumerable<IMetadata> Children { get; }
        public int SavedHash { get; internal set; }
    }
}