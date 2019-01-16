using System;
using System.Linq;
using Library.Model;
using ModelContract;

namespace Library.Logic.ViewModel
{
    public class MethodItem : TreeViewItem
    {
        private readonly string _details;

        public MethodItem(MethodMetadata source) : base(source)
        {
            _details =
                $"{(source.ReturnType?.Name != null ? "Method: " + source.ReturnType.Name : "Constructor: ")} {source.Name}";
            if (source.Parameters.Count() == 0)
            {
                _details += $"() {Environment.NewLine}";
            }
            else
            {
                _details += "(";
                foreach (IParameterMetadata param in source.Parameters)
                    _details += $"{param.Name} : {param.MyType}, ";
                _details = _details.Remove(_details.Length - 2, 1) + ")\n";
            }

            _details += $"Modifiers: {source.ModifiersString()}";
            if (source.Name.StartsWith(".ctor"))
                FullName = $"[Constructor] " + Name;
            else if (source.Name.StartsWith("set_"))
                FullName = $"[Property Set Method] " + Name;
            else if (source.Name.StartsWith("get_"))
                FullName = $"[Property Get Method] " + Name;
            else
                FullName = $"[Method] " + Name;
        }

        public override string Details => _details;

        public override string FullName { get; }

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