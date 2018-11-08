using Library.Data.Mode.Enums;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Library.Data.Model
{
    internal class TypeMetadata : IMetadata
    {
        public string Details
        {
            get
            {
                var ret = $"Type: {m_typeName}{(m_BaseType != null ? ",extends " + m_BaseType.Name : string.Empty)}";
                if (m_ImplementedInterfaces.Count() > 0)
                {
                    ret += ",implements ";
                    foreach (var intf in m_ImplementedInterfaces)
                        ret += $"{intf.Name}, ";
                }
                ret += $"\nType Kind: {m_TypeKind.ToString()}\n";
                ret += $"Modifiers: {m_Modifiers.Item1.ToString()}," +
                    $"{m_Modifiers.Item2.ToString()},{m_Modifiers.Item3.ToString()}.";
                return ret;
            }
        }

        #region constructors
        internal TypeMetadata(Type type)
        {
            if (type == null)
                throw new ArgumentNullException("Type can't be null.");
            m_DeclaringType = EmitDeclaringType(type.DeclaringType);
            m_Constructors = MethodMetadata.EmitMethods(type.GetConstructors());
            m_Methods = MethodMetadata.EmitMethods(type.GetMethods());
            m_NestedTypes = EmitNestedTypes(type.GetNestedTypes());
            m_ImplementedInterfaces = EmitImplements(type.GetInterfaces());
            m_GenericArguments = !type.IsGenericTypeDefinition ? null : TypeMetadata.EmitGenericArguments(type.GetGenericArguments());
            m_Modifiers = EmitModifiers(type);
            m_BaseType = EmitExtends(type.BaseType);
            m_Properties = PropertyMetadata.EmitProperties(type.GetProperties());
            m_TypeKind = GetTypeKind(type);
            m_Attributes = type.GetCustomAttributes(false).Cast<Attribute>();
            List<AttributeMetadata> amList = new List<AttributeMetadata>();
            amList.AddRange(m_Attributes.Select(n => new AttributeMetadata(n)));
            m_typeName = type.Name;

            List<IMetadata> elems = new List<IMetadata>();
            elems.AddRange(amList);
            elems.AddRange(m_ImplementedInterfaces);
            elems.Add(m_BaseType);
            elems.Add(m_DeclaringType);
            elems.AddRange(m_Properties);
            elems.AddRange(m_Constructors);
            elems.AddRange(m_Methods);
            elems.AddRange(m_NestedTypes);
            if (m_GenericArguments != null)
                elems.AddRange(m_GenericArguments);
            Children = elems;
        }
        #endregion

        #region API
        internal enum TypeKind
        {
            EnumType, StructType, InterfaceType, ClassType
        }
        internal static TypeMetadata EmitReference(Type type)
        {
            if (!type.IsGenericType)
                return new TypeMetadata(type.Name, type.GetNamespace());
            else
                return new TypeMetadata(type.Name, type.GetNamespace(), EmitGenericArguments(type.GetGenericArguments()));
        }
        internal static IEnumerable<TypeMetadata> EmitGenericArguments(IEnumerable<Type> arguments)
        {
            return from Type _argument in arguments select EmitReference(_argument);
        }
        #endregion

        #region private
        //vars
        private string m_typeName;
        private string m_NamespaceName;
        private TypeMetadata m_BaseType;
        private IEnumerable<TypeMetadata> m_GenericArguments;
        private Tuple<AccessLevel, SealedEnum, AbstractENum> m_Modifiers;
        private TypeKind m_TypeKind;
        private IEnumerable<Attribute> m_Attributes;
        private IEnumerable<TypeMetadata> m_ImplementedInterfaces;
        private IEnumerable<TypeMetadata> m_NestedTypes;
        private IEnumerable<PropertyMetadata> m_Properties;
        private TypeMetadata m_DeclaringType;
        private IEnumerable<MethodMetadata> m_Methods;
        private IEnumerable<MethodMetadata> m_Constructors;

        public string Name => m_typeName;

        public IEnumerable<IMetadata> Children { get; }

        //constructors
        private TypeMetadata(string typeName, string namespaceName)
        {
            if (typeName == null || namespaceName == null)
                throw new ArgumentNullException("Type can't be null.");
            m_typeName = typeName;
            m_NamespaceName = namespaceName;
        }
        private TypeMetadata(string typeName, string namespaceName, IEnumerable<TypeMetadata> genericArguments) : this(typeName, namespaceName)
        {
            m_GenericArguments = genericArguments;
        }
        //methods
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
        private static TypeKind GetTypeKind(Type type) //#80 TPA: Reflection - Invalid return value of GetTypeKind() 
        {
            return type.IsEnum ? TypeKind.EnumType :
                   type.IsValueType ? TypeKind.StructType :
                   type.IsInterface ? TypeKind.InterfaceType :
                   TypeKind.ClassType;
        }
        static Tuple<AccessLevel, SealedEnum, AbstractENum> EmitModifiers(Type type)
        {
            //set defaults 
            AccessLevel _access = AccessLevel.IsPrivate;
            AbstractENum _abstract = AbstractENum.NotAbstract;
            SealedEnum _sealed = SealedEnum.NotSealed;
            // check if not default 
            if (type.IsPublic)
                _access = AccessLevel.IsPublic;
            else if (type.IsNestedPublic)
                _access = AccessLevel.IsPublic;
            else if (type.IsNestedFamily)
                _access = AccessLevel.IsProtected;
            else if (type.IsNestedFamANDAssem)
                _access = AccessLevel.IsProtectedInternal;
            if (type.IsSealed)
                _sealed = SealedEnum.Sealed;
            if (type.IsAbstract)
                _abstract = AbstractENum.Abstract;
            return new Tuple<AccessLevel, SealedEnum, AbstractENum>(_access, _sealed, _abstract);
        }
        private static TypeMetadata EmitExtends(Type baseType)
        {
            if (baseType == null || baseType == typeof(Object) || baseType == typeof(ValueType) || baseType == typeof(Enum))
                return null;
            return EmitReference(baseType);
        }
        #endregion

        public override int GetHashCode()
        {
            //return savedHash;
            var hash = 37;
            hash *= 17 + m_typeName.GetHashCode();
            //hash *= 17 + m_NamespaceName.GetHashCode();
            return hash;
        }

        public override bool Equals(object obj)
        {
            if (this.GetType() != obj.GetType())
                return false;
            TypeMetadata tm = ((TypeMetadata)obj);
            if (this.m_typeName == tm.m_typeName)
            {
                if (m_NamespaceName != tm.m_NamespaceName)
                    return false;
            }
            return false;
        }

        public override string ToString()
        {
            return m_typeName;
        }
    }
}