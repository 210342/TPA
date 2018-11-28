using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Runtime.Serialization;

namespace Library.Data.Model
{
    [DataContract(Name = "Assembly")]
    [Serializable]
    public class AssemblyMetadata : IMetadata
    {
        internal AssemblyMetadata()
        {
            // for view model initialization (actually more for serializers)
        }

        internal AssemblyMetadata(Assembly assembly)
        {
            if (assembly == null)
                throw new ArgumentNullException("Assembly can't be null");
            m_Name = assembly.ManifestModule.Name;
            m_Namespaces = from Type _type in assembly.GetTypes()
                           where _type.GetVisible()
                           group _type by _type.GetNamespace() into _group
                           orderby _group.Key
                           select new NamespaceMetadata(_group.Key, _group);
            savedHash = assembly.GetHashCode();
        }

        private string m_Name;
        [DataMember(Name = "m_namespaces")]
        private IEnumerable<NamespaceMetadata> m_Namespaces;

        [DataMember(Name = "Name")]
        public string Name
        {
            get
            {
                return m_Name;
            }
            protected set
            {
                this.m_Name = value;
            }
        }
        //public string Name => m_Name;
        public string Details {
            get
            {
                return $"Assembly name: {m_Name}, has {m_Namespaces.Count()} namespaces."; 
            }
        }
        //[DataMember(Name = "Children")]
        public IEnumerable<IMetadata> Children
        {
            get
            {
                return m_Namespaces;
            }
            set
            {
                this.m_Namespaces = (IEnumerable < NamespaceMetadata > )value;
            }
        }
        //public IEnumerable<IMetadata> Children => m_Namespaces;
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
            if (this.m_Name == ((AssemblyMetadata)obj).m_Name)
                return true;
            else
                return false;
        }

        public override string ToString()
        {
            return Name;
        }
    }

}