using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using TPA.Reflection.Model;

namespace LibraryTests.Data.Model
{
    [TestClass]
    public class AssemblyMetadataTests
    {

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void AssemblyMetadataExceptionOnNull() => new AssemblyMetadata(null);
    }
}
