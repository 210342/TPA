using System.Collections.Generic;
using System.Reflection;
using System.Linq;
using System;
using System.Runtime.Serialization;
using ModelContract;

namespace Library.Model
{
    public class PropertyMetadata : AbstractMapper, IPropertyMetadata
    {
        #region properties
        public string Name { get; }
        public ITypeMetadata MyType { get; private set; }
        public IEnumerable<IMetadata> Children
        {

            get
            {
                return new[] { MyType };
            }
            set
            {
                foreach (var elem in value)
                {
                    MyType = (TypeMetadata)elem;
                    break;
                }
            }
        }
        public int SavedHash { get; }

        #endregion

        #region constructors

        private PropertyMetadata(string propertyName, TypeMetadata propertyType)
        {
            if (propertyName == null || propertyType == null)
                throw new ArgumentNullException("Neither propertyName or TypeMetadata can be null.");
            Name = propertyName;
            MyType = propertyType;
            SavedHash = 23;
            SavedHash *= 31 + Name.GetHashCode();
            SavedHash *= 31 + MyType.GetHashCode();
        }
        internal PropertyMetadata() { }

        public PropertyMetadata(IPropertyMetadata propertyMetadata)
        {
            Name = propertyMetadata.Name;
            SavedHash = propertyMetadata.SavedHash;
            if(AlreadyMapped.TryGetValue(propertyMetadata.MyType.SavedHash, out IMetadata item))
            {
                MyType = item as ITypeMetadata;
            }
            else
            {
                ITypeMetadata newType = new TypeMetadata(propertyMetadata.MyType);
                MyType = newType;
                AlreadyMapped.Add(newType.SavedHash, newType);
            }
        }

        #endregion

        internal static IEnumerable<PropertyMetadata> EmitProperties(IEnumerable<PropertyInfo> props)
        {
            return from prop in props
                   where prop.GetGetMethod().GetVisible() || prop.GetSetMethod().GetVisible()
                   select new PropertyMetadata(prop.Name, TypeMetadata.EmitReference(prop.PropertyType));
        }

        #region private


        #endregion
        public override int GetHashCode()
        {
            return SavedHash;
        }
        public override bool Equals(object obj)
        {
            if (this.GetType() != obj.GetType())
                return false;
            PropertyMetadata pm = ((PropertyMetadata)obj);
            if (this.Name == pm.Name)
            {
                if (MyType != pm.MyType)
                    return false;
            }
            return false;
        }
        public override string ToString()
        {
            return Name + " : " + MyType.Name + "<Property>";
        }
    }
}