using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ModelContract
{
    public interface IAssemblyMetadata : IMetadata
    {
        IEnumerable<INamespaceMetadata> Namespaces { get; } 
    }
}
