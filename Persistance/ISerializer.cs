using System;
using System.Collections.Generic;
using System.IO;

namespace Persistance
{
    public interface ISerializer : IPersister
    {
        Stream SerializationStream { get; set; }
        string SourceName { get; set; }
        System.Type NodeType { get; set; }
        bool IsInitialised { get; }
        IEnumerable<Type> KnownTypes { get; set; }

        void InitialiseSerialization();
    }
}
