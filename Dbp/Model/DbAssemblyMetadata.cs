using ModelContract;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dbp.Model
{
    public class DbAssemblyMetadata : AbstractMapper
    {
        public string Name { get; set; }
        public int SavedHash { get; set; }
        public ICollection<DbNamespaceMetadata> Namespaces { get; set; }

        public DbAssemblyMetadata(IAssemblyMetadata assemblyMetadata)
        {
            Name = assemblyMetadata.Name;
            SavedHash = assemblyMetadata.SavedHash;
            List<DbNamespaceMetadata> namespaces = new List<DbNamespaceMetadata>();
            foreach (INamespaceMetadata child in assemblyMetadata.Namespaces)
                if (AlreadyMappedNamespaces.TryGetValue(child.SavedHash, out DbNamespaceMetadata item))
                {
                    namespaces.Add(item);
                }
                else
                {
                    DbNamespaceMetadata newNamespace = new DbNamespaceMetadata(child);
                    namespaces.Add(newNamespace);
                    AlreadyMappedNamespaces.Add(newNamespace.SavedHash, newNamespace);
                }

            Namespaces = namespaces;
            foreach (DbNamespaceMetadata _namespace in Namespaces)
            {
                foreach (DbTypeMetadata type in _namespace.Types)
                {
                    type.MapTypes();
                }
            }
        }
    }
}
