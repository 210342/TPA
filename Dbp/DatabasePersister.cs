using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.Configuration;
using System.Linq;
using System.Reflection;
using Dbp.Model;
using ModelContract;
using Persistance;

namespace DatabasePersistence
{
    [Export(typeof(IPersister))]
    public class DatabasePersister : IPersister
    {
        private DbModelAccessContext context;
        private readonly string _originalTarget;

        public DatabasePersister()
        {
            _originalTarget = ConfigurationManager.ConnectionStrings["DbSource"].ConnectionString;
        }

        public FileSystemDependency FileSystemDependency => FileSystemDependency.Independent;

        public void Access(string target)
        {
            context?.Dispose();
            context = string.IsNullOrEmpty(target) 
                ? new DbModelAccessContext(_originalTarget) 
                : new DbModelAccessContext(target);
        }

        public void Dispose()
        {
            context?.Dispose();
            GC.SuppressFinalize(this);
        }

        public IAssemblyMetadata Load()
        {
            DbAssemblyMetadata result = context.Assemblies.FirstOrDefault();
            ExplicitLoading(result);
            return result as IAssemblyMetadata;
        }

        public void Save(IAssemblyMetadata obj)
        {
            DbAssemblyMetadata root = obj as DbAssemblyMetadata ?? new DbAssemblyMetadata(obj);
            context.Assemblies.Add(root);
            context.SaveChanges();
        }

        private void ExplicitLoading(DbAssemblyMetadata loadedRoot)
        {
        }
    }
}