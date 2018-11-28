//____________________________________________________________________________
//
//  Copyright (C) 2018, Mariusz Postol LODZ POLAND.
//
//  To be in touch join the community at GITTER: https://gitter.im/mpostol/TP
//  Modified for custom needs by Adrian Fijalkowski.
//____________________________________________________________________________

using Library.Data.Model;
using System.Collections.Generic;

namespace Library.Logic.TreeView
{
    public abstract class TreeViewItem
    {
        private bool m_WasBuilt;
        private bool m_IsExpanded;

        public string Type => this.GetType().ToString();
        public IMetadata ModelObject => Metadata;
        public string Name { get; protected set; }
        public string Details { get; protected set; }
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
            this.Details = metadata.Details;
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
                List<TreeViewItem> tmp = new List<TreeViewItem>();
                foreach (IMetadata elem in Metadata.Children)
                {
                    if (elem != null)
                    {
                        //yield return GetTreeItem(elem);
                        tmp.Add(GetTreeItem(elem));
                    }
                }
                return tmp;
            }
            else return null;
        }
        protected abstract TreeViewItem GetTreeItem(IMetadata elem);

        public override string ToString()
        {
            return Name;
        }
    }
}

