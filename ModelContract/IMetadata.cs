using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ModelContract
{
    public interface IMetadata
    {
        string Name { get; }
        IEnumerable<IMetadata> Children { get; }
        int SavedHash { get; }
    }
}
