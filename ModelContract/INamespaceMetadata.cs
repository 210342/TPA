using System.Collections.Generic;

namespace ModelContract
{
    public interface INamespaceMetadata : IMetadata
    {
        IEnumerable<ITypeMetadata> Types { get; }
    }
}