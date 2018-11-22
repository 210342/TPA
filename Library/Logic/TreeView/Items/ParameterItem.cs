using Library.Data.Model;
using Library.Data;

namespace Library.Logic.TreeView.Items
{
    public class ParameterItem : TreeViewItem
    {
        public ParameterItem(ParameterMetadata source) : base(source)
        {

        }
        protected override TreeViewItem GetTreeItem(IMetadata elem)
        {
            TreeViewItem tvi = null;
            if (DataLoadedDictionary.Items.TryGetValue(elem.GetHashCode(), out IMetadata returnValue))
            {
                tvi = TreeViewItemFactory.GetTreeViewItem(elem);
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
