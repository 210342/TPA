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

        public IPersister ProvidePersister()
        {
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
            return persister;
        }
    }
}
