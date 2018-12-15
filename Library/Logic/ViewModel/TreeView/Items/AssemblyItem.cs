using Library.Model;

namespace Library.Logic.ViewModel
{
    public class AssemblyItem : TreeViewItem
    {
        public AssemblyItem(AssemblyMetadata source) : base(source)
        {
            
        }

        protected override TreeViewItem GetChildOfType(IMetadata metadata)
        {
            switch (metadata)
            {
                case NamespaceMetadata namesp:
                    return new NamespaceItem(namesp);
            }
            return null;
        }
    }
}