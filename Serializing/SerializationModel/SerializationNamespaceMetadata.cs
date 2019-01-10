using System.Collections.Generic;
using System.Runtime.Serialization;
using ModelContract;

namespace SerializationModel
{
    [DataContract(Name = "Namespace")]
    public class SerializationNamespaceMetadata : AbstractMapper, INamespaceMetadata
    {
        public SerializationNamespaceMetadata(INamespaceMetadata namespaceMetadata)
        {
            Name = namespaceMetadata.Name;
            SavedHash = namespaceMetadata.SavedHash;
            var types = new List<ITypeMetadata>();
            foreach (var child in namespaceMetadata.Types)
                if (AlreadyMapped.TryGetValue(child.SavedHash, out var item))
                {
                    types.Add(item as ITypeMetadata);
                }
                else
                {
                    ITypeMetadata newType = new SerializationTypeMetadata(child);
                    types.Add(newType);
                    AlreadyMapped.Add(newType.SavedHash, newType);
                }

            Types = types;
        }

        [DataMember(Name = "Types")] public IEnumerable<ITypeMetadata> Types { get; private set; }

        [DataMember(Name = "Name")] public string Name { get; private set; }

        [DataMember(Name = "Hash")] public int SavedHash { get; private set; }

        public IEnumerable<IMetadata> Children => Types;
    }
}