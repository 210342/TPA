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
    [KnownType(typeof(SerializationAssemblyMetadata))]
    [KnownType(typeof(SerializationAttributeMetadata))]
    [KnownType(typeof(SerializationMethodMetadata))]
    [KnownType(typeof(SerializationNamespaceMetadata))]
    [KnownType(typeof(SerializationParameterMetadata))]
    [KnownType(typeof(SerializationPropertyMetadata))]
    [KnownType(typeof(SerializationTypeMetadata))]
    public class SerializationTypeMetadata : ITypeMetadata
    {
        [DataMember]
        public string NamespaceName { get; }
        [DataMember]
        public ITypeMetadata BaseType { get; }
        [DataMember]
        public IEnumerable<ITypeMetadata> GenericArguments { get; }
        [DataMember]
        public Tuple<AccessLevelEnum, SealedEnum, AbstractEnum> Modifiers { get; }
        [DataMember]
        public TypeKindEnum TypeKind { get; }
        [DataMember]
        public IEnumerable<IAttributeMetadata> Attributes { get; }
        [DataMember]
        public IEnumerable<ITypeMetadata> ImplementedInterfaces { get; }
        [DataMember]
        public IEnumerable<ITypeMetadata> NestedTypes { get; }
        [DataMember]
        public IEnumerable<IPropertyMetadata> Properties { get; }
        [DataMember]
        public ITypeMetadata DeclaringType { get; }
        [DataMember]
        public IEnumerable<IMethodMetadata> Methods { get; }
        [DataMember]
        public IEnumerable<IMethodMetadata> Constructors { get; }
        [DataMember]
        public string Name { get; }
        [DataMember]
        public int SavedHash { get; }
        public IEnumerable<IMetadata> Children { get; private set; }

        public SerializationTypeMetadata(ITypeMetadata typeMetadata)
        {
            Name = typeMetadata.Name;
            SavedHash = typeMetadata.SavedHash;
            NamespaceName = typeMetadata.NamespaceName;

            // Base type
            if (typeMetadata.BaseType is null)
            {
                BaseType = null;
            }
            else if (MappingDictionary.AlreadyMapped.TryGetValue(typeMetadata.BaseType.SavedHash, out IMetadata item))
            {
                BaseType = item as ITypeMetadata;
            }
            else
            {
                ITypeMetadata newType = new SerializationTypeMetadata(typeMetadata.BaseType);
                BaseType = newType;
                MappingDictionary.AlreadyMapped.Add(newType.SavedHash, newType);
            }

            // Generic Arguments
            if (typeMetadata.GenericArguments is null)
            {
                GenericArguments = null;
            }
            else
            {
                List<ITypeMetadata> genericArguments = new List<ITypeMetadata>();
                foreach (ITypeMetadata genericArgument in typeMetadata.GenericArguments)
                {
                    if (MappingDictionary.AlreadyMapped.TryGetValue(genericArgument.SavedHash, out IMetadata item))
                    {
                        genericArguments.Add(item as ITypeMetadata);
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

            // Modifiers
            Modifiers = typeMetadata.Modifiers;

            // Type kind
            TypeKind = typeMetadata.TypeKind;

            // Attributes
            if (typeMetadata.Attributes is null)
            {
                Attributes = Enumerable.Empty<IAttributeMetadata>();
            }
            else
            {
                List<IAttributeMetadata> attributes = new List<IAttributeMetadata>();
                foreach (IAttributeMetadata attribute in typeMetadata.Attributes)
                {
                    if (MappingDictionary.AlreadyMapped.TryGetValue(attribute.SavedHash, out IMetadata item))
                    {
                        attributes.Add(item as IAttributeMetadata);
                    }
                    else
                    {
                        IAttributeMetadata newAttribute = new SerializationAttributeMetadata(attribute);
                        attributes.Add(newAttribute);
                        MappingDictionary.AlreadyMapped.Add(newAttribute.SavedHash, newAttribute);
                    }
                }
                Attributes = attributes;
            }

            // Interfaces
            if (typeMetadata.ImplementedInterfaces is null)
            {
                ImplementedInterfaces = Enumerable.Empty<ITypeMetadata>();
            }
            else
            {
                List<ITypeMetadata> interfaces = new List<ITypeMetadata>();
                foreach (ITypeMetadata implementedInterface in typeMetadata.ImplementedInterfaces)
                {
                    if (MappingDictionary.AlreadyMapped.TryGetValue(implementedInterface.SavedHash, out IMetadata item))
                    {
                        interfaces.Add(item as ITypeMetadata);
                    }
                    else
                    {
                        ITypeMetadata newInterface = new SerializationTypeMetadata(implementedInterface);
                        interfaces.Add(newInterface);
                        MappingDictionary.AlreadyMapped.Add(newInterface.SavedHash, newInterface);
                    }
                }
                ImplementedInterfaces = interfaces;
            }

            // Nested Types
            if (typeMetadata.NestedTypes is null)
            {
                NestedTypes = null;
            }
            else
            {
                List<ITypeMetadata> nestedTypes = new List<ITypeMetadata>();
                foreach (ITypeMetadata nestedType in typeMetadata.NestedTypes)
                {
                    if (MappingDictionary.AlreadyMapped.TryGetValue(nestedType.SavedHash, out IMetadata item))
                    {
                        nestedTypes.Add(item as ITypeMetadata);
                    }
                    else
                    {
                        ITypeMetadata newType = new SerializationTypeMetadata(nestedType);
                        nestedTypes.Add(newType);
                        MappingDictionary.AlreadyMapped.Add(newType.SavedHash, newType);
                    }
                }
                NestedTypes = nestedTypes;
            }

            // Properties
            if (typeMetadata.Properties is null)
            {
                Properties = Enumerable.Empty<IPropertyMetadata>();
            }
            else
            {
                List<IPropertyMetadata> properties = new List<IPropertyMetadata>();
                foreach (IPropertyMetadata property in typeMetadata.Properties)
                {
                    if (MappingDictionary.AlreadyMapped.TryGetValue(property.SavedHash, out IMetadata item))
                    {
                        properties.Add(item as IPropertyMetadata);
                    }
                    else
                    {
                        IPropertyMetadata newProperty = new SerializationPropertyMetadata(property);
                        properties.Add(newProperty);
                        MappingDictionary.AlreadyMapped.Add(newProperty.SavedHash, newProperty);
                    }
                }
                Properties = properties;
            }

            //Declaring type
            if (typeMetadata.DeclaringType is null)
            {
                DeclaringType = null;
            }
            else if (MappingDictionary.AlreadyMapped.TryGetValue(typeMetadata.DeclaringType.SavedHash, out IMetadata item))
            {
                DeclaringType = item as ITypeMetadata;
            }
            else
            {
                ITypeMetadata newType = new SerializationTypeMetadata(typeMetadata.DeclaringType);
                DeclaringType = newType;
                MappingDictionary.AlreadyMapped.Add(newType.SavedHash, newType);
            }

            // Methods
            if (typeMetadata.Methods is null)
            {
                Methods = Enumerable.Empty<IMethodMetadata>();
            }
            else
            {
                List<IMethodMetadata> methods = new List<IMethodMetadata>();
                foreach (IMethodMetadata method in typeMetadata.Methods)
                {
                    if (MappingDictionary.AlreadyMapped.TryGetValue(method.SavedHash, out IMetadata item))
                    {
                        methods.Add(item as IMethodMetadata);
                    }
                    else
                    {
                        IMethodMetadata newMethod = new SerializationMethodMetadata(method);
                        methods.Add(newMethod);
                        MappingDictionary.AlreadyMapped.Add(newMethod.SavedHash, newMethod);
                    }
                }
                Methods = methods;
            }

            // Constructors
            // Methods
            if (typeMetadata.Methods is null)
            {
                Constructors = Enumerable.Empty<IMethodMetadata>();
            }
            else
            {
                List<IMethodMetadata> constructors = new List<IMethodMetadata>();
                foreach (IMethodMetadata constructor in typeMetadata.Methods)
                {
                    if (MappingDictionary.AlreadyMapped.TryGetValue(constructor.SavedHash, out IMetadata item))
                    {
                        constructors.Add(item as IMethodMetadata);
                    }
                    else
                    {
                        IMethodMetadata newMethod = new SerializationMethodMetadata(constructor);
                        constructors.Add(newMethod);
                        MappingDictionary.AlreadyMapped.Add(newMethod.SavedHash, newMethod);
                    }
                }
                Constructors = constructors;
            }

            FillChildren(new StreamingContext());
        }

        [OnDeserialized]
        private void FillChildren(StreamingContext context)
        {
            List<IAttributeMetadata> amList = new List<IAttributeMetadata>();
            if (Attributes != null)
                amList.AddRange(Attributes.Select(n => n));
            List<IMetadata> elems = new List<IMetadata>();
            elems.AddRange(amList);
            if (ImplementedInterfaces != null)
                elems.AddRange(ImplementedInterfaces);
            if (BaseType != null)
                elems.Add(BaseType);
            if (DeclaringType != null)
                elems.Add(DeclaringType);
            if (Properties != null)
                elems.AddRange(Properties);
            if (Constructors != null)
                elems.AddRange(Constructors);
            if (Methods != null)
                elems.AddRange(Methods);
            if (NestedTypes != null)
                elems.AddRange(NestedTypes);
            if (GenericArguments != null)
                elems.AddRange(GenericArguments);
            this.Children = elems;
        }
    }
}
