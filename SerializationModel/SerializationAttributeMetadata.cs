using ModelContract;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace SerializationModel
{
    [DataContract(Name = "Attribute")]
    public class SerializationAttributeMetadata : IAttributeMetadata
    {
        [DataMember(Name = "Name")]
        public string Name { get; private set; }
        [DataMember(Name = "Hash")]
        public int SavedHash { get; private set; }
        public IEnumerable<IMetadata> Children => null;

        public SerializationAttributeMetadata(IAttributeMetadata attributeMetadata)
        {
            Name = attributeMetadata.Name;
            SavedHash = attributeMetadata.SavedHash;
        }
    }
}
