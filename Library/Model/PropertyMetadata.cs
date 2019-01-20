﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using ModelContract;

namespace Library.Model
{
    public class PropertyMetadata : AbstractMapper, IPropertyMetadata
    {
        internal static IEnumerable<PropertyMetadata> EmitProperties(IEnumerable<PropertyInfo> props)
        {
            return from prop in props
                where prop.GetGetMethod().GetVisible() || prop.GetSetMethod().GetVisible()
                select new PropertyMetadata(prop.Name, TypeMetadata.EmitReference(prop.PropertyType));
        }

        public override int GetHashCode()
        {
            return SavedHash;
        }

        public override bool Equals(object obj)
        {
            if (GetType() != obj.GetType())
                return false;
            PropertyMetadata pm = (PropertyMetadata) obj;
            if (Name == pm.Name)
                if (MyType != pm.MyType)
                    return false;
            return false;
        }

        public override string ToString()
        {
            return Name + " : " + MyType.Name + "<Property>";
        }

        #region properties

        public string Name { get; }
        public ITypeMetadata MyType { get; private set; }

        public IEnumerable<IMetadata> Children
        {
            get => new[] {MyType};
            set
            {
                foreach (IMetadata elem in value)
                {
                    MyType = (TypeMetadata) elem;
                    break;
                }
            }
        }

        public int SavedHash { get; }

        #endregion

        #region constructors

        internal PropertyMetadata(string propertyName, TypeMetadata propertyType)
        {
            if (propertyName == null || propertyType == null)
                throw new ArgumentNullException("Neither propertyName or MyType can be null.");
            Name = propertyName;
            MyType = propertyType;
            SavedHash = 23;
            SavedHash *= 31 + Name.GetHashCode();
            SavedHash *= 31 + MyType.GetHashCode();
        }

        internal PropertyMetadata()
        {
        }

        public PropertyMetadata(IPropertyMetadata propertyMetadata)
        {
            Name = propertyMetadata.Name;
            SavedHash = propertyMetadata.SavedHash;
            if (AlreadyMapped.TryGetValue(propertyMetadata.MyType.SavedHash, out IMetadata item))
            {
                MyType = item as ITypeMetadata;
            }
            else
            {
                MyType = new TypeMetadata(propertyMetadata.MyType.SavedHash, propertyMetadata.MyType.Name);
            }
        }

        #endregion

        public void MapTypes()
        {
            if (AlreadyMapped.TryGetValue(MyType.SavedHash, out IMetadata item))
            {
                MyType = item as ITypeMetadata;
            }
            else
            {
                AlreadyMapped.Add(MyType.SavedHash, MyType);
            }
        }

        #region private

        #endregion
    }
}