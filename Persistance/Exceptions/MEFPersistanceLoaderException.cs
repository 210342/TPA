using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Persistance.Exceptions
{
    public class MEFPersistanceLoaderException : Exception
    {
        internal MEFPersistanceLoaderException() { }
        internal MEFPersistanceLoaderException(string message) : base(message) { }
        internal MEFPersistanceLoaderException(string message, Exception innerException) : base(message, innerException) { }
    }
}
