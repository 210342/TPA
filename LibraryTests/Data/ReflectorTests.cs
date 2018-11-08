using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Reflection;
using System.Diagnostics.CodeAnalysis;
using Library.Data;

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
            new Reflector((string)null);
        }
        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void ReflectorAssemblyArgThrowsOnNull()
        {
            new Reflector((Assembly)null);
        }
    }
}
