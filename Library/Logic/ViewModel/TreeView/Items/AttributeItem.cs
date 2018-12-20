using Library.Model;

namespace Library.Logic.ViewModel
{
    public class AttributeItem : TreeViewItem
    {
        private string _details;
        public override string Details
        {
            get
            {
                return _details;
            }
        }
        public AttributeItem(AttributeMetadata source) : base(source)
        {
            _details = $"Attribute name: {source.Name}.";
        }

        protected override TreeViewItem GetChildOfType(IMetadata metadata)
        {
            switch (metadata)
            {
                case AttributeMetadata attribute:
                    return new AttributeItem(attribute);
                case ParameterMetadata param:
                    return new ParameterItem(param);
                case TypeMetadata type:
                    return new TypeItem(type);
            }
            return null;
        }
    }
}
