using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using TPA.Reflection;
using System.Reflection;
using System.Diagnostics.CodeAnalysis;

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
