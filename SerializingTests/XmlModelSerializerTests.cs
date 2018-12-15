using Library.Data;
using Library.Model;
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

        [TestInitialize]
        public void SetUp()
        {
            List<Type> knownTypes = new List<Type>
                (new Type[] { typeof(MethodMetadata), typeof(ParameterMetadata),
                    typeof(TypeMetadata), typeof(AttributeMetadata),
                    typeof(PropertyMetadata), typeof(NamespaceMetadata),
                    typeof(AssemblyMetadata)});
            _sut = new XmlModelSerializer(knownTypes, typeof(IMetadata));
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
            Reflector reflector = new Reflector(System.Reflection.Assembly.GetAssembly(this.GetType()));
            object original = reflector.m_AssemblyModel;
            _sut.Save(original);
            Assert.AreNotEqual(0, _serializationStream.Length);
        }

        [TestMethod()]
        public void StreamLoadTest()
        {
            Reflector reflector = new Reflector(System.Reflection.Assembly.GetAssembly(this.GetType()));
            object original = reflector.m_AssemblyModel;
            _sut.Save(original);
            object loaded = _sut.Load();
            Assert.IsTrue(loaded is AssemblyMetadata);
            Assert.IsFalse((loaded as AssemblyMetadata).Children == null);
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