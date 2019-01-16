using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.Configuration;
using System.Linq;
using System.Reflection;
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
            DbAssemblyMetadata result = context.Assemblies.OrderByDescending(n => n.Id).FirstOrDefault();
            ExplicitLoading(result);
            return result;
        }

        public void Save(IAssemblyMetadata obj)
        {
            DbAssemblyMetadata root = obj as DbAssemblyMetadata ?? new DbAssemblyMetadata(obj);
            context.Assemblies.Add(root);
            context.SaveChanges();
        }

        private void ExplicitLoading(DbAssemblyMetadata loadedRoot)
        {
            context.Entry(loadedRoot).Collection(a => a.NamespacesList).Load();
            foreach (DbNamespaceMetadata _namespace in loadedRoot.NamespacesList)
            {
                context.Entry(_namespace).Collection(n => n.TypesList).Load();
                foreach (DbTypeMetadata type in _namespace.TypesList)
                {
                    context.Entry(type).Collection(t => t.ImplementedInterfacesList).Load();
                    context.Entry(type).Collection(t => t.MethodsList).Load();
                    context.Entry(type).Collection(t => t.NestedTypesList).Load();
                    context.Entry(type).Collection(t => t.ConstructorsList).Load();
                    context.Entry(type).Collection(t => t.GenericArgumentsList).Load();
                    context.Entry(type).Collection(t => t.PropertiesList).Load();
                    context.Entry(type).Collection(t => t.AttributesList).Load();
                    foreach (DbPropertyMetadata property in type.PropertiesList)
                    {
                        context.Entry(property).Reference(p => p.DbMyType).Load();
                    }
                    foreach (DbMethodMetadata method in type.MethodsList)
                    {
                        context.Entry(method).Reference(m => m.DbReturnType).Load();
                        context.Entry(method).Collection(m => m.ParametersList).Load();
                        context.Entry(method).Collection(m => m.GenericArgumentsList).Load();
                        foreach (DbParameterMetadata parameter in method.ParametersList)
                        {
                            context.Entry(parameter).Reference(p => p.DbMyType).Load();
                        }
                    }
                }
            }
        }
    }
}