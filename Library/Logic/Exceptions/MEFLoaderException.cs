using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Library.Logic.Exceptions
{
    internal class MEFLoaderException : Exception
    {
        internal MEFLoaderException() { }
        internal MEFLoaderException(string message) : base(message) { }
        internal MEFLoaderException(string message, Exception innerException) : base(message, innerException) { }
    }
}
