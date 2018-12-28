using ModelContract;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace SerializationModel
{
    [DataContract]
    [KnownType(typeof(SerializationAssemblyMetadata))]
    [KnownType(typeof(SerializationAttributeMetadata))]
    [KnownType(typeof(SerializationMethodMetadata))]
    [KnownType(typeof(SerializationNamespaceMetadata))]
    [KnownType(typeof(SerializationParameterMetadata))]
    [KnownType(typeof(SerializationPropertyMetadata))]
    [KnownType(typeof(SerializationTypeMetadata))]
    public class SerializationAttributeMetadata : IAttributeMetadata
    {
        [DataMember]
        public string Name { get; }
        [DataMember]
        public int SavedHash { get; }
        public IEnumerable<IMetadata> Children => null;

        public SerializationAttributeMetadata(IAttributeMetadata attributeMetadata)
        {
            Name = attributeMetadata.Name;
            SavedHash = attributeMetadata.SavedHash;
        }
    }
}
