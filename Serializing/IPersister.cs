using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Serializing
{
    public interface IPersister
    {
        void Save(object obj);
        object Load(string path);
    }
}
