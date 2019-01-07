using ModelContract;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace DatabasePersistence.DBModel
{
    public class DbAttributeMetadata : AbstractMapper, IAttributeMetadata
    {
        public string Name { get; set; }
        public int SavedHash { get; protected set; }
        public IEnumerable<IMetadata> Children { get => null; set => NullOp(); }

        private void NullOp()
        {
            
        }

        public DbAttributeMetadata(IAttributeMetadata attributeMetadata)
        {
            Name = attributeMetadata.Name;
            SavedHash = attributeMetadata.SavedHash;
        }

        public DbAttributeMetadata() { }
    }
}
