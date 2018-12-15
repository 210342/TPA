using Library.Model;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Diagnostics.CodeAnalysis;

namespace LibraryTests.Data.Model
{
    [ExcludeFromCodeCoverage]
    [TestClass]
    public class AttributeMetadataTests
    {
        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void AttributeMetadataExceptionOnNull()
        {
            new AttributeMetadata(null);
        }
    }
}
