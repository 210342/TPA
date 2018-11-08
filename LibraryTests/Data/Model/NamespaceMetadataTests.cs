using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Diagnostics.CodeAnalysis;
using TPA.Reflection.Model;

namespace LibraryTests.Data.Model
{
    [ExcludeFromCodeCoverage]
    [TestClass]
    public class NamespaceMetadataTests
    {
        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void NamespaceMetadataThrowsOnNull()
        {
            new NamespaceMetadata(null, null);
        }
    }
}