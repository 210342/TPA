using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Library.Data.Model
{
    public interface IMetadata
    {
        string Name { get; }
        IEnumerable<IMetadata> Children { get; }
    }
}
