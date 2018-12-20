using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Tracing.Exceptions
{
    public class MEFTracingLoaderException : Exception
    {
        internal MEFTracingLoaderException() { }
        internal MEFTracingLoaderException(string message) : base(message) { }
        internal MEFTracingLoaderException(string message, Exception innerException) : base(message, innerException) { }
    }
}
