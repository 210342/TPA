using System;
using System.Collections.Generic;
using System.Linq;
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

        public string NamespaceName { get; }
        public ITypeMetadata BaseType { get; }
        public IEnumerable<ITypeMetadata> GenericArguments { get; }
        public Tuple<AccessLevelEnum, SealedEnum, AbstractEnum> Modifiers { get; }
        public TypeKindEnum TypeKind { get; }
        public IEnumerable<IAttributeMetadata> Attributes { get; }
        public IEnumerable<ITypeMetadata> ImplementedInterfaces { get; }
        public IEnumerable<ITypeMetadata> NestedTypes { get; }
        public IEnumerable<IPropertyMetadata> Properties { get; }
        public ITypeMetadata DeclaringType { get; }
        public IEnumerable<IMethodMetadata> Methods { get; }
        public IEnumerable<IMethodMetadata> Constructors { get; }
        public string Name { get; }
        public IEnumerable<IMetadata> Children { get; }
        public int SavedHash { get; }
    }

    [TestClass]
    public class SerializationTypeMetadataTests
    {
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