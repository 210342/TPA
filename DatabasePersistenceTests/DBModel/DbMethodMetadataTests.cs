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
    public class DbMethodMetadataTests
    {
        [TestInitialize]
        public void NullifyDictionary()
        {
            FieldInfo field = typeof(AbstractMapper).GetField("<AlreadyMappedMethods>k__BackingField",
                BindingFlags.Static | BindingFlags.NonPublic);
            field.SetValue(null, new Dictionary<int, DbMethodMetadata>());
        }

        [TestMethod]
        public void CopyCtorTest()
        {
            MethodTest tmp = new MethodTest();
            DbMethodMetadata sut = new DbMethodMetadata(tmp);
            Assert.IsTrue(tmp.Name.Equals(sut.Name));
            Assert.AreEqual(tmp.SavedHash, sut.SavedHash);
            Assert.AreEqual(tmp.Parameters.Count(), sut.Parameters.Count());
            Assert.IsNull(sut.GenericArguments);
            Assert.IsNull(sut.Modifiers);
            Assert.IsFalse(sut.IsExtension);
            Assert.IsTrue(tmp.ReturnType.Name.Equals(sut.ReturnType.Name));
        }

        [TestMethod()]
        public void DbMethodMetadataEmptyCtorTest()
        {
            Assert.IsNotNull(new DbMethodMetadata());
        }
    }

    [ExcludeFromCodeCoverage]
    internal class MethodTest : IMethodMetadata
    {
        internal MethodTest()
        {
            Name = "name";
            SavedHash = 1;
            IsExtension = false;
            Parameters = Enumerable.Empty<IParameterMetadata>();
            ReturnType = new TypeTest("type");
        }

        public string Name { get; internal set; }

        public IEnumerable<IMetadata> Children => Enumerable.Empty<IMetadata>();

        public int SavedHash { get; internal set; }

        public IEnumerable<ITypeMetadata> GenericArguments { get; internal set; }

        public ITypeMetadata ReturnType { get; internal set; }

        public bool IsExtension { get; internal set; }

        public IEnumerable<IParameterMetadata> Parameters { get; internal set; }

        public Tuple<AccessLevelEnum, AbstractEnum, StaticEnum, VirtualEnum> Modifiers { get; internal set; }
        public void MapTypes() { }
    }
}