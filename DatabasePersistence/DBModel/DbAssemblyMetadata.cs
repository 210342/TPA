using ModelContract;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace DatabasePersistence.DBModel
{
    public class DbAssemblyMetadata : AbstractMapper, IAssemblyMetadata
    {
        public IEnumerable<INamespaceMetadata> Namespaces { get; set; }
        public string Name { get; set; }
        public int SavedHash { get; set; }
        public IEnumerable<IMetadata> Children { get { return Namespaces; } }

        public DbAssemblyMetadata(IAssemblyMetadata assemblyMetadata)
        {
            Name = assemblyMetadata.Name;
            SavedHash = assemblyMetadata.SavedHash;
            List<INamespaceMetadata> namespaces = new List<INamespaceMetadata>();
            foreach (INamespaceMetadata child in assemblyMetadata.Namespaces)
            {
                if (AlreadyMapped.TryGetValue(child.SavedHash, out IMetadata item))
                {
                    namespaces.Add(item as INamespaceMetadata);
                }
                else
                {
                    INamespaceMetadata newNamespace = new DbNamespaceMetadata(child);
                    namespaces.Add(newNamespace);
                    AlreadyMapped.Add(newNamespace.SavedHash, newNamespace);
                }
            }
            Namespaces = namespaces;
        }

        public DbAssemblyMetadata()
        {
        }
    }
}
