using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ModelContract
{
    public interface INamespaceMetadata : IMetadata
    {
        IEnumerable<ITypeMetadata> Types { get; }
    }
}
