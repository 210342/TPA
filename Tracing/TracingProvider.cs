using System.ComponentModel.Composition;
using System.ComponentModel.Composition.Hosting;
using Tracing.Exceptions;

namespace Tracing
{
    public class TracingProvider
    {
        [Import(typeof(ITracing))]
        private ITracing _tracer = null;
        private CompositionContainer _container;

        public DirectoryCatalog DirectoryCatalog { get; set; }

        public ITracing ProvideTracer()
        {
            AggregateCatalog catalog = new AggregateCatalog();
            // catalog.Catalogs.Add(new AssemblyCatalog(Assembly.GetCallingAssembly()));
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
