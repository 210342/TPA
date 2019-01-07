using ModelContract;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DatabasePersistence.DBModel
{
    public abstract class AbstractMapper
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        [NotMapped]
        protected static Dictionary<int, IMetadata> AlreadyMapped { get; } = new Dictionary<int, IMetadata>();
    }
}
