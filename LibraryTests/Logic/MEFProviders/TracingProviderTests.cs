﻿using System.ComponentModel.Composition.Hosting;
using System.Diagnostics.CodeAnalysis;
using Library.Logic.MEFProviders;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Library.Data.MEFProviders.Tests
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
            Assert.IsNotNull(_sut.ProvideTracer());
        }
    }
}