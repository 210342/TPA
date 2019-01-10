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
            var namespaces = new List<INamespaceMetadata>();
            foreach (var child in assemblyMetadata.Namespaces)
                if (AlreadyMapped.TryGetValue(child.SavedHash, out var item))
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
        }

        [DataMember(Name = "Namespaces")] public IEnumerable<INamespaceMetadata> Namespaces { get; private set; }

        [DataMember(Name = "Name")] public string Name { get; private set; }

        [DataMember(Name = "Hash")] public int SavedHash { get; private set; }

        public IEnumerable<IMetadata> Children => Namespaces;
    }
}