using System.Collections.Generic;

namespace ModelContract
{
    public interface IMetadata
    {
        string Name { get; }
        IEnumerable<IMetadata> Children { get; }
        int SavedHash { get; }
    }
}