using ModelContract;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dbp.Model
{
    public class DbParameterMetadata : AbstractMapper
    {
        public string Name { get; set; }
        public int SavedHash { get; set; }
        public DbTypeMetadata MyType { get; set; }

        public DbParameterMetadata(IParameterMetadata parameterMetadata)
        {
            Name = parameterMetadata.Name;
            SavedHash = parameterMetadata.SavedHash;
            if (AlreadyMappedTypes.TryGetValue(
                parameterMetadata.MyType.SavedHash, out DbTypeMetadata item))
            {
                MyType = item;
            }
            else
            {
                MyType = new DbTypeMetadata(
                    parameterMetadata.MyType.SavedHash, parameterMetadata.MyType.Name);
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
