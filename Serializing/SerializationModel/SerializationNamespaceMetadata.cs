using ModelContract;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace SerializationModel
{
    [DataContract(Name = "Namespace")]
    public class SerializationNamespaceMetadata : AbstractMapper, INamespaceMetadata
    {
        [DataMember(Name = "Types")]
        public IEnumerable<ITypeMetadata> Types { get; private set; }
        [DataMember(Name = "Name")]
        public string Name { get; private set; }
        [DataMember(Name = "Hash")]
        public int SavedHash { get; private set; }
        public IEnumerable<IMetadata> Children { get { return Types; } }

        public SerializationNamespaceMetadata(INamespaceMetadata namespaceMetadata)
        {
            Name = namespaceMetadata.Name;
            SavedHash = namespaceMetadata.SavedHash;
            List<ITypeMetadata> types = new List<ITypeMetadata>();
            foreach (ITypeMetadata child in namespaceMetadata.Types)
            {
                if (AlreadyMapped.TryGetValue(child.SavedHash, out IMetadata item))
                {
                    types.Add(item as ITypeMetadata);
                }
                else
                {
                    ITypeMetadata newType = new SerializationTypeMetadata(child);
                    types.Add(newType);
                    AlreadyMapped.Add(newType.SavedHash, newType);
                }
            }
            Types = types;
        }
    }
}
