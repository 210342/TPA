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
            if (AlreadyMapped.TryGetValue(parameterMetadata.TypeMetadata.SavedHash, out IMetadata item))
            {
                TypeMetadata = item as ITypeMetadata;
            }
            else
            {
                // use temporary constructor to save its hash, retrieve actual object afterr all mapping has been done
                TypeMetadata = new SerializationTypeMetadata(
                    new SerializationTypeMetadata(
                        parameterMetadata.TypeMetadata.SavedHash, parameterMetadata.TypeMetadata.Name));
            }
        }

        [DataMember(Name = "Type")] public ITypeMetadata TypeMetadata { get; private set; }

        [DataMember(Name = "Name")] public string Name { get; private set; }

        [DataMember(Name = "Hash")] public int SavedHash { get; private set; }

        public IEnumerable<IMetadata> Children => new[] {TypeMetadata};

        public void MapTypes()
        {
            if (!TypeMetadata.Mapped && AlreadyMapped.TryGetValue(TypeMetadata.SavedHash, out IMetadata item))
            {
                TypeMetadata = item as ITypeMetadata;
            }
        }
    }
}