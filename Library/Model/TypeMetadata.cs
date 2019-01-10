﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using ModelContract;

namespace Library.Model
{
    public class TypeMetadata : AbstractMapper, ITypeMetadata
    {
        private void FillChildren(StreamingContext context)
        {
            var amList = new List<IAttributeMetadata>();
            if (Attributes != null)
                amList.AddRange(Attributes.Select(n => n));
            var elems = new List<IMetadata>();
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

        public string Name { get; }
        public string NamespaceName { get; }
        public ITypeMetadata BaseType { get; }
        public IEnumerable<ITypeMetadata> GenericArguments { get; }
        public Tuple<AccessLevelEnum, SealedEnum, AbstractEnum> Modifiers { get; }
        public TypeKindEnum TypeKind { get; }
        public IEnumerable<IAttributeMetadata> Attributes { get; }
        public IEnumerable<ITypeMetadata> ImplementedInterfaces { get; }
        public IEnumerable<ITypeMetadata> NestedTypes { get; }
        public IEnumerable<IPropertyMetadata> Properties { get; }
        public ITypeMetadata DeclaringType { get; }
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
            Properties = PropertyMetadata.EmitProperties(type.GetProperties());
            TypeKind = GetTypeKind(type);

            Attributes = new List<AttributeMetadata>();
            type.GetCustomAttributes(false).Cast<Attribute>().ToList().ForEach(n =>
                ((List<AttributeMetadata>) Attributes).Add(new AttributeMetadata(n)));
            Name = type.Name;
            //FILL CHILDREN
            FillChildren(new StreamingContext());

            SavedHash = type.GetHashCode();
        }

        internal TypeMetadata()
        {
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
            else if (AlreadyMapped.TryGetValue(typeMetadata.BaseType.SavedHash, out var item))
            {
                BaseType = item as ITypeMetadata;
            }
            else
            {
                ITypeMetadata newType = new TypeMetadata(typeMetadata.BaseType);
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
                var genericArguments = new List<ITypeMetadata>();
                foreach (var genericArgument in typeMetadata.GenericArguments)
                    if (AlreadyMapped.TryGetValue(genericArgument.SavedHash, out var item))
                    {
                        genericArguments.Add(item as ITypeMetadata);
                    }
                    else
                    {
                        ITypeMetadata newType = new TypeMetadata(genericArgument);
                        genericArguments.Add(newType);
                        AlreadyMapped.Add(newType.SavedHash, newType);
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
                var attributes = new List<IAttributeMetadata>();
                foreach (var attribute in typeMetadata.Attributes)
                    if (AlreadyMapped.TryGetValue(attribute.SavedHash, out var item))
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
                var interfaces = new List<ITypeMetadata>();
                foreach (var implementedInterface in typeMetadata.ImplementedInterfaces)
                    if (AlreadyMapped.TryGetValue(implementedInterface.SavedHash, out var item))
                    {
                        interfaces.Add(item as ITypeMetadata);
                    }
                    else
                    {
                        ITypeMetadata newInterface = new TypeMetadata(implementedInterface);
                        interfaces.Add(newInterface);
                        AlreadyMapped.Add(newInterface.SavedHash, newInterface);
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
                var nestedTypes = new List<ITypeMetadata>();
                foreach (var nestedType in typeMetadata.NestedTypes)
                    if (AlreadyMapped.TryGetValue(nestedType.SavedHash, out var item))
                    {
                        nestedTypes.Add(item as ITypeMetadata);
                    }
                    else
                    {
                        ITypeMetadata newType = new TypeMetadata(nestedType);
                        nestedTypes.Add(newType);
                        AlreadyMapped.Add(newType.SavedHash, newType);
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
                var properties = new List<IPropertyMetadata>();
                foreach (var property in typeMetadata.Properties)
                    if (AlreadyMapped.TryGetValue(property.SavedHash, out var item))
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
            else if (AlreadyMapped.TryGetValue(typeMetadata.DeclaringType.SavedHash, out var item))
            {
                DeclaringType = item as ITypeMetadata;
            }
            else
            {
                ITypeMetadata newType = new TypeMetadata(typeMetadata.DeclaringType);
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
                var methods = new List<IMethodMetadata>();
                foreach (var method in typeMetadata.Methods)
                    if (AlreadyMapped.TryGetValue(method.SavedHash, out var item))
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
            // Methods
            if (typeMetadata.Methods is null)
            {
                Constructors = Enumerable.Empty<IMethodMetadata>();
            }
            else
            {
                var constructors = new List<IMethodMetadata>();
                foreach (var constructor in typeMetadata.Methods)
                    if (AlreadyMapped.TryGetValue(constructor.SavedHash, out var item))
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
        }

        private TypeMetadata(string typeName, string namespaceName, int hash)
        {
            if (typeName == null || namespaceName == null)
                throw new ArgumentNullException("Type can't be null.");
            Name = typeName;
            NamespaceName = namespaceName;
            SavedHash = hash;
        }

        private TypeMetadata(string typeName, string namespaceName, IEnumerable<TypeMetadata> genericArguments,
            int hash)
            : this(typeName, namespaceName, hash)
        {
            GenericArguments = genericArguments;
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
            var _access = AccessLevelEnum.IsPrivate;
            var _abstract = AbstractEnum.NotAbstract;
            var _sealed = SealedEnum.NotSealed;
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
            var tm = (TypeMetadata) obj;
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