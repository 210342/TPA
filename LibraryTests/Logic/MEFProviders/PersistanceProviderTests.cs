using System.ComponentModel.Composition.Hosting;
using System.Diagnostics.CodeAnalysis;
using Library.Logic.MEFProviders;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Library.Data.MEFProviders.Tests
{
    [TestClass]
    [ExcludeFromCodeCoverage]
    public class PersistanceProviderTests
    {
        private readonly PersistenceProvider _sut = new PersistenceProvider();

        [TestMethod]
        public void ProvidePersisterTest()
        {
            _sut.DirectoryCatalog = new DirectoryCatalog(".");
            Assert.IsNotNull(_sut.ProvidePersister());
        }
    }
}