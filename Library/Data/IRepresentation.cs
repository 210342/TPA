using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Library.Data
{
    public interface IRepresentation
    {
        string Name { get; }
        string FullName { get; }
        IEnumerable<string> Children { get; }
        IEnumerable<string> Print();
    }
}
