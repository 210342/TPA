using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using ModelContract;

namespace DatabasePersistence.DBModel
{
    public abstract class AbstractMapper
    {
        [NotMapped]
        protected static Dictionary<int, DbNamespaceMetadata> AlreadyMappedNamespaces { get; }
               = new Dictionary<int, DbNamespaceMetadata>();
        [NotMapped]
        protected static Dictionary<int, DbTypeMetadata> AlreadyMappedTypes { get; }
            = new Dictionary<int, DbTypeMetadata>();
        [NotMapped]
        protected static Dictionary<int, DbAttributeMetadata> AlreadyMappedAttributes { get; }
            = new Dictionary<int, DbAttributeMetadata>();
        [NotMapped]
        protected static Dictionary<int, DbPropertyMetadata> AlreadyMappedProperties { get; }
            = new Dictionary<int, DbPropertyMetadata>();
        [NotMapped]
        protected static Dictionary<int, DbMethodMetadata> AlreadyMappedMethods { get; }
            = new Dictionary<int, DbMethodMetadata>();
        [NotMapped]
        protected static Dictionary<int, DbParameterMetadata> AlreadyMappedParameters { get; }
            = new Dictionary<int, DbParameterMetadata>();
    }
}