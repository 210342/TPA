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
            var processed = new HashSet<IMetadata>();
            Console.WriteLine(context.Model.Count());
            context.Model.ToList().ForEach(n => Console.WriteLine(n?.GetType()));
            IAssemblyMetadata assembly = 
                (IAssemblyMetadata)context.Model.Select(n => n)
                .Where(n => n is DbAssemblyMetadata).First();
            LoadGraph(assembly, processed);
            return assembly;
        }

        public void Save(object obj)
        {
            var processed = new HashSet<IMetadata>();

            if (!typeof(IAssemblyMetadata).IsAssignableFrom(obj.GetType()))
                throw new InvalidOperationException("Can't assign from given type.");
            AbstractMapper assembly = (AbstractMapper)obj;

            LoadGraph((IMetadata)assembly, processed);
            foreach(var el in processed)
            {
                context.Model.AddOrUpdate((AbstractMapper)el);
            }
            //processed.ToList().ForEach(n => context.Model.Add((AbstractMapper)n));
            //context.Model.Add(assembly);
            context.SaveChanges();
        }

        bool Contains(DbSet<AbstractMapper> set, AbstractMapper element)
        {
            foreach(var el in set)
            {
                if (el.Id == element.Id)
                    return true;
            }
            return false;
        }

        private void LoadGraph(IMetadata head, HashSet<IMetadata> processed)
        {
            if(head != null && !processed.Contains(head))
            {
                processed.Add(head);
                if(head.Children != null)
                {
                    foreach (var child in head.Children)
                    {
                        LoadGraph(child, processed);
                    }
                }
            }
        }
    }
}
