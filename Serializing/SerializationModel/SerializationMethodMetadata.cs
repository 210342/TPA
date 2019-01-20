using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using ModelContract;

namespace SerializationModel
{
    [DataContract(Name = "Method")]
    public class SerializationMethodMetadata : AbstractMapper, IMethodMetadata
    {
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
                    if (AlreadyMapped.TryGetValue(genericArgument.SavedHash, out IMetadata mappedArgument))
                    {
                        genericArguments.Add(mappedArgument as ITypeMetadata);
                    }
                    else
                    {
                        // use temporary constructor to save its hash, retrieve actual object afterr all mapping has been done
                        genericArguments.Add(new SerializationTypeMetadata(
                            new SerializationTypeMetadata(genericArgument.SavedHash, genericArgument.Name)));
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
                // use temporary constructor to save its hash, retrieve actual object afterr all mapping has been done
                ReturnType = new SerializationTypeMetadata(
                    new SerializationTypeMetadata(methodMetadata.ReturnType.SavedHash, methodMetadata.ReturnType.Name));
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

                Parameters = parameters;
            }

            FillChildren(new StreamingContext());
        }

        [DataMember(Name = "GenericArguments")]
        public IEnumerable<ITypeMetadata> GenericArguments { get; private set; }

        [DataMember(Name = "ReturnType")] public ITypeMetadata ReturnType { get; private set; }

        [DataMember(Name = "IsExtension")] public bool IsExtension { get; private set; }

        [DataMember(Name = "Parameters")] public IEnumerable<IParameterMetadata> Parameters { get; private set; }

        [DataMember(Name = "Modifiers")]
        public Tuple<AccessLevelEnum, AbstractEnum, StaticEnum, VirtualEnum> Modifiers { get; private set; }

        [DataMember(Name = "Name")] public string Name { get; private set; }

        [DataMember(Name = "Hash")] public int SavedHash { get; private set; }

        public IEnumerable<IMetadata> Children { get; private set; }

        [OnDeserialized]
        private void FillChildren(StreamingContext context)
        {
            List<IMetadata> elems = new List<IMetadata> {ReturnType};
            elems.AddRange(Parameters);
            Children = elems;
        }

        public void MapTypes()
        {
            if (ReturnType != null
                && AlreadyMapped.TryGetValue(ReturnType.SavedHash, out IMetadata item))
            {
                ReturnType = item as ITypeMetadata;
            }
            if(GenericArguments != null)
            {
                ICollection<ITypeMetadata> actualGenericArguments = new List<ITypeMetadata>();
                foreach(ITypeMetadata type in GenericArguments)
                {
                    if (AlreadyMapped.TryGetValue(type.SavedHash, out item))
                    {
                        actualGenericArguments.Add(item as ITypeMetadata);
                    }
                    else
                    {
                        actualGenericArguments.Add(type);
                    }
                }
                GenericArguments = actualGenericArguments;
            }
            foreach (IParameterMetadata parameter in Parameters)
            {
                parameter.MapTypes();
            }
        }
    }
}