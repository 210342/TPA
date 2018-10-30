using Library.Data.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Text;

namespace Library.Data.Model
{
    internal class MethodRepresentation : IRepresentation
    {
        #region properties
        public string Name { get; private set; }
        public string FullName { get; private set; }
        public IEnumerable<TypeRepresentation> GenericArguments { get; private set; }
        public Tuple<AccessLevelEnum, AbstractEnum, StaticEnum, VirtualEnum> Modifiers { get; private set; }
        public TypeRepresentation ReturnType { get; private set; }
        public bool Extension { get; private set; }
        public IEnumerable<ParameterRepresentation> Parameters { get; private set; }
        public IEnumerable<IRepresentation> Children
        {
            get
            {
                if (GenericArguments != null)
                {
                    foreach (TypeRepresentation genericArgument in GenericArguments)
                    {
                        yield return genericArgument;
                    }
                }
                foreach (ParameterRepresentation parameter in Parameters)
                {
                    yield return parameter;
                }
                if (ReturnType != null)
                {
                    yield return ReturnType;
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

        #region constructor
        internal MethodRepresentation(MethodBase method, string className)
        {
            try
            {
                GenericArguments = ReadMetadata.ReadGenericArguments(method.GetGenericArguments());
            }
            catch(NotSupportedException)
            {
                GenericArguments = null;
            }
            ReturnType = ReadMetadata.ReadReturnType(method);
            Parameters = ReadMetadata.ReadParameters(method.GetParameters(), method.Name);
            Modifiers = ReadMetadata.ReadModifiers(method);
            Extension = ReadMetadata.ReadExtension(method);
            if (ReturnType != null)
            {
                Name = $"{ReturnType.Name} {method.Name}{ParameterRepresentation.PrintParametersHumanReadable(Parameters)}";
            }
            else
            {
                Name = $" {method.Name}{ParameterRepresentation.PrintParametersHumanReadable(Parameters)}";
            }
            FullName = $"{className}.{Name}";
        }
        #endregion

        #region Methods
        public IEnumerable<string> Print()
        {
            yield return $"NAME: {Name}";
            if (GenericArguments != null)
            {
                foreach (TypeRepresentation genericArgument in GenericArguments)
                {
                    yield return $"Generic argument: {genericArgument.Name}";
                }
            }
            foreach (ParameterRepresentation parameter in Parameters)
            {
                yield return $"Parameter: {parameter.Name}";
            }
            if(ReturnType != null)
            {
                yield return $"Returned type: {ReturnType.Name}";
            }
            else
            {
                yield return $"Returned type: void";
            }
            yield return $"Modifiers: {Modifiers.ToString()}";
            yield return $"Is extension: {Extension.ToString()}";
        }

        public override string ToString()
        {
            return string.Join(Environment.NewLine, Print());
        }
        #endregion
    }
}
