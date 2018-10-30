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
        public IEnumerable<string> Children
        {
            get
            {
                return Print();
            }
        }
        #endregion

        #region constructor
        internal MethodRepresentation(MethodBase method, string className)
        {
            Name = method.Name;
            GenericArguments = ReadMetadata.ReadGenericArguments(method.GetGenericArguments());
            ReturnType = ReadMetadata.ReadReturnType(method);
            Parameters = ReadMetadata.ReadParameters(method.GetParameters(), Name);
            FullName = $"{className}.{ReturnType.Name} {method.Name}{PrintParametersHumanReadable()}";
            Modifiers = ReadMetadata.ReadModifiers(method);
            Extension = ReadMetadata.ReadExtension(method);
        }
        #endregion

        #region Methods
        public string PrintParametersHumanReadable()
        {
            StringBuilder sb = new StringBuilder("(");
            foreach(ParameterRepresentation parameter in Parameters)
            {
                sb.Append($"{parameter.Type.Name} {parameter.Name}, ");
            }
            sb.Remove(sb.Length - 2, 2); // remove last comma and space
            sb.Append(")");
            return sb.ToString();
        }

        public IEnumerable<string> Print()
        {
            yield return $"NAME: {Name}";
            foreach (TypeRepresentation genericArgument in GenericArguments)
            {
                yield return $"Generic argument: {genericArgument.Name}";
            }
            foreach (ParameterRepresentation parameter in Parameters)
            {
                yield return $"Parameter: {parameter.Name}";
            }
            yield return $"Returned type: {ReturnType.Name}";
            yield return $"Modifiers: {Modifiers.ToString()}";
            yield return $"Is extension: {Extension.ToString()}";
        }
        #endregion
    }
}
