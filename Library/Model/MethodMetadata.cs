using Library.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Runtime.Serialization;
using System.Text;

namespace Library.Model
{
    [DataContract(Name = "Method")]
    [Serializable]
    public class MethodMetadata : IMetadata
    {
        internal static IEnumerable<MethodMetadata> EmitMethods(IEnumerable<MethodBase> methods)
        {
            return from MethodBase _currentMethod in methods
                   where _currentMethod.GetVisible()
                   select new MethodMetadata(_currentMethod);
        }

        #region private
        //vars
        [DataMember(Name = "Name")]
        private string m_Name;
        [DataMember(Name = "GenericArguments")]
        private IEnumerable<TypeMetadata> m_GenericArguments;
        [DataMember(Name = "Modifiers")]
        private Tuple<AccessLevel, AbstractENum, StaticEnum, VirtualEnum> m_Modifiers;
        [DataMember(Name = "ReturnType")]
        private TypeMetadata m_ReturnType;
        [DataMember(Name = "IsExtensionMethod")]
        private bool m_Extension;
        [DataMember(Name = "Parameters")]
        private IEnumerable<ParameterMetadata> m_Parameters;

        #region properties

        public string Name => m_Name;
        public IEnumerable<TypeMetadata> GenericArguments => m_GenericArguments;
        public TypeMetadata ReturnType => m_ReturnType;
        public bool IsExtension => m_Extension;
        public IEnumerable<ParameterMetadata> Parameters => m_Parameters;
        public IEnumerable<IMetadata> Children { get; set; }
        #endregion
        //constructor
        private MethodMetadata(MethodBase method)
        {
            m_Name = method.Name;
            m_GenericArguments = !method.IsGenericMethodDefinition ? null : TypeMetadata.EmitGenericArguments(method.GetGenericArguments());
            m_ReturnType = EmitReturnType(method);
            m_Parameters = EmitParameters(method.GetParameters());
            m_Modifiers = EmitModifiers(method);
            m_Extension = EmitExtension(method);
            FillChildren(new StreamingContext { });
            savedHash = method.GetHashCode();
        }
        internal MethodMetadata() { }

        [OnDeserialized]
        private void FillChildren(StreamingContext context)
        {
            List<IMetadata> elems = new List<IMetadata>();
            elems.Add(m_ReturnType);
            elems.AddRange(m_Parameters);
            Children = elems;
        }

        //methods
        private static IEnumerable<ParameterMetadata> EmitParameters(IEnumerable<ParameterInfo> parms)
        {
            return from parm in parms
                   select new ParameterMetadata(parm.Name, TypeMetadata.EmitReference(parm.ParameterType));
        }
        private static TypeMetadata EmitReturnType(MethodBase method)
        {
            MethodInfo methodInfo = method as MethodInfo;
            if (methodInfo == null)
                return null;
            return TypeMetadata.EmitReference(methodInfo.ReturnType);
        }
        private static bool EmitExtension(MethodBase method)
        {
            return method.IsDefined(typeof(ExtensionAttribute), true);
        }
        private static Tuple<AccessLevel, AbstractENum, StaticEnum, VirtualEnum> EmitModifiers(MethodBase method)
        {
            AccessLevel _access = AccessLevel.IsPrivate;
            if (method.IsPublic)
                _access = AccessLevel.IsPublic;
            else if (method.IsFamily)
                _access = AccessLevel.IsProtected;
            else if (method.IsFamilyAndAssembly)
                _access = AccessLevel.IsProtectedInternal;
            AbstractENum _abstract = AbstractENum.NotAbstract;
            if (method.IsAbstract)
                _abstract = AbstractENum.Abstract;
            StaticEnum _static = StaticEnum.NotStatic;
            if (method.IsStatic)
                _static = StaticEnum.Static;
            VirtualEnum _virtual = VirtualEnum.NotVirtual;
            if (method.IsVirtual)
                _virtual = VirtualEnum.Virtual;
            return new Tuple<AccessLevel, AbstractENum, StaticEnum, VirtualEnum>(_access, _abstract, _static, _virtual);
        }
        #endregion
        [DataMember(Name = "Hash")]
        private int savedHash;
        public override int GetHashCode()
        {
            return savedHash;
        }
        public override bool Equals(object obj)
        {
            if (this.GetType() != obj.GetType())
                return false;
            MethodMetadata mm = ((MethodMetadata)obj);
            if (this.m_Name == mm.m_Name)
            {
                if (m_ReturnType != mm.m_ReturnType)
                    return false;
                int counter = 0;
                foreach (var el in this.m_Parameters)
                    if (mm.m_Parameters.Any(n => n.Equals(el)))
                        ++counter;
                if (counter != this.m_Parameters.Count())
                    return false;
                return true;
            }
            else
                return false;
        }
        public override string ToString()
        {
            StringBuilder paramsString = new StringBuilder("(");
            if (m_Parameters.Count() != 0)
            {

                foreach (ParameterMetadata parameter in m_Parameters)
                {
                    paramsString.Append($"{parameter.Type.Name} {parameter.Name}, ");
                }
                paramsString.Remove(paramsString.Length - 2, 2); // remove last comma and space
                paramsString.Append(")");
            }
            else
            {
                paramsString.Append(")");
            }
            return $"{(m_ReturnType != null ? "" + m_ReturnType.Name : "")} {m_Name}{paramsString.ToString()}";
        }

        public string ModifiersString()
        {
            return (m_Modifiers?.Item1.ToString()) + (m_Modifiers?.Item2.ToString())
                + (m_Modifiers?.Item3.ToString() + m_Modifiers?.Item4.ToString());
        }
    }
}