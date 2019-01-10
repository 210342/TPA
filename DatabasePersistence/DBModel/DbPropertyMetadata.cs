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
            if (AlreadyMapped.TryGetValue(propertyMetadata.MyType.SavedHash, out var item))
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

        public virtual ITypeMetadata MyType { get; internal set; }
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
    }
}