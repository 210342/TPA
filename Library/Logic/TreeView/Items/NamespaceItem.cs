using Library.Data.Model;
using Library.Data;

namespace Library.Logic.TreeView.Items
{
    public class NamespaceItem : TreeViewItem
    {
        public NamespaceItem(NamespaceMetadata source) : base(source)
        {
            
        }
        protected override TreeViewItem GetTreeItem(IMetadata elem)
        {
            TreeViewItem tvi = null;

            if (DataLoadedDictionary.Items.TryGetValue(elem.GetHashCode(), out IMetadata returnValue))
                tvi = TreeViewItemFactory.GetTreeViewItem(returnValue);
            else
            {
                if (!(elem is TypeMetadata))
                    throw new System.InvalidOperationException($"Can't add object of typ {elem.GetType()} " +
                        $"as TypeMetadata.");
                tvi = TreeViewItemFactory.GetTreeViewItem(elem);
                DataLoadedDictionary.Items.Add(elem.GetHashCode(), elem);
            }

            return tvi;
        }
    }
}