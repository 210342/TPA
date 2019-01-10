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
    public class MethodMetadataTests
    {
        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void EmitMethodsThrowsOnNull()
        {
            MethodMetadata.EmitMethods(null);
        }

        [TestMethod]
        public void EmitMethodsReturnsValue()
        {
            var methods =
                new List<MethodBase>(typeof(TestClass).GetMethods());
            var methodsMeta =
                new List<MethodMetadata>(MethodMetadata.EmitMethods(methods));
            Assert.AreEqual(methods.Count, methodsMeta.Count);
        }

        [TestMethod]
        public void CopyCtorTest()
        {
            var ctor = typeof(MethodMetadata).GetConstructor(
                BindingFlags.Instance | BindingFlags.NonPublic,
                null, new[] {typeof(MethodBase)}, null);

            var tmp = (MethodMetadata) ctor.Invoke(new object[] {typeof(TestClass).GetMethods().First()});
            var sut = new MethodMetadata(tmp);
            Assert.IsTrue(tmp.Name.Equals(sut.Name));
            Assert.AreEqual(tmp.SavedHash, sut.SavedHash);
            Assert.AreEqual(tmp.Parameters.Count(), sut.Parameters.Count());
            Assert.IsNull(sut.GenericArguments);
            Assert.IsTrue(tmp.Modifiers.Equals(sut.Modifiers));
            Assert.IsFalse(sut.IsExtension);
            Assert.IsTrue(tmp.ReturnType.Name.Equals(sut.ReturnType.Name));
        }

        protected class TestClass
        {
            public void Test1()
            {
            }

            public void Test2()
            {
            }
        }
    }
}