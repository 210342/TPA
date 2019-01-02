using Library.Logic.MEFProviders.Exceptions;
using System;
using System.ComponentModel.Composition;
using System.ComponentModel.Composition.Hosting;
using Tracing;

namespace Library.Logic.MEFProviders
{
    public class TracingProvider
    {
        [Import(typeof(ITracing))]
        private ITracing _tracer = null;
        private CompositionContainer _container;

        public DirectoryCatalog DirectoryCatalog { get; set; }

        public TracingProvider()
        {

        }

        public TracingProvider(string assemblyPath)
        {
            DirectoryCatalog = new DirectoryCatalog(assemblyPath);
        }

        public ITracing ProvideTracer()
        {
            if (DirectoryCatalog == null)
                throw new MEFLoaderException("Directory catalog can't be null");
            AggregateCatalog catalog = new AggregateCatalog();
            catalog.Catalogs.Add(DirectoryCatalog);
            _container = new CompositionContainer(catalog);
            try
            {
                _container.ComposeParts(this);
            }
            catch (CompositionException compositionException)
            {
                throw new MEFLoaderException("Couldn't compose application", compositionException);
            }
            catch (Exception ex)
            {
                throw new MEFLoaderException($"Couldn't compose application, reason:{Environment.NewLine}{ex.GetType()}{Environment.NewLine}{ex.Message}");
            }
            if (_tracer is null)
            {
                throw new MEFLoaderException($"Could not load {typeof(ITracing)}");
            }
            else
            {
                return _tracer;
            }
        }
    }
}
