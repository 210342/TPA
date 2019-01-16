using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using ModelContract;

namespace DatabasePersistence.DBModel
{
    public class DbParameterMetadata : AbstractMapper, IParameterMetadata
    {
        public DbParameterMetadata(IParameterMetadata parameterMetadata)
        {
            Name = parameterMetadata.Name;
            SavedHash = parameterMetadata.SavedHash;
            if (AlreadyMapped.TryGetValue(parameterMetadata.MyType.SavedHash, out IMetadata item))
            {
                MyType = item as ITypeMetadata;
            }
            else
            {
                MyType = new DbTypeMetadata(
                    parameterMetadata.MyType.SavedHash, parameterMetadata.MyType.Name);
            }
        }

        public DbParameterMetadata()
        {
        }

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
            if (!MyType.Mapped && AlreadyMapped.TryGetValue(MyType.SavedHash, out IMetadata item))
            {
                MyType = item as ITypeMetadata;
            }
        }

        public virtual ICollection<DbMethodMetadata> Methods { get; set; }

        #endregion

    }
}