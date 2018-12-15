using Library.Model;

namespace Library.Logic.ViewModel
{
    public class NamespaceItem : TreeViewItem
    {
        public NamespaceItem(NamespaceMetadata source) : base(source)
        {
            
        }

        protected override TreeViewItem GetChildOfType(IMetadata metadata)
        {
            switch (metadata)
            {
                case NamespaceMetadata namesp:
                    return new NamespaceItem(namesp);
                case TypeMetadata type:
                    return new TypeItem(type);
            }
            return null;
        }
    }
}