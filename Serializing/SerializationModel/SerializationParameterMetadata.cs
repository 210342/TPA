using ModelContract;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace SerializationModel
{
    [DataContract(Name = "Parameter")]
    public class SerializationParameterMetadata : AbstractMapper, IParameterMetadata
    {
        [DataMember(Name = "Type")]
        public ITypeMetadata TypeMetadata { get; private set; }
        [DataMember(Name = "Name")]
        public string Name { get; private set; }
        [DataMember(Name = "Hash")]
        public int SavedHash { get; private set; }
        public IEnumerable<IMetadata> Children
        {
            get
            {
                return new[] { TypeMetadata };
            }
        }

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
                ITypeMetadata newType = new SerializationTypeMetadata(parameterMetadata.TypeMetadata);
                TypeMetadata = newType;
                AlreadyMapped.Add(newType.SavedHash, newType);
            }
        }
    }
}
