using ModelContract;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace SerializationModel
{
    [DataContract
    [KnownType(typeof(SerializationAssemblyMetadata))]
    [KnownType(typeof(SerializationAttributeMetadata))]
    [KnownType(typeof(SerializationMethodMetadata))]
    [KnownType(typeof(SerializationNamespaceMetadata))]
    [KnownType(typeof(SerializationParameterMetadata))]
    [KnownType(typeof(SerializationPropertyMetadata))]
    [KnownType(typeof(SerializationTypeMetadata))]
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
            SavedHash = methodMetadata.SavedHash;
            IsExtension = methodMetadata.IsExtension;
            Modifiers = methodMetadata.Modifiers;

            // Generic Arguments
            if (methodMetadata.GenericArguments is null)
            {
                GenericArguments = null;
            }
            else
            {
                List<ITypeMetadata> genericArguments = new List<ITypeMetadata>();
                foreach (ITypeMetadata genericArgument in methodMetadata.GenericArguments)
                {
                    if (MappingDictionary.AlreadyMapped.TryGetValue(genericArgument.SavedHash, out IMetadata mappedArgument))
                    {
                        genericArguments.Add(mappedArgument as ITypeMetadata);
                    }
                    else
                    {
                        ITypeMetadata newType = new SerializationTypeMetadata(genericArgument);
                        genericArguments.Add(newType);
                        MappingDictionary.AlreadyMapped.Add(newType.SavedHash, newType);
                    }
                }
                GenericArguments = genericArguments;
            }

            // Return type
            if (MappingDictionary.AlreadyMapped.TryGetValue(methodMetadata.ReturnType.SavedHash, out IMetadata item))
            {
                ReturnType = item as ITypeMetadata;
            }
            else
            {
                ITypeMetadata newType = new SerializationTypeMetadata(methodMetadata.ReturnType);
                ReturnType = newType;
                MappingDictionary.AlreadyMapped.Add(newType.SavedHash, newType);
            }

            // Parameters
            if (methodMetadata.Parameters is null)
            {
                Parameters = Enumerable.Empty<IParameterMetadata>();
            }
            else
            {
                List<IParameterMetadata> parameters = new List<IParameterMetadata>();
                foreach (IParameterMetadata parameter in methodMetadata.Parameters)
                {
                    if (MappingDictionary.AlreadyMapped.TryGetValue(parameter.SavedHash, out item))
                    {
                        parameters.Add(item as IParameterMetadata);
                    }
                    else
                    {
                        IParameterMetadata newParameter = new SerializationParameterMetadata(parameter);
                        parameters.Add(newParameter);
                        MappingDictionary.AlreadyMapped.Add(newParameter.SavedHash, newParameter);
                    }
                }
                Parameters = parameters;
            }

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
