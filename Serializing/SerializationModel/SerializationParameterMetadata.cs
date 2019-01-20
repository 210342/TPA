using System.Collections.Generic;
using System.Runtime.Serialization;
using ModelContract;

namespace SerializationModel
{
    [DataContract(Name = "Parameter")]
    public class SerializationParameterMetadata : AbstractMapper, IParameterMetadata
    {
        public SerializationParameterMetadata(IParameterMetadata parameterMetadata)
        {
            Name = parameterMetadata.Name;
            SavedHash = parameterMetadata.SavedHash;
            if (AlreadyMapped.TryGetValue(parameterMetadata.MyType.SavedHash, out IMetadata item))
            {
                MyType = item as ITypeMetadata;
            }
            else
            {
                // use temporary constructor to save its hash, retrieve actual object afterr all mapping has been done
                MyType = new SerializationTypeMetadata(
                    new SerializationTypeMetadata(
                        parameterMetadata.MyType.SavedHash, parameterMetadata.MyType.Name));
            }
        }

        [DataMember(Name = "Type")] public ITypeMetadata MyType { get; private set; }

        [DataMember(Name = "Name")] public string Name { get; private set; }

        [DataMember(Name = "Hash")] public int SavedHash { get; private set; }

        public IEnumerable<IMetadata> Children => new[] {MyType};

        public void MapTypes()
        {
            if (AlreadyMapped.TryGetValue(MyType.SavedHash, out IMetadata item))
            {
                MyType = item as ITypeMetadata;
            }
        }
    }
}