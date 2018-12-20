using Library.Model;

namespace Library.Logic.ViewModel
{
    public class AssemblyItem : TreeViewItem
    {
        private string _details;
        public override string Details {
            get
            {
                return _details;
            }
        }

        public AssemblyItem(AssemblyMetadata source) : base(source)
        {
            _details =  $"Assembly name: {source.Name}, " +
                $"has {new System.Collections.Generic.List<IMetadata>(source.Children).Count} namespaces.";
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