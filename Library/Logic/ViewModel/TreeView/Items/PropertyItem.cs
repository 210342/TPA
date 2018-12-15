using Library.Model;

namespace Library.Logic.ViewModel
{
    public class PropertyItem : TreeViewItem
    {
        public PropertyItem(PropertyMetadata source) : base(source)
        {

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
