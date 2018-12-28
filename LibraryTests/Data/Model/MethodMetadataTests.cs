using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Reflection;
using Library.Model;
using System.Linq;

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

        [TestMethod]
        public void CopyCtorTest()
        {
            ConstructorInfo ctor = typeof(MethodMetadata).GetConstructor(
                 BindingFlags.Instance | BindingFlags.NonPublic,
                 null, new Type[] { typeof(MethodBase) }, null);

            MethodMetadata tmp = (MethodMetadata)ctor.Invoke(new object[] { typeof(TestClass).GetMethods().First()});
            MethodMetadata sut = new MethodMetadata(tmp);
            Assert.IsTrue(tmp.Name.Equals(sut.Name));
            Assert.AreEqual(tmp.SavedHash, sut.SavedHash);
            Assert.AreEqual(tmp.Parameters.Count(), sut.Parameters.Count());
            Assert.IsNull(sut.GenericArguments);
            Assert.IsTrue(tmp.Modifiers.Equals(sut.Modifiers));
            Assert.IsFalse(sut.IsExtension);
            Assert.IsTrue(tmp.ReturnType.Name.Equals(sut.ReturnType.Name));
        }
    }
}