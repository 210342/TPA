using Microsoft.VisualStudio.TestTools.UnitTesting;
using DatabasePersistence.DBModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DatabasePersistence.DBModel.Tests
{
    [TestClass()]
    public class DbAssemblyMetadataTests
    {
        [TestMethod()]
        public void DbAssemblyMetadataTest()
        {
            Assert.IsNotNull(new DbAssemblyMetadata());
        }

        [TestMethod()]
        public void DbAssemblyMetadataTest1()
        {
            var sut = new DbAssemblyMetadata
            {
                Namespaces = new[] { new DbNamespaceMetadata() { Name = "test1" } }
            };
            Assert.IsNotNull(sut.NamespacesList);
            Assert.AreEqual(1, sut.NamespacesList.Count);
        }
    }
}