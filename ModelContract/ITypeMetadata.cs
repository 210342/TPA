using System;
using System.Collections.Generic;

namespace ModelContract
{
    public interface ITypeMetadata : IMetadata
    {
        string NamespaceName { get; }
        ITypeMetadata BaseType { get; }
        IEnumerable<ITypeMetadata> GenericArguments { get; }
        Tuple<AccessLevelEnum, SealedEnum, AbstractEnum> Modifiers { get; }
        TypeKindEnum TypeKind { get; }
        IEnumerable<IAttributeMetadata> Attributes { get; }
        IEnumerable<ITypeMetadata> ImplementedInterfaces { get; }
        IEnumerable<ITypeMetadata> NestedTypes { get; }
        IEnumerable<IPropertyMetadata> Properties { get; }
        ITypeMetadata DeclaringType { get; }
        IEnumerable<IMethodMetadata> Methods { get; }
        IEnumerable<IMethodMetadata> Constructors { get; }
    }
}