using System;
using System.Diagnostics.CodeAnalysis;
using System.Reflection;
using Library.Data;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace LibraryTests.Data
{
    [ExcludeFromCodeCoverage]
    [TestClass]
    public class ReflectorTests
    {
        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void ReflectorStringArgThrowsOnNull()
        {
            new Reflector((string) null);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void ReflectorAssemblyArgThrowsOnNull()
        {
            new Reflector((Assembly) null);
        }
    }
}