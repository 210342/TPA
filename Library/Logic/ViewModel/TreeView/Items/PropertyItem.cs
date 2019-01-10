using Library.Model;
using ModelContract;

namespace Library.Logic.ViewModel
{
    public class PropertyItem : TreeViewItem
    {
        public PropertyItem(PropertyMetadata source) : base(source)
        {
            Details = $"Property: {source.Name} : {source.MyType.Name}";
        }

        public override string Details { get; }

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