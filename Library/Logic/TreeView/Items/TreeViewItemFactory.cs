using Library.Data.Model;

namespace Library.Logic.TreeView.Items
{
    internal static class TreeViewItemFactory
    {
        public static TreeViewItem GetTreeViewItem(IMetadata metadata)
        {
            switch( metadata )
            {
                case AssemblyMetadata assembly:
                    return new AssemblyItem(assembly);
                case NamespaceMetadata namesp:
                    return new NamespaceItem(namesp);
                case AttributeMetadata attribute:
                    return new AttributeItem(attribute);
                case MethodMetadata method:
                    return new MethodItem(method);
                case ParameterMetadata param:
                    return new ParameterItem(param);
                case PropertyMetadata property:
                    return new PropertyItem(property);
                case TypeMetadata type:
                    return new TypeItem(type);
            }
            return null;
        }
    }
}
