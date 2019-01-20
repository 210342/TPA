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

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Conventions.Remove<ManyToManyCascadeDeleteConvention>();
            modelBuilder.Conventions.Remove<OneToManyCascadeDeleteConvention>();

            //------------------- ASSEMBLY -----------------------

            modelBuilder.Entity<DbAssemblyMetadata>().HasKey(a => a.SavedHash);
            modelBuilder.Entity<DbAssemblyMetadata>().Map(a =>
            {
                a.MapInheritedProperties();
                a.ToTable("Assemblies");
            });
            modelBuilder.Entity<DbAssemblyMetadata>()
                .HasMany(a => a.EFNamespaces)
                .WithRequired()
                .Map(x => x.MapKey("NamespaceAssemblyId"));

            //------------------  ATTRIBUTES  --------------------

            modelBuilder.Entity<DbAttributeMetadata>().HasKey(a => a.SavedHash);
            modelBuilder.Entity<DbAttributeMetadata>().Map(a =>
            {
                a.MapInheritedProperties();
                a.ToTable("Attributes");
            });

            //-------------------- METHODS -----------------------

            modelBuilder.Entity<DbMethodMetadata>().HasKey(m => m.SavedHash);
            modelBuilder.Entity<DbMethodMetadata>().Map(m =>
            {
                m.MapInheritedProperties();
                m.ToTable("Methods");
            });
            modelBuilder.Entity<DbMethodMetadata>()
                .HasOptional(m => m.EFReturnType)
                .WithMany()
                .Map(x => x.MapKey("MethodReturnTypeId"));
            modelBuilder.Entity<DbMethodMetadata>()
                .HasMany(m => m.EFGenericArguments)
                .WithMany()
                .Map(mg =>
                {
                    mg.MapLeftKey("MethodGenArgId");
                    mg.MapRightKey("GenericArgMethId");
                    mg.ToTable("MethodsGenericArguments");
                });
            modelBuilder.Entity<DbMethodMetadata>()
                .HasMany(m => m.EFParameters)
                .WithMany()
                .Map(mp =>
                {
                    mp.MapLeftKey("MethodParamId");
                    mp.MapRightKey("ParameterMethId");
                    mp.ToTable("MethodsParameters");
                });

            //------------------  NAMESPACES  --------------------

            modelBuilder.Entity<DbNamespaceMetadata>().HasKey(a => a.SavedHash);
            modelBuilder.Entity<DbNamespaceMetadata>().Map(m =>
            {
                m.MapInheritedProperties();
                m.ToTable("Namespaces");
            });
            modelBuilder.Entity<DbNamespaceMetadata>()
                .HasMany(n => n.EFTypes)
                .WithOptional()
                .Map(x => x.MapKey("TypeNamespaceId"));

            //------------------  PARAMETERS  --------------------

            modelBuilder.Entity<DbParameterMetadata>().HasKey(p => p.SavedHash);
            modelBuilder.Entity<DbParameterMetadata>().Map(p =>
            {
                p.MapInheritedProperties();
                p.ToTable("Parameters");
            });
            modelBuilder.Entity<DbParameterMetadata>()
                .HasRequired(p => p.EFMyType)
                .WithMany()
                .Map(x => x.MapKey("ParameterTypeId"));

            //------------------  PROPERTIES  --------------------

            modelBuilder.Entity<DbPropertyMetadata>().HasKey(p => p.SavedHash);
            modelBuilder.Entity<DbPropertyMetadata>().Map(p =>
            {
                p.MapInheritedProperties();
                p.ToTable("Properties");
            });
            modelBuilder.Entity<DbPropertyMetadata>()
                .HasRequired(p => p.EFMyType)
                .WithMany()
                .Map(x => x.MapKey("PropertyTypeId"));

            //---------------------  TYPES  -----------------------

            modelBuilder.Entity<DbTypeMetadata>().HasKey(t => t.SavedHash);
            modelBuilder.Entity<DbTypeMetadata>().Map(t =>
            {
                t.MapInheritedProperties();
                t.ToTable("Types");
            });
            modelBuilder.Entity<DbTypeMetadata>()
                .HasMany(t => t.EFAttributes)
                .WithMany()
                .Map(ta =>
                {
                    ta.MapLeftKey("TypeAttId");
                    ta.MapRightKey("AttributeTypId");
                    ta.ToTable("TypesAttributes");
                });
            modelBuilder.Entity<DbTypeMetadata>()
                .HasMany(t => t.EFImplementedInterfaces)
                .WithMany()
                .Map(ti =>
                {
                    ti.MapLeftKey("TypeIntId");
                    ti.MapRightKey("InterfaceTypId");
                    ti.ToTable("TypesInterfaces");
                });
            modelBuilder.Entity<DbTypeMetadata>()
                .HasOptional(t => t.EFBaseType)
                .WithMany()
                .Map(x => x.MapKey("BaseTypeId"));
            modelBuilder.Entity<DbTypeMetadata>()
                .HasMany(t => t.EFGenericArguments)
                .WithMany()
                .Map(tg =>
                {
                    tg.MapLeftKey("TypeGenId");
                    tg.MapRightKey("GenericArgTypId");
                    tg.ToTable("TypesGenericArguments");
                });
            modelBuilder.Entity<DbTypeMetadata>()
                .HasOptional(t => t.EFDeclaringType)
                .WithMany(t => t.EFNestedTypes)
                .Map(x => x.MapKey("DeclaringTypeId"));
            modelBuilder.Entity<DbTypeMetadata>()
                .HasMany(t => t.EFProperties)
                .WithMany()
                .Map(tp =>
                {
                    tp.MapLeftKey("TypePropId");
                    tp.MapRightKey("PropertyTypId");
                    tp.ToTable("TypesProperties");
                });
            modelBuilder.Entity<DbTypeMetadata>()
                .HasMany(t => t.EFMethodsAndConstructors)
                .WithRequired()
                .Map(x => x.MapKey("MethodTypId"));
        }
    }
}