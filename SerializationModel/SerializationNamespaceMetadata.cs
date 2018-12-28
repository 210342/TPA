﻿using ModelContract;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace SerializationModel
{
    [DataContract]
    public class SerializationNamespaceMetadata : INamespaceMetadata
    {
        [DataMember]
        public IEnumerable<ITypeMetadata> Types { get; }
        [DataMember]
        public string Name { get; }
        [DataMember]
        public int SavedHash { get; }
        public IEnumerable<IMetadata> Children { get { return Types; } }

        public SerializationNamespaceMetadata(INamespaceMetadata namespaceMetadata)
        {
            Name = namespaceMetadata.Name;
            SavedHash = namespaceMetadata.SavedHash;
            Types = namespaceMetadata.Types;
        }
    }
}
