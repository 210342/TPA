using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Library.Data.Model
{
    internal class AttributeMetadata : IMetadata
    {
        internal AttributeMetadata(Attribute attribute)
        {
            if (attribute == null)
                throw new ArgumentNullException("Attribute can't be null.");
            m_Name = attribute.GetType().Name;
            savedHash = attribute.GetHashCode();
        }
        private string m_Name;
        public string Name => m_Name;

        public IEnumerable<IMetadata> Children => null;
        private int savedHash;
        public override int GetHashCode()
        {
            return savedHash;
        }
        public override bool Equals(object obj)
        {
            if (this.GetType() != obj.GetType())
                return false;
            if (this.m_Name == ((AttributeMetadata)obj).m_Name)
                return true;
            else
                return false;
        }
        public override string ToString()
        {
            return "[" + m_Name + "]";
        }
    }
}
