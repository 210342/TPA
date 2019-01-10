using System.Collections.Generic;
using System.Runtime.Serialization;
using ModelContract;

namespace SerializationModel
{
    [DataContract]
    public abstract class AbstractMapper
    {
        protected static Dictionary<int, IMetadata> AlreadyMapped { get; } = new Dictionary<int, IMetadata>();
    }
}