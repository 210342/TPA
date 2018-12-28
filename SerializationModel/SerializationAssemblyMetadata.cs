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
    public class SerializationAssemblyMetadata : IAssemblyMetadata
    {
        [DataMember]
        public IEnumerable<INamespaceMetadata> Namespaces { get; }
        [DataMember]
        public string Name { get; }
        [DataMember]
        public int SavedHash { get; }
        public IEnumerable<IMetadata> Children { get { return Namespaces; } }

        public SerializationAssemblyMetadata(IAssemblyMetadata assemblyMetadata)
        {
            Name = assemblyMetadata.Name;
            SavedHash = assemblyMetadata.SavedHash;
            List<INamespaceMetadata> namespaces = new List<INamespaceMetadata>();
            foreach (INamespaceMetadata child in assemblyMetadata.Namespaces)
            {
                if (MappingDictionary.AlreadyMapped.TryGetValue(child.SavedHash, out IMetadata item))
                {
                    namespaces.Add(item as INamespaceMetadata);
                }
                else
                {
                    INamespaceMetadata newNamespace = new SerializationNamespaceMetadata(child);
                    namespaces.Add(newNamespace);
                    MappingDictionary.AlreadyMapped.Add(newNamespace.SavedHash, newNamespace);
                }
            }
            Namespaces = namespaces;
        }
    }
}
