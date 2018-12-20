using System;
using System.Collections.Generic;
using System.IO;

namespace Persistance
{
    public interface ISerializer : IPersister
    {
        Stream SerializationStream { get; set; }
        string SourceName { get; set; }
        IEnumerable<System.Type> KnownTypes { get; set; }
        System.Type NodeType { get; set; }
        bool Initialised { get; }

        void InitialiseSerialization();
    }
}
