using System;
using System.Diagnostics.CodeAnalysis;
using System.Reflection;
using Library.Data;
using Library.Logic.ViewModel;
using Library.Model;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace LibraryTests.Logic.TreeView.Tests
{
    [ExcludeFromCodeCoverage]
    [TestClass]
    public class TreeViewItemTests
    {
        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void TreeViewItemThrowsOnNull()
        {
            new AssemblyItem(null);
        }

        [TestMethod]
        public void TreeViewItemChildrenNullOnNotExpanded()
        {
            var typeMeta = new TypeMetadata(typeof(Type));
            var tvi = new TypeItem(typeMeta);
            Assert.IsNull(tvi.Children[0]);
        }

        [TestMethod]
        public void TreeViewItemChildrenNotNullOnExpanded()
        {
            var typeMeta = new TypeMetadata(typeof(Type));
            var tvi = new TypeItem(typeMeta)
            {
                IsExpanded = true
            };
            Assert.IsNotNull(tvi.Children);
        }

        [TestMethod]
        public void DictionaryNotChangingOnSameIMetadatas()
        {
            var typeMeta = new TypeMetadata(typeof(Type));
            var method = typeof(TreeViewItem).GetMethod("EnumerateRootChildren",
                BindingFlags.Instance | BindingFlags.NonPublic);
            var tvi = new TypeItem(typeMeta);
            method.Invoke(tvi, null);
            var initialDictSize = DataLoadedDictionary.Items.Count;
            method.Invoke(tvi, null);
            var nextlDictSize = DataLoadedDictionary.Items.Count;
            Assert.AreEqual(initialDictSize, nextlDictSize);
        }
    }
}