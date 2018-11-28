using Library.Data.Mode.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;

namespace Library.Data.Model
{
    [DataContract(Name = "Type")]
    [Serializable]
    public class TypeMetadata : IMetadata
    {
        public string Details
        {
            get
            {
                var ret = $"Type: {m_typeName}{(m_BaseType != null ? ",extends " + m_BaseType.Name : string.Empty)}";
                if (m_ImplementedInterfaces != null)
                {
                    ret += ",implements ";
                    foreach (var intf in m_ImplementedInterfaces)
                        ret += $"{intf.Name}, ";
                }
                ret += $"\nType Kind: {m_TypeKind.ToString()}\n";
                ret += $"Modifiers: {m_Modifiers?.Item1.ToString()}," +
                    $"{m_Modifiers?.Item2.ToString()},{m_Modifiers?.Item3.ToString()}.";
                return ret;
            }
        }
        public void Build()
        {

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
            m_typeName = type.Name;
            //FILL CHILDREN
            FillChildren(new StreamingContext { });
            
            _cachedHash = type.GetHashCode();
        }
        #endregion

        #region OnDeserializing
        [OnDeserialized]
        private void FillChildren(StreamingContext context)
        {
            List<AttributeMetadata> amList = new List<AttributeMetadata>();
            if(m_Attributes != null)
                amList.AddRange(m_Attributes.Select(n => new AttributeMetadata(n)));
            List<IMetadata> elems = new List<IMetadata>();
            elems.AddRange(amList);
            if(m_ImplementedInterfaces != null)
                elems.AddRange(m_ImplementedInterfaces);
            if (m_BaseType != null)
                elems.Add(m_BaseType);
            if (m_DeclaringType != null)
                elems.Add(m_DeclaringType);
            if (m_Properties != null)
                elems.AddRange(m_Properties);
            if (m_Constructors != null)
                elems.AddRange(m_Constructors);
            if (m_Methods != null)
                elems.AddRange(m_Methods);
            if (m_NestedTypes != null)
                elems.AddRange(m_NestedTypes);
            if (m_GenericArguments != null)
                elems.AddRange(m_GenericArguments);
            this.Children = elems;
        }
        #endregion

        #region API
        internal enum TypeKind
        {
            Reference, EnumType, StructType, InterfaceType, ClassType
        }
        internal static TypeMetadata EmitReference(Type type)
        {           
            if (!type.IsGenericType)
                return new TypeMetadata(type.Name, type.GetNamespace(), 
                    type.GetHashCode());
            else
                return new TypeMetadata(type.Name, type.GetNamespace(), EmitGenericArguments(type.GetGenericArguments()),
                    type.GetHashCode());
        }
        internal static IEnumerable<TypeMetadata> EmitGenericArguments(IEnumerable<Type> arguments)
        {
            return from Type _argument in arguments select EmitReference(_argument);
        }
        #endregion

        #region private
        //vars
        private string m_typeName;
        [DataMember(Name = "Namespace")]
        private string m_NamespaceName;
        [DataMember(Name = "BaseType")]
        private TypeMetadata m_BaseType;
        [DataMember(Name = "GenericArguments")]
        private IEnumerable<TypeMetadata> m_GenericArguments;
        [DataMember(Name = "Modifiers")]
        private Tuple<AccessLevel, SealedEnum, AbstractENum> m_Modifiers;
        [DataMember(Name = "TypeKind")]
        private TypeKind m_TypeKind;
        [DataMember(Name = "Attributes")]
        private IEnumerable<Attribute> m_Attributes;
        [DataMember(Name = "ImplementedInterfaces")]
        private IEnumerable<TypeMetadata> m_ImplementedInterfaces;
        [DataMember(Name = "NestedTypes")]
        private IEnumerable<TypeMetadata> m_NestedTypes;
        [DataMember(Name = "Properties")]
        private IEnumerable<PropertyMetadata> m_Properties;
        [DataMember(Name = "DeclaringType")]
        private TypeMetadata m_DeclaringType;
        [DataMember(Name = "Methods")]
        private IEnumerable<MethodMetadata> m_Methods;
        [DataMember(Name = "Constructors")]
        private IEnumerable<MethodMetadata> m_Constructors;

        [DataMember(Name = "Name")]
        public string Name
        {
            get
            {
                return m_typeName;
            }
            protected set
            {
                this.m_typeName = value;
            }
        }

        public IEnumerable<IMetadata> Children { get; set; }

        //constructors
        private TypeMetadata(string typeName, string namespaceName, int hash)
        {
            if (typeName == null || namespaceName == null)
                throw new ArgumentNullException("Type can't be null.");
            m_typeName = typeName;
            m_NamespaceName = namespaceName;
            _cachedHash = hash;
        }
        private TypeMetadata(string typeName, string namespaceName, IEnumerable<TypeMetadata> genericArguments, int hash) 
            : this(typeName, namespaceName, hash)
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

        [DataMember(Name = "Hash")]
        private int _cachedHash = 0;

        public override int GetHashCode()
        {
            return _cachedHash;
        }

        public override bool Equals(object obj)
        {
            if (GetType() != obj.GetType())
                return false;
            TypeMetadata tm = ((TypeMetadata)obj);
            if (m_typeName == tm.m_typeName)
            {
                if (m_NamespaceName != tm.m_NamespaceName)
                    return false;
                return true;
            }
            return false;
        }

        public override string ToString()
        {
            return m_typeName;
        }
    }
}