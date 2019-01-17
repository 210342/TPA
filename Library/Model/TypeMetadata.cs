using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Runtime.Serialization;
using ModelContract;

namespace Library.Model
{
    public class TypeMetadata : AbstractMapper, ITypeMetadata
    {
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

        #region properties

        public bool Mapped { get; }
        public string Name { get; }
        public string NamespaceName { get; }
        public ITypeMetadata BaseType { get; private set; }
        public IEnumerable<ITypeMetadata> GenericArguments { get; private set; }
        public Tuple<AccessLevelEnum, SealedEnum, AbstractEnum> Modifiers { get; }
        public TypeKindEnum TypeKind { get; }
        public IEnumerable<IAttributeMetadata> Attributes { get; }
        public IEnumerable<ITypeMetadata> ImplementedInterfaces { get; private set; }
        public IEnumerable<ITypeMetadata> NestedTypes { get; private set; }
        public IEnumerable<IPropertyMetadata> Properties { get; }
        public ITypeMetadata DeclaringType { get; private set; }
        public IEnumerable<IMethodMetadata> Methods { get; }
        public IEnumerable<IMethodMetadata> Constructors { get; }
        public IEnumerable<IMetadata> Children { get; private set; }
        public int SavedHash { get; }

        #endregion

        #region constructors

        internal TypeMetadata(Type type)
        {
            if (type == null)
                throw new ArgumentNullException("Type can't be null.");
            NamespaceName = type.Namespace;
            DeclaringType = EmitDeclaringType(type.DeclaringType);
            Constructors = MethodMetadata.EmitMethods(type.GetConstructors());
            Methods = MethodMetadata.EmitMethods(type.GetMethods());
            NestedTypes = EmitNestedTypes(type.GetNestedTypes());
            ImplementedInterfaces = EmitImplements(type.GetInterfaces());
            GenericArguments = !type.IsGenericTypeDefinition ? null : EmitGenericArguments(type.GetGenericArguments());
            Modifiers = EmitModifiers(type);
            BaseType = EmitExtends(type.BaseType);
            Properties = EmitPropertiesAndFields(type);
            TypeKind = GetTypeKind(type);

            Attributes = new List<AttributeMetadata>();
            type.CustomAttributes.ToList().ForEach(n =>
                ((List<AttributeMetadata>) Attributes).Add(new AttributeMetadata(n)));
            Name = type.Name;
            //FILL CHILDREN
            FillChildren(new StreamingContext());

            SavedHash = type.GetHashCode();
            Mapped = true;
        }

        internal TypeMetadata()
        {
            Mapped = false;
        }

        public TypeMetadata(ITypeMetadata typeMetadata)
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
                BaseType = new TypeMetadata(typeMetadata.BaseType.SavedHash, typeMetadata.BaseType.Name);
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
                        genericArguments.Add(new TypeMetadata(genericArgument.SavedHash, genericArgument.Name));
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
                        IAttributeMetadata newAttribute = new AttributeMetadata(attribute);
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
                        interfaces.Add(new TypeMetadata(implementedInterface.SavedHash, implementedInterface.Name));
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
                        nestedTypes.Add(new TypeMetadata(nestedType.SavedHash, nestedType.Name));
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
                        IPropertyMetadata newProperty = new PropertyMetadata(property);
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
                DeclaringType = new TypeMetadata(typeMetadata.DeclaringType.SavedHash, typeMetadata.DeclaringType.Name);
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
                        IMethodMetadata newMethod = new MethodMetadata(method);
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
                        IMethodMetadata newMethod = new MethodMetadata(constructor);
                        constructors.Add(newMethod);
                        AlreadyMapped.Add(newMethod.SavedHash, newMethod);
                    }

                Constructors = constructors;
            }

            FillChildren(new StreamingContext());
            Mapped = true;
        }

        private TypeMetadata(string typeName, string namespaceName, int hash)
        {
            if (typeName == null || namespaceName == null)
                throw new ArgumentNullException("Type can't be null.");
            Name = typeName;
            NamespaceName = namespaceName;
            SavedHash = hash;
            Mapped = false;
        }

        private TypeMetadata(string typeName, string namespaceName, IEnumerable<TypeMetadata> genericArguments,
            int hash)
            : this(typeName, namespaceName, hash)
        {
            GenericArguments = genericArguments;
        }

        internal TypeMetadata(int hashCode, string name)
        {
            SavedHash = hashCode;
            Name = name;
            Mapped = false;
        }

        #endregion

        #region API

        internal static TypeMetadata EmitReference(Type type)
        {
            if (!type.IsGenericType)
                return new TypeMetadata(type.Name, type.GetNamespace(),
                    type.GetHashCode());
            return new TypeMetadata(type.Name, type.GetNamespace(), EmitGenericArguments(type.GetGenericArguments()),
                type.GetHashCode());
        }

        internal static IEnumerable<TypeMetadata> EmitGenericArguments(IEnumerable<Type> arguments)
        {
            return from Type _argument in arguments select EmitReference(_argument);
        }

        #endregion

        #region methods

        private IEnumerable<IPropertyMetadata> EmitPropertiesAndFields(Type type)
        {
            IEnumerable<IPropertyMetadata> fields = from FieldInfo field
                    in type.GetFields(
                        BindingFlags.NonPublic | BindingFlags.Public | BindingFlags.Instance)
                select new PropertyMetadata(field.Name, EmitReference(field.FieldType));
            return fields.Concat(PropertyMetadata.EmitProperties(type.GetProperties()));
        }

        private TypeMetadata EmitDeclaringType(Type declaringType)
        {
            if (declaringType == null)
                return null;
            return EmitReference(declaringType);
        }

        private IEnumerable<TypeMetadata> EmitNestedTypes(IEnumerable<Type> nestedTypes)
        {
            return from _type in nestedTypes
                where _type.GetVisible()
                select new TypeMetadata(_type);
        }

        private IEnumerable<TypeMetadata> EmitImplements(IEnumerable<Type> interfaces)
        {
            return from currentInterface in interfaces
                select EmitReference(currentInterface);
        }

        private static TypeKindEnum
            GetTypeKind(Type type) //#80 TPA: Reflection - Invalid return value of GetTypeKind() 
        {
            return type.IsEnum ? TypeKindEnum.EnumType :
                type.IsValueType ? TypeKindEnum.StructType :
                type.IsInterface ? TypeKindEnum.InterfaceType :
                TypeKindEnum.ClassType;
        }

        private static Tuple<AccessLevelEnum, SealedEnum, AbstractEnum> EmitModifiers(Type type)
        {
            //set defaults 
            AccessLevelEnum _access = AccessLevelEnum.IsPrivate;
            AbstractEnum _abstract = AbstractEnum.NotAbstract;
            SealedEnum _sealed = SealedEnum.NotSealed;
            // check if not default 
            if (type.IsPublic)
                _access = AccessLevelEnum.IsPublic;
            else if (type.IsNestedPublic)
                _access = AccessLevelEnum.IsPublic;
            else if (type.IsNestedFamily)
                _access = AccessLevelEnum.IsProtected;
            else if (type.IsNestedFamANDAssem)
                _access = AccessLevelEnum.IsProtectedInternal;
            if (type.IsSealed)
                _sealed = SealedEnum.Sealed;
            if (type.IsAbstract)
                _abstract = AbstractEnum.Abstract;
            return new Tuple<AccessLevelEnum, SealedEnum, AbstractEnum>(_access, _sealed, _abstract);
        }

        private static TypeMetadata EmitExtends(Type baseType)
        {
            if (baseType == null || baseType == typeof(object) || baseType == typeof(ValueType) ||
                baseType == typeof(Enum))
                return null;
            return EmitReference(baseType);
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

        #endregion

        #region object overrides

        public override int GetHashCode()
        {
            return SavedHash;
        }

        public override bool Equals(object obj)
        {
            if (GetType() != obj.GetType())
                return false;
            TypeMetadata tm = (TypeMetadata) obj;
            if (Name == tm.Name)
            {
                if (NamespaceName != tm.NamespaceName)
                    return false;
                return true;
            }

            return false;
        }

        public override string ToString()
        {
            return Name;
        }

        public string ModifiersString()
        {
            return Modifiers?.Item1 + Modifiers?.Item2.ToString() + Modifiers?.Item3 + Modifiers;
        }

        #endregion
    }
}