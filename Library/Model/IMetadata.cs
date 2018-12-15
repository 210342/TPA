using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Library.Model
{
    public interface IMetadata
    {
        string Name { get; }
        string Details { get; }
        IEnumerable<IMetadata> Children { get; }
    }
}
