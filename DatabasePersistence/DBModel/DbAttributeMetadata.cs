using ModelContract;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace DatabasePersistence.DBModel
{
    public class DbAttributeMetadata : AbstractMapper, IAttributeMetadata
    {
        public string Name { get; private set; }
        public int SavedHash { get; private set; }
        public IEnumerable<IMetadata> Children => null;

        public DbAttributeMetadata(IAttributeMetadata attributeMetadata)
        {
            Name = attributeMetadata.Name;
            SavedHash = attributeMetadata.SavedHash;
        }

        public DbAttributeMetadata() { }
    }
}
