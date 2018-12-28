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
            Namespaces = assemblyMetadata.Namespaces;
            Name = assemblyMetadata.Name;
            SavedHash = assemblyMetadata.SavedHash;
        }
    }
}
