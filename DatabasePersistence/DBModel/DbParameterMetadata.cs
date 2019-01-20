using ModelContract;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DatabasePersistence.DBModel
{
    public class DbParameterMetadata : AbstractMapper, IParameterMetadata
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

        public DbParameterMetadata() { }

        public DbParameterMetadata(IParameterMetadata parameterMetadata)
        {
            Name = parameterMetadata.Name;
            SavedHash = parameterMetadata.SavedHash;
            if (AlreadyMappedTypes.TryGetValue(
                parameterMetadata.MyType.SavedHash, out DbTypeMetadata item))
            {
                EFMyType = item;
            }
            else
            {
                EFMyType = new DbTypeMetadata(
                    parameterMetadata.MyType.SavedHash, parameterMetadata.MyType.Name);
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
