using Library.Data.Enums;
using Library.Data.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Runtime.CompilerServices;

namespace Library.Data
{
    public static class ReadMetadata
    {
        #region Dictionary
        public static Dictionary<string, IRepresantation> AlreadyRead { get; } =
                new Dictionary<string, IRepresantation>();
        #endregion

        #region Method methods
        internal static IEnumerable<MethodRepresantation> ReadMethods(IEnumerable<MethodBase> methods, string typeName)
        {
            foreach(MethodBase method in methods)
            {
                if(method.GetVisible())
                {
                    string expectedName = $"{typeName}.{ReadReturnType(method).Name} {method.Name}{ReadParameters(method.GetParameters(), method.Name)}";
                    if (AlreadyRead.TryGetValue(expectedName, out IRepresantation reference))
                    {
                        yield return reference as MethodRepresantation;
                    }
                    else
                    {
                        MethodRepresantation newMethod = new MethodRepresantation(method, typeName);
                        AlreadyRead.Add(newMethod.FullName, newMethod);
                        yield return newMethod;
                    }
                }
            }
        }

        internal static Tuple<AccessLevelEnum, AbstractEnum, StaticEnum, VirtualEnum> ReadModifiers(MethodBase method)
        {
            AccessLevelEnum access = AccessLevelEnum.IsPrivate;
            if (method.IsPublic)
                access = AccessLevelEnum.IsPublic;
            else if (method.IsFamily)
                access = AccessLevelEnum.IsProtected;
            else if (method.IsFamilyAndAssembly)
                access = AccessLevelEnum.IsProtectedInternal;
            AbstractEnum _abstract = AbstractEnum.NotAbstract;
            if (method.IsAbstract)
                _abstract = AbstractEnum.Abstract;
            StaticEnum _static = StaticEnum.NotStatic;
            if (method.IsStatic)
                _static = StaticEnum.Static;
            VirtualEnum _virtual = VirtualEnum.NotVirtual;
            if (method.IsVirtual)
                _virtual = VirtualEnum.Virtual;
            return new Tuple<AccessLevelEnum, AbstractEnum, StaticEnum, VirtualEnum>(access, _abstract, _static, _virtual);
        }

        internal static IEnumerable<ParameterRepresantation> ReadParameters(IEnumerable<ParameterInfo> parameters, string methodName)
        {
            return from parameter in parameters
                   select new ParameterRepresantation(parameter.Name, ReadReference(parameter.ParameterType), methodName);
        }

        internal static TypeRepresantation ReadReturnType(MethodBase method)
        {
            MethodInfo methodInfo = method as MethodInfo;
            if (methodInfo == null)
                return null;
            return ReadMetadata.ReadReference(methodInfo.ReturnType);
        }

        internal static bool ReadExtension(MethodBase method)
        {
            return method.IsDefined(typeof(ExtensionAttribute), true);
        }
        #endregion

        #region Type methods
        internal static Tuple<AccessLevelEnum, AbstractEnum, SealedEnum> ReadModifiers(Type type)
        {
            AccessLevelEnum access = AccessLevelEnum.IsPrivate;
            if (type.IsPublic)
                access = AccessLevelEnum.IsPublic;
            else if (type.IsNestedPublic)
                access = AccessLevelEnum.IsPublic;
            else if (type.IsNestedFamily)
                access = AccessLevelEnum.IsProtected;
            else if (type.IsNestedFamANDAssem)
                access = AccessLevelEnum.IsProtectedInternal;
            AbstractEnum _abstract = AbstractEnum.NotAbstract;
            if (type.IsAbstract)
                _abstract = AbstractEnum.Abstract;
            SealedEnum _sealed = SealedEnum.NotSealed;
            if (type.IsSealed)
                _sealed = SealedEnum.Sealed;
            return new Tuple<AccessLevelEnum, AbstractEnum, SealedEnum>(access, _abstract, _sealed);
        }

        internal static TypeRepresantation ReadReference(Type type)
        {
            AlreadyRead.TryGetValue(type.Name, out IRepresantation reference);
            if(reference != null)
            {
                return (TypeRepresantation)reference;
            }
            else
            {
                if (!type.IsGenericType)
                {
                    reference = new TypeRepresantation(type.Name, type.GetNamespace());
                    AlreadyRead.Add(reference.FullName, reference);
                    return (TypeRepresantation)reference;
                }
                else
                {
                    reference = new TypeRepresantation(type.Name, type.GetNamespace(), ReadGenericArguments(type.GetGenericArguments()));
                    AlreadyRead.Add(reference.FullName, reference);
                    return (TypeRepresantation)reference;
                }
            }
        }


        internal static IEnumerable<TypeRepresantation> ReadGenericArguments(IEnumerable<Type> arguments)
        {
            if (arguments.Any())
                return from Type _argument in arguments select ReadReference(_argument);
            else
                return null;
        }

        internal static TypeRepresantation ReadDeclaringType(Type declaringType)
        {
            if (declaringType == null)
                return null;
            AlreadyRead.TryGetValue(declaringType.FullName, out IRepresantation reference);
            if (reference != null)
            {
                return (TypeRepresantation)reference;
            }
            else
            {
                return ReadMetadata.ReadReference(declaringType);
            }
        }

        internal static IEnumerable<TypeRepresantation> ReadNestedTypes(IEnumerable<Type> nestedTypes)
        {
            foreach(Type type in nestedTypes)
            {
                AlreadyRead.TryGetValue(type.FullName, out IRepresantation reference);
            }
            return from type in nestedTypes
                   where type.GetVisible()
                   select new TypeRepresantation(type);
        }

        internal static IEnumerable<TypeRepresantation> ReadImplements(IEnumerable<Type> interfaces)
        {
            return from currentInterface in interfaces
                   select ReadMetadata.ReadReference(currentInterface);
        }

        internal static TypeKindEnum GetTypeKind(Type type)
        {
            return type.IsEnum ? TypeKindEnum.EnumType :
                   type.IsValueType ? TypeKindEnum.StructType :
                   type.IsInterface ? TypeKindEnum.InterfaceType :
                   TypeKindEnum.ClassType;
        }

        internal static TypeRepresantation ReadExtends(Type baseType)
        {
            if (baseType == null || baseType == typeof(Object) || baseType == typeof(ValueType) || baseType == typeof(Enum))
                return null;
            return ReadMetadata.ReadReference(baseType);
        }
        #endregion

        #region Property method
        internal static IEnumerable<PropertyRepresantation> ReadProperties(IEnumerable<PropertyInfo> properties, string className)
        {
            return from property in properties
                   where property.GetGetMethod().GetVisible() || property.GetSetMethod().GetVisible()
                   select new PropertyRepresantation(property.Name, ReadMetadata.ReadReference(property.PropertyType), className);
        }
        #endregion

        #region Namespace method
        internal static IEnumerable<TypeRepresantation> ReadTypes(IEnumerable<Type> types)
        {
            return from type in types
                   orderby type.Name
                   select new TypeRepresantation(type);
        }
        #endregion

        #region Assembly method
        internal static IEnumerable<NamespaceRepresantation> ReadNamespaces(Assembly assembly)
        {
            return from Type type in assembly.GetTypes()
                   where type.GetVisible()
                   group type by type.GetNamespace() into _group
                   orderby _group.Key
                   select new NamespaceRepresantation(_group.Key, _group);
        }
        #endregion
    }
}
