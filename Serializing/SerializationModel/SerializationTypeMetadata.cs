using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using ModelContract;

namespace SerializationModel
{
    [DataContract(Name = "Type")]
    public class SerializationTypeMetadata : AbstractMapper, ITypeMetadata
    {
        internal SerializationTypeMetadata(int hashCode, string name)
        {
            SavedHash = hashCode;
            Name = name;
            Mapped = false;
        }

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
            else if (AlreadyMapped.TryGetValue(typeMetadata.BaseType.SavedHash, out IMetadata item))
            {
                BaseType = item as ITypeMetadata;
            }
            else
            {
                BaseType = new SerializationTypeMetadata(
                    new SerializationTypeMetadata(typeMetadata.BaseType.SavedHash, typeMetadata.BaseType.Name));
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
                    if (AlreadyMapped.TryGetValue(genericArgument.SavedHash, out IMetadata item))
                    {
                        genericArguments.Add(item as ITypeMetadata);
                    }
                    else
                    {
                        genericArguments.Add(new SerializationTypeMetadata(
                            new SerializationTypeMetadata(genericArgument.SavedHash, genericArgument.Name)));
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
                    if (AlreadyMapped.TryGetValue(attribute.SavedHash, out IMetadata item))
                    {
                        attributes.Add(item as IAttributeMetadata);
                    }
                    else
                    {
                        IAttributeMetadata newAttribute = new SerializationAttributeMetadata(attribute);
                        attributes.Add(newAttribute);
                        AlreadyMapped.Add(newAttribute.SavedHash, newAttribute);
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
                    if (AlreadyMapped.TryGetValue(implementedInterface.SavedHash, out IMetadata item))
                    {
                        interfaces.Add(item as ITypeMetadata);
                    }
                    else
                    {
                        interfaces.Add(new SerializationTypeMetadata(
                            new SerializationTypeMetadata(implementedInterface.SavedHash, implementedInterface.Name)));
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
                    if (AlreadyMapped.TryGetValue(nestedType.SavedHash, out IMetadata item))
                    {
                        nestedTypes.Add(item as ITypeMetadata);
                    }
                    else
                    {
                        nestedTypes.Add(new SerializationTypeMetadata(
                            new SerializationTypeMetadata(nestedType.SavedHash, nestedType.Name)));
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
                    if (AlreadyMapped.TryGetValue(property.SavedHash, out IMetadata item))
                    {
                        properties.Add(item as IPropertyMetadata);
                    }
                    else
                    {
                        IPropertyMetadata newProperty = new SerializationPropertyMetadata(property);
                        properties.Add(newProperty);
                        AlreadyMapped.Add(newProperty.SavedHash, newProperty);
                    }

                Properties = properties;
            }

            //Declaring type
            if (typeMetadata.DeclaringType is null)
            {
                DeclaringType = null;
            }
            else if (AlreadyMapped.TryGetValue(typeMetadata.DeclaringType.SavedHash, out IMetadata item))
            {
                DeclaringType = item as ITypeMetadata;
            }
            else
            {
                DeclaringType = new SerializationTypeMetadata(
                    new SerializationTypeMetadata(typeMetadata.DeclaringType.SavedHash, typeMetadata.DeclaringType.Name));
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
                    if (AlreadyMapped.TryGetValue(method.SavedHash, out IMetadata item))
                    {
                        methods.Add(item as IMethodMetadata);
                    }
                    else
                    {
                        IMethodMetadata newMethod = new SerializationMethodMetadata(method);
                        methods.Add(newMethod);
                        AlreadyMapped.Add(newMethod.SavedHash, newMethod);
                    }

                Methods = methods;
            }

            // Constructors
            if (typeMetadata.Methods is null)
            {
                Constructors = Enumerable.Empty<IMethodMetadata>();
            }
            else
            {
                List<IMethodMetadata> constructors = new List<IMethodMetadata>();
                foreach (IMethodMetadata constructor in typeMetadata.Methods)
                    if (AlreadyMapped.TryGetValue(constructor.SavedHash, out IMetadata item))
                    {
                        constructors.Add(item as IMethodMetadata);
                    }
                    else
                    {
                        IMethodMetadata newMethod = new SerializationMethodMetadata(constructor);
                        constructors.Add(newMethod);
                        AlreadyMapped.Add(newMethod.SavedHash, newMethod);
                    }

                Constructors = constructors;
            }

            FillChildren(new StreamingContext());
            Mapped = true;
        }

        public bool Mapped { get; }
        [DataMember(Name = "NamespaceName")] public string NamespaceName { get; private set; }

        [DataMember(Name = "BaseType")] public ITypeMetadata BaseType { get; private set; }

        [DataMember(Name = "GenericArguments")]
        public IEnumerable<ITypeMetadata> GenericArguments { get; private set; }

        [DataMember(Name = "Modifiers")]
        public Tuple<AccessLevelEnum, SealedEnum, AbstractEnum> Modifiers { get; private set; }

        [DataMember(Name = "TypeKind")] public TypeKindEnum TypeKind { get; private set; }

        [DataMember(Name = "Attributes")] public IEnumerable<IAttributeMetadata> Attributes { get; private set; }

        [DataMember(Name = "Interfaces")] public IEnumerable<ITypeMetadata> ImplementedInterfaces { get; private set; }

        [DataMember(Name = "NestedTypes")] public IEnumerable<ITypeMetadata> NestedTypes { get; private set; }

        [DataMember(Name = "Properties")] public IEnumerable<IPropertyMetadata> Properties { get; private set; }

        [DataMember(Name = "DeclaringType")] public ITypeMetadata DeclaringType { get; private set; }

        [DataMember(Name = "Methods")] public IEnumerable<IMethodMetadata> Methods { get; private set; }

        [DataMember(Name = "Constructors")] public IEnumerable<IMethodMetadata> Constructors { get; private set; }

        [DataMember(Name = "Name")] public string Name { get; private set; }

        [DataMember(Name = "Hash")] public int SavedHash { get; private set; }

        public IEnumerable<IMetadata> Children { get; private set; }

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
            Children = elems;
        }

        public void MapTypes()
        {
            if (BaseType != null && !BaseType.Mapped 
                && AlreadyMapped.TryGetValue(BaseType.SavedHash, out IMetadata item))
            {
                BaseType = item as ITypeMetadata;
            }
            if (DeclaringType != null && !DeclaringType.Mapped
                && AlreadyMapped.TryGetValue(DeclaringType.SavedHash, out item))
            {
                DeclaringType = item as ITypeMetadata;
            }
            if (GenericArguments != null)
            {
                ICollection<ITypeMetadata> actualGenericArguments = new List<ITypeMetadata>();
                foreach (ITypeMetadata type in GenericArguments)
                {
                    if (!type.Mapped && AlreadyMapped.TryGetValue(type.SavedHash, out item))
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
            if (ImplementedInterfaces != null)
            {
                ICollection<ITypeMetadata> actualImplementedInterfaces = new List<ITypeMetadata>();
                foreach (ITypeMetadata type in ImplementedInterfaces)
                {
                    if (!type.Mapped && AlreadyMapped.TryGetValue(type.SavedHash, out item))
                    {
                        actualImplementedInterfaces.Add(item as ITypeMetadata);
                    }
                    else
                    {
                        actualImplementedInterfaces.Add(type);
                    }
                }
                ImplementedInterfaces = actualImplementedInterfaces;
            }
            if (NestedTypes != null)
            {
                ICollection<ITypeMetadata> actualNestedTypes = new List<ITypeMetadata>();
                foreach (ITypeMetadata type in NestedTypes)
                {
                    if (!type.Mapped && AlreadyMapped.TryGetValue(type.SavedHash, out item))
                    {
                        actualNestedTypes.Add(item as ITypeMetadata);
                    }
                    else
                    {
                        actualNestedTypes.Add(type);
                    }
                }
                NestedTypes = actualNestedTypes;
            }

            foreach (IMethodMetadata method in Methods)
            {
                method.MapTypes();
            }
            foreach (IMethodMetadata method in Constructors)
            {
                method.MapTypes();
            }
            foreach (IPropertyMetadata property in Properties)
            {
                property.MapTypes();
            }
        }
    }
}