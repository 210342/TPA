using System;
using System.Collections.Generic;
using System.Linq;

namespace Library.Data.Model
{
    public class NamespaceMetadata : IMetadata
    {
        public string Details
        {
            get
            {
                return $"Namespace {m_NamespaceName}, contains {m_Types.Count()} types";
            }
        }

        internal NamespaceMetadata(string name, IEnumerable<Type> types)
        {
            m_NamespaceName = name;
            m_Types = from type in types orderby type.Name select new TypeMetadata(type);
            savedHash = name.GetHashCode();
        }

        private string m_NamespaceName;
        private IEnumerable<TypeMetadata> m_Types;
        public IEnumerable<IMetadata> Children => m_Types;

        public string Name => m_NamespaceName;
        private int savedHash;
        public override int GetHashCode()
        {
            return savedHash;
        }
        public override bool Equals(object obj)
        {
            if (this.GetType() != obj.GetType())
                return false;
            NamespaceMetadata nm = ((NamespaceMetadata)obj);
            if (this.m_NamespaceName == nm.m_NamespaceName)
            {
                return true;
            }
            else
                return false;
        }
        public override string ToString()
        {
            return m_NamespaceName;
        }
    }
}