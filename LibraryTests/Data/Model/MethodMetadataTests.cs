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
    public class MethodMetadataTests
    {
        protected class TestClass
        {
            public void Test1() { }
            public void Test2() { }
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void EmitMethodsThrowsOnNull()
        {
            MethodMetadata.EmitMethods(null);
        }
        [TestMethod]
        public void EmitMethodsReturnsValue()
        {
            List < MethodBase > methods = 
                new List<MethodBase>(typeof(TestClass).GetMethods());
            List<MethodMetadata> methodsMeta = 
                new List<MethodMetadata>(MethodMetadata.EmitMethods(methods));
            Assert.AreEqual(methods.Count, methodsMeta.Count);
        }
    }
}