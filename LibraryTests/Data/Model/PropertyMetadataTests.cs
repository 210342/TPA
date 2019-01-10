using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Reflection;
using Library.Model;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace LibraryTests.Data.Model
{
    [ExcludeFromCodeCoverage]
    [TestClass]
    public class PropertyMetadataTests
    {
        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void EmitPropertiesThrowsOnNull()
        {
            PropertyMetadata.EmitProperties(null);
        }

        [TestMethod]
        public void EmitPropertiesReturnsFine()
        {
            var properties =
                new List<PropertyInfo>(typeof(TestClass).GetProperties());
            var propertiesMeta =
                new List<PropertyMetadata>(PropertyMetadata.EmitProperties(properties));
            Assert.AreEqual(properties.Count, propertiesMeta.Count);
        }

        [TestMethod]
        [ExpectedException(typeof(TargetInvocationException))]
        public void PropertyMetadataThrowsOnNull()
        {
            var ctor = typeof(PropertyMetadata).GetConstructor(
                BindingFlags.Instance | BindingFlags.NonPublic,
                null, new[] {typeof(string), typeof(TypeMetadata)}, null);

            ctor.Invoke(new object[] {null, null});
        }

        [TestMethod]
        public void CopyCtorTest()
        {
            var ctor = typeof(PropertyMetadata).GetConstructor(
                BindingFlags.Instance | BindingFlags.NonPublic,
                null, new[] {typeof(string), typeof(TypeMetadata)}, null);
            var tmp = (PropertyMetadata) ctor.Invoke(new object[]
                {"asdf", new TypeMetadata(typeof(PropertyMetadataTests))});
            var sut = new PropertyMetadata(tmp);
            Assert.IsTrue(tmp.Name.Equals(sut.Name));
            Assert.AreEqual(tmp.SavedHash, sut.SavedHash);
            Assert.IsTrue(tmp.MyType.Name.Equals(sut.MyType.Name));
        }

        protected class TestClass
        {
            private Type PropertyOne { get; }
            private Type PropertyTwo { get; }
        }
    }
}