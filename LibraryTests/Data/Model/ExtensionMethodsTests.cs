﻿using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Diagnostics.CodeAnalysis;
using System.Reflection;
using Library.Data.Model;

namespace LibraryTests.Data.Model
{
    [ExcludeFromCodeCoverage]
    [TestClass]
    public class ExtensionMethodsTests
    {
        private class PrivateTestType {}

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void GetVisibleForTypeThrowsExceptionOnNull()
        {
            ExtensionMethods.GetVisible((Type)null);
        }
        [TestMethod]
        public void GetVisibleForTypeReturnTrue()
        {
            Assert.AreEqual(true, ExtensionMethods.GetVisible(typeof(System.Type)));
        }
        [TestMethod]
        public void GetVisibleForTypeReturnFalse()
        {
            Assert.AreEqual(false, ExtensionMethods.GetVisible(typeof(PrivateTestType)));
        }
        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void GetVisibleForMethodThrowsExceptionOnNull()
        {
            ExtensionMethods.GetVisible((MethodBase)null);
        }
        [TestMethod]
        public void GetVisibleForMethodReturnTrue()
        {
            Assert.AreEqual(true, ExtensionMethods.GetVisible(typeof(System.Type).GetMethod("ToString")));
        }
        [TestMethod]
        public void GetVisibleForMethodReturnFalse()
        {
            Assert.AreEqual(false, ExtensionMethods.GetVisible(typeof(TypeMetadata).GetMethod("GetTypeKind", 
                BindingFlags.Static | BindingFlags.NonPublic)));
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
            string nameSpaceName = ExtensionMethods.GetNamespace(typeof(Type));
            Assert.AreEqual(typeof(Type).GetType().GetNamespace(), nameSpaceName);
        }
    }
}
