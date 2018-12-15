using Library.Model;

namespace Library.Logic.ViewModel
{
    public class ParameterItem : TreeViewItem
    {
        public ParameterItem(ParameterMetadata source) : base(source)
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
