using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;

namespace Library.Model
{
    [DataContract(Name = "Namespace")]
    [Serializable]
    public class NamespaceMetadata : IMetadata
    {
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