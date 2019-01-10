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
            if (AlreadyMapped.TryGetValue(parameterMetadata.TypeMetadata.SavedHash, out var item))
            {
                TypeMetadata = item as ITypeMetadata;
            }
            else
            {
                ITypeMetadata newType = new SerializationTypeMetadata(parameterMetadata.TypeMetadata);
                TypeMetadata = newType;
                AlreadyMapped.Add(newType.SavedHash, newType);
            }
        }

        [DataMember(Name = "Type")] public ITypeMetadata TypeMetadata { get; private set; }

        [DataMember(Name = "Name")] public string Name { get; private set; }

        [DataMember(Name = "Hash")] public int SavedHash { get; private set; }

        public IEnumerable<IMetadata> Children => new[] {TypeMetadata};
    }
}