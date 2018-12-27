using Library.Model;
using ModelContract;

namespace Library.Logic.ViewModel
{
    public class ParameterItem : TreeViewItem
    {
        private string _details;
        public override string Details
        {
            get
            {
                return _details;
            }
        }

        public ParameterItem(ParameterMetadata source) : base(source)
        {
            _details = $"Parameter: {source.Name} : {source.TypeMetadata.Name}";
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
