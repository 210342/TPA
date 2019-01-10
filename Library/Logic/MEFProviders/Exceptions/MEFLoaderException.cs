using System;

namespace Library.Logic.MEFProviders.Exceptions
{
    public class MEFLoaderException : Exception
    {
        internal MEFLoaderException()
        {
        }

        internal MEFLoaderException(string message) : base(message)
        {
        }

        internal MEFLoaderException(string message, Exception innerException) : base(message, innerException)
        {
        }
    }
}