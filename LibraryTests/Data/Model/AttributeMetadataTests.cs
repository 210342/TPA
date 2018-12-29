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
            new AttributeMetadata(default(Attribute));
        }

        [TestMethod]
        public void CopyCtorTest()
        {
            AttributeMetadata tmp = new AttributeMetadata(new TestClassAttribute());
            AttributeMetadata sut = new AttributeMetadata(tmp);
            Assert.IsTrue(tmp.Name.Equals(sut.Name));
            Assert.AreEqual(tmp.SavedHash, sut.SavedHash);
        }
    }
}
