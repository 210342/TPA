using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Reflection;
using Library.Model;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace LibraryTests.Data.Model
{
    [ExcludeFromCodeCoverage]
    [TestClass]
    public class TypeMetadataTests
    {
        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void TypeMetadataThrowsOnNull()
        {
            new TypeMetadata(default(Type));
        }

        [TestMethod]
        [ExpectedException(typeof(TargetInvocationException))]
        public void TypeMetadataTwoArgThrowsOnNull()
        {
            ConstructorInfo ctor = typeof(TypeMetadata).GetConstructor(
                BindingFlags.Instance | BindingFlags.NonPublic,
                null, new[] {typeof(string), typeof(string), typeof(int)}, null);
            ctor.Invoke(new object[] {null, null, null});
        }

        [TestMethod]
        [ExpectedException(typeof(TargetInvocationException))]
        public void TypeMetadataGenericArgThrowsOnNull()
        {
            ConstructorInfo ctor = typeof(TypeMetadata).GetConstructor(
                BindingFlags.Instance | BindingFlags.NonPublic,
                null, new[]
                {
                    typeof(string), typeof(string), typeof(IEnumerable<TypeMetadata>),
                    typeof(int)
                }, null);
            ctor.Invoke(new object[] {null, null, null, null});
        }

        [TestMethod]
        public void EmitReferenceOfGeneric()
        {
            TypeMetadata obj = TypeMetadata.EmitReference(typeof(List<object>));
            PropertyInfo notNull = obj.GetType().GetProperty("GenericArguments",
                BindingFlags.Public | BindingFlags.Instance);
            Assert.IsNotNull(notNull.GetValue(obj));
        }

        [TestMethod]
        public void EmitReferenceOfNonGeneric()
        {
            TypeMetadata obj = TypeMetadata.EmitReference(typeof(object));
            PropertyInfo Null = obj.GetType().GetProperty("GenericArguments",
                BindingFlags.Public | BindingFlags.Instance);

            Assert.IsNull(Null.GetValue(obj));
        }

        [TestMethod]
        public void EmitGenericArgumentsReturns()
        {
            List<TypeMetadata> obj =
                new List<TypeMetadata>(TypeMetadata.EmitGenericArguments(new[] {typeof(List<object>)}));
            Assert.AreNotEqual(0, obj.Count);
        }

        [TestMethod]
        public void EmitDeclaringTypeReturnsNull()
        {
            TypeMetadata typeMeta = new TypeMetadata(typeof(Type));
            MethodInfo method = typeof(TypeMetadata).GetMethod("EmitDeclaringType",
                BindingFlags.NonPublic | BindingFlags.Instance);
            object value = method.Invoke(typeMeta, new object[] {null});
            Assert.IsNull(value);
        }

        [TestMethod]
        public void EmitDeclaringTypeReturnsValue()
        {
            TypeMetadata typeMeta = new TypeMetadata(typeof(Type));
            MethodInfo method = typeof(TypeMetadata).GetMethod("EmitDeclaringType",
                BindingFlags.NonPublic | BindingFlags.Instance);
            object value = method.Invoke(typeMeta, new object[] {typeof(Type)});
            Assert.IsNotNull(value);
        }

        [TestMethod]
        [ExpectedException(typeof(TargetInvocationException))]
        public void EmitNestedTypesThrowsOnNull()
        {
            TypeMetadata typeMeta = new TypeMetadata(typeof(Type));
            MethodInfo method = typeof(TypeMetadata).GetMethod("EmitNestedTypes", BindingFlags.NonPublic |
                                                                                  BindingFlags.Instance);
            object value = method.Invoke(typeMeta, new object[] {null});
        }

        [TestMethod]
        public void EmitNestedTypesResturns()
        {
            TypeMetadata typeMeta = new TypeMetadata(typeof(Type));
            MethodInfo method = typeof(TypeMetadata).GetMethod("EmitNestedTypes", BindingFlags.NonPublic
                                                                                  | BindingFlags.Instance);
            object value = method.Invoke(typeMeta, new object[] {new[] {typeof(Console)}});
            List<TypeMetadata> list = new List<TypeMetadata>((IEnumerable<TypeMetadata>) value);
            Assert.AreNotEqual(0, list.Count);
        }

        [TestMethod]
        [ExpectedException(typeof(TargetInvocationException))]
        public void EmitImplementsThrowsOnNull()
        {
            TypeMetadata typeMeta = new TypeMetadata(typeof(Type));
            MethodInfo method = typeof(TypeMetadata).GetMethod("EmitImplements",
                BindingFlags.NonPublic | BindingFlags.Instance);
            object value = method.Invoke(typeMeta, new object[] {null});
        }

        [TestMethod]
        public void EmitImplementsReturns()
        {
            TypeMetadata typeMeta = new TypeMetadata(typeof(Console));
            MethodInfo method = typeof(TypeMetadata).GetMethod("EmitImplements",
                BindingFlags.NonPublic | BindingFlags.Instance);
            object value = method.Invoke(typeMeta, new object[] {new[] {typeof(TypeMetadata)}});
            List<TypeMetadata> list;
            if (value is TypeMetadata)
                list = new List<TypeMetadata> {value as TypeMetadata};
            else
                list = new List<TypeMetadata>((IEnumerable<TypeMetadata>) value);
            Assert.AreNotEqual(0, list.Count);
        }

        [TestMethod]
        [ExpectedException(typeof(TargetInvocationException))]
        public void EmitModifiersThrowsOnNull()
        {
            MethodInfo method = typeof(TypeMetadata).GetMethod("EmitModifiers",
                BindingFlags.NonPublic | BindingFlags.Static);
            object value = method.Invoke(null, new object[] {null});
        }

        [TestMethod]
        public void EmitExtendsReturnsNullOnNull()
        {
            MethodInfo method =
                typeof(TypeMetadata).GetMethod("EmitExtends", BindingFlags.NonPublic | BindingFlags.Static);
            object value = method.Invoke(null, new object[] {null});
            Assert.IsNull(value);
        }

        [TestMethod]
        public void EmitExtendsReturns()
        {
            MethodInfo method =
                typeof(TypeMetadata).GetMethod("EmitExtends", BindingFlags.NonPublic | BindingFlags.Static);
            object value = method.Invoke(null, new object[] {typeof(TestClass)});
            List<TypeMetadata> list;
            if (value is TypeMetadata)
                list = new List<TypeMetadata> {value as TypeMetadata};
            else
                list = new List<TypeMetadata>((IEnumerable<TypeMetadata>) value);
            Assert.AreNotEqual(0, list.Count);
        }

        [TestMethod]
        public void CopyCtorTest()
        {
            TypeMetadata tmp = new TypeMetadata(typeof(TypeMetadata));
            TypeMetadata sut = new TypeMetadata(tmp);
            Assert.IsTrue(tmp.Name.Equals(sut.Name));
            Assert.AreEqual(tmp.SavedHash, sut.SavedHash);
            Assert.AreEqual(tmp.ImplementedInterfaces.Count(), sut.ImplementedInterfaces.Count());
            Assert.AreEqual(tmp.Properties.Count(), sut.Properties.Count());
            Assert.IsNull(sut.GenericArguments);
            Assert.IsTrue(tmp.Modifiers.Equals(sut.Modifiers));
            Assert.AreEqual(tmp.NamespaceName, sut.NamespaceName);
        }

        internal class TestClass : TypeMetadata
        {
            internal TestClass(Type type) : base(type)
            {
            }

            public class NestedTestClass
            {
            }
        }
    }
}