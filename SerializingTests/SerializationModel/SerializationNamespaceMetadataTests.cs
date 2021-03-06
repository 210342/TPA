﻿using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Reflection;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using ModelContract;

namespace SerializationModel.Tests
{
    [TestClass]
    [ExcludeFromCodeCoverage]
    public class SerializationNamespaceMetadataTests
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
            INamespaceMetadata tmp = new NamespaceTest();
            SerializationNamespaceMetadata sut = new SerializationNamespaceMetadata(tmp);
            Assert.IsTrue(tmp.Name.Equals(sut.Name));
            Assert.AreEqual(tmp.SavedHash, sut.SavedHash);
            Assert.AreEqual(tmp.Types.Count(), sut.Types.Count());
        }
    }

    [ExcludeFromCodeCoverage]
    internal class NamespaceTest : INamespaceMetadata
    {
        internal NamespaceTest()
        {
            Name = "name";
            SavedHash = 1;
            Types = Enumerable.Empty<ITypeMetadata>();
        }

        public IEnumerable<ITypeMetadata> Types { get; internal set; }
        public string Name { get; internal set; }
        public IEnumerable<IMetadata> Children { get; }
        public int SavedHash { get; internal set; }
    }
}