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
    public class SerializationPropertyMetadata : IPropertyMetadata
    {
        [DataMember]
        public ITypeMetadata MyType { get; }
        [DataMember]
        public string Name { get; }
        [DataMember]
        public int SavedHash { get; }
        public IEnumerable<IMetadata> Children
        {
            get
            {
                return new[] { MyType };
            }
        }

        public SerializationPropertyMetadata(IPropertyMetadata propertyMetadata)
        {
            Name = propertyMetadata.Name;
            SavedHash = propertyMetadata.SavedHash;
            if (MappingDictionary.AlreadyMapped.TryGetValue(propertyMetadata.MyType.SavedHash, out IMetadata item))
            {
                MyType = item as ITypeMetadata;
            }
            else
            {
                ITypeMetadata newType = new SerializationTypeMetadata(propertyMetadata.MyType);
                MyType = newType;
                MappingDictionary.AlreadyMapped.Add(newType.SavedHash, newType);
            }
        }
    }
}
