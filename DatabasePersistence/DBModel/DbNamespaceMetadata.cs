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
        [Column]
        public DbAssemblyMetadata DbAssemblyMetadata { get; set; }
        public int DbAssemblyMetadataId { get; set; }
        [NotMapped]
        public IEnumerable<ITypeMetadata> Types { get;  set; }
        public string Name { get; set; }
        public int SavedHash { get; protected set; }
        [NotMapped]
        public IEnumerable<IMetadata> Children { get { return Types; } set { SetType(value); } }

        private void SetType(IEnumerable<IMetadata> list)
        {
            if (list != null)
            {
                var elements = new List<ITypeMetadata>();
                list.ToList().ForEach(n => elements.Add((ITypeMetadata)n));
                this.Types = elements;
            }
            else
                this.Types = null;
        }

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
