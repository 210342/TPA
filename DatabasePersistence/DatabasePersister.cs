using DatabasePersistence.DBModel;
using ModelContract;
using Persistance;
using System;
using System.Collections.Generic;
using System.Linq;

namespace DatabasePersistence
{
    public class DatabasePersister : IPersister
    {
        private DbModelAccesContext context;

        private string _target;

        public string Target { get => _target; set => SetTarget(value); }

        private void SetTarget(string connStr)
        {
            context = new DbModelAccesContext(connStr);
            this._target = connStr;
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
