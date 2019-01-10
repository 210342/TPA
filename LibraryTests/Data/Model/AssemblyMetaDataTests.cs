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
    public class AssemblyMetadataTests
    {
        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void AssemblyMetadataExceptionOnNull()
        {
            new AssemblyMetadata(default(Assembly));
        }

        [TestMethod]
        public void CopyCtorTest()
        {
            var tmp = new AssemblyMetadata(Assembly.GetExecutingAssembly());
            var sut = new AssemblyMetadata(tmp);
            Assert.IsTrue(tmp.Name.Equals(sut.Name));
            Assert.AreEqual(tmp.SavedHash, sut.SavedHash);
            Assert.AreEqual(tmp.Namespaces.Count(), sut.Namespaces.Count());
        }
    }
}