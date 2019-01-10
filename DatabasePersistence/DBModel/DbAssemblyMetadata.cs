using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using ModelContract;

namespace DatabasePersistence.DBModel
{
    public class DbAssemblyMetadata : AbstractMapper, IAssemblyMetadata
    {
        //public List<AbstractMapper> Parents { get; set; }

        public DbAssemblyMetadata(IAssemblyMetadata assemblyMetadata)
        {
            Name = assemblyMetadata.Name;
            SavedHash = assemblyMetadata.SavedHash;
            List<INamespaceMetadata> namespaces = new List<INamespaceMetadata>();
            foreach (INamespaceMetadata child in assemblyMetadata.Namespaces)
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

            Namespaces = namespaces;
            foreach (INamespaceMetadata _namespace in Namespaces)
            {
                foreach (ITypeMetadata type in _namespace.Types)
                {
                    type.MapTypes();
                }
            }
        }

        public DbAssemblyMetadata()
        {
        }

        public virtual ICollection<DbNamespaceMetadata> NamespacesList { get; set; }

        public string Name { get; set; }
        public int SavedHash { get; protected set; }

        [NotMapped]
        public IEnumerable<INamespaceMetadata> Namespaces
        {
            get => NamespacesList;
            internal set => NamespacesList = value?.Cast<DbNamespaceMetadata>().ToList();
        }

        [NotMapped] public IEnumerable<IMetadata> Children => Namespaces;
    }
}