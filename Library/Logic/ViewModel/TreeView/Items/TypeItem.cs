using Library.Model;
using ModelContract;

namespace Library.Logic.ViewModel
{
    public class TypeItem : TreeViewItem
    {
        private readonly string _details;

        public TypeItem(TypeMetadata source) : base(source)
        {
            _details =
                $"Type: {source.Name}{(source.BaseType != null ? ", extends " + source.BaseType.Name : string.Empty)}";
            if (source.ImplementedInterfaces != null)
            {
                _details += ",implements ";
                foreach (ITypeMetadata intf in source.ImplementedInterfaces)
                    _details += $"{intf.Name}, ";
            }

            _details += $"\nType Kind: {source.TypeKind.ToString()}\n";
            _details += $"Modifiers: {source.ModifiersString()}.";
        }

        public override string Details => _details;

        protected override TreeViewItem GetChildOfType(IMetadata metadata)
        {
            switch (metadata)
            {
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