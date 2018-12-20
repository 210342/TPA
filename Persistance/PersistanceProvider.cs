using Persistance;
using Persistance.Exceptions;
using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.ComponentModel.Composition.Hosting;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Persistance
{
    public class PersistanceProvider
    {
        [Import(typeof(IPersister))]
        private IPersister persister;
        private CompositionContainer _container;

        public DirectoryCatalog DirectoryCatalog { get; set; }

        public PersistanceProvider()
        {

        }

        public PersistanceProvider(string assemblyPath)
        {
            DirectoryCatalog = new DirectoryCatalog(assemblyPath);
        }

        public IPersister ProvidePersister()
        {
            if (DirectoryCatalog == null)
                throw new MEFPersistanceLoaderException("Directory catalog can't be null");
            AggregateCatalog catalog = new AggregateCatalog();
            catalog.Catalogs.Add(DirectoryCatalog);
            _container = new CompositionContainer(catalog);
            try
            {
                this._container.ComposeParts(this);
            }
            catch (CompositionException compositionException)
            {
                throw new MEFPersistanceLoaderException("Couldn't compose persistance object", compositionException);
            }
            if(persister is null)
            {
                throw new MEFPersistanceLoaderException("Couldn't compose persistance object");
            }
            return persister;
        }
    }
}
