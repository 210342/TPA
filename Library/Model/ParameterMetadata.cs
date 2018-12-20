using System;
using System.Collections.Generic;
using System.Runtime.Serialization;

namespace Library.Model
{
    [DataContract(Name = "Parameter")]
    [Serializable]
    public class ParameterMetadata : IMetadata
    {
        public ParameterMetadata(string name, TypeMetadata typeMetadata)
        {
            if (name == null || typeMetadata == null)
                throw new ArgumentNullException("Neither name or TypeMetadata can be null.");
            this.m_Name = name;
            this.m_TypeMetadata = typeMetadata;
            savedHash = 17;
            savedHash *= 31 + m_Name.GetHashCode();
            savedHash *= 31 + m_TypeMetadata.GetHashCode();
        }
        internal ParameterMetadata()
        {
        }
        //private vars
        [DataMember(Name = "Name")]
        private string m_Name;
        [DataMember(Name = "Type")]
        private TypeMetadata m_TypeMetadata;

        public TypeMetadata Type { get { return m_TypeMetadata; } }
        public string Name => m_Name;

        //[DataMember(Name = "Children")]
        public IEnumerable<IMetadata> Children {

            get
            {
                return new[] { m_TypeMetadata };
            }
            set
            {
                foreach(var elem in value)
                {
                    this.m_TypeMetadata = (TypeMetadata)elem;
                    break;
                }
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