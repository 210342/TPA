using System.Collections.Generic;

namespace ModelContract
{
    public interface IAssemblyMetadata : IMetadata
    {
        IEnumerable<INamespaceMetadata> Namespaces { get; }
    }
}