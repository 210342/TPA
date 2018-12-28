using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ModelContract
{
    public static class MappingDictionary
    {
        public static Dictionary<int, IMetadata> AlreadyMapped { get; } = new Dictionary<int, IMetadata>();
    }
}
