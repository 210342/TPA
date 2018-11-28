using System.Collections.Generic;
using System.Reflection;
using System.Linq;
using System;
using System.Runtime.Serialization;

namespace Library.Data.Model
{
    [DataContract(Name = "Property")]
    [Serializable]
    public class PropertyMetadata : IMetadata
    {
        public string Details
        {
            get
            {
                return $"Property: {m_Name} : {m_TypeMetadata.Name}";
            }
        }
        internal static IEnumerable<PropertyMetadata> EmitProperties(IEnumerable<PropertyInfo> props)
        {
            return from prop in props
                   where prop.GetGetMethod().GetVisible() || prop.GetSetMethod().GetVisible()
                   select new PropertyMetadata(prop.Name, TypeMetadata.EmitReference(prop.PropertyType));
        }

        #region private
        [DataMember(Name = "Name")]
        private string m_Name;
        [DataMember(Name = "Type")]
        private TypeMetadata m_TypeMetadata;

        public string Name => m_Name;

        [DataMember(Name = "Children")]
        public IEnumerable<IMetadata> Children
            {
        
            get
            {
                return new[] { m_TypeMetadata
    };
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

        private PropertyMetadata(string propertyName, TypeMetadata propertyType)
        {
            if (propertyName == null || propertyType == null)
                throw new ArgumentNullException("Neither propertyName or TypeMetadata can be null.");
            m_Name = propertyName;
            m_TypeMetadata = propertyType;
            savedHash = 23;
            savedHash *= 31 + m_Name.GetHashCode();
            savedHash *= 31 + m_TypeMetadata.GetHashCode();
        }
        #endregion
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
            PropertyMetadata pm = ((PropertyMetadata)obj);
            if (this.m_Name == pm.m_Name)
            {
                if (m_TypeMetadata != pm.m_TypeMetadata)
                    return false;
            }
            return false;
        }
        public override string ToString()
        {
            return m_Name + " : " + m_TypeMetadata.Name + "<Property>";
        }
    }
}