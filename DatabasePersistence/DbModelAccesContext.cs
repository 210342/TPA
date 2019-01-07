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
            modelBuilder.Entity<DbNamespaceMetadata>()
                .HasRequired<DbAssemblyMetadata>(s => s.DbAssemblyMetadata)
                .WithMany(g => g.NamespacesList)
                .HasForeignKey<int>(s => s.DbAssemblyMetadataId);

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