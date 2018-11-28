using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Serializing
{
    public interface IPersister
    {
        string SourceName { get; set; }

        void Save(object obj);
        object Load();
    }
}
