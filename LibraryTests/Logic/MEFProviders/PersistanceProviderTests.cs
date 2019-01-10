using System.ComponentModel.Composition.Hosting;
using Library.Logic.MEFProviders;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Library.Data.MEFProviders.Tests
{
    [TestClass]
    public class PersistanceProviderTests
    {
        private readonly PersistanceProvider _sut = new PersistanceProvider();

        [TestMethod]
        public void ProvidePersisterTest()
        {
            _sut.DirectoryCatalog = new DirectoryCatalog(".");
            Assert.IsNotNull(_sut.ProvidePersister());
        }
    }
}