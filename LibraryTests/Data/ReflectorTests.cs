using System;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Reflection;
using Library.Data;
using Library.Model;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using ModelContract;

namespace Library.Data.Tests
{
    [ExcludeFromCodeCoverage]
    [TestClass]
    public class ReflectorTests
    {
        private Reflector _sut;

        [TestInitialize]
        public void SetUp()
        {
            _sut = new Reflector(@"TPA.ApplicationArchitecture.dll");
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void ReflectorStringArgThrowsOnNull()
        {
            new Reflector((string)null);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void ReflectorAssemblyArgThrowsOnNull()
        {
            new Reflector((Assembly)null);
        }

        [TestMethod]
        [ExpectedException(typeof(BadImageFormatException))]
        public void ReflectorAssemblyArgThrowsBadImage()
        {
            new Reflector(@"libstdc++-6.dll");
        }

        [TestMethod]
        public void AbstractClassTest()
        {
            ITypeMetadata abstractClass = _sut.AssemblyModel.Namespaces
                .Single(x => x.Name == "TPA.ApplicationArchitecture.Data")
                .Types.Single(x => x.Name == "AbstractClass");
            Assert.AreEqual(AbstractEnum.Abstract, abstractClass.Modifiers.Item3);
            Assert.AreEqual(AbstractEnum.Abstract,
                abstractClass.Methods.Single(x => x.Name == "AbstractMethod").Modifiers.Item2);
        }

        [TestMethod]
        public void ClassWithAttributesTest()
        {
            ITypeMetadata attributeClass = _sut.AssemblyModel.Namespaces
                .Single(x => x.Name == "TPA.ApplicationArchitecture.Data")
                .Types.Single(x => x.Name == "ClassWithAttribute");
            Assert.AreEqual(1, attributeClass.Attributes.Count());
        }

        [TestMethod]
        public void DerivedClassTest()
        {
            ITypeMetadata derivedClass = _sut.AssemblyModel.Namespaces
                .Single(x => x.Name == "TPA.ApplicationArchitecture.Data")
                .Types.Single(x => x.Name == "DerivedClass");
            Assert.IsNotNull(derivedClass.BaseType);
        }

        [TestMethod]
        public void GenericClassTest()
        {
            ITypeMetadata genericClass = _sut.AssemblyModel.Namespaces
                .Single(x => x.Name == "TPA.ApplicationArchitecture.Data")
                .Types.Single(x => x.Name.Contains("GenericClass"));
            Assert.AreEqual(1, genericClass.GenericArguments.Count());
            Assert.AreEqual("T", genericClass.GenericArguments.Single().Name);
            Assert.AreEqual("T", genericClass.Properties.Single(x => x.Name == "GenericProperty")
                .MyType.Name);
            Assert.AreEqual(1, genericClass.Methods.Single(x => x.Name == "GenericMethod").Parameters.Count());
            Assert.AreEqual("T", genericClass.Methods.Single(x => x.Name == "GenericMethod")
                .Parameters.Single().TypeMetadata.Name);
            Assert.AreEqual("T",
                genericClass.Methods.Single(x => x.Name == "GenericMethod").ReturnType.Name);
            //TypeMetaData lacks Fields info
        }

        [TestMethod]
        public void InterfaceTest()
        {
            ITypeMetadata interfaceClass = _sut.AssemblyModel.Namespaces
                .Single(x => x.Name == "TPA.ApplicationArchitecture.Data")
                .Types.Single(x => x.Name == "IExample");
            Assert.AreEqual(TypeKindEnum.InterfaceType, interfaceClass.TypeKind);
            Assert.AreEqual(AbstractEnum.Abstract, interfaceClass.Modifiers.Item3);
            Assert.AreEqual(AbstractEnum.Abstract,
                interfaceClass.Methods.Single(x => x.Name == "MethodA").Modifiers.Item2);
        }

        [TestMethod]
        public void ImplementedInterfaceTest()
        {
            ITypeMetadata interfaceClass = _sut.AssemblyModel.Namespaces
                .Single(x => x.Name == "TPA.ApplicationArchitecture.Data")
                .Types.Single(x => x.Name == "IExample");
            ITypeMetadata implementedInterfaceClass = _sut.AssemblyModel.Namespaces
                .Single(x => x.Name == "TPA.ApplicationArchitecture.Data")
                .Types.Single(x => x.Name == "ImplementationOfIExample");
            Assert.AreEqual("IExample", implementedInterfaceClass.ImplementedInterfaces.Single().Name);
            foreach (IMethodMetadata method in interfaceClass.Methods)
            {
                Assert.IsNotNull(implementedInterfaceClass.Methods.SingleOrDefault(x => x.Name == method.Name));
            }
        }

        [TestMethod]
        public void StructureTest()
        {
            ITypeMetadata structure = _sut.AssemblyModel.Namespaces
                .Single(x => x.Name == "TPA.ApplicationArchitecture.Data")
                .Types.Single(x => x.Name == "Structure");
            Assert.AreEqual(TypeKindEnum.StructType, structure.TypeKind);
        }

        [TestMethod]
        public void StaticClassTest()
        {
            ITypeMetadata staticClass = _sut.AssemblyModel.Namespaces
                .Single(x => x.Name == "TPA.ApplicationArchitecture.Data").
                Types.Single(x => x.Name == "StaticClass");
            Assert.AreEqual(StaticEnum.Static,
                staticClass.Methods.Single(x => x.Name == "StaticMethod1").Modifiers.Item3);
        }
    }
}