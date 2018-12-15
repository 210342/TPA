using Library.Model;

namespace Library.Logic.ViewModel
{
    public class TypeItem : TreeViewItem
    {
        public TypeItem(TypeMetadata source) : base(source)
        {

        }

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
