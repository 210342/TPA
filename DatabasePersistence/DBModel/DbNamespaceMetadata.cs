using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using ModelContract;

namespace DatabasePersistence.DBModel
{
    public class DbNamespaceMetadata : AbstractMapper, INamespaceMetadata
    {
        public DbNamespaceMetadata(INamespaceMetadata namespaceMetadata)
        {
            Name = namespaceMetadata.Name;
            SavedHash = namespaceMetadata.SavedHash;
            var types = new List<ITypeMetadata>();
            foreach (var child in namespaceMetadata.Types)
                if (AlreadyMapped.TryGetValue(child.SavedHash, out var item))
                {
                    types.Add(item as ITypeMetadata);
                }
                else
                {
                    ITypeMetadata newType = new DbTypeMetadata(child);
                    types.Add(newType);
                    AlreadyMapped.Add(newType.SavedHash, newType);
                }

            Types = types;
        }

        public DbNamespaceMetadata()
        {
        }

        #region EF

        public virtual ICollection<DbTypeMetadata> TypesList { get; set; }

        #endregion

        #region INamespaceMetadata

        public string Name { get; set; }
        public int SavedHash { get; protected set; }

        [NotMapped] public IEnumerable<IMetadata> Children => Types;

        [NotMapped]
        public IEnumerable<ITypeMetadata> Types
        {
            get => TypesList;
            internal set => TypesList = value?.Cast<DbTypeMetadata>().ToList();
        }

        #endregion
    }
}