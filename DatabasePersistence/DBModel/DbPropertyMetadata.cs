using ModelContract;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace DatabasePersistence.DBModel
{
    public class DbPropertyMetadata : AbstractMapper, IPropertyMetadata
    {
        public ITypeMetadata MyType { get; private set; }
        public string Name { get; private set; }
        public int SavedHash { get; private set; }
        public IEnumerable<IMetadata> Children
        {
            get
            {
                return new[] { MyType };
            }
        }

        public DbPropertyMetadata(IPropertyMetadata propertyMetadata)
        {
            Name = propertyMetadata.Name;
            SavedHash = propertyMetadata.SavedHash;
            if (AlreadyMapped.TryGetValue(propertyMetadata.MyType.SavedHash, out IMetadata item))
            {
                MyType = item as ITypeMetadata;
            }
            else
            {
                ITypeMetadata newType = new DbTypeMetadata(propertyMetadata.MyType);
                MyType = newType;
                AlreadyMapped.Add(newType.SavedHash, newType);
            }
        }

        public DbPropertyMetadata()
        {
        }
    }
}
