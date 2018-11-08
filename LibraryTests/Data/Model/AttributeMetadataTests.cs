

using Library.Data.Model;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;

namespace LibraryTests.Data.Model
{
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
