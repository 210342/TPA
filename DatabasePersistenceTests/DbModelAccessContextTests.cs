using Microsoft.VisualStudio.TestTools.UnitTesting;
using DatabasePersistence;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data.Entity;
using System.Threading.Tasks;

namespace DatabasePersistence.Tests
{
    [TestClass()]
    public class DbModelAccessContextTests
    {
        [TestMethod()]
        public void DbModelAccessContextTest()
        {
            Assert.IsNotNull(new DbModelAccessContext().Database);
            Assert.IsNotNull(new DbModelAccessContext().Assemblies);
        }
    }
}