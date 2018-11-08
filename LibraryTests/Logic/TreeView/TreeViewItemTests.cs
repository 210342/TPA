using System;
using System.Diagnostics.CodeAnalysis;
using System.Reflection;
using Library.Logic.TreeView;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using TPA.Reflection.Model;

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
            new TreeViewItem(null);
        }
        [TestMethod]
        public void TreeViewItemChildrenNullOnNotExpanded()
        {
            TypeMetadata typeMeta = new TypeMetadata(typeof(Type));
            var tvi = new TreeViewItem(typeMeta);
            Assert.IsNull(tvi.Children[0]);
        }
        [TestMethod]
        public void TreeViewItemChildrenNotNullOnExpanded()
        {
            TypeMetadata typeMeta = new TypeMetadata(typeof(Type));
            var tvi = new TreeViewItem(typeMeta);
            tvi.IsExpanded = true;
            Assert.IsNotNull(tvi.Children);
        }
        [TestMethod]
        public void DictionaryNotChangingOnSameIMetadatas()
        {
            TypeMetadata typeMeta = new TypeMetadata(typeof(Type));
            MethodInfo method = typeof(TreeViewItem).GetMethod("enumerateRootChildren",
                BindingFlags.Instance | BindingFlags.NonPublic);
            var tvi = new TreeViewItem(typeMeta);
            method.Invoke(tvi, null);
            int initialDictSize = DataLoadedDictionary.Items.Count;
            method.Invoke(tvi, null);
            int nextlDictSize = DataLoadedDictionary.Items.Count;
            Assert.AreEqual(initialDictSize, nextlDictSize);
        }
    }
}
