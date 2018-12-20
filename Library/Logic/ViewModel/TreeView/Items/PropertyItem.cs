using Library.Model;

namespace Library.Logic.ViewModel
{
    public class PropertyItem : TreeViewItem
    {
        private string _details;
        public override string Details
        {
            get
            {
                return _details;
            }
        }
        public PropertyItem(PropertyMetadata source) : base(source)
        {
            _details = $"Property: {source.Name} : {source.Type.Name}";
        }

        protected override TreeViewItem GetChildOfType(IMetadata metadata)
        {
            switch (metadata)
            {
                case AttributeMetadata attribute:
                    return new AttributeItem(attribute);
                case TypeMetadata type:
                    return new TypeItem(type);
            }
            return null;
        }
    }
}
