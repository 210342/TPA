using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Tracing.DatabaseHandling
{
    public interface IDatabaseWriter
    {
        void Write(string data);
    }
}
