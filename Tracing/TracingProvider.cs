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
                throw new MEFTracingLoaderException("Directory catalog can't be null");
            AggregateCatalog catalog = new AggregateCatalog();
            catalog.Catalogs.Add(DirectoryCatalog);
            _container = new CompositionContainer(catalog);
            try
            {
                _container.ComposeParts(this);
            }
            catch (CompositionException compositionException)
            {
                throw new MEFTracingLoaderException("Couldn't compose application", compositionException);
            }
            if (_tracer is null)
            {
                throw new MEFTracingLoaderException($"Could not load {typeof(ITracing)}");
            }
            else
            {
                return _tracer;
            }
        }
    }
}
