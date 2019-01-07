using ModelContract;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;

namespace DatabasePersistence.DBModel
{
    public class DbPropertyMetadata : AbstractMapper, IPropertyMetadata
    {
        public ITypeMetadata MyType { get; private set; }
        public string Name { get; set; }
        public int SavedHash { get; protected set; }
        [NotMapped]
        public IEnumerable<IMetadata> Children
        {
            get
            {
                return new[] { MyType };
            }
            set
            {
                if (value != null)
                    this.MyType = (ITypeMetadata)value.First();
                else
                    this.MyType = null;
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
