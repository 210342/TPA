using Library.Data.Enums;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Library.Data.Model
{
    internal class TypeRepresantation : IRepresantation
    {
        #region properties
        public string Name { get; private set; }
        public string FullName { get; private set; }
        public TypeRepresantation BaseType { get; private set; }
        public IEnumerable<TypeRepresantation> ImplementedInterfaces { get; private set; }
        public Tuple<AccessLevelEnum, AbstractEnum, SealedEnum> Modifiers { get; private set; }
        public TypeRepresantation DeclaringType { get; private set; }
        public TypeKindEnum TypeKind { get; private set; }
        public IEnumerable<Attribute> Attributes { get; private set; }
        public IEnumerable<TypeRepresantation> GenericArguments { get; private set; }
        public IEnumerable<TypeRepresantation> NestedTypes { get; private set; }
        public IEnumerable<PropertyRepresantation> Properties { get; private set; }
        public IEnumerable<MethodRepresantation> Methods { get; private set; }
        public IEnumerable<MethodRepresantation> Constructors { get; private set; }
        #endregion

        #region constructors
        internal TypeRepresantation(Type type)
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

        internal TypeRepresantation(string name, string namespaceName)
        {
            Name = name;
            FullName = $"{namespaceName}.{name}";
        }

        internal TypeRepresantation(string typeName, string namespaceName, IEnumerable<TypeRepresantation> genericArguments) : this(typeName, namespaceName)
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
            foreach(TypeRepresantation _interface in ImplementedInterfaces)
            {
                yield return $"Implements interface: {_interface.Name}";
            }
            yield return $"Modifiers: {Modifiers.ToString()}";
            yield return $"Type: {TypeKind.ToString()}";
            foreach(Attribute attribute in Attributes)
            {
                yield return $"Attribute: {attribute.ToString()}";
            }
            foreach(TypeRepresantation genericArgument in GenericArguments)
            {
                yield return $"Generic argument: {genericArgument.Name}";
            }
            foreach(TypeRepresantation nestedType in NestedTypes)
            {
                yield return $"Nested type: {nestedType.Name}";
            }
            foreach(PropertyRepresantation property in Properties)
            {
                yield return $"Property: {property.Name}";
            }
            foreach(MethodRepresantation constructor in Constructors)
            {
                yield return $"Constructor: {constructor.Name}{constructor.PrintParametersHumanReadable()}";
            }
            foreach (MethodRepresantation method in Methods)
            {
                yield return $"Method: {method.Name}{method.PrintParametersHumanReadable()}";
            }
            if (DeclaringType != null)
            {
                yield return $"Declaring type: {DeclaringType.FullName}";
            }
        }
    }
}
