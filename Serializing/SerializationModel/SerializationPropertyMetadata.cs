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
            if (AlreadyMapped.TryGetValue(propertyMetadata.MyType.SavedHash, out var item))
            {
                MyType = item as ITypeMetadata;
            }
            else
            {
                ITypeMetadata newType = new SerializationTypeMetadata(propertyMetadata.MyType);
                MyType = newType;
                AlreadyMapped.Add(newType.SavedHash, newType);
            }
        }

        [DataMember(Name = "Type")] public ITypeMetadata MyType { get; private set; }

        [DataMember(Name = "Name")] public string Name { get; private set; }

        [DataMember(Name = "Hash")] public int SavedHash { get; private set; }

        public IEnumerable<IMetadata> Children => new[] {MyType};
    }
}