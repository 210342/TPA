using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;

namespace Library.Data.Model
{
    [DataContract(Name = "Namespace")]
    [Serializable]
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
        internal NamespaceMetadata()
        {
        }
        private string m_NamespaceName;
        [DataMember(Name = "Types")]
        private IEnumerable<TypeMetadata> m_Types;
        //public IEnumerable<IMetadata> Children { get; set; }
        public IEnumerable<IMetadata> Children => m_Types;
        [DataMember(Name = "Name")]
        public string Name
        {
            get
            {
                return m_NamespaceName;
            }
            protected set
            {
                this.m_NamespaceName = value;
            }
        }

        

        //public string Name => m_NamespaceName;
        [DataMember(Name = "SavedHash")]
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