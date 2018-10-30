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
        public static Dictionary<string, IRepresentation> AlreadyRead { get; } =
                new Dictionary<string, IRepresentation>();
        #endregion

        #region Method methods
        internal static IEnumerable<MethodRepresentation> ReadMethods(IEnumerable<MethodBase> methods, string typeName)
        {
            foreach(MethodBase method in methods)
            {
                if(method.GetVisible())
                {
                    string returnedTypeName = ReadReturnType(method) == null ? "" : ReadReturnType(method).Name;
                    string expectedName = $"{typeName}.{returnedTypeName} {method.Name}" +
                        $"{ParameterRepresentation.PrintParametersHumanReadable(ReadParameters(method.GetParameters(), method.Name))}";
                    if (AlreadyRead.TryGetValue(expectedName, out IRepresentation reference))
                    {
                        yield return reference as MethodRepresentation;
                    }
                    else
                    {
                        MethodRepresentation newMethod = new MethodRepresentation(method, typeName);
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

        internal static IEnumerable<ParameterRepresentation> ReadParameters(IEnumerable<ParameterInfo> parameters, string methodName)
        {
            foreach(ParameterInfo parameter in parameters)
            {
                string expectedName = $"{methodName}.{parameter.Name}";
                if(AlreadyRead.TryGetValue(expectedName, out IRepresentation reference))
                {
                    yield return reference as ParameterRepresentation;
                }
                else
                {
                    ParameterRepresentation newParameter = new ParameterRepresentation
                        (parameter.Name, ReadReference(parameter.ParameterType), methodName);
                    AlreadyRead.Add(newParameter.FullName, newParameter);
                    yield return newParameter;
                }
            }
        }

        internal static TypeRepresentation ReadReturnType(MethodBase method)
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

        internal static TypeRepresentation ReadReference(Type type)
        {
            if(AlreadyRead.TryGetValue(type.FullName, out IRepresentation reference))
            {
                return reference as TypeRepresentation;
            }
            else
            {
                if (!type.IsGenericType)
                {
                    TypeRepresentation newType = new TypeRepresentation(type.Name, type.GetNamespace());
                    AlreadyRead.Add(newType.FullName, newType);
                    return newType;
                }
                else
                {
                    TypeRepresentation newType = new TypeRepresentation
                        (type.Name, type.GetNamespace(), ReadGenericArguments(type.GetGenericArguments()));
                    AlreadyRead.Add(newType.FullName, newType);
                    return newType as TypeRepresentation;
                }
            }
        }


        internal static IEnumerable<TypeRepresentation> ReadGenericArguments(IEnumerable<Type> arguments)
        {
            if (arguments.Any())
                return from Type _argument in arguments select ReadReference(_argument);
            else
                return null;
        }

        internal static TypeRepresentation ReadDeclaringType(Type declaringType)
        {
            if (declaringType == null)
                return null;
            if (AlreadyRead.TryGetValue(declaringType.FullName, out IRepresentation reference))
            {
                return reference as TypeRepresentation;
            }
            else
            {
                return ReadReference(declaringType);
            }
        }

        internal static IEnumerable<TypeRepresentation> ReadNestedTypes(IEnumerable<Type> nestedTypes)
        {
            foreach(Type type in nestedTypes)
            {
                if (type.GetVisible())
                {
                    if (AlreadyRead.TryGetValue(type.FullName, out IRepresentation reference))
                    {
                        yield return reference as TypeRepresentation;
                    }
                    else
                    {
                        TypeRepresentation newType = new TypeRepresentation(type.Name, type.GetNamespace());
                        AlreadyRead.Add(newType.FullName, newType);
                        yield return newType;
                    }
                }
            }
        }

        internal static IEnumerable<TypeRepresentation> ReadImplements(IEnumerable<Type> interfaces)
        {
            return from currentInterface in interfaces
                   select ReadReference(currentInterface);
        }

        internal static TypeKindEnum GetTypeKind(Type type)
        {
            return type.IsEnum ? TypeKindEnum.EnumType :
                   type.IsValueType ? TypeKindEnum.StructType :
                   type.IsInterface ? TypeKindEnum.InterfaceType :
                   TypeKindEnum.ClassType;
        }

        internal static TypeRepresentation ReadExtends(Type baseType)
        {
            if (baseType == null || baseType == typeof(Object) || baseType == typeof(ValueType) || baseType == typeof(Enum))
                return null;
            return ReadMetadata.ReadReference(baseType);
        }
        #endregion

        #region Property method
        internal static IEnumerable<PropertyRepresentation> ReadProperties(IEnumerable<PropertyInfo> properties, string className)
        {
            foreach(PropertyInfo property in properties)
            {
                string expectedName = $"{className}.{property.Name}";
                if(AlreadyRead.TryGetValue(expectedName, out IRepresentation reference))
                {
                    yield return reference as PropertyRepresentation;
                }
                else
                {
                    PropertyRepresentation newProperty = new PropertyRepresentation
                        (property.Name, ReadReference(property.PropertyType), className);
                    AlreadyRead.Add(newProperty.FullName, newProperty);
                    yield return newProperty;
                }
            }
        }
        #endregion

        #region Namespace method
        internal static IEnumerable<TypeRepresentation> ReadTypes(IEnumerable<Type> types)
        {
            foreach(Type type in types.OrderBy(n => n.Name))
            {
                if(AlreadyRead.TryGetValue(type.FullName, out IRepresentation reference))
                {
                    yield return reference as TypeRepresentation;
                }
                else
                {
                    TypeRepresentation newType = new TypeRepresentation(type);
                    AlreadyRead.Add(newType.FullName, newType);
                    yield return newType;
                }
            }
        }
        #endregion

        #region Assembly method
        internal static IEnumerable<NamespaceRepresentation> ReadNamespaces(Assembly assembly)
        {
            IEnumerable<IGrouping<string, Type>> namespaces = from Type type in assembly.GetTypes()
                                                              //where type.GetVisible()
                                                              group type by type.GetNamespace() into _group
                                                              select _group;
            foreach (IGrouping<string, Type> _namespace in namespaces)
            {
                if (AlreadyRead.TryGetValue(_namespace.Key, out IRepresentation reference))
                {
                    yield return reference as NamespaceRepresentation;
                }
                else
                {
                    NamespaceRepresentation newNamespace = new NamespaceRepresentation(_namespace.Key, _namespace);
                    AlreadyRead.Add(newNamespace.FullName, newNamespace);
                    yield return newNamespace;
                }
            }
        }
        #endregion
    }
}
