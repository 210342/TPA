using ModelContract;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dbp.Model
{
    public class DbTypeMetadata : AbstractMapper
    {
        public string Name { get; set; }
        public int SavedHash { get; set; }
        [NotMapped]
        public string NamespaceName { get; set; }
        public DbTypeMetadata BaseType { get; set; }
        public DbTypeMetadata DeclaringType { get; set; }
        public ICollection<DbAttributeMetadata> Attributes { get; set; }
        public ICollection<DbTypeMetadata> ImplementedInterfaces { get; set; }
        public ICollection<DbTypeMetadata> GenericArguments { get; set; }
        public ICollection<DbTypeMetadata> NestedTypes { get; set; }
        public ICollection<DbPropertyMetadata> Properties { get; set; }
        public ICollection<DbMethodMetadata> MethodsAndConstructors { get; set; }

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
            else if (AlreadyMappedTypes.TryGetValue(
                typeMetadata.BaseType.SavedHash, out DbTypeMetadata item))
            {
                BaseType = item;
            }
            else
            {
                BaseType = new DbTypeMetadata(typeMetadata.BaseType.SavedHash, typeMetadata.BaseType.Name);
            }

            //Declaring type
            if (typeMetadata.DeclaringType is null)
            {
                DeclaringType = null;
            }
            else if (AlreadyMappedTypes.TryGetValue(
                typeMetadata.DeclaringType.SavedHash, out DbTypeMetadata item))
            {
                DeclaringType = item;
            }
            else
            {
                DeclaringType = new DbTypeMetadata(typeMetadata.DeclaringType.SavedHash, typeMetadata.DeclaringType.Name);
            }

            // Generic Arguments
            if (typeMetadata.GenericArguments is null)
            {
                GenericArguments = null;
            }
            else
            {
                List<DbTypeMetadata> genericArguments = new List<DbTypeMetadata>();
                foreach (ITypeMetadata genericArgument in typeMetadata.GenericArguments)
                    if (AlreadyMappedTypes.TryGetValue(genericArgument.SavedHash, out DbTypeMetadata item))
                    {
                        genericArguments.Add(item);
                    }
                    else
                    {
                        genericArguments.Add(new DbTypeMetadata(genericArgument.SavedHash, genericArgument.Name));
                    }

                GenericArguments = genericArguments;
            }

            //// Modifiers
            //Modifiers = typeMetadata.Modifiers;

            //// Type kind
            //TypeKind = typeMetadata.TypeKind;

            // Attributes
            if (typeMetadata.Attributes is null)
            {
                Attributes = Enumerable.Empty<DbAttributeMetadata>().ToList();
            }
            else
            {
                List<DbAttributeMetadata> attributes = new List<DbAttributeMetadata>();
                foreach (IAttributeMetadata attribute in typeMetadata.Attributes)
                    if (AlreadyMappedAttributes.TryGetValue(attribute.SavedHash, out DbAttributeMetadata item))
                    {
                        attributes.Add(item);
                    }
                    else
                    {
                        DbAttributeMetadata newAttribute = new DbAttributeMetadata(attribute);
                        attributes.Add(newAttribute);
                        AlreadyMappedAttributes.Add(newAttribute.SavedHash, newAttribute);
                    }

                Attributes = attributes;
            }

            // Interfaces
            if (typeMetadata.ImplementedInterfaces is null)
            {
                ImplementedInterfaces = Enumerable.Empty<DbTypeMetadata>().ToList();
            }
            else
            {
                List<DbTypeMetadata> interfaces = new List<DbTypeMetadata>();
                foreach (ITypeMetadata implementedInterface in typeMetadata.ImplementedInterfaces)
                    if (AlreadyMappedTypes.TryGetValue(implementedInterface.SavedHash, out DbTypeMetadata item))
                    {
                        interfaces.Add(item);
                    }
                    else
                    {
                        interfaces.Add(new DbTypeMetadata(implementedInterface.SavedHash, implementedInterface.Name));
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
                List<DbTypeMetadata> nestedTypes = new List<DbTypeMetadata>();
                foreach (ITypeMetadata nestedType in typeMetadata.NestedTypes)
                    if (AlreadyMappedTypes.TryGetValue(nestedType.SavedHash, out DbTypeMetadata item))
                    {
                        nestedTypes.Add(item);
                    }
                    else
                    {
                        nestedTypes.Add(new DbTypeMetadata(nestedType.SavedHash, nestedType.Name));
                    }

                NestedTypes = nestedTypes;
            }

            // Properties
            if (typeMetadata.Properties is null)
            {
                Properties = Enumerable.Empty<DbPropertyMetadata>().ToList();
            }
            else
            {
                List<DbPropertyMetadata> properties = new List<DbPropertyMetadata>();
                foreach (IPropertyMetadata property in typeMetadata.Properties)
                    if (AlreadyMappedProperties.TryGetValue(
                        property.SavedHash, out DbPropertyMetadata item))
                    {
                        properties.Add(item);
                    }
                    else
                    {
                        DbPropertyMetadata newProperty = new DbPropertyMetadata(property);
                        properties.Add(newProperty);
                        AlreadyMappedProperties.Add(newProperty.SavedHash, newProperty);
                    }

                Properties = properties;
            }


            List<DbMethodMetadata> methods = new List<DbMethodMetadata>();
            // Methods
            if (typeMetadata.Methods != null)
            {
                foreach (IMethodMetadata method in typeMetadata.Methods)
                    if (AlreadyMappedMethods.TryGetValue(method.SavedHash, out DbMethodMetadata item))
                    {
                        methods.Add(item);
                    }
                    else
                    {
                        DbMethodMetadata newMethod = new DbMethodMetadata(method);
                        methods.Add(newMethod);
                        AlreadyMappedMethods.Add(newMethod.SavedHash, newMethod);
                    }

            }
            
            if(typeMetadata.Constructors != null)
            {
                foreach (IMethodMetadata constructor in typeMetadata.Constructors)
                    if (AlreadyMappedMethods.TryGetValue(
                        constructor.SavedHash, out DbMethodMetadata item))
                    {
                        methods.Add(item);
                    }
                    else
                    {
                        DbMethodMetadata newMethod = new DbMethodMetadata(constructor);
                        methods.Add(newMethod);
                        AlreadyMappedMethods.Add(newMethod.SavedHash, newMethod);
                    }
            }
            MethodsAndConstructors = methods;
            //FillChildren();
        }

        public DbTypeMetadata(int hash, string name)
        {
            Name = name;
            SavedHash = hash;
        }

        public void MapTypes()
        {
            DbTypeMetadata item;
            if (BaseType != null && AlreadyMappedTypes.TryGetValue(
                BaseType.SavedHash, out item))
            {
                BaseType = item;
            }
            if (DeclaringType != null 
                && AlreadyMappedTypes.TryGetValue(DeclaringType.SavedHash, out item))
            {
                DeclaringType = item;
            }
            if (GenericArguments != null)
            {
                ICollection<DbTypeMetadata> actualGenericArguments = new List<DbTypeMetadata>();
                foreach (DbTypeMetadata type in GenericArguments)
                {
                    if (AlreadyMappedTypes.TryGetValue(type.SavedHash, out item))
                    {
                        actualGenericArguments.Add(item);
                    }
                    else
                    {
                        actualGenericArguments.Add(type);
                        AlreadyMappedTypes.Add(type.SavedHash, type);
                    }
                }
                GenericArguments = actualGenericArguments;
            }
            if (ImplementedInterfaces != null)
            {
                ICollection<DbTypeMetadata> actualImplementedInterfaces = new List<DbTypeMetadata>();
                foreach (DbTypeMetadata type in ImplementedInterfaces)
                {
                    if (AlreadyMappedTypes.TryGetValue(type.SavedHash, out item))
                    {
                        actualImplementedInterfaces.Add(item);
                    }
                    else
                    {
                        actualImplementedInterfaces.Add(type);
                        AlreadyMappedTypes.Add(type.SavedHash, type);
                    }
                }
                ImplementedInterfaces = actualImplementedInterfaces;
            }

            if (NestedTypes != null)
            {
                ICollection<DbTypeMetadata> actualNestedTypes = new List<DbTypeMetadata>();
                foreach (DbTypeMetadata type in NestedTypes)
                {
                    if (AlreadyMappedTypes.TryGetValue(type.SavedHash, out item))
                    {
                        actualNestedTypes.Add(item);
                    }
                    else
                    {
                        actualNestedTypes.Add(type);
                        AlreadyMappedTypes.Add(type.SavedHash, type);
                    }
                }
                NestedTypes = actualNestedTypes;
            }

            foreach (DbMethodMetadata method in MethodsAndConstructors)
            {
                method.MapTypes();
            }
            foreach (DbPropertyMetadata property in Properties)
            {
                property.MapTypes();
            }
        }
    }
}
