using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using ModelContract;

namespace Library.Model
{
    public class AssemblyMetadata : AbstractMapper, IAssemblyMetadata
    {
        internal AssemblyMetadata(Assembly assembly)
        {
            if (assembly == null)
                throw new ArgumentNullException("Assembly can't be null");
            Name = assembly.ManifestModule.Name;
            Namespaces = from Type _type in assembly.GetTypes()
                where _type.GetVisible()
                group _type by _type.GetNamespace()
                into _group
                orderby _group.Key
                select new NamespaceMetadata(_group.Key, _group);
            SavedHash = assembly.GetHashCode();
        }

        public AssemblyMetadata(IAssemblyMetadata assemblyMetadata)
        {
            Name = assemblyMetadata.Name;
            SavedHash = assemblyMetadata.SavedHash;
            var namespaces = new List<INamespaceMetadata>();
            if (assemblyMetadata.Namespaces != null)
            {
                foreach (var child in assemblyMetadata.Namespaces)
                    if (AlreadyMapped.TryGetValue(child.SavedHash, out var item))
                    {
                        namespaces.Add(item as INamespaceMetadata);
                    }
                    else
                    {
                        INamespaceMetadata newNamespace = new NamespaceMetadata(child);
                        namespaces.Add(newNamespace);
                        AlreadyMapped.Add(newNamespace.SavedHash, newNamespace);
                    }

                Namespaces = namespaces;
            }
        }

        public int SavedHash { get; }
        public string Name { get; }
        public IEnumerable<INamespaceMetadata> Namespaces { get; }

        public IEnumerable<IMetadata> Children => Namespaces;

        public override int GetHashCode()
        {
            return SavedHash;
        }

        public override bool Equals(object obj)
        {
            if (GetType() != obj.GetType())
                return false;
            if (Name == ((AssemblyMetadata) obj).Name)
                return true;
            return false;
        }

        public override string ToString()
        {
            return Name;
        }
    }
}