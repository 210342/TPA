using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Reflection;
using Library.Model;

namespace LibraryTests.Data.Model
{
    [ExcludeFromCodeCoverage]
    [TestClass]
    public class PropertyMetadataTests
    {
        protected class TestClass
        {
            Type propertyOne { get; }
            Type propertyTwo { get; }
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void EmitPropertiesThrowsOnNull()
        {
            PropertyMetadata.EmitProperties(null);
        }

        [TestMethod]
        public void EmitPropertiesReturnsFine()
        {
            List<PropertyInfo> properties = 
                new List<PropertyInfo>(typeof(TestClass).GetProperties());
            List<PropertyMetadata> propertiesMeta = 
                new List<PropertyMetadata>(PropertyMetadata.EmitProperties(properties));
            Assert.AreEqual(properties.Count, propertiesMeta.Count);
        }
        [TestMethod]
        [ExpectedException(typeof(TargetInvocationException))]
        public void PropertyMetadataThrowsOnNull()
        {
            ConstructorInfo ctor = typeof(PropertyMetadata).GetConstructor(
                 BindingFlags.Instance | BindingFlags.NonPublic,
                 null, new Type[] { typeof(string), typeof(TypeMetadata) }, null);

            ctor.Invoke(new object[] { null, null });
        }
    }
}