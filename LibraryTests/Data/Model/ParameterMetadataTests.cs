using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Diagnostics.CodeAnalysis;
using TPA.Reflection.Model;

namespace LibraryTests.Data.Model
{
    [ExcludeFromCodeCoverage]
    [TestClass]
    public class ParameterMetadataTests
    {
        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void ParameterMetadataThrowsOnNull()
        {
            new ParameterMetadata(null, null);
        }
    }
}