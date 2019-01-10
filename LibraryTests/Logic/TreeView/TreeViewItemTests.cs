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
            TypeMetadata typeMeta = new TypeMetadata(typeof(Type));
            TypeItem tvi = new TypeItem(typeMeta);
            Assert.IsNull(tvi.Children[0]);
        }

        [TestMethod]
        public void TreeViewItemChildrenNotNullOnExpanded()
        {
            TypeMetadata typeMeta = new TypeMetadata(typeof(Type));
            TypeItem tvi = new TypeItem(typeMeta)
            {
                IsExpanded = true
            };
            Assert.IsNotNull(tvi.Children);
        }

        [TestMethod]
        public void DictionaryNotChangingOnSameIMetadatas()
        {
            TypeMetadata typeMeta = new TypeMetadata(typeof(Type));
            MethodInfo method = typeof(TreeViewItem).GetMethod("EnumerateRootChildren",
                BindingFlags.Instance | BindingFlags.NonPublic);
            TypeItem tvi = new TypeItem(typeMeta);
            method.Invoke(tvi, null);
            int initialDictSize = DataLoadedDictionary.Items.Count;
            method.Invoke(tvi, null);
            int nextlDictSize = DataLoadedDictionary.Items.Count;
            Assert.AreEqual(initialDictSize, nextlDictSize);
        }
    }
}