using DatabasePersistence.DBModel;
using System.Data.Entity;


namespace DatabasePersistence
{
    public class DbModelAccesContext : DbContext
    {
        public DbModelAccesContext()
            : base("name=DbSource")
        {
        }
        public DbModelAccesContext(string connectionString) : base(connectionString)
        {
        }

        public virtual DbSet<AbstractMapper> Model { get; set; }
    }
}