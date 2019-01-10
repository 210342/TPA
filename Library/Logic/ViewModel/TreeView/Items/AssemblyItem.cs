using System.Linq;
using Library.Model;
using ModelContract;

namespace Library.Logic.ViewModel
{
    public class AssemblyItem : TreeViewItem
    {
        public AssemblyItem(AssemblyMetadata source) : base(source)
        {
            Details = $"Assembly name: {source.Name}, " +
                      "has " + (source.Children == null ? "0" : source.Children.Count().ToString()) + " namespaces.";
        }

        public override string Details { get; }

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