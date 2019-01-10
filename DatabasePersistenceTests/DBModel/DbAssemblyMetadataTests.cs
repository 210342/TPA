using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace DatabasePersistence.DBModel.Tests
{
    [TestClass]
    public class DbAssemblyMetadataTests
    {
        [TestMethod]
        public void DbAssemblyMetadataTest()
        {
            Assert.IsNotNull(new DbAssemblyMetadata());
        }

        [TestMethod]
        public void DbAssemblyMetadataTest1()
        {
            DbAssemblyMetadata sut = new DbAssemblyMetadata
            {
                Namespaces = new[] {new DbNamespaceMetadata {Name = "test1"}}
            };
            Assert.IsNotNull(sut.NamespacesList);
            Assert.AreEqual(1, sut.NamespacesList.Count);
        }
    }
}