using Microsoft.VisualStudio.TestTools.UnitTesting;
using Library.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ModelContract;

namespace Library.Data.Tests
{
    [TestClass()]
    public class ModelMapperTests
    {
        /*
        [TestMethod()]
        public void MapFromBaseToSerializationTest()
        {
            Reflector originalRoot = new Reflector("TPA.ApplicationArchitecture.dll");
            ModelMapper sut = new ModelMapper();
            IAssemblyMetadata newRoot = sut.Map(originalRoot.m_AssemblyModel, typeof(SerializationTypeMetadata).Assembly);
            Assert.IsTrue(newRoot.GetType().IsAssignableFrom(typeof(SerializationAssemblyMetadata)));
        }
        */
    }
}