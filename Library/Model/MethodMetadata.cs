using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Runtime.Serialization;
using System.Text;
using ModelContract;

namespace Library.Model
{
    public class MethodMetadata : AbstractMapper, IMethodMetadata
    {
        private MethodMetadata(MethodBase method)
        {
            Name = method.Name;
            GenericArguments = !method.IsGenericMethodDefinition
                ? null
                : TypeMetadata.EmitGenericArguments(method.GetGenericArguments());
            ReturnType = EmitReturnType(method);
            Parameters = EmitParameters(method.GetParameters());
            Modifiers = EmitModifiers(method);
            IsExtension = EmitExtension(method);
            FillChildren(new StreamingContext());
            SavedHash = method.GetHashCode();
        }

        internal MethodMetadata()
        {
        }

        public MethodMetadata(IMethodMetadata methodMetadata)
        {
            Name = methodMetadata.Name;
            SavedHash = methodMetadata.SavedHash;
            IsExtension = methodMetadata.IsExtension;
            Modifiers = methodMetadata.Modifiers;

            // Generic Arguments
            if (methodMetadata.GenericArguments is null)
            {
                GenericArguments = null;
            }
            else
            {
                var genericArguments = new List<ITypeMetadata>();
                foreach (var genericArgument in methodMetadata.GenericArguments)
                    if (AlreadyMapped.TryGetValue(genericArgument.SavedHash, out var mappedArgument))
                    {
                        genericArguments.Add(mappedArgument as ITypeMetadata);
                    }
                    else
                    {
                        ITypeMetadata newType = new TypeMetadata(genericArgument);
                        genericArguments.Add(newType);
                        AlreadyMapped.Add(newType.SavedHash, newType);
                    }

                GenericArguments = genericArguments;
            }

            // Return type
            if (AlreadyMapped.TryGetValue(methodMetadata.ReturnType.SavedHash, out var item))
            {
                ReturnType = item as ITypeMetadata;
            }
            else
            {
                ITypeMetadata newType = new TypeMetadata(methodMetadata.ReturnType);
                ReturnType = newType;
                AlreadyMapped.Add(newType.SavedHash, newType);
            }

            // Parameters
            if (methodMetadata.Parameters is null)
            {
                Parameters = Enumerable.Empty<IParameterMetadata>();
            }
            else
            {
                var parameters = new List<IParameterMetadata>();
                foreach (var parameter in methodMetadata.Parameters)
                    if (AlreadyMapped.TryGetValue(parameter.SavedHash, out item))
                    {
                        parameters.Add(item as IParameterMetadata);
                    }
                    else
                    {
                        IParameterMetadata newParameter = new ParameterMetadata(parameter);
                        parameters.Add(newParameter);
                        AlreadyMapped.Add(newParameter.SavedHash, newParameter);
                    }

                Parameters = parameters;
            }

            FillChildren(new StreamingContext());
        }

        internal static IEnumerable<MethodMetadata> EmitMethods(IEnumerable<MethodBase> methods)
        {
            return from MethodBase _currentMethod in methods
                where _currentMethod.GetVisible()
                select new MethodMetadata(_currentMethod);
        }

        private void FillChildren(StreamingContext context)
        {
            var elems = new List<IMetadata> {ReturnType};
            elems.AddRange(Parameters);
            Children = elems;
        }

        #region properties

        public string Name { get; }
        public IEnumerable<ITypeMetadata> GenericArguments { get; }
        public ITypeMetadata ReturnType { get; }
        public bool IsExtension { get; }
        public IEnumerable<IParameterMetadata> Parameters { get; }
        public Tuple<AccessLevelEnum, AbstractEnum, StaticEnum, VirtualEnum> Modifiers { get; }
        public IEnumerable<IMetadata> Children { get; private set; }
        public int SavedHash { get; }

        #endregion

        #region methods

        private static IEnumerable<ParameterMetadata> EmitParameters(IEnumerable<ParameterInfo> parms)
        {
            return from parm in parms
                select new ParameterMetadata(parm.Name, TypeMetadata.EmitReference(parm.ParameterType));
        }

        private static TypeMetadata EmitReturnType(MethodBase method)
        {
            var methodInfo = method as MethodInfo;
            if (methodInfo == null)
                return null;
            return TypeMetadata.EmitReference(methodInfo.ReturnType);
        }

        private static bool EmitExtension(MethodBase method)
        {
            return method.IsDefined(typeof(ExtensionAttribute), true);
        }

        private static Tuple<AccessLevelEnum, AbstractEnum, StaticEnum, VirtualEnum> EmitModifiers(MethodBase method)
        {
            var _access = AccessLevelEnum.IsPrivate;
            if (method.IsPublic)
                _access = AccessLevelEnum.IsPublic;
            else if (method.IsFamily)
                _access = AccessLevelEnum.IsProtected;
            else if (method.IsFamilyAndAssembly)
                _access = AccessLevelEnum.IsProtectedInternal;
            var _abstract = AbstractEnum.NotAbstract;
            if (method.IsAbstract)
                _abstract = AbstractEnum.Abstract;
            var _static = StaticEnum.NotStatic;
            if (method.IsStatic)
                _static = StaticEnum.Static;
            var _virtual = VirtualEnum.NotVirtual;
            if (method.IsVirtual)
                _virtual = VirtualEnum.Virtual;
            return new Tuple<AccessLevelEnum, AbstractEnum, StaticEnum, VirtualEnum>(_access, _abstract, _static,
                _virtual);
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
            var mm = (MethodMetadata) obj;
            if (Name == mm.Name)
            {
                if (ReturnType != mm.ReturnType)
                    return false;
                var counter = 0;
                foreach (var el in Parameters)
                    if (mm.Parameters.Any(n => n.Equals(el)))
                        ++counter;
                if (counter != Parameters.Count())
                    return false;
                return true;
            }

            return false;
        }

        public override string ToString()
        {
            var paramsString = new StringBuilder("(");
            if (Parameters.Count() != 0)
            {
                foreach (ParameterMetadata parameter in Parameters)
                    paramsString.Append($"{parameter.TypeMetadata.Name} {parameter.Name}, ");
                paramsString.Remove(paramsString.Length - 2, 2); // remove last comma and space
                paramsString.Append(")");
            }
            else
            {
                paramsString.Append(")");
            }

            return $"{(ReturnType != null ? "" + ReturnType.Name : "")} {Name}{paramsString}";
        }

        public string ModifiersString()
        {
            return Modifiers?.Item1 + Modifiers?.Item2.ToString() + Modifiers?.Item3 + Modifiers?.Item4;
        }

        #endregion
    }
}