using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.ComponentModel.Composition.Hosting;
using System.Diagnostics;

namespace Library.Logic.ViewModel
{
    internal class TraceListenersLoader
    {
        [ImportMany(typeof(TraceListener))]
        private IEnumerable<TraceListener>
            importedTraceListener = null;

        private CompositionContainer _container;

        public void LoadTraceListeners()
        {
            AggregateCatalog catalog = new AggregateCatalog();
            _container = new CompositionContainer(catalog);
            try
            {
                this._container.ComposeParts(this);
            }
            catch (CompositionException compositionException)
            {
                Trace.TraceError("External TraceListener import failed. " +
                compositionException.Message);
            }
            if (importedTraceListener != null)
                foreach (TraceListener listener in importedTraceListener)
                    Trace.Listeners.Add(listener);
        }
    }
}
