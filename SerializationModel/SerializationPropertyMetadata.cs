using ModelContract;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace SerializationModel
{
    [DataContract(Name = "Property")]
    public class SerializationPropertyMetadata : IPropertyMetadata
    {
        [DataMember(Name = "Type")]
        public ITypeMetadata MyType { get; private set; }
        [DataMember(Name = "Name")]
        public string Name { get; private set; }
        [DataMember(Name = "Hash")]
        public int SavedHash { get; private set; }
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
