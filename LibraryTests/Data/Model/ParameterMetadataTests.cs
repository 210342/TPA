﻿using System;
using System.Diagnostics.CodeAnalysis;
using Library.Model;
using Microsoft.VisualStudio.TestTools.UnitTesting;

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

        [TestMethod]
        public void CopyCtorTest()
        {
            var tmp = new ParameterMetadata("asdf", new TypeMetadata(typeof(ParameterMetadataTests)));
            var sut = new ParameterMetadata(tmp);
            Assert.IsTrue(tmp.Name.Equals(sut.Name));
            Assert.AreEqual(tmp.SavedHash, sut.SavedHash);
            Assert.IsTrue(tmp.TypeMetadata.Name.Equals(sut.TypeMetadata.Name));
        }
    }
}