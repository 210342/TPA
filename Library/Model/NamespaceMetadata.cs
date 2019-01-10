using System;
using System.Collections.Generic;
using System.Linq;
using ModelContract;

namespace Library.Model
{
    public class NamespaceMetadata : AbstractMapper, INamespaceMetadata
    {
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
            foreach (ITypeMetadata child in namespaceMetadata.Types)
                if (AlreadyMapped.TryGetValue(child.SavedHash, out IMetadata item))
                {
                    types.Add(item as ITypeMetadata);
                }
                else
                {
                    ITypeMetadata newType = new TypeMetadata(child);
                    types.Add(newType);
                    AlreadyMapped.Add(newType.SavedHash, newType);
                }

            Types = types;
        }

        public IEnumerable<ITypeMetadata> Types { get; }
        public IEnumerable<IMetadata> Children => Types;
        public string Name { get; }
        public int SavedHash { get; }

        public override int GetHashCode()
        {
            return SavedHash;
        }

        public override bool Equals(object obj)
        {
            if (GetType() != obj.GetType())
                return false;
            NamespaceMetadata nm = (NamespaceMetadata) obj;
            if (Name == nm.Name)
                return true;
            return false;
        }

        public override string ToString()
        {
            return Name;
        }
    }
}