using System;
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
            if (string.IsNullOrEmpty(target))
            {
                context = new DbModelAccessContext(_originalTarget);
            }
            else
            {
                context = new DbModelAccessContext(target);
            }
        }

        public void Dispose()
        {
            context?.Dispose();
            GC.SuppressFinalize(this);
        }

        public IAssemblyMetadata Load()
        {
            return context.Assemblies.OrderByDescending(n => n.Id).FirstOrDefault();
        }

        public void Save(IAssemblyMetadata obj)
        {
            DbAssemblyMetadata root = obj as DbAssemblyMetadata ?? new DbAssemblyMetadata(obj);
            context.Assemblies.Add(root);
            context.SaveChanges();
        }
    }
}