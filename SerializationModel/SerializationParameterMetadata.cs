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
    public class SerializationParameterMetadata : IParameterMetadata
    {
        [DataMember]
        public ITypeMetadata TypeMetadata { get; }
        public string Name { get; }
        [DataMember]
        public int SavedHash { get; }
        public IEnumerable<IMetadata> Children
        {
            get
            {
                return new[] { TypeMetadata };
            }
        }

        public SerializationParameterMetadata(IParameterMetadata parameterMetadata)
        {
            Name = parameterMetadata.Name;
            SavedHash = parameterMetadata.SavedHash;
            if (MappingDictionary.AlreadyMapped.TryGetValue(parameterMetadata.TypeMetadata.SavedHash, out IMetadata item))
            {
                TypeMetadata = item as ITypeMetadata;
            }
            else
            {
                ITypeMetadata newType = new SerializationTypeMetadata(parameterMetadata.TypeMetadata);
                TypeMetadata = newType;
                MappingDictionary.AlreadyMapped.Add(newType.SavedHash, newType);
            }
        }
    }
}
