using DatabasePersistence.DBModel;
using ModelContract;
using Persistance;
using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.Configuration;
using System.Data.Entity;
using System.Linq;
using System.Data.Entity.Migrations;

namespace DatabasePersistence
{
    [Export(typeof(IPersister))]
    public class DatabasePersister : IPersister
    {
        private DbModelAccessContext context;
        private string connectionString;

        public string Target
        {
            get { return connectionString; } 
            set
            {
                connectionString = value;
                context?.Dispose();
                context = new DbModelAccessContext(value);
            }
        }

        public FileSystemDependency FileSystemDependency => FileSystemDependency.Independent;

        public DatabasePersister()
        {
            string connectionString = ConfigurationManager.ConnectionStrings["DbSource"].ConnectionString;
            Target = connectionString;
        }

        public void Dispose()
        {
            context?.Dispose();
        }

        public object Load()
        {
            return context.Assemblies.OrderByDescending(n => n.Id).First();
        }

        public void Save(object obj)
        {
            if (!(obj is IAssemblyMetadata))
            {
                throw new InvalidOperationException("Can't assign from given type.");
            }
            DbAssemblyMetadata root = obj as DbAssemblyMetadata;
            context.Assemblies.Add(root);
            context.SaveChanges();
        }
    }
}
