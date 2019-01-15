using System;
using System.Collections.Generic;
using Library.Data;
using ModelContract;

namespace Library.Logic.ViewModel
{
    public abstract class TreeViewItem
    {
        private bool m_IsExpanded;
        private bool m_WasBuilt;

        private TreeViewItem()
        {
            m_WasBuilt = false;
        }

        public TreeViewItem(IMetadata metadata)
        {
            if (metadata == null)
                throw new ArgumentNullException("Metadata node can't be null");
            Name = metadata.Name;
            Metadata = metadata;
        }

        public string Type => GetType().ToString();
        public IMetadata ModelObject => Metadata;
        public string Name { get; protected set; }
        public abstract string Details { get; }
        public abstract string FullName { get; }
        protected IMetadata Metadata { get; set; }
        public List<TreeViewItem> Children { get; protected set; } = new List<TreeViewItem> {null};

        public bool IsExpanded
        {
            get => m_IsExpanded;
            set
            {
                m_IsExpanded = value;
                if (m_WasBuilt)
                    return;
                if (Children != null)
                    Children.Clear();
                BuildMyself();
                m_WasBuilt = true;
            }
        }

        private void BuildMyself()
        {
            List<TreeViewItem> list = new List<TreeViewItem>(EnumerateRootChildren());
            list.ForEach(n => Children.Add(n));
        }

        private IEnumerable<TreeViewItem> EnumerateRootChildren()
        {
            if (Metadata.Children != null)
                foreach (IMetadata elem in Metadata.Children)
                    if (elem != null)
                        yield return GetTreeItem(elem);
        }

        protected TreeViewItem GetTreeItem(IMetadata elem)
        {
            TreeViewItem tvi = null;
            if (DataLoadedDictionary.Items.TryGetValue(elem.GetHashCode(), out IMetadata returnValue))
            {
                tvi = GetChildOfType(returnValue);
            }
            else
            {
                tvi = GetChildOfType(elem);
                DataLoadedDictionary.Items.Add(elem.GetHashCode(), elem);
            }

            return tvi;
        }

        protected abstract TreeViewItem GetChildOfType(IMetadata metadata);

        public override string ToString()
        {
            return Name;
        }
    }
}