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
            if (AlreadyMapped.TryGetValue(parameterMetadata.TypeMetadata.SavedHash, out var item))
            {
                TypeMetadata = item as ITypeMetadata;
            }
            else
            {
                ITypeMetadata newType = new DbTypeMetadata(parameterMetadata.TypeMetadata);
                TypeMetadata = newType;
                AlreadyMapped.Add(newType.SavedHash, newType);
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
    }
}