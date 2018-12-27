using Library.Model;
using ModelContract;
using System.Linq;

namespace Library.Logic.ViewModel
{
    public class NamespaceItem : TreeViewItem
    {
        private string _details;
        public override string Details
        {
            get
            {
                return _details;
            }
        }

        public NamespaceItem(NamespaceMetadata source) : base(source)
        {
            _details = $"Namespace {source.Name}, contains {source.Children.Count()} types";
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