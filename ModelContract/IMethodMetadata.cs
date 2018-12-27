using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ModelContract
{
    public interface IMethodMetadata : IMetadata
    {
        IEnumerable<ITypeMetadata> GenericArguments { get; }
        ITypeMetadata ReturnType { get; }
        bool IsExtension { get; }
        IEnumerable<IParameterMetadata> Parameters { get; }
        Tuple<AccessLevelEnum, AbstractEnum, StaticEnum, VirtualEnum> Modifiers { get; }
    }
}
