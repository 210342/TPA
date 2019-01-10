using System;
using System.Diagnostics.CodeAnalysis;
using System.Reflection;
using Library.Model;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace LibraryTests.Data.Model
{
    [ExcludeFromCodeCoverage]
    [TestClass]
    public class ExtensionMethodsTests
    {
        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void GetVisibleForTypeThrowsExceptionOnNull()
        {
            ((Type) null).GetVisible();
        }

        [TestMethod]
        public void GetVisibleForTypeReturnTrue()
        {
            Assert.AreEqual(true, typeof(Type).GetVisible());
        }

        [TestMethod]
        public void GetVisibleForTypeReturnFalse()
        {
            Assert.AreEqual(false, typeof(PrivateTestType).GetVisible());
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void GetVisibleForMethodThrowsExceptionOnNull()
        {
            ((MethodBase) null).GetVisible();
        }

        [TestMethod]
        public void GetVisibleForMethodReturnTrue()
        {
            Assert.AreEqual(true, typeof(Type).GetMethod("ToString").GetVisible());
        }

        [TestMethod]
        public void GetVisibleForMethodReturnFalse()
        {
            Assert.AreEqual(false, typeof(TypeMetadata).GetMethod("GetTypeKind",
                BindingFlags.Static | BindingFlags.NonPublic).GetVisible());
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void GetNamespaceThrowsExceptionOnNull()
        {
            ExtensionMethods.GetNamespace(null);
        }

        [TestMethod]
        public void GetNamespaceResturnsExpectedValue()
        {
            var nameSpaceName = typeof(Type).GetNamespace();
            Assert.AreEqual(typeof(Type).GetType().GetNamespace(), nameSpaceName);
        }

        private class PrivateTestType
        {
        }
    }
}