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

        public string Target { get; set; }

        public FileSystemDependency FileSystemDependency => FileSystemDependency.Independent;

        public DatabasePersister()
        {
            bool localFileAccess = false;
            string localFileAccessString = ConfigurationManager.AppSettings["LocalFileAccess"];
            if (localFileAccessString != null)
                localFileAccess = bool.Parse(localFileAccessString);
            if(!localFileAccess)
                Target = ConfigurationManager.AppSettings["Connection"];
            else
            {
                string assemblyPath = System.IO.Path.GetDirectoryName(System.Reflection.Assembly.GetExecutingAssembly().Location);
                string fileName = ConfigurationManager.AppSettings["DbFileName"];
                Target = $"Server=(localdb)\\mssqllocaldb;Integrated Security=true;";
            }
        }

        public void Dispose() { }

        public object Load()
        {
            object result = null;
            using (var context = new DbModelAccesContext(Target))
            {
                result = context.Assemblies.Last();
            }
            return result;
        }

        public void Save(object obj)
        {
            if (!typeof(IAssemblyMetadata).IsAssignableFrom(obj.GetType()))
            {
                throw new InvalidOperationException("Can't assign from given type.");
            }
            using (var context = new DbModelAccesContext(Target))
            {
                DbAssemblyMetadata root = obj as DbAssemblyMetadata;
                context.Assemblies.Add(root);
                context.SaveChanges();
            }
        }
    }
}
