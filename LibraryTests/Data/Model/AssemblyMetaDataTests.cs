using System;
using System.Diagnostics.CodeAnalysis;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Library.Data.Model;

namespace LibraryTests.Data.Model
{
    [ExcludeFromCodeCoverage]
    [TestClass]
    public class AssemblyMetadataTests
    {

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void AssemblyMetadataExceptionOnNull() => new AssemblyMetadata(null);
    }
}
