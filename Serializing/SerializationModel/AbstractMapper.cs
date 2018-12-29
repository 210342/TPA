using ModelContract;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace SerializationModel
{
    [DataContract]
    public abstract class AbstractMapper
    {
        public static Dictionary<int, IMetadata> AlreadyMapped { get; } = new Dictionary<int, IMetadata>();
    }
}
