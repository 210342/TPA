using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.IO;

namespace Serializing.Tests
{
    [ExcludeFromCodeCoverage]
    [TestClass()]
    public class XmlModelSerializerTests
    {
        XmlModelSerializer _sut = null;
        Stream _serializationStream;

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


        [TestInitialize]
        public void SetUp()
        {
            List<Type> knownTypes = new List<Type>
                (new Type[] { typeof(TestType), typeof(TestValue) });
            _sut = new XmlModelSerializer(knownTypes, typeof(IParent));
            _serializationStream = new MemoryStream();
            _sut.SerializationStream = _serializationStream;
        }

        [TestMethod()]
        public void XmlModelSerializerTest()
        {
            Assert.IsNotNull(_sut);
        }

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

        [TestMethod]
        public void SourceNameSetter()
        {
            _sut.SourceName = "testing.txt";
            Assert.IsFalse(string.IsNullOrEmpty(_sut.SourceName));
            Assert.IsNotNull(_sut.SerializationStream);
            Assert.IsTrue(_sut.SerializationStream is FileStream);
        }
    }
}