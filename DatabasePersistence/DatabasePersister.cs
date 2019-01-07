using DatabasePersistence.DBModel;
using ModelContract;
using Persistance;
using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.Configuration;
using System.Linq;

namespace DatabasePersistence
{
    [Export(typeof(IPersister))]
    public class DatabasePersister : IPersister
    {
        private DbModelAccesContext context;


        public string Target { get; set; }

        public FileSystemDependency FileSystemDependency => FileSystemDependency.Independent;

        public void SetTarget(string connStr)
        {
            if (context != null)
                context.Dispose();
            context = new DbModelAccesContext(connStr);
            this.Target = connStr;
        }
        public DatabasePersister()
        {
            bool localFileAccess = false;
            string localFileAccessString = ConfigurationManager.AppSettings["LocalFileAccess"];
            if (localFileAccessString != null)
                localFileAccess = bool.Parse(localFileAccessString);
            if(!localFileAccess)
                SetTarget(ConfigurationManager.AppSettings["Connection"]);
            else
            {
                string assemblyPath = System.IO.Path.GetDirectoryName(System.Reflection.Assembly.GetExecutingAssembly().Location);
                string fileName = ConfigurationManager.AppSettings["DbFileName"];
                SetTarget ($"Server=(localdb)\\mssqllocaldb;Integrated Security=true;" +
                    $"AttachDbFileName='{assemblyPath}\\{fileName}';");
            }
        }

        public void Dispose()
        {
            this.context.Dispose();
        }

        public object Load()
        {
            List<AbstractMapper> model = context.Model.ToList();
            IAssemblyMetadata assembly = 
                (IAssemblyMetadata)model.Select(n => n).Where(n => typeof(IAssemblyMetadata).IsAssignableFrom(n.GetType())).First();
            return assembly;

        }

        public void Save(object obj)
        {
            if (!typeof(IAssemblyMetadata).IsAssignableFrom(obj.GetType()))
                throw new InvalidOperationException("Can't assign from given type.");
            AbstractMapper assembly = (AbstractMapper)obj;
            context.Model.Add(assembly);
            context.SaveChanges();
        }
    }
}
