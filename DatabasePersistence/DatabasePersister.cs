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
            DbAssemblyMetadata result = context.Assemblies.FirstOrDefault();
            ExplicitLoading(result);
            InsertRedundantData(result);
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
            context.Entry(loadedRoot).Collection(a => a.EFNamespaces).Load();
            foreach(DbNamespaceMetadata _namespace in loadedRoot.EFNamespaces)
            {
                context.Entry(_namespace).Collection(n => n.EFTypes).Load();
                foreach(DbTypeMetadata type in _namespace.EFTypes)
                {
                    context.Entry(type).Reference(t => t.EFDeclaringType)?.Load();
                    context.Entry(type).Reference(t => t.EFBaseType)?.Load();
                    context.Entry(type).Collection(t => t.EFAttributes)?.Load();
                    context.Entry(type).Collection(t => t.EFGenericArguments)?.Load();
                    context.Entry(type).Collection(t => t.EFImplementedInterfaces)?.Load();
                    context.Entry(type).Collection(t => t.EFNestedTypes)?.Load();
                    context.Entry(type).Collection(t => t.EFMethodsAndConstructors)?.Load();
                    context.Entry(type).Collection(t => t.EFProperties)?.Load();
                    foreach(DbPropertyMetadata property in type.EFProperties)
                    {
                        context.Entry(property).Reference(p => p.EFMyType).Load();
                    }
                    foreach(DbMethodMetadata method in type.EFMethodsAndConstructors)
                    {
                        context.Entry(method).Reference(m => m.EFReturnType).Load();
                        context.Entry(method).Collection(m => m.EFParameters).Load();
                        context.Entry(method).Collection(m => m.EFGenericArguments).Load();
                        foreach(DbParameterMetadata parameter in method.EFParameters)
                        {
                            context.Entry(parameter).Reference(p => p.EFMyType).Load();
                        }
                    }
                }
            }
        }

        private void InsertRedundantData(DbAssemblyMetadata loadedRoot)
        {
            foreach(DbNamespaceMetadata _namespace in loadedRoot.Namespaces)
            {
                foreach(DbTypeMetadata type in _namespace.Types)
                {
                    type.NamespaceName = _namespace.Name;
                }
            }
        }
    }
}