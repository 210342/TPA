using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.IO;
using System.Reflection;
using System.Runtime.Serialization;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Persistence;

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
            _sut.GetType().GetField("dataContractSerializer", BindingFlags.NonPublic | BindingFlags.Instance)?.
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