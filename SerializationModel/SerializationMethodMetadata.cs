using ModelContract;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace SerializationModel
{
    [DataContract]
    public class SerializationMethodMetadata : IMethodMetadata
    {
        [DataMember]
        public IEnumerable<ITypeMetadata> GenericArguments { get; }
        [DataMember]
        public ITypeMetadata ReturnType { get; }
        [DataMember]
        public bool IsExtension { get; }
        [DataMember]
        public IEnumerable<IParameterMetadata> Parameters { get; }
        [DataMember]
        public Tuple<AccessLevelEnum, AbstractEnum, StaticEnum, VirtualEnum> Modifiers { get; }
        [DataMember]
        public string Name { get; }
        [DataMember]
        public int SavedHash { get; }
        public IEnumerable<IMetadata> Children { get; private set; }

        public SerializationMethodMetadata(IMethodMetadata methodMetadata)
        {
            Name = methodMetadata.Name;
            ReturnType = methodMetadata.ReturnType;
            IsExtension = methodMetadata.IsExtension;
            Parameters = methodMetadata.Parameters;
            SavedHash = methodMetadata.SavedHash;
            Modifiers = methodMetadata.Modifiers;
            GenericArguments = methodMetadata.GenericArguments;
            FillChildren(new StreamingContext());
        }

        [OnDeserialized]
        private void FillChildren(StreamingContext context)
        {
            List<IMetadata> elems = new List<IMetadata> { ReturnType };
            elems.AddRange(Parameters);
            Children = elems;
        }
    }
}
