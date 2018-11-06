
using System.Collections.Generic;
using Library.Data.Model;

namespace TPA.Reflection.Model
{
    internal class ParameterMetadata : IMetadata
    {

        public ParameterMetadata(string name, TypeMetadata typeMetadata)
        {
            this.m_Name = name;
            this.m_TypeMetadata = typeMetadata;
            savedHash = 17;
            savedHash *= 31 + m_Name.GetHashCode();
            savedHash *= 31 + m_TypeMetadata.GetHashCode();
        }

        //private vars
        private string m_Name;
        private TypeMetadata m_TypeMetadata;

        public string Name => m_Name;

        public IEnumerable<IMetadata> Children => new[] { m_TypeMetadata };
        private int savedHash;
        public override int GetHashCode()
        {
            return savedHash;
        }
        public override bool Equals(object obj)
        {
            if (this.GetType() != obj.GetType())
                return false;
            ParameterMetadata pm = ((ParameterMetadata)obj);
            if (this.m_Name == pm.m_Name)
            {
                if (m_TypeMetadata !=pm.m_TypeMetadata)
                    return false;
            }
            return false;
        }
        public override string ToString()
        {
            return m_Name + ": " + m_TypeMetadata.Name;
        }
    }
}