using System.Linq;
using Library.Model;
using ModelContract;

namespace Library.Logic.ViewModel
{
    public class NamespaceItem : TreeViewItem
    {
        public NamespaceItem(NamespaceMetadata source) : base(source)
        {
            Details = $"Namespace {source.Name}, contains {source.Children.Count()} types";
        }

        public override string Details { get; }

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