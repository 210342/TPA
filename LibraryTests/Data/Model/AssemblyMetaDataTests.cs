using System;
using System.Diagnostics.CodeAnalysis;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Library.Model;
using System.Reflection;
using System.Linq;

namespace LibraryTests.Data.Model
{
    [ExcludeFromCodeCoverage]
    [TestClass]
    public class AssemblyMetadataTests
    {

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void AssemblyMetadataExceptionOnNull() => new AssemblyMetadata(default(Assembly));

        [TestMethod]
        public void CopyCtorTest()
        {
            AssemblyMetadata tmp = new AssemblyMetadata(Assembly.GetExecutingAssembly());
            AssemblyMetadata sut = new AssemblyMetadata(tmp);
            Assert.IsTrue(tmp.Name.Equals(sut.Name));
            Assert.AreEqual(tmp.SavedHash, sut.SavedHash);
            Assert.AreEqual(tmp.Namespaces.Count(), sut.Namespaces.Count());
        }
    }
}
