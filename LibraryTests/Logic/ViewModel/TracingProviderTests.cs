using Library.Logic.ViewModel;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.ComponentModel.Composition.Hosting;
using System.Diagnostics.CodeAnalysis;

namespace LibraryTests.Logic.ViewModel.Tests
{
    [TestClass]
    [ExcludeFromCodeCoverage]
    public class TracingProviderTests
    {
        private readonly TracingProvider _sut = new TracingProvider();
        
        [TestMethod]
        public void ProvideTracerTest()
        {
            _sut.DirectoryCatalog = new DirectoryCatalog(".");
            Console.WriteLine(_sut.DirectoryCatalog.FullPath);
            foreach (string str in _sut.DirectoryCatalog.LoadedFiles)
                Console.WriteLine(str);
            Assert.IsNotNull(_sut.ProvideTracer());
        }
    }
}
