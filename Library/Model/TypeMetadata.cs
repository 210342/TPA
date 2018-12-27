using Library.Model;
using ModelContract;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;

namespace Library.Model
{
    public class TypeMetadata : ITypeMetadata
    {
        #region constructors
        internal TypeMetadata(Type type)
        {
            if (type == null)
                throw new ArgumentNullException("Type can't be null.");
            DeclaringType = EmitDeclaringType(type.DeclaringType);
            Constructors = MethodMetadata.EmitMethods(type.GetConstructors());
            Methods = MethodMetadata.EmitMethods(type.GetMethods());
            NestedTypes = EmitNestedTypes(type.GetNestedTypes());
            ImplementedInterfaces = EmitImplements(type.GetInterfaces());
            GenericArguments = !type.IsGenericTypeDefinition ? null : TypeMetadata.EmitGenericArguments(type.GetGenericArguments());
            Modifiers = EmitModifiers(type);
            BaseType = EmitExtends(type.BaseType);
            Properties = PropertyMetadata.EmitProperties(type.GetProperties());
            TypeKind = GetTypeKind(type);
            
            Attributes = new List<AttributeMetadata>();
            type.GetCustomAttributes(false).Cast<Attribute>().ToList().ForEach( n => 
                        ((List < AttributeMetadata >) Attributes).Add(new AttributeMetadata(n)));
            Name = type.Name;
            //FILL CHILDREN
            FillChildren(new StreamingContext { });
            
            SavedHash = type.GetHashCode();
        }

        internal TypeMetadata()
        {
        }

        private TypeMetadata(string typeName, string namespaceName, int hash)
        {
            if (typeName == null || namespaceName == null)
                throw new ArgumentNullException("Type can't be null.");
            Name = typeName;
            NamespaceName = namespaceName;
            SavedHash = hash;
        }
        private TypeMetadata(string typeName, string namespaceName, IEnumerable<TypeMetadata> genericArguments, int hash)
            : this(typeName, namespaceName, hash)
        {
            GenericArguments = genericArguments;
        }
        #endregion

        #region OnDeserializing
        [OnDeserialized]
        private void FillChildren(StreamingContext context)
        {
            List<IAttributeMetadata> amList = new List<IAttributeMetadata>();
            if(Attributes != null)
                amList.AddRange(Attributes.Select(n => n));
            List<IMetadata> elems = new List<IMetadata>();
            elems.AddRange(amList);
            if(ImplementedInterfaces != null)
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
        #endregion

        #region API
        
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
        private static TypeKindEnum GetTypeKind(Type type) //#80 TPA: Reflection - Invalid return value of GetTypeKind() 
        {
            return type.IsEnum ? TypeKindEnum.EnumType :
                   type.IsValueType ? TypeKindEnum.StructType :
                   type.IsInterface ? TypeKindEnum.InterfaceType :
                   TypeKindEnum.ClassType;
        }
        static Tuple<AccessLevelEnum, SealedEnum, AbstractEnum> EmitModifiers(Type type)
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
            if (baseType == null || baseType == typeof(Object) || baseType == typeof(ValueType) || baseType == typeof(Enum))
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
            TypeMetadata tm = ((TypeMetadata)obj);
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
            return (Modifiers?.Item1.ToString()) + (Modifiers?.Item2.ToString())
                + (Modifiers?.Item3.ToString() + Modifiers?.ToString());
        }

        #endregion
    }
}