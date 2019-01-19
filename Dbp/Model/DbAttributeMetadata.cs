using ModelContract;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dbp.Model
{
    public class DbAttributeMetadata : AbstractMapper
    {
        public string Name { get; set; }
        public int SavedHash { get; set; }

        public DbAttributeMetadata(IAttributeMetadata attributeMetadata)
        {
            Name = attributeMetadata.Name;
            SavedHash = attributeMetadata.SavedHash;
        }
    }
}
