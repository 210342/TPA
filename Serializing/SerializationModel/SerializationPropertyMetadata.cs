using System.Collections.Generic;
using System.Runtime.Serialization;
using ModelContract;

namespace SerializationModel
{
    [DataContract(Name = "Property")]
    public class SerializationPropertyMetadata : AbstractMapper, IPropertyMetadata
    {
        public SerializationPropertyMetadata(IPropertyMetadata propertyMetadata)
        {
            Name = propertyMetadata.Name;
            SavedHash = propertyMetadata.SavedHash;
            if (AlreadyMapped.TryGetValue(propertyMetadata.MyType.SavedHash, out IMetadata item))
            {
                MyType = item as ITypeMetadata;
            }
            else
            {
                // use temporary constructor to save its hash, retrieve actual object afterr all mapping has been done
                MyType = new SerializationTypeMetadata(
                    new SerializationTypeMetadata(
                        propertyMetadata.MyType.SavedHash, propertyMetadata.MyType.Name));
            }
        }

        [DataMember(Name = "Type")] public ITypeMetadata MyType { get; private set; }

        [DataMember(Name = "Name")] public string Name { get; private set; }

        [DataMember(Name = "Hash")] public int SavedHash { get; private set; }

        public IEnumerable<IMetadata> Children => new[] {MyType};

        public void MapTypes()
        {
            if (MyType.Mapped && AlreadyMapped.TryGetValue(MyType.SavedHash, out IMetadata item))
            {
                MyType = item as ITypeMetadata;
            }
        }
    }
}