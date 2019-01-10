using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using ModelContract;

namespace SerializationModel.Tests
{
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

    [TestClass]
    public class SerializationTypeMetadataTests
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
            TypeTest tmp = new TypeTest();
            SerializationTypeMetadata sut = new SerializationTypeMetadata(tmp);
            Assert.IsTrue(tmp.Name.Equals(sut.Name));
            Assert.AreEqual(tmp.SavedHash, sut.SavedHash);
            Assert.AreEqual(tmp.ImplementedInterfaces.Count(), sut.ImplementedInterfaces.Count());
            Assert.AreEqual(tmp.Properties.Count(), sut.Properties.Count());
            Assert.IsNull(sut.GenericArguments);
            Assert.IsNull(sut.Modifiers);
            Assert.AreEqual(tmp.NamespaceName, sut.NamespaceName);
        }
    }
}