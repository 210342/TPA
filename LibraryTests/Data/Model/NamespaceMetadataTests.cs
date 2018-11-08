using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using TPA.Reflection.Model;

namespace LibraryTests.Data.Model
{
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