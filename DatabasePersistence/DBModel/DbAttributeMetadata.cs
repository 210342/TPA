using System.Collections.Generic;
using System.Linq;
using ModelContract;

namespace DatabasePersistence.DBModel
{
    public class DbAttributeMetadata : AbstractMapper, IAttributeMetadata
    {
        public DbAttributeMetadata(IAttributeMetadata attributeMetadata)
        {
            Name = attributeMetadata.Name;
            SavedHash = attributeMetadata.SavedHash;
        }

        public DbAttributeMetadata()
        {
        }

        public string Name { get; set; }
        public int SavedHash { get; protected set; }
        public IEnumerable<IMetadata> Children => Enumerable.Empty<IMetadata>();
    }
}