using Library.Model;
using ModelContract;
using System.Linq;

namespace Library.Logic.ViewModel
{
    public class MethodItem : TreeViewItem
    {
        private string _details;
        public override string Details
        {
            get
            {
                return _details;
            }
        }

        public MethodItem(MethodMetadata source) : base(source)
        {
            _details = $"{(source.ReturnType?.Name != null ? "Method: " + source.ReturnType.Name : "Constructor: ")} {source.Name}";
            if (source.Parameters.Count() == 0)
            {
                _details += $"() {System.Environment.NewLine}";
            }
            else
            {
                _details += $"(";
                foreach (var param in source.Parameters)
                {
                    _details += $"{param.Name} : {param.TypeMetadata}, ";
                }
                _details = _details.Remove(_details.Length - 2, 1) + ")\n";
            }
            _details += $"Modifiers: {source.ModifiersString()}";
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
