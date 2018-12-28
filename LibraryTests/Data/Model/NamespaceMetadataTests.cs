using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Diagnostics.CodeAnalysis;
using Library.Model;
using System.Linq;
using System.Reflection;
using ModelContract;

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
            AssemblyMetadata tmpass = new AssemblyMetadata(Assembly.GetExecutingAssembly());
            INamespaceMetadata tmp = tmpass.Namespaces.First();
            NamespaceMetadata sut = new NamespaceMetadata(tmp);
            Assert.IsTrue(tmp.Name.Equals(sut.Name));
            Assert.AreEqual(tmp.SavedHash, sut.SavedHash);
            Assert.AreEqual(tmp.Types.Count(), sut.Types.Count());
        }
    }
}