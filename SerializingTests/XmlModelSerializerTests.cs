﻿using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.IO;
using System.Reflection;
using System.Runtime.Serialization;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Persistance;

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
            IEnumerable<Type> knownTypes = new List<Type>
                (new[] {typeof(TestType), typeof(TestValue), typeof(IParent)});
            _sut = new XmlModelSerializer
            {
                SerializationStream = _serializationStream
            };
            _sut.GetType().GetField("dataContractSerializer", BindingFlags.NonPublic | BindingFlags.Instance).
                SetValue(_sut, new DataContractSerializer(typeof(IParent), knownTypes, 100000, false, true, null));
        }

        [TestMethod]
        public void XmlModelSerializerTest()
        {
            Assert.IsNotNull(_sut);
            Assert.AreEqual(FileSystemDependency.Dependent, _sut.FileSystemDependency);
            _sut.Dispose();
        }

        [TestMethod]
        public void XmlModelSerializerStreamCtorTest()
        {
            _sut = new XmlModelSerializer(new MemoryStream());
            Assert.IsNotNull(_sut);
            Assert.AreEqual(FileSystemDependency.Dependent, _sut.FileSystemDependency);
            _sut.Dispose();
        }

        [TestMethod]
        public void XmlModelSerializerDisposeNullStreamTest()
        {
            _sut = new XmlModelSerializer(new MemoryStream());
            _sut.SerializationStream.Dispose();
            _sut.SerializationStream = null;
            _sut.Dispose();
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
            _sut.Target = "testing.txt";
            Assert.IsFalse(string.IsNullOrEmpty(_sut.Target));
            Assert.IsNotNull(_sut.SerializationStream);
            Assert.IsTrue(_sut.SerializationStream is FileStream);
            Stream oldStream = _sut.SerializationStream;
            _sut.Target = "testing.txt";
            Assert.IsFalse(_sut.SerializationStream.Equals(oldStream));
            _sut.SerializationStream.Dispose();
            _sut.SerializationStream = null;
            _sut.Target = "testing.txt";
            Assert.IsNotNull(_sut.SerializationStream);
        }

        public interface IParent
        {
            IEnumerable<TestValue> Values { get; set; }
        }

        [ExcludeFromCodeCoverage]
        [DataContract(Name ="type")]
        public class TestType : IParent
        {
            [DataMember]
            public string Name { get; set; }
            [DataMember]
            public IEnumerable<TestValue> Values { get; set; }
        }

        [ExcludeFromCodeCoverage]
        [DataContract(Name ="value")]
        public class TestValue : IParent
        {
            [DataMember]
            public string Name { get; set; }
            [DataMember]
            public string ValueType { get; set; }
            [DataMember]
            public IEnumerable<TestValue> Values { get; set; }
        }
    }
}