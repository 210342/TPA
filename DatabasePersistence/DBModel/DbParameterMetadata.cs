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
            if (AlreadyMapped.TryGetValue(parameterMetadata.TypeMetadata.SavedHash, out IMetadata item))
            {
                TypeMetadata = item as ITypeMetadata;
            }
            else
            {
                TypeMetadata = new DbTypeMetadata(
                    parameterMetadata.TypeMetadata.SavedHash, parameterMetadata.TypeMetadata.Name);
            }
        }

        public DbParameterMetadata()
        {
        }

        public virtual ITypeMetadata TypeMetadata { get; internal set; }
        public string Name { get; set; }
        public int SavedHash { get; protected set; }

        [NotMapped]
        public IEnumerable<IMetadata> Children
        {
            get => new[] {TypeMetadata};
            set
            {
                if (value != null)
                    TypeMetadata = (ITypeMetadata) value.First();
                else
                    TypeMetadata = null;
            }
        }

        public void MapTypes()
        {
            if (!TypeMetadata.Mapped && AlreadyMapped.TryGetValue(TypeMetadata.SavedHash, out IMetadata item))
            {
                TypeMetadata = item as ITypeMetadata;
            }
        }
    }
}