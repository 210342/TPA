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
    public class DbTypeMetadataTests
    {
        [TestInitialize]
        public void NullifyDictionary()
        {
            FieldInfo field = typeof(AbstractMapper).GetField("<AlreadyMappedTypes>k__BackingField",
                BindingFlags.Static | BindingFlags.NonPublic);
            field.SetValue(null, new Dictionary<int, DbTypeMetadata>());
        }

        [TestMethod]
        public void CopyCtorTest()
        {
            TypeTest tmp = new TypeTest();
            DbTypeMetadata sut = new DbTypeMetadata(tmp);
            Assert.IsTrue(tmp.Name.Equals(sut.Name));
            Assert.AreEqual(tmp.SavedHash, sut.SavedHash);
            Assert.AreEqual(tmp.ImplementedInterfaces.Count(), sut.ImplementedInterfaces.Count());
            Assert.AreEqual(tmp.Properties.Count(), sut.Properties.Count());
            Assert.IsNull(sut.GenericArguments);
            Assert.IsNull(sut.Modifiers);
            Assert.AreEqual(tmp.NamespaceName, sut.NamespaceName);
        }
    }

    [ExcludeFromCodeCoverage]
    internal class TypeTest : ITypeMetadata
    {
        internal TypeTest()
        {
            Name = "name";
            NamespaceName = "namespace";
            TypeKind = TypeKindEnum.ClassType;
            ImplementedInterfaces = Enumerable.Empty<ITypeMetadata>();
            Properties = Enumerable.Empty<IPropertyMetadata>();
            GenericArguments = null;
        }

        internal TypeTest(string name)
        {
            Name = name;
            SavedHash = 1;
        }

        public bool Mapped { get; }
        public string NamespaceName { get; internal set; }
        public ITypeMetadata BaseType { get; internal set; }
        public IEnumerable<ITypeMetadata> GenericArguments { get; internal set; }
        public Tuple<AccessLevelEnum, SealedEnum, AbstractEnum> Modifiers { get; internal set; }
        public TypeKindEnum TypeKind { get; internal set; }
        public IEnumerable<IAttributeMetadata> Attributes { get; internal set; }
        public IEnumerable<ITypeMetadata> ImplementedInterfaces { get; internal set; }
        public IEnumerable<ITypeMetadata> NestedTypes { get; internal set; }
        public IEnumerable<IPropertyMetadata> Properties { get; internal set; }
        public ITypeMetadata DeclaringType { get; internal set; }
        public IEnumerable<IMethodMetadata> Methods { get; internal set; }
        public IEnumerable<IMethodMetadata> Constructors { get; internal set; }
        public string Name { get; internal set; }
        public IEnumerable<IMetadata> Children { get; }
        public int SavedHash { get; internal set; }

        public void MapTypes() { }
    }

}