using Library.Data.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Runtime.CompilerServices;

namespace Library.Data.Model
{
    internal class MethodRepresantation : IRepresantation
    {
        #region properties
        public string Name { get; private set; }
        public IEnumerable<TypeRepresantation> GenericArguments { get; private set; }
        public Tuple<AccessLevelEnum, AbstractEnum, StaticEnum, VirtualEnum> Modifiers { get; private set; }
        public TypeRepresantation ReturnType { get; private set; }
        public bool Extension { get; private set; }
        public IEnumerable<ParameterRepresantation> Parameters { get; private set; }
        #endregion

        #region constructor
        internal MethodRepresantation(MethodBase method)
        {
            Name = method.Name;
            GenericArguments = ReadMetadata.ReadGenericArguments(method.GetGenericArguments());
            ReturnType = ReadMetadata.ReadReturnType(method);
            Parameters = ReadMetadata.ReadParameters(method.GetParameters());
            Modifiers = ReadMetadata.ReadModifiers(method);
            Extension = ReadMetadata.ReadExtension(method);
        }
        #endregion

        public string Print()
        {
            throw new NotImplementedException();
        }
    }
}
