﻿using System.Collections.Generic;
using System.Reflection;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using ModelContract;

namespace SerializationModel.Tests
{
    [TestClass]
    public class SerializationParameterMetadataTests
    {
        [TestInitialize]
        public void NullifyDictionary()
        {
            FieldInfo field = typeof(AbstractMapper).GetField("<AlreadyMapped>k__BackingField",
                BindingFlags.Static | BindingFlags.NonPublic);
            field.SetValue(null, new Dictionary<int, IMetadata>());
        }
        [TestMethod]
        public void CopyCtorTest()
        {
            ParameterTest tmp = new ParameterTest();
            SerializationParameterMetadata sut = new SerializationParameterMetadata(tmp);
            Assert.IsTrue(tmp.Name.Equals(sut.Name));
            Assert.AreEqual(tmp.SavedHash, sut.SavedHash);
            Assert.IsTrue(tmp.TypeMetadata.Name.Equals(sut.TypeMetadata.Name));
        }
    }

    internal class ParameterTest : IParameterMetadata
    {
        internal ParameterTest()
        {
            Name = "name";
            SavedHash = 1;
            TypeMetadata = new TypeTest("type");
        }

        public ITypeMetadata TypeMetadata { get; internal set; }
        public string Name { get; internal set; }
        public IEnumerable<IMetadata> Children { get; }
        public int SavedHash { get; internal set; }

        public void MapTypes() { }
    }
}