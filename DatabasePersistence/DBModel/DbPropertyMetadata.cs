using ModelContract;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DatabasePersistence.DBModel
{
    public class DbPropertyMetadata : AbstractMapper, IPropertyMetadata
    {
        #region IPropertyMetadata
        public string Name { get; set; }
        public int SavedHash { get; set; }
        [NotMapped]
        public ITypeMetadata MyType => EFMyType;
        [NotMapped]
        public IEnumerable<IMetadata> Children => throw new NotImplementedException();
        #endregion

        #region EF

        public DbTypeMetadata EFMyType { get; set; }

        #endregion

        public DbPropertyMetadata() { }

        public DbPropertyMetadata(IPropertyMetadata propertyMetadata)
        {
            Name = propertyMetadata.Name;
            SavedHash = propertyMetadata.SavedHash;

            if (AlreadyMappedTypes.TryGetValue(
                propertyMetadata.MyType.SavedHash, out DbTypeMetadata item))
            {
                EFMyType = item;
            }
            else
            {
                EFMyType = new DbTypeMetadata(propertyMetadata.MyType.SavedHash, 
                    propertyMetadata.MyType.Name);
            }
        }
        public void MapTypes()
        {
            if (AlreadyMappedTypes.TryGetValue(EFMyType.SavedHash, out DbTypeMetadata item))
            {
                EFMyType = item;
            }
            else
            {
                AlreadyMappedTypes.Add(EFMyType.SavedHash, EFMyType);
            }
        }
    }
}
