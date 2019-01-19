using ModelContract;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dbp.Model
{
    public class DbNamespaceMetadata : AbstractMapper
    {
        public string Name { get; set; }
        public int SavedHash { get; set; }
        public ICollection<DbTypeMetadata> Types { get; set; }

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
