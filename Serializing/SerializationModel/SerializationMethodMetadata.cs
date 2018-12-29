using ModelContract;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace SerializationModel
{
    [DataContract(Name = "Method")]
    public class SerializationMethodMetadata : AbstractMapper, IMethodMetadata
    {
        [DataMember(Name = "GenericArguments")]
        public IEnumerable<ITypeMetadata> GenericArguments { get; private set; }
        [DataMember(Name = "ReturnType")]
        public ITypeMetadata ReturnType { get; private set; }
        [DataMember(Name = "IsExtension")]
        public bool IsExtension { get; private set; }
        [DataMember(Name = "Parameters")]
        public IEnumerable<IParameterMetadata> Parameters { get; private set; }
        [DataMember(Name = "Modifiers")]
        public Tuple<AccessLevelEnum, AbstractEnum, StaticEnum, VirtualEnum> Modifiers { get; private set; }
        [DataMember(Name = "Name")]
        public string Name { get; private set; }
        [DataMember(Name = "Hash")]
        public int SavedHash { get; private set; }
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
                    if (AlreadyMapped.TryGetValue(genericArgument.SavedHash, out IMetadata mappedArgument))
                    {
                        genericArguments.Add(mappedArgument as ITypeMetadata);
                    }
                    else
                    {
                        ITypeMetadata newType = new SerializationTypeMetadata(genericArgument);
                        genericArguments.Add(newType);
                        AlreadyMapped.Add(newType.SavedHash, newType);
                    }
                }
                GenericArguments = genericArguments;
            }

            // Return type
            if (AlreadyMapped.TryGetValue(methodMetadata.ReturnType.SavedHash, out IMetadata item))
            {
                ReturnType = item as ITypeMetadata;
            }
            else
            {
                ITypeMetadata newType = new SerializationTypeMetadata(methodMetadata.ReturnType);
                ReturnType = newType;
                AlreadyMapped.Add(newType.SavedHash, newType);
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
                    if (AlreadyMapped.TryGetValue(parameter.SavedHash, out item))
                    {
                        parameters.Add(item as IParameterMetadata);
                    }
                    else
                    {
                        IParameterMetadata newParameter = new SerializationParameterMetadata(parameter);
                        parameters.Add(newParameter);
                        AlreadyMapped.Add(newParameter.SavedHash, newParameter);
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
