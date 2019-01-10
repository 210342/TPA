using System.Collections.Generic;
using ModelContract;

namespace Library.Model
{
    public abstract class AbstractMapper
    {
        protected static Dictionary<int, IMetadata> AlreadyMapped { get; } = new Dictionary<int, IMetadata>();
    }
}