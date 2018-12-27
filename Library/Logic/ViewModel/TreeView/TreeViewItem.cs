//____________________________________________________________________________
//
//  Copyright (C) 2018, Mariusz Postol LODZ POLAND.
//
//  To be in touch join the community at GITTER: https://gitter.im/mpostol/TP
//  Modified for custom needs by Adrian Fijalkowski.
//____________________________________________________________________________

using Library.Data;
using Library.Model;
using ModelContract;
using System.Collections.Generic;

namespace Library.Logic.ViewModel
{
    public abstract class TreeViewItem
    {
        private bool m_WasBuilt;
        private bool m_IsExpanded;

        public string Type => this.GetType().ToString();
        public IMetadata ModelObject => Metadata;
        public string Name { get; protected set; }
        public abstract string Details { get; }
        protected IMetadata Metadata { get; set; }
        public List<TreeViewItem> Children { get; protected set; } = new List<TreeViewItem>() { null };
        public bool IsExpanded
        {
            get { return m_IsExpanded; }
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

        private TreeViewItem()
        {
            this.m_WasBuilt = false;
        }
        public TreeViewItem(IMetadata metadata) : base()
        {
            if (metadata == null)
                throw new System.ArgumentNullException("Metadata node can't be null");
            this.Name = metadata.Name;
            this.Metadata = metadata;
        }

        private void BuildMyself()
        {
            var list = new List<TreeViewItem>(EnumerateRootChildren());
            list.ForEach(n => Children.Add(n));
        }

        private IEnumerable<TreeViewItem> EnumerateRootChildren()
        {
            if (Metadata.Children != null)
            {
                foreach (IMetadata elem in Metadata.Children)
                {
                    if (elem != null)
                    {
                        yield return GetTreeItem(elem);
                    }
                }
            }
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

