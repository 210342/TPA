using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.ComponentModel.Composition.Hosting;

namespace Persistance.Tests
{
    [TestClass()]
    public class PersistanceProviderTests
    {
        private readonly PersistanceProvider _sut = new PersistanceProvider();

        [TestMethod()]
        public void ProvidePersisterTest()
        {
            _sut.DirectoryCatalog = new DirectoryCatalog(".");
            Assert.IsNotNull(_sut.ProvidePersister());
        }
    }
}