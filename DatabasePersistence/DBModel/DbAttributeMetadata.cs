using ModelContract;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DatabasePersistence.DBModel
{
    public class DbAttributeMetadata : AbstractMapper, IAttributeMetadata
    {
        public string Name { get; set; }
        public int SavedHash { get; set; }
        [NotMapped]
        public IEnumerable<IMetadata> Children => null;

        public DbAttributeMetadata() { }

        public DbAttributeMetadata(IAttributeMetadata attributeMetadata)
        {
            Name = attributeMetadata.Name;
            SavedHash = attributeMetadata.SavedHash;
        }
    }
}
