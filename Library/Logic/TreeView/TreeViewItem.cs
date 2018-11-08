//____________________________________________________________________________
//
//  Copyright (C) 2018, Mariusz Postol LODZ POLAND.
//
//  To be in touch join the community at GITTER: https://gitter.im/mpostol/TP
//  Modified for custom needs by Adrian Fijalkowski.
//____________________________________________________________________________

using Library.Data.Model;
using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace Library.Logic.TreeView
{
    public class TreeViewItem
    {
        IMetadata rootItem;
        public TreeViewItem()
        {
            //Children = new ObservableCollection<TreeViewItem>() { null };
            this.m_WasBuilt = false;
        }
        public TreeViewItem(IMetadata metadata) : base()
        {
            if (metadata == null)
                throw new System.ArgumentNullException("Metadata node can't be null");
            this.rootItem = metadata;
        }
        public string Name
        {
            get
            {
                return rootItem.Name;
            }
        }
        private ObservableCollection<TreeViewItem> _children = 
            new ObservableCollection<TreeViewItem>() { null };
        public ObservableCollection<TreeViewItem> Children
        {
            get
            {
                return this._children;
            }
            set
            {
                this._children = value;
            }
        }
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

        private bool m_WasBuilt;
        private bool m_IsExpanded;
        private void BuildMyself()
        {
            var list = new List<TreeViewItem>(enumerateRootChildren());
            list.ForEach(n => Children.Add(n));
        }
        private IEnumerable<TreeViewItem> enumerateRootChildren()
        {
            if(rootItem.Children != null)
            {
                foreach (IMetadata elem in rootItem.Children)
                {
                    if (elem != null)
                    {
                        TreeViewItem tvi = null;
                        if (DataLoadedDictionary.Items.TryGetValue(elem.GetHashCode(), out IMetadata returnValue))
                            tvi = new TreeViewItem(returnValue);
                        else
                        {
                            tvi = new TreeViewItem(elem);
                            DataLoadedDictionary.Items.Add(elem.GetHashCode(), elem);
                        }
                        yield return tvi;
                    }
                }
            }
        }
        public override string ToString()
        {
            return rootItem.ToString();
        }
    }
}

