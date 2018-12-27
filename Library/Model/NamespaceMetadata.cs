using ModelContract;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;

namespace Library.Model
{
    public class NamespaceMetadata : INamespaceMetadata
    {
        public IEnumerable<ITypeMetadata> Types { get; }
        public IEnumerable<IMetadata> Children => Types;
        public string Name { get; }
        public int SavedHash { get; }

        internal NamespaceMetadata(string name, IEnumerable<Type> types)
        {
            Name = name;
            Types = from type in types orderby type.Name select new TypeMetadata(type);
            SavedHash = name.GetHashCode();
        }

        internal NamespaceMetadata()
        {
        }

        public override int GetHashCode()
        {
            return SavedHash;
        }

        public override bool Equals(object obj)
        {
            if (this.GetType() != obj.GetType())
                return false;
            NamespaceMetadata nm = ((NamespaceMetadata)obj);
            if (this.Name == nm.Name)
            {
                return true;
            }
            else
                return false;
        }

        public override string ToString()
        {
            return Name;
        }
    }
}