using System.Collections.Generic;
using System.Data.Entity;
using DatabasePersistence.DBModel;
using ModelContract;

namespace DatabasePersistence
{
    public class DbModelAccessContext : DbContext
    {
        public DbModelAccessContext() : this("name=DbSource") { }

        public DbModelAccessContext(string connectionString) : base(connectionString)
        {
            //Configuration.LazyLoadingEnabled = false;
        }

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
            modelBuilder.Entity<DbMethodMetadata>()
                .HasOptional(m => m.DbReturnType)
                .WithOptionalPrincipal()
                .Map(x => x.MapKey("ReturnTypeId"));

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
            modelBuilder.Entity<DbParameterMetadata>()
                .HasOptional(m => m.DbMyType)
                .WithOptionalPrincipal()
                .Map(x => x.MapKey("ParameterTypeId"));

            modelBuilder.Entity<DbPropertyMetadata>()
                .Map(m =>
                {
                    m.MapInheritedProperties();
                    m.ToTable("Properties");
                });
            modelBuilder.Entity<DbPropertyMetadata>()
                .HasOptional(p => p.DbMyType)
                .WithOptionalPrincipal()
                .Map(m => m.MapKey("PropertyTypeId"));

            modelBuilder.Entity<DbTypeMetadata>().Map(m =>
            {
                m.MapInheritedProperties();
                m.ToTable("Types");
            });
        }
    }
}