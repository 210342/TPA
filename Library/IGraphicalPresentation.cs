using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Library
{
    interface IGraphicalPresentation
    {
        string ClassName { get; }
        List<string> ClassProperties { get; }
        List<string> ClassMethods { get; }
        List<string> ClassAttributes { get; }
        List<string> ClassFields { get; }
    }
}
