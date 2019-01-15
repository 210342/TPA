using System.Data.Entity;
using DatabasePersistence.DBModel;

namespace DatabasePersistence
{
    public class DbModelAccessContext : DbContext
    {
        public DbModelAccessContext() : this("name=DbSource") { }

        public DbModelAccessContext(string connectionString) : base(connectionString) { }

        public virtual DbSet<DbAssemblyMetadata> Assemblies { get; set; }
        public virtual DbSet<DbAttributeMetadata> Attributes { get; set; }
        public virtual DbSet<DbMethodMetadata> Methods { get; set; }
        public virtual DbSet<DbNamespaceMetadata> Namespaces { get; set; }
        public virtual DbSet<DbParameterMetadata> Parameters { get; set; }
        public virtual DbSet<DbPropertyMetadata> Properties { get; set; }
        public virtual DbSet<DbTypeMetadata> Types { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Entity<DbAssemblyMetadata>().Map(m =>
            {
                m.MapInheritedProperties();
                m.ToTable("Assemblies");
            });

            modelBuilder.Entity<DbAttributeMetadata>().Map(m =>
            {
                m.MapInheritedProperties();
                m.ToTable("Attributes");
            });

            modelBuilder.Entity<DbMethodMetadata>().Map(m =>
            {
                m.MapInheritedProperties();
                m.ToTable("Methods");
            });

            modelBuilder.Entity<DbNamespaceMetadata>().Map(m =>
            {
                m.MapInheritedProperties();
                m.ToTable("Namespaces");
            });

            modelBuilder.Entity<DbParameterMetadata>().Map(m =>
            {
                m.MapInheritedProperties();
                m.ToTable("Parameters");
            });

            modelBuilder.Entity<DbPropertyMetadata>().Map(m =>
            {
                m.MapInheritedProperties();
                m.ToTable("Properties");
            });

            modelBuilder.Entity<DbTypeMetadata>().Map(m =>
            {
                m.MapInheritedProperties();
                m.ToTable("Types");
            });
        }
    }
}