using ModelContract;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DatabasePersistence.DBModel
{
    public class DbTypeMetadata : AbstractMapper, ITypeMetadata
    {
        #region ITypeMetadata
        public string Name { get; set; }
        public int SavedHash { get; set; }
        public Tuple<AccessLevelEnum, SealedEnum, AbstractEnum> Modifiers { get; set; }
        public TypeKindEnum TypeKind { get; }
        [NotMapped]
        public string NamespaceName { get; set; }
        [NotMapped]
        public ITypeMetadata BaseType
        {
            get => EFBaseType;
            set => EFBaseType = value as DbTypeMetadata;
        }
        [NotMapped]
        public ITypeMetadata DeclaringType
        {
            get => EFDeclaringType;
            set => EFDeclaringType = value as DbTypeMetadata;
        }
        [NotMapped]
        public IEnumerable<IAttributeMetadata> Attributes
        {
            get => EFAttributes;
            set => EFAttributes = value?.Cast<DbAttributeMetadata>().ToList();
        }
        [NotMapped]
        public IEnumerable<ITypeMetadata> ImplementedInterfaces
        {
            get => EFImplementedInterfaces;
            set => EFImplementedInterfaces = value?.Cast<DbTypeMetadata>().ToList();
        }
        [NotMapped]
        public IEnumerable<ITypeMetadata> GenericArguments
        {
            get => EFGenericArguments;
            set => EFGenericArguments = value?.Cast<DbTypeMetadata>().ToList();
        }
        [NotMapped]
        public IEnumerable<ITypeMetadata> NestedTypes
        {
            get => EFNestedTypes;
            set => EFNestedTypes = value?.Cast<DbTypeMetadata>().ToList();
        }
        [NotMapped]
        public IEnumerable<IPropertyMetadata> Properties
        {
            get => EFProperties;
            set => EFProperties = value?.Cast<DbPropertyMetadata>().ToList();
        }
        [NotMapped]
        public IEnumerable<IMethodMetadata> Constructors
        {
            get => EFMethodsAndConstructors.Where(m => m.Name.Equals(".ctor"));
            set => EFMethodsAndConstructors = value?.Concat(Methods).Cast<DbMethodMetadata>().ToList();
        }
        [NotMapped]
        public IEnumerable<IMethodMetadata> Methods
        {
            get => EFMethodsAndConstructors.Where(m => !m.Name.Equals(".ctor"));
            set => EFMethodsAndConstructors = value?.Concat(Constructors).Cast<DbMethodMetadata>().ToList();
        }
        [NotMapped]
        public IEnumerable<IMetadata> Children => throw new NotImplementedException();

        #endregion

        #region EF

        public bool IsAbstract { get; set; }
        public bool IsSealed { get; set; }
        public AccessLevelEnum AccessLevel { get; set; }
        public DbTypeMetadata EFBaseType { get; set; }
        public DbTypeMetadata EFDeclaringType { get; set; }
        public ICollection<DbAttributeMetadata> EFAttributes { get; set; }
        public ICollection<DbTypeMetadata> EFImplementedInterfaces { get; set; }
        public ICollection<DbTypeMetadata> EFGenericArguments { get; set; }
        public ICollection<DbTypeMetadata> EFNestedTypes { get; set; }
        public ICollection<DbPropertyMetadata> EFProperties { get; set; }
        public ICollection<DbMethodMetadata> EFMethodsAndConstructors { get; set; }

        #endregion

        #region Constructors

        public DbTypeMetadata() { }

        public DbTypeMetadata(ITypeMetadata typeMetadata)
        {
            Name = typeMetadata.Name;
            SavedHash = typeMetadata.SavedHash;
            NamespaceName = typeMetadata.NamespaceName;

            // Base type
            if (typeMetadata.BaseType is null)
            {
                EFBaseType = null;
            }
            else if (AlreadyMappedTypes.TryGetValue(
                typeMetadata.BaseType.SavedHash, out DbTypeMetadata item))
            {
                EFBaseType = item;
            }
            else
            {
                EFBaseType = new DbTypeMetadata(typeMetadata.BaseType.SavedHash, typeMetadata.BaseType.Name);
            }

            //Declaring type
            if (typeMetadata.DeclaringType is null)
            {
                EFDeclaringType = null;
            }
            else if (AlreadyMappedTypes.TryGetValue(
                typeMetadata.DeclaringType.SavedHash, out DbTypeMetadata item))
            {
                EFDeclaringType = item;
            }
            else
            {
                EFDeclaringType = new DbTypeMetadata(typeMetadata.DeclaringType.SavedHash, typeMetadata.DeclaringType.Name);
            }

            // Generic Arguments
            if (typeMetadata.GenericArguments is null)
            {
                EFGenericArguments = null;
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

                EFGenericArguments = genericArguments;
            }

            // Modifiers
            Modifiers = typeMetadata.Modifiers;
            AccessLevel = Modifiers.Item1;
            IsSealed = Modifiers.Item2.Equals(SealedEnum.Sealed);
            IsAbstract = Modifiers.Item3.Equals(AbstractEnum.Abstract);

            // Type kind
            TypeKind = typeMetadata.TypeKind;

            // Attributes
            if (typeMetadata.Attributes is null)
            {
                EFAttributes = Enumerable.Empty<DbAttributeMetadata>().ToList();
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

                EFAttributes = attributes;
            }

            // Interfaces
            if (typeMetadata.ImplementedInterfaces is null)
            {
                EFImplementedInterfaces = Enumerable.Empty<DbTypeMetadata>().ToList();
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

                EFImplementedInterfaces = interfaces;
            }

            // Nested Types
            if (typeMetadata.NestedTypes is null)
            {
                EFNestedTypes = null;
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

                EFNestedTypes = nestedTypes;
            }

            // Properties
            if (typeMetadata.Properties is null)
            {
                EFProperties = Enumerable.Empty<DbPropertyMetadata>().ToList();
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

                EFProperties = properties;
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
            EFMethodsAndConstructors = methods;
        }

        public DbTypeMetadata(int hash, string name)
        {
            Name = name;
            SavedHash = hash;
        }

        #endregion

        public void MapTypes()
        {
            if (EFBaseType != null && AlreadyMappedTypes.TryGetValue(
                EFBaseType.SavedHash, out DbTypeMetadata item))
            {
                EFBaseType = item;
            }
            if (EFDeclaringType != null 
                && AlreadyMappedTypes.TryGetValue(EFDeclaringType.SavedHash, out item))
            {
                EFDeclaringType = item;
            }
            if (EFGenericArguments != null)
            {
                ICollection<DbTypeMetadata> actualGenericArguments = new List<DbTypeMetadata>();
                foreach (DbTypeMetadata type in EFGenericArguments)
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
                EFGenericArguments = actualGenericArguments;
            }
            if (EFImplementedInterfaces != null)
            {
                ICollection<DbTypeMetadata> actualImplementedInterfaces = new List<DbTypeMetadata>();
                foreach (DbTypeMetadata type in EFImplementedInterfaces)
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
                EFImplementedInterfaces = actualImplementedInterfaces;
            }

            if (EFNestedTypes != null)
            {
                ICollection<DbTypeMetadata> actualNestedTypes = new List<DbTypeMetadata>();
                foreach (DbTypeMetadata type in EFNestedTypes)
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
                EFNestedTypes = actualNestedTypes;
            }

            foreach (DbMethodMetadata method in EFMethodsAndConstructors)
            {
                method.MapTypes();
            }
            foreach (DbPropertyMetadata property in EFProperties)
            {
                property.MapTypes();
            }
        }
    }
}
