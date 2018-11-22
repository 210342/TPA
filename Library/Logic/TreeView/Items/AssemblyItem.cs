using Library.Data;
using Library.Data.Model;

namespace Library.Logic.TreeView.Items
{
    public class AssemblyItem : TreeViewItem
    {
        public AssemblyItem(AssemblyMetadata source) : base(source)
        {
            
        }
        protected override TreeViewItem GetTreeItem(IMetadata elem)
        {
            TreeViewItem tvi = null;

            if (DataLoadedDictionary.Items.TryGetValue(elem.GetHashCode(), out IMetadata returnValue))
                tvi = TreeViewItemFactory.GetTreeViewItem(returnValue);
            else
            {
                if (!(elem is NamespaceMetadata))
                    throw new System.InvalidOperationException($"Can't add object of typ {elem.GetType()} " +
                        $"as NamespaceMetadata.");
                tvi = TreeViewItemFactory.GetTreeViewItem(elem);
                DataLoadedDictionary.Items.Add(elem.GetHashCode(), elem);
            }
            
            return tvi;
        }
    }
}