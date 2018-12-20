using Library.Logic.Exceptions;
using System.ComponentModel.Composition;
using System.ComponentModel.Composition.Hosting;
using System.Reflection;
using Tracing;

namespace Library.Logic.ViewModel
{
    internal class TracingProvider
    {
        [Import(typeof(ITracing))]
        private ITracing _tracer = null;
        private CompositionContainer _container;

        public DirectoryCatalog DirectoryCatalog { get; set; }

        internal ITracing ProvideTracer()
        {
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
