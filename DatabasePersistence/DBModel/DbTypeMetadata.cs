using ModelContract;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Runtime.Serialization;

namespace DatabasePersistence.DBModel
{
    public class DbTypeMetadata : AbstractMapper, ITypeMetadata
    {
        public string NamespaceName { get; private set; }
        public ITypeMetadata BaseType { get; private set; }
        public IEnumerable<ITypeMetadata> GenericArguments { get; private set; }
        public Tuple<AccessLevelEnum, SealedEnum, AbstractEnum> Modifiers { get; private set; }
        public TypeKindEnum TypeKind { get; private set; }
        public IEnumerable<IAttributeMetadata> Attributes { get; private set; }
        public IEnumerable<ITypeMetadata> ImplementedInterfaces { get; private set; }
        public IEnumerable<ITypeMetadata> NestedTypes { get; private set; }
        public IEnumerable<IPropertyMetadata> Properties { get; private set; }
        public ITypeMetadata DeclaringType { get; private set; }
        public IEnumerable<IMethodMetadata> Methods { get; private set; }
        public IEnumerable<IMethodMetadata> Constructors { get; private set; }
        public string Name { get; set; }
        public int SavedHash { get; protected set; }
        [NotMapped]
        public IEnumerable<IMetadata> Children { get; set; }

        public DbTypeMetadata(ITypeMetadata typeMetadata)
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
                ITypeMetadata newType = new DbTypeMetadata(typeMetadata.BaseType);
                BaseType = newType;
                AlreadyMapped.Add(newType.SavedHash, newType);
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
                    if (AlreadyMapped.TryGetValue(genericArgument.SavedHash, out IMetadata item))
                    {
                        genericArguments.Add(item as ITypeMetadata);
                    }
                    else
                    {
                        ITypeMetadata newType = new DbTypeMetadata(genericArgument);
                        genericArguments.Add(newType);
                        AlreadyMapped.Add(newType.SavedHash, newType);
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
                    if (AlreadyMapped.TryGetValue(attribute.SavedHash, out IMetadata item))
                    {
                        attributes.Add(item as IAttributeMetadata);
                    }
                    else
                    {
                        IAttributeMetadata newAttribute = new DbAttributeMetadata(attribute);
                        attributes.Add(newAttribute);
                        AlreadyMapped.Add(newAttribute.SavedHash, newAttribute);
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
                    if (AlreadyMapped.TryGetValue(implementedInterface.SavedHash, out IMetadata item))
                    {
                        interfaces.Add(item as ITypeMetadata);
                    }
                    else
                    {
                        ITypeMetadata newInterface = new DbTypeMetadata(implementedInterface);
                        interfaces.Add(newInterface);
                        AlreadyMapped.Add(newInterface.SavedHash, newInterface);
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
                    if (AlreadyMapped.TryGetValue(nestedType.SavedHash, out IMetadata item))
                    {
                        nestedTypes.Add(item as ITypeMetadata);
                    }
                    else
                    {
                        ITypeMetadata newType = new DbTypeMetadata(nestedType);
                        nestedTypes.Add(newType);
                        AlreadyMapped.Add(newType.SavedHash, newType);
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
                    if (AlreadyMapped.TryGetValue(property.SavedHash, out IMetadata item))
                    {
                        properties.Add(item as IPropertyMetadata);
                    }
                    else
                    {
                        IPropertyMetadata newProperty = new DbPropertyMetadata(property);
                        properties.Add(newProperty);
                        AlreadyMapped.Add(newProperty.SavedHash, newProperty);
                    }
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
                ITypeMetadata newType = new DbTypeMetadata(typeMetadata.DeclaringType);
                DeclaringType = newType;
                AlreadyMapped.Add(newType.SavedHash, newType);
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
                    if (AlreadyMapped.TryGetValue(method.SavedHash, out IMetadata item))
                    {
                        methods.Add(item as IMethodMetadata);
                    }
                    else
                    {
                        IMethodMetadata newMethod = new DbMethodMetadata(method);
                        methods.Add(newMethod);
                        AlreadyMapped.Add(newMethod.SavedHash, newMethod);
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
                    if (AlreadyMapped.TryGetValue(constructor.SavedHash, out IMetadata item))
                    {
                        constructors.Add(item as IMethodMetadata);
                    }
                    else
                    {
                        IMethodMetadata newMethod = new DbMethodMetadata(constructor);
                        constructors.Add(newMethod);
                        AlreadyMapped.Add(newMethod.SavedHash, newMethod);
                    }
                }
                Constructors = constructors;
            }

            FillChildren();
        }

        public DbTypeMetadata()
        {
        }

        private void FillChildren()
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
