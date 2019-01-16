using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using ModelContract;

namespace DatabasePersistence.DBModel
{
    public class DbPropertyMetadata : AbstractMapper, IPropertyMetadata
    {
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
                MyType = new DbTypeMetadata(propertyMetadata.MyType.SavedHash, propertyMetadata.MyType.Name);
            }
        }

        public DbPropertyMetadata()
        {
        }

        [NotMapped]
        public virtual ITypeMetadata MyType
        {
            get => DbMyType;
            internal set => DbMyType = value as DbTypeMetadata;
        }
        public virtual DbTypeMetadata DbMyType { get; set; }
        public string Name { get; set; }
        public int SavedHash { get; protected set; }

        [NotMapped]
        public IEnumerable<IMetadata> Children
        {
            get => new[] {MyType};
            set
            {
                if (value != null)
                    MyType = (ITypeMetadata) value.First();
                else
                    MyType = null;
            }
        }

        #region EF

        public void MapTypes()
        {
            if (MyType.Mapped && AlreadyMapped.TryGetValue(MyType.SavedHash, out IMetadata item))
            {
                MyType = item as ITypeMetadata;
            }
        }

        public virtual ICollection<DbTypeMetadata> Types { get; set; }

        #endregion
    }
}