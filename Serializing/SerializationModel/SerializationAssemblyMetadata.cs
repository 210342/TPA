using System.Collections.Generic;
using System.Runtime.Serialization;
using ModelContract;

namespace SerializationModel
{
    [DataContract(Name = "Assembly")]
    public class SerializationAssemblyMetadata : AbstractMapper, IAssemblyMetadata
    {
        public SerializationAssemblyMetadata(IAssemblyMetadata assemblyMetadata)
        {
            Name = assemblyMetadata.Name;
            SavedHash = assemblyMetadata.SavedHash;
            List<INamespaceMetadata> namespaces = new List<INamespaceMetadata>();
            foreach (INamespaceMetadata child in assemblyMetadata.Namespaces)
                if (AlreadyMapped.TryGetValue(child.SavedHash, out IMetadata item))
                {
                    namespaces.Add(item as INamespaceMetadata);
                }
                else
                {
                    INamespaceMetadata newNamespace = new SerializationNamespaceMetadata(child);
                    namespaces.Add(newNamespace);
                    AlreadyMapped.Add(newNamespace.SavedHash, newNamespace);
                }

            Namespaces = namespaces;
            foreach(INamespaceMetadata _namespace in Namespaces)
            {
                foreach(ITypeMetadata type in _namespace.Types)
                {
                    type.MapTypes();
                }
            }
        }

        [DataMember(Name = "Namespaces")] public IEnumerable<INamespaceMetadata> Namespaces { get; private set; }

        [DataMember(Name = "Name")] public string Name { get; private set; }

        [DataMember(Name = "Hash")] public int SavedHash { get; private set; }

        public IEnumerable<IMetadata> Children => Namespaces;
    }
}