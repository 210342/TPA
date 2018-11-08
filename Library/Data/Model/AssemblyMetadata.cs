
using Library.Data.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace TPA.Reflection.Model
{
    internal class AssemblyMetadata : IMetadata
    {

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
        private IEnumerable<NamespaceMetadata> m_Namespaces;

        public string Name => m_Name;

        public IEnumerable<IMetadata> Children => m_Namespaces;
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
            return m_Name;
        }
    }

}