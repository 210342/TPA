using Library.Data.Model;
using Library.Data;

namespace Library.Logic.TreeView.Items
{
    public class TypeItem : TreeViewItem
    {
        public TypeItem(TypeMetadata source) : base(source)
        {

        }
        protected override TreeViewItem GetTreeItem(IMetadata elem)
        {
            TreeViewItem tvi = null;
            if (DataLoadedDictionary.Items.TryGetValue(elem.GetHashCode(), out IMetadata returnValue))
            {
                tvi = TreeViewItemFactory.GetTreeViewItem(returnValue);
            }
            else
            {
                tvi = TreeViewItemFactory.GetTreeViewItem(elem);
                DataLoadedDictionary.Items.Add(elem.GetHashCode(), elem);
            }
            return tvi;
        }
    }
}
