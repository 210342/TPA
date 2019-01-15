﻿using System;
using System.ComponentModel.Composition;
using System.Configuration;
using System.Linq;
using DatabasePersistence.DBModel;
using ModelContract;
using Persistance;

namespace DatabasePersistence
{
    [Export(typeof(IPersister))]
    public class DatabasePersister : IPersister
    {
        private string connectionString;
        private DbModelAccessContext context;

        public DatabasePersister()
        {
            Target = ConfigurationManager.ConnectionStrings["DbSource"].ConnectionString;
        }

        public string Target
        {
            get => connectionString;
            set
            {
                if(!string.IsNullOrEmpty(value))
                {
                    connectionString = value;
                    context?.Dispose();
                    context = new DbModelAccessContext(value);
                }
            }
        }

        public FileSystemDependency FileSystemDependency => FileSystemDependency.Independent;

        public void Dispose()
        {
            context?.Dispose();
            GC.SuppressFinalize(this);
        }

        public IAssemblyMetadata Load()
        {
            return context.Assemblies.OrderByDescending(n => n.Id).First();
        }

        public void Save(IAssemblyMetadata obj)
        {
            DbAssemblyMetadata root = obj as DbAssemblyMetadata ?? new DbAssemblyMetadata(obj as IAssemblyMetadata);
            context.Assemblies.Add(root);
            context.SaveChanges();
        }
    }
}