using Library.Data.Enums;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Library.Data.Model
{
    internal class TypeRepresentation : IRepresentation
    {
        #region properties
        public string Name { get; private set; }
        public string FullName { get; private set; }
        public TypeRepresentation BaseType { get; private set; }
        public IEnumerable<TypeRepresentation> ImplementedInterfaces { get; private set; }
        public Tuple<AccessLevelEnum, AbstractEnum, SealedEnum> Modifiers { get; private set; }
        public TypeRepresentation DeclaringType { get; private set; }
        public TypeKindEnum TypeKind { get; private set; }
        public IEnumerable<Attribute> Attributes { get; private set; }
        public IEnumerable<TypeRepresentation> GenericArguments { get; private set; }
        public IEnumerable<TypeRepresentation> NestedTypes { get; private set; }
        public IEnumerable<PropertyRepresentation> Properties { get; private set; }
        public IEnumerable<MethodRepresentation> Methods { get; private set; }
        public IEnumerable<MethodRepresentation> Constructors { get; private set; }
        public IEnumerable<IRepresentation> Children
        {
            get
            {
                if (BaseType != null)
                {
                    yield return BaseType;
                }
                if(ImplementedInterfaces != null)
                {
                    foreach (TypeRepresentation _interface in ImplementedInterfaces)
                    {
                        yield return _interface;
                    }
                }
                if (GenericArguments != null)
                {
                    foreach (TypeRepresentation genericArgument in GenericArguments)
                    {
                        yield return genericArgument;
                    }
                }
                if(NestedTypes != null)
                {
                    foreach (TypeRepresentation nestedType in NestedTypes)
                    {
                        yield return nestedType;
                    }
                }
                if(Properties != null)
                {
                    foreach (PropertyRepresentation property in Properties)
                    {
                        yield return property;
                    }
                }
                if(Constructors != null)
                {
                    foreach (MethodRepresentation constructor in Constructors)
                    {
                        yield return constructor;
                    }
                }
                if(Methods != null)
                {
                    foreach (MethodRepresentation method in Methods)
                    {
                        yield return method;
                    }
                }
                if (DeclaringType != null)
                {
                    yield return DeclaringType;
                }
            }
        }
        public string ToStringProperty
        {
            get
            {
                return ToString();
            }
        }
        #endregion

        #region constructors
        internal TypeRepresentation(Type type)
        {
            Name = type.Name;
            FullName = type.FullName;
            DeclaringType = ReadMetadata.ReadDeclaringType(type.DeclaringType);
            Constructors = ReadMetadata.ReadMethods(type.GetConstructors(), FullName);
            Methods = ReadMetadata.ReadMethods(type.GetMethods(), FullName);
            NestedTypes = ReadMetadata.ReadNestedTypes(type.GetNestedTypes());
            ImplementedInterfaces = ReadMetadata.ReadImplements(type.GetInterfaces());
            GenericArguments = ReadMetadata.ReadGenericArguments(type.GetGenericArguments());
            Modifiers = ReadMetadata.ReadModifiers(type);
            BaseType = ReadMetadata.ReadExtends(type.BaseType);
            Properties = ReadMetadata.ReadProperties(type.GetProperties(), FullName);
            TypeKind = ReadMetadata.GetTypeKind(type);
            Attributes = type.GetCustomAttributes(inherit: true).Cast<Attribute>();
        }

        internal TypeRepresentation(string name, string namespaceName)
        {
            Name = name;
            FullName = $"{namespaceName}.{name}";
        }

        internal TypeRepresentation(string typeName, string namespaceName, IEnumerable<TypeRepresentation> genericArguments) : this(typeName, namespaceName)
        {
            GenericArguments = genericArguments;
        }
        #endregion

        public IEnumerable<string> Print()
        {
            yield return $"NAME: {FullName}";
            if(BaseType != null)
            {
                yield return $"Base type: {BaseType.FullName}";
            }
            if(ImplementedInterfaces != null)
            {
                foreach (TypeRepresentation _interface in ImplementedInterfaces)
                {
                    yield return $"Implements interface: {_interface.Name}";
                }
            }
            yield return $"Modifiers: {Modifiers.ToString()}";
            yield return $"Type: {TypeKind.ToString()}";
            if(Attributes != null)
            {
                foreach (Attribute attribute in Attributes)
                {
                    yield return $"Attribute: {attribute.ToString()}";
                }
            }
            if(GenericArguments != null)
            {
                foreach (TypeRepresentation genericArgument in GenericArguments)
                {
                    yield return $"Generic argument: {genericArgument.Name}";
                }
            }
            if(NestedTypes != null)
            {
                foreach (TypeRepresentation nestedType in NestedTypes)
                {
                    yield return $"Nested type: {nestedType.Name}";
                }
            }
            if(Properties != null)
            {
                foreach (PropertyRepresentation property in Properties)
                {
                    yield return $"Property: {property.Name}";
                }
            }
            if(Constructors != null)
            {
                foreach (MethodRepresentation constructor in Constructors)
                {
                    yield return $"Constructor: {constructor.Name}";
                }
            }
            if(Methods != null)
            {
                foreach (MethodRepresentation method in Methods)
                {
                    yield return $"Method: {method.Name}";
                }
            }
            if (DeclaringType != null)
            {
                yield return $"Declaring type: {DeclaringType.FullName}";
            }
        }

        public override string ToString()
        {
            return string.Join(Environment.NewLine, Print());
        }
    }
}
