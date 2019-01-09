using ModelContract;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace DatabasePersistence.DBModel
{
    public class DbParameterMetadata : AbstractMapper, IParameterMetadata
    {
        public virtual ITypeMetadata TypeMetadata { get; internal set; }
        public string Name { get; set; }
        public int SavedHash { get; protected set; }
        [NotMapped]
        public IEnumerable<IMetadata> Children
        {
            get
            {
                return new[] { TypeMetadata };
            }
            set
            {
                if (value != null)
                    this.TypeMetadata = (ITypeMetadata)value.First();
                else
                    this.TypeMetadata = null;
            }
        }

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
                ITypeMetadata newType = new DbTypeMetadata(parameterMetadata.TypeMetadata);
                TypeMetadata = newType;
                AlreadyMapped.Add(newType.SavedHash, newType);
            }
        }

        public DbParameterMetadata()
        {
        }
    }
}
