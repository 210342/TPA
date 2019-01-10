using System;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Reflection;
using Library.Model;
using Microsoft.VisualStudio.TestTools.UnitTesting;

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

        [TestMethod]
        public void CopyCtorTest()
        {
            var tmpass = new AssemblyMetadata(Assembly.GetExecutingAssembly());
            var tmp = tmpass.Namespaces.First();
            var sut = new NamespaceMetadata(tmp);
            Assert.IsTrue(tmp.Name.Equals(sut.Name));
            Assert.AreEqual(tmp.SavedHash, sut.SavedHash);
            Assert.AreEqual(tmp.Types.Count(), sut.Types.Count());
        }
    }
}