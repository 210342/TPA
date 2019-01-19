using ModelContract;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dbp.Model
{
    public class DbPropertyMetadata : AbstractMapper
    {
        public string Name { get; set; }
        public int SavedHash { get; set; }
        public DbTypeMetadata MyType { get; set; }

        public DbPropertyMetadata(IPropertyMetadata propertyMetadata)
        {
            Name = propertyMetadata.Name;
            SavedHash = propertyMetadata.SavedHash;

            if (AlreadyMappedTypes.TryGetValue(
                propertyMetadata.MyType.SavedHash, out DbTypeMetadata item))
            {
                MyType = item;
            }
            else
            {
                MyType = new DbTypeMetadata(propertyMetadata.MyType.SavedHash, 
                    propertyMetadata.MyType.Name);
            }
        }
        public void MapTypes()
        {
            if (AlreadyMappedTypes.TryGetValue(MyType.SavedHash, out DbTypeMetadata item))
            {
                MyType = item;
            }
            else
            {
                AlreadyMappedTypes.Add(MyType.SavedHash, MyType);
            }
        }
    }
}
