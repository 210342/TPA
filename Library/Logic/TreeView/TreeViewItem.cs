﻿//____________________________________________________________________________
//
//  Copyright (C) 2018, Mariusz Postol LODZ POLAND.
//
//  To be in touch join the community at GITTER: https://gitter.im/mpostol/TP
//____________________________________________________________________________

using Library.Data.Model;
using Library.Logic.TreeView;
using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace TP.GraphicalData.TreeView
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
            this.rootItem = metadata;
        }
        public string Name { get; set; }
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
                        if (DataLoadedDictionary.Items.TryGetValue(elem.GetHashCode(), out TreeViewItem returnValue))
                        {
                            returnValue.m_IsExpanded = false;
                            returnValue.m_WasBuilt = false;
                            yield return returnValue;
                            //yield return null;
                            //break;
                        }
                        else
                        {
                            var obj = new TreeViewItem(elem);
                            DataLoadedDictionary.Items.Add(elem.GetHashCode(), obj);
                            yield return obj;
                        }
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

