using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using TPA.Reflection.Model;

namespace LibraryTests.Data.Model
{
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