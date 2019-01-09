using ModelContract;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace DatabasePersistence.DBModel
{
    public class DbNamespaceMetadata : AbstractMapper, INamespaceMetadata
    {
        #region EF
        public virtual ICollection<DbTypeMetadata> TypesList { get; set; }
        #endregion

        #region INamespaceMetadata
        public string Name { get; set; }
        public int SavedHash { get; protected set; }
        [NotMapped]
        public IEnumerable<IMetadata> Children { get { return Types; } }
        [NotMapped]
        public IEnumerable<ITypeMetadata> Types
        {
            get => TypesList;
            internal set => TypesList = value?.Cast<DbTypeMetadata>().ToList();
        }

        #endregion

        public DbNamespaceMetadata(INamespaceMetadata namespaceMetadata)
        {
            Name = namespaceMetadata.Name;
            SavedHash = namespaceMetadata.SavedHash;
            List<ITypeMetadata> types = new List<ITypeMetadata>();
            foreach (ITypeMetadata child in namespaceMetadata.Types)
            {
                if (AlreadyMapped.TryGetValue(child.SavedHash, out IMetadata item))
                {
                    types.Add(item as ITypeMetadata);
                }
                else
                {
                    ITypeMetadata newType = new DbTypeMetadata(child);
                    types.Add(newType);
                    AlreadyMapped.Add(newType.SavedHash, newType);
                }
            }
            Types = types;
        }

        public DbNamespaceMetadata()
        {
        }
    }
}
