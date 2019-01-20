using ModelContract;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DatabasePersistence.DBModel
{
    public class DbAssemblyMetadata : AbstractMapper, IAssemblyMetadata
    {
        #region IAssemblyMetadata
        public string Name { get; set; }
        public int SavedHash { get; set; }
        public IEnumerable<INamespaceMetadata> Namespaces
        {
            get => EFNamespaces;
            set => EFNamespaces = value?.Cast<DbNamespaceMetadata>().ToList();
        }
        [NotMapped]
        public IEnumerable<IMetadata> Children => throw new NotImplementedException();

        #endregion

        #region EF

        public ICollection<DbNamespaceMetadata> EFNamespaces { get; set; }

        #endregion

        public DbAssemblyMetadata() { }

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

            EFNamespaces = namespaces;
            foreach (DbNamespaceMetadata _namespace in EFNamespaces)
            {
                foreach (DbTypeMetadata type in _namespace.Types)
                {
                    type.MapTypes();
                }
            }
        }
    }
}
