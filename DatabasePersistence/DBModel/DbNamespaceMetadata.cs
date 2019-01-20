using ModelContract;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DatabasePersistence.DBModel
{
    public class DbNamespaceMetadata : AbstractMapper, INamespaceMetadata
    {
        #region INamespaceMetadata

        public string Name { get; set; }
        public int SavedHash { get; set; }
        public IEnumerable<ITypeMetadata> Types
        {
            get => EFTypes;
            set => EFTypes = value?.Cast<DbTypeMetadata>().ToList();
        }
        [NotMapped]
        public IEnumerable<IMetadata> Children => throw new NotImplementedException();

        #endregion

        #region EF

        public ICollection<DbTypeMetadata> EFTypes { get; set; }

        #endregion

        public DbNamespaceMetadata() { }

        public DbNamespaceMetadata(INamespaceMetadata namespaceMetadata)
        {
            Name = namespaceMetadata.Name;
            SavedHash = namespaceMetadata.SavedHash;
            List<DbTypeMetadata> types = new List<DbTypeMetadata>();
            foreach (ITypeMetadata child in namespaceMetadata.Types)
                if (AlreadyMappedTypes.TryGetValue(child.SavedHash, out DbTypeMetadata item))
                {
                    types.Add(item);
                }
                else
                {
                    DbTypeMetadata newType = new DbTypeMetadata(child);
                    types.Add(newType);
                    AlreadyMappedTypes.Add(newType.SavedHash, newType);
                }

            Types = types;
        }
    }
}
