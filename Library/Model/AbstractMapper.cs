using ModelContract;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Library.Model
{
    public abstract class AbstractMapper
    {
        protected static Dictionary<int, IMetadata> AlreadyMapped { get; } = new Dictionary<int, IMetadata>();
    }
}
