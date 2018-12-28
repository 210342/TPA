using ModelContract;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace SerializationModel
{
    [DataContract(Name = "Assembly")]
    public class SerializationAssemblyMetadata : IAssemblyMetadata
    {
        [DataMember(Name = "Namespaces")]
        public IEnumerable<INamespaceMetadata> Namespaces { get; private set; }
        [DataMember(Name = "Name")]
        public string Name { get; private set; }
        [DataMember(Name = "Hash")]
        public int SavedHash { get; private set; }
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
