using System;
using System.ComponentModel.Composition;
using System.ComponentModel.Composition.Hosting;
using Library.Logic.MEFProviders.Exceptions;
using Persistance;

namespace Library.Logic.MEFProviders
{
    public class PersistanceProvider
    {
        private CompositionContainer _container;

        [Import(typeof(IPersister))] private IPersister _persister;

        public PersistanceProvider()
        {
        }

        public PersistanceProvider(string assemblyPath)
        {
            DirectoryCatalog = new DirectoryCatalog(assemblyPath);
        }

        public DirectoryCatalog DirectoryCatalog { get; set; }

        public IPersister ProvidePersister()
        {
            if (DirectoryCatalog == null)
                throw new MEFLoaderException("Directory catalog can't be null");
            var catalog = new AggregateCatalog();
            catalog.Catalogs.Add(DirectoryCatalog);
            _container = new CompositionContainer(catalog);
            try
            {
                _container.ComposeParts(this);
            }
            catch (CompositionException compositionException)
            {
                throw new MEFLoaderException("Couldn't compose persistance object", compositionException);
            }
            catch (Exception ex)
            {
                throw new MEFLoaderException(
                    $"Couldn't compose application, reason:{Environment.NewLine}{ex.GetType()}{Environment.NewLine}{ex.Message}");
            }

            if (_persister is null) throw new MEFLoaderException("Couldn't compose persistance object");
            return _persister;
        }
    }
}