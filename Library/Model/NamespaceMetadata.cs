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

        public NamespaceMetadata(INamespaceMetadata namespaceMetadata)
        {
            Name = namespaceMetadata.Name;
            SavedHash = namespaceMetadata.SavedHash;

            List<ITypeMetadata> types = new List<ITypeMetadata>();
            foreach(ITypeMetadata child in namespaceMetadata.Types)
            {
                if (MappingDictionary.AlreadyMapped.TryGetValue(child.SavedHash, out IMetadata item))
                {
                    types.Add(item as ITypeMetadata);
                }
                else
                {
                    ITypeMetadata newType = new TypeMetadata(child);
                    types.Add(newType);
                    MappingDictionary.AlreadyMapped.Add(newType.SavedHash, newType);
                }
            }
            Types = types;
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