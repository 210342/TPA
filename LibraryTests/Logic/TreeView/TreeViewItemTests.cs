using System;
using System.Diagnostics.CodeAnalysis;
using System.Reflection;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Library.Data.Model;
using Library.Logic.TreeView.Items;
using Library.Data;
using Library.Logic.TreeView;

namespace LibraryTests.Logic.TreeView
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
            TypeMetadata typeMeta = new TypeMetadata(typeof(Type));
            var tvi = new TypeItem(typeMeta);
            Assert.IsNull(tvi.Children[0]);
        }
        [TestMethod]
        public void TreeViewItemChildrenNotNullOnExpanded()
        {
            TypeMetadata typeMeta = new TypeMetadata(typeof(Type));
            var tvi = new TypeItem(typeMeta);
            tvi.IsExpanded = true;
            Assert.IsNotNull(tvi.Children);
        }
        [TestMethod]
        public void DictionaryNotChangingOnSameIMetadatas()
        {
            TypeMetadata typeMeta = new TypeMetadata(typeof(Type));
            MethodInfo method = typeof(TreeViewItem).GetMethod("EnumerateRootChildren",
                BindingFlags.Instance | BindingFlags.NonPublic);
            var tvi = new TypeItem(typeMeta);
            method.Invoke(tvi, null);
            int initialDictSize = DataLoadedDictionary.Items.Count;
            method.Invoke(tvi, null);
            int nextlDictSize = DataLoadedDictionary.Items.Count;
            Assert.AreEqual(initialDictSize, nextlDictSize);
        }
    }
}
