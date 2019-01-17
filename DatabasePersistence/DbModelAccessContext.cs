using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.ModelConfiguration.Conventions;
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
            modelBuilder.Conventions.Remove<ManyToManyCascadeDeleteConvention>();
            modelBuilder.Conventions.Remove<OneToManyCascadeDeleteConvention>();

            modelBuilder.Entity<DbAssemblyMetadata>().Map(m =>
            {
                m.MapInheritedProperties();
                m.ToTable("Assemblies");
            });
            modelBuilder.Entity<DbAssemblyMetadata>()
                .HasMany(a => a.NamespacesList)
                .WithOptional().WillCascadeOnDelete(true);

            modelBuilder.Entity<DbAttributeMetadata>().Map(m =>
            {
                m.MapInheritedProperties();
                m.ToTable("Attributes");
            });

            //------------------- METHODS -----------------------
            modelBuilder.Entity<DbMethodMetadata>().Map(m =>
            {
                m.MapInheritedProperties();
                m.ToTable("Methods");
            });
            modelBuilder.Entity<DbMethodMetadata>()
                .HasOptional(m => m.DbReturnType)
                .WithOptionalPrincipal()
                .Map(x => x.MapKey("ReturnTypeId"));
            modelBuilder.Entity<DbMethodMetadata>()
                .HasMany(m => m.GenericArgumentsList)
                .WithOptional()
                .Map(x => x.MapKey("GenericArgumentsMethodId"));
            modelBuilder.Entity<DbMethodMetadata>()
                .HasMany(m => m.ParametersList)
                .WithOptional()
                .Map(x => x.MapKey("ParametersMethodId"));

            //------------------  NAMESPACES  --------------------
            modelBuilder.Entity<DbNamespaceMetadata>().Map(m =>
            {
                m.MapInheritedProperties();
                m.ToTable("Namespaces");
            });
            modelBuilder.Entity<DbNamespaceMetadata>()
                .HasMany(n => n.TypesList)
                .WithOptional()
                .Map(x => x.MapKey("NamespaceId"));

            //------------------  PARAMETERS  --------------------
            modelBuilder.Entity<DbParameterMetadata>().Map(m =>
            {
                m.MapInheritedProperties();
                m.ToTable("Parameters");
            });
            modelBuilder.Entity<DbParameterMetadata>()
                .HasOptional(m => m.DbMyType)
                .WithOptionalPrincipal()
                .Map(x => x.MapKey("ParameterTypeId"));

            //------------------  PROPERTIES  --------------------
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
             
            //---------------------  TYPES  -----------------------
            modelBuilder.Entity<DbTypeMetadata>().Map(m =>
            {
                m.MapInheritedProperties();
                m.ToTable("Types");
            });
            modelBuilder.Entity<DbTypeMetadata>()
                .HasMany(i => i.TypesImplementingMe)
                .WithMany(t => t.ImplementedInterfacesList)
                .Map(it =>
                {
                    it.MapLeftKey("TypeId");
                    it.MapRightKey("InterfaceBaseTypeId");
                    it.ToTable("TypesImplementingTypes");
                });
            modelBuilder.Entity<DbTypeMetadata>()
                .HasMany(t => t.MethodsList)
                .WithMany()
                .Map(mt =>
                {
                    mt.MapLeftKey("TypeDefiningMethodId");
                    mt.MapRightKey("MethodId");
                    mt.ToTable("TypesMethods");
                });
            modelBuilder.Entity<DbTypeMetadata>()
                .HasMany(t => t.ConstructorsList)
                .WithMany()
                .Map(ct =>
                {
                    ct.MapLeftKey("TypeDefiningConstructorId");
                    ct.MapRightKey("ConstructorsId");
                    ct.ToTable("TypesConstructors");
                });
            modelBuilder.Entity<DbTypeMetadata>()
                .HasMany(t => t.PropertiesList)
                .WithOptional()
                .Map(x => x.MapKey("TypeDefiningPropertyId"));
            modelBuilder.Entity<DbTypeMetadata>()
                .HasMany(t => t.NestedTypesList)
                .WithOptional(nt => nt.DbDeclaringType)
                .Map(x => x.MapKey("TypeDefiningTypeId"));
            modelBuilder.Entity<DbTypeMetadata>()
                .HasMany(t => t.AttributesList)
                .WithOptional()
                .Map(x => x.MapKey("TypeAttributedId"));
            modelBuilder.Entity<DbTypeMetadata>()
                .HasMany(t => t.GenericArgumentsList)
                .WithOptional()
                .Map(x => x.MapKey("TypeUsingGenericArgumentsId"));
            modelBuilder.Entity<DbTypeMetadata>()
                .HasOptional(t => t.DbBaseType)
                .WithOptionalPrincipal()
                .Map(x => x.MapKey("BaseTypeId"));
        }
    }
}