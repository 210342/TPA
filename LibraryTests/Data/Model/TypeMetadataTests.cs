using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Reflection;
using Library.Data.Model;

namespace LibraryTests.Data.Model
{
    [ExcludeFromCodeCoverage]
    [TestClass]
    public class TypeMetadataTests
    {

        internal class TestClass : TypeMetadata
        {
            internal TestClass(Type type) : base(type)
            {
            }

            public class NestedTestClass
            {

            }
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void TypeMetadataThrowsOnNull()
        {
            new TypeMetadata(null);
        }
        [TestMethod]
        [ExpectedException(typeof(TargetInvocationException))]
        public void TypeMetadataTwoArgThrowsOnNull()
        {
            ConstructorInfo ctor = typeof(TypeMetadata).GetConstructor(
                 BindingFlags.Instance | BindingFlags.NonPublic,
                 null, new Type[] { typeof(string), typeof(string) }, null);
            ctor.Invoke(new object[] { null, null });
        }
        [TestMethod]
        [ExpectedException(typeof(TargetInvocationException))]
        public void TypeMetadataGenericArgThrowsOnNull()
        {
            ConstructorInfo ctor = typeof(TypeMetadata).GetConstructor(
                 BindingFlags.Instance | BindingFlags.NonPublic,
                 null, new Type[] { typeof(string), typeof(string), typeof(IEnumerable<TypeMetadata>) },null);
            ctor.Invoke(new object[] { null, null, null });
        }

        [TestMethod]
        public void EmitReferenceOfGeneric()
        {
            var obj = TypeMetadata.EmitReference(typeof(List<object>));
            var notNull = obj.GetType().GetField("m_GenericArguments", 
                BindingFlags.NonPublic | BindingFlags.Instance);
            Assert.IsNotNull(notNull.GetValue(obj));
        }
        [TestMethod]
        public void EmitReferenceOfNonGeneric()
        {
            var obj = TypeMetadata.EmitReference(typeof(object));
            var Null = obj.GetType().GetField("m_GenericArguments",
                BindingFlags.NonPublic | BindingFlags.Instance);
            
            Assert.IsNull(Null.GetValue(obj));
        }
        [TestMethod]
        public void EmitGenericArgumentsReturns()
        {
            List<TypeMetadata> obj =
                new List<TypeMetadata>(TypeMetadata.EmitGenericArguments(new[] { typeof(List<object>) }));
            Assert.AreNotEqual(0, obj.Count);
        }
        [TestMethod]
        public void EmitDeclaringTypeReturnsNull()
        {
            TypeMetadata typeMeta = new TypeMetadata(typeof(Type));
            MethodInfo method = typeof(TypeMetadata).GetMethod("EmitDeclaringType",
                                    BindingFlags.NonPublic | BindingFlags.Instance);
            object value = method.Invoke(typeMeta, new object[] { null });
            Assert.IsNull(value);
        }
        [TestMethod]
        public void EmitDeclaringTypeReturnsValue()
        {
            TypeMetadata typeMeta = new TypeMetadata(typeof(Type));
            MethodInfo method = typeof(TypeMetadata).GetMethod("EmitDeclaringType",
                                    BindingFlags.NonPublic | BindingFlags.Instance);
            object value = method.Invoke(typeMeta, new object[] { typeof(Type) });
            Assert.IsNotNull(value);
        }
        [TestMethod]
        [ExpectedException(typeof(TargetInvocationException))]
        public void EmitNestedTypesThrowsOnNull()
        {
            TypeMetadata typeMeta = new TypeMetadata(typeof(Type));
            MethodInfo method = typeof(TypeMetadata).GetMethod("EmitNestedTypes", BindingFlags.NonPublic |
                BindingFlags.Instance);
            object value = method.Invoke(typeMeta, new object[] { null });
        }
        [TestMethod]
        public void EmitNestedTypesResturns()
        {
            TypeMetadata typeMeta = new TypeMetadata(typeof(Type));
            MethodInfo method = typeof(TypeMetadata).GetMethod("EmitNestedTypes", BindingFlags.NonPublic 
                | BindingFlags.Instance);
            object value = method.Invoke(typeMeta, new object[] { new[] { typeof(Console) } });
            List<TypeMetadata> list = new List<TypeMetadata>( (IEnumerable<TypeMetadata>)value );
            Assert.AreNotEqual(0, list.Count);
        }
        [TestMethod]
        [ExpectedException(typeof(TargetInvocationException))]
        public void EmitImplementsThrowsOnNull()
        {
            TypeMetadata typeMeta = new TypeMetadata(typeof(Type));
            MethodInfo method = typeof(TypeMetadata).GetMethod("EmitImplements", 
                BindingFlags.NonPublic | BindingFlags.Instance);
            object value = method.Invoke(typeMeta, new object[] { null });
        }
        [TestMethod]
        public void EmitImplementsReturns()
        {
            TypeMetadata typeMeta = new TypeMetadata(typeof(Console));
            MethodInfo method = typeof(TypeMetadata).GetMethod("EmitImplements", 
                BindingFlags.NonPublic | BindingFlags.Instance);
            object value = method.Invoke(typeMeta, new object[] { new[] { typeof(TypeMetadata) } });
            List<TypeMetadata> list;
            if (value is TypeMetadata)
                list = new List<TypeMetadata>() { value as TypeMetadata };
            else
                list = new List<TypeMetadata>((IEnumerable<TypeMetadata>)value);
            Assert.AreNotEqual(0, list.Count);
        }
        [TestMethod]
        [ExpectedException(typeof(TargetInvocationException))]
        public void EmitModifiersThrowsOnNull()
        {
            MethodInfo method = typeof(TypeMetadata).GetMethod("EmitModifiers",
                BindingFlags.NonPublic | BindingFlags.Static);
            object value = method.Invoke(null, new object[] { null });
        }
        [TestMethod]
        public void EmitExtendsReturnsNullOnNull()
        {
            MethodInfo method = typeof(TypeMetadata).GetMethod("EmitExtends", BindingFlags.NonPublic | BindingFlags.Static);
            object value = method.Invoke(null, new object[] { null });
            Assert.IsNull(value);
        }
        [TestMethod]
        public void EmitExtendsReturns()
        {
            MethodInfo method = typeof(TypeMetadata).GetMethod("EmitExtends", BindingFlags.NonPublic | BindingFlags.Static);
            object value = method.Invoke(null, new object[] { typeof(TestClass) });
            List<TypeMetadata> list;
            if (value is TypeMetadata)
                list = new List<TypeMetadata>() { value as TypeMetadata };
            else
                list = new List<TypeMetadata>((IEnumerable<TypeMetadata>)value);
            Assert.AreNotEqual(0, list.Count);
        }
    }
}