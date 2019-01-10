using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.IO;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Serializing.Tests
{
    [ExcludeFromCodeCoverage]
    [TestClass]
    public class XmlModelSerializerTests
    {
        private Stream _serializationStream;
        private XmlModelSerializer _sut;


        [TestInitialize]
        public void SetUp()
        {
            _serializationStream = new MemoryStream();
            List<Type> knownTypes = new List<Type>
                (new[] {typeof(TestType), typeof(TestValue)});
            _sut = new XmlModelSerializer
            {
                SerializationStream = _serializationStream
            };
        }

        [TestMethod]
        public void XmlModelSerializerTest()
        {
            Assert.IsNotNull(_sut);
        }

        /*
        [TestMethod()]
        public void StreamSaveTest()
        {
            object original = new TestType() { Name = "Type", Values = new[] { new TestValue() } };
            _sut.Save(original);
            Assert.AreNotEqual(0, _serializationStream.Length);
        }

        [TestMethod()]
        public void StreamLoadTest()
        {
            object original = new TestType() { Name = "Type", Values = new[] { new TestValue() } };
            _sut.Save(original);
            object loaded = _sut.Load();
            Assert.IsTrue(loaded is TestType);
            Assert.IsFalse((loaded as TestType).Values == null);
        }
        */
        [TestMethod]
        public void SourceNameSetter()
        {
            _sut.Target = "testing.txt";
            Assert.IsFalse(string.IsNullOrEmpty(_sut.Target));
            Assert.IsNotNull(_sut.SerializationStream);
            Assert.IsTrue(_sut.SerializationStream is FileStream);
        }

        public interface IParent
        {
            IEnumerable<TestValue> Values { get; set; }
        }

        public class TestType : IParent
        {
            public string Name { get; set; }
            public IEnumerable<TestValue> Values { get; set; }
        }

        public class TestValue : IParent
        {
            public string Name { get; set; }
            public string ValueType { get; set; }
            public IEnumerable<TestValue> Values { get; set; }
        }
    }
}